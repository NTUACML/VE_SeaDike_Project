namespace VE_SD
{
    partial class Form_UserSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_UserSetting));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chk_ServiceOut = new System.Windows.Forms.CheckBox();
            this.chk_RemoveUserLog = new System.Windows.Forms.CheckBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_OK = new System.Windows.Forms.Button();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmb_軟體開啟時的視窗大小 = new System.Windows.Forms.ComboBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(2, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(442, 288);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tabPage1.Controls.Add(this.cmb_軟體開啟時的視窗大小);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.chk_ServiceOut);
            this.tabPage1.Controls.Add(this.chk_RemoveUserLog);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(434, 255);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chk_ServiceOut
            // 
            this.chk_ServiceOut.AutoSize = true;
            this.chk_ServiceOut.Location = new System.Drawing.Point(5, 43);
            this.chk_ServiceOut.Name = "chk_ServiceOut";
            this.chk_ServiceOut.Size = new System.Drawing.Size(124, 24);
            this.chk_ServiceOut.TabIndex = 1;
            this.chk_ServiceOut.Text = "提供服務訊息";
            this.chk_ServiceOut.UseVisualStyleBackColor = true;
            // 
            // chk_RemoveUserLog
            // 
            this.chk_RemoveUserLog.AutoSize = true;
            this.chk_RemoveUserLog.Location = new System.Drawing.Point(6, 17);
            this.chk_RemoveUserLog.Name = "chk_RemoveUserLog";
            this.chk_RemoveUserLog.Size = new System.Drawing.Size(300, 24);
            this.chk_RemoveUserLog.TabIndex = 0;
            this.chk_RemoveUserLog.Text = "每次關閉軟體後將登入使用者資訊清除";
            this.chk_RemoveUserLog.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(434, 255);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_OK
            // 
            this.btn_OK.Location = new System.Drawing.Point(283, 290);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(75, 28);
            this.btn_OK.TabIndex = 1;
            this.btn_OK.Text = "確定";
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Location = new System.Drawing.Point(358, 290);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(75, 28);
            this.btn_Cancel.TabIndex = 2;
            this.btn_Cancel.Text = "取消";
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "軟體開啟時的視窗大小";
            // 
            // cmb_軟體開啟時的視窗大小
            // 
            this.cmb_軟體開啟時的視窗大小.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_軟體開啟時的視窗大小.FormattingEnabled = true;
            this.cmb_軟體開啟時的視窗大小.Items.AddRange(new object[] {
            "最大",
            "正常"});
            this.cmb_軟體開啟時的視窗大小.Location = new System.Drawing.Point(171, 78);
            this.cmb_軟體開啟時的視窗大小.Name = "cmb_軟體開啟時的視窗大小";
            this.cmb_軟體開啟時的視窗大小.Size = new System.Drawing.Size(102, 28);
            this.cmb_軟體開啟時的視窗大小.TabIndex = 3;
            // 
            // Form_UserSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 319);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_OK);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("標楷體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_UserSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "軟體偏好設定";
            this.Load += new System.EventHandler(this.Form_UserSetting_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chk_RemoveUserLog;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.CheckBox chk_ServiceOut;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmb_軟體開啟時的視窗大小;
    }
}