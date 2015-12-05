namespace VE_SD
{
    partial class Form_RDExamProgress
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.listBox_SectSetting = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsp_cond = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsp_progressbar = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.檔案ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開啟舊的專案檔ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退出此檢核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.步驟ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.參數設定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.開始檢核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart_Plot = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage_BasicParameter = new System.Windows.Forms.TabPage();
            this.textBox_Beta = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox_Lenda = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_Kd = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox_Ks = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_T0 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Kr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox_Slope = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_HWL = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_H0 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage_AddBlock = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox_YO = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox_XO = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRemoveSects = new System.Windows.Forms.Button();
            this.btn_AddASect = new System.Windows.Forms.Button();
            this.propertyGrid_Block = new System.Windows.Forms.PropertyGrid();
            this.tabPage_RunCheck = new System.Windows.Forms.TabPage();
            this.btn_Test = new System.Windows.Forms.Button();
            this.label_Show = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Plot)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage_BasicParameter.SuspendLayout();
            this.tabPage_AddBlock.SuspendLayout();
            this.tabPage_RunCheck.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_SectSetting
            // 
            this.listBox_SectSetting.FormattingEnabled = true;
            this.listBox_SectSetting.ItemHeight = 15;
            this.listBox_SectSetting.Location = new System.Drawing.Point(16, 32);
            this.listBox_SectSetting.Name = "listBox_SectSetting";
            this.listBox_SectSetting.Size = new System.Drawing.Size(163, 484);
            this.listBox_SectSetting.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsp_cond,
            this.tsp_progressbar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 596);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1051, 31);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsp_cond
            // 
            this.tsp_cond.Name = "tsp_cond";
            this.tsp_cond.Size = new System.Drawing.Size(128, 26);
            this.tsp_cond.Text = "toolStripStatusLabel1";
            // 
            // tsp_progressbar
            // 
            this.tsp_progressbar.Name = "tsp_progressbar";
            this.tsp_progressbar.Size = new System.Drawing.Size(350, 25);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.檔案ToolStripMenuItem,
            this.步驟ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 檔案ToolStripMenuItem
            // 
            this.檔案ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.開啟舊的專案檔ToolStripMenuItem,
            this.退出此檢核ToolStripMenuItem});
            this.檔案ToolStripMenuItem.Name = "檔案ToolStripMenuItem";
            this.檔案ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.檔案ToolStripMenuItem.Text = "檔案";
            // 
            // 開啟舊的專案檔ToolStripMenuItem
            // 
            this.開啟舊的專案檔ToolStripMenuItem.Name = "開啟舊的專案檔ToolStripMenuItem";
            this.開啟舊的專案檔ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.開啟舊的專案檔ToolStripMenuItem.Text = "開啟舊的專案檔";
            // 
            // 退出此檢核ToolStripMenuItem
            // 
            this.退出此檢核ToolStripMenuItem.Name = "退出此檢核ToolStripMenuItem";
            this.退出此檢核ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.退出此檢核ToolStripMenuItem.Text = "退出此檢核";
            // 
            // 步驟ToolStripMenuItem
            // 
            this.步驟ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.參數設定ToolStripMenuItem,
            this.開始檢核ToolStripMenuItem});
            this.步驟ToolStripMenuItem.Name = "步驟ToolStripMenuItem";
            this.步驟ToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.步驟ToolStripMenuItem.Text = "步驟";
            // 
            // 參數設定ToolStripMenuItem
            // 
            this.參數設定ToolStripMenuItem.Name = "參數設定ToolStripMenuItem";
            this.參數設定ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.參數設定ToolStripMenuItem.Text = "參數設定";
            // 
            // 開始檢核ToolStripMenuItem
            // 
            this.開始檢核ToolStripMenuItem.Name = "開始檢核ToolStripMenuItem";
            this.開始檢核ToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.開始檢核ToolStripMenuItem.Text = "開始檢核";
            // 
            // chart_Plot
            // 
            chartArea1.Name = "ChartArea1";
            this.chart_Plot.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart_Plot.Legends.Add(legend1);
            this.chart_Plot.Location = new System.Drawing.Point(185, 7);
            this.chart_Plot.Name = "chart_Plot";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart_Plot.Series.Add(series1);
            this.chart_Plot.Size = new System.Drawing.Size(850, 320);
            this.chart_Plot.TabIndex = 3;
            this.chart_Plot.Text = "chart_Plot";
            this.chart_Plot.Click += new System.EventHandler(this.chart_Plot_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage_BasicParameter);
            this.tabControl1.Controls.Add(this.tabPage_AddBlock);
            this.tabControl1.Controls.Add(this.tabPage_RunCheck);
            this.tabControl1.Font = new System.Drawing.Font("標楷體", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1051, 566);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage_BasicParameter
            // 
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Beta);
            this.tabPage_BasicParameter.Controls.Add(this.label10);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Lenda);
            this.tabPage_BasicParameter.Controls.Add(this.label9);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Kd);
            this.tabPage_BasicParameter.Controls.Add(this.label8);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Ks);
            this.tabPage_BasicParameter.Controls.Add(this.label7);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_T0);
            this.tabPage_BasicParameter.Controls.Add(this.label6);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Kr);
            this.tabPage_BasicParameter.Controls.Add(this.label5);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_Slope);
            this.tabPage_BasicParameter.Controls.Add(this.label4);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_HWL);
            this.tabPage_BasicParameter.Controls.Add(this.label3);
            this.tabPage_BasicParameter.Controls.Add(this.textBox_H0);
            this.tabPage_BasicParameter.Controls.Add(this.label2);
            this.tabPage_BasicParameter.Controls.Add(this.label1);
            this.tabPage_BasicParameter.Location = new System.Drawing.Point(4, 25);
            this.tabPage_BasicParameter.Name = "tabPage_BasicParameter";
            this.tabPage_BasicParameter.Size = new System.Drawing.Size(1043, 537);
            this.tabPage_BasicParameter.TabIndex = 2;
            this.tabPage_BasicParameter.Text = "設計條件參數";
            this.tabPage_BasicParameter.UseVisualStyleBackColor = true;
            // 
            // textBox_Beta
            // 
            this.textBox_Beta.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Beta.Location = new System.Drawing.Point(402, 325);
            this.textBox_Beta.Name = "textBox_Beta";
            this.textBox_Beta.Size = new System.Drawing.Size(100, 27);
            this.textBox_Beta.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("標楷體", 14F);
            this.label10.Location = new System.Drawing.Point(37, 314);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(359, 38);
            this.label10.TabIndex = 17;
            this.label10.Text = "入射波與堤體法線之垂線夾角\r\n(正負15度修正後) ..........β     =";
            // 
            // textBox_Lenda
            // 
            this.textBox_Lenda.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Lenda.Location = new System.Drawing.Point(402, 278);
            this.textBox_Lenda.Name = "textBox_Lenda";
            this.textBox_Lenda.Size = new System.Drawing.Size(100, 27);
            this.textBox_Lenda.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("標楷體", 14F);
            this.label9.Location = new System.Drawing.Point(37, 281);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(359, 19);
            this.label9.TabIndex = 15;
            this.label9.Text = "波力折減係數...............λ     =";
            // 
            // textBox_Kd
            // 
            this.textBox_Kd.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Kd.Location = new System.Drawing.Point(402, 241);
            this.textBox_Kd.Name = "textBox_Kd";
            this.textBox_Kd.Size = new System.Drawing.Size(100, 27);
            this.textBox_Kd.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("標楷體", 14F);
            this.label8.Location = new System.Drawing.Point(37, 244);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(359, 19);
            this.label8.TabIndex = 13;
            this.label8.Text = "繞射係數 .................. Kd    =";
            // 
            // textBox_Ks
            // 
            this.textBox_Ks.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Ks.Location = new System.Drawing.Point(402, 210);
            this.textBox_Ks.Name = "textBox_Ks";
            this.textBox_Ks.Size = new System.Drawing.Size(100, 27);
            this.textBox_Ks.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("標楷體", 14F);
            this.label7.Location = new System.Drawing.Point(37, 213);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(359, 19);
            this.label7.TabIndex = 11;
            this.label7.Text = "淺化係數 .................. Ks    =";
            // 
            // textBox_T0
            // 
            this.textBox_T0.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_T0.Location = new System.Drawing.Point(402, 82);
            this.textBox_T0.Name = "textBox_T0";
            this.textBox_T0.Size = new System.Drawing.Size(100, 27);
            this.textBox_T0.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("標楷體", 14F);
            this.label6.Location = new System.Drawing.Point(37, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(359, 19);
            this.label6.TabIndex = 9;
            this.label6.Text = "深海波週期.................. T0   =";
            // 
            // textBox_Kr
            // 
            this.textBox_Kr.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Kr.Location = new System.Drawing.Point(402, 178);
            this.textBox_Kr.Name = "textBox_Kr";
            this.textBox_Kr.Size = new System.Drawing.Size(100, 27);
            this.textBox_Kr.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("標楷體", 14F);
            this.label5.Location = new System.Drawing.Point(37, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(359, 19);
            this.label5.TabIndex = 7;
            this.label5.Text = "折射係數 .................. Kr    =";
            // 
            // textBox_Slope
            // 
            this.textBox_Slope.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Slope.Location = new System.Drawing.Point(401, 145);
            this.textBox_Slope.Name = "textBox_Slope";
            this.textBox_Slope.Size = new System.Drawing.Size(100, 27);
            this.textBox_Slope.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("標楷體", 14F);
            this.label4.Location = new System.Drawing.Point(37, 148);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(359, 19);
            this.label4.TabIndex = 5;
            this.label4.Text = "海床坡度 .................. Slope =";
            // 
            // textBox_HWL
            // 
            this.textBox_HWL.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_HWL.Location = new System.Drawing.Point(401, 113);
            this.textBox_HWL.Name = "textBox_HWL";
            this.textBox_HWL.Size = new System.Drawing.Size(100, 27);
            this.textBox_HWL.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("標楷體", 14F);
            this.label3.Location = new System.Drawing.Point(37, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(359, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "設計潮位  .................. HWL  =";
            // 
            // textBox_H0
            // 
            this.textBox_H0.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_H0.Location = new System.Drawing.Point(401, 48);
            this.textBox_H0.Name = "textBox_H0";
            this.textBox_H0.Size = new System.Drawing.Size(100, 27);
            this.textBox_H0.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("標楷體", 14F);
            this.label2.Location = new System.Drawing.Point(37, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(359, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "深海波波高.................. H0   =";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("標楷體", 14F);
            this.label1.Location = new System.Drawing.Point(37, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(379, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "深海波波向.................. 波向 = E";
            // 
            // tabPage_AddBlock
            // 
            this.tabPage_AddBlock.Controls.Add(this.label12);
            this.tabPage_AddBlock.Controls.Add(this.textBox_YO);
            this.tabPage_AddBlock.Controls.Add(this.label11);
            this.tabPage_AddBlock.Controls.Add(this.textBox_XO);
            this.tabPage_AddBlock.Controls.Add(this.button1);
            this.tabPage_AddBlock.Controls.Add(this.btnRemoveSects);
            this.tabPage_AddBlock.Controls.Add(this.btn_AddASect);
            this.tabPage_AddBlock.Controls.Add(this.propertyGrid_Block);
            this.tabPage_AddBlock.Controls.Add(this.listBox_SectSetting);
            this.tabPage_AddBlock.Controls.Add(this.chart_Plot);
            this.tabPage_AddBlock.Location = new System.Drawing.Point(4, 25);
            this.tabPage_AddBlock.Name = "tabPage_AddBlock";
            this.tabPage_AddBlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_AddBlock.Size = new System.Drawing.Size(1043, 537);
            this.tabPage_AddBlock.TabIndex = 0;
            this.tabPage_AddBlock.Text = "形塊設定";
            this.tabPage_AddBlock.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("標楷體", 13F);
            this.label12.Location = new System.Drawing.Point(190, 425);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(71, 18);
            this.label12.TabIndex = 11;
            this.label12.Text = "Y參考點";
            // 
            // textBox_YO
            // 
            this.textBox_YO.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_YO.Location = new System.Drawing.Point(267, 421);
            this.textBox_YO.Name = "textBox_YO";
            this.textBox_YO.ReadOnly = true;
            this.textBox_YO.Size = new System.Drawing.Size(100, 27);
            this.textBox_YO.TabIndex = 10;
            this.toolTip1.SetToolTip(this.textBox_YO, "力矩計算時的參考點Y座標");
            this.textBox_YO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_YO_KeyPress);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("標楷體", 13F);
            this.label11.Location = new System.Drawing.Point(190, 385);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 18);
            this.label11.TabIndex = 9;
            this.label11.Text = "X參考點";
            // 
            // textBox_XO
            // 
            this.textBox_XO.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_XO.Location = new System.Drawing.Point(267, 381);
            this.textBox_XO.Name = "textBox_XO";
            this.textBox_XO.ReadOnly = true;
            this.textBox_XO.Size = new System.Drawing.Size(100, 27);
            this.textBox_XO.TabIndex = 8;
            this.toolTip1.SetToolTip(this.textBox_XO, "力矩計算時的參考點X座標");
            this.textBox_XO.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_XO_KeyPress);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 344);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 31);
            this.button1.TabIndex = 7;
            this.button1.Text = "力矩計算參考點調整";
            this.toolTip1.SetToolTip(this.button1, "變更力矩參考點");
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRemoveSects
            // 
            this.btnRemoveSects.Location = new System.Drawing.Point(158, 7);
            this.btnRemoveSects.Name = "btnRemoveSects";
            this.btnRemoveSects.Size = new System.Drawing.Size(21, 19);
            this.btnRemoveSects.TabIndex = 6;
            this.btnRemoveSects.Text = "-";
            this.toolTip1.SetToolTip(this.btnRemoveSects, "刪除Block");
            this.btnRemoveSects.UseVisualStyleBackColor = true;
            // 
            // btn_AddASect
            // 
            this.btn_AddASect.Location = new System.Drawing.Point(132, 5);
            this.btn_AddASect.Name = "btn_AddASect";
            this.btn_AddASect.Size = new System.Drawing.Size(21, 20);
            this.btn_AddASect.TabIndex = 5;
            this.btn_AddASect.Text = "+";
            this.toolTip1.SetToolTip(this.btn_AddASect, "新增一個新的Block");
            this.btn_AddASect.UseVisualStyleBackColor = true;
            // 
            // propertyGrid_Block
            // 
            this.propertyGrid_Block.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid_Block.HelpVisible = false;
            this.propertyGrid_Block.Location = new System.Drawing.Point(373, 344);
            this.propertyGrid_Block.Name = "propertyGrid_Block";
            this.propertyGrid_Block.Size = new System.Drawing.Size(662, 172);
            this.propertyGrid_Block.TabIndex = 4;
            // 
            // tabPage_RunCheck
            // 
            this.tabPage_RunCheck.Controls.Add(this.btn_Test);
            this.tabPage_RunCheck.Controls.Add(this.label_Show);
            this.tabPage_RunCheck.Location = new System.Drawing.Point(4, 25);
            this.tabPage_RunCheck.Name = "tabPage_RunCheck";
            this.tabPage_RunCheck.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_RunCheck.Size = new System.Drawing.Size(1043, 537);
            this.tabPage_RunCheck.TabIndex = 1;
            this.tabPage_RunCheck.Text = "檢核";
            this.tabPage_RunCheck.UseVisualStyleBackColor = true;
            // 
            // btn_Test
            // 
            this.btn_Test.Location = new System.Drawing.Point(28, 53);
            this.btn_Test.Name = "btn_Test";
            this.btn_Test.Size = new System.Drawing.Size(75, 23);
            this.btn_Test.TabIndex = 1;
            this.btn_Test.Text = "測試";
            this.btn_Test.UseVisualStyleBackColor = true;
            this.btn_Test.Click += new System.EventHandler(this.btn_Test_Click);
            // 
            // label_Show
            // 
            this.label_Show.AutoSize = true;
            this.label_Show.Location = new System.Drawing.Point(25, 18);
            this.label_Show.Name = "label_Show";
            this.label_Show.Size = new System.Drawing.Size(55, 15);
            this.label_Show.TabIndex = 0;
            this.label_Show.Text = "label1";
            // 
            // Form_RDExamProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 627);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form_RDExamProgress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form_RDExamProgress";
            this.Load += new System.EventHandler(this.Form_RDExamProgress_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Plot)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage_BasicParameter.ResumeLayout(false);
            this.tabPage_BasicParameter.PerformLayout();
            this.tabPage_AddBlock.ResumeLayout(false);
            this.tabPage_AddBlock.PerformLayout();
            this.tabPage_RunCheck.ResumeLayout(false);
            this.tabPage_RunCheck.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_SectSetting;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsp_cond;
        private System.Windows.Forms.ToolStripProgressBar tsp_progressbar;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 檔案ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出此檢核ToolStripMenuItem;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Plot;
        private System.Windows.Forms.ToolStripMenuItem 步驟ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 參數設定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 開始檢核ToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage_AddBlock;
        private System.Windows.Forms.PropertyGrid propertyGrid_Block;
        private System.Windows.Forms.TabPage tabPage_RunCheck;
        private System.Windows.Forms.Label label_Show;
        private System.Windows.Forms.Button btn_Test;
        private System.Windows.Forms.Button btnRemoveSects;
        private System.Windows.Forms.Button btn_AddASect;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TabPage tabPage_BasicParameter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_Beta;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox_Lenda;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_Kd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox_Ks;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_T0;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Kr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox_Slope;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_HWL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_H0;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox_YO;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox_XO;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem 開啟舊的專案檔ToolStripMenuItem;
    }
}