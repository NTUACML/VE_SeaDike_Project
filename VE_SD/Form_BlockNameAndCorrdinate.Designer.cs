namespace VE_SD
{
    partial class Form_BlockNameAndCorrdinate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_BlockNameAndCorrdinate));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_NameInput = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.gp_AddCoordinate = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label_Show = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.X = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Insert = new System.Windows.Forms.Button();
            this.textBox_AddNumber = new System.Windows.Forms.TextBox();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_OK = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btn_RemovePoints = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.gp_AddNewName = new System.Windows.Forms.GroupBox();
            this.btn_SetName = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.gp_AddCoordinate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.gp_AddNewName.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(6, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "形塊名稱:";
            // 
            // textBox_NameInput
            // 
            this.textBox_NameInput.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.textBox_NameInput.Location = new System.Drawing.Point(110, 49);
            this.textBox_NameInput.Name = "textBox_NameInput";
            this.textBox_NameInput.Size = new System.Drawing.Size(375, 30);
            this.textBox_NameInput.TabIndex = 1;
            this.textBox_NameInput.TextChanged += new System.EventHandler(this.textBox_NameInput_TextChanged);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // gp_AddCoordinate
            // 
            this.gp_AddCoordinate.Controls.Add(this.button1);
            this.gp_AddCoordinate.Controls.Add(this.label_Show);
            this.gp_AddCoordinate.Controls.Add(this.label2);
            this.gp_AddCoordinate.Controls.Add(this.dataGridView1);
            this.gp_AddCoordinate.Controls.Add(this.btn_Insert);
            this.gp_AddCoordinate.Controls.Add(this.textBox_AddNumber);
            this.gp_AddCoordinate.Controls.Add(this.btn_Cancel);
            this.gp_AddCoordinate.Controls.Add(this.btn_OK);
            this.gp_AddCoordinate.Controls.Add(this.chart1);
            this.gp_AddCoordinate.Controls.Add(this.btn_RemovePoints);
            this.gp_AddCoordinate.Controls.Add(this.btn_Add);
            this.gp_AddCoordinate.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_AddCoordinate.Location = new System.Drawing.Point(27, 119);
            this.gp_AddCoordinate.Name = "gp_AddCoordinate";
            this.gp_AddCoordinate.Size = new System.Drawing.Size(566, 469);
            this.gp_AddCoordinate.TabIndex = 2;
            this.gp_AddCoordinate.TabStop = false;
            this.gp_AddCoordinate.Text = "第二步: 形塊座標";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(218, 159);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(86, 31);
            this.button1.TabIndex = 12;
            this.button1.Text = "Reverse";
            this.toolTip1.SetToolTip(this.button1, "顛倒選取欄位之順序");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_Show
            // 
            this.label_Show.AutoSize = true;
            this.label_Show.Font = new System.Drawing.Font("標楷體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label_Show.Location = new System.Drawing.Point(438, 54);
            this.label_Show.Name = "label_Show";
            this.label_Show.Size = new System.Drawing.Size(49, 13);
            this.label_Show.TabIndex = 11;
            this.label_Show.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(375, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 19);
            this.label2.TabIndex = 10;
            this.label2.Text = "列";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.X,
            this.Y});
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.dataGridView1.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.dataGridView1.Location = new System.Drawing.Point(10, 29);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(202, 434);
            this.dataGridView1.TabIndex = 9;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridView1_KeyPress);
            this.dataGridView1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridView1_KeyUp);
            // 
            // X
            // 
            this.X.HeaderText = "X";
            this.X.Name = "X";
            this.X.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.X.Width = 70;
            // 
            // Y
            // 
            this.Y.HeaderText = "Y";
            this.Y.Name = "Y";
            this.Y.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Y.Width = 70;
            // 
            // btn_Insert
            // 
            this.btn_Insert.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Insert.Location = new System.Drawing.Point(217, 76);
            this.btn_Insert.Name = "btn_Insert";
            this.btn_Insert.Size = new System.Drawing.Size(86, 31);
            this.btn_Insert.TabIndex = 7;
            this.btn_Insert.Text = "Insert";
            this.toolTip1.SetToolTip(this.btn_Insert, "插入新的點");
            this.btn_Insert.UseVisualStyleBackColor = true;
            this.btn_Insert.Click += new System.EventHandler(this.btn_Insert_Click);
            // 
            // textBox_AddNumber
            // 
            this.textBox_AddNumber.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_AddNumber.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.textBox_AddNumber.Location = new System.Drawing.Point(310, 29);
            this.textBox_AddNumber.Name = "textBox_AddNumber";
            this.textBox_AddNumber.Size = new System.Drawing.Size(59, 27);
            this.textBox_AddNumber.TabIndex = 6;
            this.textBox_AddNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_AddNumber_KeyPress);
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Cancel.Location = new System.Drawing.Point(474, 159);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(86, 31);
            this.btn_Cancel.TabIndex = 5;
            this.btn_Cancel.Text = "Cancel";
            this.toolTip1.SetToolTip(this.btn_Cancel, "放棄並退出編輯");
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_OK
            // 
            this.btn_OK.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_OK.Location = new System.Drawing.Point(382, 159);
            this.btn_OK.Name = "btn_OK";
            this.btn_OK.Size = new System.Drawing.Size(86, 31);
            this.btn_OK.TabIndex = 4;
            this.btn_OK.Text = "OK";
            this.toolTip1.SetToolTip(this.btn_OK, "完成編輯");
            this.btn_OK.UseVisualStyleBackColor = true;
            this.btn_OK.Click += new System.EventHandler(this.btn_OK_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(217, 196);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(343, 267);
            this.chart1.TabIndex = 3;
            this.chart1.Text = "chart1";
            this.chart1.Click += new System.EventHandler(this.chart1_Click);
            // 
            // btn_RemovePoints
            // 
            this.btn_RemovePoints.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_RemovePoints.Location = new System.Drawing.Point(217, 113);
            this.btn_RemovePoints.Name = "btn_RemovePoints";
            this.btn_RemovePoints.Size = new System.Drawing.Size(86, 31);
            this.btn_RemovePoints.TabIndex = 2;
            this.btn_RemovePoints.Text = "Remove";
            this.toolTip1.SetToolTip(this.btn_RemovePoints, "刪除選取的點");
            this.btn_RemovePoints.UseVisualStyleBackColor = true;
            this.btn_RemovePoints.Click += new System.EventHandler(this.btn_RemovePoints_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Add.Location = new System.Drawing.Point(218, 29);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(86, 31);
            this.btn_Add.TabIndex = 1;
            this.btn_Add.Text = "Add";
            this.toolTip1.SetToolTip(this.btn_Add, "新增新的點");
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // gp_AddNewName
            // 
            this.gp_AddNewName.Controls.Add(this.btn_SetName);
            this.gp_AddNewName.Controls.Add(this.textBox_NameInput);
            this.gp_AddNewName.Controls.Add(this.label1);
            this.gp_AddNewName.Font = new System.Drawing.Font("標楷體", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gp_AddNewName.Location = new System.Drawing.Point(27, 2);
            this.gp_AddNewName.Name = "gp_AddNewName";
            this.gp_AddNewName.Size = new System.Drawing.Size(572, 100);
            this.gp_AddNewName.TabIndex = 3;
            this.gp_AddNewName.TabStop = false;
            this.gp_AddNewName.Text = "第一步:形塊名稱";
            this.gp_AddNewName.Enter += new System.EventHandler(this.gp_AddNewName_Enter);
            // 
            // btn_SetName
            // 
            this.btn_SetName.Font = new System.Drawing.Font("標楷體", 12F);
            this.btn_SetName.Location = new System.Drawing.Point(491, 51);
            this.btn_SetName.Name = "btn_SetName";
            this.btn_SetName.Size = new System.Drawing.Size(75, 23);
            this.btn_SetName.TabIndex = 2;
            this.btn_SetName.Text = "輸入";
            this.btn_SetName.UseVisualStyleBackColor = true;
            this.btn_SetName.Click += new System.EventHandler(this.btn_SetName_Click);
            // 
            // Form_BlockNameAndCorrdinate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 600);
            this.Controls.Add(this.gp_AddNewName);
            this.Controls.Add(this.gp_AddCoordinate);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MaximizeBox = false;
            this.Name = "Form_BlockNameAndCorrdinate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "形塊形狀定義";
            this.Load += new System.EventHandler(this.Form_BlockNameAndCorrdinate_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.gp_AddCoordinate.ResumeLayout(false);
            this.gp_AddCoordinate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.gp_AddNewName.ResumeLayout(false);
            this.gp_AddNewName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_NameInput;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.GroupBox gp_AddNewName;
        private System.Windows.Forms.GroupBox gp_AddCoordinate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button btn_RemovePoints;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_OK;
        private System.Windows.Forms.Button btn_SetName;
        private System.Windows.Forms.TextBox textBox_AddNumber;
        private System.Windows.Forms.Button btn_Insert;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_Show;
        private System.Windows.Forms.DataGridViewTextBoxColumn X;
        private System.Windows.Forms.DataGridViewTextBoxColumn Y;
        private System.Windows.Forms.Button button1;
    }
}