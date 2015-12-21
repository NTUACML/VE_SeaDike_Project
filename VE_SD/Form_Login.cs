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
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();
        }
        private Form1 mainForm = null;
        public Form_Login(Form callingForm)
        {
            mainForm = callingForm as Form1;
            InitializeComponent();
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您確定要取消登入? 取消登入後將自動關閉程式!", "登入取消", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                this.Close();
            }
            else
            {
                //不做任何事情.
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            //檢查登入資訊是否合理.
            string getName = textBox_UserName.Text;
            string getcode = textBox_Code.Text;

            //檢查機制暫時沒寫.

            //

            //設定表單變數.
            //http://stackoverflow.com/questions/4822980/how-to-access-a-form-control-for-another-form
            this.mainForm.LoginTextSetting = "OK"; //*************************
            this.Close();
        }

    }
}
