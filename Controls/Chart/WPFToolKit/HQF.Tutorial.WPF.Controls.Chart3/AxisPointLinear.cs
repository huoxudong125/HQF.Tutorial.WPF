using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
    public class AxisPointLinear : AxisPoint
    {
        public readonly LinearAxis Axis;
        public readonly double Min;
        public readonly double Max;  // larger than Min;
        public readonly double Range;
        public readonly double MouseAxisValueAbsolute;

        public AxisPointLinear(Chart xChart, LinearAxis xAxis, Point xPoint, double xMin, double xMax)
            : base(xChart, xAxis, xPoint)
        {
            Min = xMin;
            Max = xMax;
            Range = xMax - xMin;
            Axis = xAxis;
            MouseAxisValueAbsolute = xMin + (MouseAxisValueRelative * Range);
        } // constructor

        public override string ToString()
        {
            string s = "Mouse: ";
            s += MouseAxisValueRelative.ToString("0.000%");
            s += "  =>  ";
            s += MouseAxisValueAbsolute.ToString("#,##0.000");
            s += " EUR for ";
            s += Axis.Orientation;
            s += "-Axis ";
            s += Axis.Title;
            return s;
        } //

    } // class

 
}
