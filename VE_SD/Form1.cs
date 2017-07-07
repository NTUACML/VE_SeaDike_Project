using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.IO;
using System.Net.NetworkInformation;
using System.Management.Instrumentation;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml;

namespace VE_SD
{
    public partial class Form1 : Form
    {
        string _LoginInUserID;
        string _LoginInUserName;
        bool _PSKEYCORRECT;
        //bool loginsuccess = false;
        bool _RemoveLogInDataWhenClosing = false;//是否於每次軟體正常關閉時,移除登入使用者資訊,則下次登入時必須重新輸入.


        string _SystemReferenceStoreFolder= Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD";
        private string 驗證機碼存放位置 = "C:\\LKK";
        private string Exepath = System.IO.Directory.GetCurrentDirectory();
        private string PORT = "2016";
        private bool _提供服務訊息=true;//是否提供服務訊息傳送到主機.
        private static string _軟體開啟時的視窗大小="正常";
        private static bool  _碼頭檢核開啟時預設數字 = true;
        private static bool _防波堤檢核開啟時預設數字 = true;

        private static string[] photo = new string[] { };
        private static Dictionary<int, int> 照片對照表 = new Dictionary<int, int>();
        //UdpClient U;//宣告UDP通訊物件.
        //Thread th;//宣告監聽用執行緒.

        public Form1()
        {
            InitializeComponent();
        }

        #region 公用參數取用區
        public string SystemReferenceStoreFolder
        {
            get { return _SystemReferenceStoreFolder; }
        }
        public string LoginTextSetting
        {
            get { return label_LoginCond.Text; }
            set { label_LoginCond.Text = value; }
        }
        public string LoginInUserID
        {
            get { return _LoginInUserID; }
            set { _LoginInUserID = value; }
        }
        public string LoginInUserName
        {
            get { return _LoginInUserName; }
            set { _LoginInUserName = value; }
        }
        public bool PSKEYCONDITIONCORRECT
        {
            get { return _PSKEYCORRECT; }
            set { _PSKEYCORRECT = value; }
        }
        public bool RemoveLoginDataWhenClosing
        {
            get { return _RemoveLogInDataWhenClosing; }
            set { _RemoveLogInDataWhenClosing = value; }
        }
        public bool 提供服務訊息
        {
            get { return _提供服務訊息; }
            set { _提供服務訊息 =value; }
        }
        public string 軟體開啟時的視窗大小
        {
            get { return _軟體開啟時的視窗大小; }
            set { _軟體開啟時的視窗大小 = value; }
        }
        public bool 碼頭檢核開啟時預設數字
        {
            get { return _碼頭檢核開啟時預設數字; }
            set { _碼頭檢核開啟時預設數字 = value; }
        }
        public bool 防波堤檢核開啟時預設數字
        {
            get { return _防波堤檢核開啟時預設數字; }
            set { _防波堤檢核開啟時預設數字 = value; }
        }
        public string 程式運作路徑
        {
            get { return Exepath; }
        }
        #endregion 

        private static string 驗證Msg = "";
        private static bool 驗證Bool;
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(_SystemReferenceStoreFolder))
            {
                Directory.CreateDirectory(_SystemReferenceStoreFolder);
            }
            _RemoveLogInDataWhenClosing = false; //預設為不刪除.
            _提供服務訊息 = true;
            label_LoginCond.Text = "NO";
            TSP_Progressbar.Visible = false;
            TSSTATUS_label.Text = "歡迎使用浩海工程顧問公司檢核程式!";
            Form_Welcome fwel = new Form_Welcome();
            fwel.ShowDialog();




            //跳出登入視窗.
            System.Threading.Thread.Sleep(1000);
            Form_Login flogin = new Form_Login(this, "Exit Then End All");//"Exit Then End All": 若使用者選擇不登入,則直接關閉.
            flogin.ShowDialog();


            //更改Toolstrip排列,Layout方法要更改為Overflow with horizontal[Horizontal Stack with overflow]
            TSP_UserInfoShow.Text = "目前登入使用者ID:" + _LoginInUserID + ",名稱為" + _LoginInUserName;
            TSP_UserInfoShow.Alignment = ToolStripItemAlignment.Right;
            TSP_UserInfoShow.RightToLeft = RightToLeft.No;

            TSP_ChangeUserBtn.Alignment = ToolStripItemAlignment.Right;
            TSP_ChangeUserBtn.RightToLeft = RightToLeft.No;
            TSP_Validate.Alignment = ToolStripItemAlignment.Right;
            TSP_Validate.RightToLeft = RightToLeft.No;
            TSP_Validate.BackColor = Color.Gray;


            //載入使用者登入設定.
            LoadingProgramSystemReference();

            //更改排列順序.
            //TSP_Progressbar.Alignment = ToolStripItemAlignment.Right;
            //TSP_Progressbar.RightToLeft = RightToLeft.No;
            //TSP_Progressbar.Visible = true;
            //TSP_Progressbar.Style = ProgressBarStyle.Marquee;
            Array.Resize(ref photo, 2);
            photo[0] = "STDVESD";
            photo[1] = "MTExamVESD";

            //載入圖檔;
            imageList1.ImageSize = new Size(256, 150);// 200);
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            int ic = 0;
            for (int i = 0; i <= photo.GetUpperBound(0); i++)
            {
                if (File.Exists(Exepath + "\\PIC\\" + photo[i] + ".JPG"))
                {
                    imageList1.Images.Add(Image.FromFile(Exepath + "\\PIC\\" + photo[i] + ".JPG"));//new Bitmap(Exepath + "\\PIC\\" + photo[i] + ".JPG"));

                    照片對照表.Add(i, ic);
                    ic += 1;
                }
                else
                {
                    照片對照表.Add(i, -9999);
                }
            }
            驗證Bool = false;
            bk_Validate.RunWorkerAsync();
            
            if(_軟體開啟時的視窗大小=="最大")
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        public void LoadingProgramSystemReference()
        {
            //載入設定,若無則直接跳過.
            string SystemReferenceFileName = _SystemReferenceStoreFolder + "\\System.Info";
            //以XML格式處理.
            FileInfo f1 = new FileInfo(SystemReferenceFileName);
            if(!f1.Exists)
            {
                MessageBox.Show("No File exists");
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(SystemReferenceFileName);
            bool 提供服務訊息Inner = true;
            bool 每次關閉軟體後刪除使用者登入資訊 = false;
            string 開啟軟體時的視窗大小 = null;
            bool 碼頭檢核開啟時預設數字Inner=true;
            bool 防波堤檢核開啟時預設數字Inner=true;
            try
            {
                //開啟失敗,則跳出.
                //每次關閉軟體後刪除使用者登入資訊
                XmlNode RNode = doc.SelectSingleNode("Root/每次關閉軟體後刪除使用者登入資訊");
                if (object.Equals(RNode, null))
                {
                    //MessageBox.Show("H1");
                    return;
                }
                XmlElement Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 每次關閉軟體後刪除使用者登入資訊))
                {
                    //MessageBox.Show("H2");
                    return;
                }

                //提供服務訊息
                RNode = doc.SelectSingleNode("Root/提供服務訊息");
                if (object.Equals(RNode, null))
                {
                    //MessageBox.Show("H3");
                    return;

                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value").ToString(), out 提供服務訊息Inner))
                {
                    //MessageBox.Show("H4");
                    return;
                }

                //啟動時視窗大小
                RNode = doc.SelectSingleNode("Root/啟動時視窗大小");
                if(object.Equals(RNode,null))
                {
                    return;
                }
                Relement = (XmlElement)RNode;
                開啟軟體時的視窗大小 = Relement.GetAttribute("Value").ToString();
                if (開啟軟體時的視窗大小!="正常" && 開啟軟體時的視窗大小 != "最大")
                {
                    return;
                }

                //碼頭檢核開啟時預設數字
                RNode = doc.SelectSingleNode("Root/碼頭檢核開啟時預設數字");
                if (object.Equals(RNode, null))
                {
                    return;
                }
                Relement = (XmlElement)RNode;
                if(!bool.TryParse(Relement.GetAttribute("Value"),out 碼頭檢核開啟時預設數字Inner))
                {
                    return;
                }

                //防波堤開啟時預設數字
                RNode = doc.SelectSingleNode("Root/防波堤檢核開啟時預設數字");
                if (object.Equals(RNode, null))
                {
                    return;
                }
                Relement = (XmlElement)RNode;
                if (!bool.TryParse(Relement.GetAttribute("Value"), out 防波堤檢核開啟時預設數字Inner))
                {
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace.ToString());

            }

            _RemoveLogInDataWhenClosing = 每次關閉軟體後刪除使用者登入資訊;
            _提供服務訊息 = 提供服務訊息Inner;
            _軟體開啟時的視窗大小 = 開啟軟體時的視窗大小;
            _防波堤檢核開啟時預設數字 = 防波堤檢核開啟時預設數字Inner;
            _碼頭檢核開啟時預設數字 = 碼頭檢核開啟時預設數字Inner;

        }
        public void SavingProgramSystemReference()
        {
            //儲存系統設定.
            string SystemReferenceFileName = _SystemReferenceStoreFolder + "\\System.Info";
            if(!Directory.Exists(_SystemReferenceStoreFolder))
            {
                Directory.CreateDirectory(_SystemReferenceStoreFolder);
            }
            //以XML格式儲存.
            XmlDocument doc = new XmlDocument();
            XmlElement Root = doc.CreateElement("Root");
            doc.AppendChild(Root);

            XmlElement 每次關閉軟體後刪除使用者登入資訊= doc.CreateElement("每次關閉軟體後刪除使用者登入資訊");
            每次關閉軟體後刪除使用者登入資訊.SetAttribute("Value", _RemoveLogInDataWhenClosing.ToString());

            XmlElement 提供服務訊息Node = doc.CreateElement("提供服務訊息");
            提供服務訊息Node.SetAttribute("Value", _提供服務訊息.ToString());

            XmlElement 開啟軟體時的視窗大小Node = doc.CreateElement("開啟軟體時的視窗大小");
            開啟軟體時的視窗大小Node.SetAttribute("Value", _軟體開啟時的視窗大小);

            XmlElement 碼頭檢核開啟時預設數字Node = doc.CreateElement("碼頭檢核開啟時預設數字");
            碼頭檢核開啟時預設數字Node.SetAttribute("Value", _碼頭檢核開啟時預設數字.ToString());


            XmlElement 防波堤檢核開啟時預設數字Node = doc.CreateElement("防波堤檢核開啟時預設數字");
            防波堤檢核開啟時預設數字Node.SetAttribute("Value", _防波堤檢核開啟時預設數字.ToString());

            Root.AppendChild(每次關閉軟體後刪除使用者登入資訊);
            Root.AppendChild(提供服務訊息Node);
            Root.AppendChild(開啟軟體時的視窗大小Node);
            Root.AppendChild(碼頭檢核開啟時預設數字Node);
            Root.AppendChild(防波堤檢核開啟時預設數字Node);

            doc.Save(SystemReferenceFileName);

        }
        private void 海堤檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_StandardRDC_Click(sender, e);
            return;


            //此功能已取消,目前主表單已直接改為海堤檢核主表單.
            //開啟海堤檢核主表單.
            if (_提供服務訊息)
            {
                this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')開啟了標準海堤檢核工具,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            }
            Form_RDExamProgress frdexam = new Form_RDExamProgress(this);
            frdexam.ShowDialog();
            //return;


            //Form_SDM fsdm = new Form_SDM();
            //fsdm.ShowDialog();

        }
        private void 海堤檢核給Kavy玩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //string temp1 = "C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\Release\\Temp2.docx";
            //string temp2 = "C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\Release\\Temp_2.txt";
            //File.Copy("C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\Release\\Output_Template.docx", temp1, true);
            //File.Copy("C:\\Users\\Andy\\Desktop\\VE_SeaDike_Project\\VE_SD\\bin\\Release\\Test.txt", temp2, true);
            //發送操作指令("TRANSFER:Temp2.docx" + "\n" + _LoginInUserID + "\n" + _LoginInUserName);
            //發送檔案給主機(temp1);
            //while(bk_SendFIle.IsBusy)
            //{
            //    System.Threading.Thread.Sleep(2000);
            //}

            //發送操作指令("TRANSFER:Temp_2.txt" + "\n" + _LoginInUserID + "\n" + _LoginInUserName);
            //發送檔案給主機(temp2);


            //Wait for ok.
            //while (true)
            //{
            //    try
            //    {
            //        發送操作指令("TRANSFER:Temp_2.txt" + "\n" + _LoginInUserID + "\n" + _LoginInUserName);

            //        break;
            //    }
            //    catch
            //    {

            //    }

            //}
            //發送檔案給主機(temp2);

            //MessageBox.Show("Done");
            //Form_ForKavyTest  kavytest = new Form_ForKavyTest();
            //kavytest.ShowDialog();

        }
        private void btn_StandardRDC_Click(object sender, EventArgs e)
        {
            
            string 驗證Msg = "";
            if (檢視目前是否已有合理認證(ref 驗證Msg)) //mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref 驗證Msg))
            {
                //Nothing.
                TSP_Validate.BackColor = Color.Green;
            }
            else
            {
                //沒有驗證資訊,提示無法進行檢核計算.
                TSP_Validate.BackColor = Color.Red;
                if (MessageBox.Show("您目前沒有通過此軟體之驗證,您將無法使用檢核計算功能,僅能設定專案檔" + Environment.NewLine + "確定繼續進行?", "沒有軟體驗證", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                {
                    return;
                }
                
            }
            
            if (_提供服務訊息)
            {
                this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')開啟了標準海堤檢核工具,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            }
            Form_RDExamProgress frdexam = new Form_RDExamProgress(this);
            frdexam.ShowDialog();
        }
        private void 碼頭檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            button1_Click(sender, e);
        }
        #region 碼頭檢核開啟

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("尚不開放使用");
            //return;
            
            //碼頭檢核工具.
            string 驗證Msg = "";
            if (檢視目前是否已有合理認證(ref 驗證Msg)) //mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref 驗證Msg))
            {
                //Nothing.
                TSP_Validate.BackColor = Color.Green;
            }
            else
            {
                TSP_Validate.BackColor = Color.Red;
                //沒有驗證資訊,提示無法進行檢核計算.
                if (MessageBox.Show("您目前沒有通過此軟體之驗證,您將無法使用檢核功能,僅能設定專案檔" + Environment.NewLine + "確定繼續進行?","沒有軟體驗證",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.Cancel)
                {
                    return;
                }

            }
            
            if (_提供服務訊息)
            {
                this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')開啟了碼頭檢核工具,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            }
            Form_MTExamProgress fmtexam = new Form_MTExamProgress(this);
            fmtexam.ShowDialog();
        }
        #endregion
        private void btn_StandardRDC_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show(Exepath);
            this.textBox_ItemDescp.Text = "此為防波堤檢核程式,使用者須輸入計算所需之參數以求得防波堤設計是否符合所需之標準";
            /*
            if(照片對照表[0]==-9999)
            {
                //什麼事情都不做.
                this.pictureBox_ItemDescp.Image = null;
            }
            else
            {
                this.pictureBox_ItemDescp.Image = imageList1.Images[照片對照表[0]];
            }
            */
            
            try
            { 
              this.pictureBox_ItemDescp.Load(Exepath + "\\PIC\\STDVESD.JPG");
            }
            catch
            {
                this.pictureBox_ItemDescp.Image = null;
            }
            

        }
        private void btn_StandardRDC_MouseLeave(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "";
            this.pictureBox_ItemDescp.Image = null;
        }
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "此為碼頭檢核程式,使用者須輸入計算所需之參數以求得碼頭設計是否符合所需之標準";
            /*
            if (照片對照表[1] == -9999)
            {
                this.pictureBox_ItemDescp.Image = null;
            }
            else
            {
                this.pictureBox_ItemDescp.Image = imageList1.Images[照片對照表[1]];
            }
            */
            
            try
            {
                this.pictureBox_ItemDescp.Load(Exepath + "\\PIC\\MTExamVESD.JPG");
            }
            catch
            {
                this.pictureBox_ItemDescp.Image = null;
            }
            
        }
        private void button1_MouseLeave(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "";
            this.pictureBox_ItemDescp.Image = null;

        }
        #region "檢視說明"

        public void 檢示使用者說明書ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            { 
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.StartInfo.FileName = "VESD User Mannual Version 1.0.pdf";// 浩海工程顧問-工程輔助軟體說明手冊.pdf";// EPA SWMM User Manual Version 5.1.pdf";//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                p.Start();
            }
            catch
            {

            }
        }
        #endregion
        #region  更改使用者
        private void TSP_ChangeUserBtn_Click(object sender, EventArgs e)
        {
            //呼叫一開始的登入視窗出來.
            Form_Login flogin = new Form_Login(this, "Exit Then Return");//"Exit Then End All": 若使用者選擇不登入,則直接關閉.
            flogin.ShowDialog();

            //更新資訊.
            TSP_UserInfoShow.Text = "目前登入使用者ID:" + _LoginInUserID + ",名稱為" + _LoginInUserName;
        }
        #endregion
        #region 設定機碼
        private void 軟體驗證ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //驗證此軟體.

            //1. 檢查是否有驗證資訊.
            string F1 = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string F2 = 密碼轉16位密碼計算(F1);
            string NewKey = 密碼16位碼再加密(F2);
            //MessageBox.Show(NewKey);
            //string MKey = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string MKeyPostiion = 驗證機碼存放位置 + "\\" + "MKEY.KEY";
            string getMKkey = null;
            string gets = "";
            this._PSKEYCORRECT = false;
            if(!Directory.Exists(驗證機碼存放位置))
            {
                //沒有驗證過.
                TSP_Validate.BackColor = Color.Red;
                Form_EnterKey fkey = new Form_EnterKey(this, NewKey, "請連絡相關人員來提供您驗證密碼" + Environment.NewLine + F2);
                fkey.ShowDialog();

                if (!_PSKEYCORRECT)
                {
                    //失敗.
                    if (_提供服務訊息)
                    {
                        this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')有嘗試驗證軟體之活動且驗證已失敗,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                    }
                }
                else
                {
                    Directory.CreateDirectory(驗證機碼存放位置);
                    StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                    sw1.WriteLine(NewKey);
                    sw1.Flush();
                    sw1.Close();
                    MessageBox.Show("驗證已通過", "驗證軟體", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this._PSKEYCORRECT = false;
                    if (_提供服務訊息)
                    {
                        this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')已成功完成軟體驗證(驗證密碼為'" + F2 + "'),員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                    }
                    TSP_Validate.BackColor = Color.Green;
                }
            }
            else
            {
                if(!File.Exists(MKeyPostiion))
                {
                    //無此檔案,視為無驗證過.
                    TSP_Validate.BackColor = Color.Red;
                    Form_EnterKey fkey = new Form_EnterKey(this, NewKey, "請連絡相關人員來提供您驗證密碼" + Environment.NewLine + F2);
                    fkey.ShowDialog();

                    if (!_PSKEYCORRECT)
                    {
                        //失敗.
                        if (_提供服務訊息)
                        {
                            this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')有嘗試驗證軟體之活動且驗證已失敗,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                        }
                    }
                    else
                    {
                        Directory.CreateDirectory(驗證機碼存放位置);
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(NewKey);
                        sw1.Flush();
                        sw1.Close();
                        this._PSKEYCORRECT = false;
                        MessageBox.Show("驗證已通過", "驗證軟體", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (_提供服務訊息)
                        {
                            this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')已成功完成軟體驗證(驗證密碼為'" + F2 + "'),員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                        }
                        TSP_Validate.BackColor = Color.Green;
                    }
                }
                else
                {
                    //讀取檔案的第一行.
                    StreamReader sr1 = new StreamReader(MKeyPostiion);
                    gets = sr1.ReadLine();
                    getMKkey = gets;//取得轉換完成之密碼.如1234-5678
                    sr1.Close();

                    if(getMKkey!=NewKey)
                    {
                        //失敗.
                        //殺掉檔案,顯示失敗.
                        if (_提供服務訊息)
                        {
                            this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')有嘗試驗證軟體之活動且驗證因密碼不符合已失敗,員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));

                        }
                        MessageBox.Show("您的驗證已過時,請聯絡相關人員提供最新之驗證", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(MKeyPostiion);
                        TSP_Validate.BackColor = Color.Red;
                    }
                    else
                    {
                        //沒事.
                        if (_提供服務訊息)
                        {
                            this.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + GetMacAddress() + "', IP(IPV4) = '" + MyIP() + "')有嘗試驗證軟體之活動且驗證通過(驗證密碼為'" + F2 + "'),員工編號為'" + _LoginInUserID + "',員工名稱為'" + _LoginInUserName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                        }
                        MessageBox.Show("您的驗證無誤","軟體驗證",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        TSP_Validate.BackColor = Color.Green;
                    }
                }

            }
        }
        private void 軟體機碼設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string MKey = 機碼密碼初步加密(GetCPUID()+ "_" + GetMacAddress());//獲取此機器之數據.
            string MKeyPostiion =驗證機碼存放位置+ "\\" + "MKEY.KEY"; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string MPSPosition = 驗證機碼存放位置 + "\\" + "MPS.KEY";
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!Directory.Exists(驗證機碼存放位置))
            {
                //採取自動建立此資料夾.
                Directory.CreateDirectory(驗證機碼存放位置);
                //無驗證碼.
                //創建新的驗證資訊.
                StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                sw1.WriteLine(MKey);
                sw1.Flush();
                sw1.Close();
                string NewPSKey;
                NewPSKey = 產生隨機碼();
                StreamWriter sw2 = new StreamWriter(MPSPosition);
                sw2.WriteLine(NewPSKey);
                sw2.Flush();
                sw2.Close();
                MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _PSKEYCORRECT = false;
                return;
            }
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            FileInfo mk = new FileInfo(MKeyPostiion);
            FileInfo mps = new FileInfo(MPSPosition);
            string getMKey = null;
            string getmpsKey = null;
            string gets = "";
            if(mk.Exists)
            {
                //讀取檔案的第一行.
                StreamReader sr1 = new StreamReader(MKeyPostiion);
                gets = sr1.ReadLine();
                getMKey = gets;
                sr1.Close();
            }
            if(mps.Exists)
            {
                StreamReader sr2 = new StreamReader(MPSPosition);
                getmpsKey = sr2.ReadLine();
                sr2.Close();
            }
            getmpsKey = getmpsKey == "" ? null : getmpsKey; //若空白則視為錯誤.
            //getmpsKey = 密碼解密(getmpsKey);
            if(object.Equals(getMKey, null))
            {
                //無機碼.
                if(object.Equals(getmpsKey,null))
                {
                    //無驗證碼.
                    //創建新的驗證資訊.
                    StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                    sw1.WriteLine(MKey);
                    sw1.Flush();
                    sw1.Close();
                    string NewPSKey;
                    NewPSKey = 產生隨機碼();
                    StreamWriter sw2 = new StreamWriter(MPSPosition);
                    sw2.WriteLine(NewPSKey);
                    sw2.Flush();
                    sw2.Close();
                    MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _PSKEYCORRECT = false;
                    return;
                }
                else
                {
                    //有驗證密碼.
                    _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                    Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey,"您的驗證資訊需更新");
                    fkeyy.ShowDialog();//強制顯示.

                    if(_PSKEYCORRECT)
                    {
                        //驗證通過.
                        //更新機碼.
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(MKey);
                        sw1.Flush();
                        sw1.Close();
                        MessageBox.Show("您已成功更新驗證資訊!");
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        //驗證取消.
                        MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }

                }
            }//有無MKKey.
            else
            {
                //有機碼.
                if(getMKey!=MKey)
                {
                    //機碼不正確.
                    //提示機碼錯誤,需要提供驗證才能再次設定.
                    //若驗證密碼失效,則表示整體驗證系統崩潰,則全部重新設定.
                    if(object.Equals(getmpsKey, null))
                    {
                        //驗證密碼錯誤,視為整體崩潰.
                        string NewPSKey;
                        NewPSKey = 產生隨機碼();
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(MKey);
                        sw1.Flush();
                        sw1.Close();
                        StreamWriter sw2 = new StreamWriter(MPSPosition);
                        sw2.WriteLine(NewPSKey);
                        sw2.Flush();
                        sw2.Close();
                        MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        //有驗證密碼,但機碼不正確.
                        _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                        Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey,"您的驗證資訊需重新設定");
                        fkeyy.ShowDialog();//強制顯示.

                        if(_PSKEYCORRECT)
                        {
                            //更新機碼與驗證密碼.
                            string NewPSKey;
                            NewPSKey = 產生隨機碼();
                            StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                            sw1.WriteLine(MKey);
                            sw1.Flush();
                            sw1.Close();
                            StreamWriter sw2 = new StreamWriter(MPSPosition);
                            sw2.WriteLine(NewPSKey);
                            sw2.Flush();
                            sw2.Close();
                            MessageBox.Show("您已成功更新驗證資訊!您新的驗證密碼為" + NewPSKey +",請務必記錄下您的驗證密碼","驗證更新",MessageBoxButtons.OK ,MessageBoxIcon.Information);
                            _PSKEYCORRECT = false;
                            return;
                        }
                        else
                        {
                            MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _PSKEYCORRECT = false;
                            return;
                        }
                    }//有無驗證密碼.
                }//得到的機碼不一致.
                else
                {
                    //機碼一致,但是密碼可能受損.
                    if(object.Equals(getmpsKey,null))
                    {
                        //驗證密碼受損.
                        string NewPSKey;
                        NewPSKey = 產生隨機碼();
                        StreamWriter sw2 = new StreamWriter(MPSPosition);
                        sw2.WriteLine(NewPSKey);
                        sw2.Flush();
                        sw2.Close();
                        MessageBox.Show("您已成功更新驗證密碼,您新的驗證密碼是:" + NewPSKey + ",請務必紀錄起來","驗證密碼更新",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        if(MessageBox.Show("您的驗證資訊已完整,您是否要繼續更新資訊(包含新的驗證密碼)?", "驗證系統", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.OK)
                        {
                            //更新機碼設定(其實不用更新)與密碼.
                            _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                            Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey, "您需要輸入舊的驗證密碼");
                            fkeyy.ShowDialog();//強制顯示.

                            if(_PSKEYCORRECT)
                            {
                                //驗證通過.
                                string NewPSKey;
                                NewPSKey = 產生隨機碼();
                                StreamWriter sw2 = new StreamWriter(MPSPosition);
                                sw2.WriteLine(NewPSKey);
                                sw2.Flush();
                                sw2.Close();
                                MessageBox.Show("您已成功更新驗證密碼,您新的驗證密碼是:" + NewPSKey + ",請務必紀錄起來", "驗證密碼更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _PSKEYCORRECT = false;
                                return;

                            }
                            else
                            {
                                //取消操作.
                                MessageBox.Show("取消操作", "驗證設定更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _PSKEYCORRECT = false;
                                return;
                            }

                        }
                        else
                        { 
                            _PSKEYCORRECT = false;
                            return;
                        }
                    }
                }
            }

            //方法:
            // 1. 取得此電腦mac與CPU資訊.
            //     或是CPU訊息(但這是好方法嗎 ?).
            // 2. 隨機產生一組密碼. 
            // 3. 將此資訊透過簡單加密填入新建立之機碼內.

            //若已有機碼,則需要輸入驗證密碼.
            //string keyPathString = "SOFTWARE\\VESD_TOOL";
            //string pathstring = null;
            //string BuiltPASSKey = 取得內存密碼();//NULL表示沒有此值.
            //Microsoft.Win32.RegistryKey startnn=Microsoft.Win32.Registry.LocalMachine;
            //Microsoft.Win32.RegistryKey programnn=null; //= startnn.OpenSubKey(keyPathString);
            //try
            //{
            //    //startnn = Microsoft.Win32.Registry.LocalMachine;
            //    programnn= startnn.OpenSubKey(keyPathString);
            //}
            //catch
            //{
            //    //沒有此機碼位置.
            //    //直接建立新的.
            //    string NewPSKey;
            //    NewPSKey = 產生隨機碼();
            //    MessageBox.Show(NewPSKey.ToString());
            //    //設定新的機碼,並且設定新的PASS KEY.
            //    Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //    Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //    programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //    programName.SetValue("PASS_CODE", NewPSKey);
            //    programName.Close();

            //    MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    _PSKEYCORRECT = false;
            //    return;
            //}
            //if(programnn!=null)
            //{
            //    try
            //    {
            //        pathstring = (string)programnn.GetValue("PATH_S");//獲取機碼.
            //    }
            //    catch
            //    {

            //    }

            //    //比對機碼是否正確.
            //    if(pathstring!=MKey)
            //    {
            //        //機碼不一樣.
            //        //提示機碼錯誤,需要提供驗證才能再次設定.
            //        //若驗證密碼失效,則表示整體驗證系統崩潰,則全部重新設定.
            //        if (object.Equals(BuiltPASSKey, null))
            //        {
            //            string NewPSKey;
            //            NewPSKey = 產生隨機碼();
            //            //設定新的機碼,並且設定新的PASS KEY.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //            programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //            programName.SetValue("PASS_CODE",NewPSKey);
            //            programName.Close();

            //            MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //        else
            //        {
            //            //仍有合理之密碼,需要提供密碼來完成驗證,完成驗證後才能更新機碼,否則視為取消操作.
            //            _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
            //            Form_EnterKey fkeyy = new Form_EnterKey(this, BuiltPASSKey);
            //            fkeyy.ShowDialog();//強制顯示.

            //            if(_PSKEYCORRECT)
            //            {
            //                //若驗證正確,則更新驗證機碼.
            //                Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //                Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //                programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));
            //                MessageBox.Show("您已成功更新驗證機碼!");
            //                _PSKEYCORRECT = false;
            //                return;
            //            }
            //            else
            //            {
            //                //驗證失敗[使用者取消].
            //                MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                _PSKEYCORRECT = false;
            //                return;
            //            }


            //         }//是否驗證密碼仍可以辨識.
            //    }//機碼是否正確.
            //    else
            //    {
            //        //機碼正確,但若密碼損毀,則重新提供.
            //        if(object.Equals(BuiltPASSKey,null))
            //        {
            //            string NewPSKey;
            //            NewPSKey = 產生隨機碼();
            //            //設定新的機碼,並且設定新的PASS KEY.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //            programName.SetValue("PASS_CODE", NewPSKey);
            //            programName.Close();
            //            MessageBox.Show("由於您的驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }
            //    } //有機碼,但機碼是否正確之Loop.
            //}
            //else
            //{
            //    //沒有此鍵,需根據是否有密碼Key來決定是否須提供驗證:
            //    //1. 已有密碼Key,則需輸入密碼.
            //    //2. 沒有密碼Key,則直接創建密碼Key與驗證碼,並顯示驗證密碼給使用者.
            //    if(object.Equals(BuiltPASSKey,null))
            //    {
            //        //沒有正確的驗證密碼且機碼不存在.
            //        //創建新的驗證密碼與機碼.
            //        string NewPSKey;
            //        NewPSKey = 產生隨機碼();
            //        //設定新的機碼,並且設定新的PASS KEY.
            //        Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //        Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //        programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //        programName.SetValue("PASS_CODE", NewPSKey);
            //        programName.Close();

            //        MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        _PSKEYCORRECT = false;
            //        return;

            //    }
            //    else
            //    {
            //        //沒有機碼,但仍有驗證密碼.
            //        _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
            //        Form_EnterKey fkeyy = new Form_EnterKey(this, BuiltPASSKey);
            //        fkeyy.ShowDialog();//強制顯示.

            //        if (_PSKEYCORRECT)
            //        {
            //            //若驗證正確,則更新驗證機碼.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //            programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));
            //            MessageBox.Show("您已成功更新驗證機碼!");
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //        else
            //        {
            //            //驗證失敗[使用者取消].
            //            MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //    }

            //    //Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //    //Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //    //programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //    //programName.Close();
            //}
            //MessageBox.Show(GetCPUID());
            //MessageBox.Show(GetMacAddress());

        }
        private string 產生隨機碼()
        {
            int n = 10;//碼數.
            RealRandom rr = new RealRandom();
            int getASCII;
            string ResultR = "";
            //產生ASCII CODE以供轉換到Char.
            //數字範圍:
            //48(0)~57(9).
            //大寫字母範圍:
            //65(A)~90(Z)
            //小寫字母範圍:
            //97(a)~122(z).
            int i = 1;
            while(i<=n)
            {
                getASCII = rr.Next(47, 123);
                if((getASCII>=48 && getASCII<=57)|| (getASCII>=65 && getASCII<=90) || (getASCII>=97 && getASCII<=122))
                {
                    //Ok.
                    ResultR += Convert.ToChar(getASCII);
                    i = i + 1;
                }
                
            }
            return ResultR;
        }
        //private string 取得內存密碼()
        //{
        //    string CodeValue = null ;
        //    string keyPathString = "SOFTWARE\\VESD_TOOL";
        //    MessageBox.Show("H");
        //    string pathstring = null;
        //    try
        //    {
        //        Microsoft.Win32.RegistryKey startnn = Microsoft.Win32.Registry.LocalMachine;
        //        Microsoft.Win32.RegistryKey programnn = startnn.OpenSubKey(keyPathString);
        //        if (programnn != null)
        //        {
        //            try
        //            {
        //                CodeValue = (string)programnn.GetValue("PASS_CODE");
        //            }
        //            catch
        //            {
        //                //Do nothing.
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return CodeValue;
        //}
        public bool 檢視目前是否已有合理認證(ref string Msg)
        {
            string F1 = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string F2 = 密碼轉16位密碼計算(F1);
            string NewKey = 密碼16位碼再加密(F2);
            string MKeyPostiion = 驗證機碼存放位置 + "\\" + "MKEY.KEY";
            FileInfo mk = new FileInfo(MKeyPostiion);
            string getMKey = null;
            string gets = "";
            if (mk.Exists)
            {
                //讀取檔案的第一行.
                StreamReader sr1 = new StreamReader(MKeyPostiion);
                gets = sr1.ReadLine();
                getMKey = gets;//取得轉換完成之密碼.如1234-5678
                sr1.Close();
            }
            if(object.Equals(getMKey,null))
            {
                //沒有.
                return false;
            }
            else
            {
                //有Key,但是否合理要檢查一下.
                if(NewKey!=getMKey)
                {
                    //失敗.
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //public bool 檢視目前是否已設定正確機碼來鎖定機器(ref string Msg)
        //{
        //    string NewKey = 密碼加密(GetCPUID() + "_" + GetMacAddress());//獲取此機器之數據並且加密.
        //    string MKeyPostiion =驗證機碼存放位置 + "\\" +  "MKEY.KEY"; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    string MPSPosition = 驗證機碼存放位置 + "\\" + "MPS.KEY";
        //    FileInfo mk = new FileInfo(MKeyPostiion);
        //    FileInfo mps = new FileInfo(MPSPosition);
        //    string getMKey = null;
        //    string getmpsKey = null;
        //    string gets = "";
        //    if (mk.Exists)
        //    {
        //        //讀取檔案的第一行.
        //        StreamReader sr1 = new StreamReader(MKeyPostiion);
        //        gets = sr1.ReadLine();
        //        getMKey = gets;
        //        sr1.Close();
        //    }
        //    if (mps.Exists)
        //    {
        //        StreamReader sr2 = new StreamReader(MPSPosition);
        //        getmpsKey = sr2.ReadLine();
        //        sr2.Close();
        //    }
        //    getmpsKey = getmpsKey == "" ? null : getmpsKey;
        //    //getMKey = 密碼解密(getMKey);//解密.
        //    if (object.Equals(getMKey,null))
        //    {
        //        if(object.Equals(getmpsKey,null))
        //        {
        //            Msg = "您的驗證資訊受損,請重新認證並取得新的驗證密碼";
        //            return false;
        //        }
        //        else
        //        {
        //            //有驗證密碼.
        //            Msg = "您的驗證資訊已遺失,請重新認證,認證過程中需輸入您舊的驗證密碼";
        //            return false;
        //        }
        //    }
        //    else
        //    {
                
        //        //有驗證機碼.
        //        if(getMKey!=NewKey)
        //        {
        //            //驗證機碼不一致.
        //            if(object.Equals(getMKey,null))
        //            {
        //                Msg = "您驗證資訊已過期,請重新更新驗證資訊並取得新的驗證密碼";
        //                return false;
        //            }
        //            else
        //            {
        //                Msg = "您的驗證資訊錯誤!請重新更新驗證資訊(過程中需輸入驗證密碼)並取得最新的驗證密碼";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            //驗證機碼相同,但可能缺失驗證密碼.
        //            if(object.Equals(getMKey,null))
        //            {
        //                Msg = "您的驗證密碼發生錯誤,可能已遺失,請立即更新最新的驗證密碼";
        //                return false;
        //            }
        //            else
        //            {
        //                Msg = "";
        //                return true;//ok.
        //            }
        //        }
        //    }
        //    //string keyPathString = "SOFTWARE\\VESD_TOOL";
        //    //string pathstring = null;
        //    //string BuiltPASSKey = 取得內存密碼();
        //    //Msg = "";

        //    //Microsoft.Win32.RegistryKey startnn = Microsoft.Win32.Registry.LocalMachine;
        //    //Microsoft.Win32.RegistryKey programnn = startnn.OpenSubKey(keyPathString);
        //    //if (programnn != null)
        //    //{
        //    //    try
        //    //    {
        //    //        pathstring = (string)programnn.GetValue("PATH_S");//獲取機碼.
        //    //    }
        //    //    catch
        //    //    {

        //    //    }
        //    //    if(object.Equals(BuiltPASSKey,null))
        //    //    {
        //    //        Msg = "您的驗證密碼遺失,請重新認證此電腦,並記錄提供的驗證密碼";
        //    //        return false;
        //    //    }
        //    //    else if(pathstring==NewKey)
        //    //    {
        //    //        //ok.
        //    //        Msg = "無問題";
        //    //        return true;
        //    //    }
        //    //    else // if(pathstring!=NewKey)
        //    //    {
        //    //        //錯誤.
        //    //        Msg = "驗證資訊錯誤,請重新認證此電腦(認證時須輸入驗證密碼)";
        //    //        return false;
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    //無機碼.
        //    //    Msg = "您目前沒有驗證資訊,請認證此電腦以便使用後續功能";
        //    //    return false;
        //    //}

        //}
        private string 機碼密碼初步加密(string InputS) //,string InputS2)
        {
            //將
            if(object.Equals(InputS, null))
            {
                return null;
            }
            string result = "";
            //string inputs3 = InputS + "_" + InputS2;
            foreach (char c in InputS)
            {
                int asciicode;
                try
                {
                    asciicode = Convert.ToInt32(c);
                    asciicode += 1;
                    result += Convert.ToChar(asciicode);
                }
                catch
                {
                    //無法轉換.
                    result += c;
                }
            }
            return result;
            //return InputS + "_" + InputS2;
        }
        public string 密碼轉16位密碼計算(string InputS)
        {
            //機碼轉換預設是8+6.
            if(object.Equals(InputS,null))
            {
                return null;
            }
            int i = 0;
            int[] A1 = new int[] { };
            Array.Resize(ref A1, 16);
            int[] A2 = new int[] { };
            Array.Resize(ref A2, 16);
            for(int ix=0;ix<A1.GetLength(0);ix++)
            {
                A1[ix] = 30;
            }
            for (int ix = 0; ix < A2.GetLength(0); ix++)
            {
                A2[ix] = 30;
            }
            int f1 = 0;
            int f2 = 0;
            string r1 = "";
            foreach(char c in InputS)
            {
                int asciicode=0;
                try
                {
                    asciicode = Convert.ToInt32(c);
                    asciicode += 1;
                }
                catch
                { }
                if(i<=15)
                {
                    A1[f1] = asciicode;
                    f1 += 1;
                }
                else if(i>16 && i<=32)
                {
                    A2[f2] = asciicode;
                    f2 += 1;
                }
                i = i + 1;
            }


            int[] A3 = A1;
            //轉換成16位.
            for(int ix=0;ix<A1.GetLength(0);ix++)
            {
                int currentnumber;
                currentnumber = A1[ix] + A2[ix];
                string temps = "";
                int testI = -9999;
                int tempcount = currentnumber;
                while(true)
                {
                    temps = tempcount.ToString();
                    tempcount = 0;
                    foreach(char c in temps)
                    {
                        testI = -9999;
                        if(int.TryParse(c.ToString(),out testI))
                        {
                            tempcount += testI;
                        }
                    }
                    if(tempcount<10)
                    {
                        break;
                    }
                    else
                    {
                        //Continue.
                    }
                }
                //
                A3[ix] = tempcount;
            }


            i = 1;
            for(int ix=0;ix<A3.GetLength(0);ix++)
            {

                    //r1 += A3[ix].ToString();

                 r1 += A3[ix].ToString();
                if(i%4==0 && i!=A3.GetLength(0))
                {
                    r1 += "-";
                }

                i = i + 1;
            }
            return r1;

        }      
        public string 密碼16位碼再加密(string inputs)
        {
            //輸入值為"2312-5678-5567-3421.
            string[] ss = inputs.Split('-');
            //Each group need to calculate the number.
            string result = "";
            for(int i=0;i<ss.GetLength(0);i++)
            {
                double a;
                double b;
                double c;
                double x;
                a = double.Parse(ss[i].Substring(0, 1));
                b = double.Parse(ss[i].Substring(1, 1));
                c = double.Parse(ss[i].Substring(2, 1));
                x = double.Parse(ss[i].Substring(3, 1));

                double y = a * Math.Pow(x, 2.0) + b * x + c;

                //Finding 二位數.
                result += 二位數轉換輸出(y);
                if(i==1)
                {
                    result += "-";
                }
            }

            return result;
        }
        private string 二位數轉換輸出(double ii)
        {
            ii = Math.Abs(ii);
            if(ii<10)
            {
                return "0" + ii.ToString();
            }
            else if(ii<100)
            {
                return ii.ToString();
            }
            else
            {
                string s = ii.ToString();
                double currentvalue = 0;
                while(true)
                {
                    currentvalue = 0;
                    foreach(char c in s)
                    {
                        currentvalue = currentvalue + int.Parse(c.ToString());
                    }
                    if(currentvalue<100)
                    {
                        break;
                    }
                }
                if(currentvalue<10)
                {
                    return "0" + currentvalue.ToString();
                }
                else
                {
                    return currentvalue.ToString();
                }
            }
        }
        //public string 密碼解密(string InputS)
        //{
        //    if(object.Equals(InputS,null))
        //    {
        //        return null;
        //    }
        //    string result = "";
        //    //string inputs3 = InputS + "_" + InputS2;
        //    foreach (char c in InputS)
        //    {
        //        int asciicode;
        //        try
        //        {
        //            asciicode = Convert.ToInt32(c);
        //            asciicode -= 1;
        //            result += Convert.ToChar(asciicode);
        //        }
        //        catch
        //        {
        //            //無法轉換.
        //            result += c;
        //        }
        //    }
        //    return result;
        //}
        //private string 密碼解密(string Inputs)
        //{
            
        //}
        public string GetCPUID()
        {
            try
            {
                string cpuInfo = "";//序號.
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }
        public string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }
        private void bk_Validate_DoWork(object sender, DoWorkEventArgs e)
        {
            驗證Bool = 檢視目前是否已有合理認證(ref 驗證Msg);//mainForm.檢視目前是否已設定正確機碼來鎖定機器(ref 驗證Msg))    
        }
        private void bk_Validate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (驗證Bool)
            {
                //Nothing.
                TSP_Validate.BackColor = Color.Green;
            }
            else
            {
                TSP_Validate.BackColor = Color.Red;
            }
            驗證Msg = "";
        }
        #endregion 
        public string 取得中文數字碼(int N)
        {
            //整數.
            Dictionary<int, string> 數字與國字碼對照 = new Dictionary<int, string>();
            數字與國字碼對照.Add(0, "零");
            數字與國字碼對照.Add(1, "一");
            數字與國字碼對照.Add(2, "二");
            數字與國字碼對照.Add(3, "三");
            數字與國字碼對照.Add(4, "四");
            數字與國字碼對照.Add(5, "五");
            數字與國字碼對照.Add(6, "六");
            數字與國字碼對照.Add(7, "七");
            數字與國字碼對照.Add(8, "八");
            數字與國字碼對照.Add(9, "九");
            數字與國字碼對照.Add(10, "十");
            
            //目前沒有完整寫完.
            if(N<0 || N>10)
            {
                return "沒有寫完喔";
            }
            else
            {
                return 數字與國字碼對照[N];
            }
        }
        #region 發送訊息給主機
        public void 發送操作指令(string 操作訊息)
        {
            //發送訊息給固定IP.
            //目前IP寫死.
            string[] OtherIP = { "140.112.63.207" };//實體證實可傳送之主機IP位址.

            //string IP = textBox_IP.Text.ToString();
            int Port = int.Parse(PORT);
            byte[] B = Encoding.Default.GetBytes(操作訊息);
            UdpClient S = new UdpClient();
            foreach(string f in OtherIP)
            {
                S.Send(B, B.Length, f, Port);//發送訊息.
            }
            S.Close();
        }
        public string GetIP()
        {
            string name = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(name);
            IPAddress[] addr = entry.AddressList;
            if (addr[1].ToString().Split('.').Length == 4)
            {
                return addr[1].ToString();
            }
            return addr[2].ToString();
        }
        private void bk_SendFIle_DoWork(object sender, DoWorkEventArgs e)
        {
            //MessageBox.Show("HP:" + _傳送檔案使用Port);
            string tempFile = e.Argument.ToString();
            try
            {
                //MessageBox.Show(GetIP());
                //複製新的檔案為暫存檔案,以避免直接開啟檔案被鎖定.
                TcpClient tcpclient = new TcpClient();
                IAsyncResult result = tcpclient.BeginConnect(IPAddress.Parse("140.112.63.207"), int.Parse(_傳送檔案使用Port), null, null);
                bool success = result.AsyncWaitHandle.WaitOne(4000, true);
                if (!success)
                {
                    //Fail..
                    //MessageBox.Show("Fail");
                    tcpclient.Close();
                    //MessageBox.Show("Fail2");
                    e.Result = null;
                    _傳送成功與否 = false;
                    
                    //MessageBox.Show("Fail3");
                    bk_SendFIle.CancelAsync();
                }
                //MessageBox.Show("HH");
                StreamReader sw1 = new StreamReader(tempFile);// 傳送檔案路徑);
                //sw1.Close();
                //tcpclient.Connect(new IPEndPoint(IPAddress.Parse("140.112.63.207"), int.Parse(_傳送檔案使用Port)));//"140.112.63.207"), int.Parse("2015")));
                byte[] buffer = new byte[1500];
                long bytesSent = 0;
                while (bytesSent < sw1.BaseStream.Length)
                {
                    int bytesRead = sw1.BaseStream.Read(buffer, 0, 1500);
                    tcpclient.GetStream().Write(buffer, 0, bytesRead);
                    bytesSent += bytesRead;
                }
                tcpclient.Close();
                sw1.Close();
                _傳送成功與否 = true;
                
            }
            //catch()
            catch // (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());
                //Fail to send file.
                _傳送成功與否 = false;
            }
        }

        private void bk_SendFIle_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("Done");
            FileInfo ftemp = new FileInfo(_傳送檔案暫時名稱);
            string tempFile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD\\TempOutputingFile" + ftemp.Extension;
            //MessageBox.Show("H");
            //傳送成功.
            //if (!object.Equals(_傳送檔案暫時名稱, null))
            //{


                if (File.Exists(tempFile))
                {
                    File.Delete(tempFile);
                }
            //}

            //若傳送失敗,則考慮再傳送一次.
            if (_傳送成功與否)
            {
                _傳送檔案暫時名稱 = null;
            }
            else
            {
                
                //bk_SendFIle.RunWorkerAsync(tempFile);
                //return;
            }
            
        }
        private void bk_AccessServerForDownload_DoWork(object sender, DoWorkEventArgs e)
        {
            //MessageBox.Show("H");
            bool GoingToDown = false;
            string getPort = null;
            string _TempName = e.Argument.ToString();
            FileInfo ftemp = new FileInfo(_TempName);
            //string tempFile = _SystemReferenceStoreFolder + "\\TempOutputingFile" + ftemp.Extension;
            try
            {
                TcpClient tcpclient = new TcpClient();
                //tcpclient.SendTimeout = 1000;//只等兩秒.
                //tcpclient.ReceiveTimeout = 1000;//只等兩秒.
                IAsyncResult result = tcpclient.BeginConnect(IPAddress.Parse("140.112.63.207"), 2017, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(1000, true);
                if(!success)
                {
                    //Fail..
                    //MessageBox.Show("Fail");
                    tcpclient.Close();
                    //MessageBox.Show("Fail2");
                    e.Result = null;
                    _傳送成功與否 = false;
                    //MessageBox.Show("Fail3");
                    bk_AccessServerForDownload.CancelAsync();
                }
                //tcpclient.Connect("140.112.63.207", 2017);
                Stream str = tcpclient.GetStream();
                //UTF8Encoding asen = new UTF8Encoding();
                //ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = Encoding.Default.GetBytes("TRANSFER:" +  _LoginInUserID + "\n" + _LoginInUserName + "\n" + ftemp.Name);
                str.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];

                int k = str.Read(bb, 0, 100);//讀取回傳訊息.
                //str.Read(bb, 0, 100);
                string news="";// = Encoding.Default.GetString(bb);
                for (int i = 0; i < k; i++)
                {
                    news = news + Convert.ToChar(bb[i]);
                }
                //MessageBox.Show(news);
                if (news != "")
                {
                    if (news.IndexOf("OK:") != -1)
                    {
                        
                        getPort =news.Replace("OK:","");
                        GoingToDown = true;
                        
                    }
                    else
                    {
                        GoingToDown = false;
                    }
                }
                tcpclient.Close();
                if(!GoingToDown)
                {
                    //MessageBox.Show("Fails to send file");
                    e.Result = null;
                    bk_AccessServerForDownload.CancelAsync();
                    return;
                }

                //MessageBox.Show(getPort);
                System.Threading.Thread.Sleep(8000);


                TcpClient tcpclient2 = new TcpClient();
                //IAsyncResult result2 = tcpclient.BeginConnect(IPAddress.Parse("140.112.63.207"), int.Parse(getPort), null, null);
                //bool success2 = result2.AsyncWaitHandle.WaitOne(1500, true);
                //if (!success2)
                //{
                    //Fail..
                    //MessageBox.Show("Fail");
                 //   tcpclient2.Close();
                    //MessageBox.Show("Fail2");
                 //   e.Result = null;
                    _傳送成功與否 = false;

                //   MessageBox.Show("Fail3");
                //bk_SendFIle.CancelAsync();
                //    bk_AccessServerForDownload.CancelAsync();
                // }
                //MessageBox.Show("HH");
                //MessageBox.Show(_系統發送暫時檔案名稱);
                StreamReader sw1 = new StreamReader(_系統發送暫時檔案名稱);// 傳送檔案路徑);
                //sw1.Close();
                tcpclient2.Connect(new IPEndPoint(IPAddress.Parse("140.112.63.207"), int.Parse(getPort)));//"140.112.63.207"), int.Parse("2015")));
                byte[] buffer = new byte[1500];
                long bytesSent = 0;
                while (bytesSent < sw1.BaseStream.Length)
                {
                    int bytesRead = sw1.BaseStream.Read(buffer, 0, 1500);
                    tcpclient2.GetStream().Write(buffer, 0, bytesRead);
                    bytesSent += bytesRead;
                }
                tcpclient2.Close();
                sw1.Close();
                _傳送成功與否 = true;


                //e.Result = "OK" + _TempName + "\n" + getPort;
                //MessageBox.Show("There");

            }
            catch // (Exception ex)
            {
                //MessageBox.Show(ex.Message.ToString());//!!!!!!!!
                //MessageBox.Show(ex.StackTrace.ToString());//!!!!!!!
                e.Result = null;
            }
        }
        private void bk_AccessServerForDownload_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            FileInfo ftemp = new FileInfo(_傳送檔案暫時名稱);
            //string tempFile = _SystemReferenceStoreFolder + "\\TUP_" + DateTime.Now.ToString("yyyyMMddHHmm") + ftemp.Extension;
            //MessageBox.Show("H");
            //傳送成功.
            //if (!object.Equals(_傳送檔案暫時名稱, null))
            //{


            if (File.Exists(_系統發送暫時檔案名稱))
            {
                File.Delete(_系統發送暫時檔案名稱);
            }
            //}
            //_系統發送暫時檔案名稱 = null;
            _傳送檔案暫時名稱 = null;
            //MessageBox.Show(e.Result.ToString());
            //bool GoingToDown = false;
            //if (object.Equals(e.Result,null))
            //{
            //    //Fails.
            //    _傳送檔案使用Port = null;
            //    _傳送檔案暫時名稱 = null;
            //    _傳送成功與否 = false;
            //    GoingToDown = false;
            //    _Server連接回傳字串 = null;
            //    _Server是否可以傳送 = false;

            //}
            //else
            //{
            //    string s = e.Result.ToString();
            //    string[] ss = s.Replace("OK", "").Split('\n');
            //    _傳送檔案暫時名稱 = ss[0];
            //    _傳送檔案使用Port = ss[1];
            //    _Server是否可以傳送 = true;
            //    _Server連接回傳字串 = ss[1];
            //    GoingToDown = true;
            //}




            //if (!GoingToDown)
            //{
            //    //失敗.
            //    return;
            //}


            //_傳送成功與否 = false;
            ////MessageBox.Show("P1");
            ////_傳送檔案暫時名稱 = ;
            ////_傳送檔案使用Port = ;// getPort;
            //bk_SendFIle.RunWorkerAsync(_傳送檔案暫時名稱);
        }
        private string _系統發送暫時檔案名稱 = null;
        private bool _傳送成功與否 = false;
        private string _傳送檔案暫時名稱 = null;
        private string _傳送檔案使用Port=null;
        //private bool _Server是否可以傳送 = false;
        //private string _Server連接回傳字串 = null;
        public void 發送檔案給主機(string 傳送檔案路徑)
        {

            //MessageBox.Show("H2");
            FileInfo ftemp = new FileInfo(傳送檔案路徑);
            if (!ftemp.Exists)
            {
                return;
            }

            //string tempFile = _SystemReferenceStoreFolder + "\\TUP_" + DateTime.Now.ToString("yyyyMMddHHmm") + ftemp.Extension;
            _系統發送暫時檔案名稱= _SystemReferenceStoreFolder + "\\TUP_" + DateTime.Now.ToString("yyyyMMddHHmm") + ftemp.Extension;
            if (File.Exists(_系統發送暫時檔案名稱))
            {
                try
                {
                    File.Delete(_系統發送暫時檔案名稱);
                }
                catch
                {
                    //表示此檔案無法移動,
                    return;
                }
            }
            File.Copy(傳送檔案路徑, _系統發送暫時檔案名稱, true);


            //連接Server的檔案下載端.
            //_Server是否可以傳送 = false;
            _傳送檔案暫時名稱 = 傳送檔案路徑;
            bk_AccessServerForDownload.RunWorkerAsync(傳送檔案路徑);



        }
        public string MyIP()
        {
            string hn = Dns.GetHostName();//取得本機電腦名稱.
            IPAddress[] ip = Dns.GetHostEntry(hn).AddressList;//取得本機IP陣列.
            foreach (IPAddress it in ip)
            {
                if (it.AddressFamily == AddressFamily.InterNetwork)//如果是IP4.
                {
                    return it.ToString();
                }
            }
            return "";

        }
        private void Listen()
        {
            //監聽程序.
            //目前沒有在使用,使用此程式會需要通過使用者帳號控制之認證.
            //PORT鎖定.

            //int Port = int.Parse(PORT);//設定監聽用的通訊戶.
            //U = new UdpClient(Port);//建立UDP監聽器實體
            ////建立本機端點資訊.
            //IPEndPoint EP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port);
            //while (true) //持續監聽的無限迴圈,有資訊就處理,無則繼續等待.
            //{
            //    byte[] B = U.Receive(ref EP);
            //    //textBox_GetWord.Text += (Encoding.Default.GetString(B) + Environment.NewLine);
            //}

        }
        #endregion
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        private void 測試傳送遠端ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TcpClient tcpclient = new TcpClient();
                tcpclient.Connect("140.112.63.207", 8001);
                Stream str = tcpclient.GetStream();
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes("This the test message");
                str.Write(ba, 0, ba.Length);
                byte[] bb = new byte[100];
                int k = str.Read(bb, 0, 100);
                string news = "";
                for (int i = 0; i < k; i++)
                {
                    news = news + Convert.ToChar(bb[i]);
                }
                MessageBox.Show(news);
                tcpclient.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                MessageBox.Show(ex.StackTrace.ToString());
            }
        }
        private void 測試密碼轉換ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string TestCode="1472-8923-3516-9864";
            MessageBox.Show(密碼16位碼再加密(TestCode));

        }
        private void 關閉此軟體ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //根據設定決定關閉軟體時的動作.
            if(_RemoveLogInDataWhenClosing)
            {
                //刪除使用者登入資訊.
                if(File.Exists(_SystemReferenceStoreFolder + "\\LoginInUserInfo.txt"))
                {
                    try
                    {
                        File.Delete(_SystemReferenceStoreFolder + "\\LoginInUserInfo.txt");
                    }
                    catch
                    {

                    }
                }
                    
            }


            //儲存新的系統設定.
            SavingProgramSystemReference();
        }
        private void 軟體偏好設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_UserSetting frm_User = new Form_UserSetting(this);
            frm_User.ShowDialog();
            SavingProgramSystemReference();
        }


    }
}
