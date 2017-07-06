namespace VE_SD
{
    partial class Frm_AnnotationSetting
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
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.btn_AnnotationReset = new System.Windows.Forms.Button();
            this.btn_YXRatioReset = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "文字大小";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.numericUpDown1.Location = new System.Drawing.Point(78, 7);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            16,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.ReadOnly = true;
            this.numericUpDown1.Size = new System.Drawing.Size(76, 27);
            this.numericUpDown1.TabIndex = 2;
            this.numericUpDown1.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Y軸/X軸Aspect比例:";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.DecimalPlaces = 2;
            this.numericUpDown2.Increment = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numericUpDown2.Location = new System.Drawing.Point(12, 77);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown2.Minimum = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(142, 27);
            this.numericUpDown2.TabIndex = 4;
            this.numericUpDown2.Value = new decimal(new int[] {
            25,
            0,
            0,
            131072});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // btn_AnnotationReset
            // 
            this.btn_AnnotationReset.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_AnnotationReset.Location = new System.Drawing.Point(160, 10);
            this.btn_AnnotationReset.Name = "btn_AnnotationReset";
            this.btn_AnnotationReset.Size = new System.Drawing.Size(48, 24);
            this.btn_AnnotationReset.TabIndex = 5;
            this.btn_AnnotationReset.Text = "Reset";
            this.toolTip1.SetToolTip(this.btn_AnnotationReset, "Reset Block Annotation文字大小");
            this.btn_AnnotationReset.UseVisualStyleBackColor = true;
            this.btn_AnnotationReset.Click += new System.EventHandler(this.btn_AnnotationReset_Click);
            // 
            // btn_YXRatioReset
            // 
            this.btn_YXRatioReset.Font = new System.Drawing.Font("微軟正黑體", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btn_YXRatioReset.Location = new System.Drawing.Point(160, 77);
            this.btn_YXRatioReset.Name = "btn_YXRatioReset";
            this.btn_YXRatioReset.Size = new System.Drawing.Size(48, 24);
            this.btn_YXRatioReset.TabIndex = 6;
            this.btn_YXRatioReset.Text = "Reset";
            this.toolTip1.SetToolTip(this.btn_YXRatioReset, "Reset Block繪圖區的Y/X Aspect Ratio");
            this.btn_YXRatioReset.UseVisualStyleBackColor = true;
            this.btn_YXRatioReset.Click += new System.EventHandler(this.btn_YXRatioReset_Click);
            // 
            // Frm_AnnotationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 116);
            this.Controls.Add(this.btn_YXRatioReset);
            this.Controls.Add(this.btn_AnnotationReset);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Frm_AnnotationSetting";
            this.Text = "Block繪圖區設定";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Frm_AnnotationSetting_FormClosed);
            this.Load += new System.EventHandler(this.Frm_AnnotationSetting_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Button btn_AnnotationReset;
        private System.Windows.Forms.Button btn_YXRatioReset;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}