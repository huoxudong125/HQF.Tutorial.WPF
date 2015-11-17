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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _Model;
        private bool _DrawNewLine = false;

        public MainWindow()
        {
            InitializeComponent();
        } // constructor

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel lViewModel = new ViewModel(myChart);
            DataContext = lViewModel;
            _Model = new Model(lViewModel);
        } // 

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if ((!_DrawNewLine) && (e.LeftButton == MouseButtonState.Pressed))
            {
                MouseButtonEventArgs lMouseButtonEventArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left);
                OnMouseLeftButtonDown(sender, lMouseButtonEventArgs);
                return;
            }

            IInputElement lInputElement = sender as IInputElement; // == basically Chart or LineSeries
            Point lPoint = e.GetPosition(lInputElement);

            Chart lChart = sender as Chart;
            if (lChart != null)
            {
                string s = string.Empty;

                // iterate through all axes
                lock (lChart.ActualAxes)
                {
                    foreach (IAxis lAxis in lChart.ActualAxes)
                    {
                        RangeAxis lRangeAxis = lAxis as RangeAxis;
                        if (lRangeAxis == null) continue; // won't happen
                        AxisPoint lAxisPoint = AxisPointFactory.getAxisPoint(lChart, lRangeAxis, lPoint);

                        s += lAxisPoint.ToString() + Environment.NewLine;
                    }
                }

                InfoBox.Text = s;
                return;
            }

            LineSeries lLineSeries = sender as LineSeries;
            if (lLineSeries != null)
            {
                IInputElement lSelection = lLineSeries.InputHitTest(lPoint);
                if (lSelection == null) return;
                InfoBox.Text = "LineSeries object: " + lSelection.GetType().ToString();
                return;
            }
        } //

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Chart lChart = sender as Chart;
            if (lChart == null) return;

            AxisPoint lAxisPointX;
            AxisPoint lAxisPointY;
            LineSeries lLineSeries = lChart.Series[0] as LineSeries;
            if (lLineSeries == null) return;   // won't happen anyway
            if (!getAxisPointsToDrawALine(sender, e, lLineSeries, out lAxisPointX, out lAxisPointY)) return;

            AxisPointDateTime lAxisPointDateTimeX = lAxisPointX as AxisPointDateTime;
            AxisPointLinear lAxisPointLinearY = lAxisPointY as AxisPointLinear;

            if (lAxisPointDateTimeX == null) return;  // Quick and dirty test. Should not happen anyway.
            if (lAxisPointLinearY == null) return;  // Might be helpful when you change the code and forget about this requirement in another scenario.

            DrawCurve_WPF_Runtime(lChart, lAxisPointDateTimeX, lAxisPointLinearY);
        } //

        // add a new curve and its first point
        private ObservableCollection<PriceClusterSimple> ManualPoints = null;
        private void DrawCurve_WPF_Runtime(Chart lChart, AxisPointDateTime lAxisPointDateTimeX, AxisPointLinear lAxisPointLinearY)
        {
            PriceClusterSimple lDataPoint = new PriceClusterSimple(lAxisPointDateTimeX.MouseAxisValueAbsolute, lAxisPointLinearY.MouseAxisValueAbsolute);

            if (ManualPoints != null)
            {
                if (_DrawNewLine)
                {
                    _DrawNewLine = false;
                    ManualPoints.Clear();
                    ManualPoints.Add(lDataPoint); // from
                    ManualPoints.Add(lDataPoint); // to
                }
                else
                {
                    ManualPoints.RemoveAt(1);
                    ManualPoints.Add(lDataPoint); // to
                }
                return;
            }

            ManualPoints = new ObservableCollection<PriceClusterSimple>();
            ManualPoints.Add(lDataPoint);
            ManualPoints.Add(lDataPoint);
            LineSeries lNewLineSeries = new LineSeries();
            lNewLineSeries.Title = "manually added curve";
            lNewLineSeries.SetBinding(LineSeries.ItemsSourceProperty, new Binding());
            lNewLineSeries.ItemsSource = ManualPoints;
            lNewLineSeries.IndependentValueBinding = new Binding("Date");
            lNewLineSeries.DependentValueBinding = new Binding("Price");
            lNewLineSeries.DependentRangeAxis = lAxisPointLinearY.Axis;
            Setter lHeightSetter = new Setter(FrameworkElement.HeightProperty, 0.0);
            Setter lWidthSetter = new Setter(FrameworkElement.WidthProperty, 0.0);
            Setter lColor = new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.Black));
            Style lStyle = new Style(typeof(Control));
            lNewLineSeries.DataPointStyle = lStyle;
            lStyle.Setters.Add(lHeightSetter);
            lStyle.Setters.Add(lWidthSetter);
            lStyle.Setters.Add(lColor);
            lChart.Series.Add(lNewLineSeries);
        } //

        private bool getAxisPointsToDrawALine(object xSender, MouseButtonEventArgs xMouseButtonEventArgs, LineSeries xLineSeries, out AxisPoint xAxisPointX, out AxisPoint xAxisPointY)
        {
            IInputElement lInputElement = xSender as IInputElement; // == basically Chart or LineSeries
            Point lPoint = xMouseButtonEventArgs.GetPosition(lInputElement);
            xAxisPointX = null;
            xAxisPointY = null;

            Chart lChart = xSender as Chart;
            if (lChart == null) return false;

            RangeAxis lRangeAxis;
            AxisPoint lAxisPoint;

            lRangeAxis = xLineSeries.ActualDependentRangeAxis as RangeAxis;
            if (lRangeAxis == null) return false; // won't happen in our example
            lAxisPoint = AxisPointFactory.getAxisPoint(lChart, lRangeAxis, lPoint);
            if (lRangeAxis.Orientation == AxisOrientation.X) xAxisPointX = lAxisPoint;
            else xAxisPointY = lAxisPoint;

            lRangeAxis = xLineSeries.ActualIndependentAxis as RangeAxis;
            if (lRangeAxis == null) return false; // won't happen in our example
            lAxisPoint = AxisPointFactory.getAxisPoint(lChart, lRangeAxis, lPoint);
            if (lRangeAxis.Orientation == AxisOrientation.X) xAxisPointX = lAxisPoint;
            else xAxisPointY = lAxisPoint;

            if ((xAxisPointX == null) || (xAxisPointY == null)) return false;
            return true;
        } //

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _DrawNewLine = true;
        } //
    }
}
