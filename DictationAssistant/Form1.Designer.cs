using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Runtime.CompilerServices;

namespace DictationAssistant
{
    partial class Form1 : System.Windows.Forms.Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.CountButton = new System.Windows.Forms.Button();
            this.ClearButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.DownButton = new System.Windows.Forms.Button();
            this.UpButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.WordListView = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CiYuListViewContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.读选定词语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.在Bing词典中查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.从此处开始自动播报ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
            this.插入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.上移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.下移ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.全选ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.反向选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Label5 = new System.Windows.Forms.Label();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.退出EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.编辑EToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示隐藏词语列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.添加词语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改选中词语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除选中词语ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空词语列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.向上移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.向下移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.反向选择ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.选项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字体FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.自动翻页ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.字词跟随ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.设置词组增强ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.辅助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.随机排序ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.随机选词ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存音频ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助HToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于AToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SaveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.ZiDongBoBaoJianGeTextBox = new System.Windows.Forms.TextBox();
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.插入LengthToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.插入WordCountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ZiDongBoBaoCiShuTextBox = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.TableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Button8 = new System.Windows.Forms.Button();
            this.Button7 = new System.Windows.Forms.Button();
            this.Button1 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button4 = new System.Windows.Forms.Button();
            this.Button5 = new System.Windows.Forms.Button();
            this.Button9 = new System.Windows.Forms.Button();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.Button11 = new System.Windows.Forms.Button();
            this.Button10 = new System.Windows.Forms.Button();
            this.ComboBox1 = new System.Windows.Forms.ComboBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.TrackBar2 = new System.Windows.Forms.TrackBar();
            this.Label7 = new System.Windows.Forms.Label();
            this.TrackBar1 = new System.Windows.Forms.TrackBar();
            this.Label6 = new System.Windows.Forms.Label();
            this.FontDialog1 = new System.Windows.Forms.FontDialog();
            this.ErrorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.GroupBox1.SuspendLayout();
            this.CiYuListViewContextMenuStrip.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.TableLayoutPanel1.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.Controls.Add(this.CountButton);
            this.GroupBox1.Controls.Add(this.ClearButton);
            this.GroupBox1.Controls.Add(this.DeleteButton);
            this.GroupBox1.Controls.Add(this.EditButton);
            this.GroupBox1.Controls.Add(this.AddButton);
            this.GroupBox1.Controls.Add(this.DownButton);
            this.GroupBox1.Controls.Add(this.UpButton);
            this.GroupBox1.Controls.Add(this.SaveButton);
            this.GroupBox1.Controls.Add(this.OpenButton);
            this.GroupBox1.Controls.Add(this.WordListView);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.GroupBox1.Location = new System.Drawing.Point(12, 27);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(426, 658);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "词语列表";
            // 
            // CountButton
            // 
            this.CountButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CountButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CountButton.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.CountButton.Image = ((System.Drawing.Image)(resources.GetObject("CountButton.Image")));
            this.CountButton.Location = new System.Drawing.Point(388, 324);
            this.CountButton.Name = "CountButton";
            this.CountButton.Size = new System.Drawing.Size(32, 32);
            this.CountButton.TabIndex = 9;
            this.CountButton.UseCompatibleTextRendering = true;
            this.CountButton.Click += new System.EventHandler(this.CountButton_Click);
            // 
            // ClearButton
            // 
            this.ClearButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ClearButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClearButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.ClearButton.Image = ((System.Drawing.Image)(resources.GetObject("ClearButton.Image")));
            this.ClearButton.Location = new System.Drawing.Point(388, 286);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(32, 32);
            this.ClearButton.TabIndex = 8;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DeleteButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.Location = new System.Drawing.Point(388, 248);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(32, 32);
            this.DeleteButton.TabIndex = 7;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EditButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.EditButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.EditButton.Image = ((System.Drawing.Image)(resources.GetObject("EditButton.Image")));
            this.EditButton.Location = new System.Drawing.Point(388, 210);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(32, 32);
            this.EditButton.TabIndex = 6;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AddButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.Location = new System.Drawing.Point(388, 172);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(32, 32);
            this.AddButton.TabIndex = 5;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // DownButton
            // 
            this.DownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DownButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.DownButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.DownButton.Image = ((System.Drawing.Image)(resources.GetObject("DownButton.Image")));
            this.DownButton.Location = new System.Drawing.Point(388, 134);
            this.DownButton.Name = "DownButton";
            this.DownButton.Size = new System.Drawing.Size(32, 32);
            this.DownButton.TabIndex = 4;
            this.DownButton.Click += new System.EventHandler(this.DownButton_Click);
            // 
            // UpButton
            // 
            this.UpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.UpButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.UpButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.UpButton.Image = ((System.Drawing.Image)(resources.GetObject("UpButton.Image")));
            this.UpButton.Location = new System.Drawing.Point(388, 96);
            this.UpButton.Name = "UpButton";
            this.UpButton.Size = new System.Drawing.Size(32, 32);
            this.UpButton.TabIndex = 3;
            this.UpButton.Click += new System.EventHandler(this.UpButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.SaveButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.SaveButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveButton.Image")));
            this.SaveButton.Location = new System.Drawing.Point(388, 58);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(32, 32);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.OpenButton.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.OpenButton.Image = ((System.Drawing.Image)(resources.GetObject("OpenButton.Image")));
            this.OpenButton.Location = new System.Drawing.Point(388, 20);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(32, 32);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // WordListView
            // 
            this.WordListView.AllowColumnReorder = true;
            this.WordListView.AllowDrop = true;
            this.WordListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.WordListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1});
            this.WordListView.ContextMenuStrip = this.CiYuListViewContextMenuStrip;
            this.WordListView.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.WordListView.FullRowSelect = true;
            this.WordListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.WordListView.HideSelection = false;
            this.WordListView.LabelEdit = true;
            this.WordListView.Location = new System.Drawing.Point(6, 20);
            this.WordListView.Name = "WordListView";
            this.WordListView.Size = new System.Drawing.Size(376, 632);
            this.WordListView.TabIndex = 0;
            this.WordListView.UseCompatibleStateImageBehavior = false;
            this.WordListView.View = System.Windows.Forms.View.Details;
            this.WordListView.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.WordListView_AfterLabelEdit);
            this.WordListView.BeforeLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.WordListView_BeforeLabelEdit);
            this.WordListView.DragDrop += new System.Windows.Forms.DragEventHandler(this.WordListView_DragDrop);
            this.WordListView.DragOver += new System.Windows.Forms.DragEventHandler(this.WordListView_DragOver);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "词语";
            this.ColumnHeader1.Width = 300;
            // 
            // CiYuListViewContextMenuStrip
            // 
            this.CiYuListViewContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.CiYuListViewContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.读选定词语ToolStripMenuItem,
            this.在Bing词典中查看ToolStripMenuItem,
            this.从此处开始自动播报ToolStripMenuItem,
            this.ToolStripMenuItem9,
            this.插入ToolStripMenuItem,
            this.修改ToolStripMenuItem,
            this.移除ToolStripMenuItem,
            this.ToolStripMenuItem5,
            this.上移ToolStripMenuItem,
            this.下移ToolStripMenuItem,
            this.ToolStripMenuItem6,
            this.全选ToolStripMenuItem1,
            this.反向选择ToolStripMenuItem});
            this.CiYuListViewContextMenuStrip.Name = "ContextMenuStrip1";
            this.CiYuListViewContextMenuStrip.Size = new System.Drawing.Size(214, 262);
            // 
            // 读选定词语ToolStripMenuItem
            // 
            this.读选定词语ToolStripMenuItem.Name = "读选定词语ToolStripMenuItem";
            this.读选定词语ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.读选定词语ToolStripMenuItem.Text = "读选定词语";
            this.读选定词语ToolStripMenuItem.Click += new System.EventHandler(this.读选定词语ToolStripMenuItem_Click);
            // 
            // 在Bing词典中查看ToolStripMenuItem
            // 
            this.在Bing词典中查看ToolStripMenuItem.Name = "在Bing词典中查看ToolStripMenuItem";
            this.在Bing词典中查看ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.在Bing词典中查看ToolStripMenuItem.Text = "在Bing词典中查看";
            this.在Bing词典中查看ToolStripMenuItem.Click += new System.EventHandler(this.在Bing词典中查看ToolStripMenuItem_Click);
            // 
            // 从此处开始自动播报ToolStripMenuItem
            // 
            this.从此处开始自动播报ToolStripMenuItem.Name = "从此处开始自动播报ToolStripMenuItem";
            this.从此处开始自动播报ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.从此处开始自动播报ToolStripMenuItem.Text = "从此处开始自动播报";
            this.从此处开始自动播报ToolStripMenuItem.Click += new System.EventHandler(this.从此处开始自动播报ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem9
            // 
            this.ToolStripMenuItem9.Name = "ToolStripMenuItem9";
            this.ToolStripMenuItem9.Size = new System.Drawing.Size(210, 6);
            // 
            // 插入ToolStripMenuItem
            // 
            this.插入ToolStripMenuItem.Name = "插入ToolStripMenuItem";
            this.插入ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.插入ToolStripMenuItem.Text = "插入";
            this.插入ToolStripMenuItem.Click += new System.EventHandler(this.插入ToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.修改ToolStripMenuItem.Text = "修改(&M)";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // 移除ToolStripMenuItem
            // 
            this.移除ToolStripMenuItem.Name = "移除ToolStripMenuItem";
            this.移除ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.移除ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.移除ToolStripMenuItem.Text = "移除(&R)";
            this.移除ToolStripMenuItem.Click += new System.EventHandler(this.移除ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem5
            // 
            this.ToolStripMenuItem5.Name = "ToolStripMenuItem5";
            this.ToolStripMenuItem5.Size = new System.Drawing.Size(210, 6);
            // 
            // 上移ToolStripMenuItem
            // 
            this.上移ToolStripMenuItem.Name = "上移ToolStripMenuItem";
            this.上移ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.上移ToolStripMenuItem.Text = "上移";
            this.上移ToolStripMenuItem.Click += new System.EventHandler(this.上移ToolStripMenuItem_Click);
            // 
            // 下移ToolStripMenuItem
            // 
            this.下移ToolStripMenuItem.Name = "下移ToolStripMenuItem";
            this.下移ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.下移ToolStripMenuItem.Text = "下移";
            this.下移ToolStripMenuItem.Click += new System.EventHandler(this.下移ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem6
            // 
            this.ToolStripMenuItem6.Name = "ToolStripMenuItem6";
            this.ToolStripMenuItem6.Size = new System.Drawing.Size(210, 6);
            // 
            // 全选ToolStripMenuItem1
            // 
            this.全选ToolStripMenuItem1.Name = "全选ToolStripMenuItem1";
            this.全选ToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.全选ToolStripMenuItem1.Size = new System.Drawing.Size(213, 24);
            this.全选ToolStripMenuItem1.Text = "全选(&A)";
            this.全选ToolStripMenuItem1.Click += new System.EventHandler(this.全选ToolStripMenuItem1_Click);
            // 
            // 反向选择ToolStripMenuItem
            // 
            this.反向选择ToolStripMenuItem.Name = "反向选择ToolStripMenuItem";
            this.反向选择ToolStripMenuItem.Size = new System.Drawing.Size(213, 24);
            this.反向选择ToolStripMenuItem.Text = "反向选择(&I)";
            this.反向选择ToolStripMenuItem.Click += new System.EventHandler(this.反向选择ToolStripMenuItem_Click);
            // 
            // Label5
            // 
            this.Label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Label5.Font = new System.Drawing.Font("微软雅黑", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label5.Location = new System.Drawing.Point(6, 199);
            this.Label5.MinimumSize = new System.Drawing.Size(146, 82);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(414, 207);
            this.Label5.TabIndex = 11;
            this.Label5.Text = "词语列表\r\n 已隐藏 ";
            this.Label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Label5.Visible = false;
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.MenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.编辑EToolStripMenuItem,
            this.选项ToolStripMenuItem,
            this.辅助ToolStripMenuItem,
            this.帮助HToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(803, 28);
            this.MenuStrip1.TabIndex = 1;
            this.MenuStrip1.Text = "菜单栏";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.保存SToolStripMenuItem,
            this.ToolStripMenuItem1,
            this.退出EToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.FileToolStripMenuItem.Text = "文件(&F)";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.OpenToolStripMenuItem.Text = "打开(&O)...";
            this.OpenToolStripMenuItem.ToolTipText = "从一个TXT文件中批量添加词语到词语列表（每行作为一个词语）";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.OpenToolStripMenuItem_Click);
            // 
            // 保存SToolStripMenuItem
            // 
            this.保存SToolStripMenuItem.Name = "保存SToolStripMenuItem";
            this.保存SToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.保存SToolStripMenuItem.Text = "保存(&S)...";
            this.保存SToolStripMenuItem.ToolTipText = "将词语列表保存到一个TXT文件中（每个词语作为一行）";
            this.保存SToolStripMenuItem.Click += new System.EventHandler(this.保存SToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(153, 6);
            // 
            // 退出EToolStripMenuItem
            // 
            this.退出EToolStripMenuItem.Name = "退出EToolStripMenuItem";
            this.退出EToolStripMenuItem.Size = new System.Drawing.Size(156, 26);
            this.退出EToolStripMenuItem.Text = "退出(&E)";
            this.退出EToolStripMenuItem.ToolTipText = "退出自动默写程序";
            this.退出EToolStripMenuItem.Click += new System.EventHandler(this.退出EToolStripMenuItem_Click);
            // 
            // 编辑EToolStripMenuItem
            // 
            this.编辑EToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示隐藏词语列表ToolStripMenuItem,
            this.ToolStripMenuItem2,
            this.添加词语ToolStripMenuItem,
            this.修改选中词语ToolStripMenuItem,
            this.移除选中词语ToolStripMenuItem,
            this.清空词语列表ToolStripMenuItem,
            this.ToolStripMenuItem3,
            this.向上移动ToolStripMenuItem,
            this.向下移动ToolStripMenuItem,
            this.ToolStripMenuItem4,
            this.全选ToolStripMenuItem,
            this.反向选择ToolStripMenuItem1});
            this.编辑EToolStripMenuItem.Name = "编辑EToolStripMenuItem";
            this.编辑EToolStripMenuItem.Size = new System.Drawing.Size(71, 24);
            this.编辑EToolStripMenuItem.Text = "编辑(&E)";
            // 
            // 显示隐藏词语列表ToolStripMenuItem
            // 
            this.显示隐藏词语列表ToolStripMenuItem.Name = "显示隐藏词语列表ToolStripMenuItem";
            this.显示隐藏词语列表ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.显示隐藏词语列表ToolStripMenuItem.Text = "显示/隐藏词语控制区(&L)";
            this.显示隐藏词语列表ToolStripMenuItem.ToolTipText = "显示或隐藏词语列表";
            this.显示隐藏词语列表ToolStripMenuItem.Click += new System.EventHandler(this.显示隐藏词语列表ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem2
            // 
            this.ToolStripMenuItem2.Name = "ToolStripMenuItem2";
            this.ToolStripMenuItem2.Size = new System.Drawing.Size(248, 6);
            // 
            // 添加词语ToolStripMenuItem
            // 
            this.添加词语ToolStripMenuItem.Name = "添加词语ToolStripMenuItem";
            this.添加词语ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.添加词语ToolStripMenuItem.Text = "添加词语(&A)...";
            this.添加词语ToolStripMenuItem.ToolTipText = "添加一个词语";
            this.添加词语ToolStripMenuItem.Click += new System.EventHandler(this.添加词语ToolStripMenuItem_Click);
            // 
            // 修改选中词语ToolStripMenuItem
            // 
            this.修改选中词语ToolStripMenuItem.Name = "修改选中词语ToolStripMenuItem";
            this.修改选中词语ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.修改选中词语ToolStripMenuItem.Text = "修改选中词语(&M)";
            this.修改选中词语ToolStripMenuItem.ToolTipText = "修改选中的词语";
            this.修改选中词语ToolStripMenuItem.Click += new System.EventHandler(this.修改选中词语ToolStripMenuItem_Click);
            // 
            // 移除选中词语ToolStripMenuItem
            // 
            this.移除选中词语ToolStripMenuItem.Name = "移除选中词语ToolStripMenuItem";
            this.移除选中词语ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.移除选中词语ToolStripMenuItem.Text = "移除选中词语(&R)";
            this.移除选中词语ToolStripMenuItem.ToolTipText = "选中选中的词语";
            this.移除选中词语ToolStripMenuItem.Click += new System.EventHandler(this.移除选中词语ToolStripMenuItem_Click);
            // 
            // 清空词语列表ToolStripMenuItem
            // 
            this.清空词语列表ToolStripMenuItem.Name = "清空词语列表ToolStripMenuItem";
            this.清空词语列表ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.清空词语列表ToolStripMenuItem.Text = "清空词语列表(&C)";
            this.清空词语列表ToolStripMenuItem.ToolTipText = "清空词语列表";
            this.清空词语列表ToolStripMenuItem.Click += new System.EventHandler(this.清空词语列表ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem3
            // 
            this.ToolStripMenuItem3.Name = "ToolStripMenuItem3";
            this.ToolStripMenuItem3.Size = new System.Drawing.Size(248, 6);
            // 
            // 向上移动ToolStripMenuItem
            // 
            this.向上移动ToolStripMenuItem.Name = "向上移动ToolStripMenuItem";
            this.向上移动ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.向上移动ToolStripMenuItem.Text = "向上移动(&U)";
            this.向上移动ToolStripMenuItem.ToolTipText = "向上移动选中词语";
            this.向上移动ToolStripMenuItem.Click += new System.EventHandler(this.向上移动ToolStripMenuItem_Click);
            // 
            // 向下移动ToolStripMenuItem
            // 
            this.向下移动ToolStripMenuItem.Name = "向下移动ToolStripMenuItem";
            this.向下移动ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.向下移动ToolStripMenuItem.Text = "向下移动(&D)";
            this.向下移动ToolStripMenuItem.ToolTipText = "向下移动选中词语";
            this.向下移动ToolStripMenuItem.Click += new System.EventHandler(this.向下移动ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem4
            // 
            this.ToolStripMenuItem4.Name = "ToolStripMenuItem4";
            this.ToolStripMenuItem4.Size = new System.Drawing.Size(248, 6);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(251, 26);
            this.全选ToolStripMenuItem.Text = "全选(&A)";
            this.全选ToolStripMenuItem.ToolTipText = "选择全部词语";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 反向选择ToolStripMenuItem1
            // 
            this.反向选择ToolStripMenuItem1.Name = "反向选择ToolStripMenuItem1";
            this.反向选择ToolStripMenuItem1.Size = new System.Drawing.Size(251, 26);
            this.反向选择ToolStripMenuItem1.Text = "反向选择(&I)";
            this.反向选择ToolStripMenuItem1.ToolTipText = "选择没有选中的词语";
            this.反向选择ToolStripMenuItem1.Click += new System.EventHandler(this.反向选择ToolStripMenuItem1_Click);
            // 
            // 选项ToolStripMenuItem
            // 
            this.选项ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.字体FToolStripMenuItem,
            this.ToolStripMenuItem8,
            this.自动翻页ToolStripMenuItem,
            this.字词跟随ToolStripMenuItem,
            this.ToolStripMenuItem7,
            this.设置词组增强ToolStripMenuItem});
            this.选项ToolStripMenuItem.Name = "选项ToolStripMenuItem";
            this.选项ToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.选项ToolStripMenuItem.Text = "选项(&O)";
            // 
            // 字体FToolStripMenuItem
            // 
            this.字体FToolStripMenuItem.Name = "字体FToolStripMenuItem";
            this.字体FToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.字体FToolStripMenuItem.Text = "字体(&F)";
            this.字体FToolStripMenuItem.ToolTipText = "设置词语列表的字体";
            this.字体FToolStripMenuItem.Click += new System.EventHandler(this.字体FToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem8
            // 
            this.ToolStripMenuItem8.Name = "ToolStripMenuItem8";
            this.ToolStripMenuItem8.Size = new System.Drawing.Size(179, 6);
            // 
            // 自动翻页ToolStripMenuItem
            // 
            this.自动翻页ToolStripMenuItem.Checked = true;
            this.自动翻页ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.自动翻页ToolStripMenuItem.Name = "自动翻页ToolStripMenuItem";
            this.自动翻页ToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.自动翻页ToolStripMenuItem.Text = "自动翻页";
            this.自动翻页ToolStripMenuItem.ToolTipText = "自动滚动到正在播报的词语的位置";
            this.自动翻页ToolStripMenuItem.Click += new System.EventHandler(this.自动翻页ToolStripMenuItem_Click);
            // 
            // 字词跟随ToolStripMenuItem
            // 
            this.字词跟随ToolStripMenuItem.Checked = true;
            this.字词跟随ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.字词跟随ToolStripMenuItem.Name = "字词跟随ToolStripMenuItem";
            this.字词跟随ToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.字词跟随ToolStripMenuItem.Text = "字词跟随";
            this.字词跟随ToolStripMenuItem.ToolTipText = "自动选中正在播报的词语";
            this.字词跟随ToolStripMenuItem.Click += new System.EventHandler(this.字词跟随ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem7
            // 
            this.ToolStripMenuItem7.Name = "ToolStripMenuItem7";
            this.ToolStripMenuItem7.Size = new System.Drawing.Size(179, 6);
            // 
            // 设置词组增强ToolStripMenuItem
            // 
            this.设置词组增强ToolStripMenuItem.Name = "设置词组增强ToolStripMenuItem";
            this.设置词组增强ToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.设置词组增强ToolStripMenuItem.Text = "设置词组增强";
            this.设置词组增强ToolStripMenuItem.ToolTipText = "通过音频文件增强语音（文件名与词组相同，查找时忽略不可作为文件名的字符）";
            this.设置词组增强ToolStripMenuItem.Click += new System.EventHandler(this.设置词组增强ToolStripMenuItem_Click);
            // 
            // 辅助ToolStripMenuItem
            // 
            this.辅助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.随机排序ToolStripMenuItem,
            this.随机选词ToolStripMenuItem,
            this.保存音频ToolStripMenuItem});
            this.辅助ToolStripMenuItem.Name = "辅助ToolStripMenuItem";
            this.辅助ToolStripMenuItem.Size = new System.Drawing.Size(74, 24);
            this.辅助ToolStripMenuItem.Text = "辅助(&A)";
            // 
            // 随机排序ToolStripMenuItem
            // 
            this.随机排序ToolStripMenuItem.Name = "随机排序ToolStripMenuItem";
            this.随机排序ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.随机排序ToolStripMenuItem.Text = "随机排序";
            this.随机排序ToolStripMenuItem.ToolTipText = "打乱现有词语的顺序";
            this.随机排序ToolStripMenuItem.Click += new System.EventHandler(this.随机排序ToolStripMenuItem_Click);
            // 
            // 随机选词ToolStripMenuItem
            // 
            this.随机选词ToolStripMenuItem.Name = "随机选词ToolStripMenuItem";
            this.随机选词ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.随机选词ToolStripMenuItem.Text = "随机选词";
            this.随机选词ToolStripMenuItem.ToolTipText = "随机保留指定数量的词语";
            this.随机选词ToolStripMenuItem.Click += new System.EventHandler(this.随机选词ToolStripMenuItem_Click);
            // 
            // 保存音频ToolStripMenuItem
            // 
            this.保存音频ToolStripMenuItem.Name = "保存音频ToolStripMenuItem";
            this.保存音频ToolStripMenuItem.Size = new System.Drawing.Size(152, 26);
            this.保存音频ToolStripMenuItem.Text = "保存音频";
            this.保存音频ToolStripMenuItem.Click += new System.EventHandler(this.保存音频ToolStripMenuItem_Click);
            // 
            // 帮助HToolStripMenuItem
            // 
            this.帮助HToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关于AToolStripMenuItem});
            this.帮助HToolStripMenuItem.Name = "帮助HToolStripMenuItem";
            this.帮助HToolStripMenuItem.Size = new System.Drawing.Size(75, 24);
            this.帮助HToolStripMenuItem.Text = "帮助(&H)";
            // 
            // 关于AToolStripMenuItem
            // 
            this.关于AToolStripMenuItem.Name = "关于AToolStripMenuItem";
            this.关于AToolStripMenuItem.Size = new System.Drawing.Size(143, 26);
            this.关于AToolStripMenuItem.Text = "关于(&A)";
            this.关于AToolStripMenuItem.ToolTipText = "显示\"关于\"对话框";
            this.关于AToolStripMenuItem.Click += new System.EventHandler(this.关于AToolStripMenuItem_Click);
            // 
            // OpenFileDialog1
            // 
            this.OpenFileDialog1.DefaultExt = "txt";
            this.OpenFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
            this.OpenFileDialog1.Multiselect = true;
            this.OpenFileDialog1.Title = "打开词语列表文件";
            // 
            // SaveFileDialog1
            // 
            this.SaveFileDialog1.DefaultExt = "txt";
            this.SaveFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
            this.SaveFileDialog1.Title = "保存为词语列表文件";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.ZiDongBoBaoJianGeTextBox);
            this.GroupBox2.Controls.Add(this.ZiDongBoBaoCiShuTextBox);
            this.GroupBox2.Controls.Add(this.Label4);
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.Controls.Add(this.Label2);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.GroupBox2.Location = new System.Drawing.Point(444, 27);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(350, 193);
            this.GroupBox2.TabIndex = 2;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "播报信息";
            // 
            // ZiDongBoBaoJianGeTextBox
            // 
            this.ZiDongBoBaoJianGeTextBox.ContextMenuStrip = this.ZiDongBoBaoJianGeTextBoxContextMenuStrip;
            this.ZiDongBoBaoJianGeTextBox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ErrorProvider1.SetIconAlignment(this.ZiDongBoBaoJianGeTextBox, System.Windows.Forms.ErrorIconAlignment.MiddleLeft);
            this.ZiDongBoBaoJianGeTextBox.Location = new System.Drawing.Point(253, 68);
            this.ZiDongBoBaoJianGeTextBox.Name = "ZiDongBoBaoJianGeTextBox";
            this.ZiDongBoBaoJianGeTextBox.Size = new System.Drawing.Size(86, 34);
            this.ZiDongBoBaoJianGeTextBox.TabIndex = 2;
            this.ZiDongBoBaoJianGeTextBox.Text = "3";
            this.ZiDongBoBaoJianGeTextBox.TextChanged += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox_TextChanged);
            this.ZiDongBoBaoJianGeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ZiDongBoBaoCiShuTextBox_KeyPress);
            // 
            // ZiDongBoBaoJianGeTextBoxContextMenuStrip
            // 
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem,
            this.ToolStripSeparator1,
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem,
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem,
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem,
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem,
            this.ToolStripSeparator2,
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem,
            this.ToolStripSeparator3,
            this.插入LengthToolStripMenuItem,
            this.插入WordCountToolStripMenuItem});
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Name = "ZiDongBoBaoJianGeTextBoxContextMenuStrip";
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.Size = new System.Drawing.Size(233, 214);
            // 
            // ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Text = "撤销(&U)";
            this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(229, 6);
            // 
            // ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Text = "剪切(&T)";
            this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem_Click);
            // 
            // ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Text = "复制(&C)";
            this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem_Click);
            // 
            // ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Text = "粘贴(&P)";
            this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem_Click);
            // 
            // ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Text = "删除(&D)";
            this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(229, 6);
            // 
            // ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem
            // 
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Name = "ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem";
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Text = "全选(&A)";
            this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem.Click += new System.EventHandler(this.ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(229, 6);
            // 
            // 插入LengthToolStripMenuItem
            // 
            this.插入LengthToolStripMenuItem.Name = "插入LengthToolStripMenuItem";
            this.插入LengthToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.插入LengthToolStripMenuItem.Text = "插入[Length] (&L)";
            this.插入LengthToolStripMenuItem.Click += new System.EventHandler(this.插入LengthToolStripMenuItem_Click);
            // 
            // 插入WordCountToolStripMenuItem
            // 
            this.插入WordCountToolStripMenuItem.Name = "插入WordCountToolStripMenuItem";
            this.插入WordCountToolStripMenuItem.Size = new System.Drawing.Size(232, 24);
            this.插入WordCountToolStripMenuItem.Text = "插入[WordCount] (&W)";
            this.插入WordCountToolStripMenuItem.Click += new System.EventHandler(this.插入WordCountToolStripMenuItem_Click);
            // 
            // ZiDongBoBaoCiShuTextBox
            // 
            this.ZiDongBoBaoCiShuTextBox.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.ZiDongBoBaoCiShuTextBox.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.ZiDongBoBaoCiShuTextBox.Location = new System.Drawing.Point(253, 103);
            this.ZiDongBoBaoCiShuTextBox.MaxLength = 5;
            this.ZiDongBoBaoCiShuTextBox.Name = "ZiDongBoBaoCiShuTextBox";
            this.ZiDongBoBaoCiShuTextBox.Size = new System.Drawing.Size(86, 34);
            this.ZiDongBoBaoCiShuTextBox.TabIndex = 4;
            this.ZiDongBoBaoCiShuTextBox.Text = "2";
            this.ZiDongBoBaoCiShuTextBox.TextChanged += new System.EventHandler(this.ZiDongBoBaoCiShuTextBox_TextChanged);
            // 
            // Label4
            // 
            this.Label4.Font = new System.Drawing.Font("微软雅黑", 20F);
            this.Label4.Location = new System.Drawing.Point(7, 135);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(332, 49);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "等待播报";
            this.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Label3
            // 
            this.Label3.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Label3.Location = new System.Drawing.Point(7, 103);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(212, 29);
            this.Label3.TabIndex = 3;
            this.Label3.Text = "自动播报次数/词";
            // 
            // Label2
            // 
            this.Label2.Cursor = System.Windows.Forms.Cursors.Default;
            this.Label2.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.Label2.Location = new System.Drawing.Point(7, 68);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(212, 29);
            this.Label2.TabIndex = 1;
            this.Label2.Tag = "";
            this.Label2.Text = "自动播报间隔/秒";
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("微软雅黑", 20F);
            this.Label1.Location = new System.Drawing.Point(7, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(333, 49);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "本词语播报次数：0";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GroupBox3
            // 
            this.GroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox3.Controls.Add(this.TableLayoutPanel1);
            this.GroupBox3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.GroupBox3.Location = new System.Drawing.Point(444, 226);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(350, 207);
            this.GroupBox3.TabIndex = 3;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "播报控制";
            // 
            // TableLayoutPanel1
            // 
            this.TableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.ColumnCount = 2;
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.TableLayoutPanel1.Controls.Add(this.Button8, 0, 4);
            this.TableLayoutPanel1.Controls.Add(this.Button7, 0, 4);
            this.TableLayoutPanel1.Controls.Add(this.Button1, 0, 0);
            this.TableLayoutPanel1.Controls.Add(this.Button2, 1, 0);
            this.TableLayoutPanel1.Controls.Add(this.Button3, 0, 1);
            this.TableLayoutPanel1.Controls.Add(this.Button4, 0, 3);
            this.TableLayoutPanel1.Controls.Add(this.Button5, 1, 3);
            this.TableLayoutPanel1.Location = new System.Drawing.Point(13, 22);
            this.TableLayoutPanel1.Name = "TableLayoutPanel1";
            this.TableLayoutPanel1.RowCount = 5;
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.TableLayoutPanel1.Size = new System.Drawing.Size(326, 179);
            this.TableLayoutPanel1.TabIndex = 7;
            // 
            // Button8
            // 
            this.Button8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button8.Location = new System.Drawing.Point(3, 143);
            this.Button8.Name = "Button8";
            this.Button8.Size = new System.Drawing.Size(157, 33);
            this.Button8.TabIndex = 8;
            this.Button8.Text = "再报一遍(&M)";
            this.Button8.UseVisualStyleBackColor = true;
            this.Button8.Click += new System.EventHandler(this.Button8_Click);
            // 
            // Button7
            // 
            this.Button7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button7.Location = new System.Drawing.Point(166, 143);
            this.Button7.Name = "Button7";
            this.Button7.Size = new System.Drawing.Size(157, 33);
            this.Button7.TabIndex = 7;
            this.Button7.Text = "报上一个(&L)";
            this.Button7.UseVisualStyleBackColor = true;
            this.Button7.Click += new System.EventHandler(this.Button7_Click);
            // 
            // Button1
            // 
            this.Button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button1.Location = new System.Drawing.Point(3, 3);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(157, 29);
            this.Button1.TabIndex = 1;
            this.Button1.Text = "自动播报(&S)";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // Button2
            // 
            this.Button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button2.Location = new System.Drawing.Point(166, 3);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(157, 29);
            this.Button2.TabIndex = 3;
            this.Button2.Text = "停止播报(&D)";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TableLayoutPanel1.SetColumnSpan(this.Button3, 2);
            this.Button3.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button3.Location = new System.Drawing.Point(3, 38);
            this.Button3.Name = "Button3";
            this.TableLayoutPanel1.SetRowSpan(this.Button3, 2);
            this.Button3.Size = new System.Drawing.Size(320, 64);
            this.Button3.TabIndex = 4;
            this.Button3.Text = "暂停自动播报(&P)";
            this.Button3.UseVisualStyleBackColor = true;
            this.Button3.Click += new System.EventHandler(this.Button3_Click);
            // 
            // Button4
            // 
            this.Button4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button4.Location = new System.Drawing.Point(3, 108);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(157, 29);
            this.Button4.TabIndex = 5;
            this.Button4.Text = "报下一个(&N)";
            this.Button4.UseVisualStyleBackColor = true;
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Button5
            // 
            this.Button5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button5.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button5.Location = new System.Drawing.Point(166, 108);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(157, 29);
            this.Button5.TabIndex = 6;
            this.Button5.Text = "记录归零(&C)";
            this.Button5.UseVisualStyleBackColor = true;
            this.Button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // Button9
            // 
            this.Button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Button9.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button9.Location = new System.Drawing.Point(444, 648);
            this.Button9.Name = "Button9";
            this.Button9.Size = new System.Drawing.Size(350, 37);
            this.Button9.TabIndex = 4;
            this.Button9.Text = "隐藏词语列表";
            this.Button9.UseVisualStyleBackColor = true;
            this.Button9.Click += new System.EventHandler(this.Button9_Click);
            // 
            // GroupBox4
            // 
            this.GroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox4.Controls.Add(this.Button11);
            this.GroupBox4.Controls.Add(this.Button10);
            this.GroupBox4.Controls.Add(this.ComboBox1);
            this.GroupBox4.Controls.Add(this.Label8);
            this.GroupBox4.Controls.Add(this.TrackBar2);
            this.GroupBox4.Controls.Add(this.Label7);
            this.GroupBox4.Controls.Add(this.TrackBar1);
            this.GroupBox4.Controls.Add(this.Label6);
            this.GroupBox4.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.GroupBox4.Location = new System.Drawing.Point(444, 439);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(350, 202);
            this.GroupBox4.TabIndex = 5;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "引擎设置";
            // 
            // Button11
            // 
            this.Button11.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button11.Location = new System.Drawing.Point(214, 155);
            this.Button11.Name = "Button11";
            this.Button11.Size = new System.Drawing.Size(126, 41);
            this.Button11.TabIndex = 7;
            this.Button11.Text = "英文阅读引擎";
            this.Button11.UseVisualStyleBackColor = true;
            this.Button11.Click += new System.EventHandler(this.Button11_Click);
            // 
            // Button10
            // 
            this.Button10.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Button10.Location = new System.Drawing.Point(83, 155);
            this.Button10.Name = "Button10";
            this.Button10.Size = new System.Drawing.Size(126, 41);
            this.Button10.TabIndex = 6;
            this.Button10.Text = "中文阅读引擎";
            this.Button10.UseVisualStyleBackColor = true;
            this.Button10.Click += new System.EventHandler(this.Button10_Click);
            // 
            // ComboBox1
            // 
            this.ComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ComboBox1.FormattingEnabled = true;
            this.ComboBox1.ItemHeight = 20;
            this.ComboBox1.Location = new System.Drawing.Point(84, 124);
            this.ComboBox1.MaxDropDownItems = 12;
            this.ComboBox1.Name = "ComboBox1";
            this.ComboBox1.Size = new System.Drawing.Size(256, 28);
            this.ComboBox1.TabIndex = 5;
            this.ComboBox1.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // Label8
            // 
            this.Label8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label8.Location = new System.Drawing.Point(4, 124);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(67, 72);
            this.Label8.TabIndex = 4;
            this.Label8.Text = "引擎：";
            this.Label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TrackBar2
            // 
            this.TrackBar2.Location = new System.Drawing.Point(83, 73);
            this.TrackBar2.Minimum = -10;
            this.TrackBar2.Name = "TrackBar2";
            this.TrackBar2.Size = new System.Drawing.Size(256, 56);
            this.TrackBar2.TabIndex = 3;
            this.TrackBar2.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TrackBar2.Scroll += new System.EventHandler(this.TrackBar2_Scroll);
            // 
            // Label7
            // 
            this.Label7.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label7.Location = new System.Drawing.Point(4, 73);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(67, 45);
            this.Label7.TabIndex = 2;
            this.Label7.Text = "语速：";
            this.Label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TrackBar1
            // 
            this.TrackBar1.LargeChange = 10;
            this.TrackBar1.Location = new System.Drawing.Point(83, 22);
            this.TrackBar1.Maximum = 100;
            this.TrackBar1.Name = "TrackBar1";
            this.TrackBar1.Size = new System.Drawing.Size(256, 56);
            this.TrackBar1.TabIndex = 1;
            this.TrackBar1.TickFrequency = 5;
            this.TrackBar1.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TrackBar1.Value = 100;
            this.TrackBar1.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // Label6
            // 
            this.Label6.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label6.Location = new System.Drawing.Point(4, 22);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(67, 45);
            this.Label6.TabIndex = 0;
            this.Label6.Text = "音量：";
            this.Label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FontDialog1
            // 
            this.FontDialog1.ShowColor = true;
            // 
            // ErrorProvider1
            // 
            this.ErrorProvider1.ContainerControl = this;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 697);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.Button9);
            this.Controls.Add(this.GroupBox3);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.MenuStrip1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip1;
            this.MinimumSize = new System.Drawing.Size(552, 680);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动默写 V2β";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GroupBox1.ResumeLayout(false);
            this.CiYuListViewContextMenuStrip.ResumeLayout(false);
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.ZiDongBoBaoJianGeTextBoxContextMenuStrip.ResumeLayout(false);
            this.GroupBox3.ResumeLayout(false);
            this.TableLayoutPanel1.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    #endregion
    private System.Windows.Forms.GroupBox GroupBox1;

        

        private System.Windows.Forms.ListView WordListView;

        

        private System.Windows.Forms.Button OpenButton;

        

        private System.Windows.Forms.Button SaveButton;

        

        private System.Windows.Forms.Button UpButton;

        

        private System.Windows.Forms.Button DownButton;

        

        private System.Windows.Forms.Button AddButton;

        

        private System.Windows.Forms.Button EditButton;

        

        private System.Windows.Forms.Button DeleteButton;

        

        private System.Windows.Forms.Button ClearButton;

        

        private System.Windows.Forms.Button CountButton;

        

        private System.Windows.Forms.MenuStrip MenuStrip1;

        

        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;

        

        private System.Windows.Forms.OpenFileDialog OpenFileDialog1;

        

        private System.Windows.Forms.ColumnHeader ColumnHeader1;

        

        private System.Windows.Forms.ToolStripMenuItem OpenToolStripMenuItem;

        

        private System.Windows.Forms.SaveFileDialog SaveFileDialog1;

        

        private System.Windows.Forms.GroupBox GroupBox2;

        

        private System.Windows.Forms.Label Label1;

        

        private System.Windows.Forms.TextBox ZiDongBoBaoCiShuTextBox;

        

        private System.Windows.Forms.Label Label3;

        

        private System.Windows.Forms.TextBox ZiDongBoBaoJianGeTextBox;

        

        private System.Windows.Forms.Label Label2;

        

        private System.Windows.Forms.Label Label4;

        

        private System.Windows.Forms.GroupBox GroupBox3;

        

        private System.Windows.Forms.ToolStripMenuItem 保存SToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem1;

        

        private System.Windows.Forms.ToolStripMenuItem 退出EToolStripMenuItem;

        

        private System.Windows.Forms.Button Button9;

        

        private System.Windows.Forms.Label Label5;

        

        private System.Windows.Forms.ContextMenuStrip CiYuListViewContextMenuStrip;

        

        private System.Windows.Forms.ToolStripMenuItem 上移ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 下移ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 移除ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 反向选择ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem5;

        

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem6;

        

        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem1;

        

        private System.Windows.Forms.ToolStripMenuItem 编辑EToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 显示隐藏词语列表ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem2;

        

        private System.Windows.Forms.ToolStripMenuItem 添加词语ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 移除选中词语ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 清空词语列表ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripMenuItem 修改选中词语ToolStripMenuItem;

        

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem3;

        

        private System.Windows.Forms.ToolStripMenuItem 向上移动ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 向下移动ToolStripMenuItem;

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem4;

        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 反向选择ToolStripMenuItem1;

        private System.Windows.Forms.GroupBox GroupBox4;
        
        private System.Windows.Forms.TrackBar TrackBar1;

        private System.Windows.Forms.Label Label6;
        
        private System.Windows.Forms.TrackBar TrackBar2;
        
        private System.Windows.Forms.Label Label7;
        
        private System.Windows.Forms.Label Label8;

        private System.Windows.Forms.ComboBox ComboBox1;

        private System.Windows.Forms.Button Button11;

        private System.Windows.Forms.Button Button10;

        private System.Windows.Forms.ToolStripMenuItem 选项ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 自动翻页ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 字词跟随ToolStripMenuItem;

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem7;

        private System.Windows.Forms.ToolStripMenuItem 设置词组增强ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 帮助HToolStripMenuItem;
        
        private System.Windows.Forms.ToolStripMenuItem 关于AToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 辅助ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 随机排序ToolStripMenuItem;

        private System.Windows.Forms.ToolStripMenuItem 随机选词ToolStripMenuItem;
        
        private System.Windows.Forms.FontDialog FontDialog1;

        private System.Windows.Forms.ToolStripMenuItem 字体FToolStripMenuItem;

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem8;

        private System.Windows.Forms.ToolStripMenuItem 读选定词语ToolStripMenuItem;

        private System.Windows.Forms.ToolStripSeparator ToolStripMenuItem9;

        private System.Windows.Forms.ToolStripMenuItem 在Bing词典中查看ToolStripMenuItem;

        private ContextMenuStrip ZiDongBoBaoJianGeTextBoxContextMenuStrip;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox撤销ToolStripMenuItem;

        private ToolStripSeparator ToolStripSeparator1;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox剪切ToolStripMenuItem;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox复制ToolStripMenuItem;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox粘贴ToolStripMenuItem;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox删除ToolStripMenuItem;

        private ToolStripSeparator ToolStripSeparator2;

        private ToolStripMenuItem ZiDongBoBaoJianGeTextBox全选ToolStripMenuItem;
        
        private ErrorProvider ErrorProvider1;

        private ToolStripSeparator ToolStripSeparator3;

        private ToolStripMenuItem 插入LengthToolStripMenuItem;

        private ToolStripMenuItem 插入WordCountToolStripMenuItem;

        private ToolStripMenuItem 保存音频ToolStripMenuItem;

        private ToolStripMenuItem 插入ToolStripMenuItem;

        private ToolStripMenuItem 从此处开始自动播报ToolStripMenuItem;

        private TableLayoutPanel TableLayoutPanel1;

        private Button Button8;

        private Button Button7;

        private Button Button1;

        private Button Button2;

        private Button Button3;

        private Button Button4;

        private Button Button5;
    }
}
