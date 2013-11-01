VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"; "mscomctl.ocx"
Begin VB.Form Form1 
   AutoRedraw      =   -1  'True
   BorderStyle     =   1  'Fixed Single
   Caption         =   "自动默写"
   ClientHeight    =   7335
   ClientLeft      =   150
   ClientTop       =   480
   ClientWidth     =   12765
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   7335
   ScaleWidth      =   12765
   StartUpPosition =   2  '屏幕中心
   Begin VB.Frame Frame3 
      Caption         =   "引擎设置"
      Height          =   1695
      Left            =   7560
      TabIndex        =   23
      Top             =   5040
      Width           =   5055
      Begin VB.CommandButton Command11 
         Caption         =   "切换到英文阅读引擎"
         Height          =   255
         Left            =   2520
         TabIndex        =   32
         Top             =   1320
         Width           =   2415
      End
      Begin VB.CommandButton Command10 
         Caption         =   "切换到中文阅读引擎"
         Height          =   255
         Left            =   120
         TabIndex        =   31
         Top             =   1320
         Width           =   2415
      End
      Begin VB.ComboBox Combo1 
         Height          =   300
         Left            =   720
         Style           =   2  'Dropdown List
         TabIndex        =   29
         Top             =   960
         Width           =   4215
      End
      Begin MSComctlLib.Slider Slider2 
         Height          =   255
         Left            =   720
         TabIndex        =   25
         Top             =   240
         Width           =   4215
         _ExtentX        =   7435
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   10
         SmallChange     =   5
         Max             =   100
         SelStart        =   100
         TickFrequency   =   5
         Value           =   100
      End
      Begin MSComctlLib.Slider Slider1 
         Height          =   255
         Left            =   720
         TabIndex        =   27
         Top             =   600
         Width           =   4215
         _ExtentX        =   7435
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   3
         Min             =   -10
      End
      Begin VB.Label Label8 
         AutoSize        =   -1  'True
         Caption         =   "引擎："
         Height          =   180
         Left            =   120
         TabIndex        =   28
         Top             =   960
         Width           =   660
      End
      Begin VB.Label Label9 
         AutoSize        =   -1  'True
         Caption         =   "语速："
         Height          =   180
         Left            =   120
         TabIndex        =   26
         Top             =   600
         Width           =   540
      End
      Begin VB.Label Label11 
         AutoSize        =   -1  'True
         Caption         =   "音量："
         Height          =   180
         Left            =   120
         TabIndex        =   24
         Top             =   240
         Width           =   540
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "词语播报"
      Height          =   4815
      Left            =   7560
      TabIndex        =   5
      Top             =   120
      Width           =   5055
      Begin VB.CommandButton Command3 
         Caption         =   "再报一遍(&A)"
         Height          =   375
         Left            =   3360
         TabIndex        =   19
         Top             =   3720
         Width           =   1455
      End
      Begin VB.CommandButton Command9 
         Caption         =   "报上一个(&L)"
         Height          =   375
         Left            =   3360
         TabIndex        =   17
         Top             =   3240
         Width           =   1455
      End
      Begin VB.CommandButton Command7 
         Caption         =   "暂停自动播报(&P)"
         Height          =   1455
         Left            =   710
         TabIndex        =   13
         Top             =   2640
         Width           =   470
      End
      Begin VB.CommandButton Command1 
         Caption         =   "报下一个(&N)"
         Height          =   375
         Left            =   1800
         TabIndex        =   16
         Top             =   3240
         Width           =   1455
      End
      Begin VB.CommandButton Command5 
         Caption         =   "从头开始(&H)"
         Height          =   375
         Left            =   1800
         TabIndex        =   18
         Top             =   3720
         Width           =   1455
      End
      Begin VB.TextBox Text1 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   9
         Text            =   "3"
         Top             =   720
         Width           =   1695
      End
      Begin VB.CommandButton Command6 
         Caption         =   "开始自动播报(&S)"
         Height          =   1455
         Left            =   240
         TabIndex        =   12
         Top             =   2640
         Width           =   470
      End
      Begin VB.TextBox Text2 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   11
         Text            =   "2"
         Top             =   1080
         Width           =   1695
      End
      Begin VB.CheckBox Check1 
         Caption         =   "自动播报"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Left            =   1800
         TabIndex        =   15
         Top             =   2700
         Width           =   3015
      End
      Begin VB.Label Label15 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2400
         TabIndex        =   43
         Top             =   1440
         Width           =   2055
      End
      Begin VB.Label Label14 
         Caption         =   "个"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Left            =   4440
         TabIndex        =   42
         Top             =   1440
         Width           =   435
      End
      Begin VB.Label Label13 
         Caption         =   "正在播报第"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Left            =   240
         TabIndex        =   41
         Top             =   1440
         Width           =   2175
      End
      Begin VB.Label Label12 
         Caption         =   "暂停自动播报后不把“自动播报”这个勾去掉的话，按“报下一个”、“报上一个”或“再报一遍”后会自动开始下一轮自动播报。"
         Height          =   615
         Left            =   240
         TabIndex        =   14
         Top             =   2040
         Width           =   4695
      End
      Begin VB.Label Label3 
         Caption         =   "将于"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   360
         TabIndex        =   20
         Top             =   4200
         Width           =   975
      End
      Begin VB.Label Label4 
         Caption         =   "秒后自动播报"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2205
         TabIndex        =   22
         Top             =   4200
         Width           =   2655
      End
      Begin VB.Label Label5 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   1335
         TabIndex        =   21
         Top             =   4200
         Width           =   855
      End
      Begin VB.Label Label1 
         Caption         =   "本词语播报次数"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Left            =   240
         TabIndex        =   6
         Top             =   240
         Width           =   3045
      End
      Begin VB.Label Label2 
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   3480
         TabIndex        =   7
         Top             =   240
         Width           =   1335
      End
      Begin VB.Label Label6 
         Caption         =   "自动播报间隔（秒）"
         Height          =   180
         Left            =   360
         TabIndex        =   8
         Top             =   720
         Width           =   1620
      End
      Begin VB.Label Label7 
         Caption         =   "自动播报次数（每个词语）"
         Height          =   180
         Left            =   360
         TabIndex        =   10
         Top             =   1080
         Width           =   2160
      End
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   11880
      Top             =   0
   End
   Begin VB.CommandButton Command2 
      Caption         =   "显示/隐藏词语控制区"
      Height          =   6975
      Left            =   7080
      TabIndex        =   4
      Top             =   240
      Width           =   375
   End
   Begin VB.Frame Frame1 
      Caption         =   "词语控制"
      Height          =   7095
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6855
      Begin VB.CommandButton cmdSave 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":324A
         Style           =   1  'Graphical
         TabIndex        =   40
         ToolTipText     =   "将列表项保存到一个TXT文件中（每个词语作为一行）"
         Top             =   720
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdAdd 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":334C
         Style           =   1  'Graphical
         TabIndex        =   39
         ToolTipText     =   "添加列表项"
         Top             =   2160
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDelete 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":344E
         Style           =   1  'Graphical
         TabIndex        =   38
         ToolTipText     =   "删除选定列表项"
         Top             =   3120
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdUp 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3550
         Style           =   1  'Graphical
         TabIndex        =   37
         ToolTipText     =   "向上移动"
         Top             =   1200
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDown 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3652
         Style           =   1  'Graphical
         TabIndex        =   36
         ToolTipText     =   "向下移动"
         Top             =   1680
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdClear 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":3994
         Style           =   1  'Graphical
         TabIndex        =   35
         ToolTipText     =   "清空列表"
         Top             =   3600
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdOpen 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3B36
         Style           =   1  'Graphical
         TabIndex        =   34
         ToolTipText     =   "从一个TXT文件中添加列表项（每行作为一个词语）"
         Top             =   240
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton Command4 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":3C38
         Style           =   1  'Graphical
         TabIndex        =   33
         ToolTipText     =   "修改选中列表项"
         Top             =   2640
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdShowCount 
         Caption         =   "计算词语数量"
         Height          =   1455
         Left            =   6360
         TabIndex        =   2
         Top             =   4200
         Width           =   375
      End
      Begin VB.CommandButton Command8 
         Caption         =   "快速选词"
         Enabled         =   0   'False
         Height          =   975
         Left            =   6360
         TabIndex        =   3
         Top             =   5880
         Width           =   375
      End
      Begin VB.ListBox List1 
         BeginProperty Font 
            Name            =   "宋体"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   6585
         ItemData        =   "Form1.frx":3D6A
         Left            =   120
         List            =   "Form1.frx":3D6C
         TabIndex        =   1
         Top             =   240
         Width           =   6255
      End
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   11280
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
      Filter          =   "文本文档(*.txt)|*.txt"
   End
   Begin VB.Label Label10 
      Caption         =   "随机排序：先计算词语数量，在使用快速选词中的随机选词，将随机选词数量填写为词语数量即可。"
      Height          =   465
      Left            =   7560
      TabIndex        =   30
      Top             =   6840
      Width           =   5040
   End
   Begin VB.Menu mnuFile 
      Caption         =   "文件(&F)"
      Begin VB.Menu mnuOpen 
         Caption         =   "从TXT文件中添加词语(&O)..."
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuSave 
         Caption         =   "将词语保存到TXT文件中(&S)..."
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuFileSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "退出(&E)"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "编辑(&E)"
      Begin VB.Menu mnuShowWordControl 
         Caption         =   "显示/隐藏词语控制区"
      End
      Begin VB.Menu mnuEditSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAdd 
         Caption         =   "添加词语..."
      End
      Begin VB.Menu mnuDelete 
         Caption         =   "删除选中词语..."
         Shortcut        =   {DEL}
      End
      Begin VB.Menu mnuClear 
         Caption         =   "清空词语列表"
      End
      Begin VB.Menu mnuModify 
         Caption         =   "修改选中词语..."
      End
      Begin VB.Menu mnuEditSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuUp 
         Caption         =   "向上移动"
      End
      Begin VB.Menu mnuDown 
         Caption         =   "向下移动"
      End
      Begin VB.Menu mnuEditSep3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSelected 
         Caption         =   "快速选词..."
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "帮助(&H)"
      Begin VB.Menu mnuAbout 
         Caption         =   "关于(&A)..."
      End
   End
   Begin VB.Menu mnuListPopup 
      Caption         =   "列表弹出菜单"
      Visible         =   0   'False
      Begin VB.Menu mnuUp_ListPopup 
         Caption         =   "上移"
      End
      Begin VB.Menu mnuDown_ListPopup 
         Caption         =   "下移"
      End
      Begin VB.Menu mnuModify_ListPopup 
         Caption         =   "修改"
      End
      Begin VB.Menu mnuDelete_ListPopup 
         Caption         =   "删除"
      End
      Begin VB.Menu mnuListPopupSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSpeakSelect_ListPopup 
         Caption         =   "读选定的词语"
      End
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim WithEvents A As SpVoice
Attribute A.VB_VarHelpID = -1
Dim objVoices As ISpeechObjectTokens
Dim i As Integer
Dim intBBCS As Integer
Dim intYGMS As Integer
Dim EnglishVoiceGood As Boolean, ChineseVoiceGood As Boolean

Private Sub A_EndStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
Command1.Enabled = True
Command3.Enabled = True
Command7.Enabled = True
Command9.Enabled = True
intBBCS = intBBCS + 1
Label2.Caption = CStr(intBBCS)
If Check1.Value = 1 Then
    If intBBCS = Val(Text2.Text) Then
        If i < List1.ListCount Then
        intYGMS = 0
        Label5.Caption = Text1.Text
        Timer1.Enabled = True
        If Val(Text1.Text) = 0 Then
        Timer1_Timer
        End If
        Else
        i = 0
        End If
    Else
        intYGMS = 0
        Label5.Caption = Text1.Text
        Timer1.Enabled = True
        If Val(Text1.Text) = 0 Then
        Timer1_Timer
        End If
    End If
End If
End Sub


Private Sub A_StartStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
Command1.Enabled = False
Command3.Enabled = False
Command7.Enabled = False
Command9.Enabled = False
Timer1.Enabled = False
Label5.Caption = "0"
End Sub


Private Sub cmdAdd_Click()
  Dim sTmp As String
  sTmp = InputBox("输入要添加的新项目：")
  If Len(sTmp) = 0 Then Exit Sub
  List1.AddItem sTmp
  SetListButtons
End Sub



Private Sub cmdDelete_Click()
  If List1.ListIndex > -1 Then
    If MsgBox("您真的要删除“" & List1.Text & "”吗？", vbQuestion + vbYesNo) = vbYes Then
      List1.RemoveItem List1.ListIndex
      SetListButtons
    End If
  End If
End Sub



Private Sub cmdSave_Click()
CommonDialog1.FileName = ""
CommonDialog1.ShowSave
If CommonDialog1.FileName <> "" Then
    Dim Temp As String
    Open CommonDialog1.FileName For Output As #1
        For i = 0 To List1.ListCount - 1
        Print #1, List1.List(i)
        Next
    Close #1
End If
End Sub



Private Sub Combo1_Click()
Set A.Voice = objVoices.Item(Combo1.ListIndex)
End Sub

Private Sub Command1_Click()
If i >= List1.ListCount Then
MsgBox "已经播完最后一个了。"
Exit Sub
End If
intBBCS = 0
List1.ListIndex = i
A.Speak List1.List(i), SVSFlagsAsync
i = i + 1
Label15.Caption = i
Label2.Caption = CStr(intBBCS)
End Sub

Private Sub cmdOpen_Click()
CommonDialog1.FileName = ""
CommonDialog1.ShowOpen
If CommonDialog1.FileName <> "" Then
OpenFile CommonDialog1.FileName
End If
End Sub

Private Sub cmdShowCount_Click()
MsgBox "词语数量：" & List1.ListCount
End Sub


Private Sub Command10_Click()
If Command10.Tag = "" Then
MsgBox "您的电脑中没有中文阅读引擎。"
Exit Sub
ElseIf ChineseVoiceGood = False Then
    If MsgBox("在您的电脑中没有发现质量较高的中文阅读引擎，" & _
    "是否切换到质量较差的中文阅读引擎？", vbYesNo) = vbNo Then
    Exit Sub
    End If
End If
Combo1.ListIndex = Val(Command10.Tag)
End Sub

Private Sub Command11_Click()
If Command11.Tag = "" Then
MsgBox "您的电脑中没有英文阅读引擎。"
Exit Sub
ElseIf EnglishVoiceGood = False Then
    If MsgBox("在您的电脑中没有发现质量较高的英文阅读引擎，" & _
    "是否切换到质量较差的英文阅读引擎？", vbYesNo) = vbNo Then
    Exit Sub
    End If
End If
Combo1.ListIndex = Val(Command11.Tag)
End Sub

Private Sub Command2_Click()
If Frame1.Visible = True Then
Frame1.Visible = False
Frame2.Left = 120
Frame3.Left = 120
Label10.Left = 120
Command2.Left = 5285
Me.Width = 5900
Else
Frame1.Visible = True
Frame2.Left = 7560
Frame3.Left = 7560
Label10.Left = 7560
Command2.Left = 7080
Me.Width = 12855
End If
End Sub

Private Sub Command3_Click()
If (i <= List1.ListCount) And (i > 0) Then
List1.ListIndex = i - 1
A.Speak List1.List(i - 1), SVSFlagsAsync
Else
MsgBox "还没报过了！"
End If
End Sub

Private Sub cmdClear_Click()
List1.Clear
SetListButtons
End Sub

Private Sub Command4_Click()
  If List1.ListIndex > -1 Then
    Dim strNewText As String
    strNewText = InputBox("您准备将“" & List1.Text & "”修改成", , List1.Text)
    If Len(strNewText) > 0 Then
      List1.List(List1.ListIndex) = strNewText
      SetListButtons
    End If
  End If
End Sub

Private Sub Command5_Click()
i = 0
End Sub

Private Sub cmdUp_Click()
  On Error Resume Next
  Dim nItem As Integer
  
  With List1
    If .ListIndex < 0 Then Exit Sub
    nItem = .ListIndex
    If nItem = 0 Then Exit Sub  '不能将第一个项目向上移动
    '向上移动项目
    .AddItem .Text, nItem - 1
    '删除旧项目
    .RemoveItem nItem + 1
    '选择刚刚移动的项目
    .Selected(nItem - 1) = True
  End With
End Sub

Private Sub cmdDown_Click()
  On Error Resume Next
  Dim nItem As Integer
  
  With List1
    If .ListIndex < 0 Then Exit Sub
    nItem = .ListIndex
    If nItem = .ListCount - 1 Then Exit Sub '不能将最后的项目向下移动
    '向下移动项目
    .AddItem .Text, nItem + 2
    '删除旧的项目
    .RemoveItem nItem
    '选择刚刚移动的项目
    .Selected(nItem + 1) = True
  End With
End Sub

Private Sub Command6_Click()
If List1.ListCount = 0 Then
MsgBox "请先添加词语！", vbOKOnly, Me.Caption
Exit Sub
End If
Label5.Caption = Text1.Text
intYGMS = 0
Timer1.Enabled = True
Check1.Value = 1
i = 0
Command1_Click
End Sub

Private Sub Command7_Click()
Timer1.Enabled = False
End Sub

Private Sub Command8_Click()
If List1.ListCount > 0 Then Form2.Show 1
End Sub

Private Sub Command9_Click()
If i <= List1.ListCount And i > 1 Then
i = i - 2
Command1_Click
Else
MsgBox "已经是第1个了！"
End If
End Sub

Private Sub Form_Load()
Dim EnglishGrade As Integer, ChineseGrade As Integer '声明用于记录语音引擎质量等级的变量
Dim objVoice As SpObjectToken
Dim strVoiceLanguage As String
Dim strVoiceName As String
EnglishGrade = 0 '初始化用于记录英文语音引擎质量等级的变量
ChineseGrade = 0 '初始化用于记录中文语音引擎质量等级的变量
Set A = New SpVoice '创建SpVoice对象实例
A.Volume = 100 '设定音量值为100
Set objVoices = A.GetVoices '读取发音人物集合
For i = 0 To objVoices.Count - 1 '遍历发音人物集合
Set objVoice = objVoices.Item(i) '读取发音人物集合中相应索引的语音对象
strVoiceName = objVoice.GetDescription '读取发音人物名称
Combo1.AddItem strVoiceName '在组合框中添加
    strVoiceLanguage = objVoice.GetAttribute("Language") '读取发音人物语言
    If strVoiceLanguage = "409" Or strVoiceLanguage Like "*;409" Or strVoiceLanguage Like "409;*" Or strVoiceLanguage Like "*;409;*" Then '当发音人物语言为英文时
        If (EnglishGrade = 0) Or (EnglishGrade > GetEnglishGrade(strVoiceName) And GetEnglishGrade(strVoiceName) <> 0) Then
        Command11.Tag = i
        EnglishGrade = GetEnglishGrade(strVoiceName)
        End If
    ElseIf strVoiceLanguage = "804" Or strVoiceLanguage Like "*;804" Or strVoiceLanguage Like "804;*" Or strVoiceLanguage Like "*;804;*" Then '当发音人物语言为中文时
        If (ChineseGrade = 0) Or (ChineseGrade > GetChineseGrade(strVoiceName) And GetChineseGrade(strVoiceName) <> 0) Then
        Command10.Tag = i
        ChineseGrade = GetChineseGrade(strVoiceName)
        End If
    End If
Next
EnglishVoiceGood = EnglishGrade
ChineseVoiceGood = ChineseGrade
If Combo1.ListCount <> 0 Then Combo1.ListIndex = 0
Slider1.Value = A.Rate
Slider2.Value = A.Volume
i = 0
intBBCS = 0
'支持文件关联方式打开：
Dim FileName As String
FileName = Command '读取运行参数
If Len(FileName) > 0 Then '运行参数不等于空时
    If Left$(FileName, 1) = Chr(34) Then '第一个字符等于“"”时
    FileName = Mid(FileName, 2, Len(FileName) - 2) '取第2个字符到（最后一个字符-1）的字符串
    End If
    If FSO.FileExists(FileName) = True Then '存在这个文件时
    OpenFile FileName '打开这个文件
    End If
End If
End Sub




Private Sub List1_Click()
  SetListButtons
End Sub

Sub SetListButtons()
  Dim IntListIndex As Integer
  Dim IntListCount As Integer
  IntListIndex = List1.ListIndex
  IntListCount = List1.ListCount
  '设置移动按钮的状态
  cmdUp.Enabled = (IntListIndex > 0)
  cmdDown.Enabled = ((IntListIndex > -1) And (IntListIndex < (IntListCount - 1)))
  cmdDelete.Enabled = (IntListIndex > -1)
  cmdClear.Enabled = IntListCount
  Command8.Enabled = IntListCount
  Command4.Enabled = (IntListIndex > -1)
End Sub

Private Sub List1_MouseDown(Button As Integer, Shift As Integer, X As Single, Y As Single)
If Button = 2 Then
    Dim pos As Long, idx As Long
    pos = X / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(List1.hwnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    List1.ListIndex = idx
    PopupMenu mnuListPopup
    Else
    List1.ListIndex = -1
    End If
End If
End Sub

Private Sub List1_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    Dim pos As Long, idx As Long
    pos = X / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(List1.hwnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    List1.ToolTipText = List1.List(idx)
    Else
    List1.ToolTipText = ""
    End If
End Sub

Private Sub mnuAbout_Click()
frmAbout.Show 1
End Sub



Private Sub mnuAdd_Click()
If List1.Visible = False Then Exit Sub
cmdAdd_Click
End Sub

Private Sub mnuClear_Click()
If List1.Visible = False Then Exit Sub
cmdClear_Click
End Sub

Private Sub mnuDelete_Click()
If List1.Visible = False Then Exit Sub
cmdDelete_Click
End Sub

Private Sub mnuDelete_ListPopup_Click()
mnuDelete_Click
End Sub

Private Sub mnuDown_Click()
If List1.Visible = False Then Exit Sub
cmdDown_Click
End Sub

Private Sub mnuDown_ListPopup_Click()
mnuDown_Click
End Sub

Private Sub mnuExit_Click()
Unload Me
End Sub

Private Sub mnuModify_Click()
If List1.Visible = False Then Exit Sub
Command4_Click
End Sub

Private Sub mnuModify_ListPopup_Click()
mnuModify_Click
End Sub

Private Sub mnuOpen_Click()
Call cmdOpen_Click
End Sub

Private Sub mnuSave_Click()
Call cmdSave_Click
End Sub


Private Sub mnuSelected_Click()
Command8_Click
End Sub

Private Sub mnuSpeakSelect_ListPopup_Click()
If i - 1 = List1.ListIndex Then
Command3_Click
Else
i = List1.ListIndex
Command1_Click
End If
End Sub

Private Sub mnuShowWordControl_Click()
Command2_Click
End Sub

Private Sub mnuUp_Click()
If List1.Visible = False Then Exit Sub
cmdUp_Click
End Sub

Private Sub mnuUp_ListPopup_Click()
mnuUp_Click
End Sub

Private Sub Slider1_Change()
A.Rate = Slider1.Value
End Sub


Private Sub Slider2_Change()
A.Volume = Slider2.Value
End Sub


Private Sub Text1_Change()
If Text1.Text = "" Then
Text1.Text = "0"
Text1.SelStart = 0
Text1.SelLength = 1
End If
End Sub

Private Sub Text1_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = vbKeyReturn Then
Text2.SelStart = 0
Text2.SelLength = Len(Text2.Text)
Text2.SetFocus
End If
End Sub

Private Sub Text1_KeyPress(KeyAscii As Integer)
If KeyAscii <> 8 And KeyAscii < 48 Or KeyAscii > 57 Then KeyAscii = 0
End Sub



Private Sub Text2_Change()
If Val(Text2.Text) = 0 Then
Text2.Text = "1"
Text2.SelStart = 0
Text2.SelLength = 1
End If
End Sub

Private Sub Text2_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = vbKeyReturn Then Call Command6_Click
End Sub

Private Sub Text2_KeyPress(KeyAscii As Integer)
If KeyAscii <> 8 And KeyAscii < 48 Or KeyAscii > 57 Then KeyAscii = 0
End Sub

Private Sub Timer1_Timer()
intYGMS = intYGMS + 1
If Val(Text1.Text) - intYGMS > 0 Then
Label5.Caption = CStr(Val(Text1.Text) - intYGMS)
Else
Label5.Caption = "0"
End If
If intYGMS >= Val(Text1.Text) Then
    If intBBCS >= Val(Text2.Text) Or i = 0 Then
    Command1_Click
    Else
    Command3_Click
    End If
End If
End Sub



Private Function GetEnglishGrade(VoiceName As String) As Integer
Select Case VoiceName
Case "VW Julie"
GetEnglishGrade = 1
Case "VW Paul"
GetEnglishGrade = 2
Case "VW Kate"
GetEnglishGrade = 3
Case Else
GetEnglishGrade = 0
End Select
End Function

Private Function GetChineseGrade(VoiceName As String) As Integer
Select Case VoiceName
Case "VW Lily"
GetChineseGrade = 1
Case "VW Hui"
GetChineseGrade = 2
Case "VW Liang"
GetChineseGrade = 3
Case "VW Wang"
GetChineseGrade = 4
Case "Girl XiaoKun"
GetChineseGrade = 5
Case Else
GetChineseGrade = 0
End Select
End Function

Public Sub OpenFile(FileName As String)
    Dim Temp As String
    Open FileName For Input As #1
    While Not EOF(1)
        Line Input #1, Temp
        If Temp <> "" Then List1.AddItem Temp
    Wend
    Close #1
    SetListButtons
End Sub
