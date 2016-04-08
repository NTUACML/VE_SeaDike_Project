using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;


namespace VE_SD
{
    public partial class Form_Login : Form
    {
        string LoginFileFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\VSSD";
        
        string LeastUserLoginInFile = "LoginInUserInfo.txt";
        public Form_Login()
        {
            InitializeComponent();
        }
        private string _LoginCondiotn;
        private Form1 mainForm = null;
        public Form_Login(Form callingForm,string LoginIn)
        {
            _LoginCondiotn = LoginIn;
            //"Exit Then End All"
            //"Exit Then Return"
            mainForm = callingForm as Form1;


            InitializeComponent();
        }
        private void Form_Login_Load(object sender, EventArgs e)
        {
            //加入直接顯示上一次最後登入之使用者資訊檔案.
            //MessageBox.Show(LoginFileFolder);
            //MessageBox.Show(Dns.GetHostName());
            FileInfo fi = new FileInfo(LoginFileFolder + "\\" + LeastUserLoginInFile); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (fi.Exists)
            {
                StreamReader sr = new StreamReader(LoginFileFolder + "\\" + LeastUserLoginInFile);//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                string s1, s2;
                try
                {
                    //第一行:使用者ID.
                    //第二行:使用者名稱.
                    s1 = sr.ReadLine();
                    if (s1 != "")
                    {
                        textBox_UserID.Text = s1;
                    }
                    s2 = sr.ReadLine();
                    if (s2 != "")
                    {
                        textBox_UserName.Text = s2;
                    }
                }
                catch
                {
                    textBox_UserID.Text = "";
                    textBox_UserName.Text = "";
                }
                sr.Close();
            }//是否此檔案存在.

            if(_LoginCondiotn=="Exit Then End All")
            {
                //Do nothing currently.
                label_Show.Text = "";
            }
            else if(_LoginCondiotn=="Exit Then Return")
            {
                this.Text = "變更登入之使用者";
                this.btn_Login.Text = "變更";
                this.btn_cancel.Text = "取消變更";
                label_Show.Text = "請輸入新的使用者名稱與員工編號";
            }
        }
        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (_LoginCondiotn == "Exit Then End All")
            {
                if (MessageBox.Show("您確定要取消登入? 取消登入後將自動關閉程式!", "登入取消", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.Close();
                    mainForm.Close();//關閉所有執行程序.
                }
                else
                {
                    //不做任何事情.
                }
            }
            else if(_LoginCondiotn== "Exit Then Return")
            {
                //不做任何事情.
                this.Close();
            }
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            //檢查登入資訊是否合理.
            string getID = textBox_UserID.Text;
            string getName = textBox_UserName.Text;

            if(getID=="" || getName=="" )
            {
                //錯誤.
                MessageBox.Show("您沒有填入正確的資訊!!", "登入使用者資訊", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //寫出檔案.
            if(!Directory.Exists(LoginFileFolder))
            {
                Directory.CreateDirectory(LoginFileFolder);
            }
            StreamWriter sw= new StreamWriter(LoginFileFolder + "\\" + LeastUserLoginInFile);
            sw.WriteLine(getID);
            sw.WriteLine(getName);
            sw.Flush();
            sw.Close();

            this.mainForm.發送操作指令("電腦主機'" + Dns.GetHostName() + "'(MAC IP = '" + mainForm.GetMacAddress() + "', IP(IPV4) = '" + mainForm.MyIP() + "')有登入活動,員工編號為'" + getID + "',員工名稱為'" + getName + "',時間為:" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
            //設定表單變數.
            //http://stackoverflow.com/questions/4822980/how-to-access-a-form-control-for-another-form
            this.mainForm.LoginTextSetting = "OK"; //*************************
            this.mainForm.LoginInUserID = getID;
            this.mainForm.LoginInUserName = getName;
            this.Close();
        }


    }
}
