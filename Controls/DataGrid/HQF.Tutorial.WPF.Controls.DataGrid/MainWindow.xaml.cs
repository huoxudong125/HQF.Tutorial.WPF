using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;

namespace HQF.Tutorial.WPF.Controls.DataGrid
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

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(0);
        } //

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

        public class Person
        {
            public bool Alive { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public DateTime Birthday { get; set; }

            public double Age
            {
                get { return DateTime.Now.Subtract(Birthday).TotalDays/365; }
            }

            public string Homepage { get; set; }
        } //
    }
}