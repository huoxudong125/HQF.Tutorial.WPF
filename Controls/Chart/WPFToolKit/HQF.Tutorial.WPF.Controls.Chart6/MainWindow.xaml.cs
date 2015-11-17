using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HQF.Tutorial.WPF.Controls.Chart6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double _YMin, _YMax;
        private int _XMin = 0, _XMax = 2000;

        public MainWindow()
        {
            InitializeComponent();
        } // constructor

        private void Window_Loaded(object xSender, RoutedEventArgs e)
        {
            // add arbitrary LineSeries points
            List<Point> lPoints = new List<Point>();
            Random lRandom = new Random();
            double y = 1.0;
            double lYMin = double.MaxValue;
            double lYMax = double.MinValue;

            for (int i = _XMin; i < _XMax; i++)
            {
                double lChange = lRandom.NextDouble() - 0.5;

                y += lChange / 100.0;
                if (y > lYMax) lYMax = y;
                if (y < lYMin) lYMin = y;
                Point lPoint = new Point((double)i, y);
                lPoints.Add(lPoint);
            }

            _YMax = lYMax;
            _YMin = lYMin;
            VBar.Maximum = lYMax;
            VBar.Minimum = lYMin;
            HBar.Minimum = _XMin;
            HBar.Maximum = _XMax;

            // we clone the list to avoid trouble (deep copy)
            List<Point> lPoints2 = (from p in lPoints select new Point(p.X, p.Y)).ToList();

            MyLineSeries1.ItemsSource = lPoints;
            MyLineSeries2.ItemsSource = lPoints2;
        } //

        private void HBar_ValueChanged(object xSender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (XAxis2 == null) return;

            double? lMax = XAxis2.Maximum; if (lMax == null) lMax = XAxis2.ActualMaximum;
            //double? lMin = XAxis2.Minimum; if (lMin == null) lMin = XAxis2.ActualMinimum;
            double lZoom = (_XMax - _XMin) * HBarZoom.Value / 100.0 / 2.0;
            double lValue = HBar.Value; // We do not use e.NewValue, because this event is called from many sources.

            if (lValue > lMax)
            {
                XAxis2.Maximum = Math.Max(lValue + lZoom, _XMax);        // widen the range first!
                XAxis2.Minimum = Math.Min(XAxis2.Maximum.Value - lZoom, _XMin);        // now we can tighten the range
                return;
            }
            XAxis2.Minimum = Math.Max(lValue - lZoom, _XMin);          // widen
            XAxis2.Maximum = Math.Min(XAxis2.Minimum.Value + lZoom, _XMax);          // tighten

            e.Handled = true;
        } //

        private void VBar_ValueChanged(object xSender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (XAxis2 == null) return;

            double? lMax = YAxis2.Maximum; if (lMax == null) lMax = YAxis2.ActualMaximum;
            //double? lMin = YAxis2.Minimum; if (lMin == null) lMin = YAxis2.ActualMinimum;
            double lZoom = (_YMax - _YMin) * VBarZoom.Value / 100.0 / 2.0;
            double lValue = VBar.Value; // We do not use e.NewValue, because this event is called from many sources.

            if (lValue > lMax)
            {
                YAxis2.Maximum = Math.Min(lValue + lZoom, _YMax);
                YAxis2.Minimum = Math.Max(YAxis2.Maximum.Value - lZoom, _YMin);
                return;
            }
            YAxis2.Minimum = Math.Max(lValue - lZoom, _YMin);
            YAxis2.Maximum = Math.Min(YAxis2.Minimum.Value + lZoom, _YMax);

            e.Handled = true;
        } // 

    } // class
}

