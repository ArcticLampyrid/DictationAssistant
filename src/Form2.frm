VERSION 5.00
Begin VB.Form Form2 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "快速选词"
   ClientHeight    =   6090
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   11610
   Icon            =   "Form2.frx":0000
   LinkTopic       =   "Form2"
   MaxButton       =   0   'False
   ScaleHeight     =   6090
   ScaleWidth      =   11610
   StartUpPosition =   1  '所有者中心
   Begin VB.TextBox Text1 
      Height          =   270
      Left            =   10200
      TabIndex        =   12
      Text            =   "60"
      Top             =   600
      Width           =   1335
   End
   Begin VB.CommandButton Command2 
      Caption         =   "随机选词"
      Height          =   375
      Left            =   10200
      TabIndex        =   11
      Top             =   960
      Width           =   1335
   End
   Begin VB.CommandButton cmdUp 
      Height          =   450
      Left            =   10200
      Picture         =   "Form2.frx":000C
      Style           =   1  'Graphical
      TabIndex        =   10
      Top             =   2160
      UseMaskColor    =   -1  'True
      Width           =   450
   End
   Begin VB.CommandButton cmdDown 
      Height          =   450
      Left            =   10200
      Picture         =   "Form2.frx":010E
      Style           =   1  'Graphical
      TabIndex        =   9
      Top             =   2760
      UseMaskColor    =   -1  'True
      Width           =   450
   End
   Begin VB.CommandButton Command1 
      Caption         =   "生效"
      Height          =   375
      Left            =   10200
      TabIndex        =   8
      Top             =   5520
      Width           =   1335
   End
   Begin VB.ListBox lstAll 
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   21.75
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5715
      Left            =   0
      TabIndex        =   5
      Top             =   255
      Width           =   4740
   End
   Begin VB.ListBox lstSelected 
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   21.75
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5715
      Left            =   5400
      TabIndex        =   4
      Top             =   255
      Width           =   4740
   End
   Begin VB.CommandButton cmdRightOne 
      Caption         =   ">"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9.75
         Charset         =   134
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   336
      Left            =   4800
      MaskColor       =   &H00000000&
      TabIndex        =   3
      Top             =   2040
      Width           =   576
   End
   Begin VB.CommandButton cmdRightAll 
      Caption         =   ">>"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9.75
         Charset         =   134
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   336
      Left            =   4800
      MaskColor       =   &H00000000&
      TabIndex        =   2
      Top             =   2400
      Width           =   576
   End
   Begin VB.CommandButton cmdLeftOne 
      Caption         =   "<"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9.75
         Charset         =   134
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   336
      Left            =   4800
      MaskColor       =   &H00000000&
      TabIndex        =   1
      Top             =   2805
      Width           =   576
   End
   Begin VB.CommandButton cmdLeftAll 
      Caption         =   "<<"
      BeginProperty Font 
         Name            =   "宋体"
         Size            =   9.75
         Charset         =   134
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   336
      Left            =   4800
      MaskColor       =   &H00000000&
      TabIndex        =   0
      Top             =   3180
      Width           =   576
   End
   Begin VB.Label Label1 
      Caption         =   "随机选词数量："
      Height          =   255
      Left            =   10200
      TabIndex        =   13
      Top             =   240
      Width           =   1335
   End
   Begin VB.Label lblAll 
      Appearance      =   0  'Flat
      AutoSize        =   -1  'True
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "所有项目:"
      ForeColor       =   &H80000008&
      Height          =   195
      Left            =   0
      TabIndex        =   7
      Tag             =   "2406"
      Top             =   0
      Width           =   630
   End
   Begin VB.Label lblSelected 
      Appearance      =   0  'Flat
      AutoSize        =   -1  'True
      BackColor       =   &H80000005&
      BackStyle       =   0  'Transparent
      Caption         =   "要保留的项目:"
      ForeColor       =   &H80000008&
      Height          =   180
      Left            =   5520
      TabIndex        =   6
      Tag             =   "2407"
      Top             =   0
      Width           =   1170
   End
End
Attribute VB_Name = "Form2"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'i在Form1是记录当前播报项目的与这里的i不关联
Private Sub cmdUp_Click()
  On Error Resume Next
  Dim nItem As Integer
  
  With lstSelected
    If .ListIndex < 0 Then Exit Sub
    nItem = .ListIndex
    If nItem = 0 Then Exit Sub  '不能将第一个项目向上移动
    '向上移动项目
    .AddItem .Text, nItem - 1
    '删除旧的项目
    .RemoveItem nItem + 1
    '选择刚刚被移动的项目
    .Selected(nItem - 1) = True
  End With
End Sub

Private Sub cmdDown_Click()
  On Error Resume Next
  Dim nItem As Integer
  
  With lstSelected
    If .ListIndex < 0 Then Exit Sub
    nItem = .ListIndex
    If nItem = .ListCount - 1 Then Exit Sub '不能将最后的项目向下移动
    '向下移动项目
    .AddItem .Text, nItem + 2
    '删除旧的项目
    .RemoveItem nItem
    '选择刚刚被移动的项目
    .Selected(nItem + 1) = True
  End With
End Sub

Private Sub cmdRightOne_Click()
  On Error Resume Next
  Dim i As Integer
  
  If lstAll.ListCount = 0 Then Exit Sub
  
  lstSelected.AddItem lstAll.Text
  i = lstAll.ListIndex
  lstAll.RemoveItem lstAll.ListIndex
  If lstAll.ListCount > 0 Then
    If i > lstAll.ListCount - 1 Then
      lstAll.ListIndex = i - 1
    Else
      lstAll.ListIndex = i
    End If
  End If
  lstSelected.ListIndex = lstSelected.NewIndex
End Sub

Private Sub cmdRightAll_Click()
  On Error Resume Next
  Dim i As Integer
  For i = 0 To lstAll.ListCount - 1
    lstSelected.AddItem lstAll.List(i)
  Next
  lstAll.Clear
  lstSelected.ListIndex = 0
End Sub

Private Sub cmdLeftOne_Click()
  On Error Resume Next
  Dim i As Integer
  
  If lstSelected.ListCount = 0 Then Exit Sub
  
  lstAll.AddItem lstSelected.Text
  i = lstSelected.ListIndex
  lstSelected.RemoveItem i
  
  lstAll.ListIndex = lstAll.NewIndex
  If lstSelected.ListCount > 0 Then
    If i > lstSelected.ListCount - 1 Then
      lstSelected.ListIndex = i - 1
    Else
      lstSelected.ListIndex = i
    End If
  End If
End Sub

Private Sub cmdLeftAll_Click()
  On Error Resume Next
  Dim i As Integer
  For i = 0 To lstSelected.ListCount - 1
    lstAll.AddItem lstSelected.List(i)
  Next
  lstSelected.Clear
  lstAll.ListIndex = lstAll.NewIndex

End Sub

Private Sub Command1_Click()
Form1.List1.Clear
For i = 0 To lstSelected.ListCount - 1
Form1.List1.AddItem lstSelected.List(i)
Next
Unload Me
Form1.SetListButtons
End Sub

Private Sub Command2_Click()
If Val(Text1.Text) > Form1.List1.ListCount Then
MsgBox "超出词语数量！"
Exit Sub
End If
Call cmdLeftAll_Click
For i = 1 To Val(Text1.Text)
Randomize
lstAll.ListIndex = Int(lstAll.ListCount * Rnd)
cmdRightOne_Click
Next
End Sub

Private Sub Form_Load()
For i = 0 To Form1.List1.ListCount - 1
lstAll.AddItem Form1.List1.List(i)
Next
If lstAll.ListCount < 60 Then Text1.Text = CStr(lstAll.ListCount)
End Sub


Private Sub lstAll_DblClick()
  cmdRightOne_Click
End Sub

Private Sub lstAll_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = vbKeyReturn Then Call lstAll_DblClick
End Sub

Private Sub lstAll_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    Dim pos As Long, idx As Long
    pos = X / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(lstAll.hwnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    lstAll.ToolTipText = lstAll.List(idx)
    Else
    lstAll.ToolTipText = ""
    End If
End Sub

Private Sub lstSelected_DblClick()
  cmdLeftOne_Click
End Sub

Private Sub lstSelected_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = vbKeyReturn Then Call lstSelected_DblClick
End Sub

Private Sub lstSelected_MouseMove(Button As Integer, Shift As Integer, X As Single, Y As Single)
    Dim pos As Long, idx As Long
    pos = X / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(lstSelected.hwnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    lstSelected.ToolTipText = lstSelected.List(idx)
    Else
    lstSelected.ToolTipText = ""
    End If
End Sub

Private Sub Text1_Change()
If Val(Text1.Text) < 1 Then
Text1.Text = "1"
Text1.SelStart = 0
Text1.SelLength = 1
End If
End Sub

Private Sub Text1_KeyDown(KeyCode As Integer, Shift As Integer)
If KeyCode = vbKeyReturn Then Call Command2_Click
End Sub

Private Sub Text1_KeyPress(KeyAscii As Integer)
If KeyAscii <> 8 And KeyAscii < 48 Or KeyAscii > 57 Then KeyAscii = 0
End Sub
