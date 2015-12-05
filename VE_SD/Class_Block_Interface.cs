using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace VE_SD
{
    public class Class_Block_Interface
    {
        private double _混凝土方塊與方塊摩擦係數 = 0.5;
        private double _混凝土方塊與拋石摩擦係數 = 0.6;
        private double _場注土方塊與拋石摩擦係數 = 0.7;
        private double _拋石與拋石摩擦係數 = 0.8;

        private double _混凝土陸上單位體積重量 = 2.3;
        private double _混凝土水中單位體積重量 = 1.27;
        private double _拋石陸上單位體積重量 = 1.8;
        private double _拋石水中單位體積重量 = 1.0;
        private double _砂土水中單位體積重量 = 1.0;
        private double _海水單位體積重量 = 1.03;

        private double _滑倒安全係數 = 1.2;
        private double _傾倒安全係數 = 1.2;

        [CategoryAttribute("摩擦係數設定")] //,DefaultValueAttribute(true)]
        public double 混凝土方塊與方塊
        {
            get { return _混凝土方塊與方塊摩擦係數; }
            set { _混凝土方塊與方塊摩擦係數 = value; }
        }
        [CategoryAttribute("摩擦係數設定")]
        public double 混凝土方塊與拋石
        {
            get { return _混凝土方塊與拋石摩擦係數; }
            set { _混凝土方塊與拋石摩擦係數 = value; }
        }
        [CategoryAttribute("摩擦係數設定")]
        public double 場注土方塊與拋石
        {
            get { return _場注土方塊與拋石摩擦係數; }
            set { _場注土方塊與拋石摩擦係數 = value; }
        }
        [CategoryAttribute("摩擦係數設定")]
        public double 拋石與拋石
        {
            get { return _拋石與拋石摩擦係數; }
            set { _拋石與拋石摩擦係數 = value; }
        }

        [CategoryAttribute("單位體積重量")] //, DefaultValueAttribute(true)]
        public double 混凝土陸上
        {
            get { return _混凝土陸上單位體積重量; }
            set { _混凝土陸上單位體積重量 = value; }
        }
        [CategoryAttribute("單位體積重量")]
        public double 混凝土水中
        {
            get { return _混凝土水中單位體積重量; }
            set { _混凝土水中單位體積重量 = value; }
        }
        [CategoryAttribute("單位體積重量")]
        public double 拋石陸上
        {
            get { return _拋石陸上單位體積重量; }
            set { _拋石陸上單位體積重量 = value; }
        }
        [CategoryAttribute("單位體積重量")]
        public double 拋石水中
        {
            get { return _拋石水中單位體積重量; }
            set { _拋石水中單位體積重量 = value; }
        }
        [CategoryAttribute("單位體積重量")]
        public double 砂土水中
        {
            get { return _砂土水中單位體積重量; }
            set { _砂土水中單位體積重量 = value; }
        }
        [CategoryAttribute("單位體積重量")]
        public double 海水
        {
            get { return _海水單位體積重量; }
            set { _海水單位體積重量 = value; }
        }

        [CategoryAttribute("安全係數")] //, DefaultValueAttribute(true)]
        public double 滑倒
        {
            get { return _滑倒安全係數; }
            set { _滑倒安全係數 = value; }
        }
        [CategoryAttribute("安全係數")]
        public double 傾倒
        {
            get { return _傾倒安全係數; }
            set { _傾倒安全係數 = value; }
        }

    }
}
