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
using Excel = Microsoft.Office.Interop.Excel;
using WORD = Microsoft.Office.Interop.Word;
using Microsoft.VisualBasic.MyServices;
using System.Diagnostics;
using System.Net;


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

        //EL矩陣.
        //Minimum EL.
        double MinEL; //根據波向來判斷.
        double MaxEL; //根據波向來判斷.
        public double[] ELArray = new double[] { };
        int ELSize = 0;

        //材質矩陣.
        private Dictionary<String, int> MaterialNameToArraySubScript = new Dictionary<string, int>();//Material Name to subscript index.
        private Dictionary<int, String> MaterialSubscriptToName = new Dictionary<int, string>();//Material Subscript in array to Name.
        public string[] MaterialArray = new string[] { };
        int MaterialCount = 0;
        private MaterialsRoughness[] MaterialsCoefArray = new MaterialsRoughness[] { };
        private int MaterialRoughnessArrayCount = 0;



        #endregion
        bool 使用者手動更新材質與摩擦 = false;
        private bool isExporting=false;
        private Form1 mainForm = null;
        string selectname = null;  //目前點選到的Block.
        Module1 Mod = null;
        RDExameTextBox_Object_Class RCOL = new RDExameTextBox_Object_Class();
        string PNGStoredFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP.PNG";
        string VESDStoredFolderPath= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP_Project.vesdp";
        private struct MaterialsRoughness
        {
            public int Id1;
            public int Id2;
            public double coef;
        }
        private struct PointFMY
        {
            public double x;
            public double y;
            public double z;
        }

        public Form_RDExamProgress()
        {
            InitializeComponent();
            //chart_Plot.Series[1].Name = "Series1";

            //chart_Plot.Series.Add("Series2");
            //chart_Plot.Series[1].ChartType = SeriesChartType.Area; //.Line;
            //chart_Plot.Series[0].Points.DataBindXY(X2, Y2);
            //chart_Plot.Series[0].Color = Color.Red;

        }
        public Form_RDExamProgress(Form callingForm)
        {
            mainForm = callingForm as Form1;//傳入物件參考.
            InitializeComponent();
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
            使用者手動更新材質與摩擦 = false;
            isExporting = false;
            //Tab 1.[公用參數設定區塊]初始化
            cmb_seawaveDir.SelectedItem = "右";
            cmb_DeepWaveActDir.SelectedItem = "E";
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
            textBox_BataFix.Text = "0";
            textBox_SFSlide.Text = "1.2";
            textBox_SFOver.Text = "1.2";
            textBox_SeaGamma.Text = "1.03";

            chk_BlockWeightCalc_HO.Checked = false;
            chk_BlockWeightCalc_HE.Checked = false;
            chk_BlockWeightCalc_BD.Checked = false;
            chk_HeadBreastCalc.Checked = false;

            label_HO_1.Enabled = false;
            label_HO_2.Enabled = false;
            label_HO_3.Enabled = false;
            label_HE_1.Enabled = false;
            label_HE_2.Enabled = false;
            label_HE_3.Enabled = false;
            label_BreastCheck_1.Enabled = false;
            label_BrestCheck_2.Enabled = false;
            label_BD_1.Enabled = false;
            label_BD_2.Enabled = false;
            label_BD_3.Enabled = false;
            label_BD_4.Enabled = false;

            textBox_HO_KDL.ReadOnly = true;
            textBox_HO_Gamma.ReadOnly = true;
            textBox_HO_slopeangle.ReadOnly = true;
            textBox_HE_KDL.ReadOnly = true;
            textBox_HE_Gamma.ReadOnly = true;
            textBox_HE_slopeangle.ReadOnly = true;
            textBox_BD_KDL.ReadOnly = true;
            textBox_BD_Gamma.ReadOnly = true;
            textBox_BD_slopeangle.ReadOnly = true;
            textBox_BD_Kt.ReadOnly = true;
            textBox_HO_KDL.Enabled = false;
            textBox_HO_Gamma.Enabled = false;
            textBox_HO_slopeangle.Enabled = false;
            textBox_HE_KDL.Enabled = false;
            textBox_HE_Gamma.Enabled = false;
            textBox_HE_slopeangle.Enabled = false;
            textBox_BD_KDL.Enabled = false;
            textBox_BD_Gamma.Enabled = false;
            textBox_BD_slopeangle.Enabled = false;
            textBox_BD_Kt.Enabled = false;
           
            textBox_HO_KDL.Text = "";
            textBox_HO_Gamma.Text = "";
            textBox_HO_slopeangle.Text = "";
            textBox_HE_KDL.Text = "";
            textBox_HE_Gamma.Text = "";
            textBox_HE_slopeangle.Text = "";
            textBox_BD_KDL.Text = "";
            textBox_BD_Gamma.Text = "";
            textBox_BD_slopeangle.Text = "";
            textBox_BD_Kt.Text = "";
            
            textBox_ConcreteAllowStress.Text = "";
            textBox_ConcreteAllowStress.Enabled = false;
            textBox_ConcreteAllowStress.ReadOnly = true;
            textBox_BK.Text = "";  
            textBox_BK.ReadOnly = true;
            textBox_BK.Enabled = false;


            //Tab 2.[材質與摩擦係數增減區塊]初始化.
            EscapeDGMaterialCellValueChangedFunction = true;
            DGMaterial.Rows.Clear();
            EscapeDGMaterialRoughnessCellContentChanged = true;
            DGMaterialRough.Rows.Clear();
            Array.Resize(ref MaterialArray, 0);MaterialCount = 0;
            Array.Resize(ref MaterialsCoefArray, 0);MaterialRoughnessArrayCount = 0;
            MaterialNameToArraySubScript.Clear();
            MaterialSubscriptToName.Clear();

            //2016/03/26.
            //加入讀取預設摩擦係數設定之功能於此.
            讀入摩擦係數初始設定();
            使用者手動更新材質與摩擦 = true;

            chart_Plot.Series.Clear();
            //Tab 3.[Block新增刪減區塊]初始化.
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
            cmb_ShowOnBlockListChoice.SelectedItem = "單位體積重";
            ELDGV1.Rows.Clear();
            ELDGV1.Enabled = false; //初始設定變更為不可操控,要等到有Block時才開始能填入.
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
            ReferencedMaterialCHKL.Items.Clear();
            btn_ModifiedBlock.Enabled = false;
            BlockArraySubscriptToName.Clear();
            BlockListSubScriptToName.Clear();
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();

            開始檢核ToolStripMenuItem.Enabled = false;
            btn_LogOutput.Enabled = false;
            btn_OutputExcel.Enabled = false;
            btn_Test.Enabled = false;

            textBox_XO.Text = "0";
            textBox_YO.Text = "0";
            xo = 0;
            yo = 0;
            textBox_XO.ReadOnly = true;
            textBox_YO.ReadOnly = true;

            btnRemoveSects.Enabled = false;
            tsp_cond.Text = "請設定或編輯您的專案檔";
            tsp_progressbar.Visible = false;
            chk_OpenFileAfterOutput.Checked = false;


            this.Text = "專案檔:未命名";

            //PropertyGrid測試.[2015/12/14].
            //propertyGrid_Block.SelectedObject = new Class_Block_Interface();


        }
        #region 摩擦係數初始設定
        private void 讀入摩擦係數初始設定()
        {
            //之後可以更改為讀取檔案之方式,但目前不開發此種操作.
            EscapeDGMaterialCellValueChangedFunction = true;
            DGMaterial.Rows.Clear();
            EscapeDGMaterialRoughnessCellContentChanged = true;
            DGMaterialRough.Rows.Clear();
            Array.Resize(ref MaterialArray, 3); MaterialCount = 3;
            MaterialArray[0] = "混凝土方塊";
            MaterialArray[1] = "混凝土拋石";
            MaterialArray[2] = "場鑄混凝土";
            Array.Resize(ref MaterialsCoefArray, 4); MaterialRoughnessArrayCount = 4;
            MaterialsCoefArray[0].coef = 0.5;//混凝土方塊與方塊.
            MaterialsCoefArray[0].Id1 = 0;
            MaterialsCoefArray[0].Id2 = 0;
            MaterialsCoefArray[1].coef = 0.6;//混凝土方塊與拋石.
            MaterialsCoefArray[1].Id1 = 0;
            MaterialsCoefArray[1].Id2 = 1;
            MaterialsCoefArray[2].coef = 0.7;//場注混凝土與拋石.
            MaterialsCoefArray[2].Id1 = 2;
            MaterialsCoefArray[2].Id2 = 1;
            MaterialsCoefArray[3].coef = 0.8;//拋石與拋石.
            MaterialsCoefArray[3].Id1 = 1;
            MaterialsCoefArray[3].Id2 = 1;
            MaterialNameToArraySubScript.Clear();
            MaterialSubscriptToName.Clear();
            for (int i = 0; i < MaterialArray.GetLength(0); i++)
            {
                DGMaterial.Rows.Add(new object[] { (i + 1).ToString(), MaterialArray[i].ToString() });
                MaterialNameToArraySubScript.Add(MaterialArray[i].ToString(), i);
                MaterialSubscriptToName.Add(i, MaterialArray[i].ToString());
            }
            //MaterialsCoefArray = MaterialsCoefArrayR;
            //MaterialRoughnessArrayCount = MaterialsCoefCountR;
            DGMaterialRough.Rows.Clear();
            for (int i = 0; i < MaterialsCoefArray.GetLength(0); i++)
            {
                DGMaterialRough.Rows.Add(new object[] { (i + 1).ToString(), MaterialsCoefArray[i].Id1 == -9999 ? "" : MaterialSubscriptToName[MaterialsCoefArray[i].Id1], MaterialsCoefArray[i].Id2 == -9999 ? "" : MaterialSubscriptToName[MaterialsCoefArray[i].Id2], MaterialsCoefArray[i].coef == -9999 ? "" : MaterialsCoefArray[i].coef.ToString() });
            }
            //設定下拉式選單.
            //設定Combobox內容.
            foreach (DataGridViewRow row in DGMaterialRough.Rows)
            {
                var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                cell.DataSource = MaterialArray;
                var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                cell2.DataSource = MaterialArray;
            }
            EscapeDGMaterialCellValueChangedFunction = false;
            EscapeDGMaterialRoughnessCellContentChanged = false;
        }
        #endregion
        #region Chart互動區
        private void chart_Plot_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ex = (System.Windows.Forms.MouseEventArgs)e;
            //MessageBox.Show(ex.X.ToString() + "," + ex.Y.ToString());
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
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname=="ARROW")
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
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname=="ARROW")
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
            else if(result.ChartElementType==ChartElementType.LegendItem)
            {
                LegendItem r = (LegendItem)result.Object;
                //Series s=(Series)r.Name;
                string hitname = r.Name;
                //MessageBox.Show(hitname);
                if (selectname == null)
                {
                    //Do nothing.
                    //chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    //chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    //selectname = result.Series.Name;

                    //chart_Plot.Series[result.Series.Name].BorderColor = Color.Red;
                    //chart_Plot.Series[result.Series.Name].BorderWidth = 2;
                    //Listbox顯示變更.
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname == "ARROW")
                    {
                        //Do nothing.
                        return;
                    }
                    else
                    {
                        listBox_SectSetting.SelectedIndex = BlockNameToListSubScript[hitname];
                    }
                }
                else if (selectname == hitname)
                {
                    //Do nothing.
                }
                else if (selectname != hitname)
                {
                    //Clear previous select.
                    //Listbox顯示變更.
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname == "ARROW")
                    {
                        //Do nothing.
                        return;
                    }
                    else
                    {
                        listBox_SectSetting.SelectedIndex = BlockNameToListSubScript[hitname];
                    }
                }

            }
            else
            {
                //新增檢查此點位是否在任一Polygon內部.
                double getPolygonArea = -10000000;
                string getPolygonName = null;
                //建立Polygon矩陣
                PP[] PolygonCol = new PP[] { };
                
                double hitX= chart_Plot.ChartAreas[0].AxisX.PixelPositionToValue(ex.X); 
                double hitY = chart_Plot.ChartAreas[0].AxisY.PixelPositionToValue(ex.Y); 
                //MessageBox.Show(hitX.ToString() + "," + hitY.ToString());
                PP PHit;
                PHit.h = hitX;
                PHit.v = hitY;
                //MessageBox.Show("X= " + (hitX).ToString() + ", Y= " + (hitY).ToString() + ".");

                for(int i=0;i<BlockMainArray.GetLength(0);i++)
                {
                    Array.Resize(ref PolygonCol, 0);
                    double[] XI = BlockMainArray[i].X;
                    double[] YI = BlockMainArray[i].Y;
                    for(int j=0;j<XI.GetLength(0);j++)
                    {
                        Array.Resize(ref PolygonCol, j + 1);
                        PolygonCol[j].h = XI[j];
                        PolygonCol[j].v = YI[j];
                    }
                    double PArea = PolygonArea(PolygonCol);
                    bool 是否在此Polygon內 = InsidePolygon(PolygonCol,PHit);
                    if(是否在此Polygon內)
                    {
                        if(PArea>getPolygonArea)
                        {
                            //MessageBox.Show(BlockMainArray[i].名稱 + ":" + PArea.ToString());
                            getPolygonName = BlockMainArray[i].名稱;
                            getPolygonArea = PArea;
                        }
                    }
                }
                if(object.Equals(getPolygonName,null))
                {
                    //找不到此點落於的Polygon.
                    if (selectname != null)
                    {
                        //Clear previous selected.
                        chart_Plot.Series[selectname].Color = Color.Black;
                        chart_Plot.Series[selectname].BorderWidth = 2;
                        //chart_Plot.Series[nowname].BorderWidth = 3;
                        selectname = null;
                        listBox_SectSetting.SelectedIndex = -1;
                        propertyGrid_Block.SelectedObject = null;
                        ReferencedMaterialCHKL.Items.Clear();
                        //不動.
                    }
                }
                else
                {
                    //有找到位於的Polygon名稱.
                    if(selectname==getPolygonName)
                    {
                        //Do nothing.
                    }
                    else
                    {
                        if (getPolygonName == "HWL" || getPolygonName.Substring(0, 1) == "E" || getPolygonName == "ARROW")
                        {
                            //Do nothing.
                            return;
                        }
                        else
                        {
                            listBox_SectSetting.SelectedIndex = BlockNameToListSubScript[getPolygonName];//會自動觸發其他程序.
                        }
                    }
                }

 
            }
        }
        public struct PP
        {
            public double h, v;
        }
        public double CrossVector(PP P1,PP P2)
        {
            return P1.h * P2.v - P2.h * P1.v;
        }
        public double PolygonArea(PP[] Polygon)
        {
            //採用行列式計算方法.
            double  area = 0;
            for(int i=0;i<Polygon.GetUpperBound(0);i++)
            {
                area += (CrossVector(Polygon[i], Polygon[i + 1]));
            }
            return Math.Abs(area);
        }
        public bool InsidePolygon(PP[] Polygon,PP P )
        {
            double angle = 0;
            int n = Polygon.GetLength(0);
            PP p1;
            PP p2;
            for(int i=0;i<Polygon.GetLength(0);i++)
            {
                p1.h = Polygon[i].h - P.h;
                p1.v = Polygon[i].v - P.v;
                p2.h = Polygon[(i + 1) % n].h - P.h;
                p2.v = Polygon[(i + 1) % n].v - P.v;
                angle += Angle2D(p1.h, p1.v, p2.h, p2.v);

            }
            if(Math.Abs(angle)<Math.PI)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private double Angle2D(double x1,double y1,double x2,double y2)
        {
            double dtheata, theat1, theat2;
            theat1 = Math.Atan2(y1, x1);
            theat2 = Math.Atan2(y2, x2);
            dtheata = theat2 - theat1;
            while (dtheata>Math.PI)
            {
                dtheata -= (Math.PI * 2);
            }
            while(dtheata<-Math.PI)
            {
                dtheata += (Math.PI * 2);
            }
            return dtheata;
        }

        #endregion 

        private void btn_Test_Click(object sender, EventArgs e)
        {
            開始檢核ToolStripMenuItem_Click(sender, e);
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
        private void cmb_seawaveDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmb_seawaveDir.SelectedIndex!=-1)
            {
                繪上EL();
            }
        }
        private void textBox_GroundELE_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_ArmorBlockEle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
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
        private void textBox_HB_KeyPress(object sender, KeyPressEventArgs e)
        {
            //選擇性參數.
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
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
        private void textBox_Beta_TextChanged(object sender, EventArgs e)
        {
            //Not handle.
            double tryD;
            double fixD = -9999;
            if (double.TryParse(textBox_Beta.Text.ToString(), out tryD))
            {
                //MessageBox.Show(tryD.ToString());
                if (tryD < 0)
                {
                    if (Math.Abs(tryD) > 15)
                    {
                        fixD = tryD + 15;
                    }
                    else
                    {
                        fixD = 0;
                    }

                }
                else if (tryD > 0)
                {
                    if (tryD > 15)
                    {
                        fixD = tryD - 15;
                    }
                    else
                    {
                        fixD = 0;
                    }
                }
                else
                {
                    fixD = tryD;
                }
                textBox_BataFix.Text = (fixD).ToString();
            }
            else
            {
                textBox_BataFix.Text = "";
            }
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
        public String 根據選擇的呈現選項回傳Block屬性(Class_BlockSect B)
        {
            if(cmb_ShowOnBlockListChoice.SelectedItem.ToString()=="無")
            {
                return "";
            }
            else if(cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "單位體積重")
            {
                return "(" + B.單位體積重量.ToString() + ")";
            }
            else if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "Moment計算")
            {
                return "(" + B.計算Moment與否.ToString() + ")";
            }
            else if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "材質")
            {
                return "(" + B.使用材質 + ")";
            }
            else
            {
                return "";
            }

        }
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
            BlockArraySubscriptToName.Add(listBox_SectSetting.Items.Count, InterfaceBlock.名稱);
            BlockNameToArraySubscript.Add(InterfaceBlock.名稱, listBox_SectSetting.Items.Count);
            //2. 新增到Listbox與Array上.
            BlockCount = listBox_SectSetting.Items.Count;
            listBox_SectSetting.Items.Add(InterfaceBlock.名稱 + 根據選擇的呈現選項回傳Block屬性(InterfaceBlock));// "(2.4)");
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

            string Msg = "";
            開始檢核ToolStripMenuItem.Enabled = (mainForm.檢視目前是否已有合理認證(ref Msg) && true);// 檢視目前是否已設定正確機碼來鎖定機器(ref Msg) && true);
            btn_Test.Enabled = 開始檢核ToolStripMenuItem.Enabled;

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
                    chart_Plot.Series[selectname].BorderWidth = 2;
                    selectname = null;
                }
                //listBox_SectSetting.SelectedIndex = -1;
                propertyGrid_Block.SelectedObject = null;
                ReferencedMaterialCHKL.Items.Clear();
                btn_ModifiedBlock.Enabled = false;
                btnRemoveSects.Enabled = false;
            }
        }
        private void ShowCurrentBlockInformation()
        {
            //1.更換Chart 繪製與加入不在這個區塊.
            if(!object.Equals(selectname, null))
            {
                //chart_Plot.Series[selectname].BorderColor = Color.Black;
                chart_Plot.Series[selectname].BorderWidth = 2;
                chart_Plot.Series[selectname].Color = Color.Black;

                label_XXX.Text = "";
                //chart_Plot.Series[selectname].MarkerBorderWidth = 2;
            }
            string nowname = BlockListSubScriptToName[listBox_SectSetting.SelectedIndex];
            for (int i = chart_Plot.Annotations.Count - 1; i >= 0; i--)
            {
                TextAnnotation TT = (TextAnnotation)chart_Plot.Annotations[i];
                if (TT.Text == nowname)
                {
                    //chart_Plot.Annotations.RemoveAt(i);
                    //label_XXX.Text = "(" + TT.AnchorX.ToString("00.00") + "," + TT.AnchorY.ToString("00.00") + ")";
                    label_XXX.Text = "(" + TT.AnchorX.ToString("00.00") + "," + TT.AnchorY.ToString("00.00") + ")\n(" + TT.X.ToString("00.00") + "," + TT.Y.ToString("00.00") + ")\n" + TT.AnchorAlignment.ToString();
                }


            }
            //MessageBox.Show(nowname);

            //chart_Plot.Series[nowname].BorderColor = Color.Red;
            chart_Plot.Series[nowname].BorderWidth = 3;
            chart_Plot.Series[nowname].Color = Color.Red;
            //chart_Plot.Series[nowname].MarkerBorderWidth = 3;
            selectname = nowname;

            //2.Property Grid更換.
            Class_Block_Interface D= new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
            D.可用材質 = MaterialArray;
            if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
            { D.使用材質 = ""; }
            propertyGrid_Block.SelectedObject = D;
            //將參考材質填入資訊:前提都是必須要有可用材質.
            ReferencedMaterialCHKL.Items.Clear();
            for(int i=0;i<MaterialArray.GetLength(0);i++)
            {
                ReferencedMaterialCHKL.Items.Add(MaterialArray[i]);
            }
            //根據此Block是否有選取設定資訊.
            string[] BlockSelectedReferncedMaterialNames = BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質;
            string[] AvailableMaterials = new string[] { };
            for(int i=0;i<BlockSelectedReferncedMaterialNames.GetLength(0);i++)
            {
                if(MaterialNameToArraySubScript.ContainsKey(BlockSelectedReferncedMaterialNames[i]))
                {
                    Array.Resize(ref AvailableMaterials, AvailableMaterials.GetLength(0) + 1);
                    AvailableMaterials[AvailableMaterials.GetUpperBound(0)] = BlockSelectedReferncedMaterialNames[i];
                }
            }
            BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質 = AvailableMaterials;//根據目前最新的可用材質清單 傳回可用的選取..
            for(int i=0;i<AvailableMaterials.GetLength(0);i++)
            {
                ReferencedMaterialCHKL.SetItemCheckState(MaterialNameToArraySubScript[AvailableMaterials[i]], CheckState.Checked);
            }
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
            chart_Plot.Series[NewI.名稱].MarkerBorderWidth = 2;
            ELDGV1.Enabled = true;
            調整Chart(chart_Plot);
            繪上EL();
        }
        #endregion
        #region Property參數值變更

        private void propertyGrid_Block_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            updateCurrentBlockPropertyGrid();
            //MessageBox.Show(e.ChangedItem.ToString());
        }
        private void ReferencedMaterialCHKL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //更新目前選定Block的周圍參考材質之設定.
            if(object.Equals(selectname,null))
            {
                return;
            }
            if(ReferencedMaterialCHKL.Items.Count==0)
            {
                return;
            }

            int id = BlockNameToListSubScript[selectname];
            string[] UseReferencedBlock = new string[] { };
            for(int i=0;i<ReferencedMaterialCHKL.Items.Count;i++)
            {
                if(ReferencedMaterialCHKL.GetItemChecked(i))
                {
                    Array.Resize(ref UseReferencedBlock, UseReferencedBlock.GetLength(0) + 1);
                    UseReferencedBlock[UseReferencedBlock.GetUpperBound(0)] = ReferencedMaterialCHKL.Items[i].ToString();
                }
            }
            BlockMainArray[id].周圍參考材質 = UseReferencedBlock;//更新.
        }
        private void updateCurrentBlockPropertyGrid()
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
            BlockMainArray[id].計算Moment與否 = D.計算Moment與否;
            BlockMainArray[id].使用材質 = D.使用材質;

            listBox_SectSetting.Items[listBox_SectSetting.SelectedIndex] = BlockListSubScriptToName[listBox_SectSetting.SelectedIndex] + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[listBox_SectSetting.SelectedIndex]);//; "(" + D.單位體積重量 + ")";

        }
        private void cmb_ShowOnBlockListChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //呈現方式變更.
            if(BlockMainArray.GetLength(0)==0)
            {
                return;
            }
            for(int i=0;i<listBox_SectSetting.Items.Count;i++)
            {
                listBox_SectSetting.Items[i] = BlockListSubScriptToName[i] + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[i]);
            }
            //完成.
        }
        private void propertyGrid_Block_Click(object sender, EventArgs e)
        {
            //Nothing.
            //MessageBox.Show("C1");
            if (listBox_SectSetting.SelectedIndex != -1)
            {
                //重新載入一次
                Class_Block_Interface D = new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;
            }
        }
        private void propertyGrid_Block_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("C2");
            if (listBox_SectSetting.SelectedIndex != -1)
            {
                //重新載入一次
                Class_Block_Interface D = new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;
            }
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
            if(BlockMainArray.GetLength(0)==0)
            {
                ELDGV1.Enabled = false;
            }
            //更新Position開始之後的Index.
            for (int i=0;i<listBox_SectSetting.Items.Count;i++)
            {
                if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "無")
                {
                    BlockArraySubscriptToName[i] = listBox_SectSetting.Items[i].ToString();
                    BlockListSubScriptToName[i] = listBox_SectSetting.Items[i].ToString();
                    BlockNameToArraySubscript[listBox_SectSetting.Items[i].ToString()] = i;
                    BlockNameToListSubScript[listBox_SectSetting.Items[i].ToString()] = i;
                }
                else
                {
                    BlockArraySubscriptToName[i] = listBox_SectSetting.Items[i].ToString().Split('(')[0];
                    BlockListSubScriptToName[i] = listBox_SectSetting.Items[i].ToString().Split('(')[0];
                    BlockNameToArraySubscript[listBox_SectSetting.Items[i].ToString().Split('(')[0]] = i;
                    BlockNameToListSubScript[listBox_SectSetting.Items[i].ToString().Split('(')[0]] = i;
                }
            }
            //
            
            //selectname = null; -->這個已經被包含到觸動函數去了.
            if(listBox_SectSetting.Items.Count==0)
            {
                //btnRemoveSects.Enabled = false;
                開始檢核ToolStripMenuItem.Enabled = false;
                btn_Test.Enabled = false;

            }

            
            BlockCount = listBox_SectSetting.Items.Count;
            繪上EL();
            //if(BlockCount==0)
            //{
            //清除此區塊之名稱文字.
            //TextAnnotation TT = new TextAnnotation();
            //for (int i = chart_Plot.Annotations.Count - 1; i >= 0; i--)
            //{
            //    TextAnnotation TT = (TextAnnotation)chart_Plot.Annotations[i];
            //    if (TT.Text == oldname)
            //    {
            //        chart_Plot.Annotations.RemoveAt(i);
            //    }


            //}
            if (object.Equals(selectname, null))
            {
                //MessageBox.Show("Selectname is null");
            }
            else
            {
                //MessageBox.Show(selectname.ToString());
            }
            
        }
        private void 調整Chart(Chart INS)
        {
            //包含繪上Block編號之文字.

            double Xmin = 1000000; // double.MaxValue;
            double Xmax = -100000; // double.MinValue;
            double Ymin = Xmin;
            double Ymax = Xmax;
            foreach (Series ss in INS.Series)
            {
                foreach (DataPoint dp in ss.Points)
                {
                    //label_Show.Text += (dp.XValue.ToString());
                    if (dp.XValue > Xmax && ss.Name.Substring(0)!="E" && ss.Name!="HWL") { Xmax = dp.XValue; }
                    if (dp.XValue < Xmin && ss.Name.Substring(0) != "E" && ss.Name != "HWL") { Xmin = dp.XValue; }
                    if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                    if (dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
                }
            }

            //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
            if(Xmin== 1000000)
            {
                Xmin = 0;
                Xmax = 0.4;
            }
            if(Ymin== 1000000)
            {
                Ymin = 0;
                Ymax = 0.4;
            }
            double xdiff = (Xmax - Xmin);
            double ydiff = (Ymax - Ymin);
            double xspace, yspace;
            xspace = 取得最佳Interval(Xmin, Xmax);
            yspace = 取得最佳Interval(Ymin, Ymax);
            //xspace = xdiff / 4.0;//5 label.
            //yspace = ydiff / 4.0;//5 label.


            //if (xdiff <= 1)
            //{
            //    xspace = 0.2;
            //}
            //else if (xdiff <= 5)
            //{
            //    xspace = 1;
            //}
            //else if (xdiff <= 10)
            //{
            //    xspace = 2;
            //}
            //else if (xdiff <= 20)
            //{
            //    xspace = 5;
            //}
            //else if (xdiff <= 50)
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
            double NewXmin = Math.Floor(Xmin / xspace) * xspace;
            double NewXmax =Math.Ceiling( Xmax/xspace)*xspace;// Xmin - xspace / 2.0 + 6.5 * xspace;// + Math.Floor((Xmax - Xmin) / xspace + 0.5) * xspace;
            double NewYmax = Math.Ceiling(Ymax/yspace)*yspace+yspace/2.0;// Ymin - yspace / 2.0 + 6.5 * yspace;
            double NewYmin = Math.Ceiling(Ymin / yspace) * yspace;
            //double NewYmax = Ymin + Math.Floor((Ymax - Ymin) / yspace + 0.5) * yspace;
            //label_Show.Text = Xmin.ToString() + "," + NewXmax.ToString(); // + ":" + NewYmax.ToString();

            INS.ChartAreas[0].AxisX.Minimum = NewXmin;// -xspace/2.0;// -xspace/2.0;// - xspace;
            INS.ChartAreas[0].AxisX.Maximum = NewXmax;// NewXmax;
            INS.ChartAreas[0].AxisX.Interval = xspace;
            INS.ChartAreas[0].AxisY.Minimum = NewYmin;// - yspace;
            INS.ChartAreas[0].AxisY.Maximum = NewYmax;// NewYmax;
            INS.ChartAreas[0].AxisY.Interval = yspace;
            INS.ChartAreas[0].RecalculateAxesScale();



            //繪上EL();

        }
        public double 取得最佳Interval(double MinV, double MaxV)
        {
            //Interval不能有過多之小數點,要盡量為非無限長之類型.
            // int[] NumberAvailable = new int[] { 2, 3, 4, 5, 8, 10 };
            //With ability to create an instance, but if you just want calculate and throw it away:
            //double tickX = new AxisAssists(aMaxX - aMinX, 8).Tick;
            AxisAssists A = new AxisAssists(MaxV - MinV, 5);
            double space = A.Tick;
            return space;
        }
    
        void 取得目前ELMIN與ELMAX(ref double[] ELMIN,ref double[] ELMAX,ref double AllCenterX)
        {
            //double xcenterright = -10000000;
            double xcenterright = 0;
            int allcount = 0;
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                double sumx = 0;
                Double[] XI = BlockMainArray[i].X;
                if (XI.GetLength(0) == 0)
                { continue; }
                
                for (int j = 0; j < XI.GetLength(0); j++)
                {
                    sumx = sumx + XI[j];
                }
                xcenterright += (sumx / (float)XI.GetLength(0));
                allcount += 1;
                //sumx = sumx / (float)XI.GetLength(0);
                //if (sumx > xcenterright) { xcenterright = sumx; }
            }
            AllCenterX = xcenterright / (float)allcount;//全部重心的x平均.
            Array.Resize(ref ELMIN, 2);
            Array.Resize(ref ELMAX, 2);
            ELMIN[0]= 1000000;
            ELMIN[1]= 1000000;
            ELMAX[0]= -100000;
            ELMAX[1]= -100000;
            //判斷.
            for (int i = 0; i < BlockMainArray.GetLength(0);i++)
            {
                Double[] XI = BlockMainArray[i].X;
                double[] YI = BlockMainArray[i].Y;
                if (XI.GetLength(0) == 0)
                { continue; }
                for(int j=0;j<XI.GetLength(0);j++)
                {
                    if(XI[j]<AllCenterX)
                    {
                        //Left Part.
                        if(YI[j]<ELMIN[0])
                        {
                            //左邊的最低點.
                            ELMIN[0] = YI[j];
                        }
                        if(YI[j]>ELMAX[0])
                        {
                            ELMAX[0] = YI[j];
                        }
                    }
                    if(XI[j]>AllCenterX)
                    {
                        //右邊的部分.
                        if(YI[j]<ELMIN[1])
                        {
                            //右邊的最低點.
                            ELMIN[1] = YI[j];
                        }
                        if(YI[j]>ELMAX[1])
                        {
                            ELMAX[1] = YI[j];
                        }
                    }
                }//Loop through each node of Blocks.
            }
        }
        void 繪上EL()
        {
            //若有EL,則調整之.
            //將ELDG的顏色全部變更為正常顏色.

            if (BlockCount == 0)
            {
                //MessageBox.Show("Block Count =0");
                //清除圖示
                //1. 清除HWL.
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["HWL"]);
                }
                catch
                {
                    //Do nothing.
                }
                //2. 清除箭頭.
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["ARROW"]);
                }
                catch
                {
                    //Do nothing.
                }
                //3. 清除EL.
                for (int i=0;i<ELSize;i++)
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
                //4.清除最大EL與最小EL[根據波向判斷].
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["EL_TOP"]);
                }
                catch
                {
                    //Do nothing.
                }
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["EL_BUTTOM"]);
                }
                catch
                {
                    //Do nothing.
                }
            }
            else
            {
                //MessageBox.Show("有Block喔");
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
                     chart_Plot.Series["HWL"].BorderColor = Color.Purple;
                     chart_Plot.Series["HWL"].Color = Color.Purple;
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

                //***********************************************************************************
                //2. 判斷目前最大EL與最小EL.
                // 首先判斷目前最右邊的Block是誰.
                double[] ELMINInner = new double[] { };
                double[] ELMAXInner = new double[] { };
                double XCenterX = -100000;
                取得目前ELMIN與ELMAX(ref ELMINInner, ref ELMAXInner,ref XCenterX);
                //根據目前選取的海向,決定ARROW圖與EL最大值與最小值.
                if(cmb_seawaveDir.SelectedItem.ToString()=="右")
                {
                    //[東].海側在右邊,箭頭往左邊畫.
                    MinEL = ELMINInner[1];
                    MaxEL = ELMAXInner[1];
                }
                else if(cmb_seawaveDir.SelectedItem.ToString()=="左")
                {
                    //[西].海側在左邊,箭頭往右邊.
                    MinEL = ELMINInner[0];
                    MaxEL = ELMAXInner[0];
                }
                //繪上ELMAX與ELMIN線段.
                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["EL_TOP"]);
                }
                catch
                {
                    //Do nothing.
                }
                chart_Plot.Series.Add("EL_TOP");
                chart_Plot.Series["EL_TOP"].ChartType = SeriesChartType.Line;

                //加入點資料.
                chart_Plot.Series["EL_TOP"].Points.AddXY(xmin, MaxEL);
                chart_Plot.Series["EL_TOP"].Points.AddXY(xmax, MaxEL);

                //設定線段.
                chart_Plot.Series["EL_TOP"].BorderDashStyle = ChartDashStyle.Dash;
                chart_Plot.Series["EL_TOP"].BorderColor = Color.Brown;
                chart_Plot.Series["EL_TOP"].Color = Color.Brown;
                chart_Plot.Series["EL_TOP"].BorderWidth = 2;
                chart_Plot.Series["EL_TOP"].IsVisibleInLegend = false;// true;

                try
                {
                    chart_Plot.Series.Remove(chart_Plot.Series["EL_BUTTOM"]);
                }
                catch
                {
                    //Do nothing.
                }
                chart_Plot.Series.Add("EL_BUTTOM");
                chart_Plot.Series["EL_BUTTOM"].ChartType = SeriesChartType.Line;

                //加入點資料.
                chart_Plot.Series["EL_BUTTOM"].Points.AddXY(xmin, MinEL);
                chart_Plot.Series["EL_BUTTOM"].Points.AddXY(xmax, MinEL);

                //設定線段.
                chart_Plot.Series["EL_BUTTOM"].BorderDashStyle = ChartDashStyle.Dash;
                chart_Plot.Series["EL_BUTTOM"].BorderColor = Color.Brown;
                chart_Plot.Series["EL_BUTTOM"].Color = Color.Brown;
                chart_Plot.Series["EL_BUTTOM"].BorderWidth = 2;
                chart_Plot.Series["EL_BUTTOM"].IsVisibleInLegend = false;// true;
                //***********************************************************************************

                //3. 繪上其餘EL線(與HWL線相同值者跳過).
                //先刪除舊的,在全部更新為新的.
                for (int i=0;i<ELSize;i++)
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
                            if(itest!=HWLValue && itest<MaxEL && itest>MinEL)
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
                            if (!repeated && itest<MaxEL && itest>MinEL)
                            {
                                Array.Resize(ref ELArray, ELSize + 1);
                                ELArray[ELSize] = itest;
                                ELSize += 1;
                            }
                        }
                    }
                }//完成Datagrid的蒐集.

                ////製作警示內容
                //string ELErrorMessage = "目前海側方向之EL最大值為" + MaxEL.ToString() + "(m),最小值為" + MinEL.ToString() + "(m)" + Environment.NewLine;
                //bool hasELError = false;
                //if( hasHWLValue && ( HWLValue>MaxEL || HWLValue<MinEL))
                //{
                //    //HWL超出目前Block海側方向之EL限度.
                //    ELErrorMessage+=( "您的HWL(m) = " + HWLValue.ToString() +"超過目前海側方向的EL範圍"+ Environment.NewLine );
                //    hasELError = true;
                //}
                //Dictionary<double, int> ErrorEL = new Dictionary<double, int>();
                //for (int i = 0; i < ELDGV1.Rows.Count - 1; i++)
                //{
                //    double itest;
                //    if (double.TryParse(ELDGV1.Rows[i].Cells[0].Value.ToString(), out itest))
                //    {
                //        if (hasHWLValue)
                //        {
                //            if (itest != HWLValue && (itest< MinEL || itest>MaxEL))
                //            {
                //                if(!ErrorEL.ContainsKey(itest))
                //                {
                //                    ELErrorMessage += ("您的EL值" + itest.ToString() + "超過目前海側方向的EL範圍" + Environment.NewLine);
                //                    ErrorEL.Add(itest, 1);
                //                    hasELError = true;
                //                }
                //            }
                //        }
                //        else
                //        {
                //            if(itest<MinEL || itest>MaxEL)
                //            {
                //                if (!ErrorEL.ContainsKey(itest))
                //                {
                //                    ELErrorMessage += ("您的EL值" + itest.ToString() + "超過目前海側方向的EL範圍" + Environment.NewLine);
                //                    ErrorEL.Add(itest, 1);
                //                    hasELError = true;
                //                }
                //            }
                //        }
                //    }
                //}



                //製作對應的EL線段.
                if (ELSize>0)
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
                        chart_Plot.Series[tpname].BorderColor = Color.Brown;
                        chart_Plot.Series[tpname].Color = Color.Brown;
                        chart_Plot.Series[tpname].BorderWidth = 2;
                        chart_Plot.Series[tpname].IsVisibleInLegend = false;
                    }
                }//若有可用之EL時.
                double Ymin = 1000000;
                double Ymax =-Ymin;
                foreach (Series ss in chart_Plot.Series)
                {
                    if(!BlockNameToArraySubscript.ContainsKey(ss.Name))
                    {
                        continue;
                    }
                    foreach (DataPoint dp in ss.Points)
                    {
                        //label_Show.Text += (dp.XValue.ToString());
                        //if (dp.XValue > Xmax) { Xmax = dp.XValue; }
                        //if (dp.XValue < Xmin) { Xmin = dp.XValue; }
                        
                        if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                        if (dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
                    }
                }
                //MessageBox.Show(BlockNameToArraySubscript.Count.ToString() + ";" + Ymax.ToString() + ";" + Ymin.ToString());


                //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
                //double xdiff = (Xmax - Xmin);
                if(Ymin== 1000000)
                {
                    Ymin = 0;
                    Ymax = 0.4;
                }
                double ydiff = (Ymax - Ymin);
                double  yspace;
                yspace = 取得最佳Interval(Ymin, Ymax);//;ydiff / 4.0;

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
                //double NewXmax = Xmin + Math.Floor((Xmax - Xmin) / xspace + 0.5) * xspace;
                ///MessageBox.Show(Ymax.ToString());
                //
                double NewYmin = Math.Floor(Ymin / yspace) * yspace;
                double NewYmax =Math.Ceiling(Ymax/yspace)*yspace+yspace/2.0;// Ymin + Math.Ceiling((Ymax - Ymin) / yspace) * yspace;
                //設定軸的範圍.
                chart_Plot.ChartAreas[0].AxisY.Minimum = NewYmin;// Ymin - yspace;
                chart_Plot.ChartAreas[0].AxisY.Maximum = NewYmax;
                chart_Plot.ChartAreas[0].AxisY.Interval = yspace;
                chart_Plot.ChartAreas[0].RecalculateAxesScale();
                //chart_Plot.Series.Add("ARROW");
                //chart_Plot.Series["ARROW"].ChartType = SeriesChartType.Line;
                //chart_Plot.Series["ARROW"].XValueType = ChartValueType.Time;
                chart_Plot.Annotations.Clear();//清除所有的Annotations.
                double xspace = chart_Plot.ChartAreas[0].AxisX.Interval;

                //海側文字.
                TextAnnotation SeaSidetext = new TextAnnotation();
                SeaSidetext.AxisX = chart_Plot.ChartAreas[0].AxisX;
                SeaSidetext.AxisY = chart_Plot.ChartAreas[0].AxisY;
                SeaSidetext.AnchorX = cmb_seawaveDir.SelectedItem.ToString()=="左"?(chart_Plot.ChartAreas[0].AxisX.Minimum+xspace/3.0):(chart_Plot.ChartAreas[0].AxisX.Maximum-xspace/3.0);
                SeaSidetext.AnchorY = NewYmax - yspace / 2.0;
                //MessageBox.Show("Ymax = " + NewYmax.ToString() + ", Ax= " + SeaSidetext.AnchorX.ToString() + " ,Ay = " + SeaSidetext.AnchorY.ToString());
                SeaSidetext.Font = new Font("微軟正黑體", 14, FontStyle.Bold);
                SeaSidetext.Text = "海側";// (cmb_seawaveDir.SelectedItem.ToString()=="E"?"<":"") + "========="+ (cmb_seawaveDir.SelectedItem.ToString()=="W"?">":"");
                chart_Plot.Annotations.Add(SeaSidetext);


                //畫上Block名稱.
                for(int i=0;i<BlockMainArray.GetLength(0);i++)
                {
                    TextAnnotation TT = new TextAnnotation();
                    double xcc = 0;
                    int PointCount = BlockMainArray[i].座標點數;
                    double ycc = 0;
                    double ymaxi = -1000000;
                    double ymini = -ymaxi;
                    double[] gx = BlockMainArray[i].X;
                    double[] gy = BlockMainArray[i].Y;
                    double aa = 0;
                    double w = 0;
                    double cx = 0;
                    double cy = 0;
                    for(int j=0;j<PointCount;j++)
                    {
                        
                        //xcc += (gx[j]);
                        //ycc += (gy[j]);
                        if (gy[j] > ymaxi) { ymaxi = gy[j]; }
                        if (gy[j] < ymini) { ymini = gy[j]; }

                    }
                    

                    for (int iii = PointCount - 1,i4=0;i4<PointCount;iii=i4++)
                    {
                        PointFMY F1;
                        F1.x = gx[iii];
                        F1.y = gy[iii];
                        F1.z = 0;
                        PointFMY F2;
                        F2.x = gx[i4];
                        F2.y = gy[i4];
                        F2.z = 0;
                        PointFMY F3=Cross(F1, F2);
                        aa = F3.z;
                        cx += ((F1.x + F2.x) * aa);
                        cy += ((F1.y + F2.y) * aa);
                        w += aa;
                    }
                    //xcc = xcc / (double)PointCount;
                    //ycc = ycc / (double)PointCount;
                    xcc = cx / 3.0 / w;
                    ycc = cy / 3.0 / w;
                    TT.Text =BlockListSubScriptToName[i];
                    TT.AllowMoving = true;
                    TT.AnchorAlignment = ContentAlignment.MiddleCenter;
                    TT.Font = new Font("微軟正黑體", 7, FontStyle.Bold);
                    TT.ForeColor = Color.OrangeRed;
                    //double hitX = chart_Plot.ChartAreas[0].AxisX.PixelPositionToValue(ex.X);
                    //double hitY = chart_Plot.ChartAreas[0].AxisY.PixelPositionToValue(ex.Y);
                    TT.AxisX = chart_Plot.ChartAreas[0].AxisX;
                    TT.AxisY = chart_Plot.ChartAreas[0].AxisY;
                    
                    //MessageBox.Show(chart_Plot.ChartAreas[0].AxisX.ValueToPixelPosition(xcc).ToString());
                    TT.AnchorX = xcc;//.AnchorX = xcc;//AnchorX = xcc; //cmb_seawaveDir.SelectedItem.ToString() == "W" ? (chart_Plot.ChartAreas[0].AxisX.Minimum + xspace / 3.0) : (chart_Plot.ChartAreas[0].AxisX.Maximum - xspace / 3.0);
                    TT.AnchorY = ycc; //(ycc + yspace / 2.0);//.AnchorY = ycc;//TT.Y= chart_Plot.ChartAreas[0].AxisY.ValueToPixelPosition(ycc);//AnchorY = ycc;// -(ymaxi- ymini)/10.0;// NewYmax - yspace / 2.0;
                                      //TT.X = xcc;
                                      //TT.Y = ycc;
                                      //string tpname = "TTTTL" + (i + 1).ToString();
                                      //chart_Plot.Series.Add(tpname);
                                      //chart_Plot.Series[tpname].ChartType = SeriesChartType.Line;
                    ///TT.AnchorOffsetX = 10;
                    //TT.AnchorOffsetY = 10;                                                 
                    ////加入點資料.
                    //chart_Plot.Series[tpname].Points.AddXY(xmin,ycc);
                    //chart_Plot.Series[tpname].Points.AddXY(xmax, ycc);

                    ////設定線段.
                    //chart_Plot.Series[tpname].Color = Color.Green ;
                    //chart_Plot.Series[tpname].BorderWidth = 2;
                    //chart_Plot.Series[tpname].IsVisibleInLegend = false;
                    //TT.TextStyle = TextStyle.Emboss;


                    chart_Plot.Annotations.Add(TT);
                }
                //chart_Plot.ChartAreas[0].AxisX.CustomLabels.Add((chart_Plot.ChartAreas[0].AxisX.Minimum + chart_Plot.ChartAreas[0].AxisX.Maximum) / 2.0, Ymax - yspace / 2.0, "====>", 1, LabelMarkStyle.LineSideMark);

                //加入點資料.
                //chart_Plot.Series["ARROW"].Points.AddXY((chart_Plot.ChartAreas[0].AxisX.Minimum+ chart_Plot.ChartAreas[0].AxisX.Maximum)/2.0, Ymax-yspace/2.0);
                //chart_Plot.Series["ARROW"].Points.AddXY(xmax, MaxEL);

                //設定線段.
                //chart_Plot.Series["ARROW"].BorderDashStyle = ChartDashStyle.Dash;
                //chart_Plot.Series["ARROW"].MarkerStyle = MarkerStyle.Star10;
                //chart_Plot.Series["ARROW"].MarkerSize = 8;

                //chart_Plot.Series["ARROW"].MarkerColor = Color.Black;
                //chart_Plot.Series["ARROW"].BorderWidth = 3;
                //chart_Plot.Series["ARROW"].IsValueShownAsLabel = true;
                //chart_Plot.Series["ARROW"].ToolTip = "#VALY=>#AXISLABEL";
                //chart_Plot.Series["ARROW"].IsVisibleInLegend = false;


                //if(hasELError)
                //{
                //    MessageBox.Show(ELErrorMessage, "EL與HWL設定錯誤", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //}
            }

        }
        
        private PointFMY Cross(PointFMY a , PointFMY b)
        {
            PointFMY RR;
            RR.x = a.y * b.z - b.y * a.z;
            RR.y=a.z* b.x - a.x * b.z;
            RR.z=a.x * b.y - a.y * b.x;
            return RR;
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
            listBox_SectSetting.Items[oldpos] = InterfaceBlock.名稱 + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[oldpos]);// "(" + BlockMainArray[oldpos].單位體積重量.ToString() + ")";
        }
         
        #endregion
        #region 利用XML儲存此專案,開一個新的,開啟舊的與另存新的.
        private void 儲存此專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
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
            string CheckTextBoxNoEmptyString = "";
            if (!CheckTextBoxNoEmpty(ref CheckTextBoxNoEmptyString))
            {
                FrmShowMsg ff = new FrmShowMsg(CheckTextBoxNoEmptyString, "資料未填完整");
                ff.Show();
                return;
            }
            儲存XML專案檔(xmlpath);
        }
        private void 儲存XML專案檔(string xmlfullpath,bool showDia=true )
        {
            //       
            string CheckTextBoxNoEmptyString = "";
            if (!CheckTextBoxNoEmpty(ref CheckTextBoxNoEmptyString))
            {
                FrmShowMsg ff = new FrmShowMsg(CheckTextBoxNoEmptyString, "資料未填完整");
                ff.Show();
                return;
            }


            XmlDocument doc = new XmlDocument();

            //*************************************************************************************
            //1. 基本姿態撰寫.
            //
            XmlElement Root = doc.CreateElement("Root");
            doc.AppendChild(Root);

            XmlElement 版本 = doc.CreateElement("版本");
            版本.SetAttribute("Value", "SeaDikeVS_P1");
            Root.AppendChild(版本);


            XmlElement 全域參數XML點 = doc.CreateElement("GlobalParameters");
            Root.AppendChild(全域參數XML點);


            //MessageBox.Show("H1");

            //*************************************************************************************
            //2. 全域參數設定區塊
            //建立子節點
            XmlElement 選擇Tab = doc.CreateElement("選擇Tab");
            選擇Tab.SetAttribute("Value", tabControl1.SelectedIndex.ToString());

            XmlElement 深海波波向info = doc.CreateElement("深海波波向");
            深海波波向info.SetAttribute("Value", cmb_DeepWaveActDir.SelectedItem.ToString());
        
            XmlElement 海側方向info = doc.CreateElement("海側方向");
            海側方向info.SetAttribute("Value",cmb_seawaveDir.SelectedItem.ToString());
            //MessageBox.Show("H1-1");

            XmlElement BlockToolTipinfo = doc.CreateElement("BlockTooltip資訊");
            BlockToolTipinfo.SetAttribute("Value", cmb_ShowOnBlockListChoice.SelectedItem.ToString());

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

            XmlElement HB計算值 = doc.CreateElement("HB值");
            HB計算值.SetAttribute("Value", textBox_HB.Text==""?"-9999":textBox_HB.Text .ToString());//若空白則寫入-9999.

            XmlElement 消波工堤身段港外側重量檢核啟用 = doc.CreateElement("消波工堤身段港外側重量檢核啟用");
            消波工堤身段港外側重量檢核啟用.SetAttribute("Value", chk_BlockWeightCalc_HO.Checked?"TRUE":"FALSE");

            XmlElement 消波工堤頭部加強重量檢核啟用 = doc.CreateElement("消波工堤頭部加強重量檢核啟用");
            消波工堤頭部加強重量檢核啟用.SetAttribute("Value", chk_BlockWeightCalc_HE.Checked ? "TRUE" : "FALSE");

            XmlElement 消波工堤身段航道側重量檢核啟用 = doc.CreateElement("消波工堤身段航道側重量檢核啟用");
            消波工堤身段航道側重量檢核啟用.SetAttribute("Value", chk_BlockWeightCalc_BD.Checked ? "TRUE" : "FALSE");

            XmlElement 胸牆部安定檢核啟用 = doc.CreateElement("胸牆部安定檢核啟用");
            胸牆部安定檢核啟用.SetAttribute("Value", chk_HeadBreastCalc.Checked ? "TRUE" : "FALSE");


            XmlElement 力矩計算參考點 = doc.CreateElement("力矩計算參考點");
            力矩計算參考點.SetAttribute("xValue", xo.ToString());
            力矩計算參考點.SetAttribute("yValue", yo.ToString());


            //加入所有參數節點到Global Parameters節點上
            全域參數XML點.AppendChild(選擇Tab);
            全域參數XML點.AppendChild(深海波波向info);
            全域參數XML點.AppendChild(BlockToolTipinfo);
            全域參數XML點.AppendChild(海側方向info);
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
            全域參數XML點.AppendChild(HB計算值);
            全域參數XML點.AppendChild(消波工堤身段港外側重量檢核啟用);
            if(chk_BlockWeightCalc_HO.Checked)
            {
                XmlElement 消波形塊安定係數 = doc.CreateElement("消波形塊安定係數");
                消波形塊安定係數.SetAttribute("Value", textBox_HO_KDL.Text);

                XmlElement 混凝土與海水之比重 = doc.CreateElement("混凝土單位體積重");
                混凝土與海水之比重.SetAttribute("Value", textBox_HO_Gamma.Text);

                XmlElement 消波塊斜坡面與水平面之夾角 = doc.CreateElement("消波塊斜坡面與水平面之夾角");
                消波塊斜坡面與水平面之夾角.SetAttribute("Value", textBox_HO_slopeangle.Text);


                消波工堤身段港外側重量檢核啟用.AppendChild(消波形塊安定係數);
                消波工堤身段港外側重量檢核啟用.AppendChild(混凝土與海水之比重);
                消波工堤身段港外側重量檢核啟用.AppendChild(消波塊斜坡面與水平面之夾角);
                
            }
            全域參數XML點.AppendChild(消波工堤頭部加強重量檢核啟用);
            if(chk_BlockWeightCalc_HE.Checked)
            {
                XmlElement 消波形塊安定係數 = doc.CreateElement("消波形塊安定係數");
                消波形塊安定係數.SetAttribute("Value", textBox_HE_KDL.Text);

                XmlElement 混凝土與海水之比重 = doc.CreateElement("混凝土單位體積重");
                混凝土與海水之比重.SetAttribute("Value", textBox_HE_Gamma.Text);

                XmlElement 消波塊斜坡面與水平面之夾角 = doc.CreateElement("消波塊斜坡面與水平面之夾角");
                消波塊斜坡面與水平面之夾角.SetAttribute("Value", textBox_HE_slopeangle.Text);


                消波工堤頭部加強重量檢核啟用.AppendChild(消波形塊安定係數);
                消波工堤頭部加強重量檢核啟用.AppendChild(混凝土與海水之比重);
                消波工堤頭部加強重量檢核啟用.AppendChild(消波塊斜坡面與水平面之夾角);
            }
            全域參數XML點.AppendChild(消波工堤身段航道側重量檢核啟用);
            if(chk_BlockWeightCalc_BD.Checked)
            {
                XmlElement 消波形塊安定係數 = doc.CreateElement("消波形塊安定係數");
                消波形塊安定係數.SetAttribute("Value", textBox_BD_KDL.Text);

                XmlElement 混凝土與海水之比重 = doc.CreateElement("混凝土單位體積重");
                混凝土與海水之比重.SetAttribute("Value", textBox_BD_Gamma.Text);

                XmlElement 消波塊斜坡面與水平面之夾角 = doc.CreateElement("消波塊斜坡面與水平面之夾角");
                消波塊斜坡面與水平面之夾角.SetAttribute("Value", textBox_BD_slopeangle.Text);

                XmlElement 波高傳遞率 = doc.CreateElement("波高傳遞率");
                波高傳遞率.SetAttribute("Value", textBox_BD_Kt.Text);

                消波工堤身段航道側重量檢核啟用.AppendChild(消波形塊安定係數);
                消波工堤身段航道側重量檢核啟用.AppendChild(混凝土與海水之比重);
                消波工堤身段航道側重量檢核啟用.AppendChild(消波塊斜坡面與水平面之夾角);
                消波工堤身段航道側重量檢核啟用.AppendChild(波高傳遞率);
            }

            全域參數XML點.AppendChild(胸牆部安定檢核啟用);
            if (chk_HeadBreastCalc.Checked)
            {
                XmlElement 混凝土容許應力 = doc.CreateElement("混凝土容許應力");
                混凝土容許應力.SetAttribute("Value", textBox_ConcreteAllowStress.Text);

                XmlElement BK = doc.CreateElement("BK");
                BK.SetAttribute("Value", textBox_BK.Text);

                XmlElement ELAbove = doc.CreateElement("ELAbove");
                ELAbove.SetAttribute("Value", textBox_ELAbove.Text);


                胸牆部安定檢核啟用.AppendChild(混凝土容許應力);
                胸牆部安定檢核啟用.AppendChild(BK);
                胸牆部安定檢核啟用.AppendChild(ELAbove);
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
            //材質與摩擦係數設定.
            // 1. 若摩擦係數沒有設定,Value填為"NONE"
            XmlElement 使用材質 = doc.CreateElement("使用材質");
            全域參數XML點.AppendChild(使用材質);
            for(int i=0;i<MaterialCount;i++)
            {
                XmlElement 使用材質I = doc.CreateElement("UseMaterial");
                使用材質I.SetAttribute("Value", MaterialArray[i].ToString());
                使用材質.AppendChild(使用材質I);
            }
            XmlElement 摩擦係數 = doc.CreateElement("摩擦係數");
            全域參數XML點.AppendChild(摩擦係數);
            for(int i=0;i<MaterialRoughnessArrayCount;i++)
            {
                XmlElement 摩擦係數I = doc.CreateElement("MaterialsFriction");
                if(MaterialsCoefArray[i].coef==-9999)
                {
                    摩擦係數I.SetAttribute("Coef", "");
                }
                else
                {
                    摩擦係數I.SetAttribute("Coef", MaterialsCoefArray[i].coef.ToString());
                }
                if(MaterialsCoefArray[i].Id1!=-9999)
                {
                    摩擦係數I.SetAttribute("Material1", MaterialArray[MaterialsCoefArray[i].Id1]);
                }
                else
                {
                    //沒有設定此參數.
                    摩擦係數I.SetAttribute("Material1", "");
                }
                if (MaterialsCoefArray[i].Id2 != -9999)
                {
                    摩擦係數I.SetAttribute("Material2", MaterialArray[MaterialsCoefArray[i].Id2]);
                }
                else
                {
                    //沒有設定此參數.
                    摩擦係數I.SetAttribute("Material2", "");
                }
                //摩擦係數I.SetAttribute("Material2", MaterialArray[MaterialsCoefArray[i].Id2]);
                摩擦係數.AppendChild(摩擦係數I);
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
                XmlElement Block是否計算Moment = doc.CreateElement("計算Moment");
                Block是否計算Moment.SetAttribute("Value", BlockMainArray[i].計算Moment與否.ToString());


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
                BlockNode.AppendChild(Block是否計算Moment);

                string[] 周圍參考材質 = BlockMainArray[i].周圍參考材質;
                for(int i2=0;i2<周圍參考材質.GetLength(0);i2++)
                {
                    XmlElement Block環繞參考材質 = doc.CreateElement("周圍參考材質");
                    Block環繞參考材質.SetAttribute("Value", 周圍參考材質[i2]);
                    BlockNode.AppendChild(Block環繞參考材質);
                }

                //MessageBox.Show("H4-2");

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
            if (showDia)
            {
                MessageBox.Show("儲存完畢!!!", "專案檔儲存", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //深海波波向info.SetAttribute("電話", "0806449");
        }
        private Boolean 是否為合適之深海波波向(string InputS)
        {
            bool r = false;
            //E
            //ESE
            //SE
            //SSE
            //S
            //SSW
            //SW
            //WSW
            //W
            //WNW
            //NW
            //NNW
            //N
            //NNE
            //NE
            //ENE
            switch(InputS)
            {
                case "E":
                    r = true;
                    break;
                case "ESE":
                    r = true;
                    break;
                case "SE":
                    r = true;
                    break;
                case "SSE":
                    r = true;
                    break;
                case "S":
                    r = true;
                    break;
                case "SSW":
                    r = true;
                    break;
                case "SW":
                    r = true;
                    break;
                case "WSW":
                    r = true;
                    break;
                case "W":
                    r = true;
                    break;
                case "WNW":
                    r = true;
                    break;
                case "NW":
                    r = true;
                    break;
                case "NNW":
                    r = true;
                    break;
                case "N":
                    r = true;
                    break;
                case "NNE":
                    r = true;
                    break;
                case "NE":
                    r = true;
                    break;
                case "ENE":
                    r = true;
                    break;
               //default:
               //     r = false;
                //    break;

            }
            return r;
        }
        private string 打開XML專案檔(string OpenPath)
        {
            //若失敗，會回傳失敗訊息.
            //若成功,會回傳空字串.
            string openMessage = "";
            double xor;// = 0.0; //已取消
            double yor;// = 0.0; //已取消

            
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
            double HBr;
            bool 啟用消波工堤身段港外側重量計算r;
            bool 啟用消波工堤頭部加強重量計算r;
            bool 啟用消波工堤身段航道側重量計算r;
            double 消波形塊堤身段港外側安定係數r;
            double 混凝土堤身段港外側單位體積重;
            double 消波塊堤身段港外側斜坡面與水平面夾角r;
            double 消波形塊堤頭部加強安定係數r;
            double 混凝土堤頭部加強單位體積重;
            double 消波塊堤頭部加強斜坡面與水平面夾角r;
            double 消波形塊堤身段航道側安定係數r;
            double 混凝土堤身段航道側單位體積重;
            double 消波塊堤身段航道側斜坡面與水平面夾角r;
            double 堤身段航道側波高傳遞率ktr;

            bool 啟用胸牆部安定檢核r;
            double 混凝土容許應力r;
            double BKr;
            double ELAbover;
            Dictionary<string, int> DDR = new Dictionary<string, int>();


            string dirr;
            string deepDirr;

            Class_BlockSect[] BlockMainArrayR = new Class_BlockSect[] { };
            int blockSizer = 0;
            int selectedBlockIndex=-1;
            double[] ELArrayR = new double[] { };
            int ELSizer = 0;

            string[] MaterialR = new string[] { };
            int MaterialCountR = 0;
            MaterialsRoughness[] MaterialsCoefArrayR = new MaterialsRoughness[] { };
            int MaterialsCoefCountR = 0;

            int SelectedTab = -1;
            string BlockToolTip資訊選擇;


            //開始來開啟.
            XmlDocument doc = new XmlDocument();
            doc.Load(OpenPath);
            try
            {
                //全域參數讀取
                XmlNode RNode = doc.SelectSingleNode("Root/版本");
                if(object.Equals(RNode,null))
                {
                    return "版本控制標籤讀取失敗";
                }
                XmlElement Relement = (XmlElement)RNode;
                if(Relement.GetAttribute("Value").ToString()!= "SeaDikeVS_P1")
                {
                    return "版本控制標籤錯誤!!";
                }

                //選擇Tab.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/選擇Tab");
                if (object.Equals(RNode, null))
                {
                    return "選擇Tab讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!int.TryParse(Relement.GetAttribute("Value").ToString(),out SelectedTab))
                {
                    return "選擇Tab值錯誤";
                }

                //深海波波向.
                RNode =doc.SelectSingleNode("Root/GlobalParameters/深海波波向");
                if (object.Equals(RNode, null))
                {
                    return "深海波波向讀取失敗";
                }
                Relement = (XmlElement)RNode;
                deepDirr = Relement.GetAttribute("Value");
                if (!是否為合適之深海波波向(deepDirr))
                {
                    return "深海波波向值錯誤" + deepDirr;
                }

                //海側方向.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/海側方向");
                if (object.Equals(RNode, null))
                {
                    return "海側方向讀取失敗";
                }
                Relement = (XmlElement)RNode;
                dirr = Relement.GetAttribute("Value");
                if (dirr != "左" && dirr != "右") // && dirr != "W" && dirr != "S")
                {
                    return "海側方向值錯誤";
                }
                //Block Tooltip資訊選擇.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/BlockTooltip資訊");
                if (object.Equals(RNode, null))
                {
                    return "Block Tooltip資訊讀取失敗";
                }
                Relement = (XmlElement)RNode;
                BlockToolTip資訊選擇 = Relement.GetAttribute("Value").ToString();
                if (BlockToolTip資訊選擇!="無" && BlockToolTip資訊選擇!= "單位體積重" && BlockToolTip資訊選擇!= "Moment計算" && BlockToolTip資訊選擇!= "材質")
                {
                    return "Block Tooltip資訊讀取失敗";
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

                //HB值.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/HB值");
                if (object.Equals(RNode, null))
                {
                    return "HB值讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out HBr))
                {
                    return "HB值讀取失敗";
                }

                //消波工重量檢核啟用.
                //1. 堤身段港外側
                RNode = doc.SelectSingleNode("Root/GlobalParameters/消波工堤身段港外側重量檢核啟用");
                if (object.Equals(RNode, null))
                {
                    return "消波工堤身段港外側重量檢核啟用讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 啟用消波工堤身段港外側重量計算r))
                {
                    return "消波工堤身段港外側重量檢核啟用讀取失敗";
                }
                XmlNode 啟用消波工堤身段港外側Node = RNode;

                if (啟用消波工堤身段港外側重量計算r)
                {
                    //消波形塊安定係數.
                    RNode = 啟用消波工堤身段港外側Node.SelectSingleNode("消波形塊安定係數");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段港外側之消波形塊安定係數讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波形塊堤身段港外側安定係數r))
                    {
                        return "堤身段港外側之消波形塊安定係數讀取失敗";
                    }

                    //混凝土單位體積重.
                    RNode = 啟用消波工堤身段港外側Node.SelectSingleNode("混凝土單位體積重");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段港外側之混凝土單位體積重讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土堤身段港外側單位體積重))
                    {
                        return "堤身段港外側之混凝土單位體積重讀取失敗";
                    }

                    //消波塊斜坡面與水平面之夾角.
                    RNode = 啟用消波工堤身段港外側Node.SelectSingleNode("消波塊斜坡面與水平面之夾角");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段港外側之消波塊斜坡面與水平面之夾角讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波塊堤身段港外側斜坡面與水平面夾角r))
                    {
                        return "堤身段港外側之消波塊斜坡面與水平面之夾角讀取失敗";
                    }

                }
                else
                {
                    消波形塊堤身段港外側安定係數r = 0;
                    混凝土堤身段港外側單位體積重 = 0;
                    消波塊堤身段港外側斜坡面與水平面夾角r = 0;
                }
                //消波工重量檢核啟用.
                //2. 堤頭部加強
                RNode = doc.SelectSingleNode("Root/GlobalParameters/消波工堤頭部加強重量檢核啟用");
                if (object.Equals(RNode, null))
                {
                    return "消波工堤頭部加強重量檢核啟用讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 啟用消波工堤頭部加強重量計算r))
                {
                    return "消波工堤頭部加強重量檢核啟用讀取失敗";
                }
                XmlNode 啟用消波工堤頭部加強Node = RNode;

                if (啟用消波工堤頭部加強重量計算r)
                {
                    //消波形塊安定係數.
                    RNode = 啟用消波工堤頭部加強Node.SelectSingleNode("消波形塊安定係數");
                    if (object.Equals(RNode, null))
                    {
                        return "堤頭部加強之消波形塊安定係數讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波形塊堤頭部加強安定係數r))
                    {
                        return "堤頭部加強之消波形塊安定係數讀取失敗";
                    }

                    //混凝土單位體積重.
                    RNode = 啟用消波工堤頭部加強Node.SelectSingleNode("混凝土單位體積重");
                    if (object.Equals(RNode, null))
                    {
                        return "堤頭部加強之混凝土單位體積重讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土堤頭部加強單位體積重))
                    {
                        return "堤頭部加強之混凝土單位體積重讀取失敗";
                    }

                    //消波塊斜坡面與水平面之夾角.
                    RNode = 啟用消波工堤頭部加強Node.SelectSingleNode("消波塊斜坡面與水平面之夾角");
                    if (object.Equals(RNode, null))
                    {
                        return "堤頭部加強之消波塊斜坡面與水平面之夾角讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波塊堤頭部加強斜坡面與水平面夾角r))
                    {
                        return "堤頭部加強之消波塊斜坡面與水平面之夾角讀取失敗";
                    }

                }
                else
                {
                    消波形塊堤頭部加強安定係數r = 0;
                    混凝土堤頭部加強單位體積重 = 0;
                    消波塊堤頭部加強斜坡面與水平面夾角r = 0;
                }
                //消波工重量檢核啟用.
                //3. 堤頭部加強
                RNode = doc.SelectSingleNode("Root/GlobalParameters/消波工堤身段航道側重量檢核啟用");
                if (object.Equals(RNode, null))
                {
                    return "消波工堤身段航道側重量檢核啟用讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 啟用消波工堤身段航道側重量計算r))
                {
                    return "消波工堤身段航道側重量檢核啟用讀取失敗";
                }
                XmlNode 啟用消波工堤身段航道側Node = RNode;

                if (啟用消波工堤身段航道側重量計算r)
                {
                    //消波形塊安定係數.
                    RNode = 啟用消波工堤身段航道側Node.SelectSingleNode("消波形塊安定係數");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段航道側之消波形塊安定係數讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波形塊堤身段航道側安定係數r))
                    {
                        return "堤身段航道側之消波形塊安定係數讀取失敗";
                    }

                    //混凝土單位體積重.
                    RNode = 啟用消波工堤身段航道側Node.SelectSingleNode("混凝土單位體積重");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段航道側之混凝土單位體積重讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土堤身段航道側單位體積重))
                    {
                        return "堤身段航道側之混凝土單位體積重讀取失敗";
                    }

                    //消波塊斜坡面與水平面之夾角.
                    RNode = 啟用消波工堤身段航道側Node.SelectSingleNode("消波塊斜坡面與水平面之夾角");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段航道側之消波塊斜坡面與水平面之夾角讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 消波塊堤身段航道側斜坡面與水平面夾角r))
                    {
                        return "堤身段航道側之消波塊斜坡面與水平面之夾角讀取失敗";
                    }
                    //波高傳遞率.
                    RNode = 啟用消波工堤身段航道側Node.SelectSingleNode("波高傳遞率");
                    if (object.Equals(RNode, null))
                    {
                        return "堤身段航道側之波高傳遞率讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 堤身段航道側波高傳遞率ktr))
                    {
                        return "堤身段航道側之波高傳遞率讀取失敗";
                    }

                }
                else
                {
                    消波形塊堤身段航道側安定係數r = 0;
                    混凝土堤身段航道側單位體積重 = 0;
                    消波塊堤身段航道側斜坡面與水平面夾角r = 0;
                    堤身段航道側波高傳遞率ktr = 0;
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
                XmlNode 啟用胸牆部檢核 = RNode;
                if(啟用胸牆部安定檢核r)
                {
                    //胸牆部安定檢核啟用.
                    RNode = 啟用胸牆部檢核.SelectSingleNode("混凝土容許應力");
                    if (object.Equals(RNode, null))
                    {
                        return "胸牆部安定檢核之混凝土容許應力讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out 混凝土容許應力r))
                    {
                        return "胸牆部安定檢核之混凝土容許應力讀取失敗";
                    }

                    //胸牆部安定檢核啟用.
                    RNode = 啟用胸牆部檢核.SelectSingleNode("BK");
                    if (object.Equals(RNode, null))
                    {
                        return "胸牆部安定檢核之BK讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out BKr))
                    {
                        return "胸牆部安定檢核之BK讀取失敗";
                    }

                    //EL Above.
                    RNode = 啟用胸牆部檢核.SelectSingleNode("ELAbove");
                    if (object.Equals(RNode, null))
                    {
                        return "胸牆部安定檢核之EL Above讀取失敗";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out ELAbover))
                    {
                        return "胸牆部安定檢核之EL Above讀取失敗";
                    }
                }
                else
                {
                    混凝土容許應力r = 0;
                    BKr = 0;
                    ELAbover = 0;
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

                //使用材質設定.
                XmlNodeList 使用材質Collection = doc.SelectNodes("Root/GlobalParameters/使用材質/UseMaterial");
                foreach(XmlNode 使用材質Node in 使用材質Collection)
                {
                    Relement = (XmlElement)使用材質Node;
                    //if(!double.TryParse(Relement.GetAttribute("Value").ToString(),out fi))
                    //{
                    //    return "讀取使用材質失敗!";
                    //}
                    Array.Resize(ref MaterialR, MaterialCountR + 1);
                    try { MaterialR[MaterialCountR] = Relement.GetAttribute("Value"); }
                    catch { return "讀取使用材質失敗!"; }
                    MaterialCountR += 1;
                }
                for(int i=0;i<MaterialR.GetLength(0);i++)
                {
                    DDR.Add(MaterialR[i], i);
                }
                //摩擦係數組合設定.
                XmlNodeList 摩擦係數Collection = doc.SelectNodes("Root/GlobalParameters/摩擦係數/MaterialsFriction");
                foreach(XmlNode 摩擦係數Node in 摩擦係數Collection)
                {
                    double fi;
                    Relement = (XmlElement)摩擦係數Node;
                    Array.Resize(ref MaterialsCoefArrayR, MaterialsCoefCountR + 1);
                    if(Relement.GetAttribute("Coef").ToString()=="")
                    {
                        MaterialsCoefArrayR[MaterialsCoefCountR].coef = -9999;//A signal to specify this is empty.
                    }
                    else
                    {
                        if (!double.TryParse(Relement.GetAttribute("Coef").ToString(),out fi))
                        {
                            return "讀取材質間摩擦係數值錯誤";
                        }
                        else
                        {
                            MaterialsCoefArrayR[MaterialsCoefCountR].coef = fi;
                        }
                    }
                    if(Relement.GetAttribute("Material1").ToString()=="")
                    {
                        //Not specified.
                        MaterialsCoefArrayR[MaterialsCoefCountR].Id1 = -9999;
                    }
                    else
                    {
                        try { MaterialsCoefArrayR[MaterialsCoefCountR].Id1 = DDR[Relement.GetAttribute("Material1").ToString()]; }
                        catch { return "讀取材質間摩擦係數設定錯誤!找不到Material1材質名稱'" + Relement.GetAttribute("Material1").ToString() + "'"; }
                    }
                    if (Relement.GetAttribute("Material2").ToString() == "")
                    {
                        //Not specified.
                        MaterialsCoefArrayR[MaterialsCoefCountR].Id2 = -9999;
                    }
                    else
                    {
                        try { MaterialsCoefArrayR[MaterialsCoefCountR].Id2 = DDR[Relement.GetAttribute("Material2").ToString()]; }
                        catch { return "讀取材質間摩擦係數設定錯誤!找不到Material2材質名稱'" + Relement.GetAttribute("Material2").ToString() + "'"; }
                    }
                    MaterialsCoefCountR += 1;
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
                    bool btest;
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

                    //計算Moment與否.
                    RNode = BlockNode.SelectSingleNode("計算Moment");
                    if (object.Equals(RNode, null))
                    {
                        return "Block讀取計算Moment狀況失敗!";
                    }
                    Relement = (XmlElement)RNode;
                    if (!bool.TryParse(Relement.GetAttribute("Value"), out btest))
                    {
                        return "Block讀取計算Moment狀況失敗!";
                    }
                    BlockMainArrayR[blockSizer].計算Moment與否 = btest;

                    //Block使用材質
                    RNode = BlockNode.SelectSingleNode("使用材質");
                    if(object.Equals(RNode,null))
                    {
                        return "Block讀取Block使用材質失敗!!";
                    }
                    Relement = (XmlElement)RNode;
                    if(Relement.GetAttribute("Value").ToString()!="")
                    {
                        if(!DDR.ContainsKey(Relement.GetAttribute("Value").ToString()))
                        {
                            return "Block讀取Block使用材質失敗!!";
                        }
                        else
                        {
                            //Nothing.
                        }
                    }
                    BlockMainArrayR[blockSizer].使用材質 = Relement.GetAttribute("Value");

                    string[] GetRE = new string[] { };
                    XmlNodeList Block周圍參考材質 = BlockNode.SelectNodes("周圍參考材質");
                    if(!object.Equals(Block周圍參考材質,null))
                    {
                        foreach (XmlNode MD in Block周圍參考材質)
                        {
                            Relement = (XmlElement)MD;
                            if(!DDR.ContainsKey(Relement.GetAttribute("Value").ToString()))
                            {
                                return "Block讀取周圍參考材質";
                            }
                            Array.Resize(ref GetRE, GetRE.GetLength(0) + 1);
                            GetRE[GetRE.GetUpperBound(0)] = Relement.GetAttribute("Value");
                        }
                    }
                    BlockMainArrayR[blockSizer].周圍參考材質 = GetRE;


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
            //檢查Block的座標順序
            Form_BlockNameAndCorrdinate p = new Form_BlockNameAndCorrdinate();
            for(int i=0;i<BlockMainArrayR.GetLength(0);i++)
            {
                if(!p.CheckIsConvexPolygonAndCounterClockWise(BlockMainArrayR[i].X,BlockMainArrayR[i].Y))
                {
                    //若排列順序非逆時針.
                    return "Block" + (i + 1).ToString() + "(從1開始)的座標矩陣錯誤,此Block非凸邊形且座標沒有依照逆時針方向紀錄!!";
                }
            }

            //==============================================================================================//
            //沒問題之後,開始替代所有設定.
            //全域參數.
            cmb_DeepWaveActDir.SelectedItem =deepDirr;
            cmb_seawaveDir.SelectedItem = dirr;
            cmb_ShowOnBlockListChoice.SelectedItem = BlockToolTip資訊選擇;
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
            textBox_HB.Text = HBr == -9999 ? "" : HBr.ToString();//若為-9999,則視為空白.

            if(啟用消波工堤身段港外側重量計算r)
            {
                textBox_HO_KDL.ReadOnly = false;
                textBox_HO_Gamma.ReadOnly = false;
                textBox_HO_slopeangle.ReadOnly = false;

                textBox_HO_KDL.Enabled = true;
                textBox_HO_Gamma.Enabled = true;
                textBox_HO_slopeangle.Enabled = true;

                label_HO_1.Enabled = true;
                label_HO_2.Enabled = true;
                label_HO_3.Enabled = true;


                chk_BlockWeightCalc_HO.Checked = true;
                textBox_HO_KDL.Text = 消波形塊堤身段港外側安定係數r.ToString();
                textBox_HO_Gamma.Text = 混凝土堤身段港外側單位體積重.ToString();
                textBox_HO_slopeangle.Text = 消波塊堤身段港外側斜坡面與水平面夾角r.ToString();
            }
            else
            {
                textBox_HO_KDL.ReadOnly = true;
                textBox_HO_Gamma.ReadOnly = true;
                textBox_HO_slopeangle.ReadOnly = true;

                label_HO_1.Enabled = false;
                label_HO_2.Enabled = false;
                label_HO_3.Enabled = false;

                textBox_HO_KDL.Enabled = false;
                textBox_HO_Gamma.Enabled = false;
                textBox_HO_slopeangle.Enabled = false;


                chk_BlockWeightCalc_HO.Checked = false;
                textBox_HO_KDL.Text = "";
                textBox_HO_Gamma.Text = "";
                textBox_HO_slopeangle.Text = "";

            }
            if(啟用消波工堤頭部加強重量計算r)
            {
                textBox_HE_KDL.ReadOnly = false;
                textBox_HE_Gamma.ReadOnly = false;
                textBox_HE_slopeangle.ReadOnly = false;

                textBox_HE_KDL.Enabled = true;
                textBox_HE_Gamma.Enabled = true;
                textBox_HE_slopeangle.Enabled = true;

                label_HE_1.Enabled = true;
                label_HE_2.Enabled = true;
                label_HE_3.Enabled = true;


                chk_BlockWeightCalc_HE.Checked = true;
                textBox_HE_KDL.Text = 消波形塊堤頭部加強安定係數r.ToString();
                textBox_HE_Gamma.Text = 混凝土堤頭部加強單位體積重.ToString();
                textBox_HE_slopeangle.Text = 消波塊堤頭部加強斜坡面與水平面夾角r.ToString();
            }
            else
            {
                textBox_HE_KDL.ReadOnly = true;
                textBox_HE_Gamma.ReadOnly = true;
                textBox_HE_slopeangle.ReadOnly = true;

                label_HE_1.Enabled = false;
                label_HE_2.Enabled = false;
                label_HE_3.Enabled = false;

                textBox_HE_KDL.Enabled = false;
                textBox_HE_Gamma.Enabled = false;
                textBox_HE_slopeangle.Enabled = false;


                chk_BlockWeightCalc_HE.Checked = false;
                textBox_HE_KDL.Text = "";
                textBox_HE_Gamma.Text = "";
                textBox_HE_slopeangle.Text = "";
            }
            if(啟用消波工堤身段航道側重量計算r)
            {

                textBox_BD_KDL.ReadOnly = false;
                textBox_BD_Gamma.ReadOnly = false;
                textBox_BD_slopeangle.ReadOnly = false;
                textBox_BD_Kt.ReadOnly = false;

                textBox_BD_KDL.Enabled = true;
                textBox_BD_Gamma.Enabled = true;
                textBox_BD_slopeangle.Enabled = true;
                textBox_BD_Kt.Enabled = true;

                label_BD_1.Enabled = true;
                label_BD_2.Enabled = true;
                label_BD_3.Enabled = true;
                label_BD_4.Enabled = true;

                chk_BlockWeightCalc_BD.Checked = true;
                textBox_BD_KDL.Text = 消波形塊堤身段航道側安定係數r.ToString();
                textBox_BD_Gamma.Text = 混凝土堤身段航道側單位體積重.ToString();
                textBox_BD_slopeangle.Text = 消波塊堤身段航道側斜坡面與水平面夾角r.ToString();
                textBox_BD_Kt.Text = 堤身段航道側波高傳遞率ktr.ToString();
            }
            else
            {
                textBox_BD_KDL.ReadOnly = true;
                textBox_BD_Gamma.ReadOnly = true;
                textBox_BD_slopeangle.ReadOnly = true;
                textBox_BD_Kt.ReadOnly = true;

                label_BD_1.Enabled = false;
                label_BD_2.Enabled = false;
                label_BD_3.Enabled = false;
                label_BD_4.Enabled = false;

                textBox_BD_KDL.Enabled = false;
                textBox_BD_Gamma.Enabled = false;
                textBox_BD_slopeangle.Enabled = false;
                textBox_BD_Kt.Enabled = false;

                chk_BlockWeightCalc_BD.Checked = false;
                textBox_BD_KDL.Text = "";
                textBox_BD_Gamma.Text = "";
                textBox_BD_slopeangle.Text = "";
                textBox_BD_Kt.Text = "";
            }

            if (啟用胸牆部安定檢核r)
            {
                textBox_ConcreteAllowStress.ReadOnly = false;
                textBox_BK.ReadOnly = false;
                textBox_ELAbove.ReadOnly = false;
                textBox_ConcreteAllowStress.Enabled = true;
                textBox_BK.Enabled = true;
                textBox_ELAbove.Enabled = true;
                label_BreastCheck_1.Enabled = true;
                label_BrestCheck_2.Enabled = true;
                label_BrestCheck_3.Enabled = true;

                chk_HeadBreastCalc.Checked = true;
                textBox_ConcreteAllowStress.Text = 混凝土容許應力r.ToString();
                textBox_BK.Text =BKr.ToString();
                textBox_ELAbove.Text = ELAbover.ToString();
            }
            else
            {
                textBox_ConcreteAllowStress.ReadOnly = true;
                textBox_BK.ReadOnly = true;
                textBox_ELAbove.ReadOnly = true;
                label_BreastCheck_1.Enabled = false;
                label_BrestCheck_2.Enabled = false;
                label_BrestCheck_3.Enabled = false;
                textBox_ConcreteAllowStress.Enabled=false;
                textBox_BK.Enabled = false;
                textBox_ELAbove.Enabled = false;

                chk_HeadBreastCalc.Checked = false;
                textBox_ConcreteAllowStress.Text = ""; // 混凝土容許應力r.ToString();
                textBox_BK.Text = ""; // BKr.ToString();
                textBox_ELAbove.Text = "";
            }

            DGMaterial.Rows.Clear();
            MaterialArray = MaterialR;
            MaterialCount = MaterialCountR;
            MaterialNameToArraySubScript.Clear();
            MaterialSubscriptToName.Clear();
            for(int i=0;i<MaterialArray.GetLength(0);i++)
            {
                DGMaterial.Rows.Add(new object[] { (i + 1).ToString(), MaterialArray[i].ToString() });
                MaterialNameToArraySubScript.Add(MaterialR[i].ToString(), i);
                MaterialSubscriptToName.Add(i, MaterialR[i].ToString());
            }
            MaterialsCoefArray = MaterialsCoefArrayR;
            MaterialRoughnessArrayCount = MaterialsCoefCountR;
            DGMaterialRough.Rows.Clear();
            for(int i=0;i<MaterialsCoefArray.GetLength(0);i++)
            {
                DGMaterialRough.Rows.Add(new object[] { (i + 1).ToString(), MaterialsCoefArray[i].Id1 == -9999 ? "" : MaterialSubscriptToName[MaterialsCoefArray[i].Id1], MaterialsCoefArray[i].Id2 == -9999 ? "" : MaterialSubscriptToName[MaterialsCoefArray[i].Id2], MaterialsCoefArray[i].coef == -9999 ? "" : MaterialsCoefArray[i].coef.ToString() });
            }
            //設定下拉式選單.
            //設定Combobox內容.
            foreach (DataGridViewRow row in DGMaterialRough.Rows)
            {
                var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                cell.DataSource = MaterialArray;
                var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                cell2.DataSource = MaterialArray;
            }



            //Block區塊,填入變數.
            listBox_SectSetting.Items.Clear();
            ReferencedMaterialCHKL.Items.Clear();
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
                BlockNameToArraySubscript.Add(BlockMainArray[i].名稱, i);
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
                //chart_Plot.Series[BlockMainArray[i].名稱].BorderColor = Color.Black;
                chart_Plot.Series[BlockMainArray[i].名稱].Color = Color.Black;
                chart_Plot.Series[BlockMainArray[i].名稱].BorderWidth = 2;
                chart_Plot.Update();
                //chart_Plot.Series[BlockMainArray[i].名稱].MarkerBorderWidth = 2; //Color.Black;
                listBox_SectSetting.Items.Add(BlockMainArray[i].名稱 + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[i]));// "(" + BlockMainArray[i].單位體積重量.ToString() + ")");
            }
            if (BlockMainArray.GetLength(0) > 0)
            {
                //MessageBox.Show("H1" + chart_Plot.Series.Count.ToString());
                ELDGV1.Enabled = true;
                調整Chart(chart_Plot);
                繪上EL();
            }

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
            if (BlockMainArray.GetLength(0) > 0)
            {
                string Msg = "";
                開始檢核ToolStripMenuItem.Enabled = (mainForm.檢視目前是否已有合理認證(ref Msg) && true);// (mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref Msg) && true);
                btn_Test.Enabled = 開始檢核ToolStripMenuItem.Enabled;
            }
            else
            {
                開始檢核ToolStripMenuItem.Enabled = false;
                btn_Test.Enabled = false;
            }
            btn_LogOutput.Enabled = false;
            btn_OutputExcel.Enabled = false;
            輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
            tsp_cond.Text = "請設定或編輯您的專案檔";
            tsp_progressbar.Visible = false;
            if(SelectedTab!=-1 && SelectedTab>=0 && SelectedTab<=tabControl1.TabCount-1)
            {
                tabControl1.SelectedIndex = SelectedTab;
            }
            //==============================================================================================//

            EscapeDGMaterialCellValueChangedFunction = false;
            EscapeDGMaterialRoughnessCellContentChanged = false;
            return "";

        }
        
        private void 開啟舊的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
            //此Function會呼叫開啟XML專案檔的程式.
            string openpath = "";
            if(OFD_專案.ShowDialog()==DialogResult.OK && OFD_專案.FileName!="")
            { openpath = OFD_專案.FileName; }
            else
            { return; }


            使用者手動更新材質與摩擦 = false;
            string 開啟檔案之訊息 = 打開XML專案檔(openpath);
            使用者手動更新材質與摩擦 = true;
            if (開啟檔案之訊息=="")
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
            if (isExporting) { return; }
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
            this.Close();

        }

        private void 另存專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
            if (BlockMainArray.GetLength(0)==0)
            { MessageBox.Show("您沒有設定任何形塊!無法儲存", "專案檔管理", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string xmlpath;// = workfoldernow + "\\Test.xml";
           if (SFD_專案.ShowDialog() == DialogResult.OK && SFD_專案.FileName!="")
           {
                xmlpath = SFD_專案.FileName; //路徑.
            }
           else
            { return; }

            string CheckTextBoxNoEmptyString = "";
            if (!CheckTextBoxNoEmpty(ref CheckTextBoxNoEmptyString))
            {
                FrmShowMsg ff = new FrmShowMsg(CheckTextBoxNoEmptyString, "您有資料未正確填完");
                ff.Show();
                return;
            }

            儲存XML專案檔(xmlpath);
            this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(xmlpath);
        }

        #endregion
        #region 消波工重量檢核計算打開
        private void chk_BlockWeightCalc_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BlockWeightCalc_HO.Checked)
            {
                label_HO_1.Enabled = true;
                label_HO_2.Enabled = true;
                label_HO_3.Enabled = true;
                label_BD_4.Enabled = true;
                textBox_HO_KDL.ReadOnly = false;
                textBox_HO_Gamma.ReadOnly = false;
                textBox_HO_slopeangle.ReadOnly = false;
                textBox_BD_Kt.ReadOnly = false;
                textBox_HO_KDL.Enabled = true;
                textBox_HO_Gamma.Enabled = true;
                textBox_HO_slopeangle.Enabled = true;
                textBox_BD_Kt.Enabled = true;
            }
            else
            {
              label_HO_1.Enabled = false;
              label_HO_2.Enabled = false;
              label_HO_3.Enabled = false;
              label_BD_4.Enabled = false;
              textBox_HO_KDL.ReadOnly = true;
              textBox_HO_Gamma.ReadOnly = true;
              textBox_HO_slopeangle.ReadOnly = true;
              textBox_BD_Kt.ReadOnly = true;
                textBox_HO_KDL.Enabled = false;
                textBox_HO_Gamma.Enabled = false;
                textBox_HO_slopeangle.Enabled = false;
                textBox_BD_Kt.Enabled = false;
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
                label_BreastCheck_1.Enabled = true;
                label_BrestCheck_2.Enabled = true;
                textBox_ConcreteAllowStress.ReadOnly = false;
                textBox_BK.ReadOnly = false;
                textBox_ConcreteAllowStress.Enabled = true;
                textBox_BK.Enabled = true;
            }
            else
            {
                label_BreastCheck_1.Enabled = false;
                label_BrestCheck_2.Enabled = false;
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
        Boolean CheckTextBoxNoEmpty(ref string ErrorMsg)
        {
            ErrorMsg = "";
            bool okOrNot = true;//True is Ok[No faults].

            if (cmb_seawaveDir.SelectedItem.ToString() == "")
            {
                ErrorMsg += ("您深海波波向沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_H0.Text.ToString() == "")
            {
                ErrorMsg += ("您深海波波高沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波高沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_T0.Text.ToString() == "")
            {
                ErrorMsg += ("您深海波週期沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
               // MessageBox.Show("您深海波週期沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
               //return false;
            }
            if(textBox_GroundELE.Text.ToString()=="")
            {
                ErrorMsg += ("您地面線(m)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您地面線(m)沒有設定!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_ArmorBlockEle.Text.ToString() == "")
            {
                ErrorMsg += ("您消波塊高程(m)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您消波塊高程(m)沒有設定!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_HWL.Text.ToString() == "")
            {
                ErrorMsg += ("您設計潮位(HWL)(m)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您設計潮位沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Slope.Text.ToString() == "")
            {
                ErrorMsg += ("您海床坡度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您海床坡度沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Kr.Text.ToString() == "")
            {
                ErrorMsg += ("您折射係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您折射係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Ks.Text.ToString() == "")
            {
                ErrorMsg += ("您淺化係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您淺化係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Kd.Text.ToString() == "")
            {
                ErrorMsg += ("您繞射係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您繞射係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Lenda.Text.ToString() == "")
            {
                ErrorMsg += ("您波力折減係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您波力折減係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Beta.Text.ToString() == "")
            {
                ErrorMsg += ("您入射波與堤體法線垂直交角沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您入射波與堤體法線垂直交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_BataFix.Text.ToString() == "")
            {
                ErrorMsg += ("您入射波與堤體法線垂直交角(修正後)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您入射波與堤體法線垂直交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_SFSlide.Text.ToString() == "")
            {
                ErrorMsg += ("您滑動安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您滑動安全係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_SFOver.Text.ToString() == "")
            {
                ErrorMsg += ("您傾倒安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您傾倒安全係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_SeaGamma.Text.ToString() == "")
            {
                ErrorMsg += ("您海水單位體積重量沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您海水單位體積重量沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (chk_BlockWeightCalc_HO.Checked)
            {
                if (textBox_HO_KDL.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段港外側」的消波形塊安定係數沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波形塊安定係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_HO_Gamma.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段港外側」的混凝土單位體積重沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您混凝土與海水之比重沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_HO_slopeangle.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段港外側」的消波塊斜坡面與水平面之交角沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波塊斜坡面與水平面之交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
               
            }
            if (chk_BlockWeightCalc_HE.Checked)
            {
                if (textBox_HE_KDL.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤頭部加強」的消波形塊安定係數沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波形塊安定係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_HE_Gamma.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤頭部加強」的混凝土單位體積重沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您混凝土與海水之比重沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_HE_slopeangle.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤頭部加強」的消波塊斜坡面與水平面之交角沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波塊斜坡面與水平面之交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }

            }
            if (chk_BlockWeightCalc_BD.Checked)
            {
                if (textBox_BD_KDL.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段航道側」的消波形塊安定係數沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波形塊安定係數沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_BD_Gamma.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段航道側」的混凝土單位體積重沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您混凝土與海水之比重沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_BD_slopeangle.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段航道側」的消波塊斜坡面與水平面之交角沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您消波塊斜坡面與水平面之交角沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_BD_Kt.Text.ToString() == "")
                {
                    ErrorMsg += ("您「堤身段航道側」的波高傳遞率Kt沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您波高傳遞率Kt沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }

            }
            if (chk_HeadBreastCalc.Checked)
            {
                if (textBox_ConcreteAllowStress.Text.ToString() == "")
                {
                    ErrorMsg += ("您「胸牆部安定檢核」的混凝土容許應力沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您「胸牆部安定檢核」的混凝土容許應力沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_BK.Text.ToString() == "")
                {
                    ErrorMsg += ("您「胸牆部安定檢核」的BK沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您「胸牆部安定檢核」的BK'沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
                if (textBox_ELAbove.Text.ToString() == "")
                {
                    ErrorMsg += ("您「胸牆部安定檢核」的EL Above沒有選擇!!!" + Environment.NewLine);
                    okOrNot = false;
                    //MessageBox.Show("您「胸牆部安定檢核」的BK'沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //return false;
                }
            }
            return okOrNot;
        }
        Boolean CanRunTheCalc(ref string ErrorMsg)
        {
            ErrorMsg = "";
            bool okOrNot = true;
            //檢查是否每個Block都設定完成.
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                //使用材質不可為空白.
                if(BlockMainArray[i].使用材質.ToString()=="")
                {
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的使用材質沒有設定!!" + Environment.NewLine);
                    okOrNot = false;
                    //return false;
                }
                double fi;
                if(!double.TryParse(BlockMainArray[i].單位體積重量.ToString(),out fi))
                {
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的單位體積重量設定錯誤!" + Environment.NewLine);
                    okOrNot = false;
                    //return false;
                }
                //座標不可為空白.
                if(BlockMainArray[i].座標點數==0)
                {
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的座標點數為零!!" + Environment.NewLine);
                    okOrNot = false;
                    //return false;
                }
                if(!MaterialNameToArraySubScript.ContainsKey(BlockMainArray[i].使用材質))
                {
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的使用材質並沒有被定義過!!" + Environment.NewLine);
                    okOrNot = false;
                    //return false;
                }

                //環繞之參考材質不可為空白[至少要一個],且設定的材質不可找不到其摩擦係數設定.
                string[] BlockReferencedMaterial = BlockMainArray[i].周圍參考材質;
                if(BlockReferencedMaterial.GetLength(0)==0)
                {
                    //環繞之參考材質無設定.
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的周圍參考材質沒有設定!!!" + Environment.NewLine);
                    okOrNot = false;
                }
                if(BlockMainArray[i].使用材質.ToString()!="")
                {
                    //在有設定使用材質的情況下檢查摩擦係數有沒有設定才有意義.
                    //任何一個參考設定都不可以缺漏.
                    string M1 = BlockMainArray[i].使用材質;
                    for(int j=0;j<BlockReferencedMaterial.GetLength(0);j++)
                    {
                        bool fullfill = false;
                        string M2 = BlockReferencedMaterial[j];
                        for(int k=0;k<MaterialsCoefArray.GetLength(0);k++)
                        {
                            string C1 = MaterialSubscriptToName[MaterialsCoefArray[k].Id1];
                            string C2 = MaterialSubscriptToName[MaterialsCoefArray[k].Id2];
                            if (C1==M1 && C2==M2 && MaterialsCoefArray[k].coef!=-9999)
                            {
                                fullfill = true;
                                break;
                            }
                            else if(C1==M2 && C2==M1 && MaterialsCoefArray[k].coef != -9999)
                            {
                                fullfill = true;
                                break;
                            }
                        }
                        if(!fullfill)
                        {
                            ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'使用材質為'" + M1 + "'，周圍參考材質'" + M2 + "'找不到設定完成的材質間摩擦係數!!!" + Environment.NewLine);
                            okOrNot = false;
                        }
                    }
                }

                //Polygon需逆時針.
                Form_BlockNameAndCorrdinate p = new Form_BlockNameAndCorrdinate();
               
                if (!p.CheckIsConvexPolygonAndCounterClockWise(BlockMainArray[i].X, BlockMainArray[i].Y))
                {
                    //若排列順序非逆時針.
                    ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的座標矩陣錯誤,此Block非凸邊形且座標沒有依照逆時針方向紀錄!!" + Environment.NewLine);
                    okOrNot = false;
                    //return false;
                 }

            }
            //Block有用到的材質間之摩擦係數不可為無設定.
            Dictionary<string, int> BlockUseMaterialNameCount = new Dictionary<string, int>();
            string[] BlockUseMaterial = new string[] { };
            
            for (int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                string 使用材質 = BlockMainArray[i].使用材質;
                if(使用材質!="")
                {
                    if(BlockUseMaterialNameCount.ContainsKey(使用材質))
                    {
                        BlockUseMaterialNameCount[使用材質] += 1;
                    }
                    else
                    {
                        Array.Resize(ref BlockUseMaterial, BlockUseMaterial.GetLength(0) + 1);
                        BlockUseMaterial[BlockUseMaterial.GetUpperBound(0)] = 使用材質;
                        BlockUseMaterialNameCount.Add(使用材質, 1);
                    }
                }
            }
            //用簡單方法來檢查摩擦係數有沒有設定.
            bool frictionError = false;
            for(int i=0;i<BlockUseMaterial.GetLength(0);i++)
            {
                string M1 = BlockUseMaterial[i];
                for(int j=0;j<BlockUseMaterial.GetLength(0);j++)
                {
                    string M2 = BlockUseMaterial[j];
                    bool hasThis = false ;
                    int fullfillcount = 0; ;//Checking if has any two same materials with different roughness.
                    //Finding effective record.
                    for(int k=0;k<MaterialsCoefArray.GetLength(0);k++)
                    {
                        if(MaterialsCoefArray[k].Id1==-9999 || MaterialsCoefArray[k].Id2==-9999)
                        {
                            continue;
                        }

                        string C1 = MaterialSubscriptToName[MaterialsCoefArray[k].Id1];
                        string C2 = MaterialSubscriptToName[MaterialsCoefArray[k].Id2];
                        if(M1==C1 && M2==C2)
                        {
                            fullfillcount += 1;//不管摩擦係數有沒有設定,有兩列完全一樣的材質設定就是錯誤.
                            if (MaterialsCoefArray[k].coef != -9999)
                                hasThis = true;
                        }
                        else if(M1==C2 && M2==C1)
                        {
                            fullfillcount += 1;//不管摩擦係數有沒有設定,有兩列完全一樣的材質設定就是錯誤.
                            if (MaterialsCoefArray[k].coef != -9999)
                                hasThis = true;
                        }

                    }
                    if(fullfillcount!=1)
                    {
                        frictionError = true;
                        //return false;
                    }
                    if (!hasThis)
                    { frictionError = true; }// return false; }
                }
            }
            if(frictionError)
            {
                ErrorMsg += ("摩擦係數設定有誤!有某一列或多列之材質間摩擦係數設定衝突或是沒有設定摩擦係數值!!" + Environment.NewLine);
                okOrNot = false;
            }


            //檢查EL設定是否正確.
            //1-1. 取得海側方向之EL Maximum與Minimum.
            double[] ELMINInner = new double[] { };
            double[] ELMAXInner = new double[] { };
            double XCenterX = -100000;
            取得目前ELMIN與ELMAX(ref ELMINInner, ref ELMAXInner, ref XCenterX);
            //根據目前選取的海向,決定ARROW圖與EL最大值與最小值.
            if (cmb_seawaveDir.SelectedItem.ToString() == "右")
            {
                //[東].海側在右邊,箭頭往左邊畫.
                MinEL = ELMINInner[1];
                MaxEL = ELMAXInner[1];
            }
            else if (cmb_seawaveDir.SelectedItem.ToString() == "左")
            {
                //[西].海側在左邊,箭頭往右邊.
                MinEL = ELMINInner[0];
                MaxEL = ELMAXInner[0];
            }
            double HWLValue = -9999;
            bool hasHWLValue = false;
            if(double.TryParse(textBox_HWL.Text.ToString(),out HWLValue))
            {
                hasHWLValue = true;
            }
            else
            {
                //無法轉換HWL.
                ErrorMsg += ("無法成功轉換設計潮位HWL值(m)!!!!" + Environment.NewLine);
                okOrNot = false;
            }
            string ELErrorMessage=( "目前海側方向之EL最大值為" + MaxEL.ToString() + "(m),最小值為" + MinEL.ToString() + "(m)" + Environment.NewLine);
            bool hasELError = false;
            if (hasHWLValue && (HWLValue > MaxEL || HWLValue < MinEL))
            {
                //HWL超出目前Block海側方向之EL限度.
                ELErrorMessage += ("您的HWL(m) = " + HWLValue.ToString() + "超過目前海側方向的EL範圍" + Environment.NewLine);
                hasELError = true;
            }
            Dictionary<double, int> ErrorEL = new Dictionary<double, int>();
            for (int i = 0; i < ELDGV1.Rows.Count - 1; i++)
            {
                double itest;
                if (double.TryParse(ELDGV1.Rows[i].Cells[0].Value.ToString(), out itest))
                {
                    if (hasHWLValue)
                    {
                        if (itest != HWLValue && (itest < MinEL || itest > MaxEL))
                        {
                            if (!ErrorEL.ContainsKey(itest))
                            {
                                ELErrorMessage += ("您的EL值" + itest.ToString() + "超過目前海側方向的EL範圍" + Environment.NewLine);
                                ErrorEL.Add(itest, 1);
                                hasELError = true;
                            }
                        }
                    }
                    else
                    {
                        if (itest < MinEL || itest > MaxEL)
                        {
                            if (!ErrorEL.ContainsKey(itest))
                            {
                                ELErrorMessage += ("您的EL值" + itest.ToString() + "超過目前海側方向的EL範圍" + Environment.NewLine);
                                ErrorEL.Add(itest, 1);
                                hasELError = true;
                            }
                        }
                    }
                }
            }
            if(hasELError)
            {
                ErrorMsg += (ELErrorMessage + Environment.NewLine);
                okOrNot = false;
            }

            //Polygon之間不可相交.

            //ErrorMsg += ("Block '" + BlockMainArray[i].名稱 + "'的座標矩陣錯誤,此Block非凸邊形且座標沒有依照逆時針方向紀錄!!" + Environment.NewLine);


            return okOrNot;
        }
        private void 開始檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
            //檢核前預檢查.

            //檢查是否有綁訂機碼.
            string 驗證Msg = "";
            if (mainForm.檢視目前是否已有合理認證(ref 驗證Msg)) //mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref 驗證Msg))
            {
                //Nothing.
            }
            else
            {
                this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')嘗試進行標準海堤檢核但缺乏軟體驗證遭到阻擋,員工編號為'" + mainForm.LoginInUserID + "',員工名稱為'" + mainForm.LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                MessageBox.Show("您無法使用此功能!!錯誤訊息:" + Environment.NewLine + 驗證Msg, "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //******************************************************
            string CheckTextBoxString = "";
            if(!CheckTextBoxNoEmpty(ref CheckTextBoxString))
            {
                FrmShowMsg FF = new FrmShowMsg(CheckTextBoxString,"您有資料未填完成或填入值不正確");
                FF.Show();
                btn_OutputExcel.Enabled = false;
                輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
                btn_LogOutput.Enabled = false;
                return;
            }

            string RunCalcString = "";
            if(!CanRunTheCalc(ref RunCalcString))
            {
                //彈出一個視窗來顯示錯誤資訊.
                FrmShowMsg FF = new FrmShowMsg(RunCalcString, "您有資料未填完成或填入值不正確");
                FF.Show();
                //失敗時,把原先的東西都關掉.
                btn_OutputExcel.Enabled = false;
                輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
                btn_LogOutput.Enabled = false;
                return;//禁止往下繼續進行.
            }
            //******
            //計算每個Block的摩擦係數.
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                string M1 = BlockMainArray[i].使用材質;
                string[] M2C = BlockMainArray[i].周圍參考材質;
                double sumv = 0;
                for(int j=0;j<M2C.GetLength(0);j++)
                {
                    string M2 = M2C[j];
                    double getv=-9999;
                    for(int k=0;k<MaterialsCoefArray.GetLength(0);k++)
                    {
                        string C1 = MaterialSubscriptToName[MaterialsCoefArray[k].Id1];
                        string c2 = MaterialSubscriptToName[MaterialsCoefArray[k].Id2];
                        double v1 = MaterialsCoefArray[k].coef;
                        if(v1==-9999)
                        {
                            continue;
                        }
                        if(C1==M1 && c2==M2)
                        {
                            getv = v1;
                            break;
                        }
                        else if(C1==M2 && c2==M1)
                        {
                            getv = v1;
                            break;
                        }
                    }
                    if(getv==-9999)
                    {
                        MessageBox.Show("出現程式錯誤!!!!此時應該要排除找不到摩擦係數的問題!!!");
                        btn_OutputExcel.Enabled = false;
                        輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
                        btn_LogOutput.Enabled = false;
                        return;
                    }
                    sumv += getv;
                }
                sumv = sumv / (double)M2C.GetLength(0);
                BlockMainArray[i].平均摩擦係數 = sumv;
            }

            //***********************
           

            //***********************************************************************************************************************//
            //帶入計算
            // Class New
            Mod = new Module1();
            Mod.DeleteAllBlockData();

            //1. H0, HWL, 海水r.
            Mod.WaterDesignInput(double.Parse(textBox_H0.Text), double.Parse(textBox_HWL.Text), double.Parse(textBox_SeaGamma.Text));
            //2. 波向, T0, Kr, Ks , Kd, lambda, beta.
            Mod.WaveDesignInput(cmb_seawaveDir.SelectedItem.ToString().ToLower() == "右" ? 1 : 0, double.Parse(textBox_T0.Text), double.Parse(textBox_Kr.Text), double.Parse(textBox_Ks.Text), double.Parse(textBox_Kd.Text), double.Parse(textBox_Lenda.Text), double.Parse(textBox_BataFix.Text));
            //3. S(海床坡度), 底面線, 消波塊高層
            Mod.BaseDesignInput(double.Parse(textBox_Slope.Text), double.Parse(textBox_GroundELE.Text), double.Parse(textBox_ArmorBlockEle.Text));
            //4. 選擇性參數給定
            double HBTest;
            Mod.OptionalVarInput(double.TryParse(textBox_HB.Text,out HBTest)? HBTest :-9999);
            //5.   其他檢核:
            //5-1. 堤身段(外側)消波工檢核計算.
            if (chk_BlockWeightCalc_HO.Checked )
            {
                Mod.WaveBreakOutsideCondition(true);
                Mod.WaveBreakOutsideInput(double.Parse(textBox_HO_Gamma.Text), double.Parse(textBox_HO_KDL.Text), double.Parse(textBox_HO_slopeangle.Text));
            }
            else
            {
                Mod.WaveBreakOutsideCondition(false);
            }
            //5-2. 堤頭部加強(上側)檢核計算.
            if (chk_BlockWeightCalc_HE.Checked)
            {
                Mod.WaveBreakUpsideCondition(true);
                Mod.WaveBreakUpsideInput(double.Parse(textBox_HE_Gamma.Text), double.Parse(textBox_HE_KDL.Text), double.Parse(textBox_HE_slopeangle.Text));
            }
            else
            {
                Mod.WaveBreakUpsideCondition(false);
            }
            //5-3. 堤身段(航道側)檢核計算.
            if (chk_BlockWeightCalc_BD.Checked)
            {
                Mod.WaveBreakInsideCondition(true);
                Mod.WaveBreakInsideInput(double.Parse(textBox_BD_Gamma.Text), double.Parse(textBox_BD_KDL.Text), double.Parse(textBox_BD_slopeangle.Text), double.Parse(textBox_BD_Kt.Text));
            }
            else
            {
                Mod.WaveBreakInsideCondition(false);
            }
            //5-4. 胸牆部安定檢核
            if (chk_HeadBreastCalc.Checked)
            {
                Mod.UpperBlockCheckCondition(true);
                Mod.UpperBlockCheckInput(double.Parse(textBox_ConcreteAllowStress.Text), double.Parse(textBox_BK.Text), double.Parse(textBox_ELAbove.Text));
            }
            else
            {
                Mod.UpperBlockCheckCondition(false);
            }

            //6. Block給定.
            for (int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                //- 迴圈塞入Block.
                // 2016/03/29. 新增是否計算Moment選項.
                int nowid = Mod.NewBlock(BlockMainArray[i].單位體積重量, BlockMainArray[i].平均摩擦係數, BlockMainArray[i].計算Moment與否);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                int 座標點數 = BlockMainArray[i].座標點數;
                for(int i2=0;i2<座標點數;i2++)
                {
                    Mod.SetBlockCoord(nowid, getx[i2], gety[i2]);
                }
            }


            //7. Level給定.
            //- 創建排序 ELA (包含HWL)
            double[] ELA = new double[] {};
            ELA = ELArray;
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = double.Parse(textBox_HWL.Text);
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = MinEL;
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = MaxEL;
            Array.Sort(ELA);
            //- Push Level
            Mod.DeleteAllLevel();
            for(int i = 0; i<ELA.GetLength(0); ++i)
            {
                Mod.NewLevel(ELA[i]);
            }
            //8. SF Input
            Mod.SF_CoefInput(double.Parse(textBox_SFSlide.Text), double.Parse(textBox_SFOver.Text));

           
            //ELArray[0]
            //**********************************************************************************************************************//

            //Mod.VarBank.G //Get all Var
            //Mod.VarBank.

            //計算.
            bool SuccessOrNot = Mod.Run();
            textBox_CheckMessageShow.Text = Mod.ErrMsg;
            if (!SuccessOrNot)
            {
                MessageBox.Show("檢核計算失敗!!!!!", "海堤檢核計算:失敗", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btn_LogOutput.Enabled = false;
                btn_OutputExcel.Enabled = false;
                輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
            }
            else
            {
                
                MessageBox.Show("檢核計算完成", "海堤檢核計算", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btn_LogOutput.Enabled = true;
                btn_OutputExcel.Enabled = true ;
                輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
            }
            //結果呈現.
            tabControl1.SelectedIndex = 4; //更換頁面.
            //檢核完成,更新RRCOL[紀錄Textbox內容]
            載入Textbox到矩陣內();
            this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')已成功進行標準海堤檢核,員工編號為'" + mainForm.LoginInUserID + "',員工名稱為'" + mainForm.LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

        }
        private void 載入Textbox到矩陣內()
        {
            RCOL = new RDExameTextBox_Object_Class();
            RCOL.深海波波向 = cmb_DeepWaveActDir.SelectedItem.ToString();
            RCOL.海側方向 = cmb_seawaveDir.SelectedItem.ToString();
            RCOL.深海波波高 = textBox_H0.Text.ToString();
            RCOL.深海波週期 = textBox_T0.Text.ToString();
            RCOL.地面線 = textBox_GroundELE.Text.ToString();
            RCOL.消波塊高程 = textBox_ArmorBlockEle.Text.ToString();
            RCOL.設計潮位 = textBox_HWL.Text.ToString();
            RCOL.海床波度 = textBox_Slope.Text.ToString();
            RCOL.折射係數 = textBox_Kr.Text.ToString();
            RCOL.淺化係數 = textBox_Ks.Text.ToString();
            RCOL.繞射係數 = textBox_Kd.Text.ToString();
            RCOL.波力折減係數 = textBox_Lenda.Text.ToString();
            RCOL.入射波與堤體法線之垂線夾角未修正 = textBox_Beta.Text.ToString();
            RCOL.入射波與堤體法線之垂線夾角修正後 = textBox_BataFix.Text.ToString();
            RCOL.滑動安全係數 = textBox_SFSlide.Text.ToString();
            RCOL.傾倒安全係數 = textBox_SFOver.Text.ToString();
            RCOL.海水單位體積重量 = textBox_SeaGamma.Text.ToString();
            RCOL.HB = textBox_HB.Text.ToString();

            RCOL.堤身段港外側消波工重量計算 = chk_BlockWeightCalc_HO.Checked;
            RCOL.堤身段港外側消波形塊安定係數 = textBox_HO_KDL.Text.ToString();
            RCOL.堤身段港外側消波塊單位體積重量 = textBox_HO_Gamma.Text.ToString();
            RCOL.堤身段港外側消波塊斜坡面與水平面夾角 = textBox_HO_slopeangle.Text.ToString();

            RCOL.堤頭部加強消波工重量計算 = chk_BlockWeightCalc_HE.Checked;
            RCOL.堤頭部加強消波形塊安定係數 = textBox_HE_KDL.Text.ToString();
            RCOL.堤頭部加強消波塊單位體積重量 = textBox_HE_Gamma.Text.ToString();
            RCOL.堤頭部加強消波塊斜坡面與水平面夾角 = textBox_HE_slopeangle.Text.ToString();

            RCOL.堤身段航道側消波工重量計算 = chk_BlockWeightCalc_BD.Checked;
            RCOL.堤身段航道側消波形塊安定係數 = textBox_BD_KDL.Text.ToString();
            RCOL.堤身段航道側消波塊單位體積重量 = textBox_BD_Gamma.Text.ToString();
            RCOL.堤身段航道側消波塊斜坡面與水平面夾角 = textBox_BD_slopeangle.Text.ToString();
            RCOL.堤身段航道側波高傳遞率 = textBox_BD_Kt.Text.ToString();

            RCOL.胸牆部安定檢核計算 = chk_HeadBreastCalc.Checked;
            RCOL.胸牆部安定檢核計算混凝土容許應力 = textBox_ConcreteAllowStress.Text.ToString();
            RCOL.胸牆部安定檢核計算BKplun= textBox_BK.Text.ToString();
            RCOL.胸牆部安定EL以上 = textBox_ELAbove.Text.ToString();

            RCOL.填表人ID=mainForm.LoginInUserID;
            RCOL.填表人名稱 = mainForm.LoginInUserName;

        }
        #endregion
        #region 輸出檢核結果EXCEL表單
        private void bkOutputExcelFile_DoWork(object sender, DoWorkEventArgs e)
        {
            string getpath = e.Argument.ToString();

        }
        private void bkOutputExcelFile_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //什麼都不做.
           // MessageBox.Show(e.UserState.ToString());

        }
        private string 寫出檢核結果Excel表(string getpath)
        {
            Excel.Application excelApp;
            Excel._Workbook wBook;
            Excel._Worksheet wSheet;
            Excel.Range range;

            excelApp = new Excel.Application();
            excelApp.Visible = false;// true;//出現.
            excelApp.DisplayAlerts = false;
            excelApp.Workbooks.Add(Type.Missing);
            wBook = excelApp.Workbooks[1];//第一個活頁簿.
            wBook.Activate();

            string PNGStoredFolderPath=Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP.PNG";
            string getMsg = "OK";

            //執行EXCEL 輸出.
            try
            {
                //輸出Chart圖檔.
                string[] TempName = new string[] { };
                for(int i=0;i<chart_Plot.Series.Count;i++)
                {
                    if(chart_Plot.Series[i].IsVisibleInLegend)
                    {
                        Array.Resize(ref TempName, TempName.GetLength(0) + 1);
                        TempName[TempName.GetUpperBound(0)] = chart_Plot.Series[i].Name;
                        chart_Plot.Series[i].IsVisibleInLegend = false;
                    }
                }
                //chart_Plot.Legends.
                if(listBox_SectSetting.SelectedIndex!=-1)
                {
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].Color = Color.Black;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].BorderWidth = 2;
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderColor = Color.Black;
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderWidth = 2;
                }
                chart_Plot.SaveImage(PNGStoredFolderPath, ChartImageFormat.Png);
                for(int i=0;i<TempName.GetLength(0);i++)
                {
                    chart_Plot.Series[TempName[i]].IsVisibleInLegend = false;
                }
                if (listBox_SectSetting.SelectedIndex != -1)
                {
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].Color = Color.Red;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].BorderWidth =3;
                }
                Mod.Get_DataBank_Data(); //Loading Running Result data.
                wSheet = (Excel._Worksheet)wBook.Worksheets[1];//第一個工作表.
                wSheet.Name = "海堤檢核計算結果報表";
                wSheet.Activate();
                wSheet.Shapes.AddPicture(PNGStoredFolderPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, 150,0 , 520, 290);
                //===========================================================================================
                //1-1. 設計條件參數
                range = wSheet.Cells[1, 1];
                range.Value = "二、設計條件參數";
                range.Font.Bold = true;
                wSheet.get_Range("A1", "B1").Merge(wSheet.get_Range("A1", "B1").MergeCells);

                range = wSheet.Cells[2, 1];
                range.Value = "深海波波向";
                range = wSheet.Cells[2, 2];
                range.Value = cmb_seawaveDir.SelectedItem.ToString();// Mod.VarBank.alpha1.ToString();
                range = wSheet.Cells[3, 1];
                range.Value = "深海波波高(m)";
                range = wSheet.Cells[3, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.H0.ToString();
                range = wSheet.Cells[4, 1];
                range.Value = "深海波週期(sec)";
                range = wSheet.Cells[4, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_T0.Text.ToString();
                range = wSheet.Cells[5, 1];
                range.Value = "設計潮位(m)";
                range = wSheet.Cells[5, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_HWL.Text.ToString();
                range = wSheet.Cells[6, 1];
                range.Value = "海床坡度";
                range = wSheet.Cells[6, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Slope.Text.ToString();
                range = wSheet.Cells[7, 1];
                range.Value = "折射係數";
                range = wSheet.Cells[7, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Kr.Text.ToString();
                range = wSheet.Cells[8, 1];
                range.Value = "淺化係數";
                range = wSheet.Cells[8, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Ks.Text.ToString();
                range = wSheet.Cells[9, 1];
                range.Value = "繞射係數";
                range = wSheet.Cells[9, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Kd.Text.ToString();
                range = wSheet.Cells[10, 1];
                range.Value = "波力折減係數";
                range = wSheet.Cells[10, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Lenda.Text.ToString();
                range = wSheet.Cells[11, 1];
                range.Value = "入射波與堤體髮線之垂線夾角";
                range = wSheet.Cells[11, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Beta.Text.ToString();
                range = wSheet.Cells[12, 1];
                range.Value = "地面線(m)";
                range = wSheet.Cells[12, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_GroundELE.Text.ToString();
                range = wSheet.Cells[13, 1];
                range.Value = "消波塊高程(m)";
                range = wSheet.Cells[13, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_ArmorBlockEle.Text.ToString();
                if (textBox_HB.Text.ToString() != "")
                {
                    range = wSheet.Cells[14, 1];
                    range.Value = "Hb";
                    range = wSheet.Cells[14, 2];
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = textBox_HB.Text.ToString();
                }

                //加上框線.
                range = wSheet.Range[wSheet.Cells[2, 1], wSheet.Cells[textBox_HB.Text.ToString() == "" ? 13 : 14, 2]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
                range.Columns.AutoFit();
                //===========================================================================================

                //===========================================================================================
                //1-2 外力計算.
                range = wSheet.Cells[16, 1];
                range.Value = "三、外力計算";
                range.Font.Bold = true;
                wSheet.get_Range("A16", "B16").Merge(wSheet.get_Range("A16", "B16").MergeCells);
                range = wSheet.Cells[17, 1];
                range.Value = "1.水深條件";
                wSheet.get_Range("A17", "B17").Merge(wSheet.get_Range("A17", "B17").MergeCells);

                range = wSheet.Cells[18, 1];
                range.Value = "h(m)";
                range = wSheet.Cells[18, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.h.ToString();
                range = wSheet.Cells[19, 1];
                range.Value = "h'(m)";
                range = wSheet.Cells[19, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.h_plun.ToString();
                range = wSheet.Cells[20, 1];
                range.Value = "hc(m)";
                range = wSheet.Cells[20, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.hc.ToString();
                range = wSheet.Cells[21, 1];
                range.Value = "d(m)";
                range = wSheet.Cells[21, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.d.ToString();
                range = wSheet.Cells[22, 1];
                range.Value = "2.有義波高H1/3及最大波高Hmax";
                wSheet.get_Range("A22", "B22").Merge(wSheet.get_Range("A22", "B22").MergeCells);
                range = wSheet.Cells[23, 1];
                range.Value = "L0(m)";
                range = wSheet.Cells[23, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.L0.ToString();
                range = wSheet.Cells[24, 1];
                range.Value = "HO'(m)";
                range = wSheet.Cells[24, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.H0_plun.ToString();
                range = wSheet.Cells[25, 1];
                range.Value = "L(m)";
                range = wSheet.Cells[25, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.L.ToString();
                range = wSheet.Cells[26, 1];
                range.Value = "h/L0";
                range = wSheet.Cells[26, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.h_D_L0.ToString();
                range = wSheet.Range[wSheet.Cells[17, 1], wSheet.Cells[26, 2]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                range = wSheet.Cells[27, 1];
                range.Value = "H1/3略算係數";
                wSheet.get_Range("A27", "C27").Merge(wSheet.get_Range("A27", "C27").MergeCells);
                range = wSheet.Cells[27, 4];
                range.Value = "Hmax略算係數";
                wSheet.get_Range("D27", "F27").Merge(wSheet.get_Range("D27", "F27").MergeCells);
                range = wSheet.Cells[28, 1];
                range.Value = "\u03B20";
                range = wSheet.Cells[29, 1];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.beta0.ToString();
                range = wSheet.Cells[28, 2];
                range.Value = "\u03B21";
                range = wSheet.Cells[29, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.beta1.ToString();
                range = wSheet.Cells[28, 3];
                range.Value = "\u03B2max";
                range = wSheet.Cells[29, 3];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.betaMax.ToString();
                range = wSheet.Cells[28, 4];
                range.Value = "\u03B20*";
                range = wSheet.Cells[29, 4];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.beta0_Star.ToString();
                range = wSheet.Cells[28, 5];
                range.Value = "\u03B21*";
                range = wSheet.Cells[29, 5];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.beta1_Star.ToString();
                range = wSheet.Cells[28, 6];
                range.Value = "\u03B2max*";
                range = wSheet.Cells[29, 6];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.betaMax_Star.ToString();
                range = wSheet.Range[wSheet.Cells[27, 1], wSheet.Cells[29, 6]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                range = wSheet.Cells[30, 1];
                range.Value = "有義波高H1/3(m)";
                range = wSheet.Cells[30, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Hs.ToString(); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!有義波高H1/3-->HS.
                range = wSheet.Cells[31, 1];
                range.Value = "tan\03B8=slope";
                range = wSheet.Cells[31, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_Slope.Text.ToString();
                int ii;
                if (textBox_HB.Text.ToString() == "")
                {
                    //顯示內部計算結果.
                    range = wSheet.Cells[32, 1];
                    range.Value = "Hb(m)";
                    range = wSheet.Cells[32, 2];
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = Mod.VarBank.hb.ToString();
                    ii = 32;
                }
                else
                {
                    ii = 31;
                }
                range = wSheet.Cells[ii + 1, 1];
                range.Value = "最大波高Hmax(m)";
                range = wSheet.Cells[ii + 1, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Hmax.ToString();

                range = wSheet.Cells[ii + 2, 1];
                range.Value = "3.波壓強度係數";
                wSheet.get_Range("A" + (ii + 2).ToString(), "B" + (ii + 2).ToString()).Merge(wSheet.get_Range("A" + (ii + 2).ToString(), "B" + (ii + 2).ToString()).MergeCells);
                range = wSheet.Cells[ii + 3, 1];
                range.Value = "\u03B11";
                range = wSheet.Cells[ii + 3, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.alpha1.ToString();
                range = wSheet.Cells[ii + 4, 1];
                range.Value = "\u03B12";
                range = wSheet.Cells[ii + 4, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.alpha2.ToString();
                range = wSheet.Cells[ii + 5, 1];
                range.Value = "\u03B13";
                range = wSheet.Cells[ii + 5, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.alpha3.ToString();
                range = wSheet.Cells[ii + 6, 1];
                range.Value = "4.液壓作用高度";
                wSheet.get_Range("A" + (ii + 6).ToString(), "B" + (ii + 6).ToString()).Merge(wSheet.get_Range("A" + (ii + 6).ToString(), "B" + (ii + 6).ToString()).MergeCells);
                range = wSheet.Cells[ii + 7, 1];
                range.Value = "\u03B7*(m)";
                range = wSheet.Cells[ii + 7, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.eta_Star.ToString();
                range = wSheet.Cells[ii + 8, 1];
                range.Value = "hc*(m)";
                range = wSheet.Cells[ii + 8, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.hc_Star.ToString();
                range = wSheet.Cells[ii + 9, 1];
                range.Value = "5.波壓力計算";
                wSheet.get_Range("A" + (ii + 9).ToString(), "B" + (ii + 9).ToString()).Merge(wSheet.get_Range("A" + (ii + 9).ToString(), "B" + (ii + 9).ToString()).MergeCells);
                range = wSheet.Cells[ii + 10, 1];
                range.Value = "P1(t/m^2)";
                range = wSheet.Cells[ii + 10, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.P1.ToString();
                range = wSheet.Cells[ii + 11, 1];
                range.Value = "P2(t/m^2)";
                range = wSheet.Cells[ii + 11, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.P2.ToString();
                range = wSheet.Cells[ii + 12, 1];
                range.Value = "P3(t/m^2)";
                range = wSheet.Cells[ii + 12, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.P3.ToString();
                range = wSheet.Cells[ii + 13, 1];
                range.Value = "P4(t/m^2)";
                range = wSheet.Cells[ii + 13, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.P4.ToString();
                range = wSheet.Range[wSheet.Cells[30, 1], wSheet.Cells[ii + 13, 2]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                range = wSheet.Cells[ii + 14, 1];
                range.Value = "分區";
                range = wSheet.Cells[ii + 14, 2];
                range.Value = "液壓強度P(t/m^2)";
                range = wSheet.Cells[ii + 14, 3];
                range.Value = "波壓FP(t/m^2)";
                range = wSheet.Cells[ii + 14, 4];
                range.Value = "力矩Y(m)";
                range = wSheet.Cells[ii + 14, 5];
                range.Value = "傾倒彎矩Mp(t-m/m)";


                //EL表格.
                int ix = 0;
                for (int iii = Mod.VarBank.EL_Out.GetUpperBound(0); iii >= 0; iii--)
                {
                    //一行一行輸出.
                    range = wSheet.Cells[ii + 15 + ix, 1];//分區
                    range.Value = "EL" + (Mod.VarBank.EL_Out[iii].EL < 0 ? "-" : "+") + Math.Abs(Mod.VarBank.EL_Out[iii].EL).ToString();
                    range = wSheet.Cells[ii + 15 + ix, 2];//波壓強度
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = Mod.VarBank.EL_Out[iii].P.ToString();
                    range = wSheet.Cells[ii + 15 + ix, 3];//波壓.
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = Mod.VarBank.EL_Out[iii].FP.ToString();
                    range = wSheet.Cells[ii + 15 + ix, 4];//力矩
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = Mod.VarBank.EL_Out[iii].Y.ToString();
                    range = wSheet.Cells[ii + 15 + ix, 5];//傾倒彎矩.
                    range.NumberFormatLocal = "0.00_ ";
                    range.Value = Mod.VarBank.EL_Out[iii].Mp.ToString();
                    //range = wSheet.Cells[ii + 15 + ix, 1];
                    //range.Value
                    ix += 1;
                }
                //填入合計.
                range = wSheet.Cells[ii + 15 + ix, 1];//分區
                range.Value = "合計";
                range = wSheet.Cells[ii + 15 + ix, 2];//波壓強度
                range.Value = "---";
                range = wSheet.Cells[ii + 15 + ix, 3];//波壓.
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Fp.ToString();
                range = wSheet.Cells[ii + 15 + ix, 4];//力矩
                range.Value = "---";
                range = wSheet.Cells[ii + 15 + ix, 5];//傾倒彎矩.
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Mp.ToString();
                range = wSheet.Range[wSheet.Cells[ii + 14, 1], wSheet.Cells[ii + 15 + ix, 5]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                ii = ii + 15 + ix;//合計列.

                range = wSheet.Cells[ii + 1, 1];//揚壓力.
                range.Value = "6.揚壓力計算";
                wSheet.get_Range("A" + (ii + 1).ToString(), "B" + (ii + 1).ToString()).Merge(wSheet.get_Range("A" + (ii + 1).ToString(), "B" + (ii + 1).ToString()).MergeCells);
                range = wSheet.Cells[ii + 2, 1];//揚壓力.
                range.Value = "Pu(t/m^2)";
                range = wSheet.Cells[ii + 2, 2];//揚壓力:PU.
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Pu.ToString();
                range = wSheet.Cells[ii + 3, 1];//揚壓力:Fu.
                range.Value = "Fu(t/m)";
                range = wSheet.Cells[ii + 3, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Fu.ToString();
                range = wSheet.Cells[ii + 4, 1];//揚壓力:Fu.
                range.Value = "Mu(t-m/m)";
                range = wSheet.Cells[ii + 4, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Mu.ToString();
                range = wSheet.Range[wSheet.Cells[ii + 1, 1], wSheet.Cells[ii + 4, 2]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                range = wSheet.Cells[ii + 5, 1];//堤體自重及抵抗彎矩.
                range.Value = "7.堤體自重及抵抗彎矩";
                wSheet.get_Range("A" + (ii + 5).ToString(), "B" + (ii + 5).ToString()).Merge(wSheet.get_Range("A" + (ii + 5).ToString(), "B" + (ii + 5).ToString()).MergeCells);
                range = wSheet.Cells[ii + 6, 1];//堤體自重及抵抗彎矩.
                range.Value = "分區";
                wSheet.get_Range("A" + (ii + 6).ToString(), "B" + (ii + 6).ToString()).Merge(wSheet.get_Range("A" + (ii + 6).ToString(), "B" + (ii + 6).ToString()).MergeCells);
                range = wSheet.Cells[ii + 6, 3];//
                range.Value = "A(m^2)";
                range = wSheet.Cells[ii + 6, 4];//
                range.Value = "\u03B3(t/m^3)";
                range = wSheet.Cells[ii + 6, 5];//
                range.Value = "W(t/m)";
                range = wSheet.Cells[ii + 6, 6];//
                range.Value = "X(m)";
                range = wSheet.Cells[ii + 6, 7];//
                range.Value = "Mw(t-m)";
                //寫出表格資訊.
                BlockResult[] BlockResultCol = Mod.VarBank.Block_Out;
                ix = 0;
                for (int iiii = Mod.VarBank.EL_Out.GetUpperBound(0) - 1; iiii >= 0; iiii--)
                {
                    int[] BlocKID = Mod.VarBank.EL_Out[iiii].BlockNum;
                    range = wSheet.Cells[ii + 7 + ix, 1];//分區: EL.
                    range.Value = "EL" + (Mod.VarBank.EL_Out[iiii].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[iiii].EL).ToString();
                    wSheet.get_Range("A" + (ii + 7 + ix).ToString(), "A" + (ii + 7 + ix + BlocKID.GetUpperBound(0)).ToString()).Merge(wSheet.get_Range("A" + (ii + 7 + ix).ToString(), "A" + (ii + 7 + ix + BlocKID.GetUpperBound(0)).ToString()).MergeCells);

                    for (int i3 = 0; i3 < BlocKID.GetLength(0); i3++)
                    {
                        int blockidget = BlocKID[i3];
                        //依序填入資訊.
                        range = wSheet.Cells[ii + 7 + ix, 2];//ID.
                        range.NumberFormatLocal = "@";
                        range.Value = "(" + (BlocKID[i3] + 1).ToString() + ")";
                        range = wSheet.Cells[ii + 7 + ix, 3];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = BlockResultCol[blockidget].A.ToString();//A.
                        range = wSheet.Cells[ii + 7 + ix, 4];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = BlockResultCol[blockidget].garma.ToString(); //Y;
                        range = wSheet.Cells[ii + 7 + ix, 5];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = BlockResultCol[blockidget].W.ToString(); //W;
                        range = wSheet.Cells[ii + 7 + ix, 6];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = BlockResultCol[blockidget].X.ToString(); //X;
                        range = wSheet.Cells[ii + 7 + ix, 7];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = BlockResultCol[blockidget].Mw.ToString(); //MW;

                        ix = ix + 1;
                    }
                }
                //填入合計資料.
                range = wSheet.Cells[ii + 7 + ix, 1];
                range.Value = "合計";
                wSheet.get_Range("A" + (ii + 7 + ix).ToString(), "B" + (ii + 7 + ix).ToString()).Merge(wSheet.get_Range("A" + (ii + 7 + ix).ToString(), "B" + (ii + 7 + ix).ToString()).MergeCells);
                range = wSheet.Cells[ii + 7 + ix, 3];
                range.Value = "---";
                range = wSheet.Cells[ii + 7 + ix, 4];
                range.Value = "---";
                range = wSheet.Cells[ii + 7 + ix, 5];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.W.ToString();//W.
                range = wSheet.Cells[ii + 7 + ix, 6];
                range.Value = "---";
                range = wSheet.Cells[ii + 7 + ix, 7];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.Mw.ToString();//Mw.
                range = wSheet.Range[wSheet.Cells[ii + 5, 1], wSheet.Cells[ii + 7 + ix, 7]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
                ii = ii + 7 + ix;

                range = wSheet.Cells[ii + 2, 1];
                range.Value = "四、堤體安定檢核";
                range.Font.Bold = true;
                wSheet.get_Range("A" + (ii + 2).ToString(), "B" + (ii + 2).ToString()).Merge(wSheet.get_Range("A" + (ii + 2).ToString(), "B" + (ii + 2).ToString()).MergeCells);
                range = wSheet.Cells[ii + 3, 1];
                range.Value = "使用者給定滑動SF";
                range = wSheet.Cells[ii + 3, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_SFSlide.Text.ToString();//使用者提供之安全係數
                wSheet.get_Range("B" + (ii + 3).ToString(), "C" + (ii + 3).ToString()).Merge(wSheet.get_Range("B" + (ii + 3).ToString(), "C" + (ii + 3).ToString()).MergeCells);
                range = wSheet.Cells[ii + 4, 1];
                range.Value = "計算出滑動SF";
                range = wSheet.Cells[ii + 4, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.CalBody_SlideSF.ToString();
                range = wSheet.Cells[ii + 4, 3];
                if (Mod.VarBank.CalBody_SlideSF >= double.Parse(textBox_SFSlide.Text.ToString()))
                {
                    //通過.
                    range.Value = "通過";
                    range.Interior.Color = ColorTranslator.ToOle(Color.Gray);
                    range.Font.Color = ColorTranslator.ToOle(Color.White);
                }
                else
                {
                    range.Value = "不通過";
                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                    range.Font.Color = ColorTranslator.ToOle(Color.White);
                }
                range = wSheet.Cells[ii + 5, 1];
                range.Value = "使用者給定傾倒SF";
                range = wSheet.Cells[ii + 5, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = textBox_SFOver.Text.ToString();//使用者提供之安全係數
                wSheet.get_Range("B" + (ii + 5).ToString(), "C" + (ii + 5).ToString()).Merge(wSheet.get_Range("B" + (ii + 5).ToString(), "C" + (ii + 5).ToString()).MergeCells);
                range = wSheet.Cells[ii + 6, 1];
                range.Value = "計算出傾倒SF";
                range = wSheet.Cells[ii + 6, 2];
                range.NumberFormatLocal = "0.00_ ";
                range.Value = Mod.VarBank.CalBody_RotateSF.ToString();
                range = wSheet.Cells[ii + 6, 3];
                if (Mod.VarBank.CalBody_RotateSF >= double.Parse(textBox_SFOver.Text.ToString()))
                {
                    //通過.
                    range.Value = "通過";
                    range.Interior.Color = ColorTranslator.ToOle(Color.Gray);
                    range.Font.Color = ColorTranslator.ToOle(Color.White);
                }
                else
                {
                    range.Value = "不通過";
                    range.Interior.Color = ColorTranslator.ToOle(Color.Red);
                    range.Font.Color = ColorTranslator.ToOle(Color.White);
                }
                range = wSheet.Range[wSheet.Cells[ii + 3, 1], wSheet.Cells[ii + 6, 3]];
                range.Borders.LineStyle = 1;
                range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);

                ii = ii + 6 + 3;
                int LargeItem = 5;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

                if (chk_BlockWeightCalc_HO.Checked || chk_BlockWeightCalc_HE.Checked || chk_BlockWeightCalc_BD.Checked)
                {
                    //消波工重量計算.
                    //1. 堤身段(港外側)
                    //2. 堤頭部加強
                    //3. 堤身段(航道側)
                    range = wSheet.Cells[ii, 1];
                    range.Value = mainForm.取得中文數字碼(LargeItem) + "、消波工重量計算";
                    range.Font.Bold = true;
                    wSheet.get_Range("A" + (ii).ToString(), "B" + (ii).ToString()).Merge(wSheet.get_Range("A" + (ii).ToString(), "B" + (ii).ToString()).MergeCells);

                    ix = 0;
                    int icc = 0;
                    if (chk_BlockWeightCalc_HO.Checked)
                    {
                        //堤身段(港外側).
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = (ix + 1).ToString() + ".堤身段(港外側)";
                        wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).Merge(wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).MergeCells);
                        icc += 1;

                        //基本參數填寫.
                        //1-1. 消波形塊安定係數.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波形塊安定係數";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HO_KDL.Text.ToString();
                        icc += 1;

                        //1-2. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊單位體積重量";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HO_Gamma.Text.ToString();
                        icc += 1;

                        //1-3. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊斜坡面與水平面之夾角";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HO_slopeangle.Text.ToString();
                        icc += 1;


                        //輸出.
                        //W1
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "W1(Ton)";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = Mod.VarBank.W1.ToString();
                        icc += 1;
                        ix += 1;
                    }

                    if (chk_BlockWeightCalc_HE.Checked)
                    {
                        //堤頭部加強.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = (ix + 1).ToString() + ".堤頭部加強";
                        wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).Merge(wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).MergeCells);
                        icc += 1;

                        //基本參數填寫.
                        //1-1. 消波形塊安定係數.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波形塊安定係數";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HE_KDL.Text.ToString();
                        icc += 1;

                        //1-2. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊單位體積重量";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HE_Gamma.Text.ToString();
                        icc += 1;

                        //1-3. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊斜坡面與水平面之夾角";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_HE_slopeangle.Text.ToString();
                        icc += 1;

                        //輸出.
                        //W2
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "W2(Ton)";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = Mod.VarBank.W2.ToString();//!!!!!!!!!!!!!!!!!!!!!!!!
                        icc += 1;
                        ix += 1;
                    }

                    if (chk_BlockWeightCalc_BD.Checked)
                    {
                        //堤身段(航道側).
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = (ix + 1).ToString() + ".堤身段(航道側)";
                        wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).Merge(wSheet.get_Range("A" + (ii + 1 + icc).ToString(), "B" + (ii + 1 + icc).ToString()).MergeCells);
                        icc += 1;

                        //基本參數填寫.
                        //1-1. 消波形塊安定係數.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波形塊安定係數";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_BD_KDL.Text.ToString();
                        icc += 1;

                        //1-2. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊單位體積重量";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_BD_Gamma.Text.ToString();
                        icc += 1;

                        //1-3. 消波塊單位體積重量.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "消波塊斜坡面與水平面之夾角";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_BD_slopeangle.Text.ToString();
                        icc += 1;

                        //1-4. 波高傳遞率.
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "波高傳遞率";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = textBox_BD_Kt.Text.ToString();
                        icc += 1;

                        //輸出.
                        //W3(Ton)
                        range = wSheet.Cells[ii + 1 + icc, 1];
                        range.Value = "W3(Ton)";
                        range = wSheet.Cells[ii + 1 + icc, 2];
                        range.NumberFormatLocal = "0.00_ ";
                        range.Value = Mod.VarBank.W3.ToString(); //!!!!!!!!!!!!!!!!!!!!!!!!
                        icc += 1;
                        ix += 1;
                    }

                    range = wSheet.Range[wSheet.Cells[ii + 1, 1], wSheet.Cells[ii + 1 + icc, 2]];
                    range.Borders.LineStyle = 1;
                    range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                    range.Borders.Weight = Excel.XlBorderWeight.xlThin;
                    range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick, Excel.XlColorIndex.xlColorIndexAutomatic);
                    LargeItem += 1;
                    ii = ii + 1 + icc + 2;//變更位置.
                }


                range = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[60000, 10]]; //.Select();//wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[ii + 15 + ix, 10]];
                range.Columns.AutoFit();
                range.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                range.Font.Name = "微軟正黑體";

                //===========================================================================================
                wBook.Worksheets.Add(After: wBook.Sheets[wBook.Sheets.Count]);
                wSheet = (Excel._Worksheet)wBook.Worksheets[wBook.Sheets.Count];//第一個工作表.
                wSheet.Name = "報表資訊";
                wSheet.Activate();
                range = wSheet.Cells[1, 1];
                range.Value = "報表產生時間";
                range = wSheet.Cells[1, 2];
                range.Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm");
                range = wSheet.Cells[2, 1];
                range.Value = "填表人員工編號";
                range = wSheet.Cells[2, 2];
                range.Value = mainForm.LoginInUserID;
                range = wSheet.Cells[3, 1];
                range.Value = "填表人名稱";
                range = wSheet.Cells[3, 2];
                range.Value = mainForm.LoginInUserName;
                range = wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[60000, 10]]; //.Select();//wSheet.Range[wSheet.Cells[1, 1], wSheet.Cells[ii + 15 + ix, 10]];
                range.Columns.AutoFit();
                range.Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                range.Font.Name = "微軟正黑體";

                //wSheet.Cells[3, 1].Value = Mod.VarBank.Block_Out[2].A ;
                //wSheet.Cells[3, 2].Value = Mod.VarBank.Block_Out[2].garma;
                //wSheet.Cells[3, 3].Value = Mod.VarBank.Block_Out[2].W;
                //wSheet.Cells[3, 4].Value = Mod.VarBank.Block_Out[2].X;
                //wSheet.Cells[3, 5].Value = Mod.VarBank.Block_Out[2].Mw;
                //range = wSheet.Cells[3, 1];
                //range.Value = Mod.VarBank.Block_Out[1]
                //range = wSheet.Cells[1, 1];
                //range.Value = "名稱";
                //range.Borders.Weight = Excel.XlBorderWeight.xlMedium;
                //range.Interior.Color = ColorTranslator.ToOle(Color.Gray) ;
                //range.Font.Color = ColorTranslator.ToOle(Color.White);
                //range.Font.Bold = true;

                //wSheet.Cells[2, 1] = "10";
                //wSheet.Cells[3, 1] = "20";
                //wSheet.Cells[4, 1].Formula = "=SUM(A2:A3)";//string.Format("A{0}:A{1}",2,4);


                //range = wSheet.Cells[1, 1];
                //range.Columns.AutoFit();
                //range = wSheet.Range[wSheet.Cells[2, 1], wSheet.Cells[3, 2]];
                //range.Borders.LineStyle = 1;
                //range.Borders.Color = ColorTranslator.ToOle(Color.Black);
                //range.Borders.Weight = Excel.XlBorderWeight.xlThick;

                //range = wSheet.Range[wSheet.Cells[2, 3], wSheet.Cells[3, 4]];
                //range.BorderAround(Type.Missing, Excel.XlBorderWeight.xlThick,Excel.XlColorIndex.xlColorIndexAutomatic);
                //range.AutoFormat(Excel.XlRangeAutoFormat.xlRangeAutoFormat3DEffects1,true, false, true, false, true, true);

                //range = wSheet.Cells[8, 1];
                //range.Value = "\u03B4=20.0";
                ////excelRange.BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlThick,
                ////XlColorIndex.xlColorIndexAutomatic, System.Drawing.Color.Black.ToArgb());
                ////excelRange.Merge(excelRange.MergeCells);
                ////_workSheet.get_Range("A15", "B15").Merge(_workSheet.get_Range("A15", "B15").MergeCells);
                wBook.SaveAs(getpath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Excel.XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            }
            catch (Exception ex)
            {
                //bkOutputExcelFile.ReportProgress(0, "出現意外:" + ex.Message.ToString());
                getMsg = "輸出Excel檔案時發生意外的狀況而失敗" + Environment.NewLine + ex.Message.ToString();
                //MessageBox.Show("輸出Excel檔案時發生意外的狀況而失敗" + Environment.NewLine + ex.Message.ToString(), "輸出失敗", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            wBook.Close(false, Type.Missing, Type.Missing);
            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            wBook = null;
            wSheet = null;
            range = null;
            excelApp = null;
            GC.Collect();
            return getMsg;
        }
        private void bkOutputExcelFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            //暫時廢棄不用.
            tsp_progressbar.Visible = false;
            tsp_cond.Text = "您已輸出完成Excel檔案,謝謝使用";
            MessageBox.Show("輸出完成!!","輸出Excel報表完成",MessageBoxButtons.OK,MessageBoxIcon.Information);
            
            if(chk_OpenFileAfterOutput.Checked)
            { 
                try
                {
                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    p.StartInfo.FileName = SFD_EXCELReport.FileName;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    p.Start();
                }
                catch
                {
                    //不做任何事情.
                }

            }
        }
        private void btn_OutputExcel_Click(object sender, EventArgs e)
        {


            輸出Word檔案ToolStripMenuItem_Click(sender, e);
            return;


            //*****************************************************************************************
            //以下為原先Excel輸出部分,已被避開.
            if(SFD_EXCELReport.ShowDialog()==DialogResult.OK && SFD_EXCELReport.FileName!="")
            {
                //輸出.
                string getpath = SFD_EXCELReport.FileName;
                if(File.Exists(getpath))
                {
                    //規定目前此excel檔案不可被開啟中.
                    if (IsFileLocked(new FileInfo(getpath)))
                    {
                        MessageBox.Show("您所預儲存的檔案已經存在且已被鎖定!!!" + Environment.NewLine + "處理中止，此檔案可能被其他檔案編輯中或是目前正被Excel打開中","輸出錯誤",MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        return;
                    }

                }
                //執行輸出.
                tsp_progressbar.Style = ProgressBarStyle.Marquee;
                tsp_progressbar.Visible = true;
                tsp_progressbar.MarqueeAnimationSpeed = 10;//0.1 sec.
                tsp_cond.Text = "輸出Excel檔案中...";

                tsp_progressbar.Visible = false;
                string getMsg = 寫出檢核結果Excel表(getpath);

                if(getMsg!="OK")
                {
                    MessageBox.Show("您的Excel表單輸出出現錯誤!" + Environment.NewLine + getMsg, "EXCEL輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tsp_cond.Text = "您沒有成功輸出檢核報表";


                }
                else
                {
                    tsp_cond.Text = "您已輸出完成Excel檔案,謝謝使用";
                    MessageBox.Show("輸出完成!!", "輸出Excel報表完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (chk_OpenFileAfterOutput.Checked)
                    {
                        try
                        {
                            Process p = new Process();
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                            p.StartInfo.FileName = SFD_EXCELReport.FileName;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                            p.Start();
                        }
                        catch
                        {
                            //不做任何事情.
                        }

                    }
                }//有無成功輸出.
                //bkOutputExcelFile.RunWorkerAsync(getpath);
                
            }
        }
        public bool IsFileLocked(FileInfo file)
        {
            if(!file.Exists)
            {
                return false;
            }
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch(IOException)
            {
                //The file is unvaliable:
                //1. Opened by other programs.
                //2. still being written.
                //3. doesn't exist.
                return true;
            }
            finally
            {
                if(stream!=null)
                {
                    stream.Close();
                }
            }
            return false;//File is not locked.
        }
        #endregion

        private void textBox_SeaGamma_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn_LogOutput_Click(object sender, EventArgs e)
        {
            if(isExporting)
            { return; }
            if(object.Equals(Mod,null))
            {
                //MessageBox.Show("你的計算主體'MOD'為Null!!!!!");
                MessageBox.Show("您目前沒有完成的檢核結果!請重新檢核,若您無法使用檢核功能,請確認您的軟體已授權,或是聯絡開發商", "Log檔案輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                MessageBox.Show("輸出完成!","輸出Log File完成",MessageBoxButtons.OK,MessageBoxIcon.Information);
                if(chk_OpenFileAfterOutput.Checked)
                 {
                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    p.StartInfo.FileName = SFD_LogFile.FileName;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    p.Start();
                }

            }
        }
        private void 輸出Log檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_LogOutput_Click(sender, e);
        }
        private void textBox_CheckMessageShow_KeyPress(object sender, KeyPressEventArgs e)
        {

            e.Handled = true;

        }
        bool hh = false;
        private void textBox_CheckMessageShow_KeyDown(object sender, KeyEventArgs e)
        {
            //檢核頁面顯示計算訊息的textbox,限制只能按下Ctrl+C[複製],Ctrl+A[全選]. 
            //
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode==Keys.A)
            {
                //MessageBox.Show("Yes");
                ((TextBox)sender).SelectAll();
                e.Handled = true;
                hh = true;
            }
            else if(e.Control && e.KeyCode==Keys.C)
            {
                e.Handled = true;
                hh = true;
            }
            else
            {
                //MessageBox.Show("Handle event");
                e.Handled = true;
                hh = true;
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
        bool EscapeELDGV1CellChange = false;
        private void ELDGV1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show("H!");
            if(EscapeELDGV1CellChange)
            {
                EscapeELDGV1CellChange = false;
                return;
            }
            繪上EL();
        }
        private void ELDGV1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //若目前沒有Block,禁止新增.
            if(BlockMainArray.GetLength(0)==0)
            {
                //刪除.
                DGMaterial.Rows.RemoveAt(e.Row.Index);// addedrow)
                EscapeELDGV1CellChange = true;
                return;
            }
        }

        private void ELDGV1_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (EscapeELDGV1CellChange)
            {
                EscapeELDGV1CellChange = false;
                return;
            }
            繪上EL();
        }
        #endregion
        #region 摩擦係數與材質設定區塊
        private void DGMaterial_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //這個方程式不用.

        }
        bool newrowadd = false;
        private void DGMaterial_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {

            //新增一個新的材質完成.
            //新增一個材質時,不允許新增之後Cell空白.
            newrowadd = true;
            EscapeDGMaterialCellValueChangedFunction = false;

        }
        bool EscapeDGMaterialCellValueChangedFunction = false;
        private void DGMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            //MessageBox.Show("OO" + EscapeDGMaterialCellValueChangedFunction.ToString());
            if(!使用者手動更新材質與摩擦)
            {
                return;
            }

            try
            {
                if (DGMaterial.Rows[e.RowIndex].Cells[1].Value == null)
                {
                    //此Row被變更為空白,但可能是在載入新的專案檔.
                    if (MaterialSubscriptToName.ContainsKey(e.RowIndex))
                    {
                        if (MessageBox.Show("您確定刪除此材質嗎?\n刪除後，與此材質相關的摩擦係數設定都會被移除!", "刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                        {
                            EscapeDGMaterialCellValueChangedFunction = true;
                            DGMaterial.Rows[e.RowIndex].Cells[1].Value = MaterialArray[e.RowIndex];
                            return;
                        }
                        else
                        {
                            DGMaterial.Rows.RemoveAt(e.RowIndex);
                            return;
                        }
                    }
                    else// if(EscapeDGMaterialCellValueChangedFunction)
                    {
                        DGMaterial.Rows.RemoveAt(e.RowIndex);
                        return;
                    }
                }
            }
            catch
            {

            }
            if (EscapeDGMaterialCellValueChangedFunction)
            {
                EscapeDGMaterialCellValueChangedFunction = false;
                return;
            }
            if (DGMaterial.Rows.Count == 1)
            {
                return;
            }

            if(newrowadd)
            {
                newrowadd = false;
                EscapeDGMaterialCellValueChangedFunction = true;
                //不允許材質名稱重覆.
                int addedrow = e.RowIndex;
                //MessageBox.Show(e.Row.Cells[1].Value.ToString());
                //MessageBox.Show(DGMaterial.Rows[0].Cells[1].Value.ToString());
                if(DGMaterial.Rows.Count==1 || addedrow>=DGMaterial.Rows.Count)
                {
                    //This is an error need to be escaped.
                    return;
                }
                if (DGMaterial.Rows[addedrow].Cells[1].Value == null)
                {
                    return;
                }
                string newname = DGMaterial.Rows[addedrow].Cells[1].Value.ToString(); //' DGMaterial.Rows[DGMaterial.Rows.Count - 1].Cells[1].Value.ToString();
                //MessageBox.Show("P0");
                if (MaterialNameToArraySubScript.ContainsKey(newname))
                {
                    MessageBox.Show("您所輸入的材質名稱'" + newname + "'重覆!!!不允許重覆的材質名稱!!", "材質與摩擦係數設定", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //Remove this line.
                    EscapeDGMaterialCellValueChangedFunction = true;
                    DGMaterial.Rows.RemoveAt(addedrow);//刪除.
                    return;
                }

                //MessageBox.Show("P1-0-1");
                for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
                {
                    DGMaterial.Rows[i].Cells[0].Value = (i + 1).ToString();
                }

                //變更矩陣.
                //MessageBox.Show("P1-0");
                MaterialNameToArraySubScript.Clear();
                MaterialSubscriptToName.Clear();
                Array.Resize(ref MaterialArray, 0);
                MaterialCount = 0;
                labelT.Text = "";
                for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
                {
                    Array.Resize(ref MaterialArray, MaterialCount + 1);
                    MaterialArray[i] = DGMaterial.Rows[i].Cells[1].Value.ToString();
                    labelT.Text += (MaterialArray[i].ToString());
                    MaterialCount += 1;
                    MaterialNameToArraySubScript.Add(DGMaterial.Rows[i].Cells[1].Value.ToString(), i);
                    MaterialSubscriptToName.Add(i, DGMaterial.Rows[i].Cells[1].Value.ToString());
                }
                //MessageBox.Show("P1-1");
                //新增摩擦參數.
               
                DataGridViewRowCollection rows = DGMaterialRough.Rows;
                int osize = rows.Count;
                for (int i = 0; i < MaterialArray.GetLength(0); i++)
                {
                    //if (i != addedrow)
                    //{
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    rows.Add(new object[] { (rows.Count + 1).ToString(), newname, MaterialSubscriptToName[i], "" });
                    //}
                }

                //設定Combobox內容.
                foreach(DataGridViewRow row in DGMaterialRough.Rows)
                {
                    var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                    cell.DataSource = MaterialArray;
                    var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                    cell2.DataSource = MaterialArray;
                }


                //MessageBox.Show("P1-2");
                if (DGMaterialRough.Rows.Count > 0) //只有一個材質時，沒有摩擦係數設定.
                { DGMaterialRough.CurrentCell = DGMaterialRough.Rows[osize>0?osize-1:0].Cells[3]; }//指定Current Cell.
                Array.Resize(ref MaterialsCoefArray, 0);
                MaterialRoughnessArrayCount = 0;
                for (int i = 0; i < DGMaterialRough.Rows.Count; i++)
                {
                    Array.Resize(ref MaterialsCoefArray, MaterialRoughnessArrayCount + 1);
                    try
                    { MaterialsCoefArray[MaterialRoughnessArrayCount].Id1 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[1].Value.ToString()]; }
                    catch { MaterialsCoefArray[MaterialRoughnessArrayCount].Id1 = -9999; }
                    try
                    {
                        MaterialsCoefArray[MaterialRoughnessArrayCount].Id2 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[2].Value.ToString()];
                    }
                    catch { MaterialsCoefArray[MaterialRoughnessArrayCount].Id2 = -9999; }
                    if(!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[MaterialRoughnessArrayCount].coef))
                    {
                        MaterialsCoefArray[MaterialRoughnessArrayCount].coef = -9999;
                    }
                    MaterialRoughnessArrayCount += 1;
                }
                //以下區段必須要有Block時才有效.
                if(BlockMainArray.GetLength(0)==0)
                {
                    return;
                }
                Class_Block_Interface D = new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;
                //將參考材質填入資訊:前提都是必須要有可用材質.
                ReferencedMaterialCHKL.Items.Clear();
                for (int i = 0; i < MaterialArray.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.Items.Add(MaterialArray[i]);
                }
                //根據此Block是否有選取設定資訊.
                string[] BlockSelectedReferncedMaterialNames = BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質;
                string[] AvailableMaterials = new string[] { };
                for (int i = 0; i < BlockSelectedReferncedMaterialNames.GetLength(0); i++)
                {
                    if (MaterialNameToArraySubScript.ContainsKey(BlockSelectedReferncedMaterialNames[i]))
                    {
                        Array.Resize(ref AvailableMaterials, AvailableMaterials.GetLength(0) + 1);
                        AvailableMaterials[AvailableMaterials.GetUpperBound(0)] = BlockSelectedReferncedMaterialNames[i];
                    }
                }
                BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質 = AvailableMaterials;//根據目前最新的可用材質清單 傳回可用的選取..
                for (int i = 0; i < AvailableMaterials.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.SetItemCheckState(MaterialNameToArraySubScript[AvailableMaterials[i]], CheckState.Checked);
                }
                //MessageBox.Show("P1-3");
                //完成新增.

                return;
            }


            //修改材質名稱時.
            //MessageBox.Show("H2");
            if(MaterialArray.GetLength(0)==0)
            {
                //MessageBox.Show("Size =0 ");
                return;
            }
            //變更材質名稱時,同步將右側摩擦係數DG與矩陣變更.
            int changerow = e.RowIndex;
            //MessageBox.Show(changerow.ToString());
            if(DGMaterial.Rows[changerow].Cells[1].Value==null)
            {
                //被更改為空白,視為刪除之.
                if (MessageBox.Show("您確定刪除此材質嗎?\n刪除後，與此材質相關的摩擦係數設定都會被移除!", "刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    EscapeDGMaterialCellValueChangedFunction = true;
                    DGMaterial.Rows[changerow].Cells[1].Value = MaterialArray[changerow];
                    return;
                }
                else
                {
                    //刪除此Row.
                    DGMaterial.Rows.RemoveAt(changerow);
                    return;
                }
                

            }
            else if(DGMaterial.Rows[changerow].Cells[1].Value.ToString()=="")
            {
                //被更改為空白,視為刪除之.
                if (MessageBox.Show("您確定刪除此材質嗎?\n刪除後，與此材質相關的摩擦係數設定都會被移除!", "刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                {
                    EscapeDGMaterialCellValueChangedFunction = true;
                    DGMaterial.Rows[changerow].Cells[1].Value = MaterialArray[changerow];
                    return;
                }
                else
                {
                    //刪除此Row.
                    DGMaterial.Rows.RemoveAt(changerow);
                    return;
                }

            }
            string changeName = DGMaterial.Rows[changerow].Cells[1].Value.ToString();
            //不允許重覆.
            bool repeated = false;
            for(int i=0;i<DGMaterial.Rows.Count-1;i++)
            {
                if(i!=changerow)
                {
                    if(DGMaterial.Rows[i].Cells[1].Value.ToString()==changeName)
                    {
                        repeated = true;
                        break;
                    }
                }
            }
            if(repeated)
            {
                MessageBox.Show("不允許將名稱更改為重覆的材質名稱!!失敗","材質名稱", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                DGMaterial.Rows[changerow].Cells[1].Value = MaterialArray[changerow];
                EscapeDGMaterialCellValueChangedFunction = true;//避免再一次進入這個Function.
                return;
            }

            //可以更改.
            //MessageBox.Show("H2-2");
            string oldname = MaterialArray[changerow];
            //MessageBox.Show(oldname + ":" + changeName);
            Array.Resize(ref MaterialArray, 0);
            MaterialCount = 0;
            //MessageBox.Show(DGMaterial.Rows.Count.ToString());
            for(int i=0;i<DGMaterial.Rows.Count-1;i++)
            {
                //MessageBox.Show(i.ToString());
                Array.Resize(ref MaterialArray, MaterialCount+1);
                MaterialArray[MaterialCount] = DGMaterial.Rows[i].Cells[1].Value.ToString();
                MaterialCount += 1;
            }
            //MessageBox.Show(MaterialArray.GetLength(0).ToString());
            MaterialNameToArraySubScript.Remove(oldname);//更改矩陣值.
            MaterialNameToArraySubScript.Add(changeName, changerow);
            MaterialSubscriptToName[changerow] =changeName;//更改Key值.

            //更改對應的摩擦係數.
            Array.Resize(ref MaterialsCoefArray, 0);
            MaterialRoughnessArrayCount = 0;
            //MessageBox.Show("H2-3");
            for(int i=0;i<DGMaterialRough.Rows.Count;i++)
            {
                if(DGMaterialRough.Rows[i].Cells[1].Value.ToString()==oldname)
                {
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    try { DGMaterialRough.Rows[i].Cells[1].Value = changeName; }
                    catch { }
                }
                if (DGMaterialRough.Rows[i].Cells[2].Value.ToString() == oldname)
                {
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    try { DGMaterialRough.Rows[i].Cells[2].Value = changeName; }//跳過因為值改變而下拉式選單尚未更新的錯誤.
                    catch { }
                }
                Array.Resize(ref MaterialsCoefArray, MaterialRoughnessArrayCount + 1);
                try
                { MaterialsCoefArray[MaterialRoughnessArrayCount].Id1 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[1].Value.ToString()]; }
                catch { MaterialsCoefArray[MaterialRoughnessArrayCount].Id1 = -9999; }
                try
                {
                    MaterialsCoefArray[MaterialRoughnessArrayCount].Id2 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[2].Value.ToString()];
                }
                catch { MaterialsCoefArray[MaterialRoughnessArrayCount].Id2 = -9999; }
                if(!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[MaterialRoughnessArrayCount].coef))
                {
                    MaterialsCoefArray[MaterialRoughnessArrayCount].coef = -9999;
                }
                MaterialRoughnessArrayCount += 1;
            }

            //設定Combobox內容.
            //MessageBox.Show(MaterialArray.ToString());
            foreach (DataGridViewRow row in DGMaterialRough.Rows)
            {
                var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                cell.DataSource = MaterialArray;//下拉式選單取代為新的
                var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                cell2.DataSource = MaterialArray;
            }
            //MessageBox.Show("H2-4");
            //更改Block內的使用材質
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                if (BlockMainArray[i].使用材質 == oldname)
                {
                    BlockMainArray[i].使用材質 = changeName;
                }
            }
            //更改Block內參考材質的名稱.
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                string[] ReferencedMaterialsOfBlock = BlockMainArray[i].周圍參考材質;
                for(int j=0;j<ReferencedMaterialsOfBlock.GetLength(0);j++)
                {
                    if(ReferencedMaterialsOfBlock[j]==oldname)
                    {
                        ReferencedMaterialsOfBlock[j] = changeName;
                    }
                }
            }

            //MessageBox.Show("H2-5");
            if (listBox_SectSetting.SelectedIndex != -1 && listBox_SectSetting.Items.Count>0)
            {
                //重新載入Property Grid與參考材質.
                Class_Block_Interface D= new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if(!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;
                //將參考材質填入資訊:前提都是必須要有可用材質.
                ReferencedMaterialCHKL.Items.Clear();
                for (int i = 0; i < MaterialArray.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.Items.Add(MaterialArray[i]);
                }
                //根據此Block是否有選取設定資訊.
                string[] BlockSelectedReferncedMaterialNames = BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質;
                string[] AvailableMaterials = new string[] { };
                for (int i = 0; i < BlockSelectedReferncedMaterialNames.GetLength(0); i++)
                {
                    if (MaterialNameToArraySubScript.ContainsKey(BlockSelectedReferncedMaterialNames[i]))
                    {
                        Array.Resize(ref AvailableMaterials, AvailableMaterials.GetLength(0) + 1);
                        AvailableMaterials[AvailableMaterials.GetUpperBound(0)] = BlockSelectedReferncedMaterialNames[i];
                    }
                }
                BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質 = AvailableMaterials;//根據目前最新的可用材質清單 傳回可用的選取..
                for (int i = 0; i < AvailableMaterials.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.SetItemCheckState(MaterialNameToArraySubScript[AvailableMaterials[i]], CheckState.Checked);
                }
            }

            EscapeDGMaterialCellValueChangedFunction = false;


        }
        int DGMaterialDeleteRow =-1;
        private void DGMaterial_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if(!使用者手動更新材質與摩擦)
            {
                return;
            }
            //MessageBox.Show("R : " + e.RowIndex.ToString());
            DGMaterialDeleteRow = e.RowIndex;
            EscapeDGMaterialCellValueChangedFunction = true;
            刪除Material的材質();

        }
        private void DGMaterial_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //準備刪除此材質,若確定刪除的話,與此材質相關的摩擦係數都會被移除.
            if (MessageBox.Show("您確定刪除此材質嗎?\n刪除後，與此材質相關的摩擦係數設定都會被移除!", "刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                e.Cancel = true;
               
                return;
            }
            else
            {
                foreach(DataGridViewRow row in DGMaterial.SelectedRows) { DGMaterialDeleteRow = row.Index; break; }
                EscapeDGMaterialCellValueChangedFunction = true;
            }

        }
        private void DGMaterial_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //根據刪除的Row，將相關的摩擦係數設定刪除.
            //此時材質矩陣尚未變更.
            //System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            //messageBoxCS.AppendFormat("{0} = {1}", "Row", e.Row);
            //messageBoxCS.AppendLine();
            //MessageBox.Show(messageBoxCS.ToString(), "UserDeletedRow Event");
            刪除Material的材質(false);
        }
        private void 刪除Material的材質(bool Removed = true)
        {

            if (DGMaterialDeleteRow == -1)
            { return; }
            if(!MaterialSubscriptToName.ContainsKey(DGMaterialDeleteRow))
            {
                //MessageBox.Show("P-1");
                return;
            }
            int deleterow = DGMaterialDeleteRow;

            //MessageBox.Show(e.Row.Index.ToString());
            string deleteMaterialName = MaterialSubscriptToName[deleterow];//取得名稱.
            //MessageBox.Show("D1");
           // MessageBox.Show(DGMaterial.Rows.Count.ToString());
            //1. 變更材質DG的序號.
            for (int i = 0; i < DGMaterial.Rows.Count - 1; i++) //此可直接Edit，所以省略一個
            {
                //if(DGMaterial.Rows[i].Cells[1].Value.ToString()!="")
                //{
                EscapeDGMaterialCellValueChangedFunction = true;
                DGMaterial.Rows[i].Cells[0].Value = (i + 1).ToString();
                //}
            }
            //MessageBox.Show("D2");
            //變更材質矩陣與相關Dict.
            MaterialSubscriptToName.Clear();
            MaterialNameToArraySubScript.Clear();
            Array.Resize(ref MaterialArray, 0);
            MaterialCount = 0;
            //MessageBox.Show("Count: " + DGMaterial.Rows.Count.ToString());
            for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
            {
                Array.Resize(ref MaterialArray, i + 1);
                MaterialArray[i] = DGMaterial.Rows[i].Cells[1].Value.ToString();
                MaterialSubscriptToName.Add(i, DGMaterial.Rows[i].Cells[1].Value.ToString());
                MaterialNameToArraySubScript.Add(DGMaterial.Rows[i].Cells[1].Value.ToString(), i);
            }
            MaterialCount = MaterialArray.GetLength(0);
            //MessageBox.Show("P1");
            //變更摩擦係數設定DG.

            for (int i = DGMaterialRough.Rows.Count - 1; i >= 0; i--)
            {
                if (DGMaterialRough.Rows[i].Cells[1].Value.ToString() == deleteMaterialName || DGMaterialRough.Rows[i].Cells[2].Value.ToString() == deleteMaterialName)
                {
                    //刪除此列.
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    DGMaterialRough.Rows.RemoveAt(i);
                }
            }

            //變更摩擦係數設定矩陣(重裝).
            Array.Resize(ref MaterialsCoefArray, 0);
            MaterialRoughnessArrayCount = 0;
            //MessageBox.Show("D1-2");
            //string ms = "";
            //foreach (KeyValuePair<string, int> entry in MaterialNameToArraySubScript)
            //{
            //    ms += (entry.Key + ":" + entry.Value);
            //    // do something with entry.Value or entry.Key
            //}
            //MessageBox.Show(ms);
            //labelT.Text = "";
            for (int i = 0; i < DGMaterialRough.Rows.Count; i++)
            {
                DGMaterialRough.Rows[i].Cells[0].Value = (i + 1).ToString();//序號更新.
                Array.Resize(ref MaterialsCoefArray, i + 1);

                try
                { MaterialsCoefArray[i].Id1 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[1].Value.ToString()]; }
                catch { MaterialsCoefArray[i].Id1 = -9999; }
                try
                {
                    MaterialsCoefArray[i].Id2 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[2].Value.ToString()];
                }
                catch { MaterialsCoefArray[i].Id2 = -9999; }
                if (!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[i].coef))
                {
                    MaterialsCoefArray[i].coef = -9999;
                }
                MaterialRoughnessArrayCount += 1;
            }

            //設定Combobox內容.
            //MessageBox.Show(MaterialArray.ToString());
            foreach (DataGridViewRow row in DGMaterialRough.Rows)
            {
                var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                cell.DataSource = MaterialArray;//更新下拉式選單.
                var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                cell2.DataSource = MaterialArray;
            }
            //MessageBox.Show("D1-3");
            //完成移除.

            //檢視所有Block,變更使用此材質之設定為空白.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                if (BlockMainArray[i].使用材質 == deleteMaterialName)
                {
                    BlockMainArray[i].使用材質 = "";
                }
            }
            //檢視所有Block，刪除使用此材質之Block參考材質.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                string[] OldName = BlockMainArray[i].周圍參考材質;
                string[] NewName = new string[] { };
                for (int j = 0; j < OldName.GetLength(0); j++)
                {
                    if (OldName[j] == deleteMaterialName)
                    {
                        //跳過.
                    }
                    else
                    {
                        Array.Resize(ref NewName, NewName.GetLength(0) + 1);
                        NewName[NewName.GetUpperBound(0)] = OldName[j];
                    }
                }
                BlockMainArray[i].周圍參考材質 = NewName;//更新.
            }
            if (listBox_SectSetting.SelectedIndex != -1 && listBox_SectSetting.Items.Count > 0)
            {
                //重新載入Property Grid.
                Class_Block_Interface D = new Class_Block_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;

                //將參考材質填入資訊:前提都是必須要有可用材質.
                //這個功能只限定選取中的呈現.
                ReferencedMaterialCHKL.Items.Clear();
                for (int i = 0; i < MaterialArray.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.Items.Add(MaterialArray[i]);
                }
                //根據此Block是否有選取設定資訊.
                string[] BlockSelectedReferncedMaterialNames = BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質;
                string[] AvailableMaterials = new string[] { };
                for (int i = 0; i < BlockSelectedReferncedMaterialNames.GetLength(0); i++)
                {
                    if (MaterialNameToArraySubScript.ContainsKey(BlockSelectedReferncedMaterialNames[i]))
                    {
                        Array.Resize(ref AvailableMaterials, AvailableMaterials.GetLength(0) + 1);
                        AvailableMaterials[AvailableMaterials.GetUpperBound(0)] = BlockSelectedReferncedMaterialNames[i];
                    }
                }
                BlockMainArray[listBox_SectSetting.SelectedIndex].周圍參考材質 = AvailableMaterials;//根據目前最新的可用材質清單 傳回可用的選取..
                for (int i = 0; i < AvailableMaterials.GetLength(0); i++)
                {
                    ReferencedMaterialCHKL.SetItemCheckState(MaterialNameToArraySubScript[AvailableMaterials[i]], CheckState.Checked);
                }
            }
            //避免又進入DGMaterial CellContentChanged Function內.

            MessageBox.Show("將材質'" + deleteMaterialName + "'與相關的摩擦係數設定刪除完畢!!", "材質管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        DataGridViewSelectedRowCollection DGMaterialRoughUserDeleteRows=null;
        bool Escape材質間摩擦係數刪除事件 = true;
        private void btn_RemoveRowMR_Click(object sender, EventArgs e)
        {
            //使用者手動刪除摩擦係數表時,
            //DG Roughness設定內移除列.
            if (MessageBox.Show("您確定要刪除這些資料嗎?", "刪除摩擦係數", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                //準備清除Row.

                //DGMaterialRoughUserDeleteRows = DGMaterialRough.SelectedCells;//.SelectedRows;
                DataGridViewSelectedCellCollection DGMRCell = DGMaterialRough.SelectedCells;
                if(DGMRCell.Count==0)
                { return;
                }
                Dictionary<int, bool> DeleteRow = new Dictionary<int, bool>();
                //DGMaterialRough.Rows.RemoveAt(DGMaterialRoughUserDeleteRows);
                for(int i=0;i<DGMRCell.Count;i++)
                {
                    if(!DeleteRow.ContainsKey(DGMRCell[i].RowIndex))
                    {
                        DeleteRow.Add(DGMRCell[i].RowIndex, true);
                    }
                }

                //for (int i=DGMaterialRoughUserDeleteRows.Count-1; i>=0;i--) // i++)
                //{
                int[] DD = new int[] { };
                foreach(int i in DeleteRow.Keys)
                {
                    Array.Resize(ref DD, DD.GetLength(0) + 1);
                    DD[DD.GetUpperBound(0)] = i;
                }
                //MessageBox.Show("SIZE:" + DD.GetLength(0).ToString());
                Array.Sort(DD);// , 0, DD.GetLength(0));
                for(int i=DD.GetLength(0)-1;i>=0;i--)
                { 
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    //MessageBox.Show(DD[i].ToString());
                    Escape材質間摩擦係數刪除事件 = i==0?false:true;
                    DGMaterialRough.Rows.RemoveAt(DD[i]);
                }
            }
        }
        private void DGMaterialRough_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //DG Roughness設定內移除列.
            if(MessageBox.Show("您確定要刪除這些資料嗎?","刪除摩擦係數",MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.Cancel)
            {
                e.Cancel = true;
            }
            else
            {
                //準備清除Row.
                DGMaterialRoughUserDeleteRows = DGMaterialRough.SelectedRows;
            }
        }
        private void DGMaterialRough_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //刪除Row完成之事件.
            if(!使用者手動更新材質與摩擦)
            {
                return;
            }
            if(Escape材質間摩擦係數刪除事件)
            {
                Escape材質間摩擦係數刪除事件 = false;
                return;
            }
            刪除材質間摩擦係數();
        }
        private void DGMaterialRough_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            //完成刪除工作.
            刪除材質間摩擦係數();
            //MessageBox.Show("刪除完成");
        }
        private void 刪除材質間摩擦係數()
        {
            //if (DGMaterialRoughUserDeleteRows == null) { return; }
            //MessageBox.Show(DGMaterialRoughUserDeleteRows.Count.ToString());
            //int[] DeleteRow = new int[] { };
            //for(int i=0;i<DGMaterialRoughUserDeleteRows.Count;i++)
            //{
            //    Array.Resize(ref DeleteRow, i + 1);
            //    DeleteRow[i] = DGMaterialRoughUserDeleteRows[i].Index;
            //}
            EscapeDGMaterialRoughnessCellContentChanged = true;

            //MessageBox.Show("DD");
            //重裝矩陣項目.
            Array.Resize(ref MaterialsCoefArray, 0);
            MaterialRoughnessArrayCount = 0;
            for (int i = 0; i < DGMaterialRough.Rows.Count; i++)
            {
                DGMaterialRough.Rows[i].Cells[0].Value = (i + 1).ToString();//序號更新.
                Array.Resize(ref MaterialsCoefArray, i + 1);
                MaterialRoughnessArrayCount += 1;
                try
                { MaterialsCoefArray[i].Id1 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[1].Value.ToString()]; }
                catch { MaterialsCoefArray[i].Id1 = -9999; }
                try
                {
                    MaterialsCoefArray[i].Id2 = MaterialNameToArraySubScript[DGMaterialRough.Rows[i].Cells[2].Value.ToString()];
                }
                catch { MaterialsCoefArray[i].Id2 = -9999; }
                if (!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[i].coef))
                {
                    MaterialsCoefArray[i].coef = -9999;
                }
            }
        }
        bool EscapeDGMaterialRoughnessCellContentChanged = false;
        private void btnAddRow_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(使用者手動更新材質與摩擦.ToString());
            if (MaterialCount == 0) { return; }

            //額外新增一個摩擦係數設定組合.
            DGMaterialRough.Rows.Add(new object[] { (DGMaterialRough.Rows.Count + 1).ToString(), "", "", "" });
            //修改矩陣.
            Array.Resize(ref MaterialsCoefArray, MaterialRoughnessArrayCount + 1);
            MaterialsCoefArray[MaterialRoughnessArrayCount].Id1 = -9999;
            MaterialsCoefArray[MaterialRoughnessArrayCount].Id2 = -9999;
            MaterialsCoefArray[MaterialRoughnessArrayCount].coef = -9999; 
            //其餘參數都不設定.
            MaterialRoughnessArrayCount += 1;
            //設定Combobox內容.
            foreach (DataGridViewRow row in DGMaterialRough.Rows)
            {
                var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                cell.DataSource = MaterialArray;//更新下拉式選單.
                var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                cell2.DataSource = MaterialArray;
            }
            EscapeDGMaterialRoughnessCellContentChanged = true;//避免再次進入CellValueChanged內.

        }
        private void DGMaterialRough_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //設定摩擦係數時.
            //不做任何事情,等實際運行前再檢查.
            if(!使用者手動更新材質與摩擦)
            {
                return;
            }
            if (e.RowIndex == -1)
            {
                return;
            }
            if (EscapeDGMaterialRoughnessCellContentChanged)
            {
                EscapeDGMaterialRoughnessCellContentChanged = false;
                return;
            }
            switch (e.ColumnIndex)
            {
                case 0:
                    break;
                case 1:
                    try
                    {
                        MaterialsCoefArray[e.RowIndex].Id1 = MaterialNameToArraySubScript[DGMaterialRough.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()];
                    }
                    catch
                    {
                        MaterialsCoefArray[e.RowIndex].Id1 = -9999;
                    }
                    break;
                case 2:
                    try
                    {
                        MaterialsCoefArray[e.RowIndex].Id2 = MaterialNameToArraySubScript[DGMaterialRough.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()];
                    }
                    catch { MaterialsCoefArray[e.RowIndex].Id2 = -9999; }
                    break;
                case 3:

                    //MessageBox.Show("H");
                    string Temp;
                    try
                    {
                        Temp = DGMaterialRough.Rows[e.RowIndex].Cells[3].Value.ToString();
                        if (!double.TryParse(Temp, out MaterialsCoefArray[e.RowIndex].coef))
                        {
                            MaterialsCoefArray[e.RowIndex].coef = -9999;
                        }
                        else
                        {
                           // MessageBox.Show("OK:" + DGMaterialRough.Rows[e.RowIndex].Cells[3].Value.ToString());
                        }
                    }
                    catch
                    {
                        //出現錯誤.
                    }


                    break;
                default:
                    break;
            }
        }



        #endregion
        #region 其他檢核
        private void chk_BlockWeightCalc_HO_CheckedChanged(object sender, EventArgs e)
        {
            if(chk_BlockWeightCalc_HO.Checked)
            {
                //勾選.
                label_HO_1.Enabled = true;
                label_HO_2.Enabled = true;
                label_HO_3.Enabled = true;
                textBox_HO_KDL.ReadOnly = false;
                textBox_HO_Gamma.ReadOnly = false;
                textBox_HO_slopeangle.ReadOnly = false;
                textBox_HO_KDL.Enabled = true;
                textBox_HO_Gamma.Enabled = true;
                textBox_HO_slopeangle.Enabled = true;
            }
            else
            {
                label_HO_1.Enabled = false;
                label_HO_2.Enabled = false;
                label_HO_3.Enabled = false;
                textBox_HO_KDL.ReadOnly = true;
                textBox_HO_Gamma.ReadOnly = true;
                textBox_HO_slopeangle.ReadOnly = true;
                textBox_HO_KDL.Enabled = false;
                textBox_HO_Gamma.Enabled = false;
                textBox_HO_slopeangle.Enabled = false;
            }
        }

        private void chk_BlockWeightCalc_HE_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BlockWeightCalc_HE.Checked)
            {
                //勾選.
                label_HE_1.Enabled = true;
                label_HE_2.Enabled = true;
                label_HE_3.Enabled = true;
                textBox_HE_KDL.ReadOnly = false;
                textBox_HE_Gamma.ReadOnly = false;
                textBox_HE_slopeangle.ReadOnly = false;
                textBox_HE_KDL.Enabled = true;
                textBox_HE_Gamma.Enabled = true;
                textBox_HE_slopeangle.Enabled = true;
            }
            else
            {
                label_HE_1.Enabled = false;
                label_HE_2.Enabled = false;
                label_HE_3.Enabled = false;
                textBox_HE_KDL.ReadOnly = true;
                textBox_HE_Gamma.ReadOnly = true;
                textBox_HE_slopeangle.ReadOnly = true;
                textBox_HE_KDL.Enabled = false;
                textBox_HE_Gamma.Enabled = false;
                textBox_HE_slopeangle.Enabled = false;
            }
        }

        private void chk_BlockWeightCalc_BD_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_BlockWeightCalc_BD.Checked)
            {
                //勾選.
                label_BD_1.Enabled = true;
                label_BD_2.Enabled = true;
                label_BD_3.Enabled = true;
                label_BD_4.Enabled = true;
                textBox_BD_KDL.ReadOnly = false;
                textBox_BD_Gamma.ReadOnly = false;
                textBox_BD_slopeangle.ReadOnly = false;
                textBox_BD_Kt.ReadOnly = false;
                textBox_BD_KDL.Enabled = true;
                textBox_BD_Gamma.Enabled = true;
                textBox_BD_slopeangle.Enabled = true;
                textBox_BD_Kt.Enabled = true;
            }
            else
            {
                label_BD_1.Enabled = false;
                label_BD_2.Enabled = false;
                label_BD_3.Enabled = false;
                label_BD_4.Enabled = false;
                textBox_BD_KDL.ReadOnly = true;
                textBox_BD_Gamma.ReadOnly = true;
                textBox_BD_slopeangle.ReadOnly = true;
                textBox_BD_Kt.ReadOnly = true;
                textBox_BD_KDL.Enabled = false;
                textBox_BD_Gamma.Enabled = false;
                textBox_BD_slopeangle.Enabled = false;
                textBox_BD_Kt.Enabled = false;
            }
        }
        private void chk_HeadBreastCalc_CheckedChanged_1(object sender, EventArgs e)
        {
            if(!chk_HeadBreastCalc.Checked)
            {
                label_BreastCheck_1.Enabled = false;
                label_BrestCheck_2.Enabled = false;
                textBox_ConcreteAllowStress.Enabled = false;
                textBox_ConcreteAllowStress.ReadOnly = true;
                textBox_BK.ReadOnly = true;
                textBox_BK.Enabled = false;
            }
            else
            {
                label_BreastCheck_1.Enabled = true;
                label_BrestCheck_2.Enabled = true;
                textBox_ConcreteAllowStress.Enabled = true;
                textBox_ConcreteAllowStress.ReadOnly = false;
                textBox_BK.ReadOnly = false;
                textBox_BK.Enabled = true;
            }
        }
        private void textBox_HO_KDL_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HO_Gamma_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HO_slopeangle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HE_KDL_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HE_Gamma_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_HE_slopeangle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BD_KDL_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BD_Gamma_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BD_slopeangle_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BD_Kt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_ConcreteAllowStress_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_BK_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        #endregion
        private void textBox_SFOver_TextChanged(object sender, EventArgs e)
        {

        }
        private void DGMaterialRough_KeyUp(object sender, KeyEventArgs e)
        {
            if(DGMaterialRough.CurrentCell.RowIndex!=-1)
            {
                if(DGMaterialRough.CurrentCell.ColumnIndex==3)
                {
                    //更新.
                    string Temp;
                    int rowindex = DGMaterialRough.CurrentCell.RowIndex;
                    try
                    {
                        Temp = DGMaterialRough.Rows[rowindex].Cells[3].Value.ToString();
                        if (!double.TryParse(Temp, out MaterialsCoefArray[rowindex].coef))
                        {
                            MaterialsCoefArray[rowindex].coef = -9999;
                        }
                        else
                        {
                            //MessageBox.Show("OK:" + DGMaterialRough.Rows[rowindex].Cells[3].Value.ToString());
                        }
                    }
                    catch
                    {
                        //出現錯誤.
                    }
                }
            }
        }
        #region Tab控制器
        //Tab Control控制器.
        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if(("D1D2D3D4D5D6D7D8D9D0".Contains(e.KeyCode.ToString())) && e.Control)
            {
                //按下數字鍵
                int selectedindex;
                int.TryParse(e.KeyCode.ToString().Replace("D", ""), out selectedindex);
                //MessageBox.Show(selectedindex.ToString());
                if (selectedindex < 1 || selectedindex > 5)
                {
                    //Do nothing.
                }
                else
                {
                    tabControl1.SelectedIndex = (selectedindex - 1);
                }
            }
            //e.Handled = !"D1D2D3D4D5D6D7D8D9D0".Contains(e.KeyCode.ToString());
            //MessageBox.Show(e.KeyCode.ToString());
        }

        #endregion
        Point? prePosition = null;
        ToolTip ToolTipChart = new ToolTip();
        private void chart_Plot_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void chart_Plot_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void textBox_HB_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_H0_TextChanged(object sender, EventArgs e)
        {

        }
        #region 檢視使用者說明書

        private void 檢視使用者說明書ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
            mainForm.檢示使用者說明書ToolStripMenuItem_Click(sender, e);//呼叫相同方法.
        }


        #endregion
        #region "輸出Word報表"
        private string WordOutputMsg = "";
        private void 輸出Word檔案ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(isExporting)
            {
                return;
            }

            if (object.Equals(Mod, null))
            {
                //MessageBox.Show("你的計算主體'MOD'為Null!!!!!");
                MessageBox.Show("您目前沒有完成的檢核結果!請重新檢核,若您無法使用檢核功能,請確認您的軟體已授權,或是聯絡開發商", "Word檔案輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string tempplateFile = "Output_Template.docx";
            if(!File.Exists(tempplateFile))
            {
                //錯誤.
                MessageBox.Show("您輸出Word報表的功能出現嚴重錯誤!找不到Template!", "Word報表輸出功能受損", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            if (SFD_WordOutput.ShowDialog() == DialogResult.OK && SFD_WordOutput.FileName != "")
            {
                string getpathword = SFD_WordOutput.FileName;
                if (File.Exists(getpathword))
                {
                    //規定目前此excel檔案不可被開啟中.
                    if (IsFileLocked(new FileInfo(getpathword)))
                    {
                        MessageBox.Show("您所預儲存的檔案已經存在且已被鎖定!!!" + Environment.NewLine + "處理中止，此檔案可能被其他檔案編輯中或是目前正被Word打開中", "輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                tsp_progressbar.Style = ProgressBarStyle.Marquee;
                tsp_progressbar.MarqueeAnimationSpeed = 10;
                tsp_progressbar.Visible = true;
                tsp_cond.Text = "輸出Word檔案報表中";

                //先將圖片輸出.
                string[] TempName = new string[] { };
                for (int i = 0; i < chart_Plot.Series.Count; i++)
                {
                    if (chart_Plot.Series[i].IsVisibleInLegend)
                    {
                        Array.Resize(ref TempName, TempName.GetLength(0) + 1);
                        TempName[TempName.GetUpperBound(0)] = chart_Plot.Series[i].Name;
                        chart_Plot.Series[i].IsVisibleInLegend = false;
                    }
                }
                //chart_Plot.Legends.
                if (listBox_SectSetting.SelectedIndex != -1)
                {
                    //MessageBox.Show(BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]);
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].Color = Color.Black;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].BorderWidth = 2;
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderColor = Color.Black;
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderWidth = 2;
                }
                //Recording Old Xmin, Xmax and Xspace.
                //TT.AxisX = chart_Plot.ChartAreas[0].AxisX;
                //TT.AxisY = chart_Plot.ChartAreas[0].AxisY;
                //double oldXmin = chart_Plot.ChartAreas[0].AxisX.Minimum;
                //double oldXmax = chart_Plot.ChartAreas[0].AxisX.Maximum;
                //double oldSpace = chart_Plot.ChartAreas[0].AxisX.Interval;
                for (int i = chart_Plot.Annotations.Count - 1; i >= 0; i--)
                {
                    TextAnnotation TT = (TextAnnotation)chart_Plot.Annotations[i];
                    if (TT.Text == "海側")
                    {
                        TT.Visible = false;
                        //chart_Plot.Annotations.RemoveAt(i);
                    }


                }
                chart_Plot.SaveImage(PNGStoredFolderPath, ChartImageFormat.Png);
                for (int i = 0; i < TempName.GetLength(0); i++)
                {
                    chart_Plot.Series[TempName[i]].IsVisibleInLegend = false;
                }
                if (listBox_SectSetting.SelectedIndex != -1)
                {
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderColor = Color.Red;
                    //chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderWidth = 3;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].Color = Color.Red;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].BorderWidth = 3;
                }
                for (int i = chart_Plot.Annotations.Count - 1; i >= 0; i--)
                {
                    TextAnnotation TT = (TextAnnotation)chart_Plot.Annotations[i];
                    if (TT.Text == "海側")
                    {
                        TT.Visible = true;
                        //chart_Plot.Annotations.RemoveAt(i);
                    }


                }

                //Runing the backgroundworker.
                WordOutputMsg = "ERROR";
                isExporting = true;
                bk_OutputWordReport.RunWorkerAsync(SFD_WordOutput.FileName);


                //string getMsg = 輸出Word報表(SFD_WordOutput.FileName);
            }
            //btn_OutputExcel_Click(sender, e);//目前已改為輸出Word檔案.
        }
        private void bk_OutputWordReport_DoWork(object sender, DoWorkEventArgs e)
        {
            WordOutputMsg = 輸出Word報表(e.Argument.ToString());

        }
        private void bk_OutputWordReport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void bk_OutputWordReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            tsp_progressbar.Visible = false;
            this.Refresh();
            isExporting = false;
            if (WordOutputMsg == "ok")
            {
                tsp_cond.Text = "您已輸出完成Word檔案,謝謝使用";
                MessageBox.Show("輸出完成!!", "輸出Word報表檔案完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //System.Threading.Thread.Sleep(000);//暫停兩秒.
                儲存XML專案檔(VESDStoredFolderPath, false);
                this.mainForm.發送檔案給主機(VESDStoredFolderPath);
                File.Delete(VESDStoredFolderPath);
                //傳送Log檔案.
                //string LogTempPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TempLog.log";

                //Mod.OutPutLogFile(LogTempPath);
                //this.mainForm.發送操作指令("TRANSFER:檢核Record.Log"   + "\n" + mainForm.LoginInUserID + "\n" + mainForm.LoginInUserName);
                //this.mainForm.發送檔案給主機(LogTempPath);
                //File.Delete(LogTempPath);
                //System.Threading.Thread.Sleep(5000);//暫停兩秒.


                this.Refresh();
                //System.Threading.Thread.Sleep(5000);//暫停兩秒.
                
                FileInfo f1 = new FileInfo(SFD_WordOutput.FileName);
                this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')完成標準海堤檢核並輸出報表(檔案名稱為'" + f1.Name + "'),員工編號為'" +mainForm.LoginInUserID + "',員工名稱為'" + mainForm.LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                //輸出專案檔的備份.
                //儲存XML專案檔(VESDStoredFolderPath, false);
                //this.mainForm.發送檔案給主機(VESDStoredFolderPath);
                //File.Delete(VESDStoredFolderPath);
                //this.mainForm.發送操作指令("TRANSFER:" + f1.Name + "\n" + mainForm.LoginInUserID + "\n" + mainForm.LoginInUserName);
                //this.mainForm.發送檔案給主機(SFD_WordOutput.FileName);
                if (chk_OpenFileAfterOutput.Checked)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                        p.StartInfo.FileName = SFD_WordOutput.FileName;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        p.Start();
                    }
                    catch
                    {
                        //不做任何事情.
                    }

                }
            }
            else
            {
                MessageBox.Show("您的Word報表輸出出現錯誤!" + Environment.NewLine + WordOutputMsg.Replace("ERROR:", ""), "WORD輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tsp_cond.Text = "您沒有成功輸出檢核報表";
            }
            if(File.Exists(PNGStoredFolderPath))
            {
                File.Delete(PNGStoredFolderPath);
            }
        }
        private string 輸出Word報表(string getPath)
        {
            string OutMsg = "ok";
            if(object.Equals(Mod,null))
            {
                //沒有作檢核.
                //MessageBox.Show("您必須要先進行檢核才可輸出Word檔案!");
                OutMsg = "ERROR:您必須要先進行檢核才可輸出Word檔案";
                return OutMsg;
            }
            string tempplateFile = "Output_Template.docx";// C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\x64\\Release\\Output_Template.docx";//!!!!!!!!!!!!!!!!!!!!!!
            string outputFile;// = "C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\x64\\Release\\TestWord.docx";//!!!!!!!!!!!!!!!!!!!!!!!!!!!
            outputFile = getPath;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            FileInfo f1 = new FileInfo(outputFile);
            if (IsFileLocked(f1))
            {
                //無法輸出
                //MessageBox.Show("您所欲輸出的檔案目前開啟或是被鎖定中,請檢視此檔案是否可被編輯,並且關閉任何開啟此檔案中的程式","輸出Word檔案錯誤",MessageBoxButtons.OK,MessageBoxIcon.Error);
                OutMsg = "ERROR:WORD檔案仍被鎖定中:" + getPath;
                return OutMsg;
                
            }
            File.Copy(tempplateFile, outputFile,true);
            Mod.Get_DataBank_Data(); //Loading Running Result data.

            WORD.Application wdApplication = null;
            wdApplication = new WORD.Application();
            wdApplication.Visible = false;// true;
            if (wdApplication != null)
            {
                try
                {
                    //WORD.Document newDocument = wdApplication.Documents.Add();
                    WORD.Document newDocument = wdApplication.Documents.Open(outputFile);

                    WORD.Range wdrange = null;
                    WORD.Table TableRef = null;
                    object unit = WORD.WdUnits.wdCharacter;
                    object lu = WORD.WdUnits.wdLine;
                    //object story = WORD.WdUnits.wdLine;
                    object count = 1;
                    object dcount = 2;
                    object tcount = 3;

                    //取得第一個Table:
                    //1. 填表資訊.
                    TableRef = newDocument.Tables[1];
                    //填表人員工編號.
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.填表人ID; //mainForm.LoginInUserID.ToString();
                                                                       //填表人名稱.
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.填表人名稱;//mainForm.LoginInUserName.ToString();
                    //時間.
                    TableRef.Rows[3].Cells[2].Range.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");

                    //第二個表格.
                    //2. 設計條件.
                    TableRef = newDocument.Tables[2];
                    //MessageBox.Show(TableRef.Rows[1].Cells[1].Range.Text.ToString());
                    //深海波波向.
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.深海波波向; //cmb_DeepWaveActDir.SelectedItem.ToString();
                    //海側方向.
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.海側方向; //cmb_seawaveDir.SelectedItem.ToString();
                    //深海波波高(m).
                    TableRef.Rows[3].Cells[2].Range.Text = RCOL.深海波波高; ;//Mod.VarBank.H0.ToString("0.00");
                    //深海波週期(sec)
                    TableRef.Rows[4].Cells[2].Range.Text = RCOL.深海波週期;//textBox_T0.Text.ToString();
                    //設計潮位(m)
                    TableRef.Rows[5].Cells[2].Range.Text = RCOL.設計潮位;//textBox_HWL.Text.ToString();
                    //海床波度
                    TableRef.Rows[6].Cells[2].Range.Text = RCOL.海床波度; //textBox_Slope.Text.ToString();
                    //折射係數Kr.
                    TableRef.Rows[7].Cells[2].Range.Text = RCOL.折射係數;//textBox_Kr.Text.ToString();
                    //淺化係數Ks.
                    TableRef.Rows[8].Cells[2].Range.Text = RCOL.淺化係數;//textBox_Ks.Text.ToString();
                    //繞射係數Kd.
                    TableRef.Rows[9].Cells[2].Range.Text = RCOL.繞射係數;// textBox_Kd.Text.ToString();
                    //波力折減係數lambda
                    TableRef.Rows[10].Cells[2].Range.Text = RCOL.波力折減係數;// textBox_Lenda.Text.ToString();
                    //入射波與堤體法線之垂向夾角(未修正).
                    TableRef.Rows[11].Cells[2].Range.Text = RCOL.入射波與堤體法線之垂線夾角未修正;//textBox_Beta.Text.ToString();
                    //入射波與堤體法線之垂向夾角(修正後).
                    TableRef.Rows[12].Cells[2].Range.Text = RCOL.入射波與堤體法線之垂線夾角修正後;//textBox_BataFix.Text.ToString();
                    //地面線(m)
                    TableRef.Rows[13].Cells[2].Range.Text = RCOL.地面線;// textBox_GroundELE.Text.ToString();
                    //消波塊高程(m)
                    TableRef.Rows[14].Cells[2].Range.Text = RCOL.消波塊高程;//textBox_ArmorBlockEle.Text.ToString();
                    //MessageBox.Show(Mod.VarBank.H0.ToString("0.00"));


                    //第三個表格.
                    //外力計算:水深條件.
                    TableRef = newDocument.Tables[3];
                    //h(m)
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.h.ToString("0.00");
                    //h'(m)
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.h_plun.ToString("0.00");
                    //hc(m)
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.hc.ToString("0.00");
                    //d(m)
                    TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.d.ToString("0.00");

                    //第四個表格.
                    //有義波高H1/3及最大波高Hmax.
                    //參數表1-1.
                    TableRef = newDocument.Tables[4];
                    //L0(m)
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.L0.ToString("0.00");
                    //H0'(m)
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.H0_plun.ToString("0.00");
                    //L(m)
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.L.ToString("0.00");
                    //h/L0
                    TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.h_D_L0.ToString("0.00");

                    //參數表1-2.
                    //H1/3與Hmax概算表.
                    TableRef = newDocument.Tables[5];
                    //B0.
                    TableRef.Rows[3].Cells[1].Range.Text = Mod.VarBank.beta0.ToString("0.00");
                    //B1.
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.beta1.ToString("0.00");
                    //Bmax.
                    TableRef.Rows[3].Cells[3].Range.Text = Mod.VarBank.betaMax.ToString("0.00");
                    //B0*.
                    TableRef.Rows[3].Cells[4].Range.Text = Mod.VarBank.beta0_Star.ToString("0.00");
                    //B1*.
                    TableRef.Rows[3].Cells[5].Range.Text = Mod.VarBank.beta1_Star.ToString("0.00");
                    //Bmax*.
                    TableRef.Rows[3].Cells[6].Range.Text = Mod.VarBank.betaMax_Star.ToString("0.00");

                    //參數表1-3.
                    //有義波高其餘計算結果.
                    TableRef = newDocument.Tables[6];
                    //有義波高H1/3(m)
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.Hs.ToString("0.00");//!!!!!!!!!!!!!!!!!!!!!!!!!
                                                                                           //tan(theta)
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.海床波度; //textBox_Slope.Text.ToString();
                    //hb(內算)[3].
                    //hb(外供)[4].
                    if (textBox_HB.Text.ToString() == "")
                    {
                        TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.hb.ToString("0.00");
                        //刪除外供.
                        TableRef.Rows[4].Delete();
                    }
                    else
                    {
                        TableRef.Rows[3].Delete();
                        TableRef.Rows[3].Cells[2].Range.Text = RCOL.HB;// textBox_HB.Text.ToString();
                    }
                    //最大波高Hmax[5-->4].
                    TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.Hmax.ToString("0.00");

                    //第七個表格.
                    //波壓強度係數.
                    TableRef = newDocument.Tables[7];
                    //a1.
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.alpha1.ToString("0.00");
                    //a2.
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.alpha2.ToString("0.00");
                    //a3.
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.alpha3.ToString("0.00");

                    //第八個表格.
                    //液壓作用高度.
                    //n*
                    TableRef = newDocument.Tables[8];
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.eta_Star.ToString("0.00");
                    //hc*
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.hc_Star.ToString("0.00");

                    //第九個表格.
                    //波壓力計算.
                    TableRef = newDocument.Tables[9];
                    //P1(t/m^2)
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.P1.ToString("0.00");
                    //P2(t/m^2)
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.P2.ToString("0.00");
                    //P3(t/m^2)
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.P3.ToString("0.00");
                    //P4(t/m^2)
                    TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.P4.ToString("0.00");


                    //第十個表格.
                    //插入各個EL分區之P,FP,Y與MP計算結果.
                    //wdrange = TableRef.Rows[4].Cells[1].Range;
                    //wdrange.Select();
                    //wdApplication.Selection.MoveDown(ref lu, 1, Type.Missing);
                    //wdApplication.Selection.MoveDown(ref lu, 1, Type.Missing);
                    //TableRef = newDocument.Tables.Add(wdApplication.Selection.Range,Mod.VarBank.EL_Out.GetLength(0)+2, 5);
                    //TableRef.Rows[1].Cells[1].Range.Text = "分區";
                    //TableRef.Rows[1].Cells[2].Range.Text = "波壓強度P(t/m^2)";
                    TableRef = newDocument.Tables[10];
                    //根據有多少EL進行插入動作.
                    for (int i = 1; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[2]);//在原本Row[2]之前插入列.
                    }
                    //填入資訊.
                    for (int i = Mod.VarBank.EL_Out.GetUpperBound(0); i >= 0; i--)
                    {
                        //分區.
                        int ix = Mod.VarBank.EL_Out.GetUpperBound(0) - i;
                        TableRef.Rows[2 + ix].Cells[1].Range.Text = "EL " + (Mod.VarBank.EL_Out[i].EL < 0 ? "-" : "+") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        //波壓強度.
                        TableRef.Rows[2 + ix].Cells[2].Range.Text = Mod.VarBank.EL_Out[i].P.ToString("0.00");
                        //波壓FP
                        TableRef.Rows[2 + ix].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].FP.ToString("0.00");
                        //力矩Y
                        TableRef.Rows[2 + ix].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Y.ToString("0.00");
                        //傾倒彎矩Mp
                        TableRef.Rows[2 + ix].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Mp.ToString("0.00");
                    }
                    //填入最後合計.
                    //波壓.
                    TableRef.Rows[2 + Mod.VarBank.EL_Out.GetLength(0)].Cells[3].Range.Text = Mod.VarBank.Fp.ToString("0.00");
                    //傾倒彎矩
                    TableRef.Rows[2 + Mod.VarBank.EL_Out.GetLength(0)].Cells[5].Range.Text = Mod.VarBank.Mp.ToString("0.00");


                    //第十一個表格.
                    //揚壓力計算
                    TableRef = newDocument.Tables[11];
                    //Pu
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.Pu.ToString("0.00");
                    //Fu
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.Fu.ToString("0.00");
                    //Mu
                    TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.Mu.ToString("0.00");

                    //第十二個表格.
                    //堤體自重與抵抗彎矩表.
                    TableRef = newDocument.Tables[12];
                    //插入不足欄位,插入不足之Block數量.
                    BlockResult[] BlockResultCol = Mod.VarBank.Block_Out;
                    for (int i = BlockResultCol.GetUpperBound(0) - 1; i >= 0; i--)
                    {
                        TableRef.Rows.Add(TableRef.Rows[2]);
                    }
                    int rowstart = 2;
                    int rowend = 2;
                    for (int i = Mod.VarBank.EL_Out.GetUpperBound(0) - 1; i >= 0; i--)
                    {

                        int[] BlocKID = Mod.VarBank.EL_Out[i].BlockNum;
                        //合併所需之儲存格.
                        rowend = rowstart + BlocKID.GetLength(0) - 1;
                        TableRef.Rows[rowend].Cells[1].Range.Text = "EL " + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString(); ;
                        for (int ii = 0; ii <= BlocKID.GetUpperBound(0); ii++)
                        {
                            int blockidGet = BlocKID[ii];
                            //編號.
                            TableRef.Rows[rowstart + ii].Cells[2].Range.Text = BlockListSubScriptToName[blockidGet];//BlockResultCol[blockidGet];// "(" + (blockidGet + 1).ToString() + ")"; ;
                                                                                                                    //A
                            TableRef.Rows[rowstart + ii].Cells[3].Range.Text = BlockResultCol[blockidGet].A.ToString("0.00");
                            //Y
                            TableRef.Rows[rowstart + ii].Cells[4].Range.Text = BlockResultCol[blockidGet].garma.ToString("0.00");
                            //W
                            TableRef.Rows[rowstart + ii].Cells[5].Range.Text = BlockResultCol[blockidGet].W.ToString("0.00");
                            //X
                            TableRef.Rows[rowstart + ii].Cells[6].Range.Text = BlockResultCol[blockidGet].X.ToString("0.00");
                            //Mw.
                            TableRef.Rows[rowstart + ii].Cells[7].Range.Text = BlockResultCol[blockidGet].Mw.ToString("0.00");
                        }
                        rowstart = rowend + 1;
                    }
                    //填入合計資料.
                    //W.
                    TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.W.ToString("0.00");
                    //Mw.
                    TableRef.Rows[rowstart].Cells[7].Range.Text = Mod.VarBank.Mw.ToString("0.00");
                    //合併儲存格.


                    rowstart = 2;
                    rowend = 2;
                    int minuscount = 0;
                    for (int i = Mod.VarBank.EL_Out.GetUpperBound(0) - 1; i >= 0; i--)
                    {
                        int[] BlocKID = Mod.VarBank.EL_Out[i].BlockNum;
                        rowend = rowstart + BlocKID.GetLength(0) - 1;
                        if (BlocKID.GetLength(0) > 2)
                        {
                            TableRef.Columns[1].Cells[rowstart - minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                            minuscount += BlocKID.GetLength(0) - 1;
                        }
                        rowstart = rowend + 1;
                    }
                    //TableRef.Rows[rowstart-minuscount].Cells[1].Merge(TableRef.Rows[rowstart-minuscount].Cells[2]);
                    //TableRef.Rows[1].Cells[1].Merge(TableRef.Rows[1].Cells[2]);

                    wdrange = TableRef.Columns[1].Cells[rowstart - minuscount].Range;
                    wdrange.Select();
                    wdApplication.Selection.MoveDown(ref lu, 1, Type.Missing);

                    //插入圖.
  
                    wdApplication.Selection.InlineShapes.AddPicture(PNGStoredFolderPath);


                    //第十三個表格.
                    //堤體安定檢查.
                    TableRef = newDocument.Tables[13];
                    //滑動檢查.
                    TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.CalBody_SlideSF.ToString("0.00");
                    if (Mod.VarBank.CalBody_SlideSF >= double.Parse(RCOL.滑動安全係數))//textBox_SFSlide.Text.ToString()))
                    {
                        TableRef.Rows[1].Cells[3].Range.Text = ">=" + RCOL.滑動安全係數 + "...OK";//textBox_SFSlide.Text.ToString() + " ...OK";
                    }
                    else
                    {
                        TableRef.Rows[1].Cells[3].Range.Text = "<" + RCOL.滑動安全係數 + "...NG";//textBox_SFSlide.Text.ToString() + " ...N.G.";
                    }
                    //傾倒檢查.
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.CalBody_RotateSF.ToString("0.00");
                    if (Mod.VarBank.CalBody_RotateSF >= double.Parse(RCOL.傾倒安全係數))//textBox_SFOver.Text.ToString()))
                    {
                        TableRef.Rows[2].Cells[3].Range.Text = ">=" + RCOL.傾倒安全係數 + "...OK";//textBox_SFOver.Text.ToString() + " ...OK";
                    }
                    else
                    {
                        TableRef.Rows[2].Cells[3].Range.Text = "<" + RCOL.傾倒安全係數 + "...NG";//textBox_SFOver.Text.ToString() + " ...N.G.";
                    }

                    //其他檢核.
                    //其他檢核為選擇性檢核項目.
                    //5-1. 消波工重量計算.
                    //5-1-A. 堤身段(港外側): 第十四個表格.
                    //  若選擇不做,則合併TABLE之儲存格,並畫上斜線.
                    TableRef = newDocument.Tables[14];
                    if (RCOL.堤身段港外側消波工重量計算)//chk_BlockWeightCalc_HO.Checked)
                    {
                        //消波形塊安定係數Kd.
                        TableRef.Rows[1].Cells[2].Range.Text = RCOL.堤身段港外側消波形塊安定係數;//textBox_HO_KDL.Text.ToString();
                        //消波塊單位體積重量rc.
                        TableRef.Rows[2].Cells[2].Range.Text = RCOL.堤身段港外側消波塊單位體積重量;//textBox_HO_Gamma.Text.ToString();
                        //消波塊斜坡面與水平面之夾角.
                        TableRef.Rows[3].Cells[2].Range.Text = RCOL.堤身段港外側消波塊斜坡面與水平面夾角;//textBox_HO_slopeangle.Text.ToString();
                        //W1(Ton).
                        TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.W1.ToString("0.00");
                    }
                    else
                    {
                        TableRef.Columns[2].Cells[1].Merge(TableRef.Columns[2].Cells[4]);
                        try
                        {
                            TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalUp].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        }
                        catch
                        {
                            TableRef.Columns[2].Cells[1].Range.Text = "---";
                        }
                }

                    //5-1-B. 堤頭部加強: 第十五個表格.
                    TableRef = newDocument.Tables[15];
                    if (RCOL.堤頭部加強消波工重量計算)//chk_BlockWeightCalc_HE.Checked)
                    {
                        //消波形塊安定係數Kd.
                        TableRef.Rows[1].Cells[2].Range.Text = RCOL.堤頭部加強消波形塊安定係數; //textBox_HE_KDL.Text.ToString();
                        //消波塊單位體積重量rc.
                        TableRef.Rows[2].Cells[2].Range.Text = RCOL.堤頭部加強消波塊單位體積重量;// textBox_HE_Gamma.Text.ToString();
                        //消波塊斜坡面與水平面之夾角.
                        TableRef.Rows[3].Cells[2].Range.Text = RCOL.堤頭部加強消波塊斜坡面與水平面夾角;//textBox_HE_slopeangle.Text.ToString();
                        //W2(Ton).
                        TableRef.Rows[4].Cells[2].Range.Text = Mod.VarBank.W2.ToString("0.00");
                    }
                    else
                    {
                        TableRef.Columns[2].Cells[1].Merge(TableRef.Columns[2].Cells[4]);
                        try
                        {
                            TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalUp].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        }
                        catch
                        {
                            TableRef.Columns[2].Cells[1].Range.Text = "---";
                        }
                        
                    }

                    //5-1-C. 堤身段(航道側): 第十六個表格.
                    TableRef = newDocument.Tables[16];
                    if (RCOL.堤身段航道側消波工重量計算)//chk_BlockWeightCalc_BD.Checked)
                    {
                        //消波形塊安定係數Kd.
                        TableRef.Rows[1].Cells[2].Range.Text = RCOL.堤身段航道側消波形塊安定係數;//textBox_BD_KDL.Text.ToString();
                        //消波塊單位體積重量rc.
                        TableRef.Rows[2].Cells[2].Range.Text = RCOL.堤身段航道側消波塊單位體積重量;//textBox_BD_Gamma.Text.ToString();
                        //消波塊斜坡面與水平面之夾角.
                        TableRef.Rows[3].Cells[2].Range.Text = RCOL.堤身段航道側消波塊斜坡面與水平面夾角;//textBox_BD_slopeangle.Text.ToString();
                        //波高傳遞率Kt.
                        TableRef.Rows[4].Cells[2].Range.Text = RCOL.堤身段航道側波高傳遞率;//textBox_BD_Kt.Text.ToString();
                        //h'/h.
                        TableRef.Rows[5].Cells[2].Range.Text = (Mod.VarBank.h_plun / Mod.VarBank.h).ToString("0.00");
                        //H1/3.
                        TableRef.Rows[6].Cells[2].Range.Text = Mod.VarBank.Hs.ToString("0.00");
                        //hc / H1/3.
                        TableRef.Rows[7].Cells[2].Range.Text = (Mod.VarBank.hc / Mod.VarBank.Hs).ToString("0.00");
                        //Ht(m)
                        TableRef.Rows[8].Cells[2].Range.Text = (double.Parse(RCOL.堤身段航道側波高傳遞率) * Mod.VarBank.Hs).ToString("0.00");//textBox_BD_Kt.Text.ToString()) * Mod.VarBank.Hs).ToString("0.00");
                        //W3(Ton).
                        TableRef.Rows[9].Cells[2].Range.Text = Mod.VarBank.W3.ToString("0.00");
                    }
                    else
                    {
                        TableRef.Columns[2].Cells[1].Merge(TableRef.Columns[2].Cells[9]);
                        //TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalDown].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        try
                        {
                            TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalUp].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        }
                        catch
                        {
                            TableRef.Columns[2].Cells[1].Range.Text = "---";
                        }
                    }

                    //6-1. 胸牆部安定檢核計算.
                    //6-1-A. 滑動SF檢核: 第十七個表格.
                    TableRef = newDocument.Tables[17];
                    //1. 滑動SF.
                    if (RCOL.胸牆部安定檢核計算)//chk_HeadBreastCalc.Checked)
                    {
                        TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.CalUpper_SlideSF.ToString("0.00");
                        if (Mod.VarBank.CalUpper_SlideSF >= double.Parse(RCOL.滑動安全係數))//textBox_SFSlide.Text.ToString()))
                        {
                            //大於等於:安全.
                            //若檢核通過
                            TableRef.Rows[1].Cells[2].Range.Text += (">=" + RCOL.滑動安全係數 + "..OK");// textBox_SFSlide.Text.ToString() + "...OK");
                            for (int i = 2; i <= 5; i++)
                            {
                                TableRef.Rows[2].Delete();
                            }
                        }
                        else
                        {
                            //若檢核不通過.
                            TableRef.Rows[1].Cells[2].Range.Text += ("<" + RCOL.滑動安全係數 + "...NG");// textBox_SFSlide.Text.ToString() + "...NG");
                            //混凝土容許應力Vc.
                            TableRef.Rows[2].Cells[2].Range.Text = RCOL.胸牆部安定檢核計算混凝土容許應力;// textBox_ConcreteAllowStress.Text.ToString();
                            //BK(內算).
                            TableRef.Rows[3].Cells[2].Range.Text = Mod.VarBank.CalBk.ToString("0.00");
                            //BK'
                            TableRef.Rows[4].Cells[2].Range.Text = RCOL.胸牆部安定檢核計算BKplun; //textBox_BK.Text.ToString();

                            TableRef.Rows[5].Cells[2].Range.Text = RCOL.胸牆部安定EL以上 ; //textBox_BK.Text.ToString();
                        }
                    }
                    else
                    {
                        for (int i = 2; i <= 5; i++)
                        {
                            TableRef.Rows[2].Delete();
                        }
                        try
                        {
                            TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalUp].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        }
                        catch
                        {
                            TableRef.Columns[2].Cells[1].Range.Text = "---";
                        }
                    }

                    //6-1-B. 傾倒SF:第18個表格
                    //
                    TableRef = newDocument.Tables[18];
                    if (RCOL.胸牆部安定檢核計算)//chk_HeadBreastCalc.Checked)
                    {
                        TableRef.Rows[1].Cells[2].Range.Text = Mod.VarBank.CalUpper_RotateSF.ToString("0.00");
                        if (Mod.VarBank.CalUpper_RotateSF >= double.Parse(RCOL.傾倒安全係數))//textBox_SFOver.Text.ToString()))
                        {
                            TableRef.Rows[1].Cells[3].Range.Text = ">=" + RCOL.傾倒安全係數 + "...OK";//textBox_SFOver.Text.ToString() + "...OK";
                        }
                        else
                        {
                            TableRef.Rows[1].Cells[3].Range.Text = "<" + RCOL.傾倒安全係數 + "...NG";//textBox_SFOver.Text.ToString() + "...NG";
                        }
                    }
                    else
                    {
                        TableRef.Rows[1].Cells[2].Merge(TableRef.Rows[1].Cells[3]);
                        try
                        {
                            TableRef.Columns[2].Borders[WORD.WdBorderType.wdBorderDiagonalUp].LineStyle = WORD.WdLineStyle.wdLineStyleSingle;
                        }
                        catch
                        {
                            TableRef.Columns[2].Cells[1].Range.Text = "---";
                        }
                    }
                    newDocument.Save(); // (outputFile);
                    newDocument.Close(false, Type.Missing, Type.Missing);
                    OutMsg = "ok";

                }
                catch (Exception ex)
                {

                    OutMsg = "ERROR:WORD處理出現錯誤" + Environment.NewLine + ex.StackTrace.ToString() + Environment.NewLine + ex.Message.ToString();

                }


                //Process p = new Process();
                //p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                //p.StartInfo.FileName = outputFile;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                //p.Start();
                //OutMsg = "ok";
            }
            else
            {
                OutMsg = "ERROR:WORD APP無法成功開啟";
            }
            wdApplication.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wdApplication);

            return OutMsg;

        }
        private void 試驗輸出Word檔案()
        { 
            string outputFile = "C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\x64\\Release\\TestWord.docx";
            WORD.Application wdApplication = null;
            wdApplication = new WORD.Application();
            wdApplication.Visible = true;
            if(wdApplication!=null)
            {
                WORD.Document newDocument = wdApplication.Documents.Add();
                WORD.Range wdrange = null;
                WORD.Table TableRef = null;
                //WORD.Range wdrange = newDocument.Range(0, 0);
                //Following code will add a Nary Equation
                //wdrange.Select();
                //WORD.Range wdFunctionR = wdApplication.Selection.OMaths
                //    .Add(wdApplication.Selection.Range);
                //WORD.OMathFunction wdFunction = wdApplication.Selection
                //    .OMaths[1].Functions.Add(wdApplication.Selection.Range,
                //    WORD.WdOMathFunctionType.wdOMathFunctionNary);
                //WORD.OMathNary wdNary = wdFunction.Nary;
                //wdNary.Char = 8721;
                //wdNary.Grow = false;
                //wdNary.SubSupLim = false;
                //wdNary.HideSub = false;
                //wdNary.HideSup = false;
                ////Following code will setup value in Nary Function
                WORD.Selection wdSelection = null; // = wdApplication.Selection;
                
                object unit = WORD.WdUnits.wdCharacter;
                object lu = WORD.WdUnits.wdLine;
                object count = 1;
                object dcount = 2;
                object tcount = 3;
                //wdSelection.MoveLeft(ref unit, ref count);
                //wdSelection.TypeText("11");
                //wdSelection.MoveLeft(ref unit, ref tcount);
                //wdSelection.TypeText("12");
                //wdSelection.MoveDown(ref lu, ref count);
                //wdSelection.TypeText("13");
                //wdNary.Application.Visible = true;
                //wdrange.Text = "This is the first line of all\n";
                wdApplication.Selection.TypeText("This is first line " + wdApplication.Selection.Start.ToString() + "\n");
                object story = WORD.WdUnits.wdLine;
                wdApplication.Selection.MoveDown(ref story, 1, Type.Missing);
                wdApplication.Selection.TypeText ("This is second line " + wdApplication.Selection.Start.ToString() + "\n");
                wdApplication.Selection.MoveDown(ref story, 1, Type.Missing);
                wdApplication.Selection.TypeText("\u03B2max * " + wdApplication.Selection.Start.ToString() + "\n");
                wdApplication.Selection.MoveDown(ref story, 1, Type.Missing);
                //插入一個表格吧.
                wdrange = newDocument.Range(wdApplication.Selection.Start, wdApplication.Selection.Start);
                TableRef=newDocument.Tables.Add(wdrange, 10, 5);
                //測試Merge與撰寫內容以及格式.
                TableRef.Rows[1].Cells[1].Range.Text = "分區";
                TableRef.Rows[1].Cells[1].Range.Font.Bold = 1;
                TableRef.Rows[1].Cells[1].Range.Font.Size = 14;
                TableRef.Rows[1].Cells[1].Range.Font.Position = 1;
                TableRef.Rows[1].Cells[1].Range.Font.Name = "微軟正黑體";

                TableRef.Rows[1].Cells[3].Range.Text = "A(m2)"; // + wdApplication.Selection.End.ToString();
                TableRef.Rows[1].Cells[3].Range.Font.Bold = 1;
                TableRef.Rows[1].Cells[3].Range.Font.Size = 14;
                TableRef.Rows[1].Cells[3].Range.Font.Position = 1;
                TableRef.Rows[1].Cells[3].Range.Font.Name = "微軟正黑體";
                wdrange = TableRef.Rows[1].Cells[3].Range;
                wdrange.Select();
                //wdrange = newDocument.Range(wdApplication.Selection.End - 2, wdApplication.Selection.End -2); //(wdApplication.Selection.Start-2, wdApplication.Selection.Start - 2);
                //wdrange.Select();
                //wdrange.Font.Subscript = 3;
                wdrange = newDocument.Range(wdApplication.Selection.End - 3, wdApplication.Selection.End - 2);
                wdrange.Font.Superscript = 3;
                //MessageBox.Show(wdApplication.Selection.Start.ToString() + ":" + wdApplication.Selection.End.ToString());
                //MessageBox.Show(wdApplication.Selection.Start.ToString());
                //wdrange.Characters(3) =;
                //TableRef.Rows[1].Cells[3

                TableRef.Rows[1].Cells[1].Merge(TableRef.Rows[1].Cells[2]);
                TableRef.Rows[1].Cells[1].Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter;
                TableRef.Rows[1].Cells[1].VerticalAlignment = WORD.WdCellVerticalAlignment.wdCellAlignVerticalCenter;
                TableRef.Rows[1].Cells[2].Range.ParagraphFormat.Alignment = WORD.WdParagraphAlignment.wdAlignParagraphCenter;
                TableRef.Rows[10].Select();
                wdApplication.Selection.MoveDown(ref story, 1, Type.Missing);
                wdApplication.Selection.TypeText("\n");
                wdApplication.Selection.MoveUp(ref story, 1, Type.Missing);
                string PNGStoredFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP.PNG";
                string[] TempName = new string[] { };
                for (int i = 0; i < chart_Plot.Series.Count; i++)
                {
                    if (chart_Plot.Series[i].IsVisibleInLegend)
                    {
                        Array.Resize(ref TempName, TempName.GetLength(0) + 1);
                        TempName[TempName.GetUpperBound(0)] = chart_Plot.Series[i].Name;
                        chart_Plot.Series[i].IsVisibleInLegend = false;
                    }
                }
                //chart_Plot.Legends.
                if (listBox_SectSetting.SelectedIndex != -1)
                {
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderColor = Color.Black;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderWidth = 2;
                }
                chart_Plot.SaveImage(PNGStoredFolderPath, ChartImageFormat.Png);
                for (int i = 0; i < TempName.GetLength(0); i++)
                {
                    chart_Plot.Series[TempName[i]].IsVisibleInLegend = false;
                }
                if (listBox_SectSetting.SelectedIndex != -1)
                {
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderColor = Color.Red;
                    chart_Plot.Series[BlockListSubScriptToName[listBox_SectSetting.SelectedIndex]].MarkerBorderWidth = 3;
                }
                wdApplication.Selection.InlineShapes.AddPicture(PNGStoredFolderPath);



                //wdrange = TableRef.Rows[1].Cells[3].Range;
                //wdrange.Select();
                //WORD.Range wdFunctionR = wdApplication.Selection.OMaths
                //    .Add(wdApplication.Selection.Range);
                //WORD.OMathFunction wdFunction = wdApplication.Selection
                //    .OMaths[1].Functions.Add(wdApplication.Selection.Range,
                //    WORD.WdOMathFunctionType.wdOMathFunctionFrac);
                //WORD.OMathFrac Frac = wdFunction.Frac;
                //WORD.OMathNary wdNary = wdFunction.Nary;

                //WORD.OMathFunc= wdFunction.Frac;
                //wdNary.Char = 8721;
                //wdNary.Grow = false;
                //wdNary.SubSupLim = false;
                //wdNary.HideSub = false;
                //wdNary.HideSup = false;



                //wdSelection = wdApplication.Selection;
                //count = 3;
                //wdSelection.MoveLeft(ref unit, count,WORD.WdMovementType.wdMove );
                //wdApplication.Selection.TypeText("BA");
                //wdSelection.TypeText("=2");
                //count =3;
                //wdSelection.MoveLeft(ref unit, ref count);
                //wdSelection.TypeText("b"); 
                //wdSelection.TypeText("Add");
                //MessageBox.Show(wdSelection.Start.ToString());


                // MessageBox.Show()
                //wdSelection.TypeText("=4");
                //wdSelection.MoveLeft(ref unit, ref tcount);
                //wdSelection.TypeText("test");// "\u03B24");
                //wdSelection.MoveUp(ref lu, ref count);
                //wdSelection.TypeText("Test2");//\u03B23");
                //TableRef.Rows[1].Cells[4].Range.Select();

                //wdrange = newDocument.Range(1, 0);
                //wdrange.Select();
                //wdrange.Text = "This is second line\n";

                //Word.Document document = this.Application.ActiveDocument;
                //Word.Range rng = document.Paragraphs[1].Range;

                //// Change the formatting. To change the font size for a right-to-left language, 
                //// such as Arabic or Hebrew, use the Font.SizeBi property instead of Font.Size.
                //rng.Font.Size = 14;
                //rng.Font.Name = "Arial";
                //rng.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;


                newDocument.SaveAs(outputFile);
                newDocument.Close(false, Type.Missing, Type.Missing);
                wdApplication.Quit();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(wdApplication);

                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.StartInfo.FileName = outputFile;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                p.Start();
            }
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            btn_Test_Click(sender, e);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(isExporting)
            {
                return;
            }
        }

        private void Form_RDExamProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isExporting) { e.Cancel=true; }
            //退出.
            if (!(BlockMainArray.GetLength(0) == 0))
            {
                //當有編輯中的專案時(有Block時,才會有警示).
                if (MessageBox.Show("您確定要關閉?按下確定後目前編輯中的專案檔會遺失所有更動", "關閉", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                { e.Cancel = true; }
                else
                {
                    e.Cancel = false;
                }

            }
            else
            {
                e.Cancel = false;
            }
            
            //if (isExporting)
            //{
            //    e.Cancel = true;
            //}
            //else
            //{

            //}
        }


    }
}
