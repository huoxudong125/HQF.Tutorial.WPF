using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQF.Tutorial.WPF.Controls.Chart5
{
    public class CurvePoint
    {
        public double CurveId { get; set; }  // Y value
        public double Time { get; set; }     // X value

        public CurvePoint(double xCurveId, double xTime)
        {
            CurveId = xCurveId;
            Time = xTime;
        } // constructor
    } // class
}
