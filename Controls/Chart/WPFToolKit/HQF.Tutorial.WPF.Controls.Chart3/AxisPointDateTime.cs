using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
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

    }
}