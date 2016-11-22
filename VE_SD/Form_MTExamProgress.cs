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
using System.Net;

namespace VE_SD
{
    public partial class Form_MTExamProgress : Form
    {

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Class_BlockSect[] BlockMainArray = new Class_BlockSect[] { };// = new Class_BlockSect();
        int BlockCount = 0; //Block Array size.
        private struct PointFMY
        {
            public double x;
            public double y;
            public double z;
        }
        string selectname = null;  //目前點選到的Block.
        Module2 Mod = null;

        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public Form_MTExamProgress()
        {
            InitializeComponent();
        }
        private Form1 mainForm = null;
        public Form_MTExamProgress(Form callingForm)
        {
            mainForm = callingForm as Form1;//傳入物件參考.
            InitializeComponent();
        }
        static XmlNode RNode;
        static XmlElement Relement;
        private void 開啟舊的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //只打開座標點位.
            if (OFD_專案.ShowDialog() == DialogResult.OK)
            {
                Class_BlockSect[] BlockMainArrayR = new Class_BlockSect[] { };
                int blockSizer = 0;
                XmlDocument doc = new XmlDocument();
                doc.Load(OFD_專案.FileName);
                bool 成功與否 = false;
                try
                {
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
                            MessageBox.Show("Block讀取失敗!點數欄位無法讀取");
                            return;
                        }
                        BlockMainArrayR[blockSizer].座標點數 = PointCount;
                        double ftest;
                        bool btest;

                        //Block單位體積重量
                        RNode = BlockNode.SelectSingleNode("單位體積重量");
                        if (object.Equals(RNode, null))
                        {
                            MessageBox.Show("Block讀取單位體積重量失敗!");
                        }
                        Relement = (XmlElement)RNode;
                        if (!double.TryParse(Relement.GetAttribute("Value"), out ftest))
                        {
                            MessageBox.Show("Block讀取單位體積重量失敗!");
                        }
                        BlockMainArrayR[blockSizer].單位體積重量 = ftest;

                        //計算Moment與否.
                        RNode = BlockNode.SelectSingleNode("計算Moment");
                        if (object.Equals(RNode, null))
                        {
                            MessageBox.Show("Block讀取計算Moment狀況失敗!");
                        }
                        Relement = (XmlElement)RNode;
                        if (!bool.TryParse(Relement.GetAttribute("Value"), out btest))
                        {
                            MessageBox.Show("Block讀取計算Moment狀況失敗!");
                        }
                        BlockMainArrayR[blockSizer].計算Moment與否 = btest;

                        //Block使用材質
                        RNode = BlockNode.SelectSingleNode("使用材質");
                        if (object.Equals(RNode, null))
                        {
                            MessageBox.Show("Block讀取Block使用材質失敗!!");
                        }
                        Relement = (XmlElement)RNode;
                        /*
                        if (Relement.GetAttribute("Value").ToString() != "")
                        {
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
                            MessageBox.Show("Block讀取座標失敗");
                        }

                        foreach (XmlNode CN in CoordinateCollection)
                        {
                            Relement = (XmlElement)CN;
                            Array.Resize(ref getx, getPointsize + 1);
                            Array.Resize(ref gety, getPointsize + 1);

                            if (!double.TryParse(Relement.GetAttribute("xValue"), out ftest))
                            {
                                MessageBox.Show("Block讀取座標時無法順利從'xValue'欄位轉換座標值");
                            }
                            getx[getPointsize] = ftest;
                            if (!double.TryParse(Relement.GetAttribute("yValue"), out ftest))
                            {
                                MessageBox.Show("Block讀取座標時無法順利從'yValue'欄位轉換座標值");
                            }
                            gety[getPointsize] = ftest;
                            getPointsize += 1;
                        }

                        if (!(getPointsize == PointCount))
                        {
                            MessageBox.Show("Block讀取座標點數量不一致!");
                        }

                        BlockMainArrayR[blockSizer].X = getx;
                        BlockMainArrayR[blockSizer].Y = gety;

                        blockSizer += 1;
                    }


                    RNode = doc.SelectSingleNode("Root/Blocks/選取Block序號");
                    if (object.Equals(RNode, null))
                    {
                        MessageBox.Show("讀取Block選取序號時失敗");
                    }
                    int itest;
                    Relement = (XmlElement)RNode;
                    if (!int.TryParse(Relement.GetAttribute("Value"), out itest))
                    {
                        MessageBox.Show("轉換Block選取序號時失敗");
                    }
                    int selectedBlockIndex = itest;
                    if (selectedBlockIndex > BlockMainArrayR.GetUpperBound(0))
                    {
                        MessageBox.Show("Block選取序號超過檔案內紀錄的Block數量!");
                    }


                    //檢核區塊.
                    成功與否 = true;
                }
                catch
                {
                    //MessageBox.Show("出現錯誤!");
                }

                Form_BlockNameAndCorrdinate p = new Form_BlockNameAndCorrdinate();
                for (int i = 0; i < BlockMainArrayR.GetLength(0); i++)
                {
                    if (!p.CheckIsConvexPolygonAndCounterClockWise(BlockMainArrayR[i].X, BlockMainArrayR[i].Y))
                    {
                        //若排列順序非逆時針.
                        MessageBox.Show("Block" + (i + 1).ToString() + "(從1開始)的座標矩陣錯誤,此Block非凸邊形且座標沒有依照逆時針方向紀錄!!");
                    }
                }
                BlockMainArray = BlockMainArrayR;
                //加入行塊資訊.
                for (int i = 0; i < BlockMainArray.GetLength(0); i++)
                {
                    BlockMainArray[i] = BlockMainArrayR[i];
                }
                toolStripStatusLabel1.Text = "開啟完成,取得" + BlockMainArray.GetLength(0).ToString() + "個區塊";

                //*****
                //填入資訊.
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

                if(成功與否)
                {
                    MessageBox.Show("成功載入");
                }
                else
                {
                    MessageBox.Show("載入失敗");
                }
               //******************
            }

        }
        private void btn_Test_Click(object sender, EventArgs e)
        {
            if(BlockMainArray.GetLength(0)==0)
            {
                MessageBox.Show("沒有任何東西需要被檢核");
                return;
            }

            //計算Block的平均摩擦係數.
            for(int i=0;i<BlockMainArray.GetLength(0);i++)
            {
                BlockMainArray[i].平均摩擦係數 = 0.6 ;
            }
            //***********************************************************************************************************************//
            //帶入計算
            Mod = new Module2();
            Mod.DeleteAllBlockData();

            // Block給定.
            for (int i = 0; i < BlockMainArray.GetLength(0); i++)
            {
                //- 迴圈塞入Block.
                // 2016/03/29. 新增是否計算Moment選項.
                int nowid = Mod.NewBlock(BlockMainArray[i].單位體積重量, BlockMainArray[i].平均摩擦係數, BlockMainArray[i].計算Moment與否);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                double[] getx = BlockMainArray[i].X;
                double[] gety = BlockMainArray[i].Y;
                int 座標點數 = BlockMainArray[i].座標點數;
                for (int i2 = 0; i2 < 座標點數; i2++)
                {
                    Mod.SetBlockCoord(nowid, getx[i2], gety[i2]);
                }
            }

            MessageBox.Show("Finished Run!");
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        }

        private void Form_MTExamProgress_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "嗨唷~這裡是碼頭檢核模組";
            data_BlockTempShow.Rows.Clear();

        }
        #region 專案控制區

        private void 開啟一個新的專案檔ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        #endregion

        private void btn_LogOutput_Click(object sender, EventArgs e)
        {
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
    }
}
