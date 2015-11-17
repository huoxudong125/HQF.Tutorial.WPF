using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQF.Tutorial.WPF.Controls.Chart5
{
    public class InfoText
    {
        public string TimeStamp { get; private set; }
        public string Text { get; private set; }

        public InfoText(string xText)
        {
            TimeStamp = DateTime.Now.ToString("HH:mm:ss_fff");
            Text = xText;
        } //

    } // classs
}
