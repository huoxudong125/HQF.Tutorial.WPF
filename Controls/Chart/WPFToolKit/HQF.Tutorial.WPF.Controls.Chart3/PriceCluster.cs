using System;

namespace HQF.Tutorial.WPF.Controls.Chart3
{
    public class PriceCluster
    {
        public DateTime Date { get; set; }
        public double PriceVW { get; set; }
        public double PriceDaimler { get; set; }

        public PriceCluster(DateTime xDate, double xPriceVW, double xPriceDaimler)
        {
            Date = xDate;
            PriceVW = xPriceVW;
            PriceDaimler = xPriceDaimler;
        } // constructor
    }
}