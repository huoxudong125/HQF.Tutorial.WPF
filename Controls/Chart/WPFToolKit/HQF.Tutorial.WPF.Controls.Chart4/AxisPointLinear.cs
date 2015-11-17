using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart4
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

    public class AxisPointDateTime : AxisPoint
    {
        public readonly DateTimeAxis Axis;
        public readonly DateTime Min;
        public readonly DateTime Max;  // larger than Min;
        public readonly TimeSpan Range;
        public readonly DateTime MouseAxisValueAbsolute;

        public AxisPointDateTime(Chart xChart, DateTimeAxis xAxis, Point xPoint, DateTime xMin, DateTime xMax)
          : base(xChart, xAxis, xPoint)
        {
            Min = xMin;
            Max = xMax;
            Range = xMax - xMin;
            Axis = xAxis;
            MouseAxisValueAbsolute = xMin.AddMinutes(MouseAxisValueRelative * Range.TotalMinutes);
        } // constructor

        public override string ToString()
        {
            string s = "Mouse: ";
            s += MouseAxisValueRelative.ToString("0.000%");
            s += "  =>  ";
            s += MouseAxisValueAbsolute.ToString("dd MMM yyyy");
            s += " for ";
            s += Axis.Orientation;
            s += "-Axis ";
            s += Axis.Title;
            return s;
        } //

    } // class

    public class AxisPointFactory
    {
        public static AxisPoint getAxisPoint(Chart xChart, RangeAxis xAxis, Point xPoint)
        {
            if (xAxis == null) return null;

            if (xAxis is LinearAxis)
            {
                // some redundant basic checks
                LinearAxis lAxis = xAxis as LinearAxis;
                double? lMin;
                double? lMax;
                lMin = lAxis.ActualMinimum;
                lMax = lAxis.ActualMaximum;

                if ((!lMin.HasValue) || (!lMax.HasValue)) return null;
                if (lMin.Value >= lMax.Value) return null;

                return new AxisPointLinear(xChart, lAxis, xPoint, lMin.Value, lMax.Value);
            }
            if (xAxis is DateTimeAxis)
            {
                // some redundant basic checks
                DateTimeAxis lAxis = xAxis as DateTimeAxis;
                DateTime? lMin;
                DateTime? lMax;
                lMin = lAxis.ActualMinimum;
                lMax = lAxis.ActualMaximum;

                if ((!lMin.HasValue) || (!lMax.HasValue)) return null;
                if (lMin.Value >= lMax.Value) return null;

                return new AxisPointDateTime(xChart, lAxis, xPoint, lMin.Value, lMax.Value);
            }

            throw new Exception("Axis type not supported yet.");
        } //
    } // class

    public abstract class AxisPoint
    {
        public readonly Chart Chart;
        public readonly Point MouseAbsoluteLocation;
        public readonly double MouseAxisValueRelative;  // a number between 0% and 100%
        public readonly double Length;  // object pixel display units, larger than zero

        public AxisPoint(Chart xChart, RangeAxis xAxis, Point xPoint)
        {
            if (xAxis.Orientation == AxisOrientation.X) Length = xAxis.ActualWidth;
            else Length = xAxis.ActualHeight;

            if (Length <= 0) throw new Exception("Chart object length is zero or less.");

            MouseAbsoluteLocation = xChart.TranslatePoint(xPoint, xAxis);
            if (xAxis.Orientation == AxisOrientation.X) MouseAxisValueRelative = MouseAbsoluteLocation.X / Length;
            else MouseAxisValueRelative = 1.0 - (MouseAbsoluteLocation.Y / Length);

            if (MouseAxisValueRelative > 1.0) MouseAxisValueRelative = 1.0;
            else if (MouseAxisValueRelative < 0.0) MouseAxisValueRelative = 0.0;
        } // constructor

    } // class
}
