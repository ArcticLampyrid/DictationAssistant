VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"; "mscomctl.ocx"
Begin VB.Form Form1 
   AutoRedraw      =   -1  'True
   BorderStyle     =   1  'Fixed Single
   Caption         =   "�Զ�Ĭд"
   ClientHeight    =   7335
   ClientLeft      =   150
   ClientTop       =   480
   ClientWidth     =   12765
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   MaxButton       =   0   'False
   ScaleHeight     =   7335
   ScaleWidth      =   12765
   StartUpPosition =   2  '��Ļ����
   Begin VB.Frame Frame3 
      Caption         =   "TTS��������"
      Height          =   1695
      Left            =   7560
      TabIndex        =   31
      Top             =   4440
      Width           =   5055
      Begin VB.ComboBox Combo1 
         Height          =   300
         Left            =   720
         Style           =   2  'Dropdown List
         TabIndex        =   37
         Top             =   1200
         Width           =   4215
      End
      Begin MSComctlLib.Slider Slider2 
         Height          =   255
         Left            =   720
         TabIndex        =   33
         Top             =   240
         Width           =   4215
         _ExtentX        =   7435
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   10
         SmallChange     =   5
         Max             =   100
         TickFrequency   =   5
      End
      Begin MSComctlLib.Slider Slider1 
         Height          =   255
         Left            =   720
         TabIndex        =   35
         Top             =   720
         Width           =   4215
         _ExtentX        =   7435
         _ExtentY        =   450
         _Version        =   393216
         LargeChange     =   3
         Min             =   -10
      End
      Begin VB.Label Label8 
         AutoSize        =   -1  'True
         Caption         =   "���棺"
         Height          =   180
         Left            =   120
         TabIndex        =   36
         Top             =   1200
         Width           =   540
      End
      Begin VB.Label Label9 
         AutoSize        =   -1  'True
         Caption         =   "���٣�"
         Height          =   180
         Left            =   120
         TabIndex        =   34
         Top             =   720
         Width           =   540
      End
      Begin VB.Label Label11 
         AutoSize        =   -1  'True
         Caption         =   "������"
         Height          =   180
         Left            =   120
         TabIndex        =   32
         Top             =   240
         Width           =   540
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "���ﲥ��"
      Height          =   4215
      Left            =   7560
      TabIndex        =   13
      Top             =   120
      Width           =   5055
      Begin VB.CommandButton Command1 
         Caption         =   "����һ��(&N)"
         Height          =   375
         Left            =   1800
         TabIndex        =   24
         Top             =   2640
         Width           =   1455
      End
      Begin VB.CommandButton Command3 
         Caption         =   "�ٱ�һ��(&A)"
         Height          =   375
         Left            =   3360
         TabIndex        =   27
         Top             =   3120
         Width           =   1455
      End
      Begin VB.CommandButton Command5 
         Caption         =   "��ͷ��ʼ(&H)"
         Height          =   375
         Left            =   1800
         TabIndex        =   26
         Top             =   3120
         Width           =   1455
      End
      Begin VB.TextBox Text1 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   17
         Text            =   "3"
         Top             =   720
         Width           =   1695
      End
      Begin VB.CommandButton Command6 
         Caption         =   "��ʼ�Զ�����(&S)"
         Height          =   375
         Left            =   360
         TabIndex        =   20
         Top             =   1440
         Width           =   2175
      End
      Begin VB.TextBox Text2 
         Height          =   270
         Left            =   3120
         MaxLength       =   3
         TabIndex        =   19
         Text            =   "2"
         Top             =   1080
         Width           =   1695
      End
      Begin VB.CheckBox Check1 
         Caption         =   "�Զ�����"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   855
         Left            =   360
         TabIndex        =   23
         Top             =   2640
         Width           =   1335
      End
      Begin VB.CommandButton Command7 
         Caption         =   "��ͣ�Զ�����(&P)"
         Height          =   375
         Left            =   2640
         TabIndex        =   21
         Top             =   1440
         Width           =   2175
      End
      Begin VB.CommandButton Command9 
         Caption         =   "����һ��(&L)"
         Height          =   375
         Left            =   3360
         TabIndex        =   25
         Top             =   2640
         Width           =   1455
      End
      Begin VB.Label Label12 
         Caption         =   "��ͣ�Զ������󲻰ѡ��Զ������������ȥ���Ļ�����������һ������������һ�������ٱ�һ�顱����Զ���ʼ��һ���Զ�������"
         Height          =   615
         Left            =   360
         TabIndex        =   22
         Top             =   1920
         Width           =   4455
      End
      Begin VB.Label Label3 
         Caption         =   "����"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   360
         TabIndex        =   28
         Top             =   3600
         Width           =   975
      End
      Begin VB.Label Label4 
         Caption         =   "����Զ�����"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2205
         TabIndex        =   30
         Top             =   3600
         Width           =   2655
      End
      Begin VB.Label Label5 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   1335
         TabIndex        =   29
         Top             =   3600
         Width           =   855
      End
      Begin VB.Label Label1 
         Caption         =   "�����ﲥ������"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   435
         Left            =   240
         TabIndex        =   14
         Top             =   240
         Width           =   3045
      End
      Begin VB.Label Label2 
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   3480
         TabIndex        =   15
         Top             =   240
         Width           =   1335
      End
      Begin VB.Label Label6 
         Caption         =   "�Զ�����������룩"
         Height          =   180
         Left            =   360
         TabIndex        =   16
         Top             =   720
         Width           =   1620
      End
      Begin VB.Label Label7 
         Caption         =   "�Զ�����������ÿ�����"
         Height          =   180
         Left            =   360
         TabIndex        =   18
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
      Caption         =   "��ʾ/���ش������"
      Height          =   6975
      Left            =   7080
      TabIndex        =   12
      Top             =   240
      Width           =   375
   End
   Begin VB.Frame Frame1 
      Caption         =   "�������"
      Height          =   7095
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6855
      Begin VB.CommandButton Command4 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":324A
         Style           =   1  'Graphical
         TabIndex        =   7
         ToolTipText     =   "�޸�ѡ���б���"
         Top             =   2640
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdShowCount 
         Caption         =   "�����������"
         Height          =   1455
         Left            =   6360
         TabIndex        =   10
         Top             =   4200
         Width           =   375
      End
      Begin VB.CommandButton Command8 
         Caption         =   "����ѡ��"
         Enabled         =   0   'False
         Height          =   975
         Left            =   6360
         TabIndex        =   11
         Top             =   5880
         Width           =   375
      End
      Begin VB.ListBox List1 
         BeginProperty Font 
            Name            =   "����"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   6585
         ItemData        =   "Form1.frx":337C
         Left            =   240
         List            =   "Form1.frx":337E
         TabIndex        =   1
         Top             =   240
         Width           =   6135
      End
      Begin VB.CommandButton cmdOpen 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3380
         Style           =   1  'Graphical
         TabIndex        =   2
         ToolTipText     =   "��һ��TXT�ļ�������б��ÿ����Ϊһ�����"
         Top             =   240
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdClear 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":3482
         Style           =   1  'Graphical
         TabIndex        =   9
         ToolTipText     =   "����б�"
         Top             =   3600
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDown 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3624
         Style           =   1  'Graphical
         TabIndex        =   5
         ToolTipText     =   "�����ƶ�"
         Top             =   1680
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdUp 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3966
         Style           =   1  'Graphical
         TabIndex        =   4
         ToolTipText     =   "�����ƶ�"
         Top             =   1200
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDelete 
         Enabled         =   0   'False
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3A68
         Style           =   1  'Graphical
         TabIndex        =   8
         ToolTipText     =   "ɾ��ѡ���б���"
         Top             =   3120
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdAdd 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3B6A
         Style           =   1  'Graphical
         TabIndex        =   6
         ToolTipText     =   "����б���"
         Top             =   2160
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdSave 
         Height          =   330
         Left            =   6360
         Picture         =   "Form1.frx":3C6C
         Style           =   1  'Graphical
         TabIndex        =   3
         ToolTipText     =   "���б���浽һ��TXT�ļ��У�ÿ��������Ϊһ�У�"
         Top             =   720
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
      Caption         =   "����˵�����ȼ��������������ʹ�ÿ���ѡ���е����ѡ�ʣ������ѡ��������дΪ�����������ɽ����������"
      BeginProperty Font 
         Name            =   "����"
         Size            =   14.25
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   945
      Left            =   7560
      TabIndex        =   38
      Top             =   6240
      Width           =   5040
   End
   Begin VB.Menu mnuFile 
      Caption         =   "�ļ�(&F)"
      Begin VB.Menu mnuOpen 
         Caption         =   "��TXT�ļ�������б���(&O)..."
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuSave 
         Caption         =   "���б���浽TXT�ļ���(&S)..."
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuFileSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "�˳�(&E)"
      End
   End
   Begin VB.Menu mnuHelp 
      Caption         =   "����(&H)"
      Begin VB.Menu mnuAbout 
         Caption         =   "����(&A)..."
      End
   End
   Begin VB.Menu mnuListPopup 
      Caption         =   "�б����˵�"
      Visible         =   0   'False
      Begin VB.Menu mnuSpeakSelect 
         Caption         =   "��ѡ���Ĵ���"
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
Dim i As Integer
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
  sTmp = InputBox("����Ҫ��ӵ�����Ŀ��")
  If Len(sTmp) = 0 Then Exit Sub
  List1.AddItem sTmp
  SetListButtons
End Sub



Private Sub cmdDelete_Click()
  If List1.ListIndex > -1 Then
    If MsgBox("ɾ�� '" & List1.Text & "'?", vbQuestion + vbYesNo) = vbYes Then
      List1.RemoveItem List1.ListIndex
      SetListButtons
    End If
  End If
End Sub



Private Sub cmdSave_Click()
CommonDialog1.Filter = "�ı��ĵ�(*.txt)|*.txt"
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
Set A.Voice = A.GetVoices.Item(Combo1.ListIndex)
End Sub

Private Sub Command1_Click()
If i >= List1.ListCount Then
MsgBox "�Ѿ��������һ���ˡ�"
Exit Sub
End If
intBBCS = 0
List1.ListIndex = i
A.Speak List1.List(i), SVSFlagsAsync
i = i + 1
Label2.Caption = CStr(intBBCS)
End Sub

Private Sub cmdOpen_Click()
CommonDialog1.Filter = "�ı��ĵ�(*.txt)|*.txt"
CommonDialog1.ShowOpen
If CommonDialog1.FileName <> "" Then
    Dim Temp As String
    Open CommonDialog1.FileName For Input As #1
    While Not EOF(1)
        Line Input #1, Temp
        If Temp <> "" Then List1.AddItem Temp
    Wend
    Close #1
    SetListButtons
    List1.SetFocus
End If
End Sub

Private Sub cmdShowCount_Click()
MsgBox "����������" & List1.ListCount
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
If i <= List1.ListCount And i > 0 Then
List1.ListIndex = i - 1
A.Speak List1.List(i - 1), SVSFlagsAsync
End If
End Sub

Private Sub cmdClear_Click()
List1.Clear
SetListButtons
End Sub

Private Sub Command4_Click()
  If List1.ListIndex > -1 Then
    Dim strNewText As String
    strNewText = InputBox("��׼���� '" & List1.Text & "'�޸ĳ�?", , List1.Text)
    If strNewText <> "" Then
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
    If nItem = 0 Then Exit Sub  '���ܽ���һ����Ŀ�����ƶ�
    '�����ƶ���Ŀ
    .AddItem .Text, nItem - 1
    'ɾ������Ŀ
    .RemoveItem nItem + 1
    'ѡ��ո��ƶ�����Ŀ
    .Selected(nItem - 1) = True
  End With
End Sub

Private Sub cmdDown_Click()
  On Error Resume Next
  Dim nItem As Integer
  
  With List1
    If .ListIndex < 0 Then Exit Sub
    nItem = .ListIndex
    If nItem = .ListCount - 1 Then Exit Sub '���ܽ�������Ŀ�����ƶ�
    '�����ƶ���Ŀ
    .AddItem .Text, nItem + 2
    'ɾ���ɵ���Ŀ
    .RemoveItem nItem
    'ѡ��ո��ƶ�����Ŀ
    .Selected(nItem + 1) = True
  End With
End Sub

Private Sub Command6_Click()
If List1.ListCount = 0 Then
MsgBox "������Ӵ��", vbOKOnly, Me.Caption
Exit Sub
End If
MsgBox "��ʼ���޸Ĳ���������룩�Ͳ�������Ҳ�ἰʱ��Ч�����ޱ�Ҫ�������޸ġ�", vbOKOnly, Me.Caption
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
Form2.Show 1
End Sub

Private Sub Command9_Click()
If i <= List1.ListCount And i > 1 Then
i = i - 2
Command1_Click
End If
End Sub

Private Sub Form_Load()
Set A = New SpVoice
A.Volume = 100
For i = 0 To A.GetVoices.Count - 1
Combo1.AddItem A.GetVoices.Item(i).GetDescription
Next
Combo1.ListIndex = 0
Slider1.Value = A.Rate
Slider2.Value = A.Volume
i = 0
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
  '�����ƶ���ť��״̬
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



Private Sub mnuExit_Click()
Unload Me
End Sub

Private Sub mnuOpen_Click()
Call cmdOpen_Click
End Sub

Private Sub mnuSave_Click()
Call cmdSave_Click
End Sub

Private Sub mnuSpeakSelect_Click()
If i - 1 = List1.ListIndex Then
Command3_Click
Else
i = List1.ListIndex
Command1_Click
End If
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
