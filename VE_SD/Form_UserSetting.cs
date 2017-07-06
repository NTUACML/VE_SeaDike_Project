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
    public partial class Form_UserSetting : Form
    {
        public Form_UserSetting()
        {
            InitializeComponent();
        }
        private Form1 mainForm = null;
        public Form_UserSetting(Form callingForm)
        {
            mainForm = callingForm as Form1;//傳入物件參考.
            InitializeComponent();
        }

        private void Form_UserSetting_Load(object sender, EventArgs e)
        {
            tabControl1.Appearance = TabAppearance.Buttons;
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.ItemSize = new Size(0, 1);

            //根據目前使用者偏好設定,顯示相關之控制項狀態.
            chk_RemoveUserLog.Checked = mainForm.RemoveLoginDataWhenClosing;
            chk_ServiceOut.Checked = mainForm.提供服務訊息;
            cmb_軟體開啟時的視窗大小.SelectedItem = mainForm.軟體開啟時的視窗大小;

            chk_防波堤預設填入數字.Checked = mainForm.防波堤檢核開啟時預設數字;
            chk_碼頭預設填入數字.Checked = mainForm.碼頭檢核開啟時預設數字;
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_OK_Click(object sender, EventArgs e)
        {
            //將勾選之設定傳回form1的變數內.

            mainForm.RemoveLoginDataWhenClosing = chk_RemoveUserLog.Checked;
            mainForm.提供服務訊息 = chk_ServiceOut.Checked;
            mainForm.軟體開啟時的視窗大小 = cmb_軟體開啟時的視窗大小.SelectedItem.ToString();
            mainForm.防波堤檢核開啟時預設數字 = chk_防波堤預設填入數字.Checked;
            mainForm.碼頭檢核開啟時預設數字 = chk_碼頭預設填入數字.Checked;
            //完成後關閉表單.
            this.Close();
        }
    }
}
