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
    public partial class Form_EnterKey : Form
    {
        private string _GetCode;
        private string _ExplainS;
        public Form_EnterKey()
        {
            InitializeComponent();
        }
        private Form1 mainForm = null;
        public Form_EnterKey(Form callingForm,string GetCODE,string explain)
        {
            mainForm = callingForm as Form1;
            _GetCode = GetCODE;
            _ExplainS = explain;
            InitializeComponent();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            if(textBox_CODE.Text.ToString()!=_GetCode)
            {
                MessageBox.Show("密碼錯誤!", "認證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                //完成認證.
                mainForm.PSKEYCONDITIONCORRECT = true;
                this.Close();
            }
        }

        private void Form_EnterKey_Load(object sender, EventArgs e)
        {
            mainForm.PSKEYCONDITIONCORRECT = false; //變更為驗證失敗.
            this.label_SHOW.Text = _ExplainS;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            mainForm.PSKEYCONDITIONCORRECT = false;
            this.Close();
        }

        private void Form_EnterKey_FormClosing(object sender, FormClosingEventArgs e)
        {
            //什麼都不做.
        }
    }
}
