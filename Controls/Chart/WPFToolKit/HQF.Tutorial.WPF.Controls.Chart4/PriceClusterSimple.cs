using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQF.Tutorial.WPF.Controls.Chart4
{
    // class

    public class PriceClusterSimple
    {
        public DateTime Date { get; set; }
        public double Price { get; set; }

        public PriceClusterSimple(DateTime xDate, double xPrice)
        {
            Date = xDate;
            Price = xPrice;
        } // constructor
    } // class
}
