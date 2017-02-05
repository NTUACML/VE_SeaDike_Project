using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE_SD
{
    public class RealRandom
    {
        //產生0~1之間的亂數.
        public double NextDouble()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return rnd.NextDouble();
        }

        //產出指定範圍內的亂數.
        public double NextDouble(double minValue,double maxValue)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            return minValue + rnd.NextDouble() * (maxValue - minValue);
        }
        /// <summary>
       /// 產生指定範圍內的整數亂數，由Random.Next()實做
       /// </summary>
       /// <param name="minValue">開始值</param>
       /// <param name="maxValue">結束值</param>
       /// <returns>整數亂數</returns>
       public int Next(int minValue, int maxValue)
      {
         Random rnd = new Random(Guid.NewGuid().GetHashCode());
         return rnd.Next(minValue, maxValue);
       }
    
       /// <summary>
       /// 產生正負一的亂數
       /// </summary>
       /// <returns>隨機的0或1</returns>
       public int NextPNOne()
       {
         Random rnd = new Random(Guid.NewGuid().GetHashCode());
        return rnd.NextDouble() < 0.5 ? -1 : 1;
       }
    }
}
