using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VE_SD
{
    public partial class Form1 : Form
    {
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
        private void Form1_Load(object sender, EventArgs e)
        {
            label_LoginCond.Text = "NO";
            TSP_Progressbar.Visible = false;
            TSSTATUS_label.Text = "歡迎使用本程式";
            Form_Welcome fwel = new Form_Welcome();
            fwel.ShowDialog();


            //跳出登入視窗.
            System.Threading.Thread.Sleep(1000);
            Form_Login flogin = new Form_Login(this);
            flogin.ShowDialog();


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
    }
}
