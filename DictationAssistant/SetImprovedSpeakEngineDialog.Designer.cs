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
    partial class SetImprovedSpeakEngineDialog : System.Windows.Forms.Form
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
            this.Label1 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.FolderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.Button1 = new System.Windows.Forms.Button();
            this.CheckBox1 = new System.Windows.Forms.CheckBox();
            this.Button2 = new System.Windows.Forms.Button();
            this.Button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Label1
            // 
            this.Label1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Label1.Location = new System.Drawing.Point(11, 9);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(124, 29);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "词组增强目录：";
            this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TextBox1
            // 
            this.TextBox1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.TextBox1.Location = new System.Drawing.Point(139, 9);
            this.TextBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(372, 34);
            this.TextBox1.TabIndex = 1;
            this.TextBox1.Click += new System.EventHandler(this.TextBox1_TextChanged);
            // 
            // FolderBrowserDialog1
            // 
            this.FolderBrowserDialog1.Description = "词组增强目录：";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(515, 9);
            this.Button1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(71, 29);
            this.Button1.TabIndex = 2;
            this.Button1.Text = "浏览";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // CheckBox1
            // 
            this.CheckBox1.AutoSize = true;
            this.CheckBox1.Location = new System.Drawing.Point(14, 51);
            this.CheckBox1.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.CheckBox1.Name = "CheckBox1";
            this.CheckBox1.Size = new System.Drawing.Size(121, 24);
            this.CheckBox1.TabIndex = 3;
            this.CheckBox1.Text = "启用词组增强";
            this.CheckBox1.UseVisualStyleBackColor = true;
            // 
            // Button2
            // 
            this.Button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Button2.Location = new System.Drawing.Point(515, 46);
            this.Button2.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(71, 29);
            this.Button2.TabIndex = 4;
            this.Button2.Text = "确定";
            this.Button2.UseVisualStyleBackColor = true;
            this.Button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Button3
            // 
            this.Button3.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Button3.Location = new System.Drawing.Point(440, 46);
            this.Button3.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(71, 29);
            this.Button3.TabIndex = 5;
            this.Button3.Text = "取消";
            this.Button3.UseVisualStyleBackColor = true;
            // 
            // SetImprovedSpeakEngineDialog
            // 
            this.AcceptButton = this.Button2;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Button3;
            this.ClientSize = new System.Drawing.Size(593, 82);
            this.Controls.Add(this.Button3);
            this.Controls.Add(this.Button2);
            this.Controls.Add(this.CheckBox1);
            this.Controls.Add(this.Button1);
            this.Controls.Add(this.TextBox1);
            this.Controls.Add(this.Label1);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetImprovedSpeakEngineDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "词组增强";
            this.Load += new System.EventHandler(this.SetImprovedSpeakEngineDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.Label Label1;

        private System.Windows.Forms.TextBox TextBox1;

        private System.Windows.Forms.FolderBrowserDialog FolderBrowserDialog1;

        private System.Windows.Forms.Button Button1;

        private System.Windows.Forms.CheckBox CheckBox1;

        private System.Windows.Forms.Button Button2;

        private System.Windows.Forms.Button Button3;
    }
}
