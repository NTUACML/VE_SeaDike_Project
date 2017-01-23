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
    public partial class FrmShowMsg : Form
    {
        public FrmShowMsg()
        {
            InitializeComponent();
        }
        string ShowMsg = "";
        string titleMsg = "";
        public FrmShowMsg(string InputMsg,string InputTitle)
        {
            ShowMsg = InputMsg;
            titleMsg= InputTitle;
            InitializeComponent();
        }

        private void btn_Sure_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmShowMsg_Load(object sender, EventArgs e)
        {
            this.Text = titleMsg;// = ShowMsg;
            this.textBox1.Text  = ShowMsg;

            this.textBox1.SelectionStart = 0;
            this.textBox1.DeselectAll();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                //MessageBox.Show("Yes");
                ((TextBox)sender).SelectAll();
                e.Handled = true;
            }
            else if (e.Control && e.KeyCode == Keys.C)
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;//完全禁止輸入字.
        }
    }
}
