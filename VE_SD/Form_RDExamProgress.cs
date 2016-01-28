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
using System.IO;
using System.Xml;


namespace VE_SD
{
    public partial class Form_RDExamProgress : Form
    {
        #region 重要參數
        public double xo = 0.0;
        public double yo = 0.0;
        //public double H0 = 13.22;
        //public double T0 = 14.30;
        //public double HWL = 2.44;
        //public double Slope = 0.025;
        //public double Kr = 0.810;
        //public double Ks = 1.117;
        //public double Kd = 1000;
        //public double lambda = 0.9;
        //public double beta = 0;
        //public double SFSlide = 1.2;
        //public double SFOver = 1.2;

        private string 打開專案檔的名稱 = null;
        
        //以上為Global參數.

        public Class_BlockSect[] BlockMainArray=new Class_BlockSect[] { };// = new Class_BlockSect();
        int BlockCount = 0; //Block Array size
        public Class_BlockSect InterfaceBlock;
        private Dictionary<string, int> BlockNameToArraySubscript = new Dictionary<string, int>();//Block Name To Array Subscript.
        public Dictionary<string, int> BlockNameToListSubScript = new Dictionary<string, int>(); //Block Name To List Subscript.
        private Dictionary<int, String> BlockListSubScriptToName = new Dictionary<int, string>();//Block List Subscript to Name
        private Dictionary<int, String> BlockArraySubscriptToName = new Dictionary<int, string>();//Block Array Subscript to Name.

        public double[] ELArray = new double[] { };
        int ELSize = 0;


        #endregion
        string selectname = null;  //目前點選到的Block.
        Module1 Mod = null;

        public Form_RDExamProgress()
        {
            InitializeComponent();
            //chart_Plot.Series[1].Name = "Series1";

            //chart_Plot.Series.Add("Series2");
            //chart_Plot.Series[1].ChartType = SeriesChartType.Area; //.Line;
            //chart_Plot.Series[0].Points.DataBindXY(X2, Y2);
            //chart_Plot.Series[0].Color = Color.Red;

        }
        public Class_BlockSect BlockObj
        {
            get { return InterfaceBlock; }
            set { InterfaceBlock = value; }
        }

        private void Form_RDExamProgress_Load(object sender, EventArgs e)
        {
            //double[] X1 = { 5, 20, 20, 5, 5 }; //,5,1 };
            //double[] Y1 = { 20, 20, 10, 10, 20 }; //,20,20 };
            //double[] X2 = { 5, 20 }; //,20, 5 };
            //double[] Y2 = { 10, 20 }; //, 20, 20 };

            //Tab 1.[公用參數設定區塊]初始化
            cmb_seawaveDir.SelectedItem = "E";
            textBox_H0.Text = "13.22";
            textBox_T0.Text = "14.30";
            textBox_GroundELE.Text = "-7.5";
            textBox_ArmorBlockEle.Text = "5.66";
            textBox_HWL.Text = "+2.44";
            textBox_Slope.Text = "0.025";
            textBox_Kr.Text = "0.810";
            textBox_Ks.Text = "1.117";
            textBox_Kd.Text = "1.000";
            textBox_Lenda.Text = "0.90";
            textBox_Beta.Text = "0";
            textBox_SFSlide.Text = "1.2";
            textBox_SFOver.Text = "1.2";
            textBox_SeaGamma.Text = "1.03";

            chk_BlockWeightCalc.Checked = false;
            chk_HeadBreastCalc.Checked = false;

            label18.Enabled = false;
            label19.Enabled = false;
            label20.Enabled = false;
            label21.Enabled = false;
            label22.Enabled = false;
            label24.Enabled = false;

            textBox_KDL.ReadOnly = true;
            textBox_Sr.ReadOnly = true;
            textBox_Kt.ReadOnly = true;
            textBox_slopeangle.ReadOnly = true;
            textBox_KDL.Text = "";
            textBox_Sr.Text = "";
            textBox_Kt.Text = "";
            textBox_slopeangle.Text = "";
            textBox_ConcreteAllowStress.Text = "";
            textBox_BK.Text = "";
            textBox_KDL.Enabled = false;
            textBox_Sr.Enabled = false;
            textBox_Kt.Enabled = false;
            textBox_slopeangle.Enabled = false;
            textBox_ConcreteAllowStress.ReadOnly = true;
            textBox_BK.ReadOnly = true;
            textBox_ConcreteAllowStress.Enabled = false;
            textBox_BK.Enabled = false;




            chart_Plot.Series.Clear();
            //Tab 2.[Block新增刪減區塊]初始化.
            //chart_Plot.Series[0].ChartType = SeriesChartType.Range; //.Line;
            //chart_Plot.Series[0]["AreaDrawingtyle"] = "Polygon";
            //chart_Plot.Series[0].Points.Add(new DataPoint(0, new double[] { 0, 0 }));
            //chart_Plot.Series[0].Points.Add(new DataPoint(10, new double[] { 0,20 })); // 40, 20,10,0 }));
            //chart_Plot.Series[0].Points.Add(new DataPoint(10, new double[] { 0, 30 }));
            //chart_Plot.Series[0].Points.Add(new DataPoint(10, new double[] { 0, 40 }));
            //////chart_Plot.Series[0].Points.Add(new DataPoint(5, new double[] { 0, 10,20,40}));
            //chart_Plot.Series[0].BorderColor = Color.Black;
            //chart_Plot.Series[0].Color = Color.LightGray;//= Color.Transparent;
            //chart_Plot.Series[0].Name = "Block 1";


            //chart_Plot.Series.Add("Block 2");
            //chart_Plot.Series[1].ChartType = SeriesChartType.Range; //.Line;
            ////chart_Plot.Series[1].Points.Add(new DataPoint(5, new double[] { 20, 10 })); //.DataBindXY(X1,Y1);
            //chart_Plot.Series[1].Points.Add(new DataPoint(10, new double[] { 10, 0 }));
            //chart_Plot.Series[1].Points.Add(new DataPoint(10, new double[] { 0, 10 }));
            //chart_Plot.Series[1].Points.Add(new DataPoint(30, new double[] { 0, 10 }));
            //chart_Plot.Series[1].Points.Add(new DataPoint(30, new double[] { 10, 0 }));
            //chart_Plot.Series[1].BorderColor = Color.Black;
            //chart_Plot.Series[1].Color = Color.LightGray;//= Color.Transparent;
            
            //設定EL
            ELDGV1.Rows.Clear(); 

            chart_Plot.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_Plot.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            listBox_SectSetting.Items.Clear();
            Array.Resize(ref BlockMainArray, 0);
            BlockCount = 0;
            Array.Resize(ref ELArray, 0);
            ELSize = 0;
            InterfaceBlock = null;
            打開專案檔的名稱 = null;
            selectname = null;
            propertyGrid_Block.SelectedObject = null;
            btn_ModifiedBlock.Enabled = false;
            BlockArraySubscriptToName.Clear();
            BlockListSubScriptToName.Clear();
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();

            開始檢核ToolStripMenuItem.Enabled = false;
            textBox_XO.Text = "0";
            textBox_YO.Text = "0";
            xo = 0;
            yo = 0;
            textBox_XO.ReadOnly = true;
            textBox_YO.ReadOnly = true;

            btnRemoveSects.Enabled = false;
            tsp_cond.Text = "請設定或編輯您的專案檔";
            tsp_progressbar.Visible = false;


            this.Text = "專案檔:未命名";

            //PropertyGrid測試.[2015/12/14].
            //propertyGrid_Block.SelectedObject = new Class_Block_Interface();


        }

        #region Chart互動區
        private void chart_Plot_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ex = (System.Windows.Forms.MouseEventArgs)e;

            HitTestResult result = chart_Plot.HitTest(ex.X, ex.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                string hitname = result.Series.Name;
                //MessageBox.Show(result.Series .Name );

                //Clear previous select
                if (selectname == null)
                {
                    //Do nothing.
                    //chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    //chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    //selectname = result.Series.Name;

                    //chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    //chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    //Listbox顯示變更.
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E")
                    {
                        //Do nothing.
                        return;
                    }
                    else
                    {
                        listBox_SectSetting.SelectedIndex = BlockNameToListSubScript[result.Series.Name];
                    }
                }
                else if (selectname == result.Series.Name)
                {
                    //Do nothing.
                }
                else if (selectname != result.Series.Name)
                {
                    //Clear previous select.
                    //chart_Plot.Series[selectname].BorderColor = Color.Black;
                    //chart_Plot.Series[selectname].BorderWidth = 1;
                    //chart_Plot.Series[selectname].Color = Color.Black;
                    //chart_Plot.Series[selectname].MarkerBorderWidth = 1;

                    //chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    //chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    //chart_Plot.Series[result.Series.Name].Color = Color.Red;
                    //chart_Plot.Series[result.Series.Name].MarkerBorderWidth = 2;
                    //selectname = result.Series.Name;

                    //Listbox顯示變更.
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E")
                    {
                        //Do nothing.
                        return;
                    }
                    else
                    {
                        listBox_SectSetting.SelectedIndex = BlockNameToListSubScript[result.Series.Name];
                    }
                }
            }
            else
            {
                if (selectname != null)
                {
                    //Clear previous selected.
                    chart_Plot.Series[selectname].Color = Color.Black;
                    chart_Plot.Series[selectname].MarkerBorderWidth = 1;
                    selectname = null;
                    listBox_SectSetting.SelectedIndex = -1;
                    propertyGrid_Block.SelectedObject = null;
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

        private bool JudgeTheTextBoxHandle(TextBox tt, KeyPressEventArgs ei)
        {
            char key_char = ei.KeyChar;
            string nowtext = tt.Text;
            //tsp_cond.Text = textBox_H0.SelectionStart.ToString();
            if ((int)key_char == 46 && tt.SelectionStart==0) //textBox_H0.SelectionStart == 0)
            {
                //.
                //e.Handled = true;
                return true;
            }
            else if ((int)key_char == 45 && tt.SelectionStart!=0) //textBox_H0.SelectionStart != 0)
            {
                //-
                //e.Handled = true;
                return true;
            }
            else if ((int)key_char == 43 && tt.SelectionStart!=0) //textBox_H0.SelectionStart != 0)
            {
                //+
                //e.Handled = true;
                return true;
            }
            else if( (int)key_char==46 && nowtext.IndexOf(".")!=-1)
            {
                return true;
            }
            else if((int)key_char == 45 && nowtext.IndexOf("-") != -1)
            {
                return true;
            }
            else if ((int)key_char == 43 && nowtext.IndexOf("+") != -1)
            {
                return true;
            }
            else if (((int)key_char < 48 | (int)key_char > 58) & (int)key_char != 8 & (int)key_char != 43 && (int)key_char != 45 && (int)key_char != 46)
            {
                //e.Handled = true;
                return true;
            }
            else { return false; }
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
            //if (((int)e.KeyChar < 48 | (int)e.KeyChar > 58) & (int)e.KeyChar != 8 & (int)e.KeyChar != 45) //)
            // {
            //     e.Handled = true;
            // }
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender,e);
        }
        private void textBox_YO_KeyPress(object sender, KeyPressEventArgs e)
        {
            //char key_char = e.KeyChar;
            //MessageBox.Show(((int)key_char).ToString());
            //if (((int)e.KeyChar < 48 | (int)e.KeyChar > 58) & (int)e.KeyChar != 8 & (int)e.KeyChar != 45) //)
            // {
            //     e.Handled = true;
            // }
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        #endregion
        #region 設計條件Textbox輸入區域
        private void textBox_H0_KeyPress(object sender, KeyPressEventArgs e)
        {
            //限制只能輸入數字與小數點.
            //char key_char = e.KeyChar;
            //MessageBox.Show(((int)key_char).ToString());
            //char key_char = e.KeyChar;
            ////tsp_cond.Text = textBox_H0.SelectionStart.ToString();
            //if( (int)key_char==46 && textBox_H0.SelectionStart==0)
            //{
            //    //.
            //    e.Handled = true;
            //}
            //else if((int)key_char==45 && textBox_H0.SelectionStart!=0)
            //{
            //    //-
            //    e.Handled = true;
            //}
            //else if((int)key_char==43 && textBox_H0.SelectionStart !=0)
            //{
            //    //+
            //    e.Handled = true;
            //}
            //else if(((int)key_char < 48 | (int)key_char > 58) & (int)key_char != 8 & (int)key_char!=43 && (int)key_char!=45 && (int)key_char!=46)
            //{
            //    e.Handled = true;
            //}
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_T0_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HWL_KeyPress(object sender, KeyPressEventArgs e)
        {
            bool h = JudgeTheTextBoxHandle((TextBox)sender, e);
            if(!h)
            {
                繪上EL();//呼叫.
            }
            else
            {
                e.Handled = h;
            }

        }
        private void textBox_Slope_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Kr_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Ks_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Kd_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Lenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Beta_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_SFSlide_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_SFOver_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_SeaGamma_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        #endregion

        #region 形塊增加與刪除,點選之主要區域
        private void btn_AddASect_Click(object sender, EventArgs e)
        {
            //新增一個形塊.
            InterfaceBlock = null;
            Form_BlockNameAndCorrdinate form_blockNameAnsCoordinate = new Form_BlockNameAndCorrdinate(this,BlockNameToListSubScript);
            form_blockNameAnsCoordinate.ShowDialog();

            if(object.Equals(InterfaceBlock,null))
            {
                //Nothing
                return;
            }

            
            //設定成功.

            //MessageBox.Show("Here");
            //1. 將新的Block名稱新增到Dictionary上.
            BlockNameToListSubScript.Add(InterfaceBlock.名稱, listBox_SectSetting.Items.Count);
            BlockListSubScriptToName.Add(listBox_SectSetting.Items.Count, InterfaceBlock.名稱);

            //2. 新增到Listbox與Array上.
            BlockCount = listBox_SectSetting.Items.Count;
            listBox_SectSetting.Items.Add(InterfaceBlock.名稱);
            Array.Resize(ref BlockMainArray, BlockCount + 1);
            BlockMainArray[BlockCount] = InterfaceBlock;
            //此時參數都是預設值.
            BlockCount += 1;

            //3. 加上新的Block到繪圖視窗內.
            AddNewChart(InterfaceBlock);
            //MessageBox.Show("HHH");

            //selectname = InterfaceBlock.名稱;
            //MessageBox.Show("H4");
            //4. 更新目前選擇的.
            listBox_SectSetting.SetSelected(BlockCount-1,true); //設定Listbox點選項目.

            開始檢核ToolStripMenuItem.Enabled = true;
            //btnRemoveSects.Enabled = true;
            //btn_ModifiedBlock.Enabled = true;
            InterfaceBlock = null;
            //5. 顯示Property Grid預設參數.
            //propertyGrid_Block.SelectedObject = new Class_Block_Interface();
        }
        //Listbox點選變更時的設定
        private void listBox_SectSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox_SectSetting.SelectedIndex!=-1)
            {
                ShowCurrentBlockInformation();
                btn_ModifiedBlock.Enabled = true;
                btnRemoveSects.Enabled = true;
            }
            else
            {
                if (!object.Equals(selectname, null))
                {
                    chart_Plot.Series[selectname].Color = Color.Black;
                    chart_Plot.Series[selectname].MarkerBorderWidth = 1;
                    selectname = null;
                }
                //listBox_SectSetting.SelectedIndex = -1;
                propertyGrid_Block.SelectedObject = null;
                btn_ModifiedBlock.Enabled = false;
                btnRemoveSects.Enabled = false;
            }
        }
        private void ShowCurrentBlockInformation()
        {
            //1.更換Chart 繪製與加入不在這個區塊.
            if(!object.Equals(selectname, null))
            {
                chart_Plot.Series[selectname].BorderColor = Color.Black;
                chart_Plot.Series[selectname].BorderWidth = 1;
                chart_Plot.Series[selectname].Color = Color.Black;
                chart_Plot.Series[selectname].MarkerBorderWidth = 1;
            }
            string nowname = BlockListSubScriptToName[listBox_SectSetting.SelectedIndex];
            //MessageBox.Show(nowname);

            chart_Plot.Series[nowname].BorderColor = Color.Red;
            chart_Plot.Series[nowname].BorderWidth = 2;
            chart_Plot.Series[nowname].Color = Color.Red;
            chart_Plot.Series[nowname].MarkerBorderWidth = 2;
            selectname = nowname;

            //2.Property Grid更換.
            propertyGrid_Block.SelectedObject = new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
        }
        private struct Linkage
        {
            public double xb1;
            public double xb2;
            public double yb1;
            public double yb2;
            public int Preid;
            public int Nextid;
        }
        private Linkage[] CreateLinks(double[] xi,double[] yi)
        {
            Linkage[] LL = new Linkage[] { };
            int LinkageCount = 0;

            if(xi.GetUpperBound(0)!=yi.GetUpperBound(0))
            {
                return null;
            }
            else
            {
                for(int i=0;i<xi.GetUpperBound(0);i++)
                {
                    Array.Resize(ref LL, LinkageCount + 1);
                    LL[LinkageCount].xb1 = xi[i];
                    LL[LinkageCount].xb2 = xi[i + 1];
                    LL[LinkageCount].yb1 = yi[i];
                    LL[LinkageCount].yb2 = yi[i + 1];
                    LL[LinkageCount].Preid = i;
                    LL[LinkageCount].Nextid = i + 1;
                    LinkageCount += 1;
                }
                if(!(xi[xi.GetUpperBound(0)]==xi[0] && yi[yi.GetUpperBound(0)]==yi[0]))
                {
                    Array.Resize(ref LL, LinkageCount + 1);
                    LL[LinkageCount].xb1 = xi[xi.GetUpperBound(0)];
                    LL[LinkageCount].xb2 = xi[0];
                    LL[LinkageCount].yb1 = yi[yi.GetUpperBound(0)];
                    LL[LinkageCount].yb2 = yi[0];
                    LL[LinkageCount].Preid = yi.GetUpperBound(0);
                    LL[LinkageCount].Nextid =0;
                    LinkageCount += 1;
                }
                return LL;
            }

        }
        private void AddNewChart(Class_BlockSect NewI)
        {
            //新增一個新的Chart進來.
            double[] getx = NewI.X;
            double[] gety = NewI.Y;
            int PointCount = NewI.座標點數;
            ////準備加入新的Chart.
            ////MessageBox.Show("H4");
            chart_Plot.Series.Add(NewI.名稱);
            chart_Plot.Series[NewI.名稱].ChartType = SeriesChartType.Line;
            //chart_Plot.Series[NewI.名稱]["AreaDrawingtyle"] = "Polygon";
            ////加入DataPoint
            //string ss = "";
            //for(int i=0;i< dpxsize;i++)
            //{
            //    chart_Plot.Series[NewI.名稱].Points.Add(new DataPoint(dpx[i], new double[] { newyu[i], newyb[i] }));
            //    ss = ss + ("X= " + dpx[i].ToString() + ", Y = (" + newyb[i].ToString() + " , " + newyu[i].ToString() + ");");
            //}
            //MessageBox.Show(ss);

            //完成.
            for(int i=0;i<PointCount;i++)
            {
                //
                chart_Plot.Series[NewI.名稱].Points.AddXY(getx[i], gety[i]);
            }
            if(!(getx[getx.GetUpperBound(0)]==getx[0] && gety[gety.GetUpperBound(0)]==gety[0]))
            {
                chart_Plot.Series[NewI.名稱].Points.AddXY(getx[0], gety[0]);
            }

            chart_Plot.Series[NewI.名稱].BorderColor = Color.Black ;
            chart_Plot.Series[NewI.名稱].Color = Color.Black;//= Color.Transparent;
            chart_Plot.Series[NewI.名稱].MarkerBorderWidth = 1;

            調整Chart(chart_Plot);
            
            繪上EL();
        }
        #endregion

        #region Property參數值變更

        private void propertyGrid_Block_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            updateCurrentBlock();
            //MessageBox.Show(e.ChangedItem.ToString());
        }
        private void updateCurrentBlock()
        {
            if(object.Equals(selectname,null))
            {
                return;
            }
            Class_Block_Interface D = (Class_Block_Interface)propertyGrid_Block.SelectedObject;

            int id = BlockNameToListSubScript[selectname];
            //BlockMainArray[id].場注土方塊與拋石摩擦係數 = D.場注土方塊與拋石;
            //BlockMainArray[id].拋石水中單位體積重量 = D.拋石水中;
            //BlockMainArray[id].拋石與拋石摩擦係數 = D.拋石與拋石;
            //BlockMainArray[id].拋石陸上單位體積重量 = D.拋石陸上;
            //BlockMainArray[id].海水單位體積重量 = D.海水;
            //BlockMainArray[id].混凝土方塊與拋石摩擦係數 = D.混凝土方塊與拋石;
            //BlockMainArray[id].混凝土方塊與方塊摩擦係數 = D.混凝土方塊與方塊;
            //BlockMainArray[id].混凝土水中單位體積重量 = D.混凝土水中;
            //BlockMainArray[id].混凝土陸上單位體積重量 = D.混凝土陸上;
            //BlockMainArray[id].砂土水中單位體積重量 = D.砂土水中;

            BlockMainArray[id].單位體積重量 = D.單位體積重量;
            BlockMainArray[id].使用材質 = D.使用材質;
        }
        //刪除Block
        private void btnRemoveSects_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("您確定要刪除此Block嗎?","刪除Block",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel)
            {
                return;
            }



            //變更相關的Dict.
            string oldname = selectname;
            int position = BlockNameToListSubScript[oldname];
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();
            BlockListSubScriptToName.Clear();
            BlockArraySubscriptToName.Clear();
            //從Array中刪除Block.
            RemoveAt(ref BlockMainArray, position);


            //從Chart中移除.
            chart_Plot.Series.Remove(chart_Plot.Series[oldname]);
            //try
            //{
            //    BlockNameToListSubScript.Remove(selectname);
            //}
            //catch { }
            //try
            //{
            //    BlockNameToArraySubscript.Remove(selectname);
            //}
            //catch { }
            //try
            //{
            //    BlockListSubScriptToName.Remove(position);
            //}
            //catch { }
            //try
            //{
            //    BlockArraySubscriptToName.Remove(position);
            //}
            //catch { }

            //刪除Listbox的Item.
            selectname = null;
            listBox_SectSetting.Items.RemoveAt(position);//觸動.
            listBox_SectSetting.SelectedIndex = -1;
            //更新Position開始之後的Index.
            for (int i=0;i<listBox_SectSetting.Items.Count;i++)
            {
                BlockArraySubscriptToName[i] = listBox_SectSetting.Items[i].ToString();
                BlockListSubScriptToName[i] = listBox_SectSetting.Items[i].ToString();
                BlockNameToArraySubscript[listBox_SectSetting.Items[i].ToString()] = i;
                BlockNameToListSubScript[listBox_SectSetting.Items[i].ToString()] = i;
            }
            //
            
            //selectname = null; -->這個已經被包含到觸動函數去了.
            if(listBox_SectSetting.Items.Count==0)
            {
                //btnRemoveSects.Enabled = false;
                開始檢核ToolStripMenuItem.Enabled = false;
            }
            BlockCount = listBox_SectSetting.Items.Count;
        }
        private void 調整Chart(Chart INS)
        {
            double Xmin = 1000000; // double.MaxValue;
            double Xmax = -100000; // double.MinValue;
            double Ymin = Xmin;
            double Ymax = Xmax;
            foreach (Series ss in INS.Series)
            {
                foreach (DataPoint dp in ss.Points)
                {
                    //label_Show.Text += (dp.XValue.ToString());
                    if (dp.XValue > Xmax) { Xmax = dp.XValue; }
                    if (dp.XValue < Xmin) { Xmin = dp.XValue; }
                    if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                    if (dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
                }
            }

            //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
            double xdiff = (Xmax - Xmin);
            double ydiff = (Ymax - Ymin);
            double xspace, yspace;
            if (xdiff <= 1)
            {
                xspace = 0.2;
            }
            else if (xdiff <= 5)
            {
                xspace = 1;
            }
            else if (xdiff <= 10)
            {
                xspace = 2;
            }
            else if (xdiff <= 20)
            {
                xspace = 5;
            }
            else if (xdiff <= 50)
            {
                xspace = 10;
            }
            else
            {
                xspace = 100;
            }
            if (ydiff <= 1)
            {
                yspace = 0.2;
            }
            else if (ydiff <= 5)
            {
                yspace = 1;
            }
            else if (ydiff <= 10)
            {
                yspace = 2;
            }
            else if (ydiff <= 20)
            {
                yspace = 5;
            }
            else if (ydiff <= 50)
            {
                yspace = 10;
            }
            else
            {
                yspace = 100;
            }
            double NewXmax = Xmin + Math.Floor((Xmax - Xmin) / xspace + 0.5) * xspace;
            double NewYmax = Ymin + Math.Floor((Ymax - Ymin) / yspace + 0.5) * yspace;
            label_Show.Text = Xmin.ToString() + "," + NewXmax.ToString(); // + ":" + NewYmax.ToString();

            INS.ChartAreas[0].AxisX.Minimum = Xmin - xspace;
            INS.ChartAreas[0].AxisX.Maximum = NewXmax;
            INS.ChartAreas[0].AxisX.Interval = xspace;
            INS.ChartAreas[0].AxisY.Minimum = Ymin - yspace;
            INS.ChartAreas[0].AxisY.Maximum = NewYmax;
            INS.ChartAreas[0].AxisY.Interval = yspace;
            INS.ChartAreas[0].RecalculateAxesScale();


            //繪上EL();

        }
        void 繪上EL()
        {
            //若有EL,則調整之.
            if (BlockCount == 0)
            {
                //MessageBox.Show("Block Count =0");
                //清除.
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["HWL"]);
                }
                catch
                {
                    //Do nothing.
                }
                for(int i=0;i<ELSize;i++)
                {
                   try
                    {
                        chart_Plot.Series.Remove(chart_Plot.Series["E" + (i + 1).ToString()]);
                    }
                    catch
                    {
                        //nothing.
                    }
                }
            }
            else
            {
                //MessageBox.Show("有Block喔");
                //MessageBox.Show("OO");
                //有Data.
                double xmin = chart_Plot.ChartAreas[0].AxisX.Minimum;
                double xmax = chart_Plot.ChartAreas[0].AxisX.Maximum;
                double HWLValue;
                bool hasHWLValue;
                if(double.TryParse(textBox_HWL.Text,out HWLValue)) //可以順利轉換HWL.
                {
                    //刪除舊的HWL,直接新增新的HWL.
                    hasHWLValue = true;
                    try
                    {
                        chart_Plot.Series.Remove(chart_Plot.Series["HWL"]);
                    }
                    catch
                    { }
                    //加入
                    chart_Plot.Series.Add("HWL");
                    chart_Plot.Series["HWL"].ChartType = SeriesChartType.Line;

                    //加入點資料.
                    chart_Plot.Series["HWL"].Points.AddXY(xmin,HWLValue);
                    chart_Plot.Series["HWL"].Points.AddXY(xmax, HWLValue);

                    //設定線段.
                     chart_Plot.Series["HWL"].BorderDashStyle=ChartDashStyle.Dash;
                     chart_Plot.Series["HWL"].BorderColor = Color.DarkGray;
                     chart_Plot.Series["HWL"].Color = Color.DarkGray;
                     chart_Plot.Series["HWL"].BorderWidth = 2;
                     chart_Plot.Series["HWL"].IsVisibleInLegend = true; // false;
                     //完成.
                }
                else
                {
                    hasHWLValue = false;
                    //刪除.
                    try
                    {
                        chart_Plot.Series.Remove(chart_Plot.Series["HWL"]);
                    }
                    catch
                    {
                        //Nothing.
                    }
                }
                //2. 繪上其餘EL線(與HWL線相同值者跳過).
                //先刪除舊的,在全部更新為新的.
                for(int i=0;i<ELSize;i++)
                {
                    try
                    {
                        chart_Plot.Series.Remove(chart_Plot.Series["E" + (i + 1).ToString()]);
                    }
                    catch
                    {
                        //nothing.
                    }
                }
                //從Datagrid蒐集可成功轉換的資料.
                Array.Resize(ref ELArray, 0);
                ELSize = 0;
                //MessageBox.Show("OO2-1: " + ELDGV1.Rows.Count.ToString());
                //由於EL的DG可以被編輯,因此若只有1個時,要再多加一個.
                for(int i=0;i<ELDGV1.Rows.Count-1;i++)
                {
                    double itest;
                    if(double.TryParse(ELDGV1.Rows[i].Cells[0].Value.ToString(),out itest))
                    {
                        if(hasHWLValue)
                        {
                            if(itest!=HWLValue)
                            {
                                bool repeated = false;
                                for(int i2=0;i2<ELSize;i2++)
                                {
                                    if(itest==ELArray[i2])
                                    { repeated = true; break; }
                                }
                                if (!repeated)
                                {
                                    Array.Resize(ref ELArray, ELSize + 1);
                                    ELArray[ELSize] = itest;
                                    ELSize += 1;
                                }
                            }
                        }
                        else
                        {
                            bool repeated = false;
                            for (int i2 = 0; i2 < ELSize; i2++)
                            {
                                if (itest == ELArray[i2])
                                { repeated = true; break; }
                            }
                            if (!repeated)
                            {
                                Array.Resize(ref ELArray, ELSize + 1);
                                ELArray[ELSize] = itest;
                                ELSize += 1;
                            }
                        }
                    }
                }//完成Datagrid的蒐集.

                //製作對應的EL線段.
                if(ELSize>0)
                {
                    //MessageBox.Show("HH");
                    for(int i=0;i<ELSize;i++)
                    {
                        string tpname = "E" + (i + 1).ToString();
                        chart_Plot.Series.Add(tpname);
                        chart_Plot.Series[tpname].ChartType = SeriesChartType.Line;

                        //加入點資料.
                        chart_Plot.Series[tpname].Points.AddXY(xmin, ELArray[i]);
                        chart_Plot.Series[tpname].Points.AddXY(xmax, ELArray[i]);

                        //設定線段.
                        chart_Plot.Series[tpname].BorderDashStyle = ChartDashStyle.DashDot ;
                        chart_Plot.Series[tpname].BorderColor = Color.LightSteelBlue ;
                        chart_Plot.Series[tpname].Color = Color.LightGray;
                        chart_Plot.Series[tpname].BorderWidth = 2;
                        chart_Plot.Series[tpname].IsVisibleInLegend = false;
                    }
                }//若有可用之EL時.
                //MessageBox.Show("OO3");
                double Ymin = 1000000;
                double Ymax =-Ymin;
                foreach (Series ss in chart_Plot.Series)
                {
                    foreach (DataPoint dp in ss.Points)
                    {
                        //label_Show.Text += (dp.XValue.ToString());
                        //if (dp.XValue > Xmax) { Xmax = dp.XValue; }
                        //if (dp.XValue < Xmin) { Xmin = dp.XValue; }
                        if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                        if (dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
                    }
                }

                //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
                //double xdiff = (Xmax - Xmin);
                double ydiff = (Ymax - Ymin);
                double  yspace;
                if (ydiff <= 1)
                {
                    yspace = 0.2;
                }
                else if (ydiff <= 5)
                {
                    yspace = 1;
                }
                else if (ydiff <= 10)
                {
                    yspace = 2;
                }
                else if (ydiff <= 20)
                {
                    yspace = 5;
                }
                else if (ydiff <= 50)
                {
                    yspace = 10;
                }
                else
                {
                    yspace = 100;
                }
                //double NewXmax = Xmin + Math.Floor((Xmax - Xmin) / xspace + 0.5) * xspace;
                ///MessageBox.Show(Ymax.ToString());
                double NewYmax = Ymin + Math.Ceiling((Ymax - Ymin) / yspace) * yspace;
                chart_Plot.ChartAreas[0].AxisY.Minimum = Ymin - yspace;
                chart_Plot.ChartAreas[0].AxisY.Maximum = NewYmax;
                chart_Plot.ChartAreas[0].AxisY.Interval = yspace;
                chart_Plot.ChartAreas[0].RecalculateAxesScale();
            }

        }
        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                arr[a] = arr[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref arr, arr.Length - 1);
        }
        private void btn_ModifiedBlock_Click(object sender, EventArgs e)
        {
            if(object.Equals(selectname,null))
            {
                return;
            }

            //點按修改此Block按鈕之後.
            InterfaceBlock = null;
            Form_BlockNameAndCorrdinate form_blockNameAnsCoordinate = new Form_BlockNameAndCorrdinate(this,BlockMainArray[BlockNameToListSubScript[selectname]], BlockNameToListSubScript);
            form_blockNameAnsCoordinate.ShowDialog();


            if (object.Equals(InterfaceBlock, null))
            {
                //Nothing
                return;
            }

            //變更Chart的名稱
            int oldpos = BlockNameToListSubScript[selectname];
            if(InterfaceBlock.名稱!=selectname)
            {
                chart_Plot.Series[selectname].Name = InterfaceBlock.名稱;
                BlockNameToArraySubscript.Add(InterfaceBlock.名稱, oldpos);
                BlockNameToListSubScript.Add(InterfaceBlock.名稱, oldpos);
                BlockNameToArraySubscript.Remove(selectname);
                BlockNameToListSubScript.Remove(selectname);
                BlockListSubScriptToName[oldpos] = InterfaceBlock.名稱;
                BlockArraySubscriptToName[oldpos] = InterfaceBlock.名稱;
                //MessageBox.Show("H1");
            }
            //不可直接等於,InterfaceBlock的參數是預設值.
            BlockMainArray[oldpos].X = InterfaceBlock.X;
            BlockMainArray[oldpos].Y = InterfaceBlock.Y;
            BlockMainArray[oldpos].座標點數 = InterfaceBlock.座標點數;
            BlockMainArray[oldpos].名稱 = InterfaceBlock.名稱;
            //變更Chart屬性.
            int PointCount = InterfaceBlock.座標點數;
            double[] getx = InterfaceBlock.X;
            double[] gety = InterfaceBlock.Y;
            string NewName= InterfaceBlock.名稱;

            selectname = NewName;

            chart_Plot.Series[NewName].Points.Clear();
            for (int i = 0; i < PointCount; i++)
            {
                chart_Plot.Series[NewName].Points.AddXY(getx[i], gety[i]);
            }
            if (!(getx[getx.GetUpperBound(0)] == getx[0] && gety[gety.GetUpperBound(0)] == gety[0]))
            {
                chart_Plot.Series[NewName].Points.AddXY(getx[0], gety[0]);
            }

            chart_Plot.Series[NewName].BorderColor = Color.Red;
            chart_Plot.Series[NewName].Color = Color.Red;//= Color.Transparent;
            chart_Plot.Series[NewName].MarkerBorderWidth = 2;
            調整Chart(chart_Plot);
            繪上EL();
 

            //修改Listbox.
            listBox_SectSetting.Items[oldpos] = InterfaceBlock.名稱;
        }
         
        #endregion



        #region 利用XML儲存此專案,開一個新的,開啟舊的與另存新的.
        private void 儲存此專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //撰寫XML檔案格式.
            //String workfoldernow = Directory.GetCurrentDirectory();
            //SaveFileDialog開啟.
            //若為開一個新的,則彈出對話框.
            if (BlockMainArray.GetLength(0) == 0)
            { MessageBox.Show("您沒有設定任何形塊!無法儲存", "專案檔管理", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string xmlpath;// = workfoldernow + "\\Test.xml";
            if (object.Equals(打開專案檔的名稱, null))
            {
                if (SFD_專案.ShowDialog() == DialogResult.OK && SFD_專案.FileName!="")
                {
                    xmlpath = SFD_專案.FileName;
                }
                else
                {
                    return;
                }
                 //路徑.
            }
            else
            {
                xmlpath = 打開專案檔的名稱;
            }

            if (!CheckTextBoxNoEmpty())
            {
                return;
            }
            儲存XML專案檔(xmlpath);
        }
        private void 儲存XML專案檔(string xmlfullpath)
        {
            //        
            if (!CheckTextBoxNoEmpty())
            {
                return;
            }


            XmlDocument doc = new XmlDocument();

            //*************************************************************************************
            //1. 基本姿態撰寫.
            //
            XmlElement Root = doc.CreateElement("Root");
            doc.AppendChild(Root);


            XmlElement 全域參數XML點 = doc.CreateElement("GlobalParameters");
            Root.AppendChild(全域參數XML點);
            //MessageBox.Show("H1");

            //*************************************************************************************
            //2. 全域參數設定區塊
            //建立子節點
            XmlElement 深海波波向info = doc.CreateElement("深海波波向");
            深海波波向info.SetAttribute("Value",cmb_seawaveDir.SelectedItem.ToString());
            //MessageBox.Show("H1-1");

            XmlElement 深海波波高info = doc.CreateElement("深海波波高");
            深海波波高info.SetAttribute("Value", textBox_H0.Text);

            XmlElement 深海波週期info = doc.CreateElement("深海波週期");
            深海波週期info.SetAttribute("Value", textBox_T0.Text);

            XmlElement 地面線info = doc.CreateElement("地面線");
            地面線info.SetAttribute("Value", textBox_GroundELE.Text);

            XmlElement 消波塊高程 = doc.CreateElement("消波塊高程");
            消波塊高程.SetAttribute("Value", textBox_ArmorBlockEle.Text);

            XmlElement 設計潮位 = doc.CreateElement("設計潮位");
            設計潮位.SetAttribute("Value", textBox_HWL.Text );

            XmlElement 海床坡度 = doc.CreateElement("海床坡度");
            海床坡度.SetAttribute("Value", textBox_Slope.Text);

            XmlElement 折射係數 = doc.CreateElement("折射係數");
            折射係數.SetAttribute("Value", textBox_Kr.Text);

            XmlElement 淺化係數 = doc.CreateElement("淺化係數");
            淺化係數.SetAttribute("Value", textBox_Ks.Text);

            XmlElement 繞射係數 = doc.CreateElement("繞射係數");
            繞射係數.SetAttribute("Value", textBox_Kd.Text );

            XmlElement 波力折減係數 = doc.CreateElement("波力折減係數");
            波力折減係數.SetAttribute("Value", textBox_Lenda.Text);

            XmlElement 入射波與堤體法線垂直交角 = doc.CreateElement("入射波與堤體法線垂直交角");
            入射波與堤體法線垂直交角.SetAttribute("Value", textBox_Beta.Text);

            XmlElement 滑動安全係數 = doc.CreateElement("滑動安全係數");
            滑動安全係數.SetAttribute("Value", textBox_SFSlide.Text);

            XmlElement 傾倒安全係數 = doc.CreateElement("傾倒安全係數");
            傾倒安全係數.SetAttribute("Value", textBox_SFOver.Text);

            XmlElement 海水單位體積重量 = doc.CreateElement("海水單位體積重量");
            海水單位體積重量.SetAttribute("Value", textBox_SeaGamma.Text);

            XmlElement 消波工重量檢核啟用 = doc.CreateElement("消波工重量檢核啟用");
            消波工重量檢核啟用.SetAttribute("Value", chk_BlockWeightCalc.Checked?"TRUE":"FALSE");

            XmlElement 胸牆部安定檢核啟用 = doc.CreateElement("胸牆部安定檢核啟用");
            胸牆部安定檢核啟用.SetAttribute("Value", chk_HeadBreastCalc.Checked ? "TRUE" : "FALSE");


            XmlElement 力矩計算參考點 = doc.CreateElement("力矩計算參考點");
            力矩計算參考點.SetAttribute("xValue", xo.ToString());
            力矩計算參考點.SetAttribute("yValue", yo.ToString());


            //加入所有參數節點到Global Parameters節點上
            全域參數XML點.AppendChild(深海波波向info);
            全域參數XML點.AppendChild(深海波波高info);
            全域參數XML點.AppendChild(深海波週期info);
            全域參數XML點.AppendChild(地面線info);
            全域參數XML點.AppendChild(消波塊高程);
            全域參數XML點.AppendChild(設計潮位);
            全域參數XML點.AppendChild(海床坡度);
            全域參數XML點.AppendChild(折射係數);
            全域參數XML點.AppendChild(淺化係數);
            全域參數XML點.AppendChild(繞射係數);
            全域參數XML點.AppendChild(波力折減係數);
            全域參數XML點.AppendChild(入射波與堤體法線垂直交角);
            全域參數XML點.AppendChild(滑動安全係數);
            全域參數XML點.AppendChild(傾倒安全係數);
            全域參數XML點.AppendChild(海水單位體積重量);
            全域參數XML點.AppendChild(消波工重量檢核啟用);
            if(chk_BlockWeightCalc.Checked)
            {
                XmlElement 消波形塊安定係數 = doc.CreateElement("消波形塊安定係數");
                消波形塊安定係數.SetAttribute("Value", textBox_KDL.Text);

                XmlElement 混凝土與海水之比重 = doc.CreateElement("混凝土與海水之比重");
                混凝土與海水之比重.SetAttribute("Value", textBox_Sr.Text);

                XmlElement 消波塊斜坡面與水平面之夾角 = doc.CreateElement("消波塊斜坡面與水平面之夾角");
                消波塊斜坡面與水平面之夾角.SetAttribute("Value", textBox_slopeangle.Text);

                XmlElement 波高傳遞率 = doc.CreateElement("波高傳遞率");
                波高傳遞率.SetAttribute("Value", textBox_Kt.Text);
                全域參數XML點.AppendChild(消波形塊安定係數);
                全域參數XML點.AppendChild(混凝土與海水之比重);
                全域參數XML點.AppendChild(消波塊斜坡面與水平面之夾角);
                全域參數XML點.AppendChild(波高傳遞率);
            }
            全域參數XML點.AppendChild(胸牆部安定檢核啟用);
            if (chk_HeadBreastCalc.Checked)
            {
                XmlElement 混凝土容許應力 = doc.CreateElement("混凝土容許應力");
                混凝土容許應力.SetAttribute("Value", textBox_ConcreteAllowStress.Text);

                XmlElement BK = doc.CreateElement("BK");
                BK.SetAttribute("Value", textBox_BK.Text);

                全域參數XML點.AppendChild(混凝土容許應力);
                全域參數XML點.AppendChild(BK);
            }
            全域參數XML點.AppendChild(力矩計算參考點);
            //EL[2016/01/24新增].
            XmlElement EL點數 = doc.CreateElement("EL點數");
            //EL點數.SetAttribute("Value", ELSize.ToString());
            全域參數XML點.AppendChild(EL點數);
            for (int i=0;i<ELSize;i++)
            {
                XmlElement ELI = doc.CreateElement("EL");// + (i + 1).ToString());
                ELI.SetAttribute("Value", ELArray[i].ToString());
                EL點數.AppendChild(ELI);
            }
            

            //MessageBox.Show("H2");

            //*************************************************************************************
            //3. Block區塊設定.
            XmlElement Block區塊最上層點 = doc.CreateElement("Blocks");
            Root.AppendChild(Block區塊最上層點);

            
            //MessageBox.Show("H3");

            //根據BlockMainArray將資訊填入.
            for (int i=0;i<=BlockMainArray.GetUpperBound(0);i++)
            {
                //每個Block都是一個新的子節點，內涵其他參數.

                XmlElement  BlockNode = doc.CreateElement("形塊");//統一的物件名稱.
                //MessageBox.Show("H4-1-1");
                BlockNode.SetAttribute("名稱", BlockListSubScriptToName[i] );
                BlockNode.SetAttribute("點數", BlockMainArray[i].座標點數.ToString());
                BlockNode.SetAttribute("序號", i.ToString());
                //MessageBox.Show("H4-1-2");
                Block區塊最上層點.AppendChild(BlockNode);
                //MessageBox.Show("H4-1");

                //依序設定參數
                //XmlElement Block混凝土方塊與方塊摩擦係數 = doc.CreateElement("混凝土方塊與方塊摩擦係數");
                //Block混凝土方塊與方塊摩擦係數.SetAttribute("Value", BlockMainArray[i].混凝土方塊與方塊摩擦係數.ToString());

                //XmlElement Block混凝土方塊與拋石摩擦係數 = doc.CreateElement("混凝土方塊與拋石摩擦係數");
                //Block混凝土方塊與拋石摩擦係數.SetAttribute("Value", BlockMainArray[i].混凝土方塊與拋石摩擦係數.ToString());

                //XmlElement Block場注土方塊與拋石摩擦係數 = doc.CreateElement("場注土方塊與拋石摩擦係數");
                //Block場注土方塊與拋石摩擦係數.SetAttribute("Value", BlockMainArray[i].場注土方塊與拋石摩擦係數.ToString());

                //XmlElement Block拋石與拋石摩擦係數 = doc.CreateElement("拋石與拋石摩擦係數");
                //Block拋石與拋石摩擦係數.SetAttribute("Value", BlockMainArray[i].拋石與拋石摩擦係數.ToString());

                //XmlElement Block混凝土陸上單位體積重量 = doc.CreateElement("混凝土陸上單位體積重量");
                //Block混凝土陸上單位體積重量.SetAttribute("Value", BlockMainArray[i].混凝土陸上單位體積重量.ToString());

                //XmlElement Block混凝土水中單位體積重量 = doc.CreateElement("混凝土水中單位體積重量");
                //Block混凝土水中單位體積重量.SetAttribute("Value", BlockMainArray[i].混凝土水中單位體積重量.ToString());

                //XmlElement Block拋石陸上單位體積重量 = doc.CreateElement("拋石陸上單位體積重量");
                //Block拋石陸上單位體積重量.SetAttribute("Value", BlockMainArray[i].拋石陸上單位體積重量.ToString());

                //XmlElement Block拋石水中單位體積重量 = doc.CreateElement("拋石水中單位體積重量");
                //Block拋石水中單位體積重量.SetAttribute("Value", BlockMainArray[i].拋石水中單位體積重量.ToString());

                //XmlElement Block砂土水中單位體積重量= doc.CreateElement("砂土水中單位體積重量");
                //Block砂土水中單位體積重量.SetAttribute("Value", BlockMainArray[i].砂土水中單位體積重量.ToString());

                //XmlElement Block海水單位體積重量 = doc.CreateElement("海水單位體積重量");
                //Block海水單位體積重量.SetAttribute("Value", BlockMainArray[i].海水單位體積重量.ToString());

                XmlElement Block單位體積重量 = doc.CreateElement("單位體積重量");
                Block單位體積重量.SetAttribute("Value", BlockMainArray[i].單位體積重量.ToString());
                XmlElement Block使用材質 = doc.CreateElement("使用材質");
                Block使用材質.SetAttribute("Value", BlockMainArray[i].使用材質.ToString());


                MessageBox.Show("H4-2");

                //BlockNode.AppendChild(Block混凝土方塊與方塊摩擦係數);
                //BlockNode.AppendChild(Block混凝土方塊與拋石摩擦係數);
                //BlockNode.AppendChild(Block場注土方塊與拋石摩擦係數);
                //BlockNode.AppendChild(Block拋石與拋石摩擦係數);
                //BlockNode.AppendChild(Block混凝土陸上單位體積重量);
                //BlockNode.AppendChild(Block混凝土水中單位體積重量);
                //BlockNode.AppendChild(Block拋石陸上單位體積重量);
                //BlockNode.AppendChild(Block拋石水中單位體積重量);
                //BlockNode.AppendChild(Block砂土水中單位體積重量);
                //BlockNode.AppendChild(Block海水單位體積重量);
                BlockNode.AppendChild(Block單位體積重量);
                BlockNode.AppendChild(Block使用材質);

                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                XmlElement Block座標點;

                for (int i2=0;i2<=getx.GetUpperBound(0);i2++)
                {
                    Block座標點= doc.CreateElement("BlockCoordinate");
                    //Block混凝土方塊與拋石摩擦係數.SetAttribute("Value", BlockMainArray[i].混凝土方塊與拋石摩擦係數.ToString());
                    Block座標點.SetAttribute("xValue", getx[i2].ToString());
                    Block座標點.SetAttribute("yValue", gety[i2].ToString());

                    BlockNode.AppendChild(Block座標點);
                }
                //MessageBox.Show("H4-3");

            }

            //點選中的Block
            XmlElement 選取Block名稱 = doc.CreateElement("選取Block序號");
            if(listBox_SectSetting.SelectedIndex==-1) //故意分開較為保險.
            {
                選取Block名稱.SetAttribute("Value", "-1");
            }
            else
            {
                選取Block名稱.SetAttribute("Value", listBox_SectSetting.SelectedIndex.ToString());
            }
            Block區塊最上層點.AppendChild(選取Block名稱);
            //*************************************************************************************



            //*************************************************************************************
            //4. 檢核結果



            //*************************************************************************************

            doc.Save(xmlfullpath);//儲存此XML文件
            MessageBox.Show("儲存完畢!!!","專案檔儲存",MessageBoxButtons.OK,MessageBoxIcon.Information);
            //深海波波向info.SetAttribute("電話", "0806449");
        }
        private string 打開XML專案檔(string OpenPath)
        {
            //若失敗，會回傳失敗訊息.
            //若成功,會回傳空字串.
            string openMessage = "";
            double xor;// = 0.0;
            double yor;// = 0.0;
            double H0r;// = 13.22;
            double T0r;// = 14.30;
            double HWLr;// = 2.44;
            double Sloper;// = 0.025;
            double Krr;//= 0.810;
            double Ksr;// = 1.117;
            double Kdr;// = 1000;
            double lambdar;// = 0.9;
            double betar;// = 0;
            double SFSlider;// = 1.2;
            double SFOverr;// = 1.2;
            double SeaGammar;
            double GroundEler;
            double ArmorGroundEler;
            bool 啟用消波工重量計算r;
            bool 啟用胸牆部安定檢核r;
            double 消波形塊安定係數r;
            double 混凝土與海水之比重srr;
            double 消波塊斜坡面與水平面夾角r;
            double 波高傳遞率ktr;
            double 混凝土容許應力r;
            double BKr;

            string dirr;

            Class_BlockSect[] BlockMainArrayR = new Class_BlockSect[] { };
            int blockSizer = 0;
            int selectedBlockIndex=-1;
            double[] ELArrayR = new double[] { };
            int ELSizer = 0;

            //開始來開啟.
            XmlDocument doc = new XmlDocument();
            doc.Load(OpenPath);
            try
            {
                //全域參數讀取.
                XmlNode RNode = doc.SelectSingleNode("Root/GlobalParameters/深海波波向");
                if (object.Equals(RNode, null))
                {
                    return "深海波波向讀取失敗";
                }
                XmlElement Relement = (XmlElement)RNode;
                dirr = Relement.GetAttribute("Value");
                if (dirr != "E" && dirr != "N" && dirr != "W" && dirr != "S")
                {
                    return "深海波波向值錯誤";
                }

                //深海波波高
                RNode = doc.SelectSingleNode("Root/GlobalParameters/深海波波高");
                if (object.Equals(RNode, null))
                {
                    return "深海波波高讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out H0r))
                {
                    return "深海波波高讀取失敗";
                }

                //深海波週期.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/深海波週期");
                if (object.Equals(RNode, null))
                {
                    return "深海波週期讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out T0r))
                {
                    return "深海波週期讀取失敗";
                }

                //地面線.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地面線");
                if(object.Equals(RNode,null))
                {
                    return "地面線讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if(!double.TryParse(Relement.GetAttribute("Value").ToString(),out GroundEler))
                {
                    return "地面線讀取失敗";
                }

                //消波塊高程.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/消波塊高程");
                if(object.Equals(RNode,null))
                {
                    return "消波塊高程讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if(!double.TryParse(Relement.GetAttribute("Value").ToString(),out ArmorGroundEler))
                {
                    return "消波塊高程讀取失敗";
                }

                //設計潮位.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/設計潮位");
                if (object.Equals(RNode, null))
                {
                    return "設計潮位讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out HWLr))
                {
                    return "設計潮位讀取失敗";
                }

                //海床坡度.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/海床坡度");
                if (object.Equals(RNode, null))
                {
                    return "海床坡度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out Sloper))
                {
                    return "海床坡度讀取失敗";
                }

                //折射係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/折射係數");
                if (object.Equals(RNode, null))
                {
                    return "折射係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out Krr))
                {
                    return "折射係數讀取失敗";
                }

                //淺化係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/淺化係數");
                if (object.Equals(RNode, null))
                {
                    return "淺化係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out Ksr))
                {
                    return "淺化係數讀取失敗";
                }

                //繞射係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/繞射係數");
                if (object.Equals(RNode, null))
                {
                    return "繞射係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out Kdr))
                {
                    return "繞射係數讀取失敗";
                }

                //波力折減係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/波力折減係數");
                if (object.Equals(RNode, null))
                {
                    return "波力折減係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out lambdar))
                {
                    return "波力折減係數讀取失敗";
                }

                //入射波與堤體法線垂直交角.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/入射波與堤體法線垂直交角");
                if (object.Equals(RNode, null))
                {
                    return "入射波與堤體法線垂直交角讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out betar))
                {
                    return "入射波與堤體法線垂直交角讀取失敗";
                }

                //滑動安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/滑動安全係數");
                if (object.Equals(RNode, null))
                {
                    return "滑動安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out SFSlider))
                {
                    return "滑動安全係數讀取失敗";
                }

                //傾倒安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/傾倒安全係數");
                if (object.Equals(RNode, null))
                {
                    return "傾倒安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out SFOverr))
                {
                    return "傾倒安全係數讀取失敗";
                }

                //海水單位體積重量.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/海水單位體積重量");
                if (object.Equals(RNode, null))
                {
                    return "海水單位體積重量讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out SeaGammar))
                {
                    return "海水單位體積重量讀取失敗";
                }

                //消波工重量檢核啟用.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/消波工重量檢核啟用");
                if (object.Equals(RNode, null))
                {
                    return "消波工重量檢核啟用讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 啟用消波工重量計算r))
                {
                    return "消波工重量檢核啟用讀取失敗";
                }
                if(啟用消波工重量計算r)
                {
                    //消波形塊安定係數.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/消波形塊安定係數");
                    if (object.Equals(RNode, null))
                    {
                        return "消波形塊安定係數讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波形塊安定係數r))
                    {
                        return "消波形塊安定係數讀取失敗";
                    }

                    //混凝土與海水之比重.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/混凝土與海水之比重");
                    if (object.Equals(RNode, null))
                    {
                        return "混凝土與海水之比重讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土與海水之比重srr))
                    {
                        return "混凝土與海水之比重讀取失敗";
                    }

                    //消波塊斜坡面與水平面之夾角.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/消波塊斜坡面與水平面之夾角");
                    if (object.Equals(RNode, null))
                    {
                        return "消波塊斜坡面與水平面之夾角讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波塊斜坡面與水平面夾角r))
                    {
                        return "消波塊斜坡面與水平面之夾角讀取失敗";
                    }

                    //波高傳遞率.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/波高傳遞率");
                    if (object.Equals(RNode, null))
                    {
                        return "波高傳遞率讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 波高傳遞率ktr))
                    {
                        return "波高傳遞率讀取失敗";
                    }


                }
                else
                {
                    消波形塊安定係數r = 0;
                    混凝土與海水之比重srr = 0;
                    消波塊斜坡面與水平面夾角r = 0;
                    波高傳遞率ktr = 0;
                }


                //胸牆部安定檢核啟用.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/胸牆部安定檢核啟用");
                if (object.Equals(RNode, null))
                {
                    return "胸牆部安定檢核啟用讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 啟用胸牆部安定檢核r))
                {
                    return "胸牆部安定檢核啟用讀取失敗";
                }
                if(啟用胸牆部安定檢核r)
                {
                    //胸牆部安定檢核啟用.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/混凝土容許應力");
                    if (object.Equals(RNode, null))
                    {
                        return "混凝土容許應力讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土容許應力r))
                    {
                        return "混凝土容許應力讀取失敗";
                    }

                    //胸牆部安定檢核啟用.
                    RNode = doc.SelectSingleNode("Root/GlobalParameters/BK");
                    if (object.Equals(RNode, null))
                    {
                        return "BK讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out BKr))
                    {
                        return "BK讀取失敗";
                    }
                }
                else
                {
                    混凝土容許應力r = 0;
                    BKr = 0;
                }

                //力矩計算參考點.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/力矩計算參考點");
                if (object.Equals(RNode, null))
                {
                    return "力矩計算參考點讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("xValue").ToString(), out xor))
                {
                    return "力矩計算參考點讀取失敗,x值轉換失敗";
                }
                if (!double.TryParse(Relement.GetAttribute("yValue").ToString(), out yor))
                {
                    return "力矩計算參考點讀取失敗,y值轉換失敗";
                }

                //EL設定.
                //RNode = doc.SelectSingleNode("Root/GlobalParameters/EL點數");
                //if (object.Equals(RNode, null))
                //{
                //    return "EL點數讀取失敗!";
                //}
                //Relement = (XmlElement)RNode;
                //if (!int.TryParse(Relement.GetAttribute("Value").ToString(), out ELSizer))
                //{
                //    return "EL點數讀取失敗!!轉換失敗";
                //}
                XmlNodeList ELCollection = doc.SelectNodes("Root/GlobalParameters/EL點數/EL");
                foreach (XmlNode ELNode in ELCollection)
                {
                    double fi;
                    Relement = (XmlElement)ELNode;
                    if(!double.TryParse(Relement.GetAttribute("Value").ToString(),out fi))
                    {
                        return "EL讀取失敗!!!";
                    }
                    Array.Resize(ref ELArrayR, ELSizer + 1);
                    ELArrayR[ELSizer] = fi;
                    ELSizer += 1;
                }
                //完成.

                //Block參數.
                XmlNodeList blockNodeCollection = doc.SelectNodes("Root/Blocks/形塊");
                foreach (XmlNode BlockNode in blockNodeCollection)
                {
                    Array.Resize(ref BlockMainArrayR, blockSizer + 1);
                    //RNode = BlockNode.SelectSingleNode("名稱");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "讀取Block的名稱失敗";
                    //}
                    int PointCount;
                    double[] getx = new double[] { };
                    double[] gety = new double[] { };
                    int getPointsize = 0;

                    Relement = (XmlElement)BlockNode;

                    BlockMainArrayR[blockSizer] = new Class_BlockSect();
                    BlockMainArrayR[blockSizer].名稱 = Relement.GetAttribute("名稱").ToString();
                    if (!int.TryParse(Relement.GetAttribute("點數").ToString(), out PointCount))
                    {
                        return "Block讀取失敗!點數欄位無法讀取";
                    }
                    BlockMainArrayR[blockSizer].座標點數 = PointCount;
                    double ftest;
                    //RNode = BlockNode.SelectSingleNode("混凝土方塊與方塊摩擦係數");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取混凝土方塊與方塊摩擦係數失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //double ftest;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取混凝土方塊與方塊摩擦係數失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].混凝土方塊與方塊摩擦係數 = ftest;


                    //RNode = BlockNode.SelectSingleNode("混凝土方塊與拋石摩擦係數");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取混凝土方塊與拋石摩擦係數失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取混凝土方塊與拋石摩擦係數失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].混凝土方塊與拋石摩擦係數 = ftest;


                    //RNode = BlockNode.SelectSingleNode("場注土方塊與拋石摩擦係數");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取場注土方塊與拋石摩擦係數失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取場注土方塊與拋石摩擦係數失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].場注土方塊與拋石摩擦係數 = ftest;


                    //RNode = BlockNode.SelectSingleNode("拋石與拋石摩擦係數");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取拋石與拋石摩擦係數失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取拋石與拋石摩擦係數失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].拋石與拋石摩擦係數 = ftest;

                    //RNode = BlockNode.SelectSingleNode("混凝土陸上單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取混凝土陸上單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取混凝土陸上單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].混凝土陸上單位體積重量 = ftest;


                    //RNode = BlockNode.SelectSingleNode("混凝土水中單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取混凝土水中單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取混凝土水中單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].混凝土水中單位體積重量 = ftest;

                    //RNode = BlockNode.SelectSingleNode("拋石陸上單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取拋石陸上單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取拋石陸上單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].拋石陸上單位體積重量 = ftest;


                    //RNode = BlockNode.SelectSingleNode("拋石水中單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取拋石水中單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取拋石水中單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].拋石水中單位體積重量 = ftest;


                    //RNode = BlockNode.SelectSingleNode("砂土水中單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取砂土水中單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取砂土水中單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].砂土水中單位體積重量 = ftest;


                    //RNode = BlockNode.SelectSingleNode("海水單位體積重量");
                    //if (object.Equals(RNode, null))
                    //{
                    //    return "Block讀取海水單位體積重量失敗!";
                    //}
                    //Relement = (XmlElement)RNode;
                    //if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    //{
                    //    return "Block讀取海水單位體積重量失敗!";
                    //}
                    //BlockMainArrayR[blockSizer].海水單位體積重量 = ftest;

                    //Block單位體積重量
                    RNode = BlockNode.SelectSingleNode("單位體積重量");
                    if (object.Equals(RNode, null))
                    {
                        return "Block讀取單位體積重量失敗!";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    {
                        return "Block讀取單位體積重量失敗!";
                    }
                    BlockMainArrayR[blockSizer].單位體積重量 = ftest;

                    //Block使用材質
                    RNode = BlockNode.SelectSingleNode("使用材質");
                    if(object.Equals(RNode,null))
                    {
                        return "Block讀取Block使用材質失敗!!";
                    }
                    Relement = (XmlElement)RNode;
                    BlockMainArrayR[blockSizer].使用材質 = Relement.GetAttribute("Value");

                    XmlNodeList CoordinateCollection = BlockNode.SelectNodes("BlockCoordinate");
                    if (object.Equals(CoordinateCollection, null))
                    {
                        return "Block讀取座標失敗";
                    }

                    foreach (XmlNode CN in CoordinateCollection)
                    {
                        Relement = (XmlElement)CN;
                        Array.Resize(ref getx, getPointsize + 1);
                        Array.Resize(ref gety, getPointsize + 1);

                        if (!double.TryParse(Relement.GetAttribute("xValue"), out ftest))
                        {
                            return "Block讀取座標時無法順利從'xValue'欄位轉換座標值";
                        }
                        getx[getPointsize] = ftest;
                        if (!double.TryParse(Relement.GetAttribute("yValue"), out ftest))
                        {
                            return "Block讀取座標時無法順利從'yValue'欄位轉換座標值";
                        }
                        gety[getPointsize] = ftest;
                        getPointsize += 1;
                    }

                    if (!(getPointsize == PointCount))
                    {
                        return "Block讀取座標點數量不一致!";
                    }

                    BlockMainArrayR[blockSizer].X = getx;
                    BlockMainArrayR[blockSizer].Y = gety;

                    blockSizer += 1;
                }

                RNode = doc.SelectSingleNode("Root/Blocks/選取Block序號");
                if (object.Equals(RNode, null))
                {
                    return "讀取Block選取序號時失敗";
                }
                int itest;
                Relement = (XmlElement)RNode;
                if (!int.TryParse(Relement.GetAttribute("Value"), out itest))
                {
                    return "轉換Block選取序號時失敗";
                }
                selectedBlockIndex = itest;
                if (selectedBlockIndex > BlockMainArrayR.GetUpperBound(0))
                {
                    return "Block選取序號超過檔案內紀錄的Block數量!";
                }


                //檢核區塊.
            }
            catch
            {
                return "出現錯誤!";
            }
            //==============================================================================================//
            //沒問題之後,開始替代所有設定.
            //全域參數.
            cmb_seawaveDir.SelectedItem = dirr;
            textBox_H0.Text = H0r.ToString();
            textBox_ArmorBlockEle.Text = ArmorGroundEler.ToString();
            textBox_GroundELE.Text = GroundEler.ToString();
            textBox_HWL.Text = HWLr.ToString();
            textBox_T0.Text = T0r.ToString();
            textBox_Slope.Text = Sloper.ToString();
            textBox_Kr.Text = Krr.ToString();
            textBox_Ks.Text = Ksr.ToString();
            textBox_Kd.Text = Kdr.ToString();
            textBox_Lenda.Text = lambdar.ToString();
            textBox_Beta.Text = betar.ToString();
            textBox_SFOver.Text = SFOverr.ToString();
            textBox_SFSlide.Text = SFSlider.ToString();
            textBox_SeaGamma.Text = SeaGammar.ToString();

            if(啟用消波工重量計算r)
            {
                textBox_KDL.ReadOnly = false;
                textBox_Sr.ReadOnly = false;
                textBox_slopeangle.ReadOnly = false;
                textBox_Kt.ReadOnly = false;
                textBox_KDL.Enabled = true;
                textBox_Sr.Enabled = true;
                textBox_slopeangle.Enabled = true;
                textBox_Kt.Enabled = true;
                label18.Enabled = true;
                label19.Enabled = true;
                label20.Enabled = true;
                label24.Enabled = true;

                chk_BlockWeightCalc.Checked = true;
                textBox_KDL.Text = 消波形塊安定係數r.ToString();
                textBox_Sr.Text = 混凝土與海水之比重srr.ToString();
                textBox_slopeangle.Text = 消波塊斜坡面與水平面夾角r.ToString();
                textBox_Kt.Text = 波高傳遞率ktr.ToString();
            }
            else
            {
                textBox_KDL.ReadOnly = true;
                textBox_Sr.ReadOnly = true;
                textBox_slopeangle.ReadOnly = true;
                textBox_Kt.ReadOnly = true;
                label18.Enabled = false;
                label19.Enabled = false;
                label20.Enabled = false;
                label24.Enabled = false;
                textBox_KDL.Enabled = false;
                textBox_Sr.Enabled = false;
                textBox_slopeangle.Enabled = false;
                textBox_Kt.Enabled = false;

                chk_BlockWeightCalc.Checked = false;
                textBox_KDL.Text = "";
                textBox_Sr.Text = "";
                textBox_slopeangle.Text = "";
                textBox_Kt.Text = "";
            }
            if (啟用胸牆部安定檢核r)
            {
                textBox_ConcreteAllowStress.ReadOnly = false;
                textBox_BK.ReadOnly = false;
                textBox_ConcreteAllowStress.Enabled = true;
                textBox_BK.Enabled = true;
                label21.Enabled = true;
                label22.Enabled = true;

                chk_HeadBreastCalc.Checked = true;
                textBox_ConcreteAllowStress.Text = 混凝土容許應力r.ToString();
                textBox_BK.Text =BKr.ToString();
            }
            else
            {
                textBox_ConcreteAllowStress.ReadOnly = true;
                textBox_BK.ReadOnly = true;
                label21.Enabled = false;
                label22.Enabled = false;
                textBox_ConcreteAllowStress.Enabled=false;
                textBox_BK.Enabled = false;

                chk_BlockWeightCalc.Checked = false;
                textBox_ConcreteAllowStress.Text = ""; // 混凝土容許應力r.ToString();
                textBox_BK.Text = ""; // BKr.ToString();
            }



            //Block區塊,填入變數.
            listBox_SectSetting.Items.Clear();
            Array.Resize(ref BlockMainArray, 0);
            Array.Resize(ref BlockMainArray, BlockMainArrayR.GetLength(0));
            BlockCount = BlockMainArrayR.GetLength(0);
            Array.Resize(ref ELArray, 0);
            Array.Resize(ref ELArray, ELSizer);
            //直接填入EL.
            ELArray = ELArrayR;
            ELSize = ELSizer;
            //填入EL的DG內,如果有資料的話.
            ELDGV1.Rows.Clear();
            DataGridViewRowCollection rows =ELDGV1.Rows;
            for (int i=0;i<ELSize;i++)
            {
                rows.Add(new object[] { ELArray[i] });
            }
            ELDGV1.CurrentCell = ELDGV1.Rows[0].Cells[0];

            //重設Block類的Dictionary.
            BlockArraySubscriptToName.Clear();
            BlockListSubScriptToName.Clear();
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();
            chart_Plot.Series.Clear();
            //加入所有形塊點.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                BlockMainArray[i] = BlockMainArrayR[i];
                BlockNameToListSubScript.Add(BlockMainArray[i].名稱, i);
                BlockListSubScriptToName.Add(i, BlockMainArray[i].名稱);

                chart_Plot.Series.Add(BlockMainArray[i].名稱);
                chart_Plot.Series[BlockMainArray[i].名稱].ChartType = SeriesChartType.Line;

                double[] getx;
                double[] gety;
                getx = BlockMainArray[i].X;
                gety = BlockMainArray[i].Y;
                for (int i2 = 0; i2 < getx.GetLength(0); i2++)
                {
                    chart_Plot.Series[BlockMainArray[i].名稱].Points.AddXY(getx[i2], gety[i2]);
                }
                if (!(getx[getx.GetUpperBound(0)] == getx[0] && gety[gety.GetUpperBound(0)] == gety[0]))
                {
                    chart_Plot.Series[BlockMainArray[i].名稱].Points.AddXY(getx[0], gety[0]);
                }
                chart_Plot.Series[BlockMainArray[i].名稱].BorderColor = Color.Black;
                chart_Plot.Series[BlockMainArray[i].名稱].Color = Color.Black;
                chart_Plot.Series[BlockMainArray[i].名稱].MarkerBorderWidth = 1; //Color.Black;
                listBox_SectSetting.Items.Add(BlockMainArray[i].名稱);
            }
            if (BlockMainArray.GetLength(0) > 0)
            {  調整Chart(chart_Plot);繪上EL(); }

            //檢核區塊.


            //其他:設定反應.
            selectname = (selectedBlockIndex == -1 ? null : BlockMainArray[selectedBlockIndex].名稱);
            if (selectedBlockIndex!=-1)
            {
                listBox_SectSetting.SelectedIndex = selectedBlockIndex;//此函數會觸動顯示Chart與Property Grid的功能.
            }
            btn_ModifiedBlock.Enabled = (selectedBlockIndex == -1 ? false : true);
            btnRemoveSects.Enabled = btn_ModifiedBlock.Enabled;
            InterfaceBlock = null;
            xo = xor;
            yo = yor;
            textBox_XO.Text = xo.ToString();
            textBox_YO.Text = yo.ToString();
            textBox_XO.ReadOnly = true;
            textBox_YO.ReadOnly = true;
            if(BlockMainArray.GetLength(0)>0)
            {
                開始檢核ToolStripMenuItem.Enabled = true;
            }
            else
            { 開始檢核ToolStripMenuItem.Enabled = false; }
            tsp_cond.Text = "請設定或編輯您的專案檔";
            tsp_progressbar.Visible = false;
            //==============================================================================================//

            return "";

        }
        private void 開啟舊的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //此Function會呼叫開啟XML專案檔的程式.
            string openpath = "";
            if(OFD_專案.ShowDialog()==DialogResult.OK && OFD_專案.FileName!="")
            { openpath = OFD_專案.FileName; }
            else
            { return; }


            string 開啟檔案之訊息 = 打開XML專案檔(openpath);
            if(開啟檔案之訊息=="")
            {
                打開專案檔的名稱 = openpath;
                this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                MessageBox.Show("開啟專案檔成功!","專案檔載入",MessageBoxButtons.OK,MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.
            }
            else
            {
                MessageBox.Show("開啟失敗!錯誤訊息:" + 開啟檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }

        }
        private void 開一個新的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(BlockMainArray.GetLength(0) == 0))
            {
                //當有編輯中的專案時(有Block時,才會有警示).
                if (MessageBox.Show("您確定要開啟新的專案檔?按下確定後目前編輯中的專案檔會遺失所有更動", "開新的專案檔", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                { return; }

            }

            Form_RDExamProgress_Load(sender, e);

        }

        private void 退出此檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //退出.
            if (!(BlockMainArray.GetLength(0) == 0))
            {
                //當有編輯中的專案時(有Block時,才會有警示).
                if (MessageBox.Show("您確定要關閉?按下確定後目前編輯中的專案檔會遺失所有更動", "關閉", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                { return; }

            }
            this.Close();
        }

        private void 另存專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(BlockMainArray.GetLength(0)==0)
            { MessageBox.Show("您沒有設定任何形塊!無法儲存", "專案檔管理", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string xmlpath;// = workfoldernow + "\\Test.xml";
           if (SFD_專案.ShowDialog() == DialogResult.OK && SFD_專案.FileName!="")
           {
                xmlpath = SFD_專案.FileName; //路徑.
            }
           else
            { return; }

            if (!CheckTextBoxNoEmpty())
            {
                return;
            }

            儲存XML專案檔(xmlpath);
            this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(xmlpath);
        }

        #endregion

        #region 消波工重量檢核計算打開
        private void chk_BlockWeightCalc_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BlockWeightCalc.Checked)
            {
                label18.Enabled = true;
                label19.Enabled = true;
                label20.Enabled = true;
                label24.Enabled = true;
                textBox_KDL.ReadOnly = false;
                textBox_Sr.ReadOnly = false;
                textBox_slopeangle.ReadOnly = false;
                textBox_Kt.ReadOnly = false;
                textBox_KDL.Enabled = true;
                textBox_Sr.Enabled = true;
                textBox_slopeangle.Enabled = true;
                textBox_Kt.Enabled = true;
            }
            else
            {
              label18.Enabled = false;
              label19.Enabled = false;
              label20.Enabled = false;
              label24.Enabled = false;
              textBox_KDL.ReadOnly = true;
              textBox_Sr.ReadOnly = true;
              textBox_slopeangle.ReadOnly = true;
              textBox_Kt.ReadOnly = true;
                textBox_KDL.Enabled = false;
                textBox_Sr.Enabled = false;
                textBox_slopeangle.Enabled = false;
                textBox_Kt.Enabled = false;
            }
        }
        private void textBox_KDL_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_Sr_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_slopeangle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        #endregion

        #region 胸牆部安定檢核計算開啟

        private void chk_HeadBreastCalc_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_HeadBreastCalc.Checked)
            {
                label21.Enabled = true;
                label22.Enabled = true;
                textBox_ConcreteAllowStress.ReadOnly = false;
                textBox_BK.ReadOnly = false;
                textBox_ConcreteAllowStress.Enabled = true;
                textBox_BK.Enabled = true;
            }
            else
            {
                label21.Enabled = false;
                label22.Enabled = false;
                textBox_ConcreteAllowStress.ReadOnly = true;
                textBox_BK.ReadOnly = true;
                textBox_ConcreteAllowStress.Enabled = false;
                textBox_BK.Enabled = false;
            }
        }
        private void textBox_ConcreteAllowStress_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BK_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        #endregion

        private void textBox_Slope_TextChanged(object sender, EventArgs e)
        {

        }

        #region 檢核主區塊
        Boolean CheckTextBoxNoEmpty()
        {
            if (cmb_seawaveDir.SelectedItem.ToString() == "")
            {
                MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_H0.Text.ToString() == "")
            {
                MessageBox.Show("您深海波波高沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_T0.Text.ToString() == "")
            {
                MessageBox.Show("您深海波週期沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_HWL.Text.ToString() == "")
            {
                MessageBox.Show("您設計潮位沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Slope.Text.ToString() == "")
            {
                MessageBox.Show("您海床坡度沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Kr.Text.ToString() == "")
            {
                MessageBox.Show("您折射係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Ks.Text.ToString() == "")
            {
                MessageBox.Show("您淺化係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Kd.Text.ToString() == "")
            {
                MessageBox.Show("您繞射係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Lenda.Text.ToString() == "")
            {
                MessageBox.Show("您波力折減係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_Beta.Text.ToString() == "")
            {
                MessageBox.Show("您入射波與堤體法線垂直交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_SFSlide.Text.ToString() == "")
            {
                MessageBox.Show("您滑動安全係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_SFOver.Text.ToString() == "")
            {
                MessageBox.Show("您傾倒安全係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (textBox_SeaGamma.Text.ToString() == "")
            {
                MessageBox.Show("您海水單位體積重量沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
            if (chk_BlockWeightCalc.Checked)
            {
                if (textBox_KDL.Text.ToString() == "")
                {
                    MessageBox.Show("您消波形塊安定係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (textBox_Sr.Text.ToString() == "")
                {
                    MessageBox.Show("您混凝土與海水之比重沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (textBox_slopeangle.Text.ToString() == "")
                {
                    MessageBox.Show("您消波塊斜坡面與水平面之交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (textBox_Kt.Text.ToString() == "")
                {
                    MessageBox.Show("您波高傳遞率Kt沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
               
            }
            if (chk_HeadBreastCalc.Checked)
            {
                if (textBox_ConcreteAllowStress.Text.ToString() == "")
                {
                    MessageBox.Show("您混凝土容許應力沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
                if (textBox_BK.Text.ToString() == "")
                {
                    MessageBox.Show("您BK'沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }
            return true;
        }

        private void 開始檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //檢核前預檢查.
            if(!CheckTextBoxNoEmpty())
            {
                return;
            }

            //Block檢查.
            //
            //
            //
            //


            //***********************************************************************************************************************//
            //帶入計算
            // Class New
            Mod = new Module1();
            Mod.DeleteAllBlockData();

            //1. H0, HWL, 海水r.
            Mod.WaterDesignInput(double.Parse(textBox_H0.Text), double.Parse(textBox_HWL.Text), double.Parse(textBox_SeaGamma.Text));
            //2. 波向, T0, Kr, Ks , Kd, lambda, beta.
            Mod.WaveDesignInput(cmb_seawaveDir.SelectedItem.ToString().ToLower() == "e" ? 1 : 0, double.Parse(textBox_T0.Text), double.Parse(textBox_Kr.Text), double.Parse(textBox_Ks.Text), double.Parse(textBox_Kd.Text), double.Parse(textBox_Lenda.Text), double.Parse(textBox_Beta.Text));
            //3. S(海床坡度), 底面線, 消波塊高層
            Mod.BaseDesignInput(double.Parse(textBox_Slope.Text), double.Parse(textBox_GroundELE.Text), double.Parse(textBox_ArmorBlockEle.Text));
            //4. Block給定.
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                //迴圈塞入Block.
                int nowid = Mod.NewBlock(BlockMainArray[i].單位體積重量, -9999); //, BlockMainArray[i].場注土方塊與拋石摩擦係數); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                int 座標點數 = BlockMainArray[i].座標點數;
                for(int i2=0;i2<座標點數;i2++)
                {
                    Mod.SetBlockCoord(nowid, getx[i2], gety[i2]);

                }
            }
            //**********************************************************************************************************************//




            //計算.
            Mod.Run();


            //結果呈現.
            textBox_CheckMessageShow.Text=Mod.ErrMsg;
            MessageBox.Show("檢核計算完成", "海堤檢核計算", MessageBoxButtons.OK, MessageBoxIcon.Information);
            tabControl1.SelectedIndex = 2; //更換頁面.



        }
        #endregion

        #region 輸出檢核結果EXCEL表單
        private void btn_OutputExcel_Click(object sender, EventArgs e)
        {
            if(SFD_EXCELReport.ShowDialog()==DialogResult.OK && SFD_EXCELReport.FileName!="")
            {
                //輸出.
                string getpath = SFD_EXCELReport.FileName;


                //執行EXCEL 輸出.




            }
        }
        #endregion

        private void textBox_SeaGamma_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_LogOutput_Click(object sender, EventArgs e)
        {
            if(object.Equals(Mod,null))
            {
                MessageBox.Show("你的計算主體'MOD'為Null!!!!!");
                return;
            }

            //String Pos = "C:\\Users\\kavy\\Desktop\\test.log";
            
            //Mod.OutPutLogFile(Pos);
            //MessageBox.Show("輸出完成!");
            if (SFD_LogFile.ShowDialog() == DialogResult.OK && SFD_LogFile.FileName != "")
            {
                string getpath = SFD_LogFile.FileName;
                //呼叫.
                Mod.OutPutLogFile(getpath);
                MessageBox.Show("輸出完成!");

            }
        }

        #region 關閉表單

        private void Form_RDExamProgress_FormClosed(object sender, FormClosedEventArgs e)
        {


            if (object.Equals(Mod, null)) { }
            else { Mod.Dispose();  }
        }
        #endregion

        #region EL變更區塊
        private void ELDGV1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("H!");
            繪上EL();
        }

        private void ELDGV1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            繪上EL();
        }
        #endregion
    }
}
