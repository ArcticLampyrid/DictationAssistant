VERSION 5.00
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"; "mscomctl.ocx"
Begin VB.Form Form1 
   AutoRedraw      =   -1  'True
   Caption         =   "�Զ�Ĭд"
   ClientHeight    =   8610
   ClientLeft      =   225
   ClientTop       =   555
   ClientWidth     =   12810
   BeginProperty Font 
      Name            =   "΢���ź�"
      Size            =   36
      Charset         =   134
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   Icon            =   "Form1.frx":0000
   LinkTopic       =   "Form1"
   ScaleHeight     =   8610
   ScaleWidth      =   12810
   StartUpPosition =   2  '��Ļ����
   Begin VB.Frame Frame3 
      Caption         =   "��������"
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1935
      Left            =   7560
      TabIndex        =   22
      Top             =   6000
      Width           =   5055
      Begin VB.CommandButton Command11 
         Caption         =   "�л���Ӣ���Ķ�����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2520
         TabIndex        =   31
         Top             =   1440
         Width           =   2415
      End
      Begin VB.CommandButton Command10 
         Caption         =   "�л��������Ķ�����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   120
         TabIndex        =   30
         Top             =   1440
         Width           =   2415
      End
      Begin VB.ComboBox Combo1 
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   720
         Style           =   2  'Dropdown List
         TabIndex        =   28
         Top             =   960
         Width           =   4215
      End
      Begin MSComctlLib.Slider Slider2 
         Height          =   255
         Left            =   720
         TabIndex        =   24
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
         TabIndex        =   26
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
         Caption         =   "���棺"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   27
         Top             =   1020
         Width           =   540
      End
      Begin VB.Label Label9 
         AutoSize        =   -1  'True
         Caption         =   "���٣�"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   25
         Top             =   600
         Width           =   540
      End
      Begin VB.Label Label11 
         AutoSize        =   -1  'True
         Caption         =   "������"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Left            =   120
         TabIndex        =   23
         Top             =   240
         Width           =   540
      End
   End
   Begin VB.Frame Frame2 
      Caption         =   "���ﲥ��"
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   5775
      Left            =   7560
      TabIndex        =   5
      Top             =   120
      Width           =   5055
      Begin VB.CommandButton Command3 
         Caption         =   "�ٱ�һ��(&A)"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2460
         TabIndex        =   18
         Top             =   4680
         Width           =   2355
      End
      Begin VB.CommandButton Command9 
         Caption         =   "����һ��(&L)"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   120
         TabIndex        =   16
         Top             =   4680
         Width           =   2355
      End
      Begin VB.CommandButton Command5 
         Caption         =   "��ͷ��ʼ(&H)"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2460
         TabIndex        =   17
         Top             =   4320
         Width           =   2355
      End
      Begin VB.CommandButton Command1 
         Caption         =   "����һ��(&N)"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   120
         TabIndex        =   15
         Top             =   4320
         Width           =   2355
      End
      Begin VB.CommandButton Command7 
         Caption         =   "��ͣ�Զ�����(&P)"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   15.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   735
         Left            =   120
         TabIndex        =   13
         Top             =   3600
         Width           =   4695
      End
      Begin VB.CommandButton Command12 
         Caption         =   "ֹͣ�Զ�����(&E)"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   2460
         TabIndex        =   43
         Top             =   3240
         Width           =   2355
      End
      Begin VB.CommandButton Command6 
         Caption         =   "��ʼ�Զ�����(&S)"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   120
         TabIndex        =   12
         Top             =   3240
         Width           =   2355
      End
      Begin VB.TextBox Text1 
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   390
         Left            =   3240
         MaxLength       =   4
         TabIndex        =   9
         Text            =   "3"
         Top             =   960
         Width           =   1575
      End
      Begin VB.TextBox Text2 
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   405
         Left            =   3240
         MaxLength       =   4
         TabIndex        =   11
         Text            =   "2"
         Top             =   1560
         Width           =   1575
      End
      Begin VB.Label Label15 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   570
         Left            =   2280
         TabIndex        =   42
         Top             =   1920
         Width           =   2055
      End
      Begin VB.Label Label14 
         Caption         =   "��"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   570
         Left            =   4320
         TabIndex        =   41
         Top             =   1920
         Width           =   435
      End
      Begin VB.Label Label13 
         Caption         =   "���ڲ�����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   570
         Left            =   120
         TabIndex        =   40
         Top             =   1920
         Width           =   2175
      End
      Begin VB.Label Label12 
         Caption         =   "��ͣ�Զ������󰴡�����һ������������һ�������ٱ�һ�顱����Զ���ʼ�Զ�������"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   10.5
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   615
         Left            =   120
         TabIndex        =   14
         Top             =   2520
         Width           =   4695
      End
      Begin VB.Label Label3 
         Caption         =   "����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   120
         TabIndex        =   19
         Top             =   5160
         Width           =   975
      End
      Begin VB.Label Label4 
         Caption         =   "����Զ�����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   2205
         TabIndex        =   21
         Top             =   5160
         Width           =   2655
      End
      Begin VB.Label Label5 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   495
         Left            =   1095
         TabIndex        =   20
         Top             =   5160
         Width           =   1095
      End
      Begin VB.Label Label1 
         Caption         =   "�����ﲥ������"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   570
         Left            =   120
         TabIndex        =   6
         Top             =   240
         Width           =   3045
      End
      Begin VB.Label Label2 
         Alignment       =   2  'Center
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   570
         Left            =   3240
         TabIndex        =   7
         Top             =   240
         Width           =   1575
      End
      Begin VB.Label Label6 
         Caption         =   "�Զ�����������룩"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   120
         TabIndex        =   8
         Top             =   960
         Width           =   1620
      End
      Begin VB.Label Label7 
         Caption         =   "�Զ�����������ÿ�����"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   120
         TabIndex        =   10
         Top             =   1560
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
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   8415
      Left            =   7080
      MaskColor       =   &H00FFFFFF&
      Picture         =   "Form1.frx":324A
      Style           =   1  'Graphical
      TabIndex        =   4
      Top             =   120
      UseMaskColor    =   -1  'True
      Width           =   375
   End
   Begin VB.Frame Frame1 
      Caption         =   "�����б�"
      BeginProperty Font 
         Name            =   "����"
         Size            =   9
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   8415
      Left            =   120
      TabIndex        =   0
      Top             =   120
      Width           =   6855
      Begin VB.CommandButton cmdSave 
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":4348
         Style           =   1  'Graphical
         TabIndex        =   39
         ToolTipText     =   "���б���浽һ��TXT�ļ��У�ÿ��������Ϊһ�У�"
         Top             =   720
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdAdd 
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":444A
         Style           =   1  'Graphical
         TabIndex        =   38
         ToolTipText     =   "����б���"
         Top             =   2160
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDelete 
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":454C
         Style           =   1  'Graphical
         TabIndex        =   37
         ToolTipText     =   "ɾ��ѡ���б���"
         Top             =   3120
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdUp 
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":464E
         Style           =   1  'Graphical
         TabIndex        =   36
         ToolTipText     =   "�����ƶ�"
         Top             =   1200
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdDown 
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":4750
         Style           =   1  'Graphical
         TabIndex        =   35
         ToolTipText     =   "�����ƶ�"
         Top             =   1680
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdClear 
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":4A92
         Style           =   1  'Graphical
         TabIndex        =   34
         ToolTipText     =   "����б�"
         Top             =   3600
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdOpen 
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         Picture         =   "Form1.frx":4C34
         Style           =   1  'Graphical
         TabIndex        =   33
         ToolTipText     =   "��һ��TXT�ļ�������б��ÿ����Ϊһ�����"
         Top             =   240
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton Command4 
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "����"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         MaskColor       =   &H00FFFFFF&
         Picture         =   "Form1.frx":4D36
         Style           =   1  'Graphical
         TabIndex        =   32
         ToolTipText     =   "�޸�ѡ���б���"
         Top             =   2640
         UseMaskColor    =   -1  'True
         Width           =   330
      End
      Begin VB.CommandButton cmdShowCount 
         Caption         =   "��"
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         TabIndex        =   2
         ToolTipText     =   "�����������"
         Top             =   4080
         Width           =   330
      End
      Begin VB.CommandButton Command8 
         Caption         =   "ѡ"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   9
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   6375
         TabIndex        =   3
         ToolTipText     =   "����ѡ��"
         Top             =   4560
         Width           =   330
      End
      Begin VB.ListBox List1 
         BeginProperty Font 
            Name            =   "΢���ź�"
            Size            =   21.75
            Charset         =   134
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   8040
         ItemData        =   "Form1.frx":4E68
         Left            =   120
         List            =   "Form1.frx":4E6A
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
      Filter          =   "�ı��ĵ�(*.txt)|*.txt"
      MaxFileSize     =   32767
   End
   Begin VB.Label Label16 
      Alignment       =   2  'Center
      AutoSize        =   -1  'True
      Caption         =   "�����б�"
      BeginProperty Font 
         Name            =   "΢���ź�"
         Size            =   27.75
         Charset         =   134
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   750
      Left            =   2295
      TabIndex        =   44
      Top             =   3240
      Visible         =   0   'False
      Width           =   2235
   End
   Begin VB.Label Label10 
      Caption         =   "��������ȼ��������������ʹ�ÿ���ѡ���е����ѡ�ʣ������ѡ��������дΪ�����������ɡ�"
      BeginProperty Font 
         Name            =   "΢���ź�"
         Size            =   9
         Charset         =   134
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   7560
      TabIndex        =   29
      Top             =   8040
      Width           =   5040
   End
   Begin VB.Menu mnuFile 
      Caption         =   "�ļ�(&F)"
      Begin VB.Menu mnuOpen 
         Caption         =   "��TXT�ļ�����Ӵ���(&O)..."
         Shortcut        =   ^O
      End
      Begin VB.Menu mnuSave 
         Caption         =   "�����ﱣ�浽TXT�ļ���(&S)..."
         Shortcut        =   ^S
      End
      Begin VB.Menu mnuFileSep 
         Caption         =   "-"
      End
      Begin VB.Menu mnuExit 
         Caption         =   "�˳�(&E)"
      End
   End
   Begin VB.Menu mnuEdit 
      Caption         =   "�༭(&E)"
      Begin VB.Menu mnuShowWordControl 
         Caption         =   "��ʾ/���ش��������"
      End
      Begin VB.Menu mnuEditSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuAdd 
         Caption         =   "��Ӵ���..."
      End
      Begin VB.Menu mnuDelete 
         Caption         =   "ɾ��ѡ�д���..."
         Shortcut        =   {DEL}
      End
      Begin VB.Menu mnuClear 
         Caption         =   "��մ����б�"
      End
      Begin VB.Menu mnuModify 
         Caption         =   "�޸�ѡ�д���..."
      End
      Begin VB.Menu mnuEditSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuUp 
         Caption         =   "�����ƶ�"
      End
      Begin VB.Menu mnuDown 
         Caption         =   "�����ƶ�"
      End
      Begin VB.Menu mnuEditSep3 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSelected 
         Caption         =   "����ѡ��..."
      End
   End
   Begin VB.Menu Menu1 
      Caption         =   "����(&T)"
      Begin VB.Menu Menu2 
         Caption         =   "���õ�����ǿĿ¼..."
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
      Begin VB.Menu mnuName_ListPopup 
         Caption         =   "��ǰѡ��*����#����"
         Enabled         =   0   'False
      End
      Begin VB.Menu mnuListPopupSep1 
         Caption         =   "-"
      End
      Begin VB.Menu mnuUp_ListPopup 
         Caption         =   "����"
      End
      Begin VB.Menu mnuDown_ListPopup 
         Caption         =   "����"
      End
      Begin VB.Menu mnuModify_ListPopup 
         Caption         =   "�޸�"
      End
      Begin VB.Menu mnuDelete_ListPopup 
         Caption         =   "ɾ��"
      End
      Begin VB.Menu mnuListPopupSep2 
         Caption         =   "-"
      End
      Begin VB.Menu mnuSpeakSelect_ListPopup 
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
Dim objVoices As ISpeechObjectTokens
Dim i As Integer
Dim intBBCS As Integer '��������
Dim intYGMS As Integer '�ѹ�����
Dim ListPopupIndex As Integer
Dim EnglishVoiceGood As Boolean, ChineseVoiceGood As Boolean

Private Sub A_EndStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
Command1.Enabled = True
Command3.Enabled = True
If Zdbb = True Then Command7.Enabled = True
Command9.Enabled = True
mnuSpeakSelect_ListPopup.Enabled = True
intBBCS = intBBCS + 1
Label2.Caption = CStr(intBBCS)
If Zdbb = True Then
    If intBBCS >= Val(Text2.Text) And i < List1.ListCount Then
            intYGMS = 0
            Label5.Caption = Text1.Text
            Timer1.Enabled = True
                If Val(Text1.Text) = 0 Then
                Timer1_Timer
                End If
    ElseIf intBBCS < Val(Text2.Text) And i <= List1.ListCount Then
        intYGMS = 0
        Label5.Caption = Text1.Text
        Timer1.Enabled = True
            If Val(Text1.Text) = 0 Then
            Timer1_Timer
            End If
    Else
    Zdbb = False
    Command6.Enabled = True
    Command12.Enabled = False
    Command7.Enabled = False
    MsgBox "�Զ���������ɣ�"
    End If
End If
End Sub


Private Sub A_StartStream(ByVal StreamNumber As Long, ByVal StreamPosition As Variant)
Command1.Enabled = False
Command3.Enabled = False
Command7.Enabled = False
Command9.Enabled = False
mnuSpeakSelect_ListPopup.Enabled = False
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
Dim Temp As Integer
Temp = List1.ListIndex
  If Temp > -1 Then
    If MsgBox("�����Ҫɾ����" & List1.List(Temp) & "����", vbQuestion + vbYesNo) = vbYes Then
      List1.RemoveItem Temp
      SetListButtons
    End If
  End If
End Sub



Private Sub cmdSave_Click()
CommonDialog1.FileName = ""
CommonDialog1.Flags = cdlOFNExplorer
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
SaveSetting "�Զ�Ĭд", "TTS����", "����", Combo1.ListIndex
End Sub

Private Sub Command1_Click()
If List1.ListCount = 0 Then
MsgBox "������Ӵ��", vbOKOnly, Me.Caption
Exit Sub
End If
If i >= List1.ListCount Then
MsgBox "�Ѿ������ˡ�"
Exit Sub
End If
intBBCS = 0
List1.ListIndex = i
If Len(DCZQML) = 0 Then
A.Speak List1.List(i), SVSFlagsAsync
Else
    Dim StreamFileName As String
    StreamFileName = List1.List(i)
    StreamFileName = Replace(StreamFileName, "\", "")
    StreamFileName = Replace(StreamFileName, "/", "")
    StreamFileName = Replace(StreamFileName, ":", "")
    StreamFileName = Replace(StreamFileName, "*", "")
    StreamFileName = Replace(StreamFileName, "?", "")
    StreamFileName = Replace(StreamFileName, Chr(34), "")
    StreamFileName = Replace(StreamFileName, "<", "")
    StreamFileName = Replace(StreamFileName, ">", "")
    StreamFileName = Replace(StreamFileName, "|", "")
    StreamFileName = DCZQML & StreamFileName & ".wav"
    If FSO.FileExists(StreamFileName) = True Then
        Dim FileStream As New SpFileStream
        Call FileStream.Open(StreamFileName)
        A.SpeakStream FileStream, SVSFlagsAsync
    Else
    A.Speak List1.List(i), SVSFlagsAsync
    End If
End If

i = i + 1
Label15.Caption = i
Label2.Caption = CStr(intBBCS)
End Sub

Private Sub cmdOpen_Click()
CommonDialog1.FileName = ""
CommonDialog1.Flags = cdlOFNExplorer Or cdlOFNAllowMultiselect
CommonDialog1.ShowOpen
If CommonDialog1.FileName <> "" Then
    If InStr(1, CommonDialog1.FileName, Chr(0)) > 0 Then
        Dim FilePath As String
        Dim FileNames() As String
        Dim x As Integer
        FilePath = Mid(CommonDialog1.FileName, 1, InStr(1, CommonDialog1.FileName, Chr(0)) - 1)
        If Right$(FilePath, 1) <> "\" Then FilePath = FilePath & "\"
        FileNames = Split(Mid(CommonDialog1.FileName, InStr(1, CommonDialog1.FileName, Chr(0)) + 1), Chr(0))
        For x = 0 To UBound(FileNames)
        OpenFile FilePath & FileNames(x)
        Next
    Else
        OpenFile CommonDialog1.FileName
End If
End If
End Sub

Private Sub cmdShowCount_Click()
MsgBox "����������" & List1.ListCount
End Sub


Private Sub Command10_Click()
If Command10.Tag = "" Then
MsgBox "���ĵ�����û�������Ķ����档"
Exit Sub
ElseIf ChineseVoiceGood = False Then
    If MsgBox("�����ĵ�����û�з��������ϸߵ������Ķ����棬" & _
    "�Ƿ��л��������ϲ�������Ķ����棿", vbYesNo) = vbNo Then
    Exit Sub
    End If
End If
Combo1.ListIndex = Val(Command10.Tag)
End Sub

Private Sub Command11_Click()
If Command11.Tag = "" Then
MsgBox "���ĵ�����û��Ӣ���Ķ����档"
Exit Sub
ElseIf EnglishVoiceGood = False Then
    If MsgBox("�����ĵ�����û�з��������ϸߵ�Ӣ���Ķ����棬" & _
    "�Ƿ��л��������ϲ��Ӣ���Ķ����棿", vbYesNo) = vbNo Then
    Exit Sub
    End If
End If
Combo1.ListIndex = Val(Command11.Tag)
End Sub

Private Sub Command12_Click()
Zdbb = False
Command6.Enabled = True
Command7.Enabled = False
Command12.Enabled = False
Timer1.Enabled = False
Label5.Caption = "0"
If A.Status.RunningState <> SRSEDone Then
A.Speak vbNullString, SVSFPurgeBeforeSpeak
Command1.Enabled = True
Command3.Enabled = True
Command9.Enabled = True
mnuSpeakSelect_ListPopup.Enabled = True
End If
MsgBox "�Զ������ѽ�����"
End Sub

Private Sub Command2_Click()
If Frame1.Visible Then
Frame1.Visible = False
Label16.Visible = True
Else
Frame1.Visible = True
Label16.Visible = False
End If
End Sub

Private Sub Command3_Click()
If (i <= List1.ListCount) And (i > 0) Then
List1.ListIndex = i - 1
    If Len(DCZQML) = 0 Then
    A.Speak List1.List(i - 1), SVSFlagsAsync
    Else
        Dim StreamFileName As String
        StreamFileName = List1.List(i - 1)
        StreamFileName = Replace(StreamFileName, "\", "")
        StreamFileName = Replace(StreamFileName, "/", "")
        StreamFileName = Replace(StreamFileName, ":", "")
        StreamFileName = Replace(StreamFileName, "*", "")
        StreamFileName = Replace(StreamFileName, "?", "")
        StreamFileName = Replace(StreamFileName, Chr(34), "")
        StreamFileName = Replace(StreamFileName, "<", "")
        StreamFileName = Replace(StreamFileName, ">", "")
        StreamFileName = Replace(StreamFileName, "|", "")
        StreamFileName = DCZQML & StreamFileName & ".wav"
        If FSO.FileExists(StreamFileName) = True Then
            Dim FileStream As New SpFileStream
            Call FileStream.Open(StreamFileName)
            A.SpeakStream FileStream, SVSFlagsAsync
        Else
        A.Speak List1.List(i - 1), SVSFlagsAsync
        End If
    End If
Else
MsgBox "��û�����ˣ�"
End If
End Sub

Private Sub cmdClear_Click()
List1.Clear
SetListButtons
End Sub

Private Sub Command4_Click()
Dim Temp As Integer
Temp = List1.ListIndex
  If Temp > -1 Then
    Dim strNewText As String
    strNewText = InputBox("��׼������" & List1.List(Temp) & "���޸ĳɣ�", , List1.Text)
    If Len(strNewText) > 0 Then
      List1.List(Temp) = strNewText
      SetListButtons
    End If
  End If
End Sub

Private Sub Command5_Click()
i = 0
If List1.ListCount > 0 Then List1.ListIndex = 0
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
Command6.Enabled = False
Command12.Enabled = True
Label5.Caption = Text1.Text
intYGMS = 0
Timer1.Enabled = True
Zdbb = True
i = 0
Command1_Click
End Sub

Private Sub Command7_Click()
Command7.Enabled = False
Timer1.Enabled = False
Command3.SetFocus
End Sub

Private Sub Command8_Click()
If List1.ListCount > 0 Then Form2.Show 1
End Sub

Private Sub Command9_Click()
If i <= List1.ListCount And i > 1 Then
i = i - 2
Command1_Click
Else
MsgBox "�Ѿ��ǵ�1���ˣ�"
End If
End Sub

Private Sub Form_Load()
Label16.Caption = "�����б�" & vbNewLine & "������"
Dim EnglishGrade As Integer, ChineseGrade As Integer '�������ڼ�¼�������������ȼ��ı���
Dim objVoice As SpObjectToken
Dim strVoiceLanguage As String
Dim strVoiceName As String
EnglishGrade = 0 '��ʼ�����ڼ�¼Ӣ���������������ȼ��ı���
ChineseGrade = 0 '��ʼ�����ڼ�¼�����������������ȼ��ı���
Set A = New SpVoice '����SpVoice����ʵ��
Set objVoices = A.GetVoices '��ȡ�������Ｏ��
    If objVoices.Count = 0 Then
    MsgBox "δ����TTS��������", vbOKOnly, "Error"
    End
    End If
For i = 0 To objVoices.Count - 1 '�����������Ｏ��
Set objVoice = objVoices.Item(i) '��ȡ�������Ｏ������Ӧ��������������
strVoiceName = objVoice.GetDescription '��ȡ������������
Combo1.AddItem strVoiceName '����Ͽ������
    strVoiceLanguage = objVoice.GetAttribute("Language") '��ȡ������������
    If strVoiceLanguage = "409" Or strVoiceLanguage Like "*;409" Or strVoiceLanguage Like "409;*" Or strVoiceLanguage Like "*;409;*" Then '��������������ΪӢ��ʱ
        If (EnglishGrade = 0) Or (EnglishGrade > GetEnglishGrade(strVoiceName) And GetEnglishGrade(strVoiceName) <> 0) Then
        Command11.Tag = i
        EnglishGrade = GetEnglishGrade(strVoiceName)
        End If
    ElseIf strVoiceLanguage = "804" Or strVoiceLanguage Like "*;804" Or strVoiceLanguage Like "804;*" Or strVoiceLanguage Like "*;804;*" Then '��������������Ϊ����ʱ
        If (ChineseGrade = 0) Or (ChineseGrade > GetChineseGrade(strVoiceName) And GetChineseGrade(strVoiceName) <> 0) Then
        Command10.Tag = i
        ChineseGrade = GetChineseGrade(strVoiceName)
        End If
    End If
Next
EnglishVoiceGood = EnglishGrade > 0 And EnglishGrade < 5
ChineseVoiceGood = ChineseGrade
Combo1.ListIndex = GetSetting("�Զ�Ĭд", "TTS����", "����", 0)
Slider1.Value = GetSetting("�Զ�Ĭд", "TTS����", "����", "0")
Slider2.Value = GetSetting("�Զ�Ĭд", "TTS����", "����", "100")
i = 0
intBBCS = 0
'֧���ļ�������ʽ�򿪣�
Dim FileName As String
FileName = Command '��ȡ���в���
If Len(FileName) > 0 Then '���в��������ڿ�ʱ
    If Left$(FileName, 1) = Chr(34) Then '��һ���ַ����ڡ�"��ʱ
    FileName = Mid(FileName, 2, Len(FileName) - 2) 'ȡ��2���ַ��������һ���ַ�-1�����ַ���
    End If
    If FSO.FileExists(FileName) = True Then '��������ļ�ʱ
    OpenFile FileName '������ļ�
    End If
End If
DCZQML = GetSetting("�Զ�Ĭд", "TTS����", "������ǿ", "")
End Sub

Private Sub Form_Resize()
On Error Resume Next
If Me.Width < 8430 Then
Me.Width = 8430
End If
If Me.Height < 9480 Then
Me.Height = 9480
End If
Frame1.Width = Me.Width - 6150
Command2.Left = Frame1.Width + Frame1.Left + 105
Frame2.Left = Command2.Width + Command2.Left + 105
Frame3.Left = Command2.Width + Command2.Left + 105
Label10.Left = Command2.Width + Command2.Left + 105
List1.Width = Frame1.Width - 600
cmdOpen.Left = List1.Width + List1.Left
cmdSave.Left = List1.Width + List1.Left
cmdUp.Left = List1.Width + List1.Left
cmdDown.Left = List1.Width + List1.Left
cmdAdd.Left = List1.Width + List1.Left
Command4.Left = List1.Width + List1.Left
cmdDelete.Left = List1.Width + List1.Left
cmdClear.Left = List1.Width + List1.Left
cmdShowCount.Left = List1.Width + List1.Left
Command8.Left = List1.Width + List1.Left
Frame1.Height = Me.Height - 1065
List1.Height = Frame1.Height - 375
Label10.Top = Me.Height - 1440
Frame3.Top = Label10.Top - 105 - Frame3.Height
Frame2.Height = Me.Height - 3585 - Frame2.Top
Label3.Top = Frame2.Height - 615
Label5.Top = Frame2.Height - 615
Label4.Top = Frame2.Height - 615
Command2.Height = Frame1.Height
Dim Temp1 As Integer
Dim Temp2 As Integer
Temp1 = Frame2.Height - 3960
Temp2 = Screen.TwipsPerPixelY
Temp1 = Temp1 + Temp2 * 5
Temp1 = Temp1 / 5
Temp1 = Temp1 \ Temp2
Temp1 = Temp1 * Temp2
Command6.Height = Temp1
Command12.Height = Temp1
Command7.Top = Command12.Top + Temp1 - Temp2
Command7.Height = Temp1 + Temp1
Command1.Top = Command7.Top + Temp1 + Temp1 - Temp2
Command1.Height = Temp1
Command5.Top = Command7.Top + Temp1 + Temp1 - Temp2
Command5.Height = Temp1
Command9.Top = Command1.Top + Temp1 - Temp2
Command9.Height = Temp1
Command3.Top = Command1.Top + Temp1 - Temp2
Command3.Height = Temp1
Label16.Left = Frame1.Left + ((Frame1.Width - Label16.Width) / 2)
Label16.Top = Frame1.Top + ((Frame1.Height - Label16.Height) / 3)
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

Private Sub List1_MouseDown(Button As Integer, Shift As Integer, x As Single, Y As Single)
If Button = 2 Then
    Dim pos As Long, idx As Long
    pos = x / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(List1.hWnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    List1.ListIndex = idx
    ListPopupIndex = idx
    mnuName_ListPopup.Caption = "��ǰѡ��" & List1.List(ListPopupIndex) & "����" & ListPopupIndex + 1 & "����"
    PopupMenu mnuListPopup
    Else
    List1.ListIndex = -1
    End If
End If
End Sub

Private Sub List1_MouseMove(Button As Integer, Shift As Integer, x As Single, Y As Single)
    Dim pos As Long, idx As Long
    pos = x / Screen.TwipsPerPixelX + Y / Screen.TwipsPerPixelY * 65536
    idx = SendMessage(List1.hWnd, LB_ITEMFROMPOINT, 0, ByVal pos)
    If idx < 65536 Then
    List1.ToolTipText = List1.List(idx)
    Else
    List1.ToolTipText = ""
    End If
End Sub

Private Sub Menu2_Click()
Form3.Show 1
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
List1.ListIndex = ListPopupIndex
mnuDelete_Click
End Sub

Private Sub mnuDown_Click()
If List1.Visible = False Then Exit Sub
cmdDown_Click
End Sub

Private Sub mnuDown_ListPopup_Click()
List1.ListIndex = ListPopupIndex
mnuDown_Click
End Sub

Private Sub mnuExit_Click()
End
End Sub

Private Sub mnuModify_Click()
If List1.Visible = False Then Exit Sub
Command4_Click
End Sub

Private Sub mnuModify_ListPopup_Click()
List1.ListIndex = ListPopupIndex
mnuModify_Click
End Sub

Private Sub mnuOpen_Click()
If List1.Visible = False Then Exit Sub
Call cmdOpen_Click
End Sub

Private Sub mnuSave_Click()
If List1.Visible = False Then Exit Sub
Call cmdSave_Click
End Sub


Private Sub mnuSelected_Click()
Command8_Click
End Sub

Private Sub mnuSpeakSelect_ListPopup_Click()
List1.ListIndex = ListPopupIndex
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
List1.ListIndex = ListPopupIndex
mnuUp_Click
End Sub

Private Sub Slider1_Change()
A.Rate = Slider1.Value
SaveSetting "�Զ�Ĭд", "TTS����", "����", Slider1.Value
End Sub


Private Sub Slider2_Change()
A.Volume = Slider2.Value
SaveSetting "�Զ�Ĭд", "TTS����", "����", Slider2.Value
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
        If i < List1.ListCount Then
        Command1_Click
        Else
        Zdbb = False
        Command6.Enabled = True
        Command12.Enabled = False
        Command7.Enabled = False
        Timer1.Enabled = False
        MsgBox "�Զ���������ɣ�"
        End If
    Else
        If i <= List1.ListCount Then
        Command3_Click
        Else
        Zdbb = False
        Command6.Enabled = True
        Command12.Enabled = False
        Command7.Enabled = False
        Timer1.Enabled = False
        MsgBox "�Զ���������ɣ�"
        End If
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
