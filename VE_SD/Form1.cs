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

namespace VE_SD
{
    public partial class Form1 : Form
    {
        string _LoginInUserID;
        string _LoginInUserName;
        bool loginsuccess = false;

        public Form1()
        {
            InitializeComponent();
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
        private void Form1_Load(object sender, EventArgs e)
        {
            label_LoginCond.Text = "NO";
            TSP_Progressbar.Visible = false;
            TSSTATUS_label.Text = "歡迎使用海堤檢核程式!";
            Form_Welcome fwel = new Form_Welcome();
            fwel.ShowDialog();


            //跳出登入視窗.
            System.Threading.Thread.Sleep(1000);
            Form_Login flogin = new Form_Login(this);
            flogin.ShowDialog();


            //更改Toolstrip排列,Layout方法要更改為Overflow with horizontal[Horizontal Stack with overflow]
            TSP_UserInfoShow.Text = "目前登入使用者ID:" + _LoginInUserID + ",名稱為" + _LoginInUserName;
            TSP_UserInfoShow.Alignment = ToolStripItemAlignment.Right;
            TSP_UserInfoShow.RightToLeft = RightToLeft.No;
            //更改排列順序.
            //TSP_Progressbar.Alignment = ToolStripItemAlignment.Right;
            //TSP_Progressbar.RightToLeft = RightToLeft.No;
            //TSP_Progressbar.Visible = true;
            //TSP_Progressbar.Style = ProgressBarStyle.Marquee;
        }

        private void 海堤檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //開啟海堤檢核主表單.
            Form_SDM fsdm = new Form_SDM();
            fsdm.ShowDialog();

        }

        private void 海堤檢核給Kavy玩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ForKavyTest  kavytest = new Form_ForKavyTest();
            kavytest.ShowDialog();

        }

        private void btn_StandardRDC_Click(object sender, EventArgs e)
        {
            Form_RDExamProgress frdexam = new Form_RDExamProgress(this);
            frdexam.ShowDialog();
        }
        private void btn_StandardRDC_MouseEnter(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "此為海堤檢核程式,使用者須輸入計算所需之參數以求得海堤設計是否符合所需之標準";
            
        }
        private void btn_StandardRDC_MouseLeave(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "";
        }

        #region "檢視說明"

        private void 檢示使用者說明書ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            p.StartInfo.FileName = "EPA SWMM User Manual Version 5.1.pdf";//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            p.Start();
        }
        #endregion
    }
}
