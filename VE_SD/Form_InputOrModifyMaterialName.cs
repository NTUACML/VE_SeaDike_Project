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
    public partial class Form_InputOrModifyMaterialName : Form
    {
        public Form_InputOrModifyMaterialName()
        {
            InitializeComponent();
        }
        private Form_MTExamProgress MTExamMainForm = null;
        string[] OldList = new string[] { };
        Dictionary<string, int> DI = new Dictionary<string, int>();
        string nowname;
        string oldname;
        int RowIndex;
        public Form_InputOrModifyMaterialName(Form callingForm,string[] List,int RowIndexi=-1,string oldnamei="")
        {
            OldList = List;
            for(int i=0;i<=OldList.GetUpperBound(0);i++)
            {
                DI.Add(OldList[i], 0);
            }
            oldname = oldnamei;
            nowname = oldnamei;
            RowIndex = RowIndexi;
            MTExamMainForm = callingForm as Form_MTExamProgress;
            InitializeComponent();
        }

        private void Form_InputOrModifyMaterialName_Load(object sender, EventArgs e)
        {
            textBox_Input.Text = oldname;
            label1.Text = "請輸入新的材質名稱";
            if(oldname=="")
            {
                btn_Add.Text = "新增";
                label_show.Text = "";
            }
            else
            {
                label_show.Text = "原始名稱:" + oldname;
                btn_Add.Text = "修改";
            }
            errorProvider1.Clear();
        }

        #region 修改名稱

        private void textBox_Input_TextChanged(object sender, EventArgs e)
        {
            if (textBox_Input.Text.Length > 0)
            {
                if (檢查名稱是否重複(textBox_Input.Text))
                {
                    //重複.
                    errorProvider1.SetError(textBox_Input, "此名稱已重複");
                    textBox_Input.Focus();
                    btn_Add.Enabled = false;
                }
                else
                {
                    //nowname = textBox_NameInput.Text
                    nowname = textBox_Input.Text;
                    errorProvider1.Clear();
                    btn_Add.Enabled = true;
                }
            }
            else
            {
                errorProvider1.Clear();
                btn_Add.Enabled = false;
            }
        }
        private bool 檢查名稱是否重複(string inS)
        {
            bool alreadyOld = DI.ContainsKey(oldname);
            if (alreadyOld && oldname == inS)
            {
                return false;//Not Repeated.
            }
            else
            {
                if (DI.ContainsKey(inS))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //return DI.ContainsKey(inS);
            }
        }

        #endregion

        private void btn_Add_Click(object sender, EventArgs e)
        {
            nowname = textBox_Input.Text;
            if(btn_Add.Text=="新增")
            {
                if (MessageBox.Show("您確定要新增嗎?", "新增材質名稱", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    MTExamMainForm.新增新的材質(nowname);
                    this.Close();
                }
            }
            else if(btn_Add.Text=="修改")
            {
                if (MessageBox.Show("您確定要修改嗎?" + Environment.NewLine + "您修改此材質名稱後,相對應的摩擦係數設定都會自動變更", "修改材質名稱", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
                {
                    return;
                }
                else
                {
                    MTExamMainForm.處理材質名稱的修改(nowname,RowIndex);
                    this.Close();
                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
