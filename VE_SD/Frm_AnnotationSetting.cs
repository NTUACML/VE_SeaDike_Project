﻿using System;
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
    public partial class Frm_AnnotationSetting : Form
    {
        public Frm_AnnotationSetting()
        {
            InitializeComponent();
        }
        private Form_MTExamProgress mainForm1 = null;
        private Form_RDExamProgress mainForm2 = null;
        private static string 使用類型 = null;
        private static double _原始AspectRatio;
        private static double _原始AnnotationSize;
        public Frm_AnnotationSetting(Form callingForm,string type,double 原始AspectRatio)
        {
            if(type=="RD")
            {
                mainForm2=callingForm as Form_RDExamProgress;
                使用類型 = "RD";
                _原始AspectRatio = 原始AspectRatio;
            }
            else if(type=="MT")
            {
                mainForm1 = callingForm as Form_MTExamProgress;
                使用類型 = "MT";
                _原始AspectRatio = 原始AspectRatio;
            }
            InitializeComponent();
        }
        private static bool _Initial = true;
        private void Frm_AnnotationSetting_Load(object sender, EventArgs e)
        {
            _原始AnnotationSize = 7;//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (使用類型 == "RD")
            {
                //numericUpDown1.Value = mainForm2.
            }
            else if(使用類型 == "MT")
            {
                _Initial = true;
                numericUpDown1.Value = mainForm1.AnnotationSize;
               // MessageBox.Show(mainForm1.ChartYXRatio.ToString());
                numericUpDown2.Value = (decimal)mainForm1.ChartYXRatio;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            //數字變動時,調整資訊.
            if (使用類型 == "RD")
            {
                
            }
            else if (使用類型 == "MT")
            {
                if (_Initial) { _Initial = false; return; }
                mainForm1.AnnotationSize = int.Parse(numericUpDown1.Value.ToString());
                mainForm1.繪上EL();
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            //數字變動時,調整資訊.
            if (使用類型 == "RD")
            {

            }
            else if (使用類型 == "MT")
            {
                if (_Initial) { _Initial = false; return; }
                mainForm1.ChartYXRatio = double.Parse(numericUpDown2.Value.ToString());
                mainForm1.以YX比例設定座標軸(null);
                //MessageBox.Show(mainForm1.ChartYXRatio.ToString());
                //mainForm1.調整Chart比例 = true;
                //mainForm1.調整Chart(null);
                //mainForm1.繪上EL();//重繪.
            }
        }

        private void Frm_AnnotationSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(使用類型 == "RD")
            {

            }
            else if(使用類型 == "MT")
            {
                mainForm1.關閉BlockLabels設定();
            }
        }

        private void btn_AnnotationReset_Click(object sender, EventArgs e)
        {
            if (使用類型 == "RD")
            {

            }
            else if (使用類型 == "MT")
            {
                numericUpDown1.Value =(decimal)_原始AnnotationSize;
            }
        }

        private void btn_YXRatioReset_Click(object sender, EventArgs e)
        {
            if (使用類型 == "RD")
            {

            }
            else if (使用類型 == "MT")
            {
                numericUpDown2.Value = (decimal)_原始AspectRatio;
            }
        }
    }
}
