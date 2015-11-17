using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
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

    }
}