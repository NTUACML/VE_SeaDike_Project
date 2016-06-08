using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VE_SD
{
    class RDExameTextBox_Object_Class
    {
        public RDExameTextBox_Object_Class()
        {
            //Nothing.
        }
        public RDExameTextBox_Object_Class(Form callingForm)
        {

            mainForm = callingForm as Form1;
        }
        private Form1 mainForm = null;

        string _填表人ID = "";
        string _填表人名稱 = "";
        string _深海波波向="";
        string _海側方向 = "";
        string _深海波波高 = "";
        string _深海波週期 = "";
        string _地面線 = "";
        string _消波塊高程 = "";
        string _設計潮位 = "";
        string _海床波度 = "";
        string _折射係數 = "";
        string _淺化係數 = "";
        string _繞射係數 = "";
        string _波力折減係數 = "";
        string _入射波與堤體法線垂線夾角未修正 = "";
        string _入射波與堤體法線垂線夾角修正後 = "";
        string _滑動安全係數 = "";
        string _傾倒安全係數 = "";
        string _海水單位體積重量 = "";
        string _HB = "";
        bool _堤身段港外側消波工重量計算=false;
        bool _堤頭部加強消波工重量計算 = false;
        bool _堤身段航道側消波工重量計算 = false;
        string _堤身段港外側消波形塊安定係數 = "";
        string _堤身段港外側消波塊單位體積重量 = "";
        string _堤身段港外側消波塊斜坡面與水平面夾角 = "";
        string _堤頭部加強消波形塊安定係數 = "";
        string _堤頭部加強消波塊單位體積重量 = "";
        string _堤頭部加強消波塊斜坡面與水平面夾角 = "";
        string _堤身段航道側消波形塊安定係數 = "";
        string _堤身段航道側消波塊單位體積重量 = "";
        string _堤身段航道側消波塊斜坡面與水平面夾角 = "";
        string _堤身段航道側波高傳遞率 = "";

        bool _胸牆部安定檢核計算 = false;
        string _胸牆部安定檢核計算混凝土容許應力 = "";
        string _胸牆部安定檢核計算BKplun = "";
        string _胸牆部安定EL值以上 = "";

        bool _地盤反力及承載力檢核計算 = false;
        string _土壤凝聚力 = "";
        string _土壤內摩擦角 = "";
        string _NC = "";
        string _Nq = "";
        string _Nr = "";
        string _拋石單位重 = "";
        string _入土深度 = "";
        string _拋石厚度 = "";
        string _地盤承載力FS = "";

        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人名稱
        {
            get { return _填表人名稱; }
            set { _填表人名稱 = value; }
        }
        public string 深海波波向
        {
            get { return _深海波波向; }
            set { _深海波波向 = value; }
        }
        public string 海側方向
        {
            get { return _海側方向; }
            set { _海側方向 = value; }
        }
        public string 深海波波高
        {
            get { return _深海波波高; }
            set { _深海波波高 = value; }
        }
        public string 深海波週期
        {
            get { return _深海波週期; }
            set { _深海波週期 = value; }
        }
        public string 地面線
        {
            get { return _地面線; }
            set { _地面線 = value; }
        }
        public string 消波塊高程
        {
            get { return _消波塊高程; }
            set { _消波塊高程 = value; }
        }
        public string 設計潮位
        {
            get { return _設計潮位; }
            set { _設計潮位 = value; }
        }
        public string 海床波度
        {
            get { return _海床波度; }
            set { _海床波度 = value; }
        }
        public string 折射係數
        {
            get { return _折射係數; }
            set { _折射係數 = value; }
        }
        public string 淺化係數
        {
            get { return _淺化係數; }
            set { _淺化係數 = value; }
        }
        public string 繞射係數
        {
            get { return _繞射係數; }
            set { _繞射係數 = value; }
        }
        public string 波力折減係數
        {
            get { return _波力折減係數; }
            set { _波力折減係數 = value; }
        }
        public string 入射波與堤體法線之垂線夾角未修正
        {
            get { return _入射波與堤體法線垂線夾角未修正; }
            set { _入射波與堤體法線垂線夾角未修正 = value; }
        }
        public string 入射波與堤體法線之垂線夾角修正後
        {
            get { return _入射波與堤體法線垂線夾角修正後; }
            set { _入射波與堤體法線垂線夾角修正後 = value; }
        }
        public string 滑動安全係數
        {
            get { return _滑動安全係數; }
            set { _滑動安全係數 = value; }
        }
        public string 傾倒安全係數
        {
            get { return _傾倒安全係數; }
            set { _傾倒安全係數 = value; }
        }
        public string 海水單位體積重量
        {
            get { return _海水單位體積重量; }
            set { _海水單位體積重量 = value; }
        }
        public string HB
        {
            get { return _HB; }
            set { _HB = value; }
        }
        public bool 堤身段港外側消波工重量計算
        {
            get { return _堤身段港外側消波工重量計算; }
            set { _堤身段港外側消波工重量計算 = value; }
        }
        public bool 堤頭部加強消波工重量計算
        {
            get { return _堤頭部加強消波工重量計算; }
            set { _堤頭部加強消波工重量計算 = value; }
        }
        public bool 堤身段航道側消波工重量計算
        {
            get { return _堤身段航道側消波工重量計算; }
            set { _堤身段航道側消波工重量計算 = value; }
        }
        public bool 胸牆部安定檢核計算
        {
            get { return _胸牆部安定檢核計算; }
            set { _胸牆部安定檢核計算 = value; }
        }
        public bool 地盤反力及承載力檢核計算
        {
            get { return _地盤反力及承載力檢核計算; }
            set { _地盤反力及承載力檢核計算 = value; }
        }

        public string 堤身段港外側消波形塊安定係數
        {
            get { return _堤身段港外側消波形塊安定係數; }
            set { _堤身段港外側消波形塊安定係數 = value; }
        }
        public string 堤身段港外側消波塊單位體積重量
        {
            get { return _堤身段港外側消波塊單位體積重量; }
            set { _堤身段港外側消波塊單位體積重量 = value; }
        }
        public string 堤身段港外側消波塊斜坡面與水平面夾角
        {
            get { return _堤身段港外側消波塊斜坡面與水平面夾角; }
            set { _堤身段港外側消波塊斜坡面與水平面夾角 = value; }
        }
        public string 堤頭部加強消波形塊安定係數
        {
            get { return _堤頭部加強消波形塊安定係數; }
            set { _堤頭部加強消波形塊安定係數 = value; }
        }
        public string 堤頭部加強消波塊單位體積重量
        {
            get { return _堤頭部加強消波塊單位體積重量; }
            set { _堤頭部加強消波塊單位體積重量 = value; }
        }
        public string 堤頭部加強消波塊斜坡面與水平面夾角
        {
            get { return _堤頭部加強消波塊斜坡面與水平面夾角; }
            set { _堤頭部加強消波塊斜坡面與水平面夾角 = value; }
        }
        public string 堤身段航道側消波形塊安定係數
        {
            get { return _堤身段航道側消波形塊安定係數; }
            set { _堤身段航道側消波形塊安定係數 = value; }
        }
        public string 堤身段航道側消波塊單位體積重量
        {
            get { return _堤身段航道側消波塊單位體積重量; }
            set { _堤身段航道側消波塊單位體積重量 = value; }
        }
        public string 堤身段航道側消波塊斜坡面與水平面夾角
        {
            get { return _堤身段航道側消波塊斜坡面與水平面夾角; }
            set { _堤身段航道側消波塊斜坡面與水平面夾角 = value; }
        }
        public string 堤身段航道側波高傳遞率
        {
            get { return _堤身段航道側波高傳遞率; }
            set { _堤身段航道側波高傳遞率 = value; }
        }
        public string 胸牆部安定檢核計算混凝土容許應力
        {
            get { return _胸牆部安定檢核計算混凝土容許應力; }
            set { _胸牆部安定檢核計算混凝土容許應力 = value; }
        }
        public string 胸牆部安定檢核計算BKplun
        {
            get { return _胸牆部安定檢核計算BKplun; }
            set { _胸牆部安定檢核計算BKplun = value; }
        }
        public string 胸牆部安定EL以上
        {
            get { return _胸牆部安定EL值以上; }
            set { _胸牆部安定EL值以上 = value; }
        }

        public string 土壤凝聚力
        {
            get { return _土壤凝聚力; }
            set { _土壤凝聚力 = value; }
        }
        public string 土壤內摩擦角
        {
            get { return _土壤內摩擦角; }
            set { _土壤內摩擦角 = value; }
        }
        public string NC
        {
            get { return _NC; }
            set { _NC = value; }
        }

        public string Nq
        {
            get { return _Nq; }
            set { _Nq = value; }
        }
        public string Nr
        {
            get { return _Nr; }
            set { _Nr = value; }
        }
        public string 拋石單位重
        {
            get { return _拋石單位重; }
            set { _拋石單位重 = value; }
        }
        public string 入土深度
        {
            get { return _入土深度; }
            set { _入土深度 = value; }
        }
        public string 拋石厚度
        {
            get { return _拋石厚度; }
            set { _拋石厚度 = value; }
        }
        public string 地盤承載力FS
        {
            get { return _地盤承載力FS; }
            set { _地盤承載力FS = value; }
        }

        //string _土壤凝聚力 = "";
        //string _土壤內摩擦角 = "";
        //string _NC = "";
        //string _Nq = "";
        //string _Nr = "";
        //string _拋石單位重 = "";
        //string _入土深度 = "";
        //string _拋石厚度 = "";
        //string _地盤承載力FS = "";
    }
}
