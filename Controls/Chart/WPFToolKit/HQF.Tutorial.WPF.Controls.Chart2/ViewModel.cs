using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;

namespace HQF.Tutorial.WPF.Controls.Chart2
{
    public class ViewModel
    {
        private readonly Chart _Chart;
        public ReadOnlyObservableCollection<PriceCluster> Points { get; private set; }
        private ObservableCollection<PriceCluster> _Points = new ObservableCollection<PriceCluster>();

        public ViewModel(Chart xChart)
        {
            _Chart = xChart;

            AddPoint(new DateTime(2014, 04, 10), 67.29, 13.85);
            AddPoint(new DateTime(2014, 04, 11), 66.15, 13.66);
            AddPoint(new DateTime(2014, 04, 14), 66.22, 13.67);
            AddPoint(new DateTime(2014, 04, 15), 63.99, 13.49);
            AddPoint(new DateTime(2014, 04, 16), 65.32, 13.62);
            AddPoint(new DateTime(2014, 04, 17), 67.29, 13.73);
            AddPoint(new DateTime(2014, 04, 22), 68.72, 13.91);
            AddPoint(new DateTime(2014, 04, 23), 67.85, 13.84);
            AddPoint(new DateTime(2014, 04, 24), 67.75, 13.78);
            AddPoint(new DateTime(2014, 04, 25), 66.29, 13.60);
            AddPoint(new DateTime(2014, 04, 28), 66.99, 13.73);
            AddPoint(new DateTime(2014, 04, 29), 67.79, 13.91);
            AddPoint(new DateTime(2014, 04, 30), 66.73, 13.79);
            AddPoint(new DateTime(2014, 05, 02), 66.24, 13.10);
            AddPoint(new DateTime(2014, 05, 05), 65.90, 13.08);
            AddPoint(new DateTime(2014, 05, 06), 65.16, 13.04);
            AddPoint(new DateTime(2014, 05, 07), 64.80, 13.18);
            AddPoint(new DateTime(2014, 05, 08), 65.00, 13.45);
            AddPoint(new DateTime(2014, 05, 09), 64.52, 13.42);
            AddPoint(new DateTime(2014, 05, 12), 65.28, 13.58);
            AddPoint(new DateTime(2014, 05, 13), 66.48, 13.40);
            AddPoint(new DateTime(2014, 05, 14), 66.74, 13.26);
            AddPoint(new DateTime(2014, 05, 15), 66.00, 12.97);
            AddPoint(new DateTime(2014, 05, 16), 65.21, 13.08);
            AddPoint(new DateTime(2014, 05, 19), 66.02, 13.38);
            AddPoint(new DateTime(2014, 05, 20), 66.46, 13.42);
            AddPoint(new DateTime(2014, 05, 21), 67.15, 13.84);
            AddPoint(new DateTime(2014, 05, 22), 67.52, 13.84);
            AddPoint(new DateTime(2014, 05, 23), 68.14, 14.06);
            AddPoint(new DateTime(2014, 05, 26), 69.61, 14.17);
            AddPoint(new DateTime(2014, 05, 27), 69.56, 14.15);
            AddPoint(new DateTime(2014, 05, 28), 69.29, 14.17);
            AddPoint(new DateTime(2014, 05, 29), 69.65, 14.18);
            AddPoint(new DateTime(2014, 05, 30), 69.70, 14.29);
            AddPoint(new DateTime(2014, 06, 02), 69.32, 14.31);
            AddPoint(new DateTime(2014, 06, 03), 69.68, 14.32);
            AddPoint(new DateTime(2014, 06, 04), 69.31, 14.31);
            AddPoint(new DateTime(2014, 06, 05), 70.31, 14.34);
            AddPoint(new DateTime(2014, 06, 06), 70.24, 14.42);
            AddPoint(new DateTime(2014, 06, 09), 70.09, 14.42);
            AddPoint(new DateTime(2014, 06, 10), 70.08, 14.47);
            AddPoint(new DateTime(2014, 06, 11), 69.66, 14.30);
            AddPoint(new DateTime(2014, 06, 12), 69.49, 14.26);
            AddPoint(new DateTime(2014, 06, 13), 69.12, 14.42);
            AddPoint(new DateTime(2014, 06, 16), 69.05, 14.44);
            AddPoint(new DateTime(2014, 06, 17), 69.65, 14.43);
            AddPoint(new DateTime(2014, 06, 18), 69.62, 14.62);
            AddPoint(new DateTime(2014, 06, 19), 70.10, 14.93);
            AddPoint(new DateTime(2014, 06, 20), 70.08, 14.93);
            AddPoint(new DateTime(2014, 06, 23), 69.46, 14.97);
            AddPoint(new DateTime(2014, 06, 24), 69.04, 15.06);
            AddPoint(new DateTime(2014, 06, 25), 68.71, 14.89);
            AddPoint(new DateTime(2014, 06, 26), 68.14, 15.12);
            AddPoint(new DateTime(2014, 06, 27), 68.33, 15.17);
            AddPoint(new DateTime(2014, 06, 30), 68.40, 15.08);
            AddPoint(new DateTime(2014, 07, 01), 69.19, 15.21);
            AddPoint(new DateTime(2014, 07, 02), 69.72, 15.20);
            AddPoint(new DateTime(2014, 07, 03), 70.44, 15.31);
            AddPoint(new DateTime(2014, 07, 04), 70.44, 15.16);
            AddPoint(new DateTime(2014, 07, 07), 69.28, 14.95);
            AddPoint(new DateTime(2014, 07, 08), 68.15, 14.84);
            AddPoint(new DateTime(2014, 07, 09), 68.16, 14.73);
            AddPoint(new DateTime(2014, 07, 10), 67.05, 14.43);
            AddPoint(new DateTime(2014, 07, 11), 66.68, 14.50);
            AddPoint(new DateTime(2014, 07, 14), 67.61, 14.60);
            AddPoint(new DateTime(2014, 07, 15), 67.28, 14.70);
            AddPoint(new DateTime(2014, 07, 16), 67.77, 14.89);
            AddPoint(new DateTime(2014, 07, 17), 66.56, 14.53);
            AddPoint(new DateTime(2014, 07, 18), 65.40, 14.52);
            AddPoint(new DateTime(2014, 07, 21), 64.84, 14.49);
            AddPoint(new DateTime(2014, 07, 22), 66.09, 14.83);
            AddPoint(new DateTime(2014, 07, 23), 65.58, 14.74);
            AddPoint(new DateTime(2014, 07, 24), 66.30, 14.92);
            AddPoint(new DateTime(2014, 07, 25), 65.15, 14.65);
            AddPoint(new DateTime(2014, 07, 28), 63.08, 14.61);
            AddPoint(new DateTime(2014, 07, 29), 63.89, 14.71);
            AddPoint(new DateTime(2014, 07, 30), 63.07, 14.43);
            AddPoint(new DateTime(2014, 07, 31), 61.88, 14.13);
            AddPoint(new DateTime(2014, 08, 01), 60.85, 13.60);
            AddPoint(new DateTime(2014, 08, 04), 61.17, 13.58);
            AddPoint(new DateTime(2014, 08, 05), 60.43, 13.61);
            AddPoint(new DateTime(2014, 08, 06), 59.82, 13.40);
            AddPoint(new DateTime(2014, 08, 07), 58.95, 13.16);
            AddPoint(new DateTime(2014, 08, 08), 59.27, 13.16);
            AddPoint(new DateTime(2014, 08, 11), 60.71, 13.36);
            AddPoint(new DateTime(2014, 08, 12), 59.85, 13.17);
            AddPoint(new DateTime(2014, 08, 13), 60.66, 13.80);
            AddPoint(new DateTime(2014, 08, 14), 61.07, 13.77);
            AddPoint(new DateTime(2014, 08, 15), 59.71, 13.65);
            AddPoint(new DateTime(2014, 08, 18), 60.99, 13.72);
            AddPoint(new DateTime(2014, 08, 19), 61.60, 13.72);
            AddPoint(new DateTime(2014, 08, 20), 61.33, 13.82);
            AddPoint(new DateTime(2014, 08, 21), 62.20, 13.86);
            AddPoint(new DateTime(2014, 08, 22), 61.65, 13.70);
            AddPoint(new DateTime(2014, 08, 25), 62.88, 13.88);
            AddPoint(new DateTime(2014, 08, 26), 63.49, 13.87);
            AddPoint(new DateTime(2014, 08, 27), 63.15, 13.89);
            AddPoint(new DateTime(2014, 08, 28), 62.16, 13.77);
            AddPoint(new DateTime(2014, 08, 29), 62.24, 13.83);
            AddPoint(new DateTime(2014, 09, 01), 61.88, 13.92);
            AddPoint(new DateTime(2014, 09, 02), 61.82, 13.92);
            AddPoint(new DateTime(2014, 09, 03), 62.90, 14.17);
            AddPoint(new DateTime(2014, 09, 04), 64.14, 14.34);
            AddPoint(new DateTime(2014, 09, 05), 65.17, 14.40);

            Points = new ReadOnlyObservableCollection<PriceCluster>(_Points);
        } // constructor

        // only to be called from the dispatcher thread!
        public void AddPoint(DateTime xDate, double xPriceVW, double xPriceDaimler)
        {
            _Points.Add(new PriceCluster(xDate, xPriceVW, xPriceDaimler));
        } //

    } // class
}
