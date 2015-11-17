using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
    public class Model
    {
        private ViewModel _ViewModel;

        public Model(ViewModel xViewModel)
        {
            _ViewModel = xViewModel;
            DispatcherTimer lTimer = new DispatcherTimer();
            lTimer.Interval = new TimeSpan(0, 0, 3);
            lTimer.Tick += new EventHandler(Timer_Tick);
            lTimer.Start();
        } // constructor

        void Timer_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            PriceCluster lPriceCluster = _ViewModel.Points.Last();
            double lVW = lPriceCluster.PriceVW * (1 + ((2.0 * (r.NextDouble() - 0.5)) / 30.0));
            double lDaimler = lPriceCluster.PriceDaimler * (1 + ((2.0 * (r.NextDouble() - 0.5)) / 30.0));
            _ViewModel.AddPoint(lPriceCluster.Date.AddDays(1), lVW, lDaimler);
        } //

    } // class
}
