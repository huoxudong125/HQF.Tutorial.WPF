using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace HQF.Tutorial.WPF.Controls.Chart5
{
    public class ViewModel : DependencyObject
    {
        private readonly MainWindow _MainWindow;
        public readonly ObservableCollection<InfoText> Messages = new ObservableCollection<InfoText>();

        public ViewModel(MainWindow xMainWindow)
        {
            _MainWindow = xMainWindow;
            _MainWindow.InfoText.ItemsSource = Messages;
        } // constructor

        public void AddNewCurves(CurvePoint[][] xCurvePoints, double xYShift)
        {
            if (!Dispatcher.CheckAccess())
            {
                Action lAction = () => { AddNewCurves(xCurvePoints, xYShift); };
                App.Current.Dispatcher.BeginInvoke(lAction);
                return;
            }

            // find the Y axis
            LinearAxis lYAxis = null;
            foreach (IAxis lAxis in _MainWindow.myChart.Axes)
            {
                if (lAxis.Orientation != AxisOrientation.Y) continue;
                lYAxis = lAxis as LinearAxis;
            }

            int lCount = xCurvePoints.GetUpperBound(0);
            LineSeries[] lLineSeries = new LineSeries[lCount];
            for (int x = 0, n = lCount; x < n; x++)
            {
                LineSeries lLine = new LineSeries();
                lLineSeries[x] = lLine;
                lLine.DependentRangeAxis = lYAxis;

                lLine.Title = "manually added curve";
                lLine.SetBinding(LineSeries.ItemsSourceProperty, new Binding());

                lLine.IndependentValueBinding = new Binding("Time");
                lLine.DependentValueBinding = new Binding("CurveId");

                Style lLineStyle = new Style(typeof(Polyline));
                //lLineStyle.Setters.Add(new Setter(Polyline.StrokeStartLineCapProperty, PenLineCap.Flat));
                //lLineStyle.Setters.Add(new Setter(Polyline.StrokeEndLineCapProperty, PenLineCap.Triangle));

                Style lPointStyle = new Style(typeof(DataPoint));

                if (xYShift == 0.0)
                {
                    lLineStyle.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 3.0));
                    lPointStyle.Setters.Add(new Setter(DataPoint.WidthProperty, 0.0));
                    lPointStyle.Setters.Add(new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(Colors.LightGreen)));
                }
                else
                {
                    lLineStyle.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 1.0));
                    lPointStyle.Setters.Add(new Setter(DataPoint.WidthProperty, 0.0));
                    lPointStyle.Setters.Add(new Setter(DataPoint.BackgroundProperty, new SolidColorBrush(Colors.Black)));
                }
                lLine.PolylineStyle = lLineStyle;
                lLine.DataPointStyle = lPointStyle;

                xCurvePoints[x][0].CurveId -= xYShift;
                xCurvePoints[x][1].CurveId -= xYShift;
                lLine.ItemsSource = xCurvePoints[x];
                _MainWindow.myChart.Series.Add(lLine);
            }
        } //

        public void AddInfoText(string xText)
        {
            InfoText lInfoText = new InfoText(xText);
            Action lAction = () => {
                Messages.Add(lInfoText);

                // and scroll to the end
                if (_MainWindow.InfoText.Items.Count <= 0) return;
                Decorator lDecorator = VisualTreeHelper.GetChild(_MainWindow.InfoText, 0) as Decorator;
                if (lDecorator == null) return;
                ScrollViewer lScrollViewer = lDecorator.Child as ScrollViewer;
                if (lScrollViewer == null) return;
                lScrollViewer.ScrollToEnd();
            };
            App.Current.Dispatcher.BeginInvoke(lAction);
        } //

    } // class
}
