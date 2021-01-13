﻿
namespace Snake.FormLib
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_Start = new System.Windows.Forms.Button();
            this.gb_Score = new System.Windows.Forms.GroupBox();
            this.lb_Score = new System.Windows.Forms.Label();
            this.gb_Score.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Start.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btn_Start.Location = new System.Drawing.Point(163, 188);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(104, 50);
            this.btn_Start.TabIndex = 0;
            this.btn_Start.Text = "Start";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // gb_Score
            // 
            this.gb_Score.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gb_Score.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.gb_Score.Controls.Add(this.lb_Score);
            this.gb_Score.Location = new System.Drawing.Point(12, 479);
            this.gb_Score.Name = "gb_Score";
            this.gb_Score.Size = new System.Drawing.Size(409, 48);
            this.gb_Score.TabIndex = 1;
            this.gb_Score.TabStop = false;
            this.gb_Score.Text = "Score";
            this.gb_Score.Visible = false;
            // 
            // lb_Score
            // 
            this.lb_Score.AutoSize = true;
            this.lb_Score.Dock = System.Windows.Forms.DockStyle.Right;
            this.lb_Score.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lb_Score.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lb_Score.Location = new System.Drawing.Point(386, 19);
            this.lb_Score.Name = "lb_Score";
            this.lb_Score.Size = new System.Drawing.Size(20, 23);
            this.lb_Score.TabIndex = 0;
            this.lb_Score.Text = "0";
            this.lb_Score.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(434, 541);
            this.Controls.Add(this.gb_Score);
            this.Controls.Add(this.btn_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(240, 300);
            this.Name = "MainForm";
            this.Text = "Snake.NET";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainForm_KeyPress);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.gb_Score.ResumeLayout(false);
            this.gb_Score.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.GroupBox gb_Score;
        public System.Windows.Forms.Label lb_Score;
    }
}
