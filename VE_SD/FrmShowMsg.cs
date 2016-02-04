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
        public FrmShowMsg(string InputMsg,string InputTitle)
        {
            textBox1.Text = InputMsg;
            this.Text = InputTitle;
            InitializeComponent();
        }

        private void btn_Sure_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
