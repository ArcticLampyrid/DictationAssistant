Attribute VB_Name = "Module1"
Public Declare Function SendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
Public Const LB_ITEMFROMPOINT = &H1A9
Public FSO As New FileSystemObject
Public DCZQML As String '单词增强目录
Private Declare Function SHBrowseForFolder Lib "shell32" (lpbi As BrowseInfo) As Long
Private Declare Sub CoTaskMemFree Lib "ole32.dll" (ByVal hMem As Long)
Private Declare Function SHGetPathFromIDList Lib "Shell32.dll" Alias "SHGetPathFromIDListA" (ByVal pidl As Long, ByVal pszPath As String) As Long
Private Type BrowseInfo
    hwndOwner As Long
    pIDLRoot As Long
    pszDisplayName As String
    lpszTitle As String
    ulFlags As Long
    lpfnCallback As Long
    lParam As Long
    iImage As Long
End Type
Private Const BIF_RETURNONLYFSDIRS          As Long = 1
Private Const BIF_DONTGOBELOWDOMAIN         As Long = 2
Private Const BIF_EDITBOX = &H10
Private Const BIF_USENEWUI = &H40

Public Function BrowseForFolder(ByVal Title As String, hWnd As Long, EditBox As Boolean, NewButton As Boolean) As String
Dim lIDList As Long
Dim sBuffer As String
Dim tBrowseInfo As BrowseInfo

    With tBrowseInfo
        .hwndOwner = hWnd
        .lpszTitle = Title
        .ulFlags = BIF_RETURNONLYFSDIRS Or BIF_DONTGOBELOWDOMAIN
        If EditBox Then .ulFlags = .ulFlags Or BIF_EDITBOX
        If NewButton Then .ulFlags = .ulFlags Or BIF_USENEWUI
    End With
    
    lIDList = SHBrowseForFolder(tBrowseInfo)
    
    If lIDList Then
        sBuffer = String$(260, vbNullChar)
        SHGetPathFromIDList lIDList, sBuffer
        BrowseForFolder = Mid$(sBuffer, 1, InStr(sBuffer, vbNullChar) - 1)
        CoTaskMemFree lIDList
    End If
End Function



