using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace VE_SD
{
    public partial class Form_RDExamProgress : Form
    {
        #region 重要參數
        double xo = 0.0;
        double yo = 0.0;

        #endregion
        string selectname = null;

        public Form_RDExamProgress()
        {
            InitializeComponent();



            //chart_Plot.Series[1].Name = "Series1";

            //chart_Plot.Series.Add("Series2");
            //chart_Plot.Series[1].ChartType = SeriesChartType.Area; //.Line;
            //chart_Plot.Series[0].Points.DataBindXY(X2, Y2);
            //chart_Plot.Series[0].Color = Color.Red;

        }

        private void Form_RDExamProgress_Load(object sender, EventArgs e)
        {
            //double[] X1 = { 5, 20, 20, 5, 5 }; //,5,1 };
            //double[] Y1 = { 20, 20, 10, 10, 20 }; //,20,20 };
            //double[] X2 = { 5, 20 }; //,20, 5 };
            //double[] Y2 = { 10, 20 }; //, 20, 20 };

            //Tab 1.[公用參數設定區塊]初始化
            textBox_H0.Text = "13.22";
            textBox_T0.Text = "14.30";
            textBox_HWL.Text = "+2.44";
            textBox_Slope.Text = "0.025";
            textBox_Kr.Text = "0.810";
            textBox_Ks.Text = "1.117";
            textBox_Kd.Text = "1.000";
            textBox_Lenda.Text = "0.90";
            textBox_Beta.Text = "0";



            //Tab 2.[Block新增刪減區塊]初始化.
            chart_Plot.Series[0].ChartType = SeriesChartType.Range; //.Line;
            chart_Plot.Series[0].Points.Add(new DataPoint(5, new double[] { 20, 10 }));
            chart_Plot.Series[0].Points.Add(new DataPoint(5, new double[] { 10, 20 }));
            chart_Plot.Series[0].Points.Add(new DataPoint(10, new double[] { 10, 20 }));
            chart_Plot.Series[0].Points.Add(new DataPoint(10, new double[] { 20, 10 }));
            //chart_Plot.Series[0].Points.Add(new DataPoint(30, new double[] { 40, 20 }));
            chart_Plot.Series[0].BorderColor = Color.Black;
            chart_Plot.Series[0].Color = Color.LightGray;//= Color.Transparent;
            chart_Plot.Series[0].Name = "Block 1";


            chart_Plot.Series.Add("Block 2");
            chart_Plot.Series[1].ChartType = SeriesChartType.Range; //.Line;
            //chart_Plot.Series[1].Points.Add(new DataPoint(5, new double[] { 20, 10 })); //.DataBindXY(X1,Y1);
            chart_Plot.Series[1].Points.Add(new DataPoint(10, new double[] { 10, 0 }));
            chart_Plot.Series[1].Points.Add(new DataPoint(10, new double[] { 0, 10 }));
            chart_Plot.Series[1].Points.Add(new DataPoint(30, new double[] { 0, 10 }));
            chart_Plot.Series[1].Points.Add(new DataPoint(30, new double[] { 10, 0 }));
            chart_Plot.Series[1].BorderColor = Color.Black;
            chart_Plot.Series[1].Color = Color.LightGray;//= Color.Transparent;

            chart_Plot.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_Plot.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            開始檢核ToolStripMenuItem.Enabled = false;
            textBox_XO.Text = "0";
            textBox_YO.Text = "0";
            textBox_XO.ReadOnly = true;
            textBox_YO.ReadOnly = true;

            btnRemoveSects.Enabled = false;
            tsp_cond.Text = "請設定或編輯您的專案檔";
            tsp_progressbar.Visible = false;


            //PropertyGrid測試.
            //propertyGrid_Block.SelectedObject = new Class_Block_Interface();


        }

        #region Chart互動區
        private void chart_Plot_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ex = (System.Windows.Forms.MouseEventArgs)e;

            HitTestResult result = chart_Plot.HitTest(ex.X, ex.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                //MessageBox.Show(result.Series .Name );

                //Clear previous select
                if (selectname == null)
                {
                    //Do nothing.
                    chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    selectname = result.Series.Name;

                    //Listbox顯示變更.

                }
                else if (selectname == result.Series.Name)
                {
                    //Do nothing.
                }
                else if (selectname != result.Series.Name)
                {
                    //Clear previous select.
                    chart_Plot.Series[selectname].BorderColor = Color.Black;
                    chart_Plot.Series[selectname].BorderWidth = 1;
                    chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    selectname = result.Series.Name;

                    //Listbox顯示變更.

                }
            }
            else
            {
                if (selectname != null)
                {
                    //Clear previous selected.
                    chart_Plot.Series[selectname].BorderColor = Color.Black;
                    chart_Plot.Series[selectname].BorderWidth = 1;
                    selectname = null;

                    //不動.
                }
            }
        }
        #endregion 

        private void btn_Test_Click(object sender, EventArgs e)
        {
            //***********************************************************


            //***********************************************************
        }

        #region 力矩計算點變更
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox_XO.ReadOnly)
            {
                textBox_XO.ReadOnly = false;
                textBox_YO.ReadOnly = false;
            }
            else
            {
                //Writing into system inside variable.
                if (Double.TryParse(textBox_XO.Text, out xo) && double.TryParse(textBox_YO.Text, out yo))
                {
                    textBox_XO.ReadOnly = true;
                    textBox_YO.ReadOnly = true;
                    tsp_cond.Text = "已經成功設定力矩參考點為(" + xo.ToString() + "," + yo.ToString() + ")";
                }
                else
                {
                    MessageBox.Show("沒有轉換成功喔!!!!");
                }
            }
        }
        private void textBox_XO_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 58) & (int)e.KeyChar != 8 & (int)e.KeyChar != 45) //)
            {
                e.Handled = true;
            }
        }
        private void textBox_YO_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char key_char = e.KeyChar;
            //MessageBox.Show(((int)key_char).ToString());
            if (((int)e.KeyChar < 48 | (int)e.KeyChar > 58) & (int)e.KeyChar != 8 & (int)e.KeyChar != 45) //)
            {
                e.Handled = true;
            }
        }

        #endregion


    }
}
