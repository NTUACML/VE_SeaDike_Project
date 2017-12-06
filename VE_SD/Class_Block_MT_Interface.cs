using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VE_SD
{
    public class Class_Block_MT_Interface
    {
        private double _單位體積重量 = 2.3;
        private double _地震時單位體積重量 = 2.3;
        private string _使用材質;
        private bool _是否為混凝土塊 = true;//="開啟";
        private string[] _可用材質=new string[] { };

        //private double _滑倒安全係數 = 1.2;
        //private double _傾倒安全係數 = 1.2;
        public Class_Block_MT_Interface()
        {
            //無,使用預設值.
        }
        //public Class_Block_Interface(double 混凝土方塊與方塊摩擦係數值, double 混凝土方塊與拋石摩擦係數值, double 場注土方塊與拋石摩擦係數值, double 拋石與拋石摩擦係數值, double 混凝土陸上單位體積重量值, double 混凝土水中單位體積重量值, double 拋石陸上單位體積重量值, double 拋石水中單位體積重量值, double 砂土水中單位體積重量值, double 海水單位體積重量值,double 單位體積重量)
        //{
        //    //_混凝土方塊與方塊摩擦係數 = 混凝土方塊與方塊摩擦係數值;
        //    //_混凝土方塊與拋石摩擦係數 = 混凝土方塊與拋石摩擦係數值;
        //    //_場注土方塊與拋石摩擦係數 = 場注土方塊與拋石摩擦係數值;
        //    //_拋石與拋石摩擦係數 = 拋石與拋石摩擦係數值;

        //    //_混凝土陸上單位體積重量 = 混凝土陸上單位體積重量值;
        //    //_混凝土水中單位體積重量 = 混凝土水中單位體積重量值;
        //    //_拋石陸上單位體積重量 = 拋石陸上單位體積重量值;
        //    //_拋石水中單位體積重量 = 拋石水中單位體積重量值;
        //    //_砂土水中單位體積重量 = 砂土水中單位體積重量值;
        //    //_海水單位體積重量 = 海水單位體積重量值;

        //    _單位體積重量 = 單位體積重量;

        //}
        public Class_Block_MT_Interface(Class_BlockSect_MT M)
        {
            //_混凝土方塊與方塊摩擦係數 = M.混凝土方塊與方塊摩擦係數;
            //_混凝土方塊與拋石摩擦係數 = M.混凝土方塊與拋石摩擦係數;
            //_場注土方塊與拋石摩擦係數 = M.場注土方塊與拋石摩擦係數;
            //_拋石與拋石摩擦係數 = M.拋石與拋石摩擦係數;

            //_混凝土陸上單位體積重量 = M.混凝土陸上單位體積重量;
            //_混凝土水中單位體積重量 = M.混凝土水中單位體積重量;
            //_拋石陸上單位體積重量 = M.拋石陸上單位體積重量;
            //_拋石水中單位體積重量 = M.拋石水中單位體積重量;
            //_砂土水中單位體積重量 = M.砂土水中單位體積重量;
            //_海水單位體積重量 = M.海水單位體積重量;
            
            _地震時單位體積重量 = M.地震時單位體積重量;
            _單位體積重量 = M.單位體積重量;
            _使用材質 = M.使用材質;
            _是否為混凝土塊 = M.是否為混凝土塊;
        }
        public string[] 可用材質
        {
            set { _可用材質 = value; }
        }

        //[CategoryAttribute("摩擦係數設定")] //,DefaultValueAttribute(true)]
        //public double 混凝土方塊與方塊
        //{
        //    get { return _混凝土方塊與方塊摩擦係數; }
        //    set { _混凝土方塊與方塊摩擦係數 = value; }
        //}
        //[CategoryAttribute("摩擦係數設定")]
        //public double 混凝土方塊與拋石
        //{
        //    get { return _混凝土方塊與拋石摩擦係數; }
        //    set { _混凝土方塊與拋石摩擦係數 = value; }
        //}
        //[CategoryAttribute("摩擦係數設定")]
        //public double 場注土方塊與拋石
        //{
        //    get { return _場注土方塊與拋石摩擦係數; }
        //    set { _場注土方塊與拋石摩擦係數 = value; }
        //}
        //[CategoryAttribute("摩擦係數設定")]
        //public double 拋石與拋石
        //{
        //    get { return _拋石與拋石摩擦係數; }
        //    set { _拋石與拋石摩擦係數 = value; }
        //}

        //[CategoryAttribute("單位體積重量")] //, DefaultValueAttribute(true)]
        //public double 混凝土陸上
        //{
        //    get { return _混凝土陸上單位體積重量; }
        //    set { _混凝土陸上單位體積重量 = value; }
        //}
        //[CategoryAttribute("單位體積重量")]
        //public double 混凝土水中
        //{
        //    get { return _混凝土水中單位體積重量; }
        //    set { _混凝土水中單位體積重量 = value; }
        //}
        //[CategoryAttribute("單位體積重量")]
        //public double 拋石陸上
        //{
        //    get { return _拋石陸上單位體積重量; }
        //    set { _拋石陸上單位體積重量 = value; }
        //}
        //[CategoryAttribute("單位體積重量")]
        //public double 拋石水中
        //{
        //    get { return _拋石水中單位體積重量; }
        //    set { _拋石水中單位體積重量 = value; }
        //}
        //[CategoryAttribute("單位體積重量")]
        //public double 砂土水中
        //{
        //    get { return _砂土水中單位體積重量; }
        //    set { _砂土水中單位體積重量 = value; }
        //}
        //[CategoryAttribute("單位體積重量")]
        //public double 海水
        //{
        //    get { return _海水單位體積重量; }
        //    set { _海水單位體積重量 = value; }
        //}
        // [CategoryAttribute("單位體積重量")]
        public double 單位體積重量
        {
            get { return _單位體積重量; }
            set { _單位體積重量 = value; }
        }
        public double 地震時單位體積重量
        {
            get { return _地震時單位體積重量; }
            set { _地震時單位體積重量 = value; }
        }
        public bool 是否為混凝土塊
        {
            get { return _是否為混凝土塊; }
            set { _是否為混凝土塊 = value; }
        }
        //[CategoryAttribute("材質")]
        [TypeConverter(typeof(List2PropertyConverter))]
        public string 使用材質
        {
            get { return _使用材質; }
            set { _使用材質 = value; }
        }

        List<string> List;
        [Browsable(false)]
        public List<string> MyList
        {
            get
            {
                if (List == null)
                {
                    List = new List<string>();
                    for (int i = 0; i < _可用材質.GetLength(0); i++)
                    {
                        List.Add(_可用材質[i]);
                    }
                }
                return List;
            }
        }

        //[CategoryAttribute("安全係數")] //, DefaultValueAttribute(true)]
        //public double 滑倒
        // {
        //    get { return _滑倒安全係數; }
        //    set { _滑倒安全係數 = value; }
        // }
        // [CategoryAttribute("安全係數")]
        // public double 傾倒
        // {
        //     get { return _傾倒安全係數; }
        //     set { _傾倒安全係數 = value; }
        // }

    }
    internal class List2PropertyConverter : StringConverter
    {
        //https://bytes.com/topic/c-sharp/answers/596701-propertygrid-dynamic-dropdown-list
        public List2PropertyConverter()
        {

        }
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //True - means show a Combobox
            //and False for show a Modal 
            return true;// base.GetStandardValuesSupported(context);
        }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //False - a option to edit values 
            //and True - set values to state readonly
            return true;// base.GetStandardValuesExclusive(context);
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> List = (context.Instance as Class_Block_MT_Interface).MyList;
            StandardValuesCollection cols = new StandardValuesCollection(List);
            return cols;// new StandardValuesCollection();
        }
    }
}
