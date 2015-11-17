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

namespace HQF.Tutorial.WPF.Controls.Chart4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Model _Model;
        private ViewModel _ViewModel;
        private Zoom _Zoom;

        public MainWindow()
        {
            InitializeComponent();
        } // constructor

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _Zoom = new Zoom(myChart);
            _ViewModel = new ViewModel(myChart);
            DataContext = _ViewModel;
            _Model = new Model(_ViewModel);
        } // 

        private void OnKeyDown(object xSender, KeyEventArgs e) { _Zoom.OnKeyDown(xSender, e); }

        private void OnMouseLeftButtonDown(object xSender, MouseButtonEventArgs e) { _Zoom.OnMouseLeftButtonDown(xSender, e); }
        private void OnMouseLeftButtonUp(object xSender, MouseButtonEventArgs e) { _Zoom.OnMouseLeftButtonUp(xSender, e); }
        private void OnMouseMove(object xSender, MouseEventArgs e) { _Zoom.OnMouseMove(xSender, e); }

        private void OnTouchDown(object xSender, TouchEventArgs e) { _Zoom.OnTouchDown(xSender, e); }
        private void OnTouchMove(object xSender, TouchEventArgs e) { _Zoom.OnTouchMove(xSender, e); }
        private void OnTouchUp(object xSender, TouchEventArgs e) { _Zoom.OnTouchUp(xSender, e); }

        DateTime _LastOnManipulationDelta;
        private void OnManipulationDelta(object sender, ManipulationDeltaEventArgs e)
        {
            if (DateTime.Now.Subtract(_LastOnManipulationDelta).TotalMilliseconds < 500) return;  // throttle
            _LastOnManipulationDelta = DateTime.Now;

            InfoBox.Text =
                  "Expansion: " + e.CumulativeManipulation.Expansion.ToString() + Environment.NewLine +
                  "Rotation: " + e.CumulativeManipulation.Rotation.ToString() + Environment.NewLine +
                  "Scale: " + e.CumulativeManipulation.Scale.ToString() + Environment.NewLine +
                  "Translation: " + e.CumulativeManipulation.Translation.ToString() + Environment.NewLine +
                  "Exp " + e.CumulativeManipulation.Expansion.X + "/" + e.CumulativeManipulation.Expansion.Y;
        } //
    }
}
