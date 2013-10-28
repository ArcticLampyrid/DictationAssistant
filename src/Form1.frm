VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"; "mscomctl.ocx"
Begin VB.Form Form1 
   AutoRedraw      =   -1  'True
   BorderStyle     =   1  'Fixed Single
   Caption         =   "自动默写"
   ClientHeight    =   7335
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   12765
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   7335
   ScaleWidth      =   12765
   StartUpPosition =   3  '窗口缺省
   Begin VB.Frame Frame3 
      Caption         =   "TTS语音引擎"
      Height          =   1695
      Left            =   7560
      TabIndex        =   30
      Top             =   4440
      Width           =   5055
      Begin VB.ComboBox Combo1 
         Height          =   300
         Left            =   1440
         Style           =   2  'Dropdown List
         TabIndex        =   33
         Top             =   1200
         Width           =   3375
      End
      Begin MSComctlLib.Slider Slider2 
         Height          =   255
         Left            =   1440
         TabIndex        =   31
         Top             =   240
         Width           =   3375
         _ExtentX        =   5953
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   10
         SmallChange     =   5
         Max             =   100
         TickFrequency   =   10
      End
      Begin MSComctlLib.Slider Slider1 
         Height          =   255
         Left            =   1440
         TabIndex        =   32
         Top             =   720
         Width           =   3375
         _ExtentX        =   5953
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   3
         Min             =   -10
      End
      Begin VB.Label Label8 
         Caption         =   "引擎："
         Height          =   255
         Left            =   240
         TabIndex        =   36
         Top             =   1200
         Width           =   975
      End
      Begin VB.Label Label9 
         Caption         =   "语速："
         Height          =   375
         Left            =   240
         TabIndex        =   35
         Top             =   720
         Width           =   975
      End
      Begin VB.Label Label11 
         Caption         =   "音量："
         Height          =   375
         Left            =   240
         TabIndex        =   34
         Top             =   240
         Width           =   975
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "词语播报"
      Height          =   4215
      Left            =   7560
      TabIndex        =   13
      Top             =   120
      Width           =   5055
      Begin VB.CommandButton Command1 
         Caption         =   "报下一个(&N)"
         Height          =   375
         Left            =   1800
         TabIndex        =   22
         Top             =   2160
         Width           =   1455
      End
      Begin VB.CommandButton Command3 
         Caption         =   "再报一遍(&A)"
         Height          =   375
         Left            =   3360
         TabIndex        =   21
         Top             =   2640
         Width           =   1455
      End
      Begin VB.CommandButton Command5 
         Caption         =   "从头开始(&H)"
         Height          =   375
         Left            =   1800
         TabIndex        =   20
         Top             =   2640
         Width           =   1455
      End
      Begin VB.TextBox Text1 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   19
         Text            =   "3"
         Top             =   960
         Width           =   1695
      End
      Begin VB.CommandButton Command6 
         Caption         =   "开始自动播报(&S)"
         Height          =   375
         Left            =   360
         TabIndex        =   18
         Top             =   1680
         Width           =   4455
      End
      Begin VB.TextBox Text2 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   17
         Text            =   "2"
         Top             =   1320
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
         Height          =   1335
         Left            =   360
         TabIndex        =   16
         Top             =   2160
         Width           =   1335
      End
      Begin VB.CommandButton Command7 
         Caption         =   "暂停自动(&P)"
         Height          =   375
         Left            =   1800
         TabIndex        =   15
         Top             =   3120
         Width           =   3015
      End
      Begin VB.CommandButton Command9 
         Caption         =   "报上一个(&L)"
         Height          =   375
         Left            =   3360
         TabIndex        =   14
         Top             =   2160
         Width           =   1455
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
         TabIndex        =   29
         Top             =   3600
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
         TabIndex        =   28
         Top             =   3600
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
         TabIndex        =   27
         Top             =   3600
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
         Height          =   495
         Left            =   240
         TabIndex        =   26
         Top             =   240
         Width           =   3135
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
         TabIndex        =   25
         Top             =   240
         Width           =   1335
      End
      Begin VB.Label Label6 
         Caption         =   "自动播报间隔（秒）"
         Height          =   255
         Left            =   360
         TabIndex        =   24
         Top             =   960
         Width           =   2655
      End
      Begin VB.Label Label7 
         Caption         =   "自动播报次数（每个词语）"
         Height          =   375
         Left            =   360
         TabIndex        =   23
         Top             =   1320
         Width           =   2655
      End
   End
   Begin VB.Timer Timer1 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   11880
      Top             =   0
   End
   Begin VB.CommandButton Command2 
      Caption         =   "显示/隐藏词语控制"
      Height          =   6975
      Left            =   7080
      TabIndex        =   9
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
      Begin VB.CommandButton Command10 
         Caption         =   "计算词语数量"
         Height          =   1455
         Left            =   6360
         TabIndex        =   11
         Top             =   4320
         Width           =   375
      End
      Begin VB.CommandButton Command8 
         Caption         =   "快速选词"
         Enabled         =   0   'False
         Height          =   975
         Left            =   6360
         TabIndex        =   10
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
         ItemData        =   "Form1.frx":0000
         Left            =   240
         List            =   "Form1.frx":0002
         TabIndex        =   8
         Top             =   240
         Width           =   6135
      End
      Begin VB.CommandButton cmdOpen 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":0004
         Style           =   1  'Graphical
         TabIndex        =   7
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
         Picture         =   "Form1.frx":0106
         Style           =   1  'Graphical
         TabIndex        =   6
         ToolTipText     =   "清空列表"
         Top             =   3840
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDown 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":02A8
         Style           =   1  'Graphical
         TabIndex        =   5
         ToolTipText     =   "向下移动"
         Top             =   2040
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdUp 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":05EA
         Style           =   1  'Graphical
         TabIndex        =   4
         ToolTipText     =   "向上移动"
         Top             =   1440
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDelete 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":06EC
         Style           =   1  'Graphical
         TabIndex        =   3
         ToolTipText     =   "删除选定列表项"
         Top             =   3240
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdAdd 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":07EE
         Style           =   1  'Graphical
         TabIndex        =   2
         ToolTipText     =   "添加列表项"
         Top             =   2640
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdSave 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":08F0
         Style           =   1  'Graphical
         TabIndex        =   1
         ToolTipText     =   "将列表项保存到一个TXT文件中（每个词语作为一行）"
         Top             =   840
         UseMaskColor    =   -1  'True
         Width           =   330
      End
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   11280
      Top             =   0
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin VB.Label Label10 
      Caption         =   "技巧说明：先计算词语数量，在使用快速选词中的随机选词，将随机选词数量填写为词语数量即可进行随机排序。"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   14.25
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   945
      Left            =   7560
      TabIndex        =   12
      Top             =   6240
      Width           =   5040
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Dim WithEvents A As SpVoice
Attribute A.VB_VarHelpID = -1
Dim I As Integer
Dim intBBCS As Integer
Dim intYGMS As Integer

Private Sub A_EndStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
Command1.Enabled = True
Command3.Enabled = True
Command7.Enabled = True
Command9.Enabled = True
intBBCS = intBBCS + 1
Label2.Caption = CStr(intBBCS)
If Check1.Value = 1 Then
    If intBBCS = Val(Text2.Text) Then
        If I < List1.ListCount Then
        intYGMS = 0
        Label5.Caption = Text1.Text
        Timer1.Enabled = True
        If Val(Text1.Text) = 0 Then
        Timer1_Timer
        End If
        Else
        I = 0
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
    If MsgBox("删除 '" & List1.Text & "'?", vbQuestion + vbYesNo) = vbYes Then
      List1.RemoveItem List1.ListIndex
      SetListButtons
    End If
  End If
End Sub



Private Sub cmdSave_Click()
CommonDialog1.Filter = "文本文档(*.txt)|*.txt"
CommonDialog1.ShowSave
If CommonDialog1.FileName <> "" Then
    Dim Temp As String
    Open CommonDialog1.FileName For Output As #1
        For I = 0 To List1.ListCount - 1
        Print #1, List1.List(I)
        Next
    Close #1
End If
End Sub

Private Sub Combo1_Click()
Set A.Voice = A.GetVoices.Item(Combo1.ListIndex)
End Sub

Private Sub Command1_Click()
If I >= List1.ListCount Then
MsgBox "已经播完最后一个了。"
Exit Sub
End If
intBBCS = 0
List1.ListIndex = I
A.Speak List1.List(I), SVSFlagsAsync
I = I + 1
Label2.Caption = CStr(intBBCS)
End Sub

Private Sub cmdOpen_Click()
CommonDialog1.Filter = "文本文档(*.txt)|*.txt"
CommonDialog1.ShowOpen
If CommonDialog1.FileName <> "" Then
    Dim Temp As String
    Open CommonDialog1.FileName For Input As #1
    While Not EOF(1)
        Line Input #1, Temp
        If Temp <> "" And Temp <> " " Then List1.AddItem Temp
    Wend
    Close #1
    SetListButtons
End If
End Sub

Private Sub Command10_Click()
MsgBox "词语数量：" & List1.ListCount
End Sub

Private Sub Command11_Click()
MsgBox Me.Width
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
Me.Width = 13000
End If
End Sub

Private Sub Command3_Click()
If I <= List1.ListCount And I > 0 Then
List1.ListIndex = I - 1
A.Speak List1.List(I - 1), SVSFlagsAsync
End If
End Sub

Private Sub Command4_Click()
List1.Clear
SetListButtons
End Sub

Private Sub Command5_Click()
I = 0
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
If Text2.Text = "" Or Text2.Text = "0" Then Text2.Text = "1"
MsgBox "单击确定按钮开始，开始后修改播报间隔（秒）和播报次数也会及时生效，如无必要，请勿修改。", vbOKOnly, Me.Caption
Label5.Caption = Text1.Text
intYGMS = 0
Timer1.Enabled = True
Check1.Value = 1
I = 0
Command1_Click
End Sub

Private Sub Command7_Click()
Timer1.Enabled = False
End Sub

Private Sub Command8_Click()
Form2.Show
End Sub

Private Sub Command9_Click()
If I <= List1.ListCount And I > 1 Then
I = I - 2
Command1_Click
End If
End Sub

Private Sub Form_Load()
Set A = New SpVoice
A.Volume = 100
For I = 0 To A.GetVoices.Count - 1
Combo1.AddItem A.GetVoices.Item(I).GetDescription
Next
Combo1.ListIndex = 0
Slider1.Value = A.Rate
Slider2.Value = A.Volume
I = 0
intBBCS = 0
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
  Command4.Enabled = IntListCount
  Command8.Enabled = IntListCount
End Sub

Private Sub Slider1_Change()
A.Rate = Slider1.Value
End Sub


Private Sub Slider2_Change()
A.Volume = Slider2.Value
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
    If intBBCS >= Val(Text2.Text) Or I = 0 Then
    Command1_Click
    Else
    Command3_Click
    End If
End If
End Sub
