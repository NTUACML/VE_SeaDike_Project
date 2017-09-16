using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE_SD
{
    class MTExameTextBox_Object_Class
    {
        public MTExameTextBox_Object_Class()
        {

        }

        string _填表人ID = "";
        string _填表人名稱 = "";
        string _設計潮位高 = "";
        string _設計潮位低 = "";
        string _殘留水位 = "";
        string _平時上載荷重 = "";
        string _地震時上載荷重 = "";
        string _船舶牽引力 = "";
        string _陸上設計震度 = "";
        string _水中設計震度 = "";
        string _水單位重 = "";
        string _繫船柱突出高度 = "";
        string _背填料內摩擦角 = "";
        string _背填料壁面摩擦角 = "";
        string _背填料水平傾斜角 = "";
        string _入土深度 = "";
        string _拋石厚度 = "";
        string _海側方向 = "";
        string _地盤基礎內摩擦角 = "";
        string _土壤凝聚力 = "";
        string _Nq = "";
        string _Nc = "";
        string _Nr = "";
        string _平時滑動安全係數 = "";
        string _平時傾倒安全係數 = "";
        string _平時地盤承載力安全係數 = "";
        string _地震時滑動安全係數 = "";
        string _地震時傾倒安全係數 = "";
        string _地震時地盤承載力安全係數 = "";
        string _陸上土壤重="";
        string _水中土壤重="";
        string _平時無設計震度土壓係數Ka = "";
        string _地震時設計震度K017土壓係數Ka="";
        string _地震時設計震度K033土壓係數Ka="";


        /*
        string ="";
        string ="";
        string ="";
        string ="";
        string ="";
        string ="";
        string ="";

            string ="";
            */
       
        //屬性區.
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
        public string 設計潮位高
        {
            get { return _設計潮位高; }
            set { _設計潮位高 = value; }
        }
        public string 設計潮位低
        {
            get { return _設計潮位低; }
            set { _設計潮位低 = value; }
        }
        public string 殘留水位
        {
            get { return _殘留水位; }
            set { _殘留水位 = value; }
        }
        public string 平時上載荷重
        {
            get { return _平時上載荷重; }
            set { _平時上載荷重 = value; }
        }
        public string 地震時上載荷重
        {
            get { return _地震時上載荷重; }
            set { _地震時上載荷重 = value; }
        }
        public string 船舶牽引力
        {
            get { return _船舶牽引力; }
            set { _船舶牽引力 = value; }
        }
        public string 陸上設計震度
        {
            get { return _陸上設計震度; }
            set { _陸上設計震度 = value; }
        }
        public string 水中設計震度
        {
            get { return _水中設計震度; }
            set { _水中設計震度 = value; }
        }
        public string 水單位重
        {
            get { return _水單位重; }
            set { _水單位重 = value; }
        }
        public string 繫船柱突出高度
        {
            get { return _繫船柱突出高度; }
            set { _繫船柱突出高度 = value; }
        }
        public string 背填料內摩擦角
        {
            get { return _背填料內摩擦角; }
            set { _背填料內摩擦角 = value; }
        }
        public string 背填料壁面摩擦角
        {
            get { return _背填料壁面摩擦角; }
            set { _背填料壁面摩擦角 = value; }
        }
        public string 背填料水平傾斜角
        {
            get { return _背填料水平傾斜角; }
            set { _背填料水平傾斜角 = value; }
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
        public string 海側方向
        {
            get { return _海側方向; }
            set { _海側方向 = value; }
        }
        public string 地盤基礎內摩擦角
        {
            get { return _地盤基礎內摩擦角; }
            set { _地盤基礎內摩擦角 = value; }
        }
        public string 土壤凝聚力
        {
            get { return _土壤凝聚力; }
            set { _土壤凝聚力 = value; }
        }
        public string Nq
        {
            get { return _Nq; }
            set { _Nq = value; }
        }
        public string Nc
        {
            get { return _Nc; }
            set { _Nc = value; }
        }
        public string Nr
        {
            get { return _Nr; }
            set { _Nr = value; }
        }
        public string 平時滑動安全係數
        {
            get { return _平時滑動安全係數; }
            set { _平時滑動安全係數 = value; }
        }
        public string 平時傾倒安全係數
        {
            get { return _平時傾倒安全係數; }
            set { _平時傾倒安全係數 = value; }
        }
        public string 平時地盤承載力安全係數
        {
            get { return _平時地盤承載力安全係數; }
            set { _平時地盤承載力安全係數 = value; }
        }
        public string 地震時滑動安全係數
        {
            get { return _地震時滑動安全係數; }
            set { _地震時滑動安全係數 = value; }
        }
        public string 地震時傾倒安全係數
        {
            get { return _地震時傾倒安全係數; }
            set { _地震時傾倒安全係數 = value; }
        }
        public string 地震時地盤承載力安全係數
        {
            get { return _地震時地盤承載力安全係數; }
            set { _地震時地盤承載力安全係數 = value; }
        }
        public string 陸上土壤重
        {
            get { return _陸上土壤重; }
            set { _陸上土壤重 = value; }
        }
        public string 水中土壤重
        {
            get { return _水中土壤重; }
            set { _水中土壤重 = value; }
        }
        public string 平時無設計震度土壓係數Ka
        {
            get { return _平時無設計震度土壓係數Ka; }
            set { _平時無設計震度土壓係數Ka = value; }
        }
        public string 地震時設計震度K017土壓係數Ka
        {
            get { return _地震時設計震度K017土壓係數Ka; }
            set { _地震時設計震度K017土壓係數Ka = value; }
        }
        public string 地震時設計震度K033土壓係數Ka
        {
            get { return _地震時設計震度K033土壓係數Ka; }
            set { _地震時設計震度K033土壓係數Ka = value; }
        }

        /*
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        public string 填表人ID
        {
            get { return _填表人ID; }
            set { _填表人ID = value; }
        }
        */





    }
}
