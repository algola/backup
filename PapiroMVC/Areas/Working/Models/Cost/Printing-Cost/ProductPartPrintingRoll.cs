using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public partial class ProductPartRollPrinting : ProductPartPrinting
    {

        public override void Update()
        {
            var gain = (ProductPartPrintingRollGain)this.GainPartOnPrinting;
            //prima si calcola 

            gain.Width = Width??100;
            gain.SmallerFormat = Part.FormatOpened;
            if (gain.SmallerFormat == "" || gain.SmallerFormat == null)
            {
                gain.SmallerFormat = Part.Format;
            }

            this.GainPartOnPrinting = gain;
        }
    }

}