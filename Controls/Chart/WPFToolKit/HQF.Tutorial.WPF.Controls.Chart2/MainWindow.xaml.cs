using System;
using System.Collections.Generic;
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

namespace HQF.Tutorial.WPF.Controls.Chart2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        private Model _Model;

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

            IInputElement lInputElement = sender as IInputElement; // == Chart, LineSeries ...
            Chart lChart = sender as Chart;
            LineSeries lLineSeries = sender as LineSeries;

            Point lPoint = e.GetPosition(lInputElement);
            if (lChart != null)
            {
                IInputElement lSelection = lChart.InputHitTest(lPoint);
                if (lSelection == null) return;
                InfoBox.Text = lSelection.GetType().ToString();
            }
            else if (lLineSeries != null)
            {
                IInputElement lSelection = lLineSeries.InputHitTest(lPoint);
                if (lSelection == null) return;
                InfoBox.Text = lSelection.GetType().ToString();
            }
        } //
    }
}
