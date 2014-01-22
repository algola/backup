using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public partial class ProductPartSheetPrinting : ProductPartPrinting
    {

        public override void Update()
        {
            var gain = (ProductPartPrintingSheetGain)GainPartOnPrinting;

            gain.LargerFormat = PrintingFormat;
            gain.SmallerFormat = Part.FormatOpened;


            if (gain.SmallerFormat == "" || gain.SmallerFormat == null)
            {
                gain.SmallerFormat = Part.Format;
            }

            gain.Quantity = this.CostDetail.TaskCost.DocumentProduct.Quantity ?? 0;
            gain.DCut = (Part.IsDCut??false)?Part.DCut:0;

            GainPartOnPrinting = gain;
        }
    }
}