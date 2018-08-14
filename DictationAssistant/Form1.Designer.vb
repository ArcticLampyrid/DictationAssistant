<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form 重写 Dispose，以清理组件列表。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows 窗体设计器所必需的
    Private components As System.ComponentModel.IContainer

    '注意: 以下过程是 Windows 窗体设计器所必需的
    '可以使用 Windows 窗体设计器修改它。
    '不要使用代码编辑器修改它。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.CountButton = New System.Windows.Forms.Button()
        Me.ClearButton = New System.Windows.Forms.Button()
        Me.DeleteButton = New System.Windows.Forms.Button()
        Me.EditButton = New System.Windows.Forms.Button()
        Me.AddButton = New System.Windows.Forms.Button()
        Me.DownButton = New System.Windows.Forms.Button()
        Me.UpButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.OpenButton = New System.Windows.Forms.Button()
        Me.WordListView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.CiYuListViewContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.读选定词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.在Bing词典中查看ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.从此处开始自动播报ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripSeparator()
        Me.插入ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.修改ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.移除ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem5 = New System.Windows.Forms.ToolStripSeparator()
        Me.上移ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.下移ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem6 = New System.Windows.Forms.ToolStripSeparator()
        Me.全选ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.反向选择ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.保存SToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.退出EToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.编辑EToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.显示隐藏词语列表ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem2 = New System.Windows.Forms.ToolStripSeparator()
        Me.添加词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.修改选中词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.移除选中词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.清空词语列表ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem3 = New System.Windows.Forms.ToolStripSeparator()
        Me.向上移动ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.向下移动ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem4 = New System.Windows.Forms.ToolStripSeparator()
        Me.全选ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.反向选择ToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.选项ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.字体FToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem8 = New System.Windows.Forms.ToolStripSeparator()
        Me.自动翻页ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.字词跟随ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem7 = New System.Windows.Forms.ToolStripSeparator()
        Me.设置词组增强ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.辅助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.随机排序ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.随机选词ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.保存音频ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.帮助HToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.关于AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ZiDongBoBaoJianGeTextBox = New System.Windows.Forms.TextBox()
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.插入LengthToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.插入WordCountToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ZiDongBoBaoCiShuTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button9 = New System.Windows.Forms.Button()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.Button11 = New System.Windows.Forms.Button()
        Me.Button10 = New System.Windows.Forms.Button()
        Me.ComboBox1 = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.TrackBar2 = New System.Windows.Forms.TrackBar()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.TrackBar1 = New System.Windows.Forms.TrackBar()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.FontDialog1 = New System.Windows.Forms.FontDialog()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.CiYuListViewContextMenuStrip.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.CountButton)
        Me.GroupBox1.Controls.Add(Me.ClearButton)
        Me.GroupBox1.Controls.Add(Me.DeleteButton)
        Me.GroupBox1.Controls.Add(Me.EditButton)
        Me.GroupBox1.Controls.Add(Me.AddButton)
        Me.GroupBox1.Controls.Add(Me.DownButton)
        Me.GroupBox1.Controls.Add(Me.UpButton)
        Me.GroupBox1.Controls.Add(Me.SaveButton)
        Me.GroupBox1.Controls.Add(Me.OpenButton)
        Me.GroupBox1.Controls.Add(Me.WordListView)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 27)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(426, 658)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "词语列表"
        '
        'CountButton
        '
        Me.CountButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CountButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.CountButton.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.CountButton.Image = CType(resources.GetObject("CountButton.Image"), System.Drawing.Image)
        Me.CountButton.Location = New System.Drawing.Point(388, 324)
        Me.CountButton.Name = "CountButton"
        Me.CountButton.Size = New System.Drawing.Size(32, 32)
        Me.CountButton.TabIndex = 9
        Me.CountButton.UseCompatibleTextRendering = True
        '
        'ClearButton
        '
        Me.ClearButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ClearButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClearButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.ClearButton.Image = CType(resources.GetObject("ClearButton.Image"), System.Drawing.Image)
        Me.ClearButton.Location = New System.Drawing.Point(388, 286)
        Me.ClearButton.Name = "ClearButton"
        Me.ClearButton.Size = New System.Drawing.Size(32, 32)
        Me.ClearButton.TabIndex = 8
        '
        'DeleteButton
        '
        Me.DeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.DeleteButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.DeleteButton.Image = CType(resources.GetObject("DeleteButton.Image"), System.Drawing.Image)
        Me.DeleteButton.Location = New System.Drawing.Point(388, 248)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(32, 32)
        Me.DeleteButton.TabIndex = 7
        '
        'EditButton
        '
        Me.EditButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.EditButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.EditButton.Image = CType(resources.GetObject("EditButton.Image"), System.Drawing.Image)
        Me.EditButton.Location = New System.Drawing.Point(388, 210)
        Me.EditButton.Name = "EditButton"
        Me.EditButton.Size = New System.Drawing.Size(32, 32)
        Me.EditButton.TabIndex = 6
        '
        'AddButton
        '
        Me.AddButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.AddButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.AddButton.Image = CType(resources.GetObject("AddButton.Image"), System.Drawing.Image)
        Me.AddButton.Location = New System.Drawing.Point(388, 172)
        Me.AddButton.Name = "AddButton"
        Me.AddButton.Size = New System.Drawing.Size(32, 32)
        Me.AddButton.TabIndex = 5
        '
        'DownButton
        '
        Me.DownButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.DownButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.DownButton.Image = CType(resources.GetObject("DownButton.Image"), System.Drawing.Image)
        Me.DownButton.Location = New System.Drawing.Point(388, 134)
        Me.DownButton.Name = "DownButton"
        Me.DownButton.Size = New System.Drawing.Size(32, 32)
        Me.DownButton.TabIndex = 4
        '
        'UpButton
        '
        Me.UpButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.UpButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.UpButton.Image = CType(resources.GetObject("UpButton.Image"), System.Drawing.Image)
        Me.UpButton.Location = New System.Drawing.Point(388, 96)
        Me.UpButton.Name = "UpButton"
        Me.UpButton.Size = New System.Drawing.Size(32, 32)
        Me.UpButton.TabIndex = 3
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.SaveButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.SaveButton.Image = CType(resources.GetObject("SaveButton.Image"), System.Drawing.Image)
        Me.SaveButton.Location = New System.Drawing.Point(388, 58)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(32, 32)
        Me.SaveButton.TabIndex = 2
        '
        'OpenButton
        '
        Me.OpenButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OpenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.OpenButton.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.OpenButton.Image = CType(resources.GetObject("OpenButton.Image"), System.Drawing.Image)
        Me.OpenButton.Location = New System.Drawing.Point(388, 20)
        Me.OpenButton.Name = "OpenButton"
        Me.OpenButton.Size = New System.Drawing.Size(32, 32)
        Me.OpenButton.TabIndex = 1
        '
        'CiYuListView
        '
        Me.WordListView.AllowColumnReorder = True
        Me.WordListView.AllowDrop = True
        Me.WordListView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.WordListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.WordListView.ContextMenuStrip = Me.CiYuListViewContextMenuStrip
        Me.WordListView.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.WordListView.FullRowSelect = True
        Me.WordListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
        Me.WordListView.HideSelection = False
        Me.WordListView.LabelEdit = True
        Me.WordListView.Location = New System.Drawing.Point(6, 20)
        Me.WordListView.Name = "CiYuListView"
        Me.WordListView.Size = New System.Drawing.Size(376, 632)
        Me.WordListView.TabIndex = 0
        Me.WordListView.UseCompatibleStateImageBehavior = False
        Me.WordListView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "词语"
        Me.ColumnHeader1.Width = 300
        '
        'CiYuListViewContextMenuStrip
        '
        Me.CiYuListViewContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.读选定词语ToolStripMenuItem, Me.在Bing词典中查看ToolStripMenuItem, Me.从此处开始自动播报ToolStripMenuItem, Me.ToolStripMenuItem9, Me.插入ToolStripMenuItem, Me.修改ToolStripMenuItem, Me.移除ToolStripMenuItem, Me.ToolStripMenuItem5, Me.上移ToolStripMenuItem, Me.下移ToolStripMenuItem, Me.ToolStripMenuItem6, Me.全选ToolStripMenuItem1, Me.反向选择ToolStripMenuItem})
        Me.CiYuListViewContextMenuStrip.Name = "ContextMenuStrip1"
        Me.CiYuListViewContextMenuStrip.Size = New System.Drawing.Size(185, 242)
        '
        '读选定词语ToolStripMenuItem
        '
        Me.读选定词语ToolStripMenuItem.Name = "读选定词语ToolStripMenuItem"
        Me.读选定词语ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.读选定词语ToolStripMenuItem.Text = "读选定词语"
        '
        '在Bing词典中查看ToolStripMenuItem
        '
        Me.在Bing词典中查看ToolStripMenuItem.Name = "在Bing词典中查看ToolStripMenuItem"
        Me.在Bing词典中查看ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.在Bing词典中查看ToolStripMenuItem.Text = "在Bing词典中查看"
        '
        '从此处开始自动播报ToolStripMenuItem
        '
        Me.从此处开始自动播报ToolStripMenuItem.Name = "从此处开始自动播报ToolStripMenuItem"
        Me.从此处开始自动播报ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.从此处开始自动播报ToolStripMenuItem.Text = "从此处开始自动播报"
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(181, 6)
        '
        '插入ToolStripMenuItem
        '
        Me.插入ToolStripMenuItem.Name = "插入ToolStripMenuItem"
        Me.插入ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.插入ToolStripMenuItem.Text = "插入"
        '
        '修改ToolStripMenuItem
        '
        Me.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem"
        Me.修改ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2
        Me.修改ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.修改ToolStripMenuItem.Text = "修改(&M)"
        '
        '移除ToolStripMenuItem
        '
        Me.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem"
        Me.移除ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.移除ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.移除ToolStripMenuItem.Text = "移除(&R)"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(181, 6)
        '
        '上移ToolStripMenuItem
        '
        Me.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem"
        Me.上移ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.上移ToolStripMenuItem.Text = "上移"
        '
        '下移ToolStripMenuItem
        '
        Me.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem"
        Me.下移ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.下移ToolStripMenuItem.Text = "下移"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(181, 6)
        '
        '全选ToolStripMenuItem1
        '
        Me.全选ToolStripMenuItem1.Name = "全选ToolStripMenuItem1"
        Me.全选ToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.全选ToolStripMenuItem1.Size = New System.Drawing.Size(184, 22)
        Me.全选ToolStripMenuItem1.Text = "全选(&A)"
        '
        '反向选择ToolStripMenuItem
        '
        Me.反向选择ToolStripMenuItem.Name = "反向选择ToolStripMenuItem"
        Me.反向选择ToolStripMenuItem.Size = New System.Drawing.Size(184, 22)
        Me.反向选择ToolStripMenuItem.Text = "反向选择(&I)"
        '
        'Label5
        '
        Me.Label5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label5.Font = New System.Drawing.Font("微软雅黑", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label5.Location = New System.Drawing.Point(6, 199)
        Me.Label5.MinimumSize = New System.Drawing.Size(146, 82)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(414, 207)
        Me.Label5.TabIndex = 11
        Me.Label5.Text = "词语列表" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & " 已隐藏 "
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.Label5.Visible = False
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Control
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.编辑EToolStripMenuItem, Me.选项ToolStripMenuItem, Me.辅助ToolStripMenuItem, Me.帮助HToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(803, 25)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "菜单栏"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.保存SToolStripMenuItem, Me.ToolStripMenuItem1, Me.退出EToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(58, 21)
        Me.FileToolStripMenuItem.Text = "文件(&F)"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.OpenToolStripMenuItem.Text = "打开(&O)..."
        Me.OpenToolStripMenuItem.ToolTipText = "从一个TXT文件中批量添加词语到词语列表（每行作为一个词语）"
        '
        '保存SToolStripMenuItem
        '
        Me.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem"
        Me.保存SToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.保存SToolStripMenuItem.Text = "保存(&S)..."
        Me.保存SToolStripMenuItem.ToolTipText = "将词语列表保存到一个TXT文件中（每个词语作为一行）"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(124, 6)
        '
        '退出EToolStripMenuItem
        '
        Me.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem"
        Me.退出EToolStripMenuItem.Size = New System.Drawing.Size(127, 22)
        Me.退出EToolStripMenuItem.Text = "退出(&E)"
        Me.退出EToolStripMenuItem.ToolTipText = "退出自动默写程序"
        '
        '编辑EToolStripMenuItem
        '
        Me.编辑EToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.显示隐藏词语列表ToolStripMenuItem, Me.ToolStripMenuItem2, Me.添加词语ToolStripMenuItem, Me.修改选中词语ToolStripMenuItem, Me.移除选中词语ToolStripMenuItem, Me.清空词语列表ToolStripMenuItem, Me.ToolStripMenuItem3, Me.向上移动ToolStripMenuItem, Me.向下移动ToolStripMenuItem, Me.ToolStripMenuItem4, Me.全选ToolStripMenuItem, Me.反向选择ToolStripMenuItem1})
        Me.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem"
        Me.编辑EToolStripMenuItem.Size = New System.Drawing.Size(59, 21)
        Me.编辑EToolStripMenuItem.Text = "编辑(&E)"
        '
        '显示隐藏词语列表ToolStripMenuItem
        '
        Me.显示隐藏词语列表ToolStripMenuItem.Name = "显示隐藏词语列表ToolStripMenuItem"
        Me.显示隐藏词语列表ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.显示隐藏词语列表ToolStripMenuItem.Text = "显示/隐藏词语控制区(&L)"
        Me.显示隐藏词语列表ToolStripMenuItem.ToolTipText = "显示或隐藏词语列表"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(200, 6)
        '
        '添加词语ToolStripMenuItem
        '
        Me.添加词语ToolStripMenuItem.Name = "添加词语ToolStripMenuItem"
        Me.添加词语ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.添加词语ToolStripMenuItem.Text = "添加词语(&A)..."
        Me.添加词语ToolStripMenuItem.ToolTipText = "添加一个词语"
        '
        '修改选中词语ToolStripMenuItem
        '
        Me.修改选中词语ToolStripMenuItem.Name = "修改选中词语ToolStripMenuItem"
        Me.修改选中词语ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.修改选中词语ToolStripMenuItem.Text = "修改选中词语(&M)"
        Me.修改选中词语ToolStripMenuItem.ToolTipText = "修改选中的词语"
        '
        '移除选中词语ToolStripMenuItem
        '
        Me.移除选中词语ToolStripMenuItem.Name = "移除选中词语ToolStripMenuItem"
        Me.移除选中词语ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.移除选中词语ToolStripMenuItem.Text = "移除选中词语(&R)"
        Me.移除选中词语ToolStripMenuItem.ToolTipText = "选中选中的词语"
        '
        '清空词语列表ToolStripMenuItem
        '
        Me.清空词语列表ToolStripMenuItem.Name = "清空词语列表ToolStripMenuItem"
        Me.清空词语列表ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.清空词语列表ToolStripMenuItem.Text = "清空词语列表(&C)"
        Me.清空词语列表ToolStripMenuItem.ToolTipText = "清空词语列表"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(200, 6)
        '
        '向上移动ToolStripMenuItem
        '
        Me.向上移动ToolStripMenuItem.Name = "向上移动ToolStripMenuItem"
        Me.向上移动ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.向上移动ToolStripMenuItem.Text = "向上移动(&U)"
        Me.向上移动ToolStripMenuItem.ToolTipText = "向上移动选中词语"
        '
        '向下移动ToolStripMenuItem
        '
        Me.向下移动ToolStripMenuItem.Name = "向下移动ToolStripMenuItem"
        Me.向下移动ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.向下移动ToolStripMenuItem.Text = "向下移动(&D)"
        Me.向下移动ToolStripMenuItem.ToolTipText = "向下移动选中词语"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(200, 6)
        '
        '全选ToolStripMenuItem
        '
        Me.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem"
        Me.全选ToolStripMenuItem.Size = New System.Drawing.Size(203, 22)
        Me.全选ToolStripMenuItem.Text = "全选(&A)"
        Me.全选ToolStripMenuItem.ToolTipText = "选择全部词语"
        '
        '反向选择ToolStripMenuItem1
        '
        Me.反向选择ToolStripMenuItem1.Name = "反向选择ToolStripMenuItem1"
        Me.反向选择ToolStripMenuItem1.Size = New System.Drawing.Size(203, 22)
        Me.反向选择ToolStripMenuItem1.Text = "反向选择(&I)"
        Me.反向选择ToolStripMenuItem1.ToolTipText = "选择没有选中的词语"
        '
        '选项ToolStripMenuItem
        '
        Me.选项ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.字体FToolStripMenuItem, Me.ToolStripMenuItem8, Me.自动翻页ToolStripMenuItem, Me.字词跟随ToolStripMenuItem, Me.ToolStripMenuItem7, Me.设置词组增强ToolStripMenuItem})
        Me.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem"
        Me.选项ToolStripMenuItem.Size = New System.Drawing.Size(62, 21)
        Me.选项ToolStripMenuItem.Text = "选项(&O)"
        '
        '字体FToolStripMenuItem
        '
        Me.字体FToolStripMenuItem.Name = "字体FToolStripMenuItem"
        Me.字体FToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.字体FToolStripMenuItem.Text = "字体(&F)"
        Me.字体FToolStripMenuItem.ToolTipText = "设置词语列表的字体"
        '
        'ToolStripMenuItem8
        '
        Me.ToolStripMenuItem8.Name = "ToolStripMenuItem8"
        Me.ToolStripMenuItem8.Size = New System.Drawing.Size(145, 6)
        '
        '自动翻页ToolStripMenuItem
        '
        Me.自动翻页ToolStripMenuItem.Checked = True
        Me.自动翻页ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.自动翻页ToolStripMenuItem.Name = "自动翻页ToolStripMenuItem"
        Me.自动翻页ToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.自动翻页ToolStripMenuItem.Text = "自动翻页"
        Me.自动翻页ToolStripMenuItem.ToolTipText = "自动滚动到正在播报的词语的位置"
        '
        '字词跟随ToolStripMenuItem
        '
        Me.字词跟随ToolStripMenuItem.Checked = True
        Me.字词跟随ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked
        Me.字词跟随ToolStripMenuItem.Name = "字词跟随ToolStripMenuItem"
        Me.字词跟随ToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.字词跟随ToolStripMenuItem.Text = "字词跟随"
        Me.字词跟随ToolStripMenuItem.ToolTipText = "自动选中正在播报的词语"
        '
        'ToolStripMenuItem7
        '
        Me.ToolStripMenuItem7.Name = "ToolStripMenuItem7"
        Me.ToolStripMenuItem7.Size = New System.Drawing.Size(145, 6)
        '
        '设置词组增强ToolStripMenuItem
        '
        Me.设置词组增强ToolStripMenuItem.Name = "设置词组增强ToolStripMenuItem"
        Me.设置词组增强ToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.设置词组增强ToolStripMenuItem.Text = "设置词组增强"
        Me.设置词组增强ToolStripMenuItem.ToolTipText = "通过音频文件增强语音（文件名与词组相同，查找时忽略不可作为文件名的字符）"
        '
        '辅助ToolStripMenuItem
        '
        Me.辅助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.随机排序ToolStripMenuItem, Me.随机选词ToolStripMenuItem, Me.保存音频ToolStripMenuItem})
        Me.辅助ToolStripMenuItem.Name = "辅助ToolStripMenuItem"
        Me.辅助ToolStripMenuItem.Size = New System.Drawing.Size(60, 21)
        Me.辅助ToolStripMenuItem.Text = "辅助(&A)"
        '
        '随机排序ToolStripMenuItem
        '
        Me.随机排序ToolStripMenuItem.Name = "随机排序ToolStripMenuItem"
        Me.随机排序ToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.随机排序ToolStripMenuItem.Text = "随机排序"
        Me.随机排序ToolStripMenuItem.ToolTipText = "打乱现有词语的顺序"
        '
        '随机选词ToolStripMenuItem
        '
        Me.随机选词ToolStripMenuItem.Name = "随机选词ToolStripMenuItem"
        Me.随机选词ToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.随机选词ToolStripMenuItem.Text = "随机选词"
        Me.随机选词ToolStripMenuItem.ToolTipText = "随机保留指定数量的词语"
        '
        '保存音频ToolStripMenuItem
        '
        Me.保存音频ToolStripMenuItem.Name = "保存音频ToolStripMenuItem"
        Me.保存音频ToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.保存音频ToolStripMenuItem.Text = "保存音频"
        '
        '帮助HToolStripMenuItem
        '
        Me.帮助HToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.关于AToolStripMenuItem})
        Me.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem"
        Me.帮助HToolStripMenuItem.Size = New System.Drawing.Size(61, 21)
        Me.帮助HToolStripMenuItem.Text = "帮助(&H)"
        '
        '关于AToolStripMenuItem
        '
        Me.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem"
        Me.关于AToolStripMenuItem.Size = New System.Drawing.Size(116, 22)
        Me.关于AToolStripMenuItem.Text = "关于(&A)"
        Me.关于AToolStripMenuItem.ToolTipText = "显示""关于""对话框"
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.DefaultExt = "txt"
        Me.OpenFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*"
        Me.OpenFileDialog1.Multiselect = True
        Me.OpenFileDialog1.Title = "打开词语列表文件"
        '
        'SaveFileDialog1
        '
        Me.SaveFileDialog1.DefaultExt = "txt"
        Me.SaveFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*"
        Me.SaveFileDialog1.Title = "保存为词语列表文件"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.ZiDongBoBaoJianGeTextBox)
        Me.GroupBox2.Controls.Add(Me.ZiDongBoBaoCiShuTextBox)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.GroupBox2.Location = New System.Drawing.Point(444, 27)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(350, 193)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "播报信息"
        '
        'ZiDongBoBaoJianGeTextBox
        '
        Me.ZiDongBoBaoJianGeTextBox.ContextMenuStrip = Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip
        Me.ZiDongBoBaoJianGeTextBox.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.ErrorProvider1.SetIconAlignment(Me.ZiDongBoBaoJianGeTextBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft)
        Me.ZiDongBoBaoJianGeTextBox.Location = New System.Drawing.Point(253, 68)
        Me.ZiDongBoBaoJianGeTextBox.Name = "ZiDongBoBaoJianGeTextBox"
        Me.ZiDongBoBaoJianGeTextBox.Size = New System.Drawing.Size(86, 29)
        Me.ZiDongBoBaoJianGeTextBox.TabIndex = 2
        Me.ZiDongBoBaoJianGeTextBox.Text = "3"
        '
        'ZiDongBoBaoJianGeTextBoxContextMenuStrip
        '
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem, Me.ToolStripSeparator1, Me.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem, Me.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem, Me.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem, Me.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem, Me.ToolStripSeparator2, Me.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem, Me.ToolStripSeparator3, Me.插入LengthToolStripMenuItem, Me.插入WordCountToolStripMenuItem})
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Name = "ZiDongBoBaoJianGeTextBoxContextMenuStrip"
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Size = New System.Drawing.Size(200, 198)
        '
        'ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Text = "撤销(&U)"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(196, 6)
        '
        'ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Text = "剪切(&T)"
        '
        'ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Text = "复制(&C)"
        '
        'ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Text = "粘贴(&P)"
        '
        'ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Text = "删除(&D)"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(196, 6)
        '
        'ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem
        '
        Me.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem"
        Me.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Text = "全选(&A)"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(196, 6)
        '
        '插入LengthToolStripMenuItem
        '
        Me.插入LengthToolStripMenuItem.Name = "插入LengthToolStripMenuItem"
        Me.插入LengthToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.插入LengthToolStripMenuItem.Text = "插入[Length] (&L)"
        '
        '插入WordCountToolStripMenuItem
        '
        Me.插入WordCountToolStripMenuItem.Name = "插入WordCountToolStripMenuItem"
        Me.插入WordCountToolStripMenuItem.Size = New System.Drawing.Size(199, 22)
        Me.插入WordCountToolStripMenuItem.Text = "插入[WordCount] (&W)"
        '
        'ZiDongBoBaoCiShuTextBox
        '
        Me.ZiDongBoBaoCiShuTextBox.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.ZiDongBoBaoCiShuTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.ZiDongBoBaoCiShuTextBox.Location = New System.Drawing.Point(253, 103)
        Me.ZiDongBoBaoCiShuTextBox.MaxLength = 5
        Me.ZiDongBoBaoCiShuTextBox.Name = "ZiDongBoBaoCiShuTextBox"
        Me.ZiDongBoBaoCiShuTextBox.Size = New System.Drawing.Size(86, 29)
        Me.ZiDongBoBaoCiShuTextBox.TabIndex = 4
        Me.ZiDongBoBaoCiShuTextBox.Text = "2"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("微软雅黑", 20.0!)
        Me.Label4.Location = New System.Drawing.Point(7, 135)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(332, 49)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "等待播报"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(7, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(212, 29)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "自动播报次数（每个词语）"
        '
        'Label2
        '
        Me.Label2.Cursor = System.Windows.Forms.Cursors.Default
        Me.Label2.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(7, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(212, 29)
        Me.Label2.TabIndex = 1
        Me.Label2.Tag = ""
        Me.Label2.Text = "自动播报间隔（秒）"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("微软雅黑", 20.0!)
        Me.Label1.Location = New System.Drawing.Point(7, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(333, 49)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "本词语播报次数：0"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.TableLayoutPanel1)
        Me.GroupBox3.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(444, 226)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(350, 207)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "播报控制"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Button8, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Button7, 0, 4)
        Me.TableLayoutPanel1.Controls.Add(Me.Button1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Button2, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Button3, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Button4, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.Button5, 1, 3)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(13, 22)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 5
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(326, 179)
        Me.TableLayoutPanel1.TabIndex = 7
        '
        'Button8
        '
        Me.Button8.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button8.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button8.Location = New System.Drawing.Point(3, 143)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(157, 33)
        Me.Button8.TabIndex = 8
        Me.Button8.Text = "再报一遍(&M)"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button7.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button7.Location = New System.Drawing.Point(166, 143)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(157, 33)
        Me.Button7.TabIndex = 7
        Me.Button7.Text = "报上一个(&L)"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button1.Location = New System.Drawing.Point(3, 3)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(157, 29)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "自动播报(&S)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button2.Location = New System.Drawing.Point(166, 3)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(157, 29)
        Me.Button2.TabIndex = 3
        Me.Button2.Text = "停止播报(&D)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.SetColumnSpan(Me.Button3, 2)
        Me.Button3.Font = New System.Drawing.Font("微软雅黑", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button3.Location = New System.Drawing.Point(3, 38)
        Me.Button3.Name = "Button3"
        Me.TableLayoutPanel1.SetRowSpan(Me.Button3, 2)
        Me.Button3.Size = New System.Drawing.Size(320, 64)
        Me.Button3.TabIndex = 4
        Me.Button3.Text = "暂停自动播报(&P)"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button4.Location = New System.Drawing.Point(3, 108)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(157, 29)
        Me.Button4.TabIndex = 5
        Me.Button4.Text = "报下一个(&N)"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button5.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button5.Location = New System.Drawing.Point(166, 108)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(157, 29)
        Me.Button5.TabIndex = 6
        Me.Button5.Text = "记录归零(&C)"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button9
        '
        Me.Button9.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button9.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button9.Location = New System.Drawing.Point(444, 648)
        Me.Button9.Name = "Button9"
        Me.Button9.Size = New System.Drawing.Size(350, 37)
        Me.Button9.TabIndex = 4
        Me.Button9.Text = "隐藏词语列表"
        Me.Button9.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.Button11)
        Me.GroupBox4.Controls.Add(Me.Button10)
        Me.GroupBox4.Controls.Add(Me.ComboBox1)
        Me.GroupBox4.Controls.Add(Me.Label8)
        Me.GroupBox4.Controls.Add(Me.TrackBar2)
        Me.GroupBox4.Controls.Add(Me.Label7)
        Me.GroupBox4.Controls.Add(Me.TrackBar1)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.GroupBox4.Location = New System.Drawing.Point(444, 439)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(350, 202)
        Me.GroupBox4.TabIndex = 5
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "引擎设置"
        '
        'Button11
        '
        Me.Button11.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button11.Location = New System.Drawing.Point(214, 155)
        Me.Button11.Name = "Button11"
        Me.Button11.Size = New System.Drawing.Size(126, 41)
        Me.Button11.TabIndex = 7
        Me.Button11.Text = "切换到英文阅读引擎"
        Me.Button11.UseVisualStyleBackColor = True
        '
        'Button10
        '
        Me.Button10.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button10.Location = New System.Drawing.Point(83, 155)
        Me.Button10.Name = "Button10"
        Me.Button10.Size = New System.Drawing.Size(126, 41)
        Me.Button10.TabIndex = 6
        Me.Button10.Text = "切换到中文阅读引擎"
        Me.Button10.UseVisualStyleBackColor = True
        '
        'ComboBox1
        '
        Me.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox1.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.ComboBox1.FormattingEnabled = True
        Me.ComboBox1.ItemHeight = 17
        Me.ComboBox1.Location = New System.Drawing.Point(84, 124)
        Me.ComboBox1.MaxDropDownItems = 12
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(256, 25)
        Me.ComboBox1.TabIndex = 5
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label8.Location = New System.Drawing.Point(4, 124)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(67, 72)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "引擎："
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TrackBar2
        '
        Me.TrackBar2.Location = New System.Drawing.Point(83, 73)
        Me.TrackBar2.Minimum = -10
        Me.TrackBar2.Name = "TrackBar2"
        Me.TrackBar2.Size = New System.Drawing.Size(256, 45)
        Me.TrackBar2.TabIndex = 3
        Me.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.Both
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label7.Location = New System.Drawing.Point(4, 73)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(67, 45)
        Me.Label7.TabIndex = 2
        Me.Label7.Text = "语速："
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TrackBar1
        '
        Me.TrackBar1.LargeChange = 10
        Me.TrackBar1.Location = New System.Drawing.Point(83, 22)
        Me.TrackBar1.Maximum = 100
        Me.TrackBar1.Name = "TrackBar1"
        Me.TrackBar1.Size = New System.Drawing.Size(256, 45)
        Me.TrackBar1.TabIndex = 1
        Me.TrackBar1.TickFrequency = 5
        Me.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.Both
        Me.TrackBar1.Value = 100
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Label6.Location = New System.Drawing.Point(4, 22)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(67, 45)
        Me.Label6.TabIndex = 0
        Me.Label6.Text = "音量："
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'FontDialog1
        '
        Me.FontDialog1.ShowColor = True
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(803, 697)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.Button9)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(552, 680)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "自动默写 V2β"
        Me.GroupBox1.ResumeLayout(False)
        Me.CiYuListViewContextMenuStrip.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ZiDongBoBaoJianGeTextBoxContextMenuStrip.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents WordListView As System.Windows.Forms.ListView
    Friend WithEvents OpenButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents UpButton As System.Windows.Forms.Button
    Friend WithEvents DownButton As System.Windows.Forms.Button
    Friend WithEvents AddButton As System.Windows.Forms.Button
    Friend WithEvents EditButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents ClearButton As System.Windows.Forms.Button
    Friend WithEvents CountButton As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ZiDongBoBaoCiShuTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ZiDongBoBaoJianGeTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents 保存SToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 退出EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents CiYuListViewContextMenuStrip As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 上移ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 下移ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 修改ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 移除ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反向选择ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 全选ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 编辑EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 显示隐藏词语列表ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 添加词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 移除选中词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 清空词语列表ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 修改选中词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem3 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 向上移动ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 向下移动ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem4 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 全选ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反向选择ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents TrackBar1 As System.Windows.Forms.TrackBar
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents TrackBar2 As System.Windows.Forms.TrackBar
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Button11 As System.Windows.Forms.Button
    Friend WithEvents Button10 As System.Windows.Forms.Button
    Friend WithEvents 选项ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 自动翻页ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 字词跟随ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem7 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 设置词组增强ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 帮助HToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 关于AToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 辅助ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 随机排序ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 随机选词ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents FontDialog1 As System.Windows.Forms.FontDialog
    Friend WithEvents 字体FToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem8 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 读选定词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem9 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 在Bing词典中查看ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ZiDongBoBaoJianGeTextBoxContextMenuStrip As ContextMenuStrip
    Friend WithEvents ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents 插入LengthToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 插入WordCountToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 保存音频ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 插入ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 从此处开始自动播报ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Button8 As Button
    Friend WithEvents Button7 As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
    Friend WithEvents Button3 As Button
    Friend WithEvents Button4 As Button
    Friend WithEvents Button5 As Button
End Class
