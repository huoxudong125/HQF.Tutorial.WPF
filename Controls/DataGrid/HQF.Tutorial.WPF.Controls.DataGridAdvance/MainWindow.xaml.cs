using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace HQF.Tutorial.WPF.Controls.DataGridAdvance
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // set the window DataContext
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var lPersons = new List<Person>();
            lPersons.Add(new Person
            {
                FirstName = "Liza",
                LastName = "Minnelli",
                Birthday = new DateTime(1946, 03, 12),
                Alive = true,
                Homepage = "www.officiallizaminnelli.com"
            });
            lPersons.Add(new Person
            {
                FirstName = "Bastian",
                LastName = "Ohta",
                Birthday = new DateTime(1975, 03, 13),
                Alive = true,
                Homepage = "www.ohta.de"
            });
            lPersons.Add(new Person
            {
                FirstName = "Albert",
                LastName = "Einstein",
                Birthday = new DateTime(1879, 03, 14),
                Alive = false,
                Homepage = "www.alberteinsteinsite.com"
            });
            lPersons.Add(new Person
            {
                FirstName = "Coenraad",
                LastName = "van Houten",
                Birthday = new DateTime(1801, 03, 15),
                Alive = false,
                Homepage = "www.vanhoutendrinks.com"
            });
            lPersons.Add(new Person
            {
                FirstName = "Andrew",
                LastName = "Miller-Jones",
                Birthday = new DateTime(1910, 03, 16),
                Alive = false,
                Homepage = "dead as a Dodo"
            });
            lPersons.Add(new Person
            {
                FirstName = "Gottlieb",
                LastName = "Daimler",
                Birthday = new DateTime(1834, 03, 17),
                Alive = false,
                Homepage = "www.daimler.com"
            });
            lPersons.Add(new Person
            {
                FirstName = "Rudolf",
                LastName = "Diesel",
                Birthday = new DateTime(1858, 03, 18),
                Alive = false,
                Homepage = "http://en.wikipedia.org/wiki/Rudolf_Diesel"
            });
            DataContext = lPersons;
        } //

        // exit the application
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        } //

        // open the hyperlink in a browser
        private void Hyperlink_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var lHyperlink = e.OriginalSource as Hyperlink;
                var lUri = lHyperlink.NavigateUri.OriginalString;
                Process.Start(lUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } //

        // find the correct DataGridRow and set the DetailsVisibility
        private void DataGridRowHeader_Button_Click(object sender, RoutedEventArgs e)
        {
            var lDependencyObject = e.OriginalSource as DependencyObject;
            //Button lButton = lDependencyObject as Button;
            //if (lButton == null) return;
            while (!(lDependencyObject is DataGridRow) && lDependencyObject != null)
                lDependencyObject = VisualTreeHelper.GetParent(lDependencyObject);
            var lRow = lDependencyObject as DataGridRow;
            if (lRow == null) return;
            //lRow.IsSelected = (lRow.DetailsVisibility != Visibility.Visible);
            lRow.DetailsVisibility = lRow.DetailsVisibility == Visibility.Collapsed
                ? Visibility.Visible
                : Visibility.Collapsed;
            Console.WriteLine(lRow.ActualHeight);
        } //

        private void DataGrid_DatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            // get the DatePicker control
            var lDatePicker = sender as DatePicker;
            lDatePicker.VerticalContentAlignment = VerticalAlignment.Center;

            // find the inner textbox and adjust the Background colour
            var lInnerTextBox = lDatePicker.Template.FindName("PART_TextBox", lDatePicker) as DatePickerTextBox;
            lInnerTextBox.Background = Brushes.Transparent;
            lInnerTextBox.VerticalContentAlignment = VerticalAlignment.Center;
            lInnerTextBox.Height = lDatePicker.ActualHeight - 2;

            // remove watermark
            var lWatermark = lInnerTextBox.Template.FindName("PART_Watermark", lInnerTextBox) as ContentControl;
            lWatermark.IsHitTestVisible = false;
            lWatermark.Focusable = false;
            lWatermark.Visibility = Visibility.Collapsed;
            lWatermark.Opacity = 0;

            // just as demo
            var lContentHost = lInnerTextBox.Template.FindName("PART_ContentHost", lInnerTextBox) as ContentControl;

            // remove ugly borders
            RemoveBorders(lInnerTextBox);
                // hardcore <span class="wp-smiley wp-emoji wp-emoji-smile" title=":)">:)</span>
        } //

        private static void RemoveBorders(DependencyObject xDependencyObject)
        {
            for (int i = 0, n = VisualTreeHelper.GetChildrenCount(xDependencyObject); i < n; i++)
            {
                var lDependencyObject = VisualTreeHelper.GetChild(xDependencyObject, i);
                RemoveBorders(lDependencyObject);
                var lBorder = lDependencyObject as Border;
                if (lBorder == null) continue;
                lBorder.BorderBrush = Brushes.Transparent;
            }
        } //

        public class Person : INotifyPropertyChanged
        {
            private bool _Alive;

            private DateTime _Birthday;

            private string _FirstName;

            private string _LastName;

            public bool Alive
            {
                get { return _Alive; }
                set
                {
                    _Alive = value;
                    OnPropertyChanged("Alive");
                }
            }

            public string FirstName
            {
                get { return _FirstName; }
                set
                {
                    _FirstName = value;
                    OnPropertyChanged("FirstName");
                }
            }

            public string LastName
            {
                get { return _LastName; }
                set
                {
                    _LastName = value;
                    OnPropertyChanged("LastName");
                }
            }

            public double Age
            {
                get { return DateTime.Now.Subtract(Birthday).TotalDays/365; }
            }

            public string Homepage { get; set; }

            public DateTime Birthday
            {
                get { return _Birthday; }
                set
                {
                    _Birthday = value;
                    OnPropertyChanged("Birthday");
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string xName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(xName));
                }
            } //
        } // class
    }
}