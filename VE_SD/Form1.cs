using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.IO;
using System.Net.NetworkInformation;
using System.Management.Instrumentation;
using System.Configuration;

namespace VE_SD
{
    public partial class Form1 : Form
    {
        string _LoginInUserID;
        string _LoginInUserName;
        bool _PSKEYCORRECT;
        bool loginsuccess = false;
        private string 驗證機碼存放位置 = "C:\\LKK";
        private string Exepath = System.IO.Directory.GetCurrentDirectory();

        public Form1()
        {
            InitializeComponent();
        }

        public string LoginTextSetting
        {
            get { return label_LoginCond.Text; }
            set { label_LoginCond.Text = value; }
        }
        public string LoginInUserID
        {
            get { return _LoginInUserID; }
            set { _LoginInUserID = value; }
        }
        public string LoginInUserName
        {
            get { return _LoginInUserName; }
            set { _LoginInUserName = value; }
        }
        public bool PSKEYCONDITIONCORRECT
        {
            get { return _PSKEYCORRECT; }
            set { _PSKEYCORRECT = value; }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            label_LoginCond.Text = "NO";
            TSP_Progressbar.Visible = false;
            TSSTATUS_label.Text = "歡迎使用海堤檢核程式!";
            Form_Welcome fwel = new Form_Welcome();
            fwel.ShowDialog();


            //跳出登入視窗.
            System.Threading.Thread.Sleep(1000);
            Form_Login flogin = new Form_Login(this,"Exit Then End All");//"Exit Then End All": 若使用者選擇不登入,則直接關閉.
            flogin.ShowDialog();


            //更改Toolstrip排列,Layout方法要更改為Overflow with horizontal[Horizontal Stack with overflow]
            TSP_UserInfoShow.Text = "目前登入使用者ID:" + _LoginInUserID + ",名稱為" + _LoginInUserName;
            TSP_UserInfoShow.Alignment = ToolStripItemAlignment.Right;
            TSP_UserInfoShow.RightToLeft = RightToLeft.No;

            TSP_ChangeUserBtn.Alignment = ToolStripItemAlignment.Right;
            TSP_ChangeUserBtn.RightToLeft = RightToLeft.No;
            //更改排列順序.
            //TSP_Progressbar.Alignment = ToolStripItemAlignment.Right;
            //TSP_Progressbar.RightToLeft = RightToLeft.No;
            //TSP_Progressbar.Visible = true;
            //TSP_Progressbar.Style = ProgressBarStyle.Marquee;
        }

        private void 海堤檢核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //此功能已取消,目前主表單已直接改為海堤檢核主表單.
            //開啟海堤檢核主表單.
            Form_RDExamProgress frdexam = new Form_RDExamProgress(this);
            frdexam.ShowDialog();
            return;


            Form_SDM fsdm = new Form_SDM();
            fsdm.ShowDialog();

        }

        private void 海堤檢核給Kavy玩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form_ForKavyTest  kavytest = new Form_ForKavyTest();
            kavytest.ShowDialog();

        }

        private void btn_StandardRDC_Click(object sender, EventArgs e)
        {
            Form_RDExamProgress frdexam = new Form_RDExamProgress(this);
            frdexam.ShowDialog();
        }
        private void btn_StandardRDC_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show(Exepath);
            this.textBox_ItemDescp.Text = "此為海堤檢核程式,使用者須輸入計算所需之參數以求得海堤設計是否符合所需之標準";
            try
            { 
              this.pictureBox_ItemDescp.Load(Exepath + "\\PIC\\STDVESD.JPG");
            }
            catch
            {
                this.pictureBox_ItemDescp.Image = null;
            }

        }
        private void btn_StandardRDC_MouseLeave(object sender, EventArgs e)
        {
            this.textBox_ItemDescp.Text = "";
            this.pictureBox_ItemDescp.Image = null;
        }
        #region "檢視說明"

        public void 檢示使用者說明書ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            { 
                Process p = new Process();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                p.StartInfo.FileName = "EPA SWMM User Manual Version 5.1.pdf";//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                p.Start();
            }
            catch
            {

            }
        }
        #endregion
        #region  更改使用者
        private void TSP_ChangeUserBtn_Click(object sender, EventArgs e)
        {
            //呼叫一開始的登入視窗出來.
            Form_Login flogin = new Form_Login(this, "Exit Then Return");//"Exit Then End All": 若使用者選擇不登入,則直接關閉.
            flogin.ShowDialog();

            //更新資訊.
            TSP_UserInfoShow.Text = "目前登入使用者ID:" + _LoginInUserID + ",名稱為" + _LoginInUserName;
        }
        #endregion
        #region 設定機碼
        private void 軟體驗證ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //驗證此軟體.

            //1. 檢查是否有驗證資訊.
            string F1 = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string F2 = 密碼轉16位密碼計算(F1);
            string NewKey = 密碼16位碼再加密(F2);
            MessageBox.Show(NewKey);
            //string MKey = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string MKeyPostiion = 驗證機碼存放位置 + "\\" + "MKEY.KEY";
            string getMKkey = null;
            string gets = "";
            this._PSKEYCORRECT = false;
            if(!Directory.Exists(驗證機碼存放位置))
            {
                //沒有驗證過.
                Form_EnterKey fkey = new Form_EnterKey(this, NewKey, "請連絡相關人員來提供您驗證密碼" + Environment.NewLine + F2);
                fkey.ShowDialog();
               
                if(!_PSKEYCORRECT)
                {
                    //失敗.
                    
                }
                else
                {
                    Directory.CreateDirectory(驗證機碼存放位置);
                    StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                    sw1.WriteLine(NewKey);
                    sw1.Flush();
                    sw1.Close();
                    this._PSKEYCORRECT = false;
                }
            }
            else
            {
                if(!File.Exists(MKeyPostiion))
                {
                    //無此檔案,視為無驗證過.
                    Form_EnterKey fkey = new Form_EnterKey(this, NewKey, "請連絡相關人員來提供您驗證密碼" + Environment.NewLine + F2);
                    fkey.ShowDialog();

                    if (!_PSKEYCORRECT)
                    {
                        //失敗.

                    }
                    else
                    {
                        Directory.CreateDirectory(驗證機碼存放位置);
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(NewKey);
                        sw1.Flush();
                        sw1.Close();
                        this._PSKEYCORRECT = false;
                    }
                }
                else
                {
                    //讀取檔案的第一行.
                    StreamReader sr1 = new StreamReader(MKeyPostiion);
                    gets = sr1.ReadLine();
                    getMKkey = gets;//取得轉換完成之密碼.如1234-5678
                    sr1.Close();

                    if(getMKkey!=NewKey)
                    {
                        //失敗.
                        //殺掉檔案,顯示失敗.
                        MessageBox.Show("您的驗證已過時,請聯絡相關人員提供最新之驗證", "驗證錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        File.Delete(MKeyPostiion);

                    }
                    else
                    {
                        //沒事.
                        MessageBox.Show("您的驗證無誤","軟體驗證",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }

            }
        }
        private void 軟體機碼設定ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string MKey = 機碼密碼初步加密(GetCPUID()+ "_" + GetMacAddress());//獲取此機器之數據.
            string MKeyPostiion =驗證機碼存放位置+ "\\" + "MKEY.KEY"; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            string MPSPosition = 驗證機碼存放位置 + "\\" + "MPS.KEY";
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!Directory.Exists(驗證機碼存放位置))
            {
                //採取自動建立此資料夾.
                Directory.CreateDirectory(驗證機碼存放位置);
                //無驗證碼.
                //創建新的驗證資訊.
                StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                sw1.WriteLine(MKey);
                sw1.Flush();
                sw1.Close();
                string NewPSKey;
                NewPSKey = 產生隨機碼();
                StreamWriter sw2 = new StreamWriter(MPSPosition);
                sw2.WriteLine(NewPSKey);
                sw2.Flush();
                sw2.Close();
                MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _PSKEYCORRECT = false;
                return;
            }
            //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            FileInfo mk = new FileInfo(MKeyPostiion);
            FileInfo mps = new FileInfo(MPSPosition);
            string getMKey = null;
            string getmpsKey = null;
            string gets = "";
            if(mk.Exists)
            {
                //讀取檔案的第一行.
                StreamReader sr1 = new StreamReader(MKeyPostiion);
                gets = sr1.ReadLine();
                getMKey = gets;
                sr1.Close();
            }
            if(mps.Exists)
            {
                StreamReader sr2 = new StreamReader(MPSPosition);
                getmpsKey = sr2.ReadLine();
                sr2.Close();
            }
            getmpsKey = getmpsKey == "" ? null : getmpsKey; //若空白則視為錯誤.
            //getmpsKey = 密碼解密(getmpsKey);
            if(object.Equals(getMKey, null))
            {
                //無機碼.
                if(object.Equals(getmpsKey,null))
                {
                    //無驗證碼.
                    //創建新的驗證資訊.
                    StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                    sw1.WriteLine(MKey);
                    sw1.Flush();
                    sw1.Close();
                    string NewPSKey;
                    NewPSKey = 產生隨機碼();
                    StreamWriter sw2 = new StreamWriter(MPSPosition);
                    sw2.WriteLine(NewPSKey);
                    sw2.Flush();
                    sw2.Close();
                    MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _PSKEYCORRECT = false;
                    return;
                }
                else
                {
                    //有驗證密碼.
                    _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                    Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey,"您的驗證資訊需更新");
                    fkeyy.ShowDialog();//強制顯示.

                    if(_PSKEYCORRECT)
                    {
                        //驗證通過.
                        //更新機碼.
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(MKey);
                        sw1.Flush();
                        sw1.Close();
                        MessageBox.Show("您已成功更新驗證資訊!");
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        //驗證取消.
                        MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }

                }
            }//有無MKKey.
            else
            {
                //有機碼.
                if(getMKey!=MKey)
                {
                    //機碼不正確.
                    //提示機碼錯誤,需要提供驗證才能再次設定.
                    //若驗證密碼失效,則表示整體驗證系統崩潰,則全部重新設定.
                    if(object.Equals(getmpsKey, null))
                    {
                        //驗證密碼錯誤,視為整體崩潰.
                        string NewPSKey;
                        NewPSKey = 產生隨機碼();
                        StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                        sw1.WriteLine(MKey);
                        sw1.Flush();
                        sw1.Close();
                        StreamWriter sw2 = new StreamWriter(MPSPosition);
                        sw2.WriteLine(NewPSKey);
                        sw2.Flush();
                        sw2.Close();
                        MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        //有驗證密碼,但機碼不正確.
                        _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                        Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey,"您的驗證資訊需重新設定");
                        fkeyy.ShowDialog();//強制顯示.

                        if(_PSKEYCORRECT)
                        {
                            //更新機碼與驗證密碼.
                            string NewPSKey;
                            NewPSKey = 產生隨機碼();
                            StreamWriter sw1 = new StreamWriter(MKeyPostiion);
                            sw1.WriteLine(MKey);
                            sw1.Flush();
                            sw1.Close();
                            StreamWriter sw2 = new StreamWriter(MPSPosition);
                            sw2.WriteLine(NewPSKey);
                            sw2.Flush();
                            sw2.Close();
                            MessageBox.Show("您已成功更新驗證資訊!您新的驗證密碼為" + NewPSKey +",請務必記錄下您的驗證密碼","驗證更新",MessageBoxButtons.OK ,MessageBoxIcon.Information);
                            _PSKEYCORRECT = false;
                            return;
                        }
                        else
                        {
                            MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _PSKEYCORRECT = false;
                            return;
                        }
                    }//有無驗證密碼.
                }//得到的機碼不一致.
                else
                {
                    //機碼一致,但是密碼可能受損.
                    if(object.Equals(getmpsKey,null))
                    {
                        //驗證密碼受損.
                        string NewPSKey;
                        NewPSKey = 產生隨機碼();
                        StreamWriter sw2 = new StreamWriter(MPSPosition);
                        sw2.WriteLine(NewPSKey);
                        sw2.Flush();
                        sw2.Close();
                        MessageBox.Show("您已成功更新驗證密碼,您新的驗證密碼是:" + NewPSKey + ",請務必紀錄起來","驗證密碼更新",MessageBoxButtons.OK,MessageBoxIcon.Information);
                        _PSKEYCORRECT = false;
                        return;
                    }
                    else
                    {
                        if(MessageBox.Show("您的驗證資訊已完整,您是否要繼續更新資訊(包含新的驗證密碼)?", "驗證系統", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)==DialogResult.OK)
                        {
                            //更新機碼設定(其實不用更新)與密碼.
                            _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
                            Form_EnterKey fkeyy = new Form_EnterKey(this, getmpsKey, "您需要輸入舊的驗證密碼");
                            fkeyy.ShowDialog();//強制顯示.

                            if(_PSKEYCORRECT)
                            {
                                //驗證通過.
                                string NewPSKey;
                                NewPSKey = 產生隨機碼();
                                StreamWriter sw2 = new StreamWriter(MPSPosition);
                                sw2.WriteLine(NewPSKey);
                                sw2.Flush();
                                sw2.Close();
                                MessageBox.Show("您已成功更新驗證密碼,您新的驗證密碼是:" + NewPSKey + ",請務必紀錄起來", "驗證密碼更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _PSKEYCORRECT = false;
                                return;

                            }
                            else
                            {
                                //取消操作.
                                MessageBox.Show("取消操作", "驗證設定更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                _PSKEYCORRECT = false;
                                return;
                            }

                        }
                        else
                        { 
                            _PSKEYCORRECT = false;
                            return;
                        }
                    }
                }
            }

            //方法:
            // 1. 取得此電腦mac與CPU資訊.
            //     或是CPU訊息(但這是好方法嗎 ?).
            // 2. 隨機產生一組密碼. 
            // 3. 將此資訊透過簡單加密填入新建立之機碼內.

            //若已有機碼,則需要輸入驗證密碼.
            //string keyPathString = "SOFTWARE\\VESD_TOOL";
            //string pathstring = null;
            //string BuiltPASSKey = 取得內存密碼();//NULL表示沒有此值.
            //Microsoft.Win32.RegistryKey startnn=Microsoft.Win32.Registry.LocalMachine;
            //Microsoft.Win32.RegistryKey programnn=null; //= startnn.OpenSubKey(keyPathString);
            //try
            //{
            //    //startnn = Microsoft.Win32.Registry.LocalMachine;
            //    programnn= startnn.OpenSubKey(keyPathString);
            //}
            //catch
            //{
            //    //沒有此機碼位置.
            //    //直接建立新的.
            //    string NewPSKey;
            //    NewPSKey = 產生隨機碼();
            //    MessageBox.Show(NewPSKey.ToString());
            //    //設定新的機碼,並且設定新的PASS KEY.
            //    Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //    Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //    programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //    programName.SetValue("PASS_CODE", NewPSKey);
            //    programName.Close();

            //    MessageBox.Show("由於您的驗證機碼與驗證密碼尚未設定,已成功建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    _PSKEYCORRECT = false;
            //    return;
            //}
            //if(programnn!=null)
            //{
            //    try
            //    {
            //        pathstring = (string)programnn.GetValue("PATH_S");//獲取機碼.
            //    }
            //    catch
            //    {

            //    }

            //    //比對機碼是否正確.
            //    if(pathstring!=MKey)
            //    {
            //        //機碼不一樣.
            //        //提示機碼錯誤,需要提供驗證才能再次設定.
            //        //若驗證密碼失效,則表示整體驗證系統崩潰,則全部重新設定.
            //        if (object.Equals(BuiltPASSKey, null))
            //        {
            //            string NewPSKey;
            //            NewPSKey = 產生隨機碼();
            //            //設定新的機碼,並且設定新的PASS KEY.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //            programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //            programName.SetValue("PASS_CODE",NewPSKey);
            //            programName.Close();

            //            MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //        else
            //        {
            //            //仍有合理之密碼,需要提供密碼來完成驗證,完成驗證後才能更新機碼,否則視為取消操作.
            //            _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
            //            Form_EnterKey fkeyy = new Form_EnterKey(this, BuiltPASSKey);
            //            fkeyy.ShowDialog();//強制顯示.

            //            if(_PSKEYCORRECT)
            //            {
            //                //若驗證正確,則更新驗證機碼.
            //                Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //                Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //                programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));
            //                MessageBox.Show("您已成功更新驗證機碼!");
            //                _PSKEYCORRECT = false;
            //                return;
            //            }
            //            else
            //            {
            //                //驗證失敗[使用者取消].
            //                MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //                _PSKEYCORRECT = false;
            //                return;
            //            }


            //         }//是否驗證密碼仍可以辨識.
            //    }//機碼是否正確.
            //    else
            //    {
            //        //機碼正確,但若密碼損毀,則重新提供.
            //        if(object.Equals(BuiltPASSKey,null))
            //        {
            //            string NewPSKey;
            //            NewPSKey = 產生隨機碼();
            //            //設定新的機碼,並且設定新的PASS KEY.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.OpenSubKey(keyPathString);
            //            programName.SetValue("PASS_CODE", NewPSKey);
            //            programName.Close();
            //            MessageBox.Show("由於您的驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            return;
            //        }
            //    } //有機碼,但機碼是否正確之Loop.
            //}
            //else
            //{
            //    //沒有此鍵,需根據是否有密碼Key來決定是否須提供驗證:
            //    //1. 已有密碼Key,則需輸入密碼.
            //    //2. 沒有密碼Key,則直接創建密碼Key與驗證碼,並顯示驗證密碼給使用者.
            //    if(object.Equals(BuiltPASSKey,null))
            //    {
            //        //沒有正確的驗證密碼且機碼不存在.
            //        //創建新的驗證密碼與機碼.
            //        string NewPSKey;
            //        NewPSKey = 產生隨機碼();
            //        //設定新的機碼,並且設定新的PASS KEY.
            //        Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //        Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //        programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //        programName.SetValue("PASS_CODE", NewPSKey);
            //        programName.Close();

            //        MessageBox.Show("由於您的驗證機碼與驗證密碼已經完全毀損,已成功重新建立之" + Environment.NewLine + "您新的驗證密碼(請務必記住此密碼)為" + NewPSKey, "您的驗證機制已重新更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        _PSKEYCORRECT = false;
            //        return;

            //    }
            //    else
            //    {
            //        //沒有機碼,但仍有驗證密碼.
            //        _PSKEYCORRECT = false;//強制變更為密碼輸入不正確.
            //        Form_EnterKey fkeyy = new Form_EnterKey(this, BuiltPASSKey);
            //        fkeyy.ShowDialog();//強制顯示.

            //        if (_PSKEYCORRECT)
            //        {
            //            //若驗證正確,則更新驗證機碼.
            //            Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //            Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //            programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));
            //            MessageBox.Show("您已成功更新驗證機碼!");
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //        else
            //        {
            //            //驗證失敗[使用者取消].
            //            MessageBox.Show("您沒有成功驗證!操作取消", "驗證失敗", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            _PSKEYCORRECT = false;
            //            return;
            //        }
            //    }

            //    //Microsoft.Win32.RegistryKey start = Microsoft.Win32.Registry.LocalMachine;
            //    //Microsoft.Win32.RegistryKey programName = start.CreateSubKey(keyPathString);
            //    //programName.SetValue("PATH_S", 密碼加密(GetCPUID(), GetMacAddress()));//寫入值.
            //    //programName.Close();
            //}
            //MessageBox.Show(GetCPUID());
            //MessageBox.Show(GetMacAddress());

        }
        private string 產生隨機碼()
        {
            int n = 10;//碼數.
            RealRandom rr = new RealRandom();
            int getASCII;
            string ResultR = "";
            //產生ASCII CODE以供轉換到Char.
            //數字範圍:
            //48(0)~57(9).
            //大寫字母範圍:
            //65(A)~90(Z)
            //小寫字母範圍:
            //97(a)~122(z).
            int i = 1;
            while(i<=n)
            {
                getASCII = rr.Next(47, 123);
                if((getASCII>=48 && getASCII<=57)|| (getASCII>=65 && getASCII<=90) || (getASCII>=97 && getASCII<=122))
                {
                    //Ok.
                    ResultR += Convert.ToChar(getASCII);
                    i = i + 1;
                }
                
            }
            return ResultR;
        }
        //private string 取得內存密碼()
        //{
        //    string CodeValue = null ;
        //    string keyPathString = "SOFTWARE\\VESD_TOOL";
        //    MessageBox.Show("H");
        //    string pathstring = null;
        //    try
        //    {
        //        Microsoft.Win32.RegistryKey startnn = Microsoft.Win32.Registry.LocalMachine;
        //        Microsoft.Win32.RegistryKey programnn = startnn.OpenSubKey(keyPathString);
        //        if (programnn != null)
        //        {
        //            try
        //            {
        //                CodeValue = (string)programnn.GetValue("PASS_CODE");
        //            }
        //            catch
        //            {
        //                //Do nothing.
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return CodeValue;
        //}
        public bool 檢視目前是否已有合理認證(ref string Msg)
        {
            string F1 = 機碼密碼初步加密(GetCPUID() + "_" + GetMacAddress());
            string F2 = 密碼轉16位密碼計算(F1);
            string NewKey = 密碼16位碼再加密(F2);
            string MKeyPostiion = 驗證機碼存放位置 + "\\" + "MKEY.KEY";
            FileInfo mk = new FileInfo(MKeyPostiion);
            string getMKey = null;
            string gets = "";
            if (mk.Exists)
            {
                //讀取檔案的第一行.
                StreamReader sr1 = new StreamReader(MKeyPostiion);
                gets = sr1.ReadLine();
                getMKey = gets;//取得轉換完成之密碼.如1234-5678
                sr1.Close();
            }
            if(object.Equals(getMKey,null))
            {
                //沒有.
                return false;
            }
            else
            {
                //有Key,但是否合理要檢查一下.
                if(NewKey!=getMKey)
                {
                    //失敗.
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        //public bool 檢視目前是否已設定正確機碼來鎖定機器(ref string Msg)
        //{
        //    string NewKey = 密碼加密(GetCPUID() + "_" + GetMacAddress());//獲取此機器之數據並且加密.
        //    string MKeyPostiion =驗證機碼存放位置 + "\\" +  "MKEY.KEY"; //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //    string MPSPosition = 驗證機碼存放位置 + "\\" + "MPS.KEY";
        //    FileInfo mk = new FileInfo(MKeyPostiion);
        //    FileInfo mps = new FileInfo(MPSPosition);
        //    string getMKey = null;
        //    string getmpsKey = null;
        //    string gets = "";
        //    if (mk.Exists)
        //    {
        //        //讀取檔案的第一行.
        //        StreamReader sr1 = new StreamReader(MKeyPostiion);
        //        gets = sr1.ReadLine();
        //        getMKey = gets;
        //        sr1.Close();
        //    }
        //    if (mps.Exists)
        //    {
        //        StreamReader sr2 = new StreamReader(MPSPosition);
        //        getmpsKey = sr2.ReadLine();
        //        sr2.Close();
        //    }
        //    getmpsKey = getmpsKey == "" ? null : getmpsKey;
        //    //getMKey = 密碼解密(getMKey);//解密.
        //    if (object.Equals(getMKey,null))
        //    {
        //        if(object.Equals(getmpsKey,null))
        //        {
        //            Msg = "您的驗證資訊受損,請重新認證並取得新的驗證密碼";
        //            return false;
        //        }
        //        else
        //        {
        //            //有驗證密碼.
        //            Msg = "您的驗證資訊已遺失,請重新認證,認證過程中需輸入您舊的驗證密碼";
        //            return false;
        //        }
        //    }
        //    else
        //    {
                
        //        //有驗證機碼.
        //        if(getMKey!=NewKey)
        //        {
        //            //驗證機碼不一致.
        //            if(object.Equals(getMKey,null))
        //            {
        //                Msg = "您驗證資訊已過期,請重新更新驗證資訊並取得新的驗證密碼";
        //                return false;
        //            }
        //            else
        //            {
        //                Msg = "您的驗證資訊錯誤!請重新更新驗證資訊(過程中需輸入驗證密碼)並取得最新的驗證密碼";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            //驗證機碼相同,但可能缺失驗證密碼.
        //            if(object.Equals(getMKey,null))
        //            {
        //                Msg = "您的驗證密碼發生錯誤,可能已遺失,請立即更新最新的驗證密碼";
        //                return false;
        //            }
        //            else
        //            {
        //                Msg = "";
        //                return true;//ok.
        //            }
        //        }
        //    }
        //    //string keyPathString = "SOFTWARE\\VESD_TOOL";
        //    //string pathstring = null;
        //    //string BuiltPASSKey = 取得內存密碼();
        //    //Msg = "";

        //    //Microsoft.Win32.RegistryKey startnn = Microsoft.Win32.Registry.LocalMachine;
        //    //Microsoft.Win32.RegistryKey programnn = startnn.OpenSubKey(keyPathString);
        //    //if (programnn != null)
        //    //{
        //    //    try
        //    //    {
        //    //        pathstring = (string)programnn.GetValue("PATH_S");//獲取機碼.
        //    //    }
        //    //    catch
        //    //    {

        //    //    }
        //    //    if(object.Equals(BuiltPASSKey,null))
        //    //    {
        //    //        Msg = "您的驗證密碼遺失,請重新認證此電腦,並記錄提供的驗證密碼";
        //    //        return false;
        //    //    }
        //    //    else if(pathstring==NewKey)
        //    //    {
        //    //        //ok.
        //    //        Msg = "無問題";
        //    //        return true;
        //    //    }
        //    //    else // if(pathstring!=NewKey)
        //    //    {
        //    //        //錯誤.
        //    //        Msg = "驗證資訊錯誤,請重新認證此電腦(認證時須輸入驗證密碼)";
        //    //        return false;
        //    //    }

        //    //}
        //    //else
        //    //{
        //    //    //無機碼.
        //    //    Msg = "您目前沒有驗證資訊,請認證此電腦以便使用後續功能";
        //    //    return false;
        //    //}

        //}
        private string 機碼密碼初步加密(string InputS) //,string InputS2)
        {
            //將
            if(object.Equals(InputS, null))
            {
                return null;
            }
            string result = "";
            //string inputs3 = InputS + "_" + InputS2;
            foreach (char c in InputS)
            {
                int asciicode;
                try
                {
                    asciicode = Convert.ToInt32(c);
                    asciicode += 1;
                    result += Convert.ToChar(asciicode);
                }
                catch
                {
                    //無法轉換.
                    result += c;
                }
            }
            return result;
            //return InputS + "_" + InputS2;
        }
        public string 密碼轉16位密碼計算(string InputS)
        {
            //機碼轉換預設是8+6.
            if(object.Equals(InputS,null))
            {
                return null;
            }
            int i = 0;
            int[] A1 = new int[] { };
            Array.Resize(ref A1, 16);
            int[] A2 = new int[] { };
            Array.Resize(ref A2, 16);
            for(int ix=0;ix<A1.GetLength(0);ix++)
            {
                A1[ix] = 30;
            }
            for (int ix = 0; ix < A2.GetLength(0); ix++)
            {
                A2[ix] = 30;
            }
            int f1 = 0;
            int f2 = 0;
            string r1 = "";
            foreach(char c in InputS)
            {
                int asciicode=0;
                try
                {
                    asciicode = Convert.ToInt32(c);
                    asciicode += 1;
                }
                catch
                { }
                if(i<=15)
                {
                    A1[f1] = asciicode;
                    f1 += 1;
                }
                else if(i>16 && i<=32)
                {
                    A2[f2] = asciicode;
                    f2 += 1;
                }
                i = i + 1;
            }


            int[] A3 = A1;
            //轉換成16位.
            for(int ix=0;ix<A1.GetLength(0);ix++)
            {
                int currentnumber;
                currentnumber = A1[ix] + A2[ix];
                string temps = "";
                int testI = -9999;
                int tempcount = currentnumber;
                while(true)
                {
                    temps = tempcount.ToString();
                    tempcount = 0;
                    foreach(char c in temps)
                    {
                        testI = -9999;
                        if(int.TryParse(c.ToString(),out testI))
                        {
                            tempcount += testI;
                        }
                    }
                    if(tempcount<10)
                    {
                        break;
                    }
                    else
                    {
                        //Continue.
                    }
                }
                //
                A3[ix] = tempcount;
            }


            i = 1;
            for(int ix=0;ix<A3.GetLength(0);ix++)
            {

                    //r1 += A3[ix].ToString();

                 r1 += A3[ix].ToString();
                if(i%4==0 && i!=A3.GetLength(0))
                {
                    r1 += "-";
                }

                i = i + 1;
            }
            return r1;

        }
       
        public string 密碼16位碼再加密(string inputs)
        {
            return inputs;
        }
        //public string 密碼解密(string InputS)
        //{
        //    if(object.Equals(InputS,null))
        //    {
        //        return null;
        //    }
        //    string result = "";
        //    //string inputs3 = InputS + "_" + InputS2;
        //    foreach (char c in InputS)
        //    {
        //        int asciicode;
        //        try
        //        {
        //            asciicode = Convert.ToInt32(c);
        //            asciicode -= 1;
        //            result += Convert.ToChar(asciicode);
        //        }
        //        catch
        //        {
        //            //無法轉換.
        //            result += c;
        //        }
        //    }
        //    return result;
        //}
        //private string 密碼解密(string Inputs)
        //{
            
        //}
        private string GetCPUID()
        {
            try
            {
                string cpuInfo = "";//序號.
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
        }
        private string GetMacAddress()
        {
            try
            {
                string mac = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        mac = mo["MacAddress"].ToString();
                        break;
                    }
                }
                moc = null;
                mc = null;
                return mac;
            }
            catch
            {
                return "unknow";
            }
        }
        #endregion 
        public string 取得中文數字碼(int N)
        {
            //整數.
            Dictionary<int, string> 數字與國字碼對照 = new Dictionary<int, string>();
            數字與國字碼對照.Add(0, "零");
            數字與國字碼對照.Add(1, "一");
            數字與國字碼對照.Add(2, "二");
            數字與國字碼對照.Add(3, "三");
            數字與國字碼對照.Add(4, "四");
            數字與國字碼對照.Add(5, "五");
            數字與國字碼對照.Add(6, "六");
            數字與國字碼對照.Add(7, "七");
            數字與國字碼對照.Add(8, "八");
            數字與國字碼對照.Add(9, "九");
            數字與國字碼對照.Add(10, "十");
            
            //目前沒有完整寫完.
            if(N<0 || N>10)
            {
                return "沒有寫完喔";
            }
            else
            {
                return 數字與國字碼對照[N];
            }
        }


    }
}
