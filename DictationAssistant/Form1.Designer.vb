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
        Me.ListView1 = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.读选定词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem9 = New System.Windows.Forms.ToolStripSeparator()
        Me.修改ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.删除ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.删除选中词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.清空词语列表ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.修改选中词语ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.设置单词增强ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.辅助ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.随机排序ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.随机选词ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.帮助HToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.关于AToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Button8 = New System.Windows.Forms.Button()
        Me.Button7 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
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
        Me.GroupBox1.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).BeginInit()
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
        Me.GroupBox1.Controls.Add(Me.ListView1)
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
        Me.CountButton.Location = New System.Drawing.Point(388, 324)
        Me.CountButton.Name = "CountButton"
        Me.CountButton.Size = New System.Drawing.Size(32, 32)
        Me.CountButton.TabIndex = 9
        Me.CountButton.Text = "计"
        Me.CountButton.UseVisualStyleBackColor = True
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
        Me.ClearButton.UseVisualStyleBackColor = True
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
        Me.DeleteButton.UseVisualStyleBackColor = True
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
        Me.EditButton.UseVisualStyleBackColor = True
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
        Me.AddButton.UseVisualStyleBackColor = True
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
        Me.DownButton.UseVisualStyleBackColor = True
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
        Me.UpButton.UseVisualStyleBackColor = True
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
        Me.SaveButton.UseVisualStyleBackColor = True
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
        Me.OpenButton.UseVisualStyleBackColor = True
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2})
        Me.ListView1.ContextMenuStrip = Me.ContextMenuStrip1
        Me.ListView1.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.ListView1.FullRowSelect = True
        Me.ListView1.HideSelection = False
        Me.ListView1.LabelEdit = True
        Me.ListView1.Location = New System.Drawing.Point(6, 20)
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(376, 632)
        Me.ListView1.TabIndex = 0
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "词语"
        Me.ColumnHeader1.Width = 140
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "播报次数"
        Me.ColumnHeader2.Width = 120
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.读选定词语ToolStripMenuItem, Me.ToolStripMenuItem9, Me.修改ToolStripMenuItem, Me.删除ToolStripMenuItem, Me.ToolStripMenuItem5, Me.上移ToolStripMenuItem, Me.下移ToolStripMenuItem, Me.ToolStripMenuItem6, Me.全选ToolStripMenuItem1, Me.反向选择ToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(163, 176)
        '
        '读选定词语ToolStripMenuItem
        '
        Me.读选定词语ToolStripMenuItem.Name = "读选定词语ToolStripMenuItem"
        Me.读选定词语ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.读选定词语ToolStripMenuItem.Text = "读选定词语"
        '
        'ToolStripMenuItem9
        '
        Me.ToolStripMenuItem9.Name = "ToolStripMenuItem9"
        Me.ToolStripMenuItem9.Size = New System.Drawing.Size(159, 6)
        '
        '修改ToolStripMenuItem
        '
        Me.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem"
        Me.修改ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.修改ToolStripMenuItem.Text = "修改(&M)"
        '
        '删除ToolStripMenuItem
        '
        Me.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem"
        Me.删除ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete
        Me.删除ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.删除ToolStripMenuItem.Text = "删除(&D)"
        '
        'ToolStripMenuItem5
        '
        Me.ToolStripMenuItem5.Name = "ToolStripMenuItem5"
        Me.ToolStripMenuItem5.Size = New System.Drawing.Size(159, 6)
        '
        '上移ToolStripMenuItem
        '
        Me.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem"
        Me.上移ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.上移ToolStripMenuItem.Text = "上移"
        '
        '下移ToolStripMenuItem
        '
        Me.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem"
        Me.下移ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
        Me.下移ToolStripMenuItem.Text = "下移"
        '
        'ToolStripMenuItem6
        '
        Me.ToolStripMenuItem6.Name = "ToolStripMenuItem6"
        Me.ToolStripMenuItem6.Size = New System.Drawing.Size(159, 6)
        '
        '全选ToolStripMenuItem1
        '
        Me.全选ToolStripMenuItem1.Name = "全选ToolStripMenuItem1"
        Me.全选ToolStripMenuItem1.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.A), System.Windows.Forms.Keys)
        Me.全选ToolStripMenuItem1.Size = New System.Drawing.Size(162, 22)
        Me.全选ToolStripMenuItem1.Text = "全选(&A)"
        '
        '反向选择ToolStripMenuItem
        '
        Me.反向选择ToolStripMenuItem.Name = "反向选择ToolStripMenuItem"
        Me.反向选择ToolStripMenuItem.Size = New System.Drawing.Size(162, 22)
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
        Me.MenuStrip1.Text = "MenuStrip1"
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
        Me.编辑EToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.显示隐藏词语列表ToolStripMenuItem, Me.ToolStripMenuItem2, Me.添加词语ToolStripMenuItem, Me.删除选中词语ToolStripMenuItem, Me.清空词语列表ToolStripMenuItem, Me.修改选中词语ToolStripMenuItem, Me.ToolStripMenuItem3, Me.向上移动ToolStripMenuItem, Me.向下移动ToolStripMenuItem, Me.ToolStripMenuItem4, Me.全选ToolStripMenuItem, Me.反向选择ToolStripMenuItem1})
        Me.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem"
        Me.编辑EToolStripMenuItem.Size = New System.Drawing.Size(59, 21)
        Me.编辑EToolStripMenuItem.Text = "编辑(&E)"
        '
        '显示隐藏词语列表ToolStripMenuItem
        '
        Me.显示隐藏词语列表ToolStripMenuItem.Name = "显示隐藏词语列表ToolStripMenuItem"
        Me.显示隐藏词语列表ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.显示隐藏词语列表ToolStripMenuItem.Text = "显示/隐藏词语控制区"
        Me.显示隐藏词语列表ToolStripMenuItem.ToolTipText = "显示或隐藏词语列表"
        '
        'ToolStripMenuItem2
        '
        Me.ToolStripMenuItem2.Name = "ToolStripMenuItem2"
        Me.ToolStripMenuItem2.Size = New System.Drawing.Size(186, 6)
        '
        '添加词语ToolStripMenuItem
        '
        Me.添加词语ToolStripMenuItem.Name = "添加词语ToolStripMenuItem"
        Me.添加词语ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.添加词语ToolStripMenuItem.Text = "添加词语(&A)..."
        Me.添加词语ToolStripMenuItem.ToolTipText = "添加一个词语"
        '
        '删除选中词语ToolStripMenuItem
        '
        Me.删除选中词语ToolStripMenuItem.Name = "删除选中词语ToolStripMenuItem"
        Me.删除选中词语ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.删除选中词语ToolStripMenuItem.Text = "删除选中词语(&D)"
        Me.删除选中词语ToolStripMenuItem.ToolTipText = "选中选中的词语"
        '
        '清空词语列表ToolStripMenuItem
        '
        Me.清空词语列表ToolStripMenuItem.Name = "清空词语列表ToolStripMenuItem"
        Me.清空词语列表ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.清空词语列表ToolStripMenuItem.Text = "清空词语列表(&C)"
        Me.清空词语列表ToolStripMenuItem.ToolTipText = "清空词语列表"
        '
        '修改选中词语ToolStripMenuItem
        '
        Me.修改选中词语ToolStripMenuItem.Name = "修改选中词语ToolStripMenuItem"
        Me.修改选中词语ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.修改选中词语ToolStripMenuItem.Text = "修改选中词语(&M)"
        Me.修改选中词语ToolStripMenuItem.ToolTipText = "修改选中的词语"
        '
        'ToolStripMenuItem3
        '
        Me.ToolStripMenuItem3.Name = "ToolStripMenuItem3"
        Me.ToolStripMenuItem3.Size = New System.Drawing.Size(186, 6)
        '
        '向上移动ToolStripMenuItem
        '
        Me.向上移动ToolStripMenuItem.Name = "向上移动ToolStripMenuItem"
        Me.向上移动ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.向上移动ToolStripMenuItem.Text = "向上移动"
        Me.向上移动ToolStripMenuItem.ToolTipText = "向上移动选中词语"
        '
        '向下移动ToolStripMenuItem
        '
        Me.向下移动ToolStripMenuItem.Name = "向下移动ToolStripMenuItem"
        Me.向下移动ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.向下移动ToolStripMenuItem.Text = "向下移动"
        Me.向下移动ToolStripMenuItem.ToolTipText = "向下移动选中词语"
        '
        'ToolStripMenuItem4
        '
        Me.ToolStripMenuItem4.Name = "ToolStripMenuItem4"
        Me.ToolStripMenuItem4.Size = New System.Drawing.Size(186, 6)
        '
        '全选ToolStripMenuItem
        '
        Me.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem"
        Me.全选ToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.全选ToolStripMenuItem.Text = "全选(&A)"
        Me.全选ToolStripMenuItem.ToolTipText = "选择全部词语"
        '
        '反向选择ToolStripMenuItem1
        '
        Me.反向选择ToolStripMenuItem1.Name = "反向选择ToolStripMenuItem1"
        Me.反向选择ToolStripMenuItem1.Size = New System.Drawing.Size(189, 22)
        Me.反向选择ToolStripMenuItem1.Text = "反向选择(&I)"
        Me.反向选择ToolStripMenuItem1.ToolTipText = "选择没有选中的词语"
        '
        '选项ToolStripMenuItem
        '
        Me.选项ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.字体FToolStripMenuItem, Me.ToolStripMenuItem8, Me.自动翻页ToolStripMenuItem, Me.字词跟随ToolStripMenuItem, Me.ToolStripMenuItem7, Me.设置单词增强ToolStripMenuItem})
        Me.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem"
        Me.选项ToolStripMenuItem.Size = New System.Drawing.Size(44, 21)
        Me.选项ToolStripMenuItem.Text = "选项"
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
        '设置单词增强ToolStripMenuItem
        '
        Me.设置单词增强ToolStripMenuItem.Name = "设置单词增强ToolStripMenuItem"
        Me.设置单词增强ToolStripMenuItem.Size = New System.Drawing.Size(148, 22)
        Me.设置单词增强ToolStripMenuItem.Text = "设置单词增强"
        Me.设置单词增强ToolStripMenuItem.ToolTipText = "通过WAV文件增强语音引擎效果（文件名与词语相同，查找时自动忽略不可作为文件名的字符）"
        '
        '辅助ToolStripMenuItem
        '
        Me.辅助ToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.随机排序ToolStripMenuItem, Me.随机选词ToolStripMenuItem})
        Me.辅助ToolStripMenuItem.Name = "辅助ToolStripMenuItem"
        Me.辅助ToolStripMenuItem.Size = New System.Drawing.Size(44, 21)
        Me.辅助ToolStripMenuItem.Text = "辅助"
        '
        '随机排序ToolStripMenuItem
        '
        Me.随机排序ToolStripMenuItem.Name = "随机排序ToolStripMenuItem"
        Me.随机排序ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.随机排序ToolStripMenuItem.Text = "随机排序"
        Me.随机排序ToolStripMenuItem.ToolTipText = "打乱现有词语的顺序"
        '
        '随机选词ToolStripMenuItem
        '
        Me.随机选词ToolStripMenuItem.Name = "随机选词ToolStripMenuItem"
        Me.随机选词ToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.随机选词ToolStripMenuItem.Text = "随机选词"
        Me.随机选词ToolStripMenuItem.ToolTipText = "随机保留指定数量的词语"
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
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.TextBox2)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.TextBox1)
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
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("微软雅黑", 20.0!)
        Me.Label4.Location = New System.Drawing.Point(7, 135)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(333, 49)
        Me.Label4.TabIndex = 5
        Me.Label4.Text = "等待播报"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TextBox2
        '
        Me.TextBox2.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.TextBox2.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox2.Location = New System.Drawing.Point(254, 103)
        Me.TextBox2.MaxLength = 4
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(86, 29)
        Me.TextBox2.TabIndex = 4
        Me.TextBox2.Text = "2"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.Label3.Location = New System.Drawing.Point(7, 103)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(221, 29)
        Me.Label3.TabIndex = 3
        Me.Label3.Text = "自动播报次数（每个词语）"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.TextBox1.ImeMode = System.Windows.Forms.ImeMode.Disable
        Me.TextBox1.Location = New System.Drawing.Point(254, 68)
        Me.TextBox1.MaxLength = 4
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(86, 29)
        Me.TextBox1.TabIndex = 2
        Me.TextBox1.Text = "3"
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("微软雅黑", 12.0!)
        Me.Label2.Location = New System.Drawing.Point(7, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(207, 29)
        Me.Label2.TabIndex = 1
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
        Me.GroupBox3.Controls.Add(Me.Button8)
        Me.GroupBox3.Controls.Add(Me.Button7)
        Me.GroupBox3.Controls.Add(Me.Button4)
        Me.GroupBox3.Controls.Add(Me.Button5)
        Me.GroupBox3.Controls.Add(Me.Button3)
        Me.GroupBox3.Controls.Add(Me.Button2)
        Me.GroupBox3.Controls.Add(Me.Button1)
        Me.GroupBox3.Font = New System.Drawing.Font("微软雅黑", 9.0!)
        Me.GroupBox3.Location = New System.Drawing.Point(444, 226)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(350, 207)
        Me.GroupBox3.TabIndex = 3
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "播报控制"
        '
        'Button8
        '
        Me.Button8.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button8.Location = New System.Drawing.Point(175, 166)
        Me.Button8.Name = "Button8"
        Me.Button8.Size = New System.Drawing.Size(165, 32)
        Me.Button8.TabIndex = 6
        Me.Button8.Text = "再报一遍(&A)"
        Me.Button8.UseVisualStyleBackColor = True
        '
        'Button7
        '
        Me.Button7.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button7.Location = New System.Drawing.Point(6, 166)
        Me.Button7.Name = "Button7"
        Me.Button7.Size = New System.Drawing.Size(165, 32)
        Me.Button7.TabIndex = 5
        Me.Button7.Text = "报上一个(&L)"
        Me.Button7.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button4.Location = New System.Drawing.Point(6, 128)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(165, 32)
        Me.Button4.TabIndex = 4
        Me.Button4.Text = "报下一个(&N)"
        Me.Button4.UseVisualStyleBackColor = True
        '
        'Button5
        '
        Me.Button5.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button5.Location = New System.Drawing.Point(175, 128)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(165, 32)
        Me.Button5.TabIndex = 3
        Me.Button5.Text = "记录归零(&C)"
        Me.Button5.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Enabled = False
        Me.Button3.Font = New System.Drawing.Font("微软雅黑", 18.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button3.Location = New System.Drawing.Point(6, 58)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(334, 64)
        Me.Button3.TabIndex = 2
        Me.Button3.Text = "暂停自动播报(&P)"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Enabled = False
        Me.Button2.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button2.Location = New System.Drawing.Point(175, 20)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(165, 32)
        Me.Button2.TabIndex = 1
        Me.Button2.Text = "停止自动播报(&E)"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("微软雅黑", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Button1.Location = New System.Drawing.Point(6, 20)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(165, 32)
        Me.Button1.TabIndex = 0
        Me.Button1.Text = "开始自动播报(&S)"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
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
        Me.TrackBar2.LargeChange = 10
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
        Me.FontDialog1.ShowApply = True
        Me.FontDialog1.ShowColor = True
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
        Me.MinimumSize = New System.Drawing.Size(551, 700)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = " 自动默写"
        Me.GroupBox1.ResumeLayout(False)
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.TrackBar2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TrackBar1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
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
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents Button8 As System.Windows.Forms.Button
    Friend WithEvents Button7 As System.Windows.Forms.Button
    Friend WithEvents Button4 As System.Windows.Forms.Button
    Friend WithEvents Button5 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents 保存SToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 退出EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button9 As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents 上移ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 下移ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 修改ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 删除ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 反向选择ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem5 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripMenuItem6 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 全选ToolStripMenuItem1 As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 编辑EToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 显示隐藏词语列表ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents 添加词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents 删除选中词语ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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
    Friend WithEvents 设置单词增强ToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
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

End Class
