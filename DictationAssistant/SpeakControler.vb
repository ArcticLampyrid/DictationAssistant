Public Class SpeakControler
    Private ReadOnly lock As New Object '约定：RaiseEvent前必须unlock

    Public Property Speaker As ISpeaker
    Public Property WordListSource As IWordListAdapter
    Private WithEvents SpeakStateControler As ISpeakStateControler
    Private NextWordIndex As Integer = 0 '下一个播报词语索引
    Private ElapsedSeconds As Integer = 0 '已过秒数
    Private _ElapsedTimes As Integer = 0 '已报次数

    Private AutoMode As Boolean = False '（是否）自动播报

    Public Property CurrentWaitingTimeCalculator As IWaitingTimeCalculator = New FixedWaitingTime(3)
    Public Property AutoMode_TimesPerWord As Integer = 2 '每词遍数

    Private WithEvents TimerForAutoSpeaking As New Timers.Timer With {.Enabled = False, .Interval = 1000}

    '警告：任何事件都可能在一个全新的线程被触发！

    ''' <summary>
    ''' 自动播报完成
    ''' </summary>
    ''' <param name="Reason">原因，0：报完，1：手动结束</param>
    ''' <remarks></remarks>
    Event AutoSpeakingCompleted(Reason As Integer)

    ''' <summary>
    ''' 下次播报间隔被改变
    ''' </summary>
    ''' <param name="i"></param>
    ''' <param name="jiangge"></param>
    ''' <remarks></remarks>
    Event UpdateWaitingTimeInfo(i As Integer, jiangge As Integer)

    Public Event Speaking(i As Integer)
    Public Event StoppedThis(i As Integer, Completed As Boolean)

    Private Function GetWaitingTime(i As Integer) As Integer
        Return CurrentWaitingTimeCalculator.CalculateWaitingTime(WordListSource(i))
    End Function
    Private Sub TimerForAutoSpeaking_Elapsed(sender As Object, e As EventArgs) Handles TimerForAutoSpeaking.Elapsed
        Dim NeedToWait As Boolean = False
        Dim CompletedAuto As Boolean = False
        Dim WaitingToSpeakWord As Integer = 0
        Dim 剩余等待时间 As Integer = 0
        Dim NumberOfWords = WordListSource.Count
        Dim LocalNextWordIndex As Integer
        SyncLock lock
            LocalNextWordIndex = NextWordIndex
        End SyncLock
        Dim WaitingTime = GetWaitingTime(LocalNextWordIndex - 1)
        SyncLock lock
            ElapsedSeconds = ElapsedSeconds + 1 '加一秒
            If LocalNextWordIndex <= NumberOfWords Then '下一个 小于 最后项目+1 时
                剩余等待时间 = WaitingTime - ElapsedSeconds
                NeedToWait = 剩余等待时间 > 0
                If ElapsedTimes >= AutoMode_TimesPerWord Then '已报次数 >= 每词遍数 时
                    WaitingToSpeakWord = LocalNextWordIndex
                    If NeedToWait = False AndAlso LocalNextWordIndex = NumberOfWords Then
                        CompletedAuto = True
                    End If
                Else
                    WaitingToSpeakWord = LocalNextWordIndex - 1
                End If
            Else
                CompletedAuto = True
            End If
            If CompletedAuto Then
                AutoMode = False
                TimerForAutoSpeaking.Stop()
            End If
        End SyncLock
        If CompletedAuto Then
            RaiseEvent AutoSpeakingCompleted(0)
        Else
            TimerForAutoSpeaking.Start()
            If NeedToWait Then
                RaiseEvent UpdateWaitingTimeInfo(WaitingToSpeakWord, 剩余等待时间)
            Else
                Speak(WaitingToSpeakWord)
            End If
        End If
    End Sub

    Public Sub Speak(Index As Integer)
        Dim LocalSpeakStateControler As ISpeakStateControler
        Dim RaiseStoppedThis As Boolean = False
        Dim LastIndex As Integer
        Dim Word As String = WordListSource(Index)
        SyncLock lock
            TimerForAutoSpeaking.Stop()
            ElapsedSeconds = 0
            If SpeakStateControler IsNot Nothing Then
                RaiseStoppedThis = True
                LastIndex = NextWordIndex - 1
                SpeakStateControler.StopSpeak()
                SpeakStateControler = Nothing
            End If
            If NextWordIndex <> Index + 1 Then
                _ElapsedTimes = 0
                NextWordIndex = Index + 1
            End If
            LocalSpeakStateControler = Speaker.Speak(Word)
        End SyncLock
        If RaiseStoppedThis Then
            RaiseEvent StoppedThis(LastIndex, False)
        End If
        RaiseEvent Speaking(Index)
        SpeakStateControler = LocalSpeakStateControler
    End Sub
    Private Sub SpeakStateControler_PlayCompleted(sender As Object, e As EventArgs) Handles SpeakStateControler.PlayCompleted
        Dim localAutoMode = False
        Dim LastIndex As Integer
        SyncLock lock
            localAutoMode = AutoMode
            LastIndex = NextWordIndex - 1
            SpeakStateControler = Nothing
            _ElapsedTimes += 1
        End SyncLock

        RaiseEvent StoppedThis(LastIndex, True)
        If localAutoMode Then
            TimerForAutoSpeaking_Elapsed(sender, e)
        End If
    End Sub
    ''' <summary>
    ''' 取下一个词语的位置
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNextWordIndex() As Integer
        SyncLock lock
            Return NextWordIndex
        End SyncLock
    End Function
    ''' <summary>
    ''' 移动位置到第一个前
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ResetProgress()
        SyncLock lock
            NextWordIndex = 0
            _ElapsedTimes = 0
        End SyncLock
    End Sub
    ''' <summary>
    ''' 播报下一个并移动位置
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SpeakNext()
        Dim Index As Integer
        SyncLock lock
            Index = NextWordIndex
        End SyncLock
        Speak(Index)
    End Sub
    ''' <summary>
    ''' 播报当前词语
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SpeakAgain()
        Dim Index As Integer
        SyncLock lock
            Index = NextWordIndex - 1
        End SyncLock
        Speak(Index)
    End Sub
    ''' <summary>
    ''' 播报上一个并移动位置
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SpeakLast()
        Dim Index As Integer
        SyncLock lock
            Index = NextWordIndex - 2
        End SyncLock
        Speak(Index)
    End Sub
    Public ReadOnly Property IsAutoMode As Boolean
        Get
            SyncLock lock
                Return AutoMode
            End SyncLock
        End Get
    End Property
    Public ReadOnly Property IsSpeaking As Boolean
        Get
            SyncLock lock
                Return SpeakStateControler IsNot Nothing
            End SyncLock
        End Get
    End Property

    Public ReadOnly Property ElapsedTimes As Integer
        Get
            SyncLock lock
                Return _ElapsedTimes
            End SyncLock
        End Get
    End Property

    Public Sub StartAuto(Optional Index As Integer = 0)
        SyncLock lock
            AutoMode = True
            ElapsedSeconds = 0
        End SyncLock
        Speak(Index)
    End Sub
    Public Sub StopAuto()
        Dim RaiseStoppedThis As Boolean = False
        Dim LastIndex As Integer
        SyncLock lock
            If AutoMode = False Then Exit Sub
            AutoMode = False
            TimerForAutoSpeaking.Stop()
            If SpeakStateControler IsNot Nothing Then
                RaiseStoppedThis = True
                LastIndex = NextWordIndex - 1
                SpeakStateControler.StopSpeak()
                SpeakStateControler = Nothing
            End If
        End SyncLock
        If RaiseStoppedThis Then
            RaiseEvent StoppedThis(LastIndex, False)
        End If
        RaiseEvent AutoSpeakingCompleted(1)
    End Sub
    Public Sub PauseAuto()
        Dim RaiseStoppedThis As Boolean = False
        Dim LastIndex As Integer
        SyncLock lock
            If AutoMode = False Then Exit Sub
            TimerForAutoSpeaking.Stop()
            If SpeakStateControler IsNot Nothing Then
                RaiseStoppedThis = True
                LastIndex = NextWordIndex - 1
                SpeakStateControler.StopSpeak()
                SpeakStateControler = Nothing
            End If
        End SyncLock
        If RaiseStoppedThis Then
            RaiseEvent StoppedThis(LastIndex, False)
        End If
    End Sub
    Public Sub ResumeAuto()
        SyncLock lock
            If AutoMode = False Then Exit Sub
            TimerForAutoSpeaking.Start()
        End SyncLock
    End Sub

    Public Sub StopThis()
        Dim RaiseStoppedThis As Boolean = False
        Dim LastIndex As Integer
        SyncLock lock
            If SpeakStateControler IsNot Nothing Then
                RaiseStoppedThis = True
                LastIndex = NextWordIndex - 1
                SpeakStateControler.StopSpeak()
                SpeakStateControler = Nothing
            End If
        End SyncLock
        If RaiseStoppedThis Then
            RaiseEvent StoppedThis(LastIndex, False)
        End If
    End Sub
End Class
