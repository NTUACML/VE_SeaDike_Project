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
        }
    }
}
