Imports 自动默写
''' <summary>
''' 词语信息接口
''' </summary>
''' <remarks></remarks>
Public Interface ICiYuXinXi
    Function GetCiYu() As String
End Interface
''' <summary>
''' 播报间隔接口
''' </summary>
''' <remarks></remarks>
Public Interface BoBaoJianGe
    Function GetBoBoJianGe(CiYu As ICiYuXinXi) As Int32
End Interface
Public Class BoBaoJianGe_GuDing
    Implements BoBaoJianGe
    Private ShiChang As Integer
    Public Sub New(ShiChang As Integer)
        Me.ShiChang = ShiChang
    End Sub
    Public Function GetBoBoJianGe(CiYu As ICiYuXinXi) As Int32 Implements BoBaoJianGe.GetBoBoJianGe
        Return ShiChang
    End Function
End Class
Public Class BoBaoJianGe_DanCiShu
    Implements BoBaoJianGe
    Private MeiGeDanCi As Int32
    Private EWai As Int32
    Public Sub New(MeiGeDanCi As Integer)
        MyClass.New(MeiGeDanCi, 0)
    End Sub
    Public Sub New(MeiGeDanCi As Integer, EWai As Integer)
        Me.MeiGeDanCi = MeiGeDanCi
        Me.EWai = EWai
    End Sub
    Public Function GetBoBoJianGe(CiYu As ICiYuXinXi) As Int32 Implements BoBaoJianGe.GetBoBoJianGe
        Return BoBaoJianGeHelper.GetWordCount(CiYu) * MeiGeDanCi + EWai
    End Function
End Class
Public Class BoBaoJianGeHelper
    ''' <summary>
    ''' 取单词数（英文）
    ''' </summary>
    ''' <param name="CiZu">词组</param>
    ''' <returns>单词数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetWordCount(CiZu As ICiYuXinXi) As Integer
        Return GetWordCount(CiZu.GetCiYu())
    End Function
    ''' <summary>
    ''' 取单词数（英文）
    ''' </summary>
    ''' <param name="CiZu">词组</param>
    ''' <returns>单词数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetWordCount(CiZu As String) As Integer
        Dim FenGeFu As String = ".,:;?!- " '分隔符
        Dim ShangYiGeShiFenGeFu As Boolean = True '上一个是分隔符
        Dim ShuLiang As Integer = 1 '数量
        For Each temp As Char In CiZu
            If FenGeFu.IndexOf(temp) <> -1 Then
                If Not ShangYiGeShiFenGeFu Then
                    ShuLiang += 1
                End If
                ShangYiGeShiFenGeFu = True
            Else
                ShangYiGeShiFenGeFu = False
            End If
        Next
        If ShangYiGeShiFenGeFu Then
            ShuLiang -= 1
        End If
        Return ShuLiang
    End Function
End Class
Public Class CiYuXinXi
    Implements ICiYuXinXi
    Private CiYu As String
    Public Sub New(CiYu As String)
        Me.CiYu = CiYu
    End Sub
    Public Function GetCiYu() As String Implements ICiYuXinXi.GetCiYu
        Return CiYu
    End Function
End Class
Public Class CiYuXinXi_FormID
    Implements ICiYuXinXi
    Public Delegate Function GetStringFormID(ID As Integer) As String
    Private GetCiYuFormIDFunction As GetStringFormID
    Private ID As Integer
    Public Function GetCiYu() As String Implements ICiYuXinXi.GetCiYu
        Return GetCiYuFormIDFunction(ID)
    End Function
    Public Sub New(ID As Integer, GetCiYuFormIDFunction As GetStringFormID)
        Me.ID = ID
        Me.GetCiYuFormIDFunction = GetCiYuFormIDFunction
    End Sub
End Class
Public Class BoBaoLuoJi
    Dim XiaYiGeWeiZhi As Integer = 0 '下一个播报词语索引
    Dim intYGMS As Integer = 0 '已过秒数
    Dim Zdbb As Boolean = False '（是否）自动播报
    Dim JianGe As BoBaoJianGe = New BoBaoJianGe_GuDing(3)

    Public ZiDongBoBao_MeiCiBianShu As Integer = 2 '每词遍数

    Private WithEvents ZiDongBoBaoShiZhong As New Timer
    Event GetCiYuShuLiang(ByRef retu As Integer)
    Event GetCiYu(i As Integer, ByRef retu As String)

    ''' <summary>
    ''' 获取词语播报次数
    ''' </summary>
    ''' <param name="i">词语索引</param>
    ''' <param name="retu">请赋值为词语已播报次数</param>
    ''' <remarks></remarks>
    Event GetCiYuBoBaoCiShu(i As Integer, ByRef retu As Integer)

    ''' <summary>
    ''' 改变词语播报次数
    ''' </summary>
    ''' <param name="i">词语索引</param>
    ''' <param name="newValue">新值</param>
    ''' <remarks></remarks>
    Event SetCiYuBoBaoCiShu(i As Integer, newValue As Integer)

    ''' <summary>
    ''' 播报
    ''' </summary>
    ''' <param name="i">词语索引</param>
    ''' <remarks></remarks>
    Event BoBao(i As Integer)

    ''' <summary>
    ''' 自动播报完成
    ''' </summary>
    ''' <param name="YuanYin">原因，0：报完，1：手动结束</param>
    ''' <remarks></remarks>
    Event ZiDongBoBaoWanCheng(YuanYin As Integer)

    ''' <summary>
    ''' 下次播报间隔被改变
    ''' </summary>
    ''' <param name="i"></param>
    ''' <param name="jiangge"></param>
    ''' <remarks></remarks>
    Event XiaCiBoBaoJianGeBeiGaiBian(i As Integer, jiangge As Integer)

    ''' <summary>
    ''' 停止当前播报
    ''' </summary>
    ''' <remarks>若当前不在播报，则不应执行任何操作！</remarks>
    Event TingZhiDangQianBoBao()

    ''' <summary>
    ''' 自动播报是否已暂停
    ''' </summary>
    ''' <param name="retu">传址参数，调用后应为结果（是否已暂停）</param>
    ''' <remarks></remarks>
    Event IsZanTing(ByRef retu As Boolean)
    Public Structure MeiCiJianGeXinXi  '每次间隔信息
        Public LeiXin As Integer '0=ShiChang/1=单词数*ShiChang
        Public ShiChang As Integer
    End Structure
    Sub New()
        ZiDongBoBaoShiZhong.Enabled = False
        ZiDongBoBaoShiZhong.Interval = 1000
    End Sub
    Public Sub SetJianGe(NewJianGe As BoBaoJianGe)
        JianGe = NewJianGe
    End Sub
    Public Function GetJianGe() As BoBaoJianGe
        Return JianGe
    End Function
    Private Function IsZanTingFunction() As Boolean
        Dim r As Boolean = False
        RaiseEvent IsZanTing(r)
        Return r
    End Function
    Private Function GetCiYuShuLiangFunction() As Integer
        Dim r As Integer = 0
        RaiseEvent GetCiYuShuLiang(r)
        Return r
    End Function
    Private Function GetCiYuBoBaoCiShuFunction(i As Integer) As Integer
        Dim r As Integer = 0
        RaiseEvent GetCiYuBoBaoCiShu(i, r)
        Return r
    End Function
    Private Function GetCiYuFunction(i As Integer) As String
        Dim r As String = ""
        RaiseEvent GetCiYu(i, r)
        Return r
    End Function
    Private Function GetJianGe(i As Integer) As Integer
        Return JianGe.GetBoBoJianGe(New CiYuXinXi_FormID(i, AddressOf GetCiYuFunction))
    End Function
    Private Sub ZiDongBoBaoShiZhong_Tick(sender As Object, e As EventArgs) Handles ZiDongBoBaoShiZhong.Tick
        Dim BXYG As Boolean '报下一个
        intYGMS = intYGMS + 1 '加一秒
        If XiaYiGeWeiZhi <= GetCiYuShuLiangFunction() Then '下一个 小于 最后项目+1 时
            If GetCiYuBoBaoCiShuFunction(XiaYiGeWeiZhi - 1) >= ZiDongBoBao_MeiCiBianShu Then '已报次数 >= 每遍次数 时
                BXYG = True '下一个
            Else
                BXYG = False '重复这个
            End If
        Else
            Zdbb = False
            ZiDongBoBaoShiZhong.Enabled = False
            RaiseEvent ZiDongBoBaoWanCheng(0)
            Exit Sub
        End If
        If GetJianGe(XiaYiGeWeiZhi - 1) - intYGMS > 0 Then
            If BXYG = True Then
                RaiseEvent XiaCiBoBaoJianGeBeiGaiBian(XiaYiGeWeiZhi + 1, GetJianGe(XiaYiGeWeiZhi - 1) - intYGMS)
            Else
                RaiseEvent XiaCiBoBaoJianGeBeiGaiBian(XiaYiGeWeiZhi + 1 - 1, GetJianGe(XiaYiGeWeiZhi - 1) - intYGMS)
            End If
        End If
        If intYGMS >= GetJianGe(XiaYiGeWeiZhi - 1) Then
            If BXYG = True Then
                If XiaYiGeWeiZhi < GetCiYuShuLiangFunction() Then
                    播报指定位置词语(XiaYiGeWeiZhi)
                Else
                    Zdbb = False
                    ZiDongBoBaoShiZhong.Enabled = False
                    RaiseEvent ZiDongBoBaoWanCheng(0)
                    Exit Sub
                End If
            Else
                播报指定位置词语(XiaYiGeWeiZhi - 1)
            End If
        End If
    End Sub

    Public Sub 播报指定位置词语(WeiZhi As Integer)
        If IsZanTingFunction() Then
            Exit Sub
        End If

        ZiDongBoBaoShiZhong.Enabled = False
        RaiseEvent TingZhiDangQianBoBao()

        RaiseEvent SetCiYuBoBaoCiShu(WeiZhi, GetCiYuBoBaoCiShuFunction(WeiZhi) + 1)
        RaiseEvent BoBao(WeiZhi)
        XiaYiGeWeiZhi = WeiZhi + 1
    End Sub
    Public Sub EndBoBao()
        If IsZanTingFunction() Then
            Exit Sub
        End If
        Dim intBBCS As Integer
        If XiaYiGeWeiZhi <= GetCiYuShuLiangFunction() And XiaYiGeWeiZhi > 0 Then
            intBBCS = GetCiYuBoBaoCiShuFunction(XiaYiGeWeiZhi - 1)
        ElseIf Zdbb Then
            Zdbb = False
            ZiDongBoBaoShiZhong.Enabled = False
            RaiseEvent ZiDongBoBaoWanCheng(0)
            Exit Sub
        End If
        If Zdbb Then
            If intBBCS >= ZiDongBoBao_MeiCiBianShu And XiaYiGeWeiZhi <> GetCiYuShuLiangFunction() Then
                intYGMS = 0
                If GetJianGe(XiaYiGeWeiZhi - 1) = 0 Then
                    ZiDongBoBaoShiZhong_Tick(Nothing, Nothing)
                Else
                    RaiseEvent XiaCiBoBaoJianGeBeiGaiBian(XiaYiGeWeiZhi + 1, GetJianGe(XiaYiGeWeiZhi - 1))
                    ZiDongBoBaoShiZhong.Enabled = True
                End If
            ElseIf intBBCS < ZiDongBoBao_MeiCiBianShu Then
                intYGMS = 0
                If GetJianGe(XiaYiGeWeiZhi - 1) = 0 Then
                    ZiDongBoBaoShiZhong_Tick(Nothing, Nothing)
                Else
                    RaiseEvent XiaCiBoBaoJianGeBeiGaiBian(XiaYiGeWeiZhi + 1 - 1, GetJianGe(XiaYiGeWeiZhi - 1))
                    ZiDongBoBaoShiZhong.Enabled = True
                End If
            Else
                Zdbb = False
                ZiDongBoBaoShiZhong.Enabled = False
                RaiseEvent ZiDongBoBaoWanCheng(0)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 取下一个词语的位置
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetXiaYiGeWeiZhi() As Integer
        Return XiaYiGeWeiZhi
    End Function
    ''' <summary>
    ''' 移动位置到第一个前
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CongTouKaiShi()
        XiaYiGeWeiZhi = 0
    End Sub
    ''' <summary>
    ''' 播报下一个并移动位置
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BoBaoXiaYiGe()
        播报指定位置词语(XiaYiGeWeiZhi)
    End Sub
    ''' <summary>
    ''' 播报当前词语
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BoBaoDangQian()
        播报指定位置词语(XiaYiGeWeiZhi - 1)
    End Sub
    ''' <summary>
    ''' 播报上一个并移动位置
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BoBaoShangYiGe()
        播报指定位置词语(XiaYiGeWeiZhi - 2)
    End Sub
    Public Function IsZddb() As Boolean
        Return Zdbb
    End Function
    Public Sub KaiShiZiDongBoBao()
        Zdbb = True
        intYGMS = 0
        BoBaoXiaYiGe()
    End Sub
    Public Sub JieShuZiDongBoBao()
        If Zdbb = False Then Exit Sub
        Zdbb = False
        ZiDongBoBaoShiZhong.Enabled = False
        RaiseEvent TingZhiDangQianBoBao()
        RaiseEvent ZiDongBoBaoWanCheng(1)
    End Sub
    Public Sub 暂停自动播报()
        If Zdbb = False Then Exit Sub
        RaiseEvent TingZhiDangQianBoBao()
        ZiDongBoBaoShiZhong.Enabled = False
    End Sub
    Public Sub 继续自动播报()
        If Zdbb = False Then Exit Sub
        ZiDongBoBaoShiZhong.Enabled = True
    End Sub
    Public Function GetXiaCiBoBaoJianGe() As Integer '取下次播报间隔
        Return GetJianGe(XiaYiGeWeiZhi - 1) - intYGMS
    End Function
End Class
