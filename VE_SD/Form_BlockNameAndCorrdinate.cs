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
    public partial class Form_BlockNameAndCorrdinate : Form
    {
        Class_BlockSect II = null;
        Dictionary<string, int> DI = new Dictionary<string, int>();
        string oname = "";
        string nowname = "";
        public Form_BlockNameAndCorrdinate()
        {
            InitializeComponent();
        }
        private Form_RDExamProgress  RDExamMainForm = null;
        public Form_BlockNameAndCorrdinate(Form callingForm,Class_BlockSect IX,Dictionary<string,int> DIX)//Form callingForm)
        {
            //修改Block
            II = IX;
            RDExamMainForm = callingForm as Form_RDExamProgress ;
            DI = DIX;
            InitializeComponent();
        }
        public Form_BlockNameAndCorrdinate(Form callingForm,Dictionary<String,int> DIX)
        {
            //新創一個Block時.
            DI = DIX;
            RDExamMainForm = callingForm as Form_RDExamProgress;
            InitializeComponent();
        }

        private void Form_BlockNameAndCorrdinate_Load(object sender, EventArgs e)
        {
            //根據傳遞資料與否決定顯示內容.
            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chart1.Series[0].BorderWidth = 2;
            //chart1.Series[0].BorderColor = Color.Red;
            chart1.Series[0].Color = Color.Red;

            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.MinorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MinorGrid.Enabled = false;
            textBox_AddNumber.Text = "4";

            if (object.Equals(II,null) || II.座標點數==0)
            {
                oname = "";
                nowname = "";
                //如果II為空白.
                gp_AddNewName.Enabled = true;
                gp_AddCoordinate.Enabled = false;

                //chart1.ChartAreas[0].AxisX.Maximum = 5;
                //chart1.ChartAreas[0].AxisX.Minimum = 0;
                //chart1.ChartAreas[0].AxisX.Interval = 1;
                //chart1.ChartAreas[0].AxisY.Maximum = 5;
                //chart1.ChartAreas[0].AxisY.Minimum = 0;
                //chart1.ChartAreas[0].AxisY.Interval = 1;

                //InitializingListViewXY();
                dataGridView1.Rows.Clear();
                btn_SetName.Text = "新增";
                textBox_NameInput.Text = "";
                toolTip1.SetToolTip(btn_SetName, "新增新的名稱");
                btn_RemovePoints.Enabled = false;
                btn_Insert.Enabled = false;
                //btn_OK.Enabled = false;
            }
            else
            {
                //如果II不為空白,填入值.
                oname = II.名稱;
                nowname = II.名稱;
                gp_AddNewName.Enabled = true;
                gp_AddCoordinate.Enabled = true;

                //直接開始載入值.
                //InitializingListViewXY();
                dataGridView1.Rows.Clear();
                DataGridViewRowCollection rows = dataGridView1.Rows;              
                
                for (int i=0;i<II.座標點數;i++)
                {
                    rows.Add(new object[] { II.X[i].ToString(), II.Y[i].ToString() });
                }
                dataGridView1.CurrentCell = dataGridView1.Rows[II.座標點數 - 1].Cells[0];
                btn_Insert.Enabled = true;
                btn_RemovePoints.Enabled = true;
                btn_SetName.Text = "修改";
                textBox_NameInput.Text = II.名稱;
                toolTip1.SetToolTip(btn_SetName, "修改為新的名稱");
                textBox_AddNumber.Text = "3";
                gp_AddCoordinate.Enabled = true;
                gp_AddNewName.Enabled = true;
                DrawingChart();
                //btn_OK.Enabled = true;
            }
            
        }

        //private void InitializingListViewXY()
        //{
        //    listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        //    listView1.View = View.Details;
        //    listView1.GridLines = true;
        //    listView1.Clear();
        //    listView1.Columns.Clear();
        //    listView1.Columns.Add("Index", 40, HorizontalAlignment.Center);
        //    listView1.Columns.Add("x", 40, HorizontalAlignment.Center);
        //    listView1.Columns.Add("y", 40, HorizontalAlignment.Center);
        //    listView1.Refresh();
        //    autoResizeColumns(listView1);
        //}
        public static void autoResizeColumns(ListView lv)
        {
            lv.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            ListView.ColumnHeaderCollection cc = lv.Columns;
            for (int i = 0; i < cc.Count; i++)
            {
                int colWidth = TextRenderer.MeasureText(cc[i].Text, lv.Font).Width + 10;
                if (colWidth > cc[i].Width)
                {
                    cc[i].Width = colWidth;
                }
            }
        }

        #region 形塊名稱設定
        private void textBox_NameInput_TextChanged(object sender, EventArgs e)
        {
            if( textBox_NameInput.Text.Length>0)
            {
                if(檢查名稱是否重複(textBox_NameInput.Text))
                {
                    //重複.
                    errorProvider1.SetError(textBox_NameInput, "此名稱已重複");
                    textBox_NameInput.Focus();
                    btn_SetName.Enabled = false;
                }
                else
                {
                    //nowname = textBox_NameInput.Text;
                    errorProvider1.Clear();
                    btn_SetName.Enabled = true;
                }
            }
            else
            {
                errorProvider1.Clear();
                btn_SetName.Enabled = false;
            }
        }
        private void btn_SetName_Click(object sender, EventArgs e)
        {
            if(btn_SetName.Text=="修改" && !object.Equals(II, null))
            {
                if ( oname == textBox_NameInput.Text)
                {
                    //等於什麼都沒變.
                    return;
                }
                if(MessageBox.Show("您確定變更您的Block名稱嗎?","變更Block名稱",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel)
                {
                    return;
                }
            }
            nowname = textBox_NameInput.Text;
            if(nowname=="")
            {
                MessageBox.Show("不可輸入空白!!!!");
                nowname = oname;
                return;
            }
            if (gp_AddCoordinate.Enabled == false)
            {
                gp_AddCoordinate.Enabled = true;
            }
            btn_SetName.Text = "修改";
            toolTip1.SetToolTip(btn_SetName, "修改為新的名稱");
        }
        private bool 檢查名稱是否重複(string inS)
        {
            bool alreadyOld = DI.ContainsKey(oname);
            if(alreadyOld && oname==inS)
            {
                return false;//Not Repeated.
            }
            else
            {
                if(DI.ContainsKey(inS))
                {
                    return true;
                }
                else
                {
                    if(inS=="HWL" || inS.Substring(0,1)=="E" || inS=="ARROW")
                    {
                        return true;//Forbidden to use HWL or E start block name.
                    }
                    else
                    {
                        return false;
                    }
                }
                //return DI.ContainsKey(inS);
            }
        }

        #endregion

        #region 新增Block Coordinate區塊
        private void textBox_AddNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            char key_char = e.KeyChar;
            if (((int)key_char < 48 | (int)key_char > 58) & (int)key_char != 8 )
            {
                //e.Handled = true;
                e.Handled = true;
            }
        }
        private void btn_Add_Click(object sender, EventArgs e)
        {
            //新增新的點.
            if(textBox_AddNumber.Text=="")
            {
                return;
            }

            int CAdd;
            int.TryParse(textBox_AddNumber.Text, out CAdd);
            if(CAdd==0)
            {
                return;
            }
            if(dataGridView1.Rows.Count==0)
            {
                DataGridViewRowCollection rows = dataGridView1.Rows;
                for(int i=0;i<CAdd;i++)
                {
                    rows.Add(new object[] {"","" });
                    
                }
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                btn_Insert.Enabled = true;
                btn_RemovePoints.Enabled = true;
            }
            else
            {
                DataGridViewSelectedRowCollection sr = dataGridView1.SelectedRows;
                int leastrow;
                if (sr.Count==0)
                {
                    leastrow = dataGridView1.CurrentCell.RowIndex;
                }
                else
                {
                    leastrow = sr[sr.Count - 1].Cells[0].RowIndex;
                }
                
                for(int i=0;i< CAdd;i++)
                {
                    dataGridView1.Rows.Insert(leastrow + 1, new object[] { "", "" });
                }
                btn_Insert.Enabled = true;
                btn_RemovePoints.Enabled = true;
                
                //Insert from selected row.
                //MessageBox.Show("Opps");
            }
            //將焦點放回datagridview1上.
            dataGridView1.Focus();
            DrawingChart();
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //畫出編號.
            DataGridView grid = (DataGridView)sender;
            String Rowindex = (e.RowIndex + 1).ToString();
            System.Drawing.Font rowFont = new System.Drawing.Font("Tahoma", 8.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (Byte)1.0);
            StringFormat CentralFormat = new StringFormat();
            CentralFormat.Alignment = StringAlignment.Far;
            CentralFormat.LineAlignment = StringAlignment.Near;
            Rectangle HB = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(Rowindex, rowFont, SystemBrushes.ControlText, HB, CentralFormat);
            
        }
        private void btn_Insert_Click(object sender, EventArgs e)
        {
            //往前插入.
            int CAdd;
            if( textBox_AddNumber.Text=="")
            {
                CAdd = 1;
            }
            else
            {
                CAdd = -1;
                int.TryParse(textBox_AddNumber.Text, out CAdd);
                if (CAdd == -1) { return; }
            }

            int nowrow = dataGridView1.CurrentCell.RowIndex;
            for (int i = 0; i < CAdd; i++)
            {
                dataGridView1.Rows.Insert(nowrow, new object[] { "", "" });
            }
            DrawingChart();
        }
        private void btn_RemovePoints_Click(object sender, EventArgs e)
        {
            DataGridViewSelectedRowCollection sr = dataGridView1.SelectedRows;
            if(sr.Count==0)
            {
                int removerow = dataGridView1.CurrentCell.RowIndex;
                dataGridView1.Rows.RemoveAt(removerow);
            }
            else
            {
                for (int i = 0; i < sr.Count; i++)
                {
                    dataGridView1.Rows.RemoveAt(sr[i].Cells[0].RowIndex);
                }

            }
            if (dataGridView1.Rows.Count == 0)
            {
                btn_Insert.Enabled = false;
                btn_RemovePoints.Enabled = false;
            }
            DrawingChart();
        }

        //*********************************************************************************************
        //繪圖.
        private void DrawingChart()
        {
            //Collecting Current Datagridview data.
            double[] x=new double[] { };
            double[] y=new double[] { };
            int xsize = 0;
            int ysize = 0;
            double xtest;
            double ytest;
            bool success = false;
            //string ss;

            if(dataGridView1.Rows.Count==0)
            {
                try
                {
                    chart1.Series[0].Points.Clear();
                    //chart1.ChartAreas[0].AxisX.Maximum = 5;
                    //chart1.ChartAreas[0].AxisX.Minimum = 0;
                    //chart1.ChartAreas[0].AxisX.Interval = 1;
                    //chart1.ChartAreas[0].AxisY.Maximum = 5;
                    //chart1.ChartAreas[0].AxisY.Minimum = 0;
                    //chart1.ChartAreas[0].AxisY.Interval = 1;
                    //label_Show.Text = "沒有成功";
                    btn_OK.Enabled = false;
                }
                catch
                { }
                return;

            }

            DataGridViewRowCollection rows = dataGridView1.Rows;
            for(int i=0;i<dataGridView1.Rows.Count;i++)
            {
                try
                {
                    if (double.TryParse(dataGridView1.Rows[i].Cells[0].Value.ToString(), out xtest))
                    {

                    }
                    else
                    {
                        label_Show.Text = "失敗: X轉換失敗-->" + (i.ToString()) + ":" + dataGridView1.Rows[i].Cells[0].Value.ToString();
                    }
                }
                catch
                {
                    label_Show.Text = "失敗: X轉換失敗-->" + (i.ToString());
                }
                try
                {

                    if (double.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out ytest))
                    {

                    }
                    else
                    {
                        label_Show.Text = "失敗: Y轉換失敗-->" + (i.ToString()) + ":" + dataGridView1.Rows[i].Cells[1].Value.ToString();
                    }
                }
                catch
                {
                    label_Show.Text = "失敗: Y轉換失敗-->" + (i.ToString());
                }
                try
                {
                    if (double.TryParse(dataGridView1.Rows[i].Cells[0].Value.ToString(), out xtest) && double.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out ytest))
                    {
                        Array.Resize(ref x, xsize + 1);
                        x[xsize] = xtest;
                        Array.Resize(ref y, ysize + 1);
                        y[ysize] = ytest;
                        xsize += 1;
                        ysize += 1;
                        success = true;
                    }
                    else
                    {
                        //label_Show.Text = "失敗:" + (i.ToString());
                        //success = false;
                        break;
                    }
                }
                catch
                {
                    //success = false;
                    break;
                }
            }
            if(!success)
            {
                //Clear the drawing
                chart1.Series[0].Points.Clear();
                //chart1.ChartAreas[0].AxisX.Maximum = 5;
                //chart1.ChartAreas[0].AxisX.Minimum = 0;
                //chart1.ChartAreas[0].AxisX.Interval = 1;
                //chart1.ChartAreas[0].AxisY.Maximum = 5;
                //chart1.ChartAreas[0].AxisY.Minimum = 0;
                //chart1.ChartAreas[0].AxisY.Interval = 1;
                //label_Show.Text = "沒有成功";
                btn_OK.Enabled = false;
                
                return;
            }
            
            chart1.Series[0].Points.Clear();
            if(ysize==1)
            {
                //跳出.
                return;
            }
            for(int i=0;i< ysize;i++)
            {
                chart1.Series[0].Points.AddXY(x[i], y[i]);
            }
           if(!(x[xsize-1]==x[0] && y[ysize - 1] == y[0]))
            { chart1.Series[0].Points.AddXY(x[0], y[0]); }
            //label_Show.Text = "成功" + ysize.ToString();
            //chart1.ChartAreas[0].RecalculateAxesScale();

            double Xmin = 1000000; // double.MaxValue;
            double Xmax = -100000; // double.MinValue;
            double Ymin = Xmin;
            double Ymax = Xmax;
            foreach(DataPoint dp in chart1.Series[0].Points)
            {
                //label_Show.Text += (dp.XValue.ToString());
                if (dp.XValue>Xmax) { Xmax = dp.XValue;}
                if(dp.XValue < Xmin) { Xmin = dp.XValue; }
                if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                if(dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
            }
            //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
            double xdiff = (Xmax - Xmin);
            double ydiff = (Ymax - Ymin);
            double xspace, yspace;
           
            if(xdiff==0)
            {
                xdiff = 1.0;
            }
            if(ydiff==0)
            {
                ydiff = 1.0;
            }
            xspace = xdiff / 4.0;
            yspace = ydiff / 4.0;
            //if(xdiff<=1)
            //{
            //    xspace = 0.2;
            //}
            //else if(xdiff<=5)
            //{
            //    xspace = 1;
            //}
            //else if(xdiff<=10)
            //{
            //    xspace = 2;
            //}
            //else if(xdiff<=20)
            //{
            //    xspace = 5;
            //}
            //else if(xdiff<=50)
            //{
            //    xspace = 10;
            //}
            //else
            //{
            //    xspace = 100;
            //}
            //if (ydiff <= 1)
            //{
            //    yspace = 0.2;
            //}
            //else if (ydiff <= 5)
            //{
            //    yspace = 1;
            //}
            //else if (ydiff <= 10)
            //{
            //    yspace = 2;
            //}
            //else if (ydiff <= 20)
            //{
            //    yspace = 5;
            //}
            //else if (ydiff <= 50)
            //{
            //    yspace = 10;
            //}
            //else
            //{
            //    yspace = 100;
            //}
            double NewXmax = Xmax;// Xmin + Math.Ceiling((Xmax - Xmin) / xspace ) * xspace;
            double NewYmax = Ymax;// Ymin + Math.Ceiling((Ymax - Ymin) / yspace ) * yspace;
            label_Show.Text = Xmin.ToString() + "," + NewXmax.ToString(); // + ":" + NewYmax.ToString();

            chart1.ChartAreas[0].AxisX.Minimum = Xmin;// Xmin-xspace;
            chart1.ChartAreas[0].AxisX.Maximum = NewXmax;
            chart1.ChartAreas[0].AxisX.Interval = xspace;
            chart1.ChartAreas[0].AxisY.Minimum = Ymin;// Ymin-yspace;
            chart1.ChartAreas[0].AxisY.Maximum = NewYmax;
            chart1.ChartAreas[0].AxisY.Interval = yspace;
            chart1.ChartAreas[0].RecalculateAxesScale();

            btn_OK.Enabled = true; ;
        }
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("H");
            DrawingChart();
        }



        //********************************************************************************************
        #endregion

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            //將變數傳回.
            int 點數 = dataGridView1.Rows.Count;
            double[] xout = new double[] { };
            double[] yout = new double[] { };
            double xtest;
            double ytest;
            //Array.Resize(ref xout, 點數);
            //Array.Resize(ref yout, 點數);
            //string ss = "";
            for (int i=0;i<點數;i++)
            {
                double.TryParse(dataGridView1.Rows[i].Cells[0].Value.ToString(), out xtest);
                double.TryParse(dataGridView1.Rows[i].Cells[1].Value.ToString(), out ytest);
                if(i==點數-1 && xtest==xout[0] && ytest==yout[0])
                {
                    break;
                }
                Array.Resize(ref xout, i + 1);
                Array.Resize(ref yout, i + 1);
                xout[i] = xtest;
                yout[i] = ytest;
                //ss = ss + ("X = " + xout[i].ToString() + " , " + yout[i].ToString());
            }

            //檢查是否為凸邊形,且點位順序為逆時針.
            //不符合時,跳出警訊.
            if(!CheckIsConvexPolygonAndCounterClockWise(xout,yout))
            {
                //失敗.
                MessageBox.Show("注意，您所輸入的Polygon形狀限定為凸邊形且點位順序應為逆時針!!","Polygon設定失敗",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }


            //MessageBox.Show(ss);
            II = new Class_BlockSect();
            II.名稱 = nowname;// textBox_NameInput.Text;
            II.X = xout;
            II.Y = yout;
            II.座標點數 = xout.GetUpperBound(0) + 1;
            RDExamMainForm.BlockObj = II;
            this.Close();
        }
        public bool CheckIsConvexPolygonAndCounterClockWise(double[] XI, double[] YI)
        {
            //檢查是否是凸邊形.
            bool result = true;
            if(!(XI[XI.GetUpperBound(0)]==XI[0] && YI[YI.GetUpperBound(0)]==YI[0]))
            {
                Array.Resize(ref XI, XI.GetUpperBound(0) + 2);
                XI[XI.GetUpperBound(0)] = XI[0];
                Array.Resize(ref YI, YI.GetUpperBound(0) + 2);
                YI[YI.GetUpperBound(0)] = YI[0];


            }
            Array.Resize(ref XI, XI.GetUpperBound(0) + 2);
            XI[XI.GetUpperBound(0)] = XI[1];
            Array.Resize(ref YI, YI.GetUpperBound(0) + 2);
            YI[YI.GetUpperBound(0)] = YI[1];

            for (int i=1;i<XI.GetUpperBound(0)-1;i++)
            {
                double x_1 = XI[i - 1];
                double y_1 = YI[i - 1];
                double x0 = XI[i];
                double y0 = YI[i];
                double x1 = XI[i + 1];
                double y1 = YI[i + 1];

                double v1x = (x0 - x_1);
                double v1y = (y0 - y_1);

                double v2x = (x1 - x0);
                double v2y = (y1 - y0);

                double crossvalue = v1x * v2y - v1y * v2x;
                if(crossvalue<0)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count==0 || dataGridView1.SelectedRows.Count==1) { return; } //無選取或只有一列不做轉換.

            //判斷Datagridview上選取的Row，進行上下顛倒替換.
            double[] getx = new double[] { };
            double[] gety = new double[] { };
            int getsize = 0;

            int[] getindex = new int[] { };
            int getindexCount = 0;
            for(int i=0;i<dataGridView1.SelectedRows.Count;i++)
            {
                Array.Resize(ref getindex, getindexCount + 1);
                getindex[getindexCount] = dataGridView1.SelectedRows[i].Index;
                getindexCount += 1;
            }

            //蒐集資料.
            for(int i=0;i<=getindex.GetUpperBound(0);i++)
            {
                Array.Resize(ref getx, getsize + 1);
                Array.Resize(ref gety, getsize + 1);
                double.TryParse(dataGridView1.Rows[getindex[i]].Cells[0].Value.ToString(), out getx[getsize]);
                double.TryParse(dataGridView1.Rows[getindex[i]].Cells[1].Value.ToString(), out gety[getsize]);
                getsize += 1;
            }

            //進行逆轉程序.
            int ic = 0;
            for(int i=getindex.GetUpperBound(0);i>=0;i--)
            {
                int nowrow = getindex[i];
                dataGridView1.Rows[nowrow].Cells[0].Value = getx[ic];
                dataGridView1.Rows[nowrow].Cells[1].Value = gety[ic];
                ic += 1;
            }

            //替換完成.

        }

        private void dataGridView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show("H");
            //DrawingChart();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("H");
            //DrawingChart();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DrawingChart();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ex = (System.Windows.Forms.MouseEventArgs)e;

            HitTestResult result = chart1.HitTest(ex.X, ex.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                MessageBox.Show(result.Series.Name);

            }
        }

        private void gp_AddNewName_Enter(object sender, EventArgs e)
        {

        }
    }
}
