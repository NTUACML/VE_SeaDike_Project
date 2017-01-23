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
    public partial class Form_SDM : Form
    {
        public Form_SDM()
        {
            InitializeComponent();
        }



        private void Form_SDM_Load(object sender, EventArgs e)
        {
            label_ShowBriefInfo.Text = "";

        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //顯示對應的說明.
            label_ShowBriefInfo.Text = "目前還沒有詳細說明";

        }

        private void linkLabel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //滑鼠點按兩下,進入此檢核項目視窗.
            Form_RDExamProgress frdexam = new Form_RDExamProgress();
            frdexam.ShowDialog();
        }

        private void linkLabel1_DoubleClick(object sender, EventArgs e)
        {
            Form_RDExamProgress frdexam = new Form_RDExamProgress();
            frdexam.ShowDialog();

        }
    }
}
