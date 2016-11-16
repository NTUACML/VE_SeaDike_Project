namespace VE_SD
{
    partial class Form_MTExamProgress
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟一個新的專案檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟舊的專案檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.btn_Test = new System.Windows.Forms.Button();
            this.btn_LogOutput = new System.Windows.Forms.Button();
            this.OFD_專案 = new System.Windows.Forms.OpenFileDialog();
            this.data_BlockTempShow = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.型塊周圍參考材質 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_BlockTempShow)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開啟一個新的專案檔ToolStripMenuItem,
            this.開啟舊的專案檔ToolStripMenuItem});
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // 開啟一個新的專案檔ToolStripMenuItem
            // 
            this.開啟一個新的專案檔ToolStripMenuItem.Name = "開啟一個新的專案檔ToolStripMenuItem";
            this.開啟一個新的專案檔ToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.開啟一個新的專案檔ToolStripMenuItem.Text = "開啟一個新的專案檔";
            this.開啟一個新的專案檔ToolStripMenuItem.Click += new System.EventHandler(this.開啟一個新的專案檔ToolStripMenuItem_Click);
            // 
            // 開啟舊的專案檔ToolStripMenuItem
            // 
            this.開啟舊的專案檔ToolStripMenuItem.Name = "開啟舊的專案檔ToolStripMenuItem";
            this.開啟舊的專案檔ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.開啟舊的專案檔ToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.開啟舊的專案檔ToolStripMenuItem.Text = "開啟舊的專案檔";
            this.開啟舊的專案檔ToolStripMenuItem.Click += new System.EventHandler(this.開啟舊的專案檔ToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Font = new System.Drawing.Font("標楷體", 11.25F);
            this.tabControl1.Location = new System.Drawing.Point(13, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1054, 454);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1046, 425);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "設計條件參數";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.data_BlockTempShow);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1046, 425);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "型塊設定";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 489);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1067, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(128, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.btn_LogOutput);
            this.tabPage3.Controls.Add(this.btn_Test);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(1046, 425);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "檢核";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // btn_Test
            // 
            this.btn_Test.Font = new System.Drawing.Font("標楷體", 15.75F);
            this.btn_Test.Location = new System.Drawing.Point(252, 36);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(183, 39);
            this.btn_Test.TabIndex = 0;
            this.btn_Test.Text = "進行檢核";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // btn_LogOutput
            // 
            this.btn_LogOutput.Font = new System.Drawing.Font("標楷體", 14F);
            this.btn_LogOutput.Location = new System.Drawing.Point(252, 114);
            this.btn_LogOutput.Name = "btn_LogOutput";
            this.btn_LogOutput.Size = new System.Drawing.Size(183, 32);
            this.btn_LogOutput.TabIndex = 1;
            this.btn_LogOutput.Text = "輸出LOG檔案";
            this.btn_LogOutput.UseVisualStyleBackColor = true;
            // 
            // OFD_專案
            // 
            this.OFD_專案.Filter = "檢核檔案|*.vesdp";
            this.OFD_專案.FilterIndex = 0;
            // 
            // data_BlockTempShow
            // 
            this.data_BlockTempShow.AllowUserToAddRows = false;
            this.data_BlockTempShow.AllowUserToDeleteRows = false;
            this.data_BlockTempShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_BlockTempShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.型塊周圍參考材質,
            this.Column4});
            this.data_BlockTempShow.Location = new System.Drawing.Point(114, 18);
            this.data_BlockTempShow.Name = "data_BlockTempShow";
            this.data_BlockTempShow.ReadOnly = true;
            this.data_BlockTempShow.RowTemplate.Height = 24;
            this.data_BlockTempShow.Size = new System.Drawing.Size(924, 379);
            this.data_BlockTempShow.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序號";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "形塊名稱";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "型塊單位體積重量";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 150;
            // 
            // 型塊周圍參考材質
            // 
            this.型塊周圍參考材質.HeaderText = "型塊周圍參考材質";
            this.型塊周圍參考材質.Name = "型塊周圍參考材質";
            this.型塊周圍參考材質.ReadOnly = true;
            this.型塊周圍參考材質.Width = 200;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "型塊使用材質";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 200;
            // 
            // Form_MTExamProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 511);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_MTExamProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "碼頭檢核";
            this.Load += new System.EventHandler(this.Form_MTExamProgress_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_BlockTempShow)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟一個新的專案檔ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開啟舊的專案檔ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btn_LogOutput;
        private System.Windows.Forms.OpenFileDialog OFD_專案;
        private System.Windows.Forms.DataGridView data_BlockTempShow;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 型塊周圍參考材質;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
    }
}