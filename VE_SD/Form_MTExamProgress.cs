using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Windows.Forms.DataVisualization.Charting;
using WORD = Microsoft.Office.Interop.Word;
using System.Diagnostics;
using System.Collections;
using System.Net;
using System.Runtime.InteropServices;//For DLL IMPORT.

namespace VE_SD
{
    public partial class Form_MTExamProgress : Form
    {
        //取得短路徑
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern Int32 GetShortPathName(String path, StringBuilder shortPath, Int32 shortPathLength);
        //

        double OldWidth;
        double OldHeight;

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //基本參數區域
        MTExameTextBox_Object_Class  RCOL = new MTExameTextBox_Object_Class();


        private string _過去使用檔案;
        //private int _過去使用檔案數量 =5;
        //Stack<string> MyData = new Stack<string>();
        string[] 使用檔案紀錄序列=new string[] { };
        object[] OBB = new object[] { };//儲存MenustripItem的成員.

        public Class_BlockSect_MT[] BlockMainArray = new Class_BlockSect_MT[] { };// = new Class_BlockSect();
        int BlockCount = 0; //Block Array size.
        //public Class_BlockSect[] BlockMainArray = new Class_BlockSect[] { };// = new Class_BlockSect();
        //int BlockCount = 0; //Block Array size
        public Class_BlockSect_MT InterfaceBlock;
        private Dictionary<string, int> BlockNameToArraySubscript = new Dictionary<string, int>();//Block Name To Array Subscript.
        public Dictionary<string, int> BlockNameToListSubScript = new Dictionary<string, int>(); //Block Name To List Subscript.
        private Dictionary<int, String> BlockListSubScriptToName = new Dictionary<int, string>();//Block List Subscript to Name
        private Dictionary<int, String> BlockArraySubscriptToName = new Dictionary<int, string>();//Block Array Subscript to Name.


        //材質矩陣.
        private Dictionary<String, int> MaterialNameToArraySubScript = new Dictionary<string, int>();//Material Name to subscript index.
        private Dictionary<int, String> MaterialSubscriptToName = new Dictionary<int, string>();//Material Subscript in array to Name.
        public string[] MaterialArray = new string[] { };
        int MaterialCount = 0;
        private MaterialsRoughness[] MaterialsCoefArray = new MaterialsRoughness[] { };
        private int MaterialRoughnessArrayCount = 0;


        //EL矩陣.
        //Minimum EL.
        double MinEL; //根據波向來判斷.
        double MaxEL; //根據波向來判斷.
        public double[] ELArray = new double[] { };
        int ELSize = 0;

        private bool isExporting = false;
        private bool isCalc = false;

        string selectname = null;  //目前點選到的Block.
        Module2 Mod = null;
        private Form1 mainForm = null;
        private string 打開專案檔的名稱 = null;
        private static bool 使用者手動更新材質與摩擦;

        private static int _AnnotationSize = 7;//CHART的ANNOTATION字體大小.
        private static double _ChartYXRatio=-9999;//Y:X ratio.
        private static bool _調整Chart比例 = false;
        private static double _xf;
        private static double _yf;
        private static double _oldXmin;
        private static double _oldXmax;
        private static double _oldYmin;
        private static double _oldYmax;
        private static double _OldChartYXRatio;
        //RDExameTextBox_ObjectClass RCOL = new RDExameTextBox_Object_Class();
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Struct宣告.
        private struct PointFMY
        {
            public double x;
            public double y;
            public double z;
        }
        private struct MaterialsRoughness
        {
            public int Id1;
            public int Id2;
            public double coef;
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //Property宣告
        public Class_BlockSect_MT BlockObj
        {
            get { return InterfaceBlock; }
            set { InterfaceBlock = value; }
        }
        public int AnnotationSize
        {
            get { return _AnnotationSize; }
            set { _AnnotationSize = value; }
        }
        public double ChartYXRatio
        {
            get { return _ChartYXRatio; }
            set { _ChartYXRatio = value; }
        }
        public bool 調整Chart比例
        {
            get { return _調整Chart比例; }
            set { _調整Chart比例 = value; }
        }


        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Form_MTExamProgress()
        {
            InitializeComponent();
        }

        public Form_MTExamProgress(Form callingForm)
        {
            mainForm = callingForm as Form1;//傳入物件參考.
            InitializeComponent();
        }
        string PNGStoredFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP.PNG";
        string VESDStoredFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TEMP_Project.vesdp";
        #region 檢核
        private void 開始檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_Test_Click(sender, e);
        }
        private void btn_Test_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }
            
            //string 驗證Msg = "";
            if (mainForm.軟體驗證是否通過) //mainForm.檢視目前是否已有合理認證(ref 驗證Msg)) //mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref 驗證Msg))
            {
                //Nothing.
            }
            else
            {
                if (mainForm.提供服務訊息)
                {
                    this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')嘗試進行碼頭檢核但缺乏軟體驗證遭到阻擋,員工編號為'" + mainForm.LoginInUserID + "',員工名稱為'" + mainForm.LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                }
                MessageBox.Show("您無法使用此功能!!錯誤訊息:" + Environment.NewLine + "您沒有正確的軟體驗證" + Environment.NewLine + "請退出檢核程式(您可先存檔)並進行軟體驗證", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            tabControl1.SelectedIndex = 3;
            isCalc = true;

            string CheckTextBoxString = "";
            if (!CheckTextBoxNoEmpty(ref CheckTextBoxString))
            {
                FrmShowMsg FF = new FrmShowMsg(CheckTextBoxString, "您有資料未填完成或填入值不正確");
                FF.Show();
                //btn_OutputExcel.Enabled = false;
                //輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
                //btn_LogOutput.Enabled = false;
                isCalc = false;
                return;
            }


            if (BlockMainArray.GetLength(0)==0)
            {
                MessageBox.Show("沒有任何東西需要被檢核");
                isCalc = false;
                return;
            }

            //計算Block的平均摩擦係數.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                string M1 = BlockMainArray[i].使用材質;
                string[] M2C = BlockMainArray[i].周圍參考材質;
                double sumv = 0;
                for (int j = 0; j < M2C.GetLength(0); j++)
                {
                    string M2 = M2C[j];
                    double getv = -9999;
                    for (int k = 0; k < MaterialsCoefArray.GetLength(0); k++)
                    {
                        string C1 = MaterialSubscriptToName[MaterialsCoefArray[k].Id1];
                        string c2 = MaterialSubscriptToName[MaterialsCoefArray[k].Id2];
                        double v1 = MaterialsCoefArray[k].coef;
                        if (v1 == -9999)
                        {
                            continue;
                        }
                        if (C1 == M1 && c2 == M2)
                        {
                            getv = v1;
                            break;
                        }
                        else if (C1 == M2 && c2 == M1)
                        {
                            getv = v1;
                            break;
                        }
                    }
                    if (getv == -9999)
                    {
                        MessageBox.Show("您有材質間摩擦係數尚未定義於檔案內(只允許檔案內新增)，材質為" + M1 + "," + M2 + Environment.NewLine + "若不清楚如何設定請詢問相關人員","材質間摩擦係數設定缺失",MessageBoxButtons.OK,MessageBoxIcon.Error);
                        //btn_OutputExcel.Enabled = false;
                        //輸出Word檔案ToolStripMenuItem.Enabled = btn_OutputExcel.Enabled;
                        //btn_LogOutput.Enabled = false;
                        isCalc = false;
                        return;
                    }
                    sumv += getv;
                }
                sumv = sumv / (double)M2C.GetLength(0);
                BlockMainArray[i].平均摩擦係數 = sumv;
            }
            //***********************************************************************************************************************//
            //帶入計算
            Mod = new Module2();
            //MessageBox.Show("這是測試");
            Mod.DeleteAllBlockData();

            // 1-1. Block給定.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                //- 迴圈塞入Block.
                // 2016/03/29. 新增是否計算Moment選項.
                int nowid = Mod.NewBlock(BlockMainArray[i].單位體積重量, BlockMainArray[i].地震時單位體積重量, BlockMainArray[i].平均摩擦係數, BlockMainArray[i].是否為混凝土塊);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                int 座標點數 = BlockMainArray[i].座標點數;
                for (int i2 = 0; i2 < 座標點數; i2++)
                {
                    Mod.SetBlockCoord(nowid, getx[i2], gety[i2]);
                }
            }

            // 1-2. Level給定
            //Mod.NewLevel(3.2);
            //Mod.NewLevel(2.7);
            //Mod.NewLevel(1.0);
            double[] ELA = new double[] { };
            ELA = ELArray;
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = MinEL; //double.Parse(textBox_HWL.Text);
            /*
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = MinEL;
            Array.Resize(ref ELA, ELA.GetLength(0) + 1);
            ELA[ELA.GetUpperBound(0)] = MaxEL;
            */
            Array.Sort(ELA,(a,b)=>b.CompareTo(a));
            //- Push Level
            Mod.DeleteAllLevel();
            for (int i = 0; i < ELA.GetLength(0); i++)
            {
                //MessageBox.Show(ELA[i].ToString());
                Mod.NewLevel(ELA[i]);
            }
            /*Mod.NewLevel(-0.6);
            Mod.NewLevel(-2.2);
            Mod.NewLevel(-3.5);
            */

            // 2. 背景參數帶入
            //- 水位設計輸入
            Mod.WaterDesignInput(double.Parse(textBox_設計潮位高.Text), double.Parse(textBox_設計潮位低.Text), double.Parse(textBox_殘留水位.Text));
            //- 力量輸入
            Mod.ForceDesignInput(double.Parse(textBox_平時上載荷重.Text), double.Parse(textBox_地震時上載荷重.Text), double.Parse(textBox_船舶牽引力.Text));
            //- 設計震度參數輸入
            Mod.EarthquakeDesignInput(double.Parse(textBox_陸上設計震度.Text), double.Parse(textBox_水中設計震度.Text));
            //- 背填料參數輸入
            Mod.MaterialDesignInput(double.Parse(textBox_背填料內摩擦角.Text), double.Parse(textBox_背填料壁面摩擦角.Text), double.Parse(textBox_背填料水平傾斜角.Text), double.Parse(textBox_BoatColumnHeight.Text));
            //- 基礎參數輸入
            Mod.BaseDesignInput(double.Parse(textBox_入土深度.Text), double.Parse(textBox_拋石厚度.Text), double.Parse(textBox_地盤基礎內摩擦角.Text), double.Parse(textBox_土壤凝聚力.Text), double.Parse(textBox_SoilR_Earth.Text), double.Parse(textBox_SoilR_Water.Text), double.Parse(textBox_rw.Text));
            //- Bearing Capacity Factor
            Mod.BC_DesignInput(double.Parse(textBox_Nq.Text), double.Parse(textBox_Nr.Text), double.Parse(textBox_Nc.Text));
            //- Safety Factor
            Mod.SF_CoefInput(double.Parse(textBox_平時滑動安全係數.Text), double.Parse(textBox_平時傾倒安全係數.Text), double.Parse(textBox_平時地盤承載力安全係數.Text));
            //- Safety Factor
            Mod.SF_CoefInput_E(double.Parse(textBox_地震時滑動安全係數.Text), double.Parse(textBox_地震時傾倒安全係數.Text), double.Parse(textBox_地震時地盤承載力安全係數.Text));
            //- 土壓係數輸入
            //Mod.KaInput(double.Parse(textBox_KaStage1.Text), double.Parse(textBox_KaStage2.Text), double.Parse(textBox_KaStage3.Text));

            if(rb_Meyerhof.Checked)
            {
                //使用公式1.
                Mod.MeyerhofCheck(true);
            }
            else
            {
                //使用公式2.
                Mod.MeyerhofCheck(false);
            }
            // Go Go Go~
            //MessageBox.Show("Calculating");
            
            Mod.Run();
            isCalc = false;
            textBox_CheckMessageShow.Text = Mod.ErrMsg;
            MessageBox.Show("完成檢核計算!!","完成檢核",MessageBoxButtons.OK,MessageBoxIcon.Information);
            textBox_CheckMessageShow.Select(0, 0);
            btn_LogOutput.Enabled = true;
            btn_OutputWord.Enabled = true;
            輸出LogToolStripMenuItem.Enabled = true;
            輸出Word報表ToolStripMenuItem.Enabled = true;
        }
        Boolean CheckTextBoxNoEmpty(ref string ErrorMsg)
        {
            ErrorMsg = "";
            bool okOrNot = true;//True is Ok[No faults].

            /*
            if (cmb_seawaveDir.SelectedItem.ToString() == "")
            {
                ErrorMsg += ("您深海波波向沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            */

            if (textBox_設計潮位高.Text == "")
            {
                ErrorMsg += ("您設計潮位高沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_設計潮位低.Text == "")
            {
                ErrorMsg += ("您設計潮位低沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_殘留水位.Text == "")
            {
                ErrorMsg += ("您殘留水位沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_平時上載荷重.Text == "")
            {
                ErrorMsg += ("您平時上載荷重沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_地震時上載荷重.Text == "")
            {
                ErrorMsg += ("您地震時上載荷重沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_船舶牽引力.Text == "")
            {
                ErrorMsg += ("您船舶牽引力沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_陸上設計震度.Text == "")
            {
                ErrorMsg += ("您陸上設計震度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_水中設計震度.Text == "")
            {
                ErrorMsg += ("您水中設計震度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_rw.Text == "")
            {
                ErrorMsg += ("您水單位重沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_BoatColumnHeight.Text == "")
            {
                ErrorMsg += ("您繫船柱突出高度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }

            if (textBox_背填料內摩擦角.Text == "")
            {
                ErrorMsg += ("您背填料內摩擦角沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_背填料壁面摩擦角.Text == "")
            {
                ErrorMsg += ("您背填料壁面摩擦角沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_背填料水平傾斜角.Text == "")
            {
                ErrorMsg += ("您背填料水平傾斜角沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_入土深度.Text == "")
            {
                ErrorMsg += ("您入土深度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_拋石厚度.Text == "")
            {
                ErrorMsg += ("您拋石厚度沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_地盤基礎內摩擦角.Text == "")
            {
                ErrorMsg += ("您地盤基礎內摩擦角沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_土壤凝聚力.Text == "")
            {
                ErrorMsg += ("您土壤凝聚力沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            /*
            if (textBox_Nq.Text == "")
            {
                ErrorMsg += ("您Nq沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Nc.Text == "")
            {
                ErrorMsg += ("您Nc沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_Nr.Text == "")
            {
                ErrorMsg += ("您Nr沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            */
            if (textBox_平時滑動安全係數.Text == "")
            {
                ErrorMsg += ("您平時滑動安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_平時傾倒安全係數.Text == "")
            {
                ErrorMsg += ("您平時傾倒安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_平時地盤承載力安全係數.Text == "")
            {
                ErrorMsg += ("您平時地盤承載力安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;
            }
            if (textBox_地震時滑動安全係數.Text == "")
            {
                ErrorMsg += ("您地震時滑動安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_地震時傾倒安全係數.Text == "")
            {
                ErrorMsg += ("您地震時傾倒安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_地震時地盤承載力安全係數.Text == "")
            {
                ErrorMsg += ("您地震時地盤承載力安全係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_SoilR_Earth.Text == "")
            {
                ErrorMsg += ("您土壤重(陸上)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_SoilR_Water.Text == "")
            {
                ErrorMsg += ("您土壤重(水中)沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            /*
            if (textBox_KaStage1.Text == "")
            {
                ErrorMsg += ("您平時設計震度土壓係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_KaStage2.Text == "")
            {
                ErrorMsg += ("您地震時水上土壓係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_KaStage3.Text == "")
            {
                ErrorMsg += ("您地震時水下土壓係數沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }*/
            /*
            if (textBox_平時上載荷重.Text == "")
            {
                ErrorMsg += ("您平時上載荷重沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            if (textBox_平時上載荷重.Text == "")
            {
                ErrorMsg += ("您平時上載荷重沒有選擇!!!" + Environment.NewLine);
                okOrNot = false;
                //MessageBox.Show("您深海波波向沒有選擇!!!", "檢核檢查", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //return false;

            }
            */
            return okOrNot;

        }
        #endregion
        #region 開啟
        static int _tempic = 0;
        static StreamReader _打開過去舊檔案R = null;
        private void 開啟過去舊檔案()
        {
            OldHeight = this.Height;
            OldWidth = this.Width;


            舊檔案1ToolStripMenuItem.Visible = false;
            舊檔案2ToolStripMenuItem.Visible = false;
            舊檔案3ToolStripMenuItem.Visible = false;
            舊檔案4ToolStripMenuItem.Visible = false;
            舊檔案5ToolStripMenuItem.Visible = false;

            舊檔案1ToolStripMenuItem.AutoToolTip = true;
            舊檔案2ToolStripMenuItem.AutoToolTip = true;
            舊檔案3ToolStripMenuItem.AutoToolTip = true;
            舊檔案4ToolStripMenuItem.AutoToolTip = true;
            舊檔案5ToolStripMenuItem.AutoToolTip = true;

            //若有檔案，則打開之.
            if (File.Exists(_過去使用檔案))
            {
                _tempic = 0;
                
                try
                {
                    _打開過去舊檔案R = new StreamReader(_過去使用檔案);
                    while(_打開過去舊檔案R.Peek()>=0)
                    {
                        string temps = _打開過去舊檔案R.ReadLine();
                        Array.Resize(ref 使用檔案紀錄序列, _tempic + 1);
                        使用檔案紀錄序列[_tempic] = temps;
                        //MyData.Push(temps);
                        _tempic += 1;
                        if(_tempic==5)
                        {
                            break;
                        }
                    }
                    _打開過去舊檔案R.Close();
                    _打開過去舊檔案R = null;

                }
                catch
                {
                    //_打開過去舊檔案R.Close();
                    _打開過去舊檔案R = null;
                }

                if(_tempic==0)
                {
                    return;
                }

                
                //將資料裝載.
                //for(int i=_tempic;i>=0;i--)
                //{
                //   MyData.Push(使用檔案紀錄序列[i]);//檔案內會是最早的在最前面,為配合Stack的Push機制(PUSH到最前面)，因此顛倒.
                //}
                
                //MyData.CopyTo(tempssinner,0);

                for(int i=0;i<_tempic;i++)
                {
                    ToolStripMenuItem tsi = (ToolStripMenuItem)OBB[i];
                    if (i < 使用檔案紀錄序列.GetLength(0))
                    {
                        tsi.Tag = 使用檔案紀錄序列[i];
                        tsi.Text = 取得縮寫(使用檔案紀錄序列[i]);
                        tsi.ToolTipText = 使用檔案紀錄序列[i];
                        tsi.Visible = true;
                    }
                    else
                    {
                        tsi.Tag = null;
                        tsi.Text =null;
                        tsi.ToolTipText = null;
                        tsi.Visible = false;
                    }
                }
            }
        }
        private void Adjust(Control ParentCtrl)
        {
            foreach(Control c in ParentCtrl.Controls)
            {
                c.Anchor = (AnchorStyles.Top | AnchorStyles.Left); //(AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top );
                Adjust(c);
            }
        }
        private void Form_MTExamProgress_Load(object sender, EventArgs e)
        {
            開啟與ReLoad();
        }
        public void 開啟與ReLoad()
        {
            //Adjust(this);
            /*
            if(mainForm.軟體開啟時的視窗大小=="最大")
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
            */
            //this.WindowState = FormWindowState.Maximized;
            //決定過去開啟的舊檔案.
            Array.Resize(ref OBB, 5);
            OBB[0] = 舊檔案1ToolStripMenuItem;
            OBB[1] = 舊檔案2ToolStripMenuItem;
            OBB[2] = 舊檔案3ToolStripMenuItem;
            OBB[3] = 舊檔案4ToolStripMenuItem;
            OBB[4] = 舊檔案5ToolStripMenuItem;

            Array.Resize(ref 使用檔案紀錄序列, 0);//, _過去使用檔案數量);//初始化.

            _過去使用檔案 = mainForm.SystemReferenceStoreFolder + "\\VESD_P2_oldfile.txt";
            開啟過去舊檔案();


            
            tsp_cond.Text = "請設定或編輯您的專案檔";
            TSP_DATETIME.Text = "";
            TSP_DATETIME.Alignment = ToolStripItemAlignment.Right;
            TSP_DATETIME.RightToLeft = RightToLeft.No;
            tsp_progressbar.Visible = false;
            //tsp_cond.Text = "OK";

            isExporting = false;
            isCalc = false;
            this.Text = "專案檔:未命名";

            if (mainForm.碼頭檢核開啟時預設數字)
            {
                //cmb_seawaveDir.SelectedItem = "右";
                textBox_設計潮位高.Text = "1.5";
                textBox_設計潮位低.Text = "0.07";
                textBox_殘留水位.Text = "0.55";
                textBox_平時上載荷重.Text = "1";
                textBox_地震時上載荷重.Text = "0.5";
                textBox_船舶牽引力.Text = "2";
                textBox_陸上設計震度.Text = "0.165";
                textBox_水中設計震度.Text = "0.33";
                textBox_背填料內摩擦角.Text = "40";
                textBox_背填料壁面摩擦角.Text = "15";
                textBox_背填料水平傾斜角.Text = "0";
                textBox_入土深度.Text = "0.5";
                textBox_拋石厚度.Text = "1.5";
                textBox_地盤基礎內摩擦角.Text = "31.5";
                textBox_土壤凝聚力.Text = "0";
                textBox_Nq.Text = "21.86";
                textBox_Nc.Text = "34.04";
                textBox_Nr.Text = "20.22";
                textBox_平時滑動安全係數.Text = "1.2";
                textBox_平時傾倒安全係數.Text = "1.2";
                textBox_平時地盤承載力安全係數.Text = "2.5";
                textBox_地震時滑動安全係數.Text = "1";
                textBox_地震時傾倒安全係數.Text = "1.1";
                textBox_地震時地盤承載力安全係數.Text = "1.5";
                textBox_rw.Text = "1.03";
                textBox_BoatColumnHeight.Text = "0.25";
                textBox_SoilR_Earth.Text = "1.8";
                textBox_SoilR_Water.Text = "1.0";

                /*
                textBox_KaStage1.Text = "0.201";
                textBox_KaStage2.Text = "0.293";
                textBox_KaStage3.Text = "0.423";
                */
            }

            EscapeDGMaterialCellValueChangedFunction = true;
            DGMaterial.Rows.Clear();
            EscapeDGMaterialRoughnessCellContentChanged = true;
            DGMaterialRough.Rows.Clear();
            Array.Resize(ref MaterialArray, 0); MaterialCount = 0;
            Array.Resize(ref MaterialsCoefArray, 0); MaterialRoughnessArrayCount = 0;
            MaterialNameToArraySubScript.Clear();
            MaterialSubscriptToName.Clear();
            if (mainForm.碼頭檢核開啟時預設數字)
            {
                讀入摩擦係數初始設定();
            }
            使用者手動更新材質與摩擦 = true;


            chart_Plot.Series.Clear();
            chart_Plot.Annotations.Clear();
            cmb_ShowOnBlockListChoice.SelectedItem = "單位體積重";
            ELDGV1.Rows.Clear();
            ELDGV1.Enabled = false; //初始設定變更為不可操控,要等到有Block時才開始能填入.
            chart_Plot.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart_Plot.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            listBox_SectSetting.Items.Clear();
            Array.Resize(ref BlockMainArray, 0);
            BlockCount = 0;
            Array.Resize(ref ELArray, 0);
            調整Chart比例 = false;
            _ChartYXRatio = -9999;
            _xf = -9999;
            _yf = -9999;
            _oldXmax = -9999;
            _oldXmin = -9999;
            _oldYmin = -9999;
            _oldYmax = -9999;
            //顯示設定ToolStripMenuItem.Visible = false;
            blockLabels設定ToolStripMenuItem.Enabled = false;
            
            ELSize = 0;
            InterfaceBlock = null;
            打開專案檔的名稱 = null;
            selectname = null;
            propertyGrid_Block.SelectedObject = null;
            ReferencedMaterialCHKL.Items.Clear();
            btn_ModifiedBlock.Enabled = false;
            btnRemoveSects.Enabled = false;
            BlockArraySubscriptToName.Clear();
            BlockListSubScriptToName.Clear();
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();


            
            textBox_CheckMessageShow.Text = "";
            //rb_Meyerhof.Checked = true;
            rb_Tergazhi.Checked = true;
            開始檢核ToolStripMenuItem.Enabled = false;
            btn_Test.Enabled = false;
            btn_LogOutput.Enabled = false;
            btn_OutputWord.Enabled = false;
            輸出LogToolStripMenuItem.Enabled = false;
            輸出Word報表ToolStripMenuItem.Enabled = false;
            //data_BlockTempShow.Rows.Clear();

        }
        private void 讀入摩擦係數初始設定()
        {
            //之後可以更改為讀取檔案之方式,但目前不開發此種操作.
            EscapeDGMaterialCellValueChangedFunction = true;
            DGMaterial.Rows.Clear();
            EscapeDGMaterialRoughnessCellContentChanged = true;
            DGMaterialRough.Rows.Clear();
            Array.Resize(ref MaterialArray, 2); MaterialCount = 2;
            MaterialArray[0] = "混凝土方塊";
            MaterialArray[1] = "混凝土拋石";
            //MaterialArray[2] = "場鑄混凝土";
            Array.Resize(ref MaterialsCoefArray, 2); MaterialRoughnessArrayCount = 2;
            MaterialsCoefArray[0].coef = 0.5;//混凝土方塊與方塊.
            MaterialsCoefArray[0].Id1 = 0;
            MaterialsCoefArray[0].Id2 = 0;
            MaterialsCoefArray[1].coef = 0.6;//混凝土方塊與拋石.
            MaterialsCoefArray[1].Id1 = 0;
            MaterialsCoefArray[1].Id2 = 1;
            /*
            MaterialsCoefArray[2].coef = 0.7;//場注混凝土與拋石.
            MaterialsCoefArray[2].Id1 = 2;
            MaterialsCoefArray[2].Id2 = 1;
            MaterialsCoefArray[3].coef = 0.8;//拋石與拋石.
            MaterialsCoefArray[3].Id1 = 1;
            MaterialsCoefArray[3].Id2 = 1;
            */
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
        #region 參數輸入
        private bool JudgeTheTextBoxHandle(TextBox tt, KeyPressEventArgs ei)
        {
            char key_char = ei.KeyChar;
            string nowtext = tt.Text;
            //tsp_cond.Text = textBox_H0.SelectionStart.ToString();
            if ((int)key_char == 46 && tt.SelectionStart == 0) //textBox_H0.SelectionStart == 0)
            {
                //.
                //e.Handled = true;
                return true;
            }
            else if ((int)key_char == 45 && tt.SelectionStart != 0) //textBox_H0.SelectionStart != 0)
            {
                //-
                //e.Handled = true;
                return true;
            }
            else if ((int)key_char == 43 && tt.SelectionStart != 0) //textBox_H0.SelectionStart != 0)
            {
                //+
                //e.Handled = true;
                return true;
            }
            else if ((int)key_char == 46 && nowtext.IndexOf(".") != -1)
            {
                return true;
            }
            else if ((int)key_char == 45 && nowtext.IndexOf("-") != -1)
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
        private void textBox_設計潮位高_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_設計潮位低_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_殘留水位_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_平時上載荷重_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_地震時上載荷重_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_船舶牽引力_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_陸上設計震度_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_水中設計震度_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_背填料內摩擦角_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_背填料壁面摩擦繳_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_背填料水平傾斜角_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_入土深度_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_拋石厚度_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_地盤基礎內摩擦角_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_土壤凝聚力_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_Nq_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox_Nq_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_Nc_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_Nr_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_平時滑動安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_平時傾倒安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_平時地盤承載力安全係數_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void textBox_平時地盤承載力安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_地震時滑動安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_地震時傾倒安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_地震時地盤承載力安全係數_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }
        private void textBox_rw_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_BoatColumnHeight_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_SoilR_Earth_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_SoilR_Water_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_KaStage1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_KaStage2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void textBox_KaStage3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = JudgeTheTextBoxHandle((TextBox)sender, e);
        }

        private void btn_ShowNcNqNr_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }

            string picFolder = mainForm.程式運作路徑 + "\\PIC\\NcNqNr.png";
            //if(!IsFileLocked(new FileInfo(picFolder)))
            //{
            //Call function to open it.
            try
            {
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.StartInfo.FileName = picFolder;// 浩海工程顧問-工程輔助軟體說明手冊.pdf";// EPA SWMM User Manual Version 5.1.pdf";//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                p.Start();
            }
            catch
            {
                MessageBox.Show("無法成功打開Nc、Nq與Nr的參考檔案", "NcNqNr參考檔案", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion
        #region 專案控制區
        static XmlNode RNode;
        static XmlElement Relement;
        static string 打開檔案之訊息 = null;
        private void 填入一個新的檔案(string 填入新的檔案)
        {
            
            if (使用檔案紀錄序列.GetLength(0)>0 && 使用檔案紀錄序列[0] == 填入新的檔案)
            {
                //不做任何事情.
            }
            else
            {
                string[] tempsinner2 = new string[] { };
                int ic = 0;
                Array.Resize(ref tempsinner2, 1);
                tempsinner2[0] = 填入新的檔案;

                for (int i = 0; i <= 使用檔案紀錄序列.GetUpperBound(0); i++) //.Count;i++)
                {

                    if (使用檔案紀錄序列[i] == 填入新的檔案)
                    {
                        //ic += 1;
                        continue;
                    }
                    Array.Resize(ref tempsinner2, ic + 2);
                    tempsinner2[ic + 1] = 使用檔案紀錄序列[i];
                    ic += 1;
                    if (ic == 4)
                    {
                        break;
                    }
                }

                使用檔案紀錄序列 = tempsinner2;
                //MessageBox.Show(使用檔案紀錄序列.GetLength(0).ToString());
                
                //更新Menustrip Item上顯示的名稱.
                for(int i=0;i<OBB.GetLength(0);i++)
                {
                    //MessageBox.Show(i.ToString());
                    ToolStripMenuItem tsi = (ToolStripMenuItem)OBB[i];
                    if (i < 使用檔案紀錄序列.GetLength(0))
                    {
                        tsi.Tag = 使用檔案紀錄序列[i];
                        tsi.Text = 取得縮寫(使用檔案紀錄序列[i]);
                        tsi.ToolTipText = 使用檔案紀錄序列[i];
                        tsi.Visible = true;
                    }
                    else
                    {
                        tsi.Tag = null;
                        tsi.Text = null;
                        tsi.Visible = false;
                        tsi.ToolTipText = null;
                    }
                    
                }
                //儲存檔案.
                StreamWriter swi1 = new StreamWriter(_過去使用檔案);
                for(int i=0;i<使用檔案紀錄序列.GetLength(0);i++)
                {
                    swi1.WriteLine(使用檔案紀錄序列[i]);
                }
                swi1.Flush();
                swi1.Close();

            }
        }
        private void 刪除一個不存在的檔案(int 位置)
        {
            if(位置>使用檔案紀錄序列.GetUpperBound(0))
            {
                return;
            }

            string[] tempsinner2 = new string[] { };
            int ic = 0;
            for(int i=0;i<使用檔案紀錄序列.GetLength(0);i++)
            {
                if(i!=位置)
                {
                    Array.Resize(ref tempsinner2, ic + 1);
                    tempsinner2[ic] = 使用檔案紀錄序列[i];
                    ic += 1;
                }
            }
            使用檔案紀錄序列 = tempsinner2;
            //更新Menustrip Item上顯示的名稱.
            for (int i = 0; i < OBB.GetLength(0); i++)
            {
                ToolStripMenuItem tsi = (ToolStripMenuItem)OBB[i];
                //MessageBox.Show(i.ToString());
                if (i < 使用檔案紀錄序列.GetLength(0))
                {
                    tsi.Tag = 使用檔案紀錄序列[i];
                    tsi.Text = 取得縮寫(使用檔案紀錄序列[i]);
                    tsi.ToolTipText = 使用檔案紀錄序列[i];
                    tsi.Visible = true;
                }
                else
                {
                    tsi.Tag = null;
                    tsi.Text = null;
                    tsi.Visible = false;
                    tsi.ToolTipText = null;
                }

            }
            //儲存檔案.
            StreamWriter swi1 = new StreamWriter(_過去使用檔案);
            for (int i = 0; i < 使用檔案紀錄序列.GetLength(0); i++)
            {
                swi1.WriteLine(使用檔案紀錄序列[i]);
            }
            swi1.Flush();
            swi1.Close();
        }
        private string 取得縮寫(string 原本路徑)
        {
            //http://stackoverflow.com/questions/8403086/long-path-with-ellipsis-in-the-middle

            const int MAX_WIDTH = 50;

            // Specify long file name
            //string fileName = @"A:\LongPath\CanBe\AnyPathYou\SpecifyHere.txt";

            // Find last '\' character
            
            int i = 原本路徑.LastIndexOf('\\');

            string tokenRight = 原本路徑.Substring(i, 原本路徑.Length - i);
            
            string tokenCenter = @"\...";
            int CaptLength;
            CaptLength= MAX_WIDTH - (tokenRight.Length + tokenCenter.Length);
            if(CaptLength<=10)
            {
                CaptLength = 10;
            }
            string tokenLeft = 原本路徑.Substring(0, CaptLength);// MAX_WIDTH - (tokenRight.Length + tokenCenter.Length));

            string shortFileName = tokenLeft + tokenCenter + tokenRight;
            return shortFileName;
        }
        private void 開啟一個新的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            {
                return;
            }

            if (!(BlockMainArray.GetLength(0) == 0))
            {
                //當有編輯中的專案時(有Block時,才會有警示).
                if (MessageBox.Show("您確定要開啟新的專案檔?按下確定後目前編輯中的專案檔會遺失所有更動" + Environment.NewLine + "若確定,請按[確定]" + Environment.NewLine  + "若否,請按取消", "開新的專案檔", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                { return; }

            }

            Form_MTExamProgress_Load(sender, e);
        }
        private void 開啟舊的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            {
                return;
            }



            if (OFD_專案.ShowDialog() == DialogResult.OK)
            {
                打開檔案之訊息 = null;
                
                打開檔案之訊息 = 打開XML專案檔(OFD_專案.FileName);
                if(打開檔案之訊息=="")
                {
                    打開專案檔的名稱 = OFD_專案.FileName;
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                    //MessageBox.Show("H2");
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }

        }
        private void 儲存此專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            {
                return;
            }
            if (BlockMainArray.GetLength(0) == 0)
            { MessageBox.Show("您沒有設定任何形塊!無法儲存", "專案檔管理", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string xmlpath;// = workfoldernow + "\\Test.xml";
            if (object.Equals(打開專案檔的名稱, null))
            {
                if (SFD_專案.ShowDialog() == DialogResult.OK && SFD_專案.FileName != "")
                {
                    xmlpath = SFD_專案.FileName;
                    打開專案檔的名稱 = xmlpath;
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
            TSP_DATETIME.Text = "最新一次編輯時間:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            儲存XML專案檔(xmlpath);
            填入一個新的檔案(打開專案檔的名稱);
        }
        private void 另存專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            {
                return;
            }

            if (BlockMainArray.GetLength(0) == 0)
            { MessageBox.Show("您沒有設定任何形塊!無法儲存", "專案檔管理", MessageBoxButtons.OK, MessageBoxIcon.Error); return; }
            string xmlpath;// = workfoldernow + "\\Test.xml";
            if (SFD_專案.ShowDialog() == DialogResult.OK && SFD_專案.FileName != "")
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

            TSP_DATETIME.Text = "最新一次編輯時間:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm");
            儲存XML專案檔(xmlpath);
            填入一個新的檔案(xmlpath);
            this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(xmlpath);

        }
        private void 退出此檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*if (isExporting || isCalc)
            {
                return;
            }

            if (MessageBox.Show("確定關閉?", "關閉檢核", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                this.Close();
            }
            */
        }

        //舊檔案打開按鈕程序.tsp_
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        #region 打開舊檔案
        private void 舊檔案1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("HH" + 舊檔案1ToolStripMenuItem.Tag.ToString());
            if(isExporting || isCalc)
            {
                return;
            }

            if(!File.Exists(舊檔案1ToolStripMenuItem.Tag.ToString()))
            {
                //若檔案不存在,則無法點開.
                MessageBox.Show("此檔案已經不存在,無法開啟", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //刪除此項目.
                刪除一個不存在的檔案(0);
            }
            else
            {
                //MessageBox.Show("HH");
                打開檔案之訊息 = 打開XML專案檔(舊檔案1ToolStripMenuItem.Tag.ToString());
                if (打開檔案之訊息 == "")
                {
                    打開專案檔的名稱 = 舊檔案1ToolStripMenuItem.Tag.ToString();
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }
        private void 舊檔案2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("HH" + 舊檔案2ToolStripMenuItem.Tag.ToString());
            if (isExporting || isCalc)
            {
                return;
            }

            if (!File.Exists(舊檔案2ToolStripMenuItem.Tag.ToString()))
            {
                //若檔案不存在,則無法點開.
                MessageBox.Show("此檔案已經不存在,無法開啟", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //刪除此項目.
                刪除一個不存在的檔案(1);
            }
            else
            {
                打開檔案之訊息 = 打開XML專案檔(舊檔案2ToolStripMenuItem.Tag.ToString());
                if (打開檔案之訊息 == "")
                {
                    打開專案檔的名稱 = 舊檔案2ToolStripMenuItem.Tag.ToString();
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }
        private void 舊檔案3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("HH" + 舊檔案3ToolStripMenuItem.Tag.ToString());

            if (isExporting || isCalc)
            {
                return;
            }

            if (!File.Exists(舊檔案3ToolStripMenuItem.Tag.ToString()))
            {
                //若檔案不存在,則無法點開.
                MessageBox.Show("此檔案已經不存在,無法開啟", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //刪除此項目.
                刪除一個不存在的檔案(2);
            }
            else
            {
                打開檔案之訊息 = 打開XML專案檔(舊檔案3ToolStripMenuItem.Tag.ToString());
                if (打開檔案之訊息 == "")
                {
                    打開專案檔的名稱 = 舊檔案3ToolStripMenuItem.Tag.ToString();
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }
        private void 舊檔案4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("HH" + 舊檔案4ToolStripMenuItem.Tag.ToString());
            if (isExporting || isCalc)
            {
                return;
            }


            if (!File.Exists(舊檔案4ToolStripMenuItem.Tag.ToString()))
            {
                //若檔案不存在,則無法點開.
                MessageBox.Show("此檔案已經不存在,無法開啟", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //刪除此項目.
                刪除一個不存在的檔案(3);
            }
            else
            {
                打開檔案之訊息 = 打開XML專案檔(舊檔案4ToolStripMenuItem.Tag.ToString());
                if (打開檔案之訊息 == "")
                {
                    打開專案檔的名稱 = 舊檔案4ToolStripMenuItem.Tag.ToString();
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }
        private void 舊檔案5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("HH" + 舊檔案5ToolStripMenuItem.Tag.ToString());
            if (isExporting || isCalc)
            {
                return;
            }

            if (!File.Exists(舊檔案5ToolStripMenuItem.Tag.ToString()))
            {
                //若檔案不存在,則無法點開.
                MessageBox.Show("此檔案已經不存在,無法開啟", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);

                //刪除此項目.
                刪除一個不存在的檔案(4);
            }
            else
            {
                打開檔案之訊息 = 打開XML專案檔(舊檔案5ToolStripMenuItem.Tag.ToString());
                if (打開檔案之訊息 == "")
                {
                    打開專案檔的名稱 = 舊檔案5ToolStripMenuItem.Tag.ToString();
                    this.Text = "專案檔:" + Path.GetFileNameWithoutExtension(打開專案檔的名稱);
                    MessageBox.Show("開啟專案檔成功!", "專案檔載入", MessageBoxButtons.OK, MessageBoxIcon.Information);//開啟成功並不會更動目前檢視的Tab.

                    //將最新的資訊填入.
                    _調整Chart比例 = false;
                    填入一個新的檔案(打開專案檔的名稱);
                    取得YX比例(null);
                }
                else
                {
                    MessageBox.Show("開啟失敗!錯誤訊息:" + 打開檔案之訊息, "打開專案檔", MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
            }
        }
        #endregion
        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //子程序.
        private string 打開XML專案檔(string path)
        {

            Class_BlockSect_MT[] BlockMainArrayR = new Class_BlockSect_MT[] { };
            int blockSizer = 0;
            int selectedBlockIndex = -1;
            double[] ELArrayR = new double[] { };
            int ELSizer = 0;
            int SelectedTab = -1;
            string 最後編輯時間R;
            double 設計潮位高R;
            double 設計潮位低R;
            double 殘留水位R;
            double 平時上載荷重R;
            double 地震時上載荷重R;
            double 船舶牽引力R;
            double 陸上設計震度R;
            double 水中設計震度R;
            double 水單位重R;
            double 繫船柱突出高度R;
            double 背填料內摩擦角R;
            double 背填料壁面摩擦角R;
            double 背填料水平傾斜角R;
            double 入土深度R;
            double 拋石厚度R;
            //string dirr;
            double 地盤內基礎內摩擦角R;
            double 土壤凝聚力R;
            
            double NcR;
            double NqR;
            double NrR;
            
            double 平時滑動安全係數R;
            double 平時傾倒安全係數R;
            double 平時地盤承載力安全係數R;
            double 地震時滑動安全係數R;
            double 地震時傾倒安全係數R;
            double 地震時地盤承載力安全係數R;
            double 土壤重陸上R;
            double 土壤重水中R;
            /*
            double 平時設計震度土壓係數R;
            double 地震時水上土壓係數R;
            double 地震時水下土壓係數R;
            */

            string BlockToolTip資訊選擇;


            Dictionary<string, int> DDR = new Dictionary<string, int>();
            string[] MaterialR = new string[] { };
            int MaterialCountR = 0;
            MaterialsRoughness[] MaterialsCoefArrayR = new MaterialsRoughness[] { };
            int MaterialsCoefCountR = 0;


            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //bool 成功與否 = false;
            try
            {
                RNode = doc.SelectSingleNode("Root/版本");
                if (object.Equals(RNode, null))
                {
                    return "版本控制標籤讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (Relement.GetAttribute("Value").ToString() != "SeaDikeVS_P2")
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
                if (!int.TryParse(Relement.GetAttribute("Value").ToString(), out SelectedTab))
                {
                    return "選擇Tab值錯誤";
                }
                RNode = doc.SelectSingleNode("Root/GlobalParameters/BlockTooltip資訊");
                if (object.Equals(RNode, null))
                {
                    return "Block Tooltip資訊讀取失敗";
                }
                Relement = (XmlElement)RNode;
                BlockToolTip資訊選擇 = Relement.GetAttribute("Value").ToString();
                if (BlockToolTip資訊選擇 != "無" && BlockToolTip資訊選擇 != "單位體積重" && BlockToolTip資訊選擇 != "Moment計算" && BlockToolTip資訊選擇 != "材質")
                {
                    return "Block Tooltip資訊讀取失敗";
                }

                RNode = doc.SelectSingleNode("Root/GlobalParameters/最後編輯時間");
                if (object.Equals(RNode,null))
                {
                    return "最後編輯時間讀取失敗";
                }
                Relement = (XmlElement)RNode;
                最後編輯時間R = Relement.GetAttribute("Value");

                /*
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
                    return "海側方向讀取失敗";
                }
                */

                //設計潮位高.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/設計潮位高");
                if (object.Equals(RNode, null))
                {
                    return "設計潮位高讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"),out 設計潮位高R))
                {
                    return "設計潮位高讀取失敗";
                }

                //設計潮位低.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/設計潮位低");
                if (object.Equals(RNode, null))
                {
                    return "設計潮位低讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 設計潮位低R))
                {
                    return "設計潮位低讀取失敗";
                }

                //殘留水位.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/殘留水位");
                if (object.Equals(RNode, null))
                {
                    return "殘留水位讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 殘留水位R))
                {
                    return "殘留水位讀取失敗";
                }

                //平時上載荷重.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/平時上載荷重");
                if (object.Equals(RNode, null))
                {
                    return "平時上載荷重讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 平時上載荷重R))
                {
                    return "平時上載荷重讀取失敗";
                }

                //地震時上載荷重.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時上載荷重");
                if (object.Equals(RNode, null))
                {
                    return "地震時上載荷重讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時上載荷重R))
                {
                    return "地震時上載荷重讀取失敗";
                }

                //船舶牽引力.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/船舶牽引力");
                if (object.Equals(RNode, null))
                {
                    return "船舶牽引力讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 船舶牽引力R))
                {
                    return "船舶牽引力讀取失敗";
                }

                //陸上設計震度
                RNode = doc.SelectSingleNode("Root/GlobalParameters/陸上設計震度");
                if (object.Equals(RNode, null))
                {
                    return "陸上設計震度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 陸上設計震度R))
                {
                    return "陸上設計震度讀取失敗";
                }

                //水中設計震度.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/水中設計震度");
                if (object.Equals(RNode, null))
                {
                    return "水中設計震度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 水中設計震度R))
                {
                    return "水中設計震度讀取失敗";
                }

                //水單位重.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/水單位重");
                if (object.Equals(RNode, null))
                {
                    return "水單位重讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 水單位重R))
                {
                    return "水單位重讀取失敗";
                }

                //繫船柱突出高度.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/繫船柱突出高度");
                if (object.Equals(RNode, null))
                {
                    return "繫船柱突出高度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 繫船柱突出高度R))
                {
                    return "繫船柱突出高度讀取失敗";
                }

                //背填料內摩擦角.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/背填料內摩擦角");
                if (object.Equals(RNode, null))
                {
                    return "背填料內摩擦角讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 背填料內摩擦角R))
                {
                    return "背填料內摩擦角讀取失敗";
                }

                //背填料壁面摩擦角.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/背填料壁面摩擦角");
                if (object.Equals(RNode, null))
                {
                    return "背填料壁面摩擦角讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 背填料壁面摩擦角R))
                {
                    return "背填料壁面摩擦角讀取失敗";
                }

                //背填料水平傾斜角.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/背填料水平傾斜角");
                if (object.Equals(RNode, null))
                {
                    return "背填料水平傾斜角讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 背填料水平傾斜角R))
                {
                    return "背填料水平傾斜角讀取失敗";
                }

                //入土深度.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/入土深度");
                if (object.Equals(RNode, null))
                {
                    return "入土深度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 入土深度R))
                {
                    return "入土深度讀取失敗";
                }

                //拋石厚度
                RNode = doc.SelectSingleNode("Root/GlobalParameters/拋石厚度");
                if (object.Equals(RNode, null))
                {
                    return "拋石厚度讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 拋石厚度R))
                {
                    return "拋石厚度讀取失敗";
                }

                //地盤內基礎內摩擦角.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地盤內基礎內摩擦角");
                if (object.Equals(RNode, null))
                {
                    return "地盤內基礎內摩擦角讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地盤內基礎內摩擦角R))
                {
                    return "地盤內基礎內摩擦角讀取失敗";
                }

                //土壤凝聚力.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/土壤凝聚力");
                if (object.Equals(RNode, null))
                {
                    return "土壤凝聚力讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 土壤凝聚力R))
                {
                    return "土壤凝聚力讀取失敗";
                }

                
                //Nq
                RNode = doc.SelectSingleNode("Root/GlobalParameters/Nq");
                //MessageBox.Show(object.Equals(RNode,null).ToString());
                if (object.Equals(RNode, null))
                {
                    return "Nq讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out NqR))
                {
                    return "Nq轉換失敗";
                }

                //Nc
                RNode = doc.SelectSingleNode("Root/GlobalParameters/Nc");
                if (object.Equals(RNode, null))
                {
                    return "Nc讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out NcR))
                {
                    return "Nc轉換失敗";
                }

                //Nr
                RNode = doc.SelectSingleNode("Root/GlobalParameters/Nr");
                if (object.Equals(RNode, null))
                {
                    return "Nr讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out NrR))
                {
                    return "Nr轉換失敗";
                }
                

                //平時滑動安全係數
                RNode = doc.SelectSingleNode("Root/GlobalParameters/平時滑動安全係數");
                if (object.Equals(RNode, null))
                {
                    return "平時滑動安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 平時滑動安全係數R))
                {
                    return "平時滑動安全係數讀取失敗";
                }

                //平時傾倒安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/平時傾倒安全係數");
                if (object.Equals(RNode, null))
                {
                    return "平時傾倒安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 平時傾倒安全係數R))
                {
                    return "平時傾倒安全係數讀取失敗";
                }

                //平時地盤承載力安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/平時地盤承載力安全係數");
                if (object.Equals(RNode, null))
                {
                    return "平時地盤承載力安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 平時地盤承載力安全係數R))
                {
                    return "平時地盤承載力安全係數讀取失敗";
                }

                //地震時滑動安全係數
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時滑動安全係數");
                if (object.Equals(RNode, null))
                {
                    return "地震時滑動安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時滑動安全係數R))
                {
                    return "地震時滑動安全係數讀取失敗";
                }

                //地震時傾倒安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時傾倒安全係數");
                if (object.Equals(RNode, null))
                {
                    return "地震時傾倒安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時傾倒安全係數R))
                {
                    return "地震時傾倒安全係數讀取失敗";
                }

                //地震時地盤承載力安全係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時地盤承載力安全係數");
                if (object.Equals(RNode, null))
                {
                    return "地震時地盤承載力安全係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時地盤承載力安全係數R))
                {
                    return "地震時地盤承載力安全係數讀取失敗";
                }

                //土壤重陸上.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/土壤重陸上");
                if (object.Equals(RNode, null))
                {
                    return "土壤重陸上讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 土壤重陸上R))
                {
                    return "土壤重陸上讀取失敗";
                }

                //土壤重水中.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/土壤重水中");
                if (object.Equals(RNode, null))
                {
                    return "土壤重水中讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 土壤重水中R))
                {
                    return "土壤重水中讀取失敗";
                }

                /*
                //平時設計震度土壓係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/平時設計震度土壓係數");
                if (object.Equals(RNode, null))
                {
                    return "平時設計震度土壓係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 平時設計震度土壓係數R))
                {
                    return "平時設計震度土壓係數讀取失敗";
                }

                //地震時K017設計震度土壓係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時水上土壓係數");
                if (object.Equals(RNode, null))
                {
                    return "地震時水上土壓係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時水上土壓係數R))
                {
                    return "地震時水上土壓係數讀取失敗";
                }

                //地震時K033設計震度土壓係數.
                RNode = doc.SelectSingleNode("Root/GlobalParameters/地震時水下土壓係數");
                if (object.Equals(RNode, null))
                {
                    return "地震時水下土壓係數讀取失敗";
                }
                Relement = (XmlElement)RNode;
                if (!double.TryParse(Relement.GetAttribute("Value"), out 地震時水下土壓係數R))
                {
                    return "地震時水下土壓係數讀取失敗";
                }
                */


                //EL讀取.
                XmlNodeList ELCollection = doc.SelectNodes("Root/GlobalParameters/EL點數/EL");
                foreach (XmlNode ELNode in ELCollection)
                {
                    double fi;
                    Relement = (XmlElement)ELNode;
                    if (!double.TryParse(Relement.GetAttribute("Value").ToString(), out fi))
                    {
                        return "EL讀取失敗!!!";
                    }
                    Array.Resize(ref ELArrayR, ELSizer + 1);
                    ELArrayR[ELSizer] = fi;
                    ELSizer += 1;
                }
                //使用材質設定.
                XmlNodeList 使用材質Collection = doc.SelectNodes("Root/GlobalParameters/使用材質/UseMaterial");
                foreach (XmlNode 使用材質Node in 使用材質Collection)
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
                for (int i = 0; i < MaterialR.GetLength(0); i++)
                {
                    DDR.Add(MaterialR[i], i);
                }
                //摩擦係數組合設定.
                XmlNodeList 摩擦係數Collection = doc.SelectNodes("Root/GlobalParameters/摩擦係數/MaterialsFriction");
                foreach (XmlNode 摩擦係數Node in 摩擦係數Collection)
                {
                    double fi;
                    Relement = (XmlElement)摩擦係數Node;
                    Array.Resize(ref MaterialsCoefArrayR, MaterialsCoefCountR + 1);
                    if (Relement.GetAttribute("Coef").ToString() == "")
                    {
                        MaterialsCoefArrayR[MaterialsCoefCountR].coef = -9999;//A signal to specify this is empty.
                    }
                    else
                    {
                        if (!double.TryParse(Relement.GetAttribute("Coef").ToString(), out fi))
                        {
                            return "讀取材質間摩擦係數值錯誤";
                        }
                        else
                        {
                            MaterialsCoefArrayR[MaterialsCoefCountR].coef = fi;
                        }
                    }
                    if (Relement.GetAttribute("Material1").ToString() == "")
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

                    BlockMainArrayR[blockSizer] = new Class_BlockSect_MT();
                    BlockMainArrayR[blockSizer].名稱 = Relement.GetAttribute("名稱").ToString();
                    if (!int.TryParse(Relement.GetAttribute("點數").ToString(), out PointCount))
                    {
                        return "Block讀取失敗!點數欄位無法讀取";
                    }
                    BlockMainArrayR[blockSizer].座標點數 = PointCount;
                    double ftest;
                    bool btest;

                    //Block單位體積重量
                    RNode = BlockNode.SelectSingleNode("單位體積重量");
                    if (object.Equals(RNode, null))
                    {
                        return "Block讀取單位體積重量失敗!";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    {
                        return  "Block讀取單位體積重量失敗!";
                    }
                    BlockMainArrayR[blockSizer].單位體積重量 = ftest;

                    //Block地震時單位體積重量
                    RNode = BlockNode.SelectSingleNode("地震時單位體積重量");
                    if (object.Equals(RNode, null))
                    {
                        return "Block讀取地震時單位體積重量失敗!";
                    }
                    Relement = (XmlElement)RNode;
                    if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                    {
                        return "Block讀取地震時單位體積重量失敗!";
                    }
                    BlockMainArrayR[blockSizer].地震時單位體積重量 = ftest;

                    //計算Moment與否.
                    RNode = BlockNode.SelectSingleNode("計算Moment");
                    if (object.Equals(RNode, null))
                    {
                        return  "Block讀取計算Moment狀況失敗!";
                    }
                    Relement = (XmlElement)RNode;
                    if (!bool.TryParse(Relement.GetAttribute("Value"), out btest))
                    {
                        return  "Block讀取計算Moment狀況失敗!";
                    }
                    BlockMainArrayR[blockSizer].是否為混凝土塊 = btest;

                    //Block使用材質
                    RNode = BlockNode.SelectSingleNode("使用材質");
                    if (object.Equals(RNode, null))
                    {
                        return  "Block讀取Block使用材質失敗!!";
                    }
                    Relement = (XmlElement)RNode;
                    /*
                    if (Relement.GetAttribute("Value").ToString() != "")
                    {
                        //檢查材質是否有被定義.
                        if (!DDR.ContainsKey(Relement.GetAttribute("Value").ToString()))
                        {
                            MessageBox.Show("Block讀取Block使用材質失敗!!";
                        }
                        else
                        {
                            //Nothing.
                        }
                    }*/
                    BlockMainArrayR[blockSizer].使用材質 = Relement.GetAttribute("Value");

                    string[] GetRE = new string[] { };
                    XmlNodeList Block周圍參考材質 = BlockNode.SelectNodes("周圍參考材質");
                    if (!object.Equals(Block周圍參考材質, null))
                    {
                        foreach (XmlNode MD in Block周圍參考材質)
                        {
                            Relement = (XmlElement)MD;
                            /*
                            if (!DDR.ContainsKey(Relement.GetAttribute("Value").ToString()))
                            {
                                return "Block讀取周圍參考材質";
                            }
                            */
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
                            return  "Block讀取座標時無法順利從'xValue'欄位轉換座標值";
                        }
                        getx[getPointsize] = ftest;
                        if (!double.TryParse(Relement.GetAttribute("yValue"), out ftest))
                        {
                            return  "Block讀取座標時無法順利從'yValue'欄位轉換座標值";
                        }
                        gety[getPointsize] = ftest;
                        getPointsize += 1;
                    }

                    if (!(getPointsize == PointCount))
                    {
                        return  "Block讀取座標點數量不一致!";
                    }

                    BlockMainArrayR[blockSizer].X = getx;
                    BlockMainArrayR[blockSizer].Y = gety;

                    blockSizer += 1;
                }


                RNode = doc.SelectSingleNode("Root/Blocks/選取Block序號");
                if (object.Equals(RNode, null))
                {
                    return  "讀取Block選取序號時失敗";
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
                //成功與否 = true;
            }
            catch
            {
                return "出現錯誤";
                //MessageBox.Show("出現錯誤!");
            }

            TSP_DATETIME.Text = "最新一次編輯時間: "+  最後編輯時間R;

            textBox_設計潮位高.Text = 設計潮位高R.ToString();
            textBox_設計潮位低.Text = 設計潮位低R.ToString();
            textBox_殘留水位.Text = 殘留水位R.ToString();
            textBox_平時上載荷重.Text = 平時上載荷重R.ToString();
            textBox_地震時上載荷重.Text = 地震時上載荷重R.ToString();
            textBox_船舶牽引力.Text = 船舶牽引力R.ToString();
            textBox_陸上設計震度.Text = 陸上設計震度R.ToString();
            textBox_水中設計震度.Text = 水中設計震度R.ToString();
            textBox_rw.Text = 水單位重R.ToString();
            textBox_BoatColumnHeight.Text = 繫船柱突出高度R.ToString();
            textBox_背填料內摩擦角.Text = 背填料內摩擦角R.ToString();
            textBox_背填料壁面摩擦角.Text = 背填料壁面摩擦角R.ToString();
            textBox_背填料水平傾斜角.Text = 背填料水平傾斜角R.ToString();
            textBox_入土深度.Text = 入土深度R.ToString();
            textBox_拋石厚度.Text = 拋石厚度R.ToString();
            //cmb_seawaveDir.SelectedItem = dirr;
            textBox_地盤基礎內摩擦角.Text = 地盤內基礎內摩擦角R.ToString();
            textBox_土壤凝聚力.Text = 土壤凝聚力R.ToString();
            
            textBox_Nq.Text = NqR.ToString();
            textBox_Nc.Text = NcR.ToString();
            textBox_Nr.Text = NrR.ToString();
            
            textBox_平時滑動安全係數.Text = 平時滑動安全係數R.ToString();
            textBox_平時傾倒安全係數.Text = 平時傾倒安全係數R.ToString();
            textBox_平時地盤承載力安全係數.Text = 平時地盤承載力安全係數R.ToString();
            textBox_地震時滑動安全係數.Text = 地震時滑動安全係數R.ToString();
            textBox_地震時傾倒安全係數.Text = 地震時傾倒安全係數R.ToString();
            textBox_地震時地盤承載力安全係數.Text = 地震時地盤承載力安全係數R.ToString();
            textBox_SoilR_Earth.Text = 土壤重陸上R.ToString();
            textBox_SoilR_Water.Text = 土壤重水中R.ToString();
            /*
            textBox_KaStage1.Text = 平時設計震度土壓係數R.ToString();
            textBox_KaStage2.Text = 地震時水上土壓係數R.ToString();
            textBox_KaStage3.Text = 地震時水下土壓係數R.ToString();
            */


            Form_BlockNameAndCorrdinate p = new Form_BlockNameAndCorrdinate();
            for (int i = 0; i < BlockMainArrayR.GetLength(0); i++)
            {
                if (!p.CheckIsConvexPolygonAndCounterClockWise(BlockMainArrayR[i].X, BlockMainArrayR[i].Y))
                {
                    //若排列順序非逆時針.
                    MessageBox.Show("Block" + (i + 1).ToString() + "(從1開始)的座標矩陣錯誤,此Block非凸邊形且座標沒有依照逆時針方向紀錄!!");
                }
            }
            MaterialArray = MaterialR;
            MaterialCount = MaterialCountR;
            MaterialNameToArraySubScript.Clear();
            MaterialSubscriptToName.Clear();
            for (int i = 0; i < MaterialArray.GetLength(0); i++)
            {
                //DGMaterial.Rows.Add(new object[] { (i + 1).ToString(), MaterialArray[i].ToString() });
                MaterialNameToArraySubScript.Add(MaterialR[i].ToString(), i);
                MaterialSubscriptToName.Add(i, MaterialR[i].ToString());
            }
            MaterialsCoefArray = MaterialsCoefArrayR;
            MaterialRoughnessArrayCount = MaterialsCoefCountR;


            //Block區塊,填入變數.
            listBox_SectSetting.Items.Clear();
            ReferencedMaterialCHKL.Items.Clear();
            BlockMainArray = BlockMainArrayR;
            Array.Resize(ref BlockMainArray, 0);
            Array.Resize(ref BlockMainArray, BlockMainArrayR.GetLength(0));
            BlockCount = BlockMainArrayR.GetLength(0);
            Array.Resize(ref ELArray, 0);
            Array.Resize(ref ELArray, ELSizer);
            ELArray = ELArrayR;
            ELSize = ELSizer;
            //填入EL的DG內,如果有資料的話.
            ELDGV1.Rows.Clear();
            DataGridViewRowCollection rows = ELDGV1.Rows;
            for (int i = 0; i < ELSize; i++)
            {
                rows.Add(new object[] { ELArray[i] });
            }
            if (ELDGV1.Rows.Count > 0)
            {
                ELDGV1.CurrentCell = ELDGV1.Rows[0].Cells[0];
            }
            //加入行塊資訊.
            //  重設Block類的Dictionary.
            BlockArraySubscriptToName.Clear();
            BlockListSubScriptToName.Clear();
            BlockNameToArraySubscript.Clear();
            BlockNameToListSubScript.Clear();
            chart_Plot.Series.Clear();
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
            blockLabels設定ToolStripMenuItem.Enabled = false;
            if (BlockMainArray.GetLength(0) > 0)
            {
                //MessageBox.Show("H1" + chart_Plot.Series.Count.ToString());
                ELDGV1.Enabled = true;
                blockLabels設定ToolStripMenuItem.Enabled = true;
                調整Chart(chart_Plot);
                繪上EL();
                //if (!_調整Chart比例)
                //{
                //    //取得YX比例(null);
                //}
                //else
                //{
                //    以YX比例設定座標軸(null);
                //}
            }

            selectname = (selectedBlockIndex == -1 ? null : BlockMainArray[selectedBlockIndex].名稱);
            cmb_ShowOnBlockListChoice.SelectedItem = BlockToolTip資訊選擇;
            if (selectedBlockIndex != -1)
            {
                listBox_SectSetting.SelectedIndex = selectedBlockIndex;//此函數會觸動顯示Chart與Property Grid的功能.
            }
            btn_ModifiedBlock.Enabled = (selectedBlockIndex == -1 ? false : true);
            btnRemoveSects.Enabled = btn_ModifiedBlock.Enabled;
            InterfaceBlock = null;
            if (BlockMainArray.GetLength(0) > 0)
            {
                string Msg = "";
                開始檢核ToolStripMenuItem.Enabled = (mainForm.軟體驗證是否通過);//檢視目前是否已有合理認證(ref Msg));// && true);// (mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref Msg) && true);
                btn_Test.Enabled = 開始檢核ToolStripMenuItem.Enabled;
            }
            else
            {
                開始檢核ToolStripMenuItem.Enabled = false;
                btn_Test.Enabled = false;
            }
            //btn_LogOutput.Enabled = false;
            tsp_cond.Text = "請設定或編輯您的專案檔";
            if (SelectedTab != -1 && SelectedTab >= 0 && SelectedTab <= tabControl1.TabCount - 1)
            {
                tabControl1.SelectedIndex = SelectedTab;
            }

            btn_LogOutput.Enabled = false;
            btn_OutputWord.Enabled = false;
            輸出LogToolStripMenuItem.Enabled = false;
            輸出Word報表ToolStripMenuItem.Enabled = false;
            //toolStripStatusLabel1.Text = "開啟完成,取得" + BlockMainArray.GetLength(0).ToString() + "個區塊";

            //*****
            //填入資訊.
            /*
            data_BlockTempShow.Rows.Clear();
            DataGridViewRowCollection rows = data_BlockTempShow.Rows;
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                //序號, 形塊名稱, 單位體積重量, 周圍參考材質, 使用材質.
                string temp = "";
                for(int i2=0;i2<BlockMainArray[i].周圍參考材質.GetLength(0);i2++)
                {
                    temp += (BlockMainArray[i].周圍參考材質[i2]);
                    if(i2!=(BlockMainArray[i].周圍參考材質.GetUpperBound(0)))
                    {
                        temp += ",";
                    }
                }
                rows.Add(new object[] { (i + 1).ToString(), BlockMainArray[i].名稱, BlockMainArray[i].單位體積重量, temp, BlockMainArray[i].使用材質 });
            }
            */
            return "";
            //******************
        }
        private void 儲存XML專案檔(string xmlfullpath, bool showDia = true)
        {
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
            版本.SetAttribute("Value", "SeaDikeVS_P2");
            Root.AppendChild(版本);


            XmlElement 全域參數XML點 = doc.CreateElement("GlobalParameters");
            Root.AppendChild(全域參數XML點);

            //2. 全域參數設定區塊
            //建立子節點
            XmlElement 選擇Tab = doc.CreateElement("選擇Tab");
            選擇Tab.SetAttribute("Value", tabControl1.SelectedIndex.ToString());

            XmlElement BlockToolTipinfo = doc.CreateElement("BlockTooltip資訊");
            BlockToolTipinfo.SetAttribute("Value", cmb_ShowOnBlockListChoice.SelectedItem.ToString());

            XmlElement 最後編輯時間 = doc.CreateElement("最後編輯時間");
            最後編輯時間.SetAttribute("Value", TSP_DATETIME.Text.Replace("最新一次編輯時間: " ,""));

            XmlElement 地盤基礎內摩擦角 = doc.CreateElement("地盤基礎內摩擦角");
            地盤基礎內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);

            XmlElement 設計潮位高 = doc.CreateElement("設計潮位高");
            設計潮位高.SetAttribute("Value", textBox_設計潮位高.Text);

            XmlElement 設計潮位低 = doc.CreateElement("設計潮位低");
            設計潮位低.SetAttribute("Value", textBox_設計潮位低.Text);

            XmlElement 殘留水位= doc.CreateElement("殘留水位");
            殘留水位.SetAttribute("Value", textBox_殘留水位.Text);

            XmlElement 平時上載荷重 = doc.CreateElement("平時上載荷重");
            平時上載荷重.SetAttribute("Value", textBox_平時上載荷重.Text);

            XmlElement 地震時上載荷重 = doc.CreateElement("地震時上載荷重");
            地震時上載荷重.SetAttribute("Value", textBox_地震時上載荷重.Text);

            XmlElement 船舶牽引力 = doc.CreateElement("船舶牽引力");
            船舶牽引力.SetAttribute("Value", textBox_船舶牽引力.Text);

            XmlElement 陸上設計震度 = doc.CreateElement("陸上設計震度");
            陸上設計震度.SetAttribute("Value", textBox_陸上設計震度.Text);

            XmlElement 水中設計震度 = doc.CreateElement("水中設計震度");
            水中設計震度.SetAttribute("Value", textBox_水中設計震度.Text);

            XmlElement 水單位重 = doc.CreateElement("水單位重");
            水單位重.SetAttribute("Value", textBox_rw.Text);

            XmlElement 繫船柱突出高度 = doc.CreateElement("繫船柱突出高度");
            繫船柱突出高度.SetAttribute("Value", textBox_BoatColumnHeight.Text);


            XmlElement 背填料內摩擦角 = doc.CreateElement("背填料內摩擦角");
            背填料內摩擦角.SetAttribute("Value", textBox_背填料內摩擦角.Text);

            XmlElement 背填料壁面摩擦角 = doc.CreateElement("背填料壁面摩擦角");
            背填料壁面摩擦角.SetAttribute("Value", textBox_背填料壁面摩擦角.Text);

            XmlElement 背填料水平傾斜角 = doc.CreateElement("背填料水平傾斜角");
            背填料水平傾斜角.SetAttribute("Value", textBox_背填料水平傾斜角.Text);

            XmlElement 入土深度 = doc.CreateElement("入土深度");
            入土深度.SetAttribute("Value", textBox_入土深度.Text);

            XmlElement 拋石厚度 = doc.CreateElement("拋石厚度");
            拋石厚度.SetAttribute("Value", textBox_拋石厚度.Text);

            //XmlElement 海側方向info = doc.CreateElement("海側方向");
            //海側方向info.SetAttribute("Value", cmb_seawaveDir.SelectedItem.ToString());


            XmlElement 土壤凝聚力 = doc.CreateElement("土壤凝聚力");
            土壤凝聚力.SetAttribute("Value", textBox_土壤凝聚力.Text);

            XmlElement 地盤內基礎內摩擦角 = doc.CreateElement("地盤內基礎內摩擦角");
            地盤內基礎內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);

            
            XmlElement Nc = doc.CreateElement("Nc");
            Nc.SetAttribute("Value", textBox_Nc.Text);

            XmlElement Nq = doc.CreateElement("Nq");
            Nq.SetAttribute("Value", textBox_Nq.Text);

            XmlElement Nr = doc.CreateElement("Nr");
            Nr.SetAttribute("Value", textBox_Nr.Text);
            

            XmlElement 平時滑動安全係數 = doc.CreateElement("平時滑動安全係數");
            平時滑動安全係數.SetAttribute("Value", textBox_平時滑動安全係數.Text);

            XmlElement 平時傾倒安全係數 = doc.CreateElement("平時傾倒安全係數");
            平時傾倒安全係數.SetAttribute("Value", textBox_平時傾倒安全係數.Text);

            XmlElement 平時地盤承載力安全係數 = doc.CreateElement("平時地盤承載力安全係數");
            平時地盤承載力安全係數.SetAttribute("Value", textBox_平時地盤承載力安全係數.Text);

            XmlElement 地震時滑動安全係數 = doc.CreateElement("地震時滑動安全係數");
            地震時滑動安全係數.SetAttribute("Value", textBox_地震時滑動安全係數.Text);


            XmlElement 地震時傾倒安全係數 = doc.CreateElement("地震時傾倒安全係數");
            地震時傾倒安全係數.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);

            XmlElement 地震時地盤承載力安全係數 = doc.CreateElement("地震時地盤承載力安全係數");
            地震時地盤承載力安全係數.SetAttribute("Value", textBox_地震時地盤承載力安全係數.Text);

            XmlElement 土壤重陸上 = doc.CreateElement("土壤重陸上");
            土壤重陸上.SetAttribute("Value", textBox_SoilR_Earth.Text);

            XmlElement 土壤重水中 = doc.CreateElement("土壤重水中");
            土壤重水中.SetAttribute("Value", textBox_SoilR_Water.Text);

            /*
            XmlElement 平時設計震度土壓係數 = doc.CreateElement("平時設計震度土壓係數");
            平時設計震度土壓係數.SetAttribute("Value", textBox_KaStage1.Text);

            XmlElement 地震時水上土壓係數 = doc.CreateElement("地震時水上土壓係數");
            地震時水上土壓係數.SetAttribute("Value", textBox_KaStage2.Text);

            XmlElement 地震時水下土壓係數 = doc.CreateElement("地震時水下土壓係數");
            地震時水下土壓係數.SetAttribute("Value", textBox_KaStage3.Text);
            */

            /*
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            XmlElement 內摩擦角 = doc.CreateElement("內摩擦角");
            內摩擦角.SetAttribute("Value", textBox_地盤基礎內摩擦角.Text);
            */

            全域參數XML點.AppendChild(選擇Tab);
            全域參數XML點.AppendChild(BlockToolTipinfo);
            全域參數XML點.AppendChild(最後編輯時間);
            全域參數XML點.AppendChild(設計潮位高);
            全域參數XML點.AppendChild(設計潮位低);
            全域參數XML點.AppendChild(殘留水位);
            全域參數XML點.AppendChild(平時上載荷重);
            全域參數XML點.AppendChild(地震時上載荷重);
            全域參數XML點.AppendChild(船舶牽引力);
            全域參數XML點.AppendChild(陸上設計震度);
            全域參數XML點.AppendChild(水中設計震度);
            全域參數XML點.AppendChild(水單位重);
            全域參數XML點.AppendChild(繫船柱突出高度);
            全域參數XML點.AppendChild(背填料內摩擦角);
            全域參數XML點.AppendChild(背填料壁面摩擦角);
            全域參數XML點.AppendChild(背填料水平傾斜角);
            全域參數XML點.AppendChild(入土深度);
            全域參數XML點.AppendChild(拋石厚度);
            //全域參數XML點.AppendChild(海側方向info);
            全域參數XML點.AppendChild(地盤內基礎內摩擦角);
            全域參數XML點.AppendChild(土壤凝聚力);
            
            全域參數XML點.AppendChild(Nq);
            全域參數XML點.AppendChild(Nc);
            全域參數XML點.AppendChild(Nr);
            
            全域參數XML點.AppendChild(平時滑動安全係數);
            全域參數XML點.AppendChild(平時傾倒安全係數);
            全域參數XML點.AppendChild(平時地盤承載力安全係數);
            全域參數XML點.AppendChild(地震時滑動安全係數);
            全域參數XML點.AppendChild(地震時傾倒安全係數);
            全域參數XML點.AppendChild(地震時地盤承載力安全係數);
            全域參數XML點.AppendChild(土壤重陸上);
            全域參數XML點.AppendChild(土壤重水中);
            /*
            全域參數XML點.AppendChild(平時設計震度土壓係數);
            全域參數XML點.AppendChild(地震時水上土壓係數);
            全域參數XML點.AppendChild(地震時水下土壓係數);
            */

            //全域參數XML點.AppendChild();
            //全域參數XML點.AppendChild();
            //全域參數XML點.AppendChild();
            //全域參數XML點.AppendChild();


            XmlElement EL點數 = doc.CreateElement("EL點數");
            //EL點數.SetAttribute("Value", ELSize.ToString());
            全域參數XML點.AppendChild(EL點數);
            for (int i = 0; i < ELSize; i++)
            {
                XmlElement ELI = doc.CreateElement("EL");// + (i + 1).ToString());
                ELI.SetAttribute("Value", ELArray[i].ToString());
                EL點數.AppendChild(ELI);
            }
            //材質與摩擦係數設定.
            // 1. 若摩擦係數沒有設定,Value填為"NONE"
            XmlElement 使用材質 = doc.CreateElement("使用材質");
            全域參數XML點.AppendChild(使用材質);
            for (int i = 0; i < MaterialCount; i++)
            {
                XmlElement 使用材質I = doc.CreateElement("UseMaterial");
                使用材質I.SetAttribute("Value", MaterialArray[i].ToString());
                使用材質.AppendChild(使用材質I);
            }
            XmlElement 摩擦係數 = doc.CreateElement("摩擦係數");
            全域參數XML點.AppendChild(摩擦係數);
            for (int i = 0; i < MaterialRoughnessArrayCount; i++)
            {
                XmlElement 摩擦係數I = doc.CreateElement("MaterialsFriction");
                if (MaterialsCoefArray[i].coef == -9999)
                {
                    摩擦係數I.SetAttribute("Coef", "");
                }
                else
                {
                    摩擦係數I.SetAttribute("Coef", MaterialsCoefArray[i].coef.ToString());
                }
                if (MaterialsCoefArray[i].Id1 != -9999)
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
            for (int i = 0; i <= BlockMainArray.GetUpperBound(0); i++)
            {
                //每個Block都是一個新的子節點，內涵其他參數.

                XmlElement BlockNode = doc.CreateElement("形塊");//統一的物件名稱.
                //MessageBox.Show("H4-1-1");
                BlockNode.SetAttribute("名稱", BlockListSubScriptToName[i]);
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
                XmlElement Block地震時單位體積重量 = doc.CreateElement("地震時單位體積重量");
                Block地震時單位體積重量.SetAttribute("Value", BlockMainArray[i].地震時單位體積重量.ToString());
                XmlElement Block使用材質 = doc.CreateElement("使用材質");
                Block使用材質.SetAttribute("Value", BlockMainArray[i].使用材質.ToString());
                XmlElement Block是否計算Moment = doc.CreateElement("計算Moment");
                Block是否計算Moment.SetAttribute("Value", BlockMainArray[i].是否為混凝土塊.ToString());


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
                BlockNode.AppendChild(Block地震時單位體積重量);
                BlockNode.AppendChild(Block使用材質);
                BlockNode.AppendChild(Block是否計算Moment);

                string[] 周圍參考材質 = BlockMainArray[i].周圍參考材質;
                for (int i2 = 0; i2 < 周圍參考材質.GetLength(0); i2++)
                {
                    XmlElement Block環繞參考材質 = doc.CreateElement("周圍參考材質");
                    Block環繞參考材質.SetAttribute("Value", 周圍參考材質[i2]);
                    BlockNode.AppendChild(Block環繞參考材質);
                }

                //MessageBox.Show("H4-2");

                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                XmlElement Block座標點;

                for (int i2 = 0; i2 <= getx.GetUpperBound(0); i2++)
                {
                    Block座標點 = doc.CreateElement("BlockCoordinate");
                    //Block混凝土方塊與拋石摩擦係數.SetAttribute("Value", BlockMainArray[i].混凝土方塊與拋石摩擦係數.ToString());
                    Block座標點.SetAttribute("xValue", getx[i2].ToString());
                    Block座標點.SetAttribute("yValue", gety[i2].ToString());

                    BlockNode.AppendChild(Block座標點);
                }
                //MessageBox.Show("H4-3");

            }

            //點選中的Block
            XmlElement 選取Block名稱 = doc.CreateElement("選取Block序號");
            if (listBox_SectSetting.SelectedIndex == -1) //故意分開較為保險.
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
        }
        #endregion
        #region 型塊設定區域
        private void chart_Plot_Enter(object sender, EventArgs e)
        {
            //為焦點時.
            //顯示設定ToolStripMenuItem.Visible = true;
        }

        private void chart_Plot_Leave(object sender, EventArgs e)
        {
            //顯示設定ToolStripMenuItem.Visible = false;
        }
        private void btn_AddASect_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }

            InterfaceBlock = null;
            Form_BlockNameAndCorrdinate form_blockNameAnsCoordinate = new Form_BlockNameAndCorrdinate(this, BlockNameToListSubScript,"MT");
            form_blockNameAnsCoordinate.ShowDialog();

            if (object.Equals(InterfaceBlock, null))
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
            listBox_SectSetting.SetSelected(BlockCount - 1, true); //設定Listbox點選項目.

            blockLabels設定ToolStripMenuItem.Enabled = true;
            //string Msg = "";
            //開始檢核ToolStripMenuItem.Enabled = (mainForm.檢視目前是否已有合理認證(ref Msg) && true);// 檢視目前是否已設定正確機碼來鎖定機器(ref Msg) && true);
            //btn_Test.Enabled = 開始檢核ToolStripMenuItem.Enabled;

            //btnRemoveSects.Enabled = true;
            //btn_ModifiedBlock.Enabled = true;
            InterfaceBlock = null;
            //5. 顯示Property Grid預設參數.
            //propertyGrid_Block.SelectedObject = new Class_Block_Interface();
        }
        private void ShowCurrentBlockInformation()
        {
            //1.更換Chart 繪製與加入不在這個區塊.
            if (!object.Equals(selectname, null))
            {
                //chart_Plot.Series[selectname].BorderColor = Color.Black;
                chart_Plot.Series[selectname].BorderWidth = 2;
                chart_Plot.Series[selectname].Color = Color.Black;

                //label_XXX.Text = "";
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
                    //label_XXX.Text = "(" + TT.AnchorX.ToString("00.00") + "," + TT.AnchorY.ToString("00.00") + ")\n(" + TT.X.ToString("00.00") + "," + TT.Y.ToString("00.00") + ")\n" + TT.AnchorAlignment.ToString();
                }


            }
            //MessageBox.Show(nowname);

            //chart_Plot.Series[nowname].BorderColor = Color.Red;
            chart_Plot.Series[nowname].BorderWidth = 3;
            chart_Plot.Series[nowname].Color = Color.Red;
            //chart_Plot.Series[nowname].MarkerBorderWidth = 3;
            selectname = nowname;

            //2.Property Grid更換.
            //BBBBBBBBBBBBBBBBBBBBBBBBBBBBB
            
            Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
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
            
        }
        private void AddNewChart(Class_BlockSect_MT NewI)
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
            for (int i = 0; i < PointCount; i++)
            {
                //
                chart_Plot.Series[NewI.名稱].Points.AddXY(getx[i], gety[i]);
            }
            if (!(getx[getx.GetUpperBound(0)] == getx[0] && gety[gety.GetUpperBound(0)] == gety[0]))
            {
                chart_Plot.Series[NewI.名稱].Points.AddXY(getx[0], gety[0]);
            }

            chart_Plot.Series[NewI.名稱].BorderColor = Color.Black;
            chart_Plot.Series[NewI.名稱].Color = Color.Black;//= Color.Transparent;
            chart_Plot.Series[NewI.名稱].MarkerBorderWidth = 2;
            ELDGV1.Enabled = true;
            調整Chart(chart_Plot);
            //取得YX比例(null);
            //;
            if(!_調整Chart比例)
            {
                取得YX比例(null);
                繪上EL();
            }
            else
            {
                以YX比例設定座標軸(null);
            }
        }
        public void 調整Chart(Chart INS)
        {
            if (object.Equals(INS, null))
            {
                INS = chart_Plot;
            }

            double Xmin = 1000000; // double.MaxValue;
            double Xmax = -100000; // double.MinValue;
            double Ymin = Xmin;
            double Ymax = Xmax;
            foreach (Series ss in INS.Series)
            {
                foreach (DataPoint dp in ss.Points)
                {
                    //label_Show.Text += (dp.XValue.ToString());
                    if (dp.XValue > Xmax && ss.Name.Substring(0) != "E" && ss.Name != "HWL") { Xmax = dp.XValue; }
                    if (dp.XValue < Xmin && ss.Name.Substring(0) != "E" && ss.Name != "HWL") { Xmin = dp.XValue; }
                    if (dp.YValues[0] > Ymax) { Ymax = dp.YValues[0]; }
                    if (dp.YValues[0] < Ymin) { Ymin = dp.YValues[0]; }
                }
            }

            //label_Show.Text = Xmin.ToString() + ":" + Xmax.ToString();
            if (Xmin == 1000000)
            {
                Xmin = 0;
                Xmax = 0.4;
            }
            if (Ymin == 1000000)
            {
                Ymin = 0;
                Ymax = 0.4;
            }
            double xdiff = (Xmax - Xmin);
            double ydiff = (Ymax - Ymin);
            double xspace, yspace;
            xspace = 取得最佳Interval(Xmin, Xmax);
            yspace = 取得最佳Interval(Ymin, Ymax);

            if (xspace > 1)
            {
                xspace = 1;
            }
            if (yspace > 1)
            {
                yspace = 1;
            }
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
            /*
            ChartArea ca = INS.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            ElementPosition cap = ca.Position;
            Size CaSize = new Size((int)(cap.Width * INS.ClientSize.Width / 100f), (int)(cap.Height * INS.ClientSize.Height / 100f));
            Size IppSize=new Size((int)(ipp0.Width*CaSize.Width/100f),(int)(ipp0.Height*CaSize.Height/100f));

            int ippNewSide = Math.Min(IppSize.Width, IppSize.Height);
            //MessageBox.Show(ippNewSide.ToString());
            MessageBox.Show(cap.Width.ToString() + "," + cap.Height.ToString());
            MessageBox.Show(IppSize.Width.ToString()+ "," +  IppSize.Height.ToString());

            float px = ipp0.Width / IppSize.Width * ippNewSide;
            float py = ipp0.Height / IppSize.Height * ippNewSide;
            */

            double NewXmin = Math.Floor(Xmin / xspace) * xspace;
            double NewXmax = Math.Ceiling(Xmax / xspace) * xspace;// Xmin - xspace / 2.0 + 6.5 * xspace;// + Math.Floor((Xmax - Xmin) / xspace + 0.5) * xspace;
            double NewYmax = Math.Ceiling(Ymax / yspace) * yspace + yspace / 2.0;// Ymin - yspace / 2.0 + 6.5 * yspace;
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

            /*
            ChartArea ca = INS.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            double xf = (double)ipp0.Width / (NewXmax - NewXmin);
            double yf = (double)ipp0.Height / (NewYmax - NewYmin);
            _ChartYXRatio = yf / xf;
            */
        }
        private void 取得YX比例(Chart INS2)
        {

            if (object.Equals(INS2, null))
            {
                INS2 = chart_Plot;
            }
            ChartArea ca = INS2.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            //Size OldClientSize = chart_Plot.ClientSize;
            //int oldClientXSize = chart_Plot.ClientSize.Width;
            //int oldClientYSize = chart_Plot.ClientSize.Height;
            _oldXmin = INS2.ChartAreas[0].AxisX.Minimum;// -xspace/2.0;// -xspace/2.0;// - xspace;
            _oldXmax = INS2.ChartAreas[0].AxisX.Maximum;// NewXmax;
                                                              //double oldXInterval = INS.ChartAreas[0].AxisX.Interval;
            _oldYmin = INS2.ChartAreas[0].AxisY.Minimum;// - yspace;
            _oldYmax = INS2.ChartAreas[0].AxisY.Maximum;// NewYmax;
                                                              //double oldYInterval = chart_Plot.ChartAreas[0].AxisY.Interval;


            //Adjusting the interval and new ymax,ymin,xmax,xmin.

            _xf = (double)ipp0.Width / (_oldXmax - _oldXmin);
            _yf = (double)ipp0.Height / (_oldYmax - _oldYmin);
            _ChartYXRatio = _yf / _xf;
            _OldChartYXRatio = _ChartYXRatio;
            //MessageBox.Show(_ChartYXRatio.ToString());

        }
        public void 以YX比例設定座標軸(Chart INS3,bool BackToOriginal=false)
        {
            if(object.Equals(INS3,null))
            {
                INS3 = chart_Plot;
            }
            ChartArea ca = INS3.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            //Size OldClientSize = chart_Plot.ClientSize;
            //int oldClientXSize = chart_Plot.ClientSize.Width;
            //int oldClientYSize = chart_Plot.ClientSize.Height;
            /*
            double oldXmin = INS3.ChartAreas[0].AxisX.Minimum;// -xspace/2.0;// -xspace/2.0;// - xspace;
            double oldXmax = INS3.ChartAreas[0].AxisX.Maximum;// NewXmax;
                                                              //double oldXInterval = INS.ChartAreas[0].AxisX.Interval;
            double oldYmin = INS3.ChartAreas[0].AxisY.Minimum;// - yspace;
            double oldYmax = INS3.ChartAreas[0].AxisY.Maximum;// NewYmax;
                                                              //double oldYInterval = chart_Plot.ChartAreas[0].AxisY.Interval;

            */
            //Adjusting the interval and new ymax,ymin,xmax,xmin.
            //float xf = (float)ipp0.Width / (float)(oldXmax - oldXmin);
            //float yf = (float)ipp0.Height / (float)(oldYmax - oldYmin);

            //if (_調整Chart比例)
            //{
            //MessageBox.Show(_ChartYXRatio.ToString());
            //先回復原本設定!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            INS3.ChartAreas[0].AxisX.Maximum = _oldXmax;
            INS3.ChartAreas[0].AxisX.Minimum = _oldXmin;
            INS3.ChartAreas[0].AxisY.Minimum = _oldYmin;
            INS3.ChartAreas[0].AxisY.Maximum = _oldYmax;
            if (BackToOriginal) { return; }
            if (_ChartYXRatio == -9999) { return; }
            if (_ChartYXRatio == _OldChartYXRatio) { return; }
            //先回復原本設定!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            //MessageBox.Show((_yf < _ChartYXRatio * _xf).ToString());
            if (_yf < _ChartYXRatio * _xf)
            {
                //以yf調整Xmax.
                INS3.ChartAreas[0].AxisX.Maximum = _oldXmin + _ChartYXRatio * ipp0.Width / _yf;
            }
            else if (_yf > _ChartYXRatio * _xf)
            {
                //以xf調整Ymax.
                INS3.ChartAreas[0].AxisY.Maximum = _oldYmin + ipp0.Height / _xf / _ChartYXRatio;
                //    }
            }
            else
            { }
            /*else
            {
                INS3.ChartAreas[0].AxisX.Maximum = _oldXmax;
                INS3.ChartAreas[0].AxisX.Minimum = _oldXmin;
                INS3.ChartAreas[0].AxisY.Minimum = _oldYmin;
                INS3.ChartAreas[0].AxisY.Maximum = _oldYmax;
            }
            */
            _調整Chart比例 = true;
            繪上EL();
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
        void 取得目前ELMIN與ELMAX(ref double[] ELMIN, ref double[] ELMAX, ref double AllCenterX)
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
            ELMIN[0] = 1000000;
            ELMIN[1] = 1000000;
            ELMAX[0] = -100000;
            ELMAX[1] = -100000;
            //判斷.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                Double[] XI = BlockMainArray[i].X;
                double[] YI = BlockMainArray[i].Y;
                if (XI.GetLength(0) == 0)
                { continue; }
                for (int j = 0; j < XI.GetLength(0); j++)
                {
                    if (XI[j] < AllCenterX)
                    {
                        //Left Part.
                        if (YI[j] < ELMIN[0])
                        {
                            //左邊的最低點.
                            ELMIN[0] = YI[j];
                        }
                        if (YI[j] > ELMAX[0])
                        {
                            ELMAX[0] = YI[j];
                        }
                    }
                    if (XI[j] > AllCenterX)
                    {
                        //右邊的部分.
                        if (YI[j] < ELMIN[1])
                        {
                            //右邊的最低點.
                            ELMIN[1] = YI[j];
                        }
                        if (YI[j] > ELMAX[1])
                        {
                            ELMAX[1] = YI[j];
                        }
                    }
                }//Loop through each node of Blocks.
            }
        }
        public void 繪上EL()
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
                for (int i = 0; i < ELSize; i++)
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
                if (double.TryParse(textBox_設計潮位高.Text, out HWLValue)) //可以順利轉換HWL.
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
                    chart_Plot.Series["HWL"].Points.AddXY(xmin, HWLValue);
                    chart_Plot.Series["HWL"].Points.AddXY(xmax, HWLValue);

                    //設定線段.
                    chart_Plot.Series["HWL"].BorderDashStyle = ChartDashStyle.Dash;
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
                取得目前ELMIN與ELMAX(ref ELMINInner, ref ELMAXInner, ref XCenterX);

                //取消根據海向決定EL的機制.
                MinEL = Math.Min(ELMINInner[0], ELMINInner[1]);
                MaxEL = Math.Max(ELMAXInner[0], ELMAXInner[1]);

                //根據目前選取的海向,決定ARROW圖與EL最大值與最小值.
                /*if (cmb_seawaveDir.SelectedItem.ToString() == "右")
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
                */
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
                for (int i = 0; i < ELSize; i++)
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
                for (int i = 0; i < ELDGV1.Rows.Count - 1; i++)
                {
                    double itest;
                    if (double.TryParse(ELDGV1.Rows[i].Cells[0].Value.ToString(), out itest))
                    {
                        if (hasHWLValue)
                        {
                            if (itest != HWLValue && itest < MaxEL && itest > MinEL)
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
                        else
                        {
                            bool repeated = false;
                            for (int i2 = 0; i2 < ELSize; i2++)
                            {
                                if (itest == ELArray[i2])
                                { repeated = true; break; }
                            }
                            if (!repeated && itest < MaxEL && itest > MinEL)
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
                if (ELSize > 0)
                {
                    //MessageBox.Show("HH");
                    for (int i = 0; i < ELSize; i++)
                    {
                        string tpname = "E" + (i + 1).ToString();
                        chart_Plot.Series.Add(tpname);
                        chart_Plot.Series[tpname].ChartType = SeriesChartType.Line;

                        //加入點資料.
                        chart_Plot.Series[tpname].Points.AddXY(xmin, ELArray[i]);
                        chart_Plot.Series[tpname].Points.AddXY(xmax, ELArray[i]);

                        //設定線段.
                        chart_Plot.Series[tpname].BorderDashStyle = ChartDashStyle.DashDot;
                        chart_Plot.Series[tpname].BorderColor = Color.Brown;
                        chart_Plot.Series[tpname].Color = Color.Brown;
                        chart_Plot.Series[tpname].BorderWidth = 2;
                        chart_Plot.Series[tpname].IsVisibleInLegend = false;
                    }
                }//若有可用之EL時.
                double Ymin = 1000000;
                double Ymax = -Ymin;
                foreach (Series ss in chart_Plot.Series)
                {
                    if (!BlockNameToArraySubscript.ContainsKey(ss.Name))
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
                if (Ymin == 1000000)
                {
                    Ymin = 0;
                    Ymax = 0.4;
                }
                double ydiff = (Ymax - Ymin);
                double yspace;
                yspace = 取得最佳Interval(Ymin, Ymax);//;ydiff / 4.0;
                if(yspace>1)
                {
                    yspace = 1;
                }
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
                double NewYmax = Math.Ceiling(Ymax / yspace) * yspace + yspace / 2.0;// Ymin + Math.Ceiling((Ymax - Ymin) / yspace) * yspace;
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

                //取消海測文字[2017/09/14].
                //海側文字.
                /*
                TextAnnotation SeaSidetext = new TextAnnotation();
                SeaSidetext.AxisX = chart_Plot.ChartAreas[0].AxisX;
                SeaSidetext.AxisY = chart_Plot.ChartAreas[0].AxisY;
                SeaSidetext.AnchorX = cmb_seawaveDir.SelectedItem.ToString() == "左" ? (chart_Plot.ChartAreas[0].AxisX.Minimum + xspace / 3.0) : (chart_Plot.ChartAreas[0].AxisX.Maximum - xspace / 3.0);
                SeaSidetext.AnchorY = NewYmax - yspace / 2.0;
                //MessageBox.Show("Ymax = " + NewYmax.ToString() + ", Ax= " + SeaSidetext.AnchorX.ToString() + " ,Ay = " + SeaSidetext.AnchorY.ToString());
                SeaSidetext.Font = new Font("微軟正黑體", 14, FontStyle.Bold);
                SeaSidetext.Text = "海側";// (cmb_seawaveDir.SelectedItem.ToString()=="E"?"<":"") + "========="+ (cmb_seawaveDir.SelectedItem.ToString()=="W"?">":"");
                chart_Plot.Annotations.Add(SeaSidetext);
                */

                //畫上Block名稱.
                for (int i = 0; i < BlockMainArray.GetLength(0); i++)
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
                    for (int j = 0; j < PointCount; j++)
                    {

                        //xcc += (gx[j]);
                        //ycc += (gy[j]);
                        if (gy[j] > ymaxi) { ymaxi = gy[j]; }
                        if (gy[j] < ymini) { ymini = gy[j]; }

                    }


                    for (int iii = PointCount - 1, i4 = 0; i4 < PointCount; iii = i4++)
                    {
                        PointFMY F1;
                        F1.x = gx[iii];
                        F1.y = gy[iii];
                        F1.z = 0;
                        PointFMY F2;
                        F2.x = gx[i4];
                        F2.y = gy[i4];
                        F2.z = 0;
                        PointFMY F3 = Cross(F1, F2);
                        aa = F3.z;
                        cx += ((F1.x + F2.x) * aa);
                        cy += ((F1.y + F2.y) * aa);
                        w += aa;
                    }
                    //xcc = xcc / (double)PointCount;
                    //ycc = ycc / (double)PointCount;
                    xcc = cx / 3.0 / w;
                    ycc = cy / 3.0 / w;
                    TT.Text = BlockListSubScriptToName[i];
                    TT.Tag = "BLOCK";
                    TT.AllowMoving = true;
                    TT.AnchorAlignment = ContentAlignment.MiddleCenter;
                    TT.Font = new Font("微軟正黑體", _AnnotationSize, FontStyle.Bold);
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
        private PointFMY Cross(PointFMY a, PointFMY b)
        {
            PointFMY RR;
            RR.x = a.y * b.z - b.y * a.z;
            RR.y = a.z * b.x - a.x * b.z;
            RR.z = a.x * b.y - a.y * b.x;
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
        public String 根據選擇的呈現選項回傳Block屬性(Class_BlockSect_MT B)
        {
            if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "無")
            {
                return "";
            }
            else if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "單位體積重")
            {
                return "(" + B.單位體積重量.ToString() + ")";
            }
            else if (cmb_ShowOnBlockListChoice.SelectedItem.ToString() == "是否為混凝土塊")
            {
                return "(" + B.是否為混凝土塊.ToString() + ")";
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
        private static Frm_AnnotationSetting frmAnnotationSetting = null;
        private static bool _UseFrmAnnotationSetting = false;
        public void 關閉BlockLabels設定()
        {
            if(!object.Equals(frmAnnotationSetting,null))
            {
                frmAnnotationSetting = null;
                _UseFrmAnnotationSetting = false;
                blockLabels設定ToolStripMenuItem.Enabled = true;
            }
        }
        private void blockLabels設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BlockMainArray.GetLength(0) == 0) {
                MessageBox.Show("需要有Block才可更改!!!", "Block繪圖區顯示調整", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //if (!_調整Chart比例)
            //{
            //獲取此時的資訊.
            /*
            ChartArea ca = chart_Plot.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            _xf = (double)ipp0.Width / (chart_Plot.ChartAreas[0].AxisX.Maximum - chart_Plot.ChartAreas[0].AxisX.Minimum);
            _yf = (double)ipp0.Height / (chart_Plot.ChartAreas[0].AxisY.Maximum - chart_Plot.ChartAreas[0].AxisY.Minimum);
            _ChartYXRatio = _yf / _xf;
            */
            //}
            _UseFrmAnnotationSetting = true;
            blockLabels設定ToolStripMenuItem.Enabled = false;
            frmAnnotationSetting = new Frm_AnnotationSetting(this, "MT",_OldChartYXRatio);
            frmAnnotationSetting.ShowDialog();// Show();

            //
        }
        #endregion
        #region 輸出區域
        private void btn_LogOutput_Click(object sender, EventArgs e)
        {
            if(isExporting || isCalc)
            { return; }

            //給周大大輸出一點東西出來.
            if (object.Equals(Mod, null))
            {
                //MessageBox.Show("你的計算主體'MOD'為Null!!!!!");
                MessageBox.Show("您目前沒有完成的檢核結果!請重新檢核,若您無法使用檢核功能,請確認您的軟體已授權,或是聯絡開發商", "Log檔案輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            if (SFD_Log.ShowDialog() == DialogResult.OK && SFD_Log.FileName != "")
            {
                string getpath = SFD_Log.FileName;
                //呼叫.
                Mod.OutPutLogFile(getpath);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                MessageBox.Show("輸出完成!", "輸出Log File完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                /*
                if (chk_OpenFileAfterOutput.Checked)
                {
                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    p.StartInfo.FileName = SFD_LogFile.FileName;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    p.Start();
                }
                */

            }

        }
        private string WordOutputMsg = "";
        private void 輸出Word報表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_OutputWord_Click(sender, e);
        }

        private void btn_OutputWord_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }

            if (object.Equals(Mod, null))
            {
                //MessageBox.Show("你的計算主體'MOD'為Null!!!!!");
                MessageBox.Show("您目前沒有完成的檢核結果!請重新檢核,若您無法使用檢核功能,請確認您的軟體已授權,或是聯絡開發商", "Word檔案輸出錯誤", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            string tempplateFile = "Output_Template_MT.docx";
            if (!File.Exists(tempplateFile))
            {
                //錯誤.
                MessageBox.Show("您輸出Word報表的功能出現嚴重錯誤!找不到Template!", "Word報表輸出功能受損", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //蒐集資料.
            載入Textbox到矩陣內();

            //進行輸出.
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
                ChartArea ca = chart_Plot.ChartAreas[0];
                ElementPosition ipp0 = ca.InnerPlotPosition;
                Size OldClientSize = chart_Plot.ClientSize;
                int oldClientXSize = chart_Plot.ClientSize.Width;
                int oldClientYSize = chart_Plot.ClientSize.Height;
                double NowXmin = chart_Plot.ChartAreas[0].AxisX.Minimum;// -xspace/2.0;// -xspace/2.0;// - xspace;
                double NowXmax = chart_Plot.ChartAreas[0].AxisX.Maximum;// NewXmax;
                //double oldXInterval=chart_Plot.ChartAreas[0].AxisX.Interval;
                double NowYmin = chart_Plot.ChartAreas[0].AxisY.Minimum;// - yspace;
                double NowYmax = chart_Plot.ChartAreas[0].AxisY.Maximum;// NewYmax;



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
                    if (!object.Equals(TT.Tag, null))
                    {
                        if (TT.Tag.ToString() == "BLOCK")
                        {
                            TT.Font = new Font("微軟正黑體", 14, FontStyle.Bold); ;
                        }
                    }
                }
                /*
                chart_Plot.ChartAreas[0].AxisX.Minimum = _oldXmin;
                chart_Plot.ChartAreas[0].AxisX.Maximum = 10.0;// _oldXmax;
                chart_Plot.ChartAreas[0].AxisY.Minimum = _oldYmin;
                chart_Plot.ChartAreas[0].AxisX.Maximum = _oldYmax;
                chart_Plot.ChartAreas[0].AxisX.Interval = 1;
                chart_Plot.ChartAreas[0].AxisY.Interval = 1;
                chart_Plot.ChartAreas[0].RecalculateAxesScale();
                */
                以YX比例設定座標軸(null, true);
                
                //double oldYInterval=chart_Plot.ChartAreas[0].AxisY.Interval;
                //MessageBox.Show(chart_Plot.ChartAreas[0].AxisX.Maximum.ToString() + "," + _oldXmax.ToString());


                //return;
                //Adjusting the interval and new ymax,ymin,xmax,xmin.
                //MessageBox.Show("Xmin = " + _oldXmin.ToString() + " , Xmax = " + _oldXmax.ToString());
                float cxf = (float)ipp0.Width/ (float)(_oldXmax - _oldXmin);
                float cyf =  (float)ipp0.Height/(float)(_oldYmax -_oldYmin);
                //MessageBox.Show("Xf = " + cxf.ToString() + ", YF = " + cyf.ToString());

                if(cxf<cyf)
                {
                    //狀況: XF使用的刻度較多,Y軸的刻度範圍較小,改以Y軸之大小來放大X軸之大小.
                    //MessageBox.Show();
                    //float newHeight = xf * (float)(oldYmax - oldYmin);
                    float newWidth = (float)(_oldXmax-_oldXmin) * cyf;
                    //MessageBox.Show("Using XF to adjust Y height: " + (newWidth).ToString());
                    //ca.InnerPlotPosition = new ElementPosition(ipp0.X, ipp0.Y, ipp0.Width,xf*(float)(oldYmax-oldYmin));
                    chart_Plot.ClientSize = new Size((int)(oldClientYSize * newWidth / (float)ipp0.Width), oldClientYSize);//(int)((float)oldClientXSize / (float)ipp0.Width * newHeight));
                }
                else if(cxf>cyf)
                {
                    //狀況: YF使用的刻度較多,X軸的刻度範圍小,改以X軸之大小來放大Y軸之大小.
                    //MessageBox.Show(((float)(oldXmax - oldXmin) * (float)yf).ToString());
                    //float newWidth = (float)(oldXmax - oldXmin) * yf;
                    float newHeight = (float)(_oldYmax - _oldYmin) * cxf; //得到新的Y軸之大小.
                    //MessageBox.Show("Adjust from using cxf, Original Height =" + oldClientYSize.ToString() + ", New Height = " + ((int)(oldClientXSize * newHeight / (float)ipp0.Height)).ToString());
                    //MessageBox.Show("Using YF to adjust X width: " + newHeight.ToString());
                    //ca.InnerPlotPosition = new ElementPosition(ipp0.X, ipp0.Y,(float)(oldXmax-oldXmin)*yf , ipp0.Height);
                    chart_Plot.ClientSize = new Size(oldClientXSize, (int)(oldClientXSize*newHeight/(float)ipp0.Height));//(int)((float)oldClientYSize/(float)ipp0.Height*newWidth), oldClientYSize);
                }
                else
                {
                    //隨意.
                    //chart_Plot.ChartAreas[0].AxisX.Interval=1;
                    //chart_Plot.ChartAreas[0].AxisY.Interval=1;
                }
                //chart_Plot.ChartAreas[0].RecalculateAxesScale();
                chart_Plot.SaveImage(PNGStoredFolderPath, ChartImageFormat.Png);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



                for (int i = 0; i < TempName.GetLength(0); i++)
                {
                    chart_Plot.Series[TempName[i]].IsVisibleInLegend = true;
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
                    if (!object.Equals(TT.Tag, null))
                    {
                        if (TT.Tag.ToString() == "BLOCK")
                        {
                            TT.Font = new Font("微軟正黑體", _AnnotationSize, FontStyle.Bold); ;
                        }
                    }
                }

                //恢復原狀
                //chart_Plot.ChartAreas[0] = ca;
                chart_Plot.ClientSize = OldClientSize;
                以YX比例設定座標軸(null);
                //ca.InnerPlotPosition= ipp0;
                //ca.Position=cap;

                //chart_Plot.ChartAreas[0].AxisX.Minimum=oldXmin;// -xspace/2.0;// -xspace/2.0;// - xspace;
                //chart_Plot.ChartAreas[0].AxisX.Maximum=oldXmax;// NewXmax;
                //chart_Plot.ChartAreas[0].AxisX.Interval= oldXInterval;
                //chart_Plot.ChartAreas[0].AxisY.Minimum= oldYmin;// - yspace;
                //chart_Plot.ChartAreas[0].AxisY.Maximum= oldYmax;// NewYmax;
                //chart_Plot.ChartAreas[0].AxisY.Interval= oldYInterval;
                //chart_Plot.ChartAreas[0].RecalculateAxesScale();

                //Runing the backgroundworker.
                WordOutputMsg = "ERROR";
                isExporting = true;
                bk_OutputWordReport.RunWorkerAsync(SFD_WordOutput.FileName);


                //string getMsg = 輸出Word報表(SFD_WordOutput.FileName);
            }

        }
        private void 載入Textbox到矩陣內()
        {
            RCOL = new MTExameTextBox_Object_Class();
            RCOL.填表人ID= mainForm.LoginInUserID;
            RCOL.填表人名稱= mainForm.LoginInUserName;
            RCOL.設計潮位高= textBox_設計潮位高.Text;
            RCOL.設計潮位低 = textBox_設計潮位低.Text;
            RCOL.殘留水位= textBox_殘留水位.Text;
            RCOL.平時上載荷重= textBox_平時上載荷重.Text;
            RCOL.地震時上載荷重= textBox_地震時上載荷重.Text;
            RCOL.船舶牽引力= textBox_船舶牽引力.Text;
            RCOL.陸上設計震度= textBox_陸上設計震度.Text;
            RCOL.水中設計震度= textBox_水中設計震度.Text;
            RCOL.水單位重 = textBox_rw.Text;//水單位重.
            RCOL.繫船柱突出高度= textBox_BoatColumnHeight.Text;//繫船柱突出高度.
            RCOL.背填料內摩擦角=textBox_背填料內摩擦角.Text;
            RCOL.背填料壁面摩擦角=textBox_背填料壁面摩擦角.Text;
            RCOL.背填料水平傾斜角=textBox_背填料水平傾斜角.Text;
            RCOL.入土深度=textBox_入土深度.Text;
            RCOL.拋石厚度=textBox_拋石厚度.Text;
            //RCOL.海側方向= cmb_seawaveDir.SelectedItem.ToString();
            RCOL.地盤基礎內摩擦角=textBox_地盤基礎內摩擦角.Text;
            RCOL.土壤凝聚力=textBox_土壤凝聚力.Text;
            RCOL.Nq=textBox_Nq.Text;
            RCOL.Nc = textBox_Nc.Text;
            RCOL.Nr = textBox_Nr.Text;
            RCOL.平時滑動安全係數=textBox_平時滑動安全係數.Text;
            RCOL.平時傾倒安全係數=textBox_平時傾倒安全係數.Text;
            RCOL.平時地盤承載力安全係數=textBox_平時地盤承載力安全係數.Text;
            RCOL.地震時滑動安全係數=textBox_地震時滑動安全係數.Text;
            RCOL.地震時傾倒安全係數=textBox_地震時傾倒安全係數.Text;
            RCOL.地震時地盤承載力安全係數=textBox_地震時地盤承載力安全係數.Text;
            RCOL.陸上土壤重=textBox_SoilR_Earth.Text;//土壤重(陸上)
            RCOL.水中土壤重=textBox_SoilR_Water.Text;//土壤重(水中)
            //RCOL.平時無設計震度土壓係數Ka=textBox_KaStage1.Text;//平時無設計震度土壓係數Ka
            //RCOL.地震時設計震度K017土壓係數Ka=textBox_KaStage2.Text;//地震時(設計震度K=0.17)土壓係數Ka
            //RCOL.地震時設計震度K033土壓係數Ka=textBox_KaStage3.Text;//地震時(設計震度K=0.33)土壓係數Ka
        }
        public bool IsFileLocked(FileInfo file)
        {
            if (!file.Exists)
            {
                return false;
            }
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //The file is unvaliable:
                //1. Opened by other programs.
                //2. still being written.
                //3. doesn't exist.
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return false;//File is not locked.
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
                if (mainForm.提供服務訊息)
                {
                    儲存XML專案檔(VESDStoredFolderPath, false);
                    this.mainForm.發送檔案給主機(VESDStoredFolderPath);
                    File.Delete(VESDStoredFolderPath);
                }
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
                if (mainForm.提供服務訊息)
                {
                    this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')完成碼頭檢核並輸出報表(檔案名稱為'" + f1.Name + "'),員工編號為'" + mainForm.LoginInUserID + "',員工名稱為'" + mainForm.LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                }//輸出專案檔的備份.
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
            /*
            if (File.Exists(PNGStoredFolderPath))
            {
                File.Delete(PNGStoredFolderPath);
            }
            */
        }
        private string 輸出Word報表(string getPath)
        {
            string OutMsg = "ok";
            if (object.Equals(Mod, null))
            {
                //沒有作檢核.
                //MessageBox.Show("您必須要先進行檢核才可輸出Word檔案!");
                OutMsg = "ERROR:您必須要先進行檢核才可輸出Word檔案";
                return OutMsg;
            }
            string tempplateFile = "Output_Template_MT.docx";// C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\x64\\Release\\Output_Template.docx";//!!!!!!!!!!!!!!!!!!!!!!
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
            File.Copy(tempplateFile, outputFile, true);


            Mod.Get_DataBank_Data(); //Loading Running Result data.
            


            WORD.Application wdApplication = null;
            wdApplication = new WORD.Application();
            wdApplication.Visible = false;// true;
            if (wdApplication != null)
            {
                try
                {
                    WORD.Document newDocument = wdApplication.Documents.Open(outputFile);

                    WORD.Range wdrange = null;
                    WORD.Table TableRef = null;
                    object unit = WORD.WdUnits.wdCharacter;
                    object lu = WORD.WdUnits.wdLine;
                    //object story = WORD.WdUnits.wdLine;
                    object count = 1;
                    object dcount = 2;
                    object tcount = 3;


                    //實際輸出區域.
                    //取得第一個Table:
                    //1. 填表資訊.
                    TableRef = newDocument.Tables[1];
                    //填表人員工編號.
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.填表人ID; //mainForm.LoginInUserID.ToString();
                                                                       //填表人名稱.
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.填表人名稱;//mainForm.LoginInUserName.ToString();
                    //時間.
                    TableRef.Rows[3].Cells[2].Range.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm");


                    //取得第二個Table:
                    //2.設計條件.
                    TableRef = newDocument.Tables[2];
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.設計潮位高;
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.設計潮位低;
                    TableRef.Rows[3].Cells[2].Range.Text = RCOL.殘留水位;
                    TableRef.Rows[4].Cells[2].Range.Text = RCOL.平時上載荷重;
                    TableRef.Rows[5].Cells[2].Range.Text = RCOL.地震時上載荷重;
                    TableRef.Rows[6].Cells[2].Range.Text = RCOL.船舶牽引力;
                    TableRef.Rows[7].Cells[2].Range.Text = RCOL.陸上設計震度;
                    TableRef.Rows[8].Cells[2].Range.Text = RCOL.水中設計震度;
                    TableRef.Rows[9].Cells[2].Range.Text = RCOL.水單位重;
                    TableRef.Rows[10].Cells[2].Range.Text = RCOL.繫船柱突出高度;
                    TableRef.Rows[11].Cells[2].Range.Text = RCOL.背填料內摩擦角;
                    TableRef.Rows[12].Cells[2].Range.Text = RCOL.背填料壁面摩擦角;
                    TableRef.Rows[13].Cells[2].Range.Text = RCOL.背填料水平傾斜角;
                    TableRef.Rows[14].Cells[2].Range.Text = RCOL.入土深度;
                    TableRef.Rows[15].Cells[2].Range.Text = RCOL.拋石厚度;
                    //TableRef.Rows[16].Cells[2].Range.Text = RCOL.海側方向;
                    TableRef.Rows[16].Cells[2].Range.Text = RCOL.地盤基礎內摩擦角;
                    TableRef.Rows[17].Cells[2].Range.Text = RCOL.土壤凝聚力;
                    TableRef.Rows[18].Cells[2].Range.Text = RCOL.Nc;
                    TableRef.Rows[19].Cells[2].Range.Text = RCOL.Nq;
                    TableRef.Rows[20].Cells[2].Range.Text = RCOL.Nr;
                    TableRef.Rows[21].Cells[2].Range.Text = RCOL.平時滑動安全係數;
                    TableRef.Rows[22].Cells[2].Range.Text = RCOL.平時傾倒安全係數;
                    TableRef.Rows[23].Cells[2].Range.Text = RCOL.平時地盤承載力安全係數;
                    TableRef.Rows[24].Cells[2].Range.Text = RCOL.地震時滑動安全係數;
                    TableRef.Rows[25].Cells[2].Range.Text = RCOL.地震時傾倒安全係數;
                    TableRef.Rows[26].Cells[2].Range.Text = RCOL.地震時地盤承載力安全係數;
                    TableRef.Rows[27].Cells[2].Range.Text = RCOL.陸上土壤重;
                    TableRef.Rows[28].Cells[2].Range.Text = RCOL.水中土壤重;
                    /*
                    TableRef.Rows[29].Cells[2].Range.Text = RCOL.平時無設計震度土壓係數Ka;
                    TableRef.Rows[30].Cells[2].Range.Text = RCOL.地震時設計震度K017土壓係數Ka;
                    TableRef.Rows[31].Cells[2].Range.Text = RCOL.地震時設計震度K033土壓係數Ka;
                    */
                    //TableRef.Rows[33].Cells[2].Range.Text =;

                    //取得第三個表格.
                    //3, 壁體自重及抵抗彎矩.
                    //
                    //MessageBox.Show(newDocument.Tables.Count.ToString());
                    TableRef = newDocument.Tables[3];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++) // Mod.EL_SectionResult; i++)
                    {
                        int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        int needsize = BlockID.GetLength(0);
                        if (i != 0)
                        { needsize += 2; }
                        for (int i2 = 0; i2 < needsize; i2++)
                        {
                            TableRef.Rows.Add(TableRef.Rows[2]);
                        }
                    }

                    int rowstart = 2;
                    int rowend = 2;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0);i++)
                    {
                        int[] BlocKID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart + BlocKID.GetLength(0) - 1;
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }

                        TableRef.Rows[rowend].Cells[1].Range.Text = "EL" + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        if(i!=0)
                        {
                            //前統計.
                            //壁體自重.
                            TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_W.ToString("0.00");
                            //力矩.
                            TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].pre_total_arm.ToString("0.00");
                            //抵抗彎矩
                            TableRef.Rows[rowstart].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_Mx.ToString("0.00");
                            for(int ii=1;ii<=7;ii++)
                            {
                                TableRef.Rows[rowstart].Cells[ii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                            rowstart = rowstart + 1;
                        }
                        TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);
                        for (int ii = 0; ii <= BlocKID.GetUpperBound(0); ii++)
                        {
                            //各個Block的結果.
                            int blockidGet = BlocKID[ii]-1;
                            TableRef.Rows[rowstart + ii].Cells[2].Range.Text = BlockListSubScriptToName[blockidGet];
                            
                            //面積
                            TableRef.Rows[rowstart + ii].Cells[3].Range.Text = Mod.VarBank .Block_Out [blockidGet].A.ToString("0.00");

                            //單位體重
                            TableRef.Rows[rowstart + ii].Cells[4].Range.Text = Mod.VarBank.Block_Out[blockidGet].Density.ToString("0.00");

                            //壁體自重
                            //Selfweight
                            TableRef.Rows[rowstart + ii].Cells[5].Range.Text = Mod.VarBank.Block_Out[blockidGet].Selfweight.ToString("0.00");

                            //力矩
                            //X
                            TableRef.Rows[rowstart + ii].Cells[6].Range.Text = Mod.VarBank.Block_Out[blockidGet].X.ToString("0.00");


                            //抵抗彎矩
                            //Mw
                            TableRef.Rows[rowstart + ii].Cells[7].Range.Text = Mod.VarBank.Block_Out[blockidGet].Mw.ToString("0.00");
                            for (int iii = 1; iii <= 7; iii++)
                            {
                                TableRef.Rows[rowstart+ii].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                        }
                        //後統計.
                        TableRef.Rows[rowend].Cells[2].Range.Text = "計";
                        TableRef.Rows[rowend].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_W.ToString("0.00");
                        //力矩.
                        TableRef.Rows[rowend].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_total_arm.ToString("0.00");
                        //抵抗彎矩
                        TableRef.Rows[rowend].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Mx.ToString("0.00");

                        rowstart = rowend + 1;
                    }


                    //忽略整體統計資訊.
                    //壁體自重.
                    //TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.W.ToString("0.00");

                    //int minuscount = 0;
                    /*
                    rowstart = 2;
                    rowend = 2;
                    
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0);i++)
                    {
                        int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart + BlockID.GetLength(0) - 1;
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }

                        TableRef.Columns[1].Cells[rowstart-minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                        minuscount += (rowend-rowstart+1 - 1);
                        rowstart = rowend + 1;
                    }
                    */

                    wdrange = TableRef.Columns[1].Cells[rowstart - 1].Range;// - minuscount].Range;
                    wdrange.Select();
                    wdApplication.Selection.MoveDown(ref lu, 1, Type.Missing);
                    //插入圖.
                    wdApplication.Selection.InlineShapes.AddPicture(PNGStoredFolderPath);


                    //取得第四個表格.
                    //4. 地震力及傾倒彎矩
                    TableRef = newDocument.Tables[4];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++) // Mod.EL_SectionResult; i++)
                    {
                        int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        int needsize = BlockID.GetLength(0);
                        if (i != 0)
                        { needsize += 2; }
                        for (int i2 = 0; i2 < needsize; i2++)
                        {
                            TableRef.Rows.Add(TableRef.Rows[2]);
                        }
                    }

                    rowstart = 2;
                    rowend = 2;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        int[] BlocKID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart + BlocKID.GetLength(0) - 1;
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }
                        TableRef.Rows[rowend].Cells[1].Range.Text = "EL" + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        if (i != 0)
                        {
                            //前統計.
                            //地震力.
                            TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_WE.ToString("0.00");
                            //力矩.
                            TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].pre_total_armE.ToString("0.00");
                            //抵抗彎矩
                            TableRef.Rows[rowstart].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_MxE.ToString("0.00");
                            for (int iii = 1; iii <= 7; iii++)
                            {
                                TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                            rowstart = rowstart + 1;
                        }
                        TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);
                        for (int ii = 0; ii <= BlocKID.GetUpperBound(0); ii++)
                        {
                            //各個Block的結果.
                            int blockidGet = BlocKID[ii] - 1;
                            TableRef.Rows[rowstart + ii].Cells[2].Range.Text = BlockListSubScriptToName[blockidGet];

                            //面積
                            TableRef.Rows[rowstart + ii].Cells[3].Range.Text = Mod.VarBank.Block_Out[blockidGet].A.ToString("0.00");

                            //單位體重
                            TableRef.Rows[rowstart + ii].Cells[4].Range.Text = Mod.VarBank.Block_Out[blockidGet].EQ_Density.ToString("0.00");

                            //地震力
                            TableRef.Rows[rowstart + ii].Cells[5].Range.Text = Mod.VarBank.Block_Out[blockidGet].Selfweight_E.ToString("0.00");

                            //力矩
                            //Y
                            TableRef.Rows[rowstart + ii].Cells[6].Range.Text = Mod.VarBank.Block_Out[blockidGet].X_E.ToString("0.00");


                            //傾倒彎矩
                            //Mw
                            TableRef.Rows[rowstart + ii].Cells[7].Range.Text = Mod.VarBank.Block_Out[blockidGet].Mw_E.ToString("0.00");
                            for (int iii = 1; iii <= 7; iii++)
                            {
                                TableRef.Rows[rowstart + ii].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                        }
                        //後統計.
                        TableRef.Rows[rowend].Cells[2].Range.Text = "計";
                        TableRef.Rows[rowend].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_WE.ToString("0.00");
                        //力矩.
                        TableRef.Rows[rowend].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_total_armE.ToString("0.00");
                        //抵抗彎矩
                        TableRef.Rows[rowend].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_MxE.ToString("0.00");

                        rowstart = rowend + 1;
                    }


                    //忽略整體統計資訊.
                    //壁體自重.
                    //TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.W.ToString("0.00");
                    /*
                    rowstart = 2;
                    rowend = 2;
                    minuscount = 0;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart + BlockID.GetLength(0) - 1;
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }

                        TableRef.Columns[1].Cells[rowstart - minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                        minuscount += (rowend - rowstart + 1 - 1);
                        rowstart = rowend + 1;
                    }
                    */


                    //取得第五個表格.
                    //5. 土壓力水平分力及傾倒彎矩.
                    //  土壓強度(平時).
                    //TableRef.Rows[29].Cells[2].Range.Text = RCOL.平時無設計震度土壓係數Ka;
                    //TableRef.Rows[30].Cells[2].Range.Text = RCOL.地震時設計震度K017土壓係數Ka;
                    //TableRef.Rows[31].Cells[2].Range.Text = RCOL.地震時設計震度K033土壓係數Ka;
                    TableRef = newDocument.Tables[5];
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.ka_0.ToString("0.00");// RCOL.平時無設計震度土壓係數Ka;


                    //第六個表格.
                    //  土壓強度(地震時 陸上)
                    // 
                    TableRef = newDocument.Tables[6];
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.陸上設計震度;
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.ka_up.ToString("0.00"); //RCOL.地震時設計震度K017土壓係數Ka;


                    //第七個表格.
                    //  土壓強度(地震時 水中)
                    TableRef = newDocument.Tables[7];
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.水中設計震度;//(0.33).ToString("0.00");
                    TableRef.Rows[2].Cells[2].Range.Text = Mod.VarBank.ka_down.ToString("0.00"); //RCOL.地震時設計震度K033土壓係數Ka;

                    //第八個表格.
                    //土壓水平力及傾倒彎矩.
                    //公式表.


                    //第九個表格.
                    //   整理表.
                    TableRef = newDocument.Tables[9];
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        //int[] BlockID = Mod.VarBank.EL_Out[i].BlockID;
                        int needsize = 1;// BlockID.GetLength(0);
                        if (i != 0)
                        { needsize += 2; }
                        for (int i2 = 0; i2 < needsize; i2++)
                        {
                            TableRef.Rows.Add(TableRef.Rows[3]);
                        }
                    }
                    //填入數據.
                    rowstart = 3;
                    rowend = 3;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        rowend = rowstart;
                        if(i==0){rowend = rowend + 1;}
                        else { rowend = rowend + 2; }
                        TableRef.Rows[rowend].Cells[1].Range.Text = "EL" + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        if(i!=0)
                        {
                            //填入前統計.
                            //平時 與 地震時.
                            //   水平分力 力矩  傾倒彎矩
                            TableRef.Rows[rowstart].Cells[3].Range.Text= Mod.VarBank.EL_Out[i].pre_sum_Fh.ToString("0.00");//水平分力  平時
                            TableRef.Rows[rowstart].Cells[4].Range.Text= Mod.VarBank.EL_Out[i].pre_total_Fhy.ToString("0.00");//力矩      平時
                            TableRef.Rows[rowstart].Cells[5].Range.Text= Mod.VarBank.EL_Out[i].pre_sum_FhMh.ToString("0.00");//傾倒彎矩  平時
                            TableRef.Rows[rowstart].Cells[6].Range.Text= Mod.VarBank.EL_Out[i].pre_sum_Fh_E.ToString("0.00");//水平分力  地震時
                            TableRef.Rows[rowstart].Cells[7].Range.Text= Mod.VarBank.EL_Out[i].pre_total_Fhy_E.ToString("0.00");//力矩      地震時
                            TableRef.Rows[rowstart].Cells[8].Range.Text= Mod.VarBank.EL_Out[i].pre_sum_FhMh_E.ToString("0.00");//傾倒彎矩  地震時
                            for (int iii = 1; iii <= 8; iii++)
                            {
                                TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                            rowstart = rowstart + 1;
                        }
                        //填入數據.
                        TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);

                        //平時-水平分力.
                        TableRef.Rows[rowstart].Cells[3].Range.Text= Mod.VarBank.EL_Out[i].Fh.ToString("0.00");
                        //平時-力矩
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Fh_y.ToString("0.00");
                        //平時-傾倒彎矩
                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Fh_Mh.ToString("0.00");

                        //地震時-水平分力.
                        TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Fh_E.ToString("0.00");
                        //地震時-力矩
                        TableRef.Rows[rowstart].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Fh_y_E.ToString("0.00");
                        //地震時-傾倒彎矩.
                        TableRef.Rows[rowstart].Cells[8].Range.Text = Mod.VarBank.EL_Out[i].Fh_Mh_E.ToString("0.00");
                        for (int iii = 1; iii <= 8; iii++)
                        {
                            TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                        }

                        rowstart = rowstart + 1;
                        //後統計
                        TableRef.Rows[rowstart].Cells[2].Range.Text = "計";
                        TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fh.ToString("0.00");//水平分力  平時
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Level_total_Fhy.ToString("0.00");//力矩      平時
                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FhMh.ToString("0.00");//傾倒彎矩  平時
                        TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fh_E.ToString("0.00");//水平分力  地震時
                        TableRef.Rows[rowstart].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_total_Fhy_E.ToString("0.00");//力矩      地震時
                        TableRef.Rows[rowstart].Cells[8].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FhMh_E.ToString("0.00");//傾倒彎矩  地震時

                        rowstart = rowend + 1;
                    }
                    /*
                    rowstart = 3;
                    rowend = 3;
                    minuscount = 0;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        //int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart;// + BlockID.GetLength(0) - 1;
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }

                        TableRef.Columns[1].Cells[rowstart - minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                        minuscount += (rowend - rowstart + 1 - 1);
                        rowstart = rowend + 1;
                    }
                    */
                    //TableRef.Rows[1].Cells[5].Merge(TableRef.Rows[1].Cells[7]);
                    //TableRef.Rows[1].Cells[2].Merge(TableRef.Rows[1].Cells[4]);
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);
                    //TableRef.Columns[2].Cells[1].Merge(TableRef.Columns[2].Cells[2]);
                    //*/

                    //第十個表格.
                    //6. 土壓垂直分力及抵抗彎矩.
                    //  表投(垂直分力)
                    TableRef = newDocument.Tables[10];



                    //第十一個表格.
                    //  整理表.
                    TableRef = newDocument.Tables[11];
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0)-1;i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[3]);
                    }
                    //填入數據.
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0);i++)
                    {
                        TableRef.Rows[3 + i].Cells[1].Range.Text = 得到英文碼(i + 1);// (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[3 + i].Cells[2].Range.Text =Mod.VarBank.EL_Out[i].Level_sum_Fv.ToString("0.00");//垂直分力-平時.
                        TableRef.Rows[3 + i].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_total_Fvx.ToString("0.00");//力臂-平時.
                        TableRef.Rows[3 + i].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FvMv.ToString("0.00");//抵抗彎矩-平時.
                        TableRef.Rows[3 + i].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fv_E.ToString("0.00");//垂直分力-平時.
                        TableRef.Rows[3 + i].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_total_Fvx_E.ToString("0.00");//力臂-平時.
                        TableRef.Rows[3 + i].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FvMv_E.ToString("0.00");//抵抗彎矩-平時.
                    }

                    //第十二個表格.
                    //7. 殘留水壓及傾倒彎矩.
                    //  表頭.
                    TableRef = newDocument.Tables[12];


                    //第十三個表格.
                    //  整理表.
                    TableRef = newDocument.Tables[13];
                    //MessageBox.Show(RCOL.殘留水位);
                    //MessageBox.Show(Mod.VarBank.EL_Out[0].EL.ToString());
                    bool UsePre = false;
                    for (int i=0;i<Mod.VarBank.EL_Out.GetLength(0);i++)
                    {
                        int needsize = 1;
                        if(i==0 && Mod.VarBank.EL_Out[i].EL>double.Parse(RCOL.殘留水位 ))
                        {
                            //MessageBox.Show("Passing");
                            //不安插新的.
                            continue;
                        }
                        if (i != 0 && Mod.VarBank.EL_Out[i].EL<=double.Parse(RCOL.殘留水位))
                        {
                            //若不須算 就只插入一格: needsize=1.
                            if (!UsePre)
                            {
                                needsize += 1;
                                UsePre = true;
                            }
                            else
                            {
                                needsize += 2;
                            }
                        }
                        for (int i2 = 0; i2 < needsize; i2++)
                        {
                            TableRef.Rows.Add(TableRef.Rows[2]);
                        }
                        //TableRef.Rows.Add(TableRef.Rows[2]);
                    }
                    //填入數據.
                    rowstart = 2;
                    rowend = 2;
                    UsePre = false;
                    for (int i=0;i<Mod.VarBank.EL_Out.GetLength(0);i++)
                    {

                        rowend = rowstart;

                        if(Mod.VarBank.EL_Out[i].EL > double.Parse(RCOL.殘留水位))
                        {
                            TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);//= (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                            rowstart += 1;
                            //MessageBox.Show("Passing 2");
                            continue;
                        }
                        else if(i!=0)
                        {
                            rowend = rowend + 2;
                        }
                        else if(i==0)
                        {
                            rowend = rowend + 1;
                        }
                        if (!UsePre) { rowend -= 1;}

                        
                        TableRef.Rows[rowend].Cells[1].Range.Text = "EL" + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        //前統計.
                        if (i!=0 && UsePre)
                        {
                            //殘留水壓
                            TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_Fw.ToString("0.00");//水平分力  平時

                            //力矩
                            TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].pre_total_Fwy.ToString("0.00");//力矩      平時

                            //傾倒彎矩
                            TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].pre_sum_FwMw.ToString("0.00");//傾倒彎矩  平時
                            for (int iii = 1; iii <= 5; iii++)
                            {
                                TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                            }
                            rowstart = rowstart + 1;
                        }
                        if (!UsePre) UsePre = true;

                        TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);
                        //殘留水壓
                        TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Fw_sum.ToString("0.00");

                        //力矩
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Fw_y.ToString("0.00");
                        //傾倒彎矩.
                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Fw_Mw_sum.ToString("0.00");
                        for (int iii = 1; iii <= 5; iii++)
                        {
                            TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                        }
                        rowstart = rowstart + 1;

                        //後統計.
                        //殘留水壓
                        TableRef.Rows[rowstart].Cells[2].Range.Text = "計";
                        TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fw.ToString("0.00");

                        //力矩
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Level_total_Fwy.ToString("0.00");
                        //傾倒彎矩.

                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FwMw.ToString("0.00");
                        rowstart = rowend + 1;
                    }

                    /*
                    rowstart = 2;
                    rowend = 2;
                    minuscount = 0;
                    UsePre = false;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        //int[] BlockID = Mod.VarBank.EL_Out[i].BlockId;
                        rowend = rowstart;// + BlockID.GetLength(0) - 1;
                        if (Mod.VarBank.EL_Out[i].EL > double.Parse(RCOL.殘留水位))
                        {
                            rowstart += 1;
                            continue;
                        }
                        if (i == 0) { rowend = rowend + 1; }
                        else { rowend = rowend + 2; }
                        if (!UsePre) { rowend -= 1;UsePre = true; }

                        TableRef.Columns[1].Cells[rowstart - minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                        minuscount += (rowend - rowstart + 1 - 1);
                        rowstart = rowend + 1;
                    }
                    */


                    //第十四個表格.
                    //8. 船舶牽引力及傾倒彎矩.
                    TableRef = newDocument.Tables[14];
                    TableRef.Rows[1].Cells[2].Range.Text = RCOL.船舶牽引力;
                    TableRef.Rows[2].Cells[2].Range.Text = RCOL.繫船柱突出高度;


                    //第十六個表格.
                    //  整理表.
                    TableRef = newDocument.Tables[15];
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0)-1;i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[2]);
                    }
                    //填入數據.
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0);i++)
                    {
                        TableRef.Rows[2 + i].Cells[1].Range.Text = 得到英文碼(i+1);
                        TableRef.Rows[2+i].Cells[2].Range.Text = RCOL.船舶牽引力;
                        TableRef.Rows[2 + i].Cells[3].Range.Text= Mod.VarBank.EL_Out[i].Ft_y.ToString("0.00");//力矩.
                        TableRef.Rows[2 + i].Cells[4].Range.Text= Mod.VarBank.EL_Out[i].Ft_Mt.ToString("0.00");//傾倒彎矩.
                    }

                    //第十七個表格.
                    //9. 垂直力及抵抗彎矩 總和表
                    //    9-1. 平時 整理表
                    TableRef = newDocument.Tables[16];
                    for(int i=0;i<Mod.VarBank.EL_Out.GetLength(0)-1;i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[3]);
                    }
                    //填入數據.
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        TableRef.Rows[3 + i].Cells[1].Range.Text = 得到英文碼(i + 1);// (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[3 + i].Cells[2].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_W.ToString("0.00");//垂直力-壁體自重.
                        TableRef.Rows[3 + i].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fv.ToString("0.00");//垂直力-垂直土壓.
                        TableRef.Rows[3 + i].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].VForcesum.ToString("0.00");//垂直力 -計.
                        TableRef.Rows[3 + i].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Mx.ToString("0.00");//抵抗彎矩-壁體自重.
                        TableRef.Rows[3 + i].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FvMv.ToString("0.00");//抵抗彎矩-垂直土壓.
                        TableRef.Rows[3 + i].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].VMomentsum.ToString("0.00");//抵抗彎矩-計.
                    }
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);

                    //第十七個表格.
                    //    9-2.地震時 整理表.
                    TableRef = newDocument.Tables[17];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0)-1; i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[3]);
                    }
                    //填入數據.
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        //"EL " +
                        TableRef.Rows[3 + i].Cells[1].Range.Text = 得到英文碼(i + 1);// (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[3 + i].Cells[2].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_W.ToString("0.00");//垂直力-壁體自重.
                        TableRef.Rows[3 + i].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fv_E.ToString("0.00");//垂直力-垂直土壓.
                        TableRef.Rows[3 + i].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].VForcesum_E.ToString("0.00");//垂直力 -計.
                        TableRef.Rows[3 + i].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Mx.ToString("0.00");//抵抗彎矩-壁體自重.
                        TableRef.Rows[3 + i].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FvMv_E.ToString("0.00");//抵抗彎矩-垂直土壓.
                        TableRef.Rows[3 + i].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].VMomentsum_E.ToString("0.00");//抵抗彎矩-計.
                    }
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);

                    //第十八個表格.
                    //  10. 水平力及傾倒彎矩 總和表.
                    //   10-1. 平時.
                    TableRef = newDocument.Tables[18];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0)-1; i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[3]);
                    }
                    //填入數據.
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        TableRef.Rows[3 + i].Cells[1].Range.Text =  得到英文碼(i+1);// (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[3 + i].Cells[2].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fh.ToString("0.00");//水平力-土壓
                        TableRef.Rows[3 + i].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fw.ToString("0.00");//水平力-殘留水壓.
                        TableRef.Rows[3 + i].Cells[4].Range.Text = RCOL.船舶牽引力;//水平力-牽引力.
                        TableRef.Rows[3 + i].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].HForcesum.ToString("0.00");//水平力-計.
                        TableRef.Rows[3 + i].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FhMh.ToString("0.00");//傾倒彎矩-土壓.
                        TableRef.Rows[3 + i].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FwMw.ToString("0.00");//傾倒彎矩-殘留水壓.
                        TableRef.Rows[3 + i].Cells[8].Range.Text = Mod.VarBank.EL_Out[i].Ft_Mt.ToString("0.00");//傾倒彎矩-牽引力.
                        TableRef.Rows[3 + i].Cells[9].Range.Text = Mod.VarBank.EL_Out[i].HMomentsum.ToString("0.00");//傾倒彎矩-計.

                    }
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);


                    //第十九個表格.
                    //   10-2. 地震時.
                    TableRef = newDocument.Tables[19];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0)-1; i++)
                    {
                        TableRef.Rows.Add(TableRef.Rows[3]);
                    }
                    //填入數據.
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        TableRef.Rows[3 + i].Cells[1].Range.Text = 得到英文碼(i + 1);//(Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[3 + i].Cells[2].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fh_E.ToString("0.00");//水平力-土壓
                        TableRef.Rows[3 + i].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_Fw.ToString("0.00");//水平力-殘留水壓.
                        TableRef.Rows[3 + i].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_WE.ToString("0.00");//水平力-牽引力.
                        TableRef.Rows[3 + i].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].HForcesum_E.ToString("0.00");//水平力-計.
                        TableRef.Rows[3 + i].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FhMh_E.ToString("0.00");//傾倒彎矩-土壓.
                        TableRef.Rows[3 + i].Cells[7].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_FwMw.ToString("0.00");//傾倒彎矩-殘留水壓.
                        TableRef.Rows[3 + i].Cells[8].Range.Text = Mod.VarBank.EL_Out[i].Level_sum_MxE.ToString("0.00");//傾倒彎矩-地震力.
                        TableRef.Rows[3 + i].Cells[9].Range.Text = Mod.VarBank.EL_Out[i].HMomentsum_E.ToString("0.00");//傾倒彎矩-計.
                    }
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);

                    //第二十個表格.
                    //11. 壁體安全檢查.
                    //    11-1. 滑動及傾倒安定檢核.
                    //       11-1-1. 安全係數表.
                    TableRef = newDocument.Tables[20];

                    //第二十一個表格.
                    //       11-1-2. 總表.
                    TableRef = newDocument.Tables[21];
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        int needsize = 1;
                        if(i>0)
                        {
                            needsize += 1;
                        }
                        for (int i2 = 0; i2 < needsize; i2++)
                        {
                            TableRef.Rows.Add(TableRef.Rows[3]);
                        }
                    }
                    rowstart = 3;
                    rowend = 3;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        rowend = rowstart + 1;
                        //TableRef.Rows[rowend].Cells[1].Range.Text = (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[rowstart].Cells[1].Range.Text = 得到英文碼(i + 1);
                        for(int iii=1;iii<=6;iii++)
                        {
                            TableRef.Rows[rowstart].Cells[iii].Range.Borders[WORD.WdBorderType.wdBorderBottom].LineStyle = WORD.WdLineStyle.wdLineStyleNone;
                        }
                        TableRef.Rows[rowstart].Cells[2].Range.Text = "滑動";
                        TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].SF_slide.ToString("0.00");
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].SF_slide >= double.Parse(RCOL.平時滑動安全係數) ? "OK" : "NG";
                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].SF_slide_E.ToString("0.00");
                        TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].SF_slide_E >= double.Parse(RCOL.地震時滑動安全係數) ? "OK" : "NG";

                        rowstart += 1;
                        //TableRef.Rows[rowstart].Cells[1].Range.Text = "EL " + (Mod.VarBank.EL_Out[i].EL >= 0 ? "+" : "-") + Math.Abs(Mod.VarBank.EL_Out[i].EL).ToString();
                        TableRef.Rows[rowstart].Cells[2].Range.Text = "傾倒";
                        TableRef.Rows[rowstart].Cells[3].Range.Text = Mod.VarBank.EL_Out[i].SF_overturning.ToString("0.00");
                        TableRef.Rows[rowstart].Cells[4].Range.Text = Mod.VarBank.EL_Out[i].SF_overturning >= double.Parse(RCOL.平時傾倒安全係數) ? "OK" : "NG";
                        TableRef.Rows[rowstart].Cells[5].Range.Text = Mod.VarBank.EL_Out[i].SF_overturning_E.ToString("0.00");
                        TableRef.Rows[rowstart].Cells[6].Range.Text = Mod.VarBank.EL_Out[i].SF_overturning_E >= double.Parse(RCOL.地震時傾倒安全係數) ? "OK" : "NG";
                        rowstart = rowend + 1;
                    }

                    //Merge.
                    /*
                    rowstart = 3;
                    rowend = 3;
                    minuscount = 0;
                    for (int i = 0; i < Mod.VarBank.EL_Out.GetLength(0); i++)
                    {
                        rowend = rowstart+1;
                                               
                        TableRef.Columns[1].Cells[rowstart - minuscount].Merge(TableRef.Columns[1].Cells[rowend - minuscount]);
                        minuscount += (rowend - rowstart + 1 - 1);
                        rowstart = rowend + 1;
                    }
                    */
                    //TableRef.Rows[1].Cells[5].Merge(TableRef.Rows[1].Cells[7]);
                    //TableRef.Rows[1].Cells[2].Merge(TableRef.Rows[1].Cells[4]);
                    //TableRef.Columns[1].Cells[1].Merge(TableRef.Columns[1].Cells[2]);
                    //TableRef.Columns[2].Cells[1].Merge(TableRef.Columns[2].Cells[2]);

                    //第二十二個表格.
                    //    11-2. 地盤承載力檢核.
                    //       11-2-1. 壁體底部反力計算.
                    TableRef = newDocument.Tables[22];
                    TableRef.Columns[2].Cells[2].Range.Text =Mod.VarBank.X.ToString("0.00");//平時 合力作用點X.
                    TableRef.Columns[3].Cells[2].Range.Text = Mod.VarBank.X_E.ToString("0.00");//平時 合力作用點X.
                    TableRef.Columns[2].Cells[3].Range.Text = Mod.VarBank.B.ToString("0.00");
                    TableRef.Columns[3].Cells[3].Range.Text = Mod.VarBank.B.ToString("0.00");
                    TableRef.Columns[2].Cells[4].Range.Text = Mod.VarBank.e.ToString("0.00");//平時 偏心量e.
                    TableRef.Columns[3].Cells[4].Range.Text = Mod.VarBank.e_E.ToString("0.00");//地震時 偏心量e.
                    if (Mod.VarBank.e <= Mod.VarBank.B/6.0)
                    {
                        TableRef.Columns[2].Cells[5].Range.Text = "e<=(b/6)";// + (Mod.VarBank.B/6.0).ToString("0.00");//
                    }
                    else
                    {
                        TableRef.Columns[2].Cells[5].Range.Text = "e>(b/6)";// + (Mod.VarBank.B / 6.0).ToString("0.00");
                    }
                    if (Mod.VarBank.e_E <= Mod.VarBank.B / 6.0)
                    {
                        TableRef.Columns[3].Cells[5].Range.Text = "e<=(b/6)";// + (Mod.VarBank.B / 6.0).ToString("0.00");//
                    }
                    else
                    {
                        TableRef.Columns[3].Cells[5].Range.Text = "e>(b/6)";// + (Mod.VarBank.B / 6.0).ToString("0.00");
                    }
                    //最大反力.
                    TableRef.Columns[2].Cells[6].Range.Text ="最大反力P1=" + Environment.NewLine + Mod.VarBank.P1.ToString("0.00") + "(t/m^2)";
                    TableRef.Columns[3].Cells[6].Range.Text = "最大反力P1=" + Environment.NewLine + Mod.VarBank.P1_E.ToString("0.00") + "(t/m^2)";

                    //最小反力.
                    TableRef.Columns[2].Cells[7].Range.Text = "最小反力P2=" + Environment.NewLine + Mod.VarBank.P2.ToString("0.00") + "(t/m^2)";
                    TableRef.Columns[3].Cells[7].Range.Text = "最小反力P2=" + Environment.NewLine + Mod.VarBank.P2_E.ToString("0.00") + "(t/m^2)";

                    //壁體底部反力分布寬B'
                    TableRef.Columns[2].Cells[8].Range.Text = "壁體底部反力分佈寬=" + Environment.NewLine + Mod.VarBank.bplum .ToString("0.00") + "(m)";
                    TableRef.Columns[3].Cells[8].Range.Text = "壁體底部反力分佈寬=" + Environment.NewLine + Mod.VarBank.bplum_E.ToString("0.00") + "(m)";

                   
                    //

                    //第二十三個表格.
                    //       11-2-2. 基礎拋石底面容許承載力計算.
                    TableRef = newDocument.Tables[23];
                    //偏心傾斜荷重角度.
                    TableRef.Columns[2].Cells[2].Range.Text = Mod.VarBank.sita.ToString("0.00") + "°";
                    TableRef.Columns[3].Cells[2].Range.Text = Mod.VarBank.sita_E.ToString("0.00") + "°";

                    //基礎拋石底面反力分布寬B''.
                    TableRef.Columns[2].Cells[3].Range.Text = "b''=" + Mod.VarBank.b_2plum.ToString("0.00") + "(m)";
                    TableRef.Columns[3].Cells[3].Range.Text = "b''=" + Mod.VarBank.b_2plum_E.ToString("0.00") + "(m)";

                    //基礎拋石底面反力R1及R2.
                    TableRef.Columns[2].Cells[4].Range.Text = "R1= " + Mod.VarBank.R1.ToString("0.00") + "(t/m^2)" + Environment.NewLine + "R2= " + Mod.VarBank.R2.ToString("0.00") + "(t/m^2)";
                    TableRef.Columns[3].Cells[4].Range.Text = "R1= " + Mod.VarBank.R1_E.ToString("0.00") + "(t/m^2)" + Environment.NewLine + "R2= " + Mod.VarBank.R2_E.ToString("0.00") + "(t/m^2)";


                    //基礎地盤之容許承載力Qa.
                    TableRef.Columns[2].Cells[5].Range.Text = "Qu=" + Mod.VarBank.Qu.ToString("0.00") + "(t/m^2)" + Environment.NewLine + "qa=" + Mod.VarBank.qa.ToString("0.00") + "(t/m^2)";
                    TableRef.Columns[3].Cells[5].Range.Text = "Qu=" + Mod.VarBank.Qu_E .ToString("0.00") + "(t/m^2)" + Environment.NewLine + "qa=" + Mod.VarBank.qa_E.ToString("0.00") + "(t/m^2)";

                    //判定.
                    if(Mod.VarBank.qa>=Mod.VarBank.R1)
                    {
                        TableRef.Columns[2].Cells[6].Range.Text = "OK";
                    }
                    else
                    {
                        TableRef.Columns[2].Cells[6].Range.Text = "FAIL";
                    }
                    if (Mod.VarBank.qa_E >= Mod.VarBank.R1)
                    {
                        TableRef.Columns[3].Cells[6].Range.Text = "OK";
                    }
                    else
                    {
                        TableRef.Columns[3].Cells[6].Range.Text = "FAIL";
                    }

                    newDocument.Save(); // (outputFile);
                    newDocument.Close(false, Type.Missing, Type.Missing);
                    OutMsg = "ok";
                    //return OutMsg;
                }
                catch(Exception ex)
                {
                    //輸出Word報表ToolStripMenuItem_Click

                       OutMsg = "ERROR:WORD處理出現錯誤" + Environment.NewLine + ex.StackTrace.ToString() + Environment.NewLine + ex.Message.ToString();
                }
            }
            else
            {
                OutMsg = "ERROR:WORD APP無法成功開啟";
            }
            wdApplication.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(wdApplication);

            return OutMsg;
        }
        private string 得到英文碼(int inputI)
        {
            if(inputI<=20)
            {
                switch(inputI)
                {
                    case 1:
                        return "I";
                        //break;
                    case 2:
                        return "II";
                    case 3:
                        return "III";
                    case 4:
                        return "IV";
                    case 5:
                        return "V";
                    case 6:
                        return "VI";
                    case 7:
                        return "VII";
                    case 8:
                        return "VIII";
                    case 9:
                        return "IX";
                    case 10:
                        return "X";
                    case 11:
                        return "XI";
                    case 12:
                        return "XII";
                    case 13:
                        return "XIII";
                    case 14:
                        return "XIV";
                    case 15:
                        return "XV";
                    case 16:
                        return "XVI";
                    case 17:
                        return "XVII";
                    case 18:
                        return "XVIII";
                    case 19:
                        return "XIX";
                    case 20:
                        return "XX";
                     default:
                        return "";
                }
            }
            else
            {
                return "";
            }
        }
        #endregion
        #region 輸出控制區域
            bool hh = false;
        private void textBox_CheckMessageShow_KeyDown(object sender, KeyEventArgs e)
        {
            //檢核頁面顯示計算訊息的textbox,限制只能按下Ctrl+C[複製],Ctrl+A[全選]. 
            //
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode == Keys.A)
            {
                //MessageBox.Show("Yes");
                ((TextBox)sender).SelectAll();
                e.Handled = true;
                hh = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
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

        private void textBox_CheckMessageShow_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        #endregion
        #region Chart互動區
        //刪除BLOCK.
        private void btnRemoveSects_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }

            if (MessageBox.Show("您確定要刪除此Block嗎?", "刪除Block", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
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
            if (BlockMainArray.GetLength(0) == 0)
            {
                ELDGV1.Enabled = false;
            }
            //更新Position開始之後的Index.
            for (int i = 0; i < listBox_SectSetting.Items.Count; i++)
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
            if (listBox_SectSetting.Items.Count == 0)
            {
                btnRemoveSects.Enabled = false;
                開始檢核ToolStripMenuItem.Enabled = false;
                btn_Test.Enabled = false;
                blockLabels設定ToolStripMenuItem.Enabled = false;
            }


            BlockCount = listBox_SectSetting.Items.Count;
            
            if (!_調整Chart比例)
            {
                取得YX比例(null);
                繪上EL();
            }
            else
            {
                以YX比例設定座標軸(null);
            }
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
        //編輯Block.
        private void btn_ModifiedBlock_Click(object sender, EventArgs e)
        {
            if (isExporting || isCalc)
            { return; }

            if (object.Equals(selectname, null))
            {
                return;
            }

            //點按修改此Block按鈕之後.
            InterfaceBlock = null;
            Form_BlockNameAndCorrdinate form_blockNameAnsCoordinate = new Form_BlockNameAndCorrdinate(this, BlockMainArray[BlockNameToListSubScript[selectname]], BlockNameToListSubScript);
            form_blockNameAnsCoordinate.ShowDialog();


            if (object.Equals(InterfaceBlock, null))
            {
                //Nothing
                return;
            }

            //變更Chart的名稱
            int oldpos = BlockNameToListSubScript[selectname];
            if (InterfaceBlock.名稱 != selectname)
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
            string NewName = InterfaceBlock.名稱;

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
            
            if (!_調整Chart比例)
            {
                取得YX比例(null);
                繪上EL();
            }
            else
            {
                以YX比例設定座標軸(null);
            }

            //修改Listbox.
            listBox_SectSetting.Items[oldpos] = InterfaceBlock.名稱 + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[oldpos]);// "(" + BlockMainArray[oldpos].單位體積重量.ToString() + ")";
        }
        private void cmb_ShowOnBlockListChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            //呈現方式變更.
            if (BlockMainArray.GetLength(0) == 0)
            {
                return;
            }
            for (int i = 0; i < listBox_SectSetting.Items.Count; i++)
            {
                listBox_SectSetting.Items[i] = BlockListSubScriptToName[i] + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[i]);
            }
            //完成.
        }
        private void listBox_SectSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Listbox點選變更時的設定
            if (listBox_SectSetting.SelectedIndex != -1)
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
                //propertyGrid_Block.SelectedObject = null;
                //ReferencedMaterialCHKL.Items.Clear();
                btn_ModifiedBlock.Enabled = false;
                btnRemoveSects.Enabled = false;
            }
        }

        //EL設定.
        bool EscapeELDGV1CellChange = false;
        private void ELDGV1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (EscapeELDGV1CellChange)
            {
                EscapeELDGV1CellChange = false;
                return;
            }
            繪上EL();
        }
        private void ELDGV1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //若目前沒有Block,禁止新增.
            if (BlockMainArray.GetLength(0) == 0)
            {
                //刪除.
                ELDGV1.Rows.RemoveAt(e.Row.Index);// addedrow)
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

        //Property Grid設定.
        private void propertyGrid_Block_Click(object sender, EventArgs e)
        {
            //Nothing.
            //MessageBox.Show("C1");
            if (listBox_SectSetting.SelectedIndex != -1)
            {
                //BBBBBBBBBBBBBBBBBBBBBBBBBBBBB
                //重新載入一次
                
                Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
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
                //BBBBBBBBBBBBBBBBBBBBBBBBBBBBB
                //重新載入一次
                
                Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
                D.可用材質 = MaterialArray;
                if (!MaterialNameToArraySubScript.ContainsKey(D.使用材質))
                { D.使用材質 = ""; }
                propertyGrid_Block.SelectedObject = D;
                
            }
        }
        private void propertyGrid_Block_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            
            updateCurrentBlockPropertyGrid();
            
        }
        private void updateCurrentBlockPropertyGrid()
        {
            if (object.Equals(selectname, null))
            {
                return;
            }
            Class_Block_MT_Interface D = (Class_Block_MT_Interface)propertyGrid_Block.SelectedObject;

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
            BlockMainArray[id].是否為混凝土塊 = D.是否為混凝土塊;
            BlockMainArray[id].使用材質 = D.使用材質;
            BlockMainArray[id].地震時單位體積重量 = D.地震時單位體積重量;
            listBox_SectSetting.Items[listBox_SectSetting.SelectedIndex] = BlockListSubScriptToName[listBox_SectSetting.SelectedIndex] + 根據選擇的呈現選項回傳Block屬性(BlockMainArray[listBox_SectSetting.SelectedIndex]);//; "(" + D.單位體積重量 + ")";

        }

        //周圍參考材質.
        private void ReferencedMaterialCHKL_SelectedIndexChanged(object sender, EventArgs e)
        {
            //更新目前選定Block的周圍參考材質之設定.
            if (object.Equals(selectname, null))
            {
                return;
            }
            if (ReferencedMaterialCHKL.Items.Count == 0)
            {
                return;
            }

            int id = BlockNameToListSubScript[selectname];
            string[] UseReferencedBlock = new string[] { };
            for (int i = 0; i < ReferencedMaterialCHKL.Items.Count; i++)
            {
                if (ReferencedMaterialCHKL.GetItemChecked(i))
                {
                    Array.Resize(ref UseReferencedBlock, UseReferencedBlock.GetLength(0) + 1);
                    UseReferencedBlock[UseReferencedBlock.GetUpperBound(0)] = ReferencedMaterialCHKL.Items[i].ToString();
                }
            }
            BlockMainArray[id].周圍參考材質 = UseReferencedBlock;//更新.
        }
        private void chart_Plot_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs ex = (System.Windows.Forms.MouseEventArgs)e;
            //MessageBox.Show(ex.X.ToString() + "," + ex.Y.ToString());
            if(BlockMainArray.GetLength(0)==0)
            {
                顯示設定ToolStripMenuItem.Visible = false;
                return;
            }
            chart_Plot.Focus();
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
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname == "ARROW")
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
                    if (hitname == "HWL" || hitname.Substring(0, 1) == "E" || hitname == "ARROW")
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
            else if (result.ChartElementType == ChartElementType.LegendItem)
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

                double hitX = chart_Plot.ChartAreas[0].AxisX.PixelPositionToValue(ex.X);
                double hitY = chart_Plot.ChartAreas[0].AxisY.PixelPositionToValue(ex.Y);
                //MessageBox.Show(hitX.ToString() + "," + hitY.ToString());
                PP PHit;
                PHit.h = hitX;
                PHit.v = hitY;
                //MessageBox.Show("X= " + (hitX).ToString() + ", Y= " + (hitY).ToString() + ".");

                for (int i = 0; i < BlockMainArray.GetLength(0); i++)
                {
                    Array.Resize(ref PolygonCol, 0);
                    double[] XI = BlockMainArray[i].X;
                    double[] YI = BlockMainArray[i].Y;
                    for (int j = 0; j < XI.GetLength(0); j++)
                    {
                        Array.Resize(ref PolygonCol, j + 1);
                        PolygonCol[j].h = XI[j];
                        PolygonCol[j].v = YI[j];
                    }
                    double PArea = PolygonArea(PolygonCol);
                    bool 是否在此Polygon內 = InsidePolygon(PolygonCol, PHit);
                    if (是否在此Polygon內)
                    {
                        if (PArea > getPolygonArea)
                        {
                            //MessageBox.Show(BlockMainArray[i].名稱 + ":" + PArea.ToString());
                            getPolygonName = BlockMainArray[i].名稱;
                            getPolygonArea = PArea;
                        }
                    }
                }
                if (object.Equals(getPolygonName, null))
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
                    if (selectname == getPolygonName)
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
        public double CrossVector(PP P1, PP P2)
        {
            return P1.h * P2.v - P2.h * P1.v;
        }
        public double PolygonArea(PP[] Polygon)
        {
            //採用行列式計算方法.
            double area = 0;
            for (int i = 0; i < Polygon.GetUpperBound(0); i++)
            {
                area += (CrossVector(Polygon[i], Polygon[i + 1]));
            }
            return Math.Abs(area);
        }
        public bool InsidePolygon(PP[] Polygon, PP P)
        {
            double angle = 0;
            int n = Polygon.GetLength(0);
            PP p1;
            PP p2;
            for (int i = 0; i < Polygon.GetLength(0); i++)
            {
                p1.h = Polygon[i].h - P.h;
                p1.v = Polygon[i].v - P.v;
                p2.h = Polygon[(i + 1) % n].h - P.h;
                p2.v = Polygon[(i + 1) % n].v - P.v;
                angle += Angle2D(p1.h, p1.v, p2.h, p2.v);

            }
            if (Math.Abs(angle) < Math.PI)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private double Angle2D(double x1, double y1, double x2, double y2)
        {
            double dtheata, theat1, theat2;
            theat1 = Math.Atan2(y1, x1);
            theat2 = Math.Atan2(y2, x2);
            dtheata = theat2 - theat1;
            while (dtheata > Math.PI)
            {
                dtheata -= (Math.PI * 2);
            }
            while (dtheata < -Math.PI)
            {
                dtheata += (Math.PI * 2);
            }
            return dtheata;
        }
        
        private void cmb_seawaveDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_seawaveDir.SelectedIndex != -1)
            {
                //繪上EL();
            }
        }
        
        #endregion
        #region 縮放表單
        private void Form_MTExamProgress_Resize(object sender, EventArgs e)
        {
            //double x = (this.Width / OldWidth);
            //double y = (this.Height / OldHeight);
            ////MessageBox.Show("HH: " + x.ToString() + ", " + y.ToString());
            //foreach(Control ctl in this.Controls)
            //{
                
            //    if (ctl.Name!= "tabControl1")
            //    {
            //        continue;
            //    }
            //    //Tab Page only
            //    foreach(Control ctl2 in ctl.Controls)
            //    {
            //        //MessageBox.Show(ctl2.Name);
            //        foreach (Control ctl3 in ctl2.Controls)
            //        {
            //            MessageBox.Show(ctl3.Name);
            //            ctl3.Width = Convert.ToInt32(x * ctl3.Width);
            //            ctl3.Height = Convert.ToInt32(y * ctl3.Height);
            //        }
            //        ctl2.Width = Convert.ToInt32(x * ctl2.Width);
            //        ctl2.Height = Convert.ToInt32(y * ctl2.Height);
            //    }
            //    ctl.Width = Convert.ToInt32(x * ctl.Width);
            //    ctl.Height = Convert.ToInt32(y * ctl.Height);


            //}
            //OldWidth = this.Width;
            //OldHeight = this.Height;
        }





        #endregion

        private void 輸出ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form_MTExamProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            if (isExporting || isCalc)
            {
                //e.Cancel = true;
                return;
            }
            if (MessageBox.Show("確定關閉?", "關閉檢核", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.Cancel)
            {
                return;
                //e.Cancel = true;
            }
            //e.Cancel = true;
            
            //this.ShowInTaskbar = false;
            mainForm.Activate();
            this.Hide();
            this.Opacity = 0;
            this.ShowInTaskbar = false;

            //this.WindowState = FormWindowState.Minimized;
            //this.Opacity = 0;
            //this.開啟與ReLoad();
            //mainForm.WindowState = FormWindowState.Maximized;// = true;
            //mainForm.ShowInTaskbar = true;
            //mainForm.Activate();
            //this.Hide();
        }

        private void 輸出LogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_LogOutput_Click(sender, e);
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if(isCalc || isExporting)
            {
                e.Cancel = true;
            }
        }
        #region 摩擦設定
        private void DGMaterial_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
        bool newrowadd = false;
        private void DGMaterial_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //新增一個新的材質完成.
            //新增一個材質時,不允許新增之後Cell空白.
            newrowadd = true;
            EscapeDGMaterialCellValueChangedFunction = false;
        }
        private static bool EscapeDGMaterialCellValueChangedFunction = false;
        private void DGMaterial_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!使用者手動更新材質與摩擦)
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

            if (newrowadd)
            {
                newrowadd = false;
                EscapeDGMaterialCellValueChangedFunction = true;
                //不允許材質名稱重覆.
                int addedrow = e.RowIndex;
                //MessageBox.Show(e.Row.Cells[1].Value.ToString());
                //MessageBox.Show(DGMaterial.Rows[0].Cells[1].Value.ToString());
                if (DGMaterial.Rows.Count == 1 || addedrow >= DGMaterial.Rows.Count)
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
                //labelT.Text = "";
                for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
                {
                    Array.Resize(ref MaterialArray, MaterialCount + 1);
                    MaterialArray[i] = DGMaterial.Rows[i].Cells[1].Value.ToString();
                    //labelT.Text += (MaterialArray[i].ToString());
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
                foreach (DataGridViewRow row in DGMaterialRough.Rows)
                {
                    var cell = (DataGridViewComboBoxCell)(row.Cells[1]);
                    cell.DataSource = MaterialArray;
                    var cell2 = (DataGridViewComboBoxCell)(row.Cells[2]);
                    cell2.DataSource = MaterialArray;
                }


                //MessageBox.Show("P1-2");
                if (DGMaterialRough.Rows.Count > 0) //只有一個材質時，沒有摩擦係數設定.
                { DGMaterialRough.CurrentCell = DGMaterialRough.Rows[osize > 0 ? osize - 1 : 0].Cells[3]; }//指定Current Cell.
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
                    if (!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[MaterialRoughnessArrayCount].coef))
                    {
                        MaterialsCoefArray[MaterialRoughnessArrayCount].coef = -9999;
                    }
                    MaterialRoughnessArrayCount += 1;
                }
                //以下區段必須要有Block時才有效.
                if (BlockMainArray.GetLength(0) == 0)
                {
                    return;
                }
                Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
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
            if (MaterialArray.GetLength(0) == 0)
            {
                //MessageBox.Show("Size =0 ");
                return;
            }
            //變更材質名稱時,同步將右側摩擦係數DG與矩陣變更.
            int changerow = e.RowIndex;
            //MessageBox.Show(changerow.ToString());
            if (DGMaterial.Rows[changerow].Cells[1].Value == null)
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
            else if (DGMaterial.Rows[changerow].Cells[1].Value.ToString() == "")
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
            for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
            {
                if (i != changerow)
                {
                    if (DGMaterial.Rows[i].Cells[1].Value.ToString() == changeName)
                    {
                        repeated = true;
                        break;
                    }
                }
            }
            if (repeated)
            {
                MessageBox.Show("不允許將名稱更改為重覆的材質名稱!!失敗", "材質名稱", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
            for (int i = 0; i < DGMaterial.Rows.Count - 1; i++)
            {
                //MessageBox.Show(i.ToString());
                Array.Resize(ref MaterialArray, MaterialCount + 1);
                MaterialArray[MaterialCount] = DGMaterial.Rows[i].Cells[1].Value.ToString();
                MaterialCount += 1;
            }
            //MessageBox.Show(MaterialArray.GetLength(0).ToString());
            MaterialNameToArraySubScript.Remove(oldname);//更改矩陣值.
            MaterialNameToArraySubScript.Add(changeName, changerow);
            MaterialSubscriptToName[changerow] = changeName;//更改Key值.

            //更改對應的摩擦係數.
            Array.Resize(ref MaterialsCoefArray, 0);
            MaterialRoughnessArrayCount = 0;
            //MessageBox.Show("H2-3");
            for (int i = 0; i < DGMaterialRough.Rows.Count; i++)
            {
                if (DGMaterialRough.Rows[i].Cells[1].Value.ToString() == oldname)
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
                if (!double.TryParse(DGMaterialRough.Rows[i].Cells[3].Value.ToString(), out MaterialsCoefArray[MaterialRoughnessArrayCount].coef))
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
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                string[] ReferencedMaterialsOfBlock = BlockMainArray[i].周圍參考材質;
                for (int j = 0; j < ReferencedMaterialsOfBlock.GetLength(0); j++)
                {
                    if (ReferencedMaterialsOfBlock[j] == oldname)
                    {
                        ReferencedMaterialsOfBlock[j] = changeName;
                    }
                }
            }

            //MessageBox.Show("H2-5");
            if (listBox_SectSetting.SelectedIndex != -1 && listBox_SectSetting.Items.Count > 0)
            {
                //重新載入Property Grid與參考材質.
                Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
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
            }

            EscapeDGMaterialCellValueChangedFunction = false;   
        }
        int DGMaterialDeleteRow = -1;
        private void DGMaterial_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (!使用者手動更新材質與摩擦)
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
            if (MessageBox.Show("您確定刪除此材質嗎?\n刪除後，與此材質相關的摩擦係數設定都會被移除!", "刪除", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
            {
                e.Cancel = true;

                return;
            }
            else
            {
                foreach (DataGridViewRow row in DGMaterial.SelectedRows) { DGMaterialDeleteRow = row.Index; break; }
                EscapeDGMaterialCellValueChangedFunction = true;
            }
        }
        private void DGMaterial_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            刪除Material的材質(false);
        }
        private void 刪除Material的材質(bool Removed = true)
        {

            if (DGMaterialDeleteRow == -1)
            { return; }
            if (!MaterialSubscriptToName.ContainsKey(DGMaterialDeleteRow))
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
                Class_Block_MT_Interface D = new Class_Block_MT_Interface(BlockMainArray[listBox_SectSetting.SelectedIndex]);
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

            //MessageBox.Show("將材質'" + deleteMaterialName + "'與相關的摩擦係數設定刪除完畢!!", "材質管理", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static DataGridViewSelectedRowCollection DGMaterialRoughUserDeleteRows = null;
        private static bool Escape材質間摩擦係數刪除事件 = true;
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
                if (DGMRCell.Count == 0)
                {
                    return;
                }
                Dictionary<int, bool> DeleteRow = new Dictionary<int, bool>();
                //DGMaterialRough.Rows.RemoveAt(DGMaterialRoughUserDeleteRows);
                for (int i = 0; i < DGMRCell.Count; i++)
                {
                    if (!DeleteRow.ContainsKey(DGMRCell[i].RowIndex))
                    {
                        DeleteRow.Add(DGMRCell[i].RowIndex, true);
                    }
                }

                //for (int i=DGMaterialRoughUserDeleteRows.Count-1; i>=0;i--) // i++)
                //{
                int[] DD = new int[] { };
                foreach (int i in DeleteRow.Keys)
                {
                    Array.Resize(ref DD, DD.GetLength(0) + 1);
                    DD[DD.GetUpperBound(0)] = i;
                }
                //MessageBox.Show("SIZE:" + DD.GetLength(0).ToString());
                Array.Sort(DD);// , 0, DD.GetLength(0));
                for (int i = DD.GetLength(0) - 1; i >= 0; i--)
                {
                    EscapeDGMaterialRoughnessCellContentChanged = true;
                    //MessageBox.Show(DD[i].ToString());
                    Escape材質間摩擦係數刪除事件 = i == 0 ? false : true;
                    DGMaterialRough.Rows.RemoveAt(DD[i]);
                }
            }
        }
        private void DGMaterialRough_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            //DG Roughness設定內移除列.
            if (MessageBox.Show("您確定要刪除這些資料嗎?", "刪除摩擦係數", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
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
            if (!使用者手動更新材質與摩擦)
            {
                return;
            }
            if (Escape材質間摩擦係數刪除事件)
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
        private static bool EscapeDGMaterialRoughnessCellContentChanged = false;
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
        private void DGMaterialRough_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!使用者手動更新材質與摩擦)
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
                    //break;
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
        private void DGMaterialRough_Click(object sender, EventArgs e)
        {

        }
        private void DGMaterialRough_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //設定摩擦係數時.
            //不做任何事情,等實際運行前再檢查.
            //MessageBox.Show("H1");
            if (!使用者手動更新材質與摩擦)
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
        private void DGMaterialRough_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //See also https://stackoverflow.com/questions/5652957/what-event-catches-a-change-of-value-in-a-combobox-in-a-datagridviewcell

            if (this.DGMaterialRough.IsCurrentCellDirty)
            {
                //This will fire the CellValueChangedEvent.
                DGMaterialRough.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void DGMaterialRough_KeyUp(object sender, KeyEventArgs e)
        {
            if (DGMaterialRough.CurrentCell.RowIndex != -1)
            {
                if (DGMaterialRough.CurrentCell.ColumnIndex == 3)
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

        #endregion

        #region 附屬
        private void 檢視使用者說明書ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isExporting) { return; }
            mainForm.檢示使用者說明書ToolStripMenuItem_Click(sender, e);
        }
        #endregion

        #region 操作設定
        private void tabControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (("D1D2D3D4D5D6D7D8D9D0".Contains(e.KeyCode.ToString())) && e.Control)
            {
                //按下數字鍵
                int selectedindex;
                int.TryParse(e.KeyCode.ToString().Replace("D", ""), out selectedindex);
                //MessageBox.Show(selectedindex.ToString());
                if (selectedindex < 1 || selectedindex > tabControl1.TabPages.Count)
                {
                    //Do nothing.
                }
                else
                {
                    tabControl1.SelectedIndex = (selectedindex - 1);
                }
            }
        }
        private void tabControl1_KeyUp(object sender, KeyEventArgs e)
        {
            /*
            if (("D1D2D3D4D5D6D7D8D9D0".Contains(e.KeyCode.ToString())) && e.Control)
            {
                //按下數字鍵
                int selectedindex;
                int.TryParse(e.KeyCode.ToString().Replace("D", ""), out selectedindex);
                //MessageBox.Show(selectedindex.ToString());
                if (selectedindex < 1 || selectedindex > tabControl1.TabPages.Count)
                {
                    //Do nothing.
                }
                else
                {
                    tabControl1.SelectedIndex = (selectedindex - 1);
                }
            }
            
            else if (e.Control && e.KeyCode == Keys.Right)
            {
                //往右.
                if (tabControl1.SelectedIndex == tabControl1.TabPages.Count - 1)
                {
                    tabControl1.SelectedIndex = 0;
                }
                else
                {
                    tabControl1.SelectedIndex += 1;
                }
            }
            else if (e.Control && e.KeyCode == Keys.Left)
            {
                if (tabControl1.SelectedIndex == 0)
                {
                    tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
                }
                else
                {
                    tabControl1.SelectedIndex -= 1;
                }
            }
            */
        }

        #endregion

        private void 測試ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 顯示比例ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //調整顯示比例之測試.
            if (BlockMainArray.GetLength(0) == 0) { return; }
            //跳出顯示比例的設定視窗.

            return;

            /*
            ChartArea ca = chart_Plot.ChartAreas[0];
            ElementPosition ipp0 = ca.InnerPlotPosition;
            Size OldClientSize = chart_Plot.ClientSize;
            int oldClientXSize = chart_Plot.ClientSize.Width;
            int oldClientYSize = chart_Plot.ClientSize.Height;
            double oldXmin = chart_Plot.ChartAreas[0].AxisX.Minimum;// -xspace/2.0;// -xspace/2.0;// - xspace;
            double oldXmax = chart_Plot.ChartAreas[0].AxisX.Maximum;// NewXmax;
            double oldXInterval = chart_Plot.ChartAreas[0].AxisX.Interval;
            double oldYmin = chart_Plot.ChartAreas[0].AxisY.Minimum;// - yspace;
            double oldYmax = chart_Plot.ChartAreas[0].AxisY.Maximum;// NewYmax;
            double oldYInterval = chart_Plot.ChartAreas[0].AxisY.Interval;


            //Adjusting the interval and new ymax,ymin,xmax,xmin.
            float xf = (float)ipp0.Width / (float)(oldXmax - oldXmin);
            float yf = (float)ipp0.Height / (float)(oldYmax - oldYmin);


            //目的是透過調整座標軸上的數值,達到等間距的目的.
            if(yf<xf)
            {
                //以yf調整Xmax.
                chart_Plot.ChartAreas[0].AxisX.Maximum = oldXmin + ipp0.Width / yf;
            }
            else if(yf>xf)
            {
                //以xf調整Ymax.
                chart_Plot.ChartAreas[0].AxisY.Maximum = oldYmin + ipp0.Height / xf;
            }
            MessageBox.Show("完成");
            */
        }


    }
}
