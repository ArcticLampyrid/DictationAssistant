VERSION 5.00
Begin VB.Form Form3 
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "���õ�����ǿĿ¼"
   ClientHeight    =   1125
   ClientLeft      =   45
   ClientTop       =   375
   ClientWidth     =   7275
   Icon            =   "Form3.frx":0000
   LinkTopic       =   "Form3"
   MaxButton       =   0   'False
   MinButton       =   0   'False
   ScaleHeight     =   1125
   ScaleWidth      =   7275
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  '����ȱʡ
   Begin VB.CommandButton Command3 
      Caption         =   "ȡ��"
      Height          =   375
      Left            =   5040
      TabIndex        =   5
      Top             =   600
      Width           =   975
   End
   Begin VB.CheckBox Check1 
      Caption         =   "���õ�����ǿ"
      Height          =   255
      Left            =   120
      TabIndex        =   4
      Top             =   600
      Width           =   1455
   End
   Begin VB.CommandButton Command2 
      Caption         =   "ȷ��"
      Height          =   375
      Left            =   6120
      TabIndex        =   3
      Top             =   600
      Width           =   975
   End
   Begin VB.CommandButton Command1 
      Caption         =   "���"
      Height          =   375
      Left            =   6120
      TabIndex        =   2
      Top             =   120
      Width           =   975
   End
   Begin VB.TextBox Text1 
      Height          =   375
      Left            =   1560
      TabIndex        =   1
      Top             =   120
      Width           =   4575
   End
   Begin VB.Label Label1 
      Caption         =   "������ǿĿ¼��"
      Height          =   255
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   1335
   End
End
Attribute VB_Name = "Form3"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
Text1.Text = BrowseForFolder("������ǿĿ¼��", Me.hWnd, False, False)
If Text1.Text = "" Then
Check1.Value = 0
Else
Check1.Value = 1
End If
End Sub

Private Sub Command2_Click()
If Check1.Value = 0 Then
DCZQML = ""
SaveSetting "�Զ�Ĭд", "TTS����", "������ǿ", DCZQML
Unload Me
Else
    If FSO.FolderExists(Text1.Text) = False Then
    MsgBox "�����ڸ��ļ��У�"
    Else
    DCZQML = Text1.Text
        If Right$(DCZQML, 1) <> "\" Then
        DCZQML = DCZQML & "\"
        End If
    SaveSetting "�Զ�Ĭд", "TTS����", "������ǿ", DCZQML
    Unload Me
    End If
End If
End Sub

Private Sub Command3_Click()
Unload Me
End Sub

Private Sub Form_Load()
If DCZQML = "" Then
Check1.Value = 0
Else
Check1.Value = 1
End If
    If Right$(DCZQML, 1) = "\" And Len(DCZQML) > 3 Then
    Text1.Text = Mid(DCZQML, 1, Len(DCZQML) - 1)
    Else
    Text1.Text = DCZQML
    End If
End Sub
