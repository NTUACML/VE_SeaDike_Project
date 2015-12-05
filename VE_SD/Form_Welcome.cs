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
    public partial class Form_Welcome : Form
    {
        int timerticker = 0;

        public Form_Welcome()
        {
            InitializeComponent();
        }

        private void Form_Welcome_Load(object sender, EventArgs e)
        {
            label1.Text = "";
            timer1.Interval = 1000;
            timer1.Start();
            timerticker = 0;
            //Application.DoEvents();
            //System.Threading.Thread.Sleep(10000);
            //Form1 fm1 = new Form1();
            //fm1.ShowDialog();
            //this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timerticker += 1;
            if (timerticker == 3)
            {
                label1.Text = "正在載入必須的檔案";
            }
            if (timerticker == 10)
            {
                label1.Text = "正在啟動程式...";
                //System.Threading.Thread.Sleep(2000);
            }
            if (timerticker == 15)
            {
                timer1.Stop();
                this.Close();
            }
            //Form1 fm1 = new Form1();
            //fm1.Show();
            //this.Visible = false;
            //Application.Run(fm1);
            //}
        }
    }
}
