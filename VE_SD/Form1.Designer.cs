﻿namespace VE_SD
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.軟體偏好設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.軟體機碼設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.軟體驗證ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.關閉此軟體ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.功能ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.海堤檢核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.海堤檢核給Kavy玩ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.測試傳送遠端ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.測試密碼轉換ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.碼頭檢核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.說明ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.檢示使用者說明書ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_LoginCond = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSSTATUS_label = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSP_Progressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.TSP_UserInfoShow = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSP_ChangeUserBtn = new System.Windows.Forms.ToolStripDropDownButton();
            this.TSP_Validate = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_StandardMT = new System.Windows.Forms.Button();
            this.btn_StandardRDC = new System.Windows.Forms.Button();
            this.gp_ItemExplan = new System.Windows.Forms.GroupBox();
            this.pictureBox_ItemDescp = new System.Windows.Forms.PictureBox();
            this.textBox_ItemDescp = new System.Windows.Forms.TextBox();
            this.bk_SendFIle = new System.ComponentModel.BackgroundWorker();
            this.bk_AccessServerForDownload = new System.ComponentModel.BackgroundWorker();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.bk_Validate = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gp_ItemExplan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ItemDescp)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem,
            this.功能ToolStripMenuItem,
            this.說明ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1113, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.設定ToolStripMenuItem,
            this.關閉此軟體ToolStripMenuItem});
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // 設定ToolStripMenuItem
            // 
            this.設定ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.軟體偏好設定ToolStripMenuItem,
            this.軟體機碼設定ToolStripMenuItem,
            this.軟體驗證ToolStripMenuItem});
            this.設定ToolStripMenuItem.Name = "設定ToolStripMenuItem";
            this.設定ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.設定ToolStripMenuItem.Text = "設定";
            // 
            // 軟體偏好設定ToolStripMenuItem
            // 
            this.軟體偏好設定ToolStripMenuItem.Name = "軟體偏好設定ToolStripMenuItem";
            this.軟體偏好設定ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.軟體偏好設定ToolStripMenuItem.Text = "軟體偏好設定";
            this.軟體偏好設定ToolStripMenuItem.Click += new System.EventHandler(this.軟體偏好設定ToolStripMenuItem_Click);
            // 
            // 軟體機碼設定ToolStripMenuItem
            // 
            this.軟體機碼設定ToolStripMenuItem.Name = "軟體機碼設定ToolStripMenuItem";
            this.軟體機碼設定ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.軟體機碼設定ToolStripMenuItem.Text = "軟體機碼設定";
            this.軟體機碼設定ToolStripMenuItem.Visible = false;
            this.軟體機碼設定ToolStripMenuItem.Click += new System.EventHandler(this.軟體機碼設定ToolStripMenuItem_Click);
            // 
            // 軟體驗證ToolStripMenuItem
            // 
            this.軟體驗證ToolStripMenuItem.Name = "軟體驗證ToolStripMenuItem";
            this.軟體驗證ToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
            this.軟體驗證ToolStripMenuItem.Text = "軟體驗證";
            this.軟體驗證ToolStripMenuItem.Click += new System.EventHandler(this.軟體驗證ToolStripMenuItem_Click);
            // 
            // 關閉此軟體ToolStripMenuItem
            // 
            this.關閉此軟體ToolStripMenuItem.Name = "關閉此軟體ToolStripMenuItem";
            this.關閉此軟體ToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.關閉此軟體ToolStripMenuItem.Text = "關閉此軟體";
            this.關閉此軟體ToolStripMenuItem.Click += new System.EventHandler(this.關閉此軟體ToolStripMenuItem_Click);
            // 
            // 功能ToolStripMenuItem
            // 
            this.功能ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.海堤檢核ToolStripMenuItem,
            this.海堤檢核給Kavy玩ToolStripMenuItem,
            this.測試傳送遠端ToolStripMenuItem,
            this.測試密碼轉換ToolStripMenuItem,
            this.碼頭檢核ToolStripMenuItem});
            this.功能ToolStripMenuItem.Name = "功能ToolStripMenuItem";
            this.功能ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.功能ToolStripMenuItem.Text = "功能";
            // 
            // 海堤檢核ToolStripMenuItem
            // 
            this.海堤檢核ToolStripMenuItem.Name = "海堤檢核ToolStripMenuItem";
            this.海堤檢核ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.海堤檢核ToolStripMenuItem.Text = "防波堤檢核";
            this.海堤檢核ToolStripMenuItem.Click += new System.EventHandler(this.海堤檢核ToolStripMenuItem_Click);
            // 
            // 海堤檢核給Kavy玩ToolStripMenuItem
            // 
            this.海堤檢核給Kavy玩ToolStripMenuItem.Name = "海堤檢核給Kavy玩ToolStripMenuItem";
            this.海堤檢核給Kavy玩ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.海堤檢核給Kavy玩ToolStripMenuItem.Text = "海堤檢核(給Kavy玩)";
            this.海堤檢核給Kavy玩ToolStripMenuItem.Visible = false;
            this.海堤檢核給Kavy玩ToolStripMenuItem.Click += new System.EventHandler(this.海堤檢核給Kavy玩ToolStripMenuItem_Click);
            // 
            // 測試傳送遠端ToolStripMenuItem
            // 
            this.測試傳送遠端ToolStripMenuItem.Name = "測試傳送遠端ToolStripMenuItem";
            this.測試傳送遠端ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.測試傳送遠端ToolStripMenuItem.Text = "測試傳送遠端";
            this.測試傳送遠端ToolStripMenuItem.Visible = false;
            this.測試傳送遠端ToolStripMenuItem.Click += new System.EventHandler(this.測試傳送遠端ToolStripMenuItem_Click);
            // 
            // 測試密碼轉換ToolStripMenuItem
            // 
            this.測試密碼轉換ToolStripMenuItem.Name = "測試密碼轉換ToolStripMenuItem";
            this.測試密碼轉換ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.測試密碼轉換ToolStripMenuItem.Text = "測試密碼轉換";
            this.測試密碼轉換ToolStripMenuItem.Visible = false;
            this.測試密碼轉換ToolStripMenuItem.Click += new System.EventHandler(this.測試密碼轉換ToolStripMenuItem_Click);
            // 
            // 碼頭檢核ToolStripMenuItem
            // 
            this.碼頭檢核ToolStripMenuItem.Name = "碼頭檢核ToolStripMenuItem";
            this.碼頭檢核ToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.碼頭檢核ToolStripMenuItem.Text = "碼頭檢核";
            this.碼頭檢核ToolStripMenuItem.Click += new System.EventHandler(this.碼頭檢核ToolStripMenuItem_Click);
            // 
            // 說明ToolStripMenuItem
            // 
            this.說明ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檢示使用者說明書ToolStripMenuItem});
            this.說明ToolStripMenuItem.Name = "說明ToolStripMenuItem";
            this.說明ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.說明ToolStripMenuItem.Text = "說明";
            // 
            // 檢示使用者說明書ToolStripMenuItem
            // 
            this.檢示使用者說明書ToolStripMenuItem.Name = "檢示使用者說明書ToolStripMenuItem";
            this.檢示使用者說明書ToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.檢示使用者說明書ToolStripMenuItem.Text = "檢示使用者說明書";
            this.檢示使用者說明書ToolStripMenuItem.Click += new System.EventHandler(this.檢示使用者說明書ToolStripMenuItem_Click);
            // 
            // label_LoginCond
            // 
            this.label_LoginCond.AutoSize = true;
            this.label_LoginCond.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_LoginCond.Location = new System.Drawing.Point(321, 24);
            this.label_LoginCond.Name = "label_LoginCond";
            this.label_LoginCond.Size = new System.Drawing.Size(33, 19);
            this.label_LoginCond.TabIndex = 1;
            this.label_LoginCond.Text = "NO";
            this.label_LoginCond.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSSTATUS_label,
            this.TSP_Progressbar,
            this.TSP_UserInfoShow,
            this.TSP_ChangeUserBtn,
            this.TSP_Validate});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 601);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1113, 30);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSSTATUS_label
            // 
            this.TSSTATUS_label.Name = "TSSTATUS_label";
            this.TSSTATUS_label.Size = new System.Drawing.Size(128, 25);
            this.TSSTATUS_label.Text = "toolStripStatusLabel1";
            // 
            // TSP_Progressbar
            // 
            this.TSP_Progressbar.Name = "TSP_Progressbar";
            this.TSP_Progressbar.Size = new System.Drawing.Size(300, 24);
            // 
            // TSP_UserInfoShow
            // 
            this.TSP_UserInfoShow.Name = "TSP_UserInfoShow";
            this.TSP_UserInfoShow.Size = new System.Drawing.Size(128, 25);
            this.TSP_UserInfoShow.Text = "toolStripStatusLabel1";
            // 
            // TSP_ChangeUserBtn
            // 
            this.TSP_ChangeUserBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.TSP_ChangeUserBtn.Image = ((System.Drawing.Image)(resources.GetObject("TSP_ChangeUserBtn.Image")));
            this.TSP_ChangeUserBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.TSP_ChangeUserBtn.Name = "TSP_ChangeUserBtn";
            this.TSP_ChangeUserBtn.Size = new System.Drawing.Size(116, 28);
            this.TSP_ChangeUserBtn.Text = "變更登入之使用者";
            this.TSP_ChangeUserBtn.Click += new System.EventHandler(this.TSP_ChangeUserBtn_Click);
            // 
            // TSP_Validate
            // 
            this.TSP_Validate.Name = "TSP_Validate";
            this.TSP_Validate.Size = new System.Drawing.Size(94, 25);
            this.TSP_Validate.Text = "                             ";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.btn_StandardMT);
            this.groupBox1.Controls.Add(this.btn_StandardRDC);
            this.groupBox1.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.groupBox1.Location = new System.Drawing.Point(13, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(399, 542);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "檢核項目選擇";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button3.Location = new System.Drawing.Point(16, 383);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(369, 115);
            this.button3.TabIndex = 3;
            this.button3.Text = "未開發";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.button2.Location = new System.Drawing.Point(16, 270);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(369, 115);
            this.button2.TabIndex = 2;
            this.button2.Text = "未開發";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btn_StandardMT
            // 
            this.btn_StandardMT.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_StandardMT.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_StandardMT.Location = new System.Drawing.Point(16, 156);
            this.btn_StandardMT.Name = "btn_StandardMT";
            this.btn_StandardMT.Size = new System.Drawing.Size(369, 115);
            this.btn_StandardMT.TabIndex = 1;
            this.btn_StandardMT.Text = "碼頭檢核";
            this.btn_StandardMT.UseVisualStyleBackColor = true;
            this.btn_StandardMT.Click += new System.EventHandler(this.button1_Click);
            this.btn_StandardMT.MouseEnter += new System.EventHandler(this.button1_MouseEnter);
            this.btn_StandardMT.MouseLeave += new System.EventHandler(this.button1_MouseLeave);
            // 
            // btn_StandardRDC
            // 
            this.btn_StandardRDC.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_StandardRDC.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_StandardRDC.Location = new System.Drawing.Point(16, 40);
            this.btn_StandardRDC.Name = "btn_StandardRDC";
            this.btn_StandardRDC.Size = new System.Drawing.Size(369, 115);
            this.btn_StandardRDC.TabIndex = 0;
            this.btn_StandardRDC.Text = "防波堤檢核";
            this.btn_StandardRDC.UseVisualStyleBackColor = true;
            this.btn_StandardRDC.Click += new System.EventHandler(this.btn_StandardRDC_Click);
            this.btn_StandardRDC.MouseEnter += new System.EventHandler(this.btn_StandardRDC_MouseEnter);
            this.btn_StandardRDC.MouseLeave += new System.EventHandler(this.btn_StandardRDC_MouseLeave);
            // 
            // gp_ItemExplan
            // 
            this.gp_ItemExplan.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gp_ItemExplan.Controls.Add(this.pictureBox_ItemDescp);
            this.gp_ItemExplan.Controls.Add(this.textBox_ItemDescp);
            this.gp_ItemExplan.Location = new System.Drawing.Point(433, 42);
            this.gp_ItemExplan.Name = "gp_ItemExplan";
            this.gp_ItemExplan.Size = new System.Drawing.Size(668, 542);
            this.gp_ItemExplan.TabIndex = 4;
            this.gp_ItemExplan.TabStop = false;
            // 
            // pictureBox_ItemDescp
            // 
            this.pictureBox_ItemDescp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_ItemDescp.Location = new System.Drawing.Point(7, 145);
            this.pictureBox_ItemDescp.Name = "pictureBox_ItemDescp";
            this.pictureBox_ItemDescp.Size = new System.Drawing.Size(655, 380);
            this.pictureBox_ItemDescp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_ItemDescp.TabIndex = 1;
            this.pictureBox_ItemDescp.TabStop = false;
            // 
            // textBox_ItemDescp
            // 
            this.textBox_ItemDescp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_ItemDescp.BackColor = System.Drawing.SystemColors.Control;
            this.textBox_ItemDescp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_ItemDescp.Font = new System.Drawing.Font("微軟正黑體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.textBox_ItemDescp.Location = new System.Drawing.Point(7, 25);
            this.textBox_ItemDescp.Multiline = true;
            this.textBox_ItemDescp.Name = "textBox_ItemDescp";
            this.textBox_ItemDescp.Size = new System.Drawing.Size(655, 119);
            this.textBox_ItemDescp.TabIndex = 0;
            // 
            // bk_SendFIle
            // 
            this.bk_SendFIle.WorkerReportsProgress = true;
            this.bk_SendFIle.WorkerSupportsCancellation = true;
            this.bk_SendFIle.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bk_SendFIle_DoWork);
            this.bk_SendFIle.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bk_SendFIle_RunWorkerCompleted);
            // 
            // bk_AccessServerForDownload
            // 
            this.bk_AccessServerForDownload.WorkerReportsProgress = true;
            this.bk_AccessServerForDownload.WorkerSupportsCancellation = true;
            this.bk_AccessServerForDownload.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bk_AccessServerForDownload_DoWork);
            this.bk_AccessServerForDownload.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bk_AccessServerForDownload_RunWorkerCompleted);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // bk_Validate
            // 
            this.bk_Validate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bk_Validate_DoWork);
            this.bk_Validate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bk_Validate_RunWorkerCompleted);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 631);
            this.Controls.Add(this.gp_ItemExplan);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label_LoginCond);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("標楷體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "浩海工程顧問公司檢核程式";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.gp_ItemExplan.ResumeLayout(false);
            this.gp_ItemExplan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_ItemDescp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 關閉此軟體ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 功能ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 海堤檢核ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 說明ToolStripMenuItem;
        private System.Windows.Forms.Label label_LoginCond;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel TSSTATUS_label;
        private System.Windows.Forms.ToolStripProgressBar TSP_Progressbar;
        private System.Windows.Forms.ToolStripMenuItem 海堤檢核給Kavy玩ToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel TSP_UserInfoShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_StandardRDC;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_StandardMT;
        private System.Windows.Forms.GroupBox gp_ItemExplan;
        private System.Windows.Forms.PictureBox pictureBox_ItemDescp;
        private System.Windows.Forms.TextBox textBox_ItemDescp;
        private System.Windows.Forms.ToolStripMenuItem 檢示使用者說明書ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 軟體偏好設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 軟體機碼設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton TSP_ChangeUserBtn;
        private System.Windows.Forms.ToolStripMenuItem 軟體驗證ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bk_SendFIle;
        private System.Windows.Forms.ToolStripMenuItem 測試傳送遠端ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 測試密碼轉換ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bk_AccessServerForDownload;
        private System.Windows.Forms.ToolStripStatusLabel TSP_Validate;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ToolStripMenuItem 碼頭檢核ToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker bk_Validate;
    }
}

