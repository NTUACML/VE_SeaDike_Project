using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE_SD
{
    public class Class_BlockSect
    {
        //變數定義區塊.
        private int _點數 = 0;
        private double[] _x=new double[] { };
        private double[] _y=new double[] { };
        private string _name = "";
        private bool _canprocess = true;
        private string _errordesp = "";
        //private double _混凝土方塊與方塊摩擦係數 = 0.5;
        //private double _混凝土方塊與拋石摩擦係數 = 0.6;
        //private double _場注土方塊與拋石摩擦係數 = 0.7;
        //private double _拋石與拋石摩擦係數 = 0.8;

        //private double _混凝土陸上單位體積重量 = 2.3;
        //private double _混凝土水中單位體積重量 = 1.27;
        //private double _拋石陸上單位體積重量 = 1.8;
        //private double _拋石水中單位體積重量 = 1.0;
        //private double _砂土水中單位體積重量 = 1.0;
        //private double _海水單位體積重量 = 1.03;
        private double _單位體積重量 = 2.3;
        private string _使用材質="";//這個Block的使用材質.
        private bool _計算Moment與否 = true;// = "開啟";
        private string[] _參考材質 = new string[] { };//這個Block使用的周圍參考材質.
        private double _平均摩擦係數;
       // private double _滑倒安全係數 = 1.2;
       // private double _傾倒安全係數 = 1.2;


        //建立式.
        public Class_BlockSect(string inputname, int pointcounts, double[] xi, double[] yi)
        {
            _點數 = pointcounts;
            Array.Resize(ref _x, _點數);
            Array.Resize(ref _y, _點數);
            _name = inputname;
            for (int i = 0; i < _點數; i++)
            {
                _x[i] = xi[i];
                _y[i] = yi[i];
            }
        }
        public Class_BlockSect()
        {

        }

        //定義Property.
        public int 座標點數
        {
            get { return _點數; }
            set { _點數 = value; }
        }
        public string 名稱
        {
            get { return _name;}
            set { _name = value; }
        }
        public bool 可否運行
        {
            get { return _canprocess; }
            set { _canprocess = value; }
        }
        public string 錯誤訊息
        {
            get
            {
                return _errordesp;
            }
            set { _errordesp = value; }
        }
        //public double 混凝土方塊與方塊摩擦係數
        //{
        //    get { return _混凝土方塊與方塊摩擦係數; }
        //    set { _混凝土方塊與方塊摩擦係數 = value; }
        //}
        //public double 混凝土方塊與拋石摩擦係數
        //{
        //    get { return _混凝土方塊與拋石摩擦係數; }
        //    set { _混凝土方塊與拋石摩擦係數 = value; }
        //}
        //public double 場注土方塊與拋石摩擦係數
        //{
        //    get { return _場注土方塊與拋石摩擦係數; }
        //    set { _場注土方塊與拋石摩擦係數 = value; }
        //}
        //public double 拋石與拋石摩擦係數
        //{
        //    get { return _拋石與拋石摩擦係數; }
        //    set { _拋石與拋石摩擦係數 = value; }
        //}
        //public double 混凝土陸上單位體積重量
        //{
        //    get { return _混凝土陸上單位體積重量; }
        //    set { _混凝土陸上單位體積重量 = value; }
        //}
        //public double 混凝土水中單位體積重量
        //{
        //    get { return _混凝土水中單位體積重量; }
        //    set { _混凝土水中單位體積重量 = value; }
        //}
        //public double 拋石陸上單位體積重量
        //{
        //    get { return _拋石陸上單位體積重量; }
        //    set { _拋石陸上單位體積重量 = value; }
        //}
        //public double 拋石水中單位體積重量
        //{
        //    get { return _拋石水中單位體積重量; }
        //    set { _拋石水中單位體積重量 = value; }
        //}
        //public double 砂土水中單位體積重量
        //{
        //    get { return _砂土水中單位體積重量; }
        //    set { _砂土水中單位體積重量 = value; }
        //}
        //public double 海水單位體積重量
        //{
        //    get { return _海水單位體積重量; }
        //    set { _海水單位體積重量 = value; }
        //}
        public double 單位體積重量
        {
            get { return _單位體積重量; }
            set { _單位體積重量 = value; }
        }
        public string 使用材質
        {
            get { return _使用材質; }
            set { _使用材質=value  ; }
        }
        public bool 計算Moment與否
        {
            get { return _計算Moment與否; }
            set { _計算Moment與否=value ; }
        }
        public double 平均摩擦係數
        {
            get { return _平均摩擦係數; }
            set { _平均摩擦係數 = value; }
        }


        //public double 滑倒安全係數
        //{
         //   get { return _滑倒安全係數; }
         //   set { _滑倒安全係數 = value; }
        //}
        //public double 傾倒
        //{
         //   get { return _傾倒安全係數; }
         //   set { _傾倒安全係數 = value; }
        //}

        public double[] X
        {
            get { return _x; }
            set { _x = value; }
        }
        public double[] Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public string[] 周圍參考材質 //這個屬性不會在Property Grid內編輯.
        {
            get { return _參考材質; }
            set { _參考材質 = value; }
        }

    }
}
