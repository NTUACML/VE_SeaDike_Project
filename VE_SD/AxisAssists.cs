using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VE_SD
{
    class AxisAssists
    {
        public double Tick { get; private set; }

        public AxisAssists(double aTick)
        {
            Tick = aTick;
        }
        public AxisAssists(double range, int mostticks)
        {
            var minimum = range / mostticks;
            var magnitude = Math.Pow(10.0, (Math.Floor(Math.Log(minimum) / Math.Log(10))));
            var residual = minimum / magnitude;
            if (residual > 5)
            {
                Tick = 10 * magnitude;
            }
            else if (residual > 2)
            {
                Tick = 5 * magnitude;
            }
            else if (residual > 1)
            {
                Tick = 2 * magnitude;
            }
            else
            {
                Tick = magnitude;
            }
        }

        public double GetClosestTickBelow(double v)
        {
            return Tick * Math.Floor(v / Tick);
        }
        public double GetClosestTickAbove(double v)
        {
            return Tick * Math.Ceiling(v / Tick);
        }
    }
}
