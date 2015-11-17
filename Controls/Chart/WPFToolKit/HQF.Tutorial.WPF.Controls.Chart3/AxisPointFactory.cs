using System;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
    public class AxisPointFactory
    {
        public static AxisPoint getAxisPoint(Chart xChart, RangeAxis xAxis, Point xPoint)
        {
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
    }
}