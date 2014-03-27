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
            TaskExecutor tsk;

            try
            {
                tsk = this.CostDetail.TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == this.CostDetail.CodTaskExecutorSelected);
                gain.Pinza = tsk.Pinza;
                gain.ControPinza = tsk.ControPinza;
                gain.Laterale = tsk.Laterale;
            }
            catch (Exception)
            {
                gain.Pinza = 0;
                gain.ControPinza = 0;
                gain.Laterale = 0;
            }

            gain.LargerFormat = PrintingFormat;            
            gain.SmallerFormat = Part.FormatOpened;

            if (gain.SmallerFormat == "" || gain.SmallerFormat == null)
            {
                gain.SmallerFormat = Part.Format;
            }

            //posso spostare questa assegnazione quando modifico la quantità del
            //DocumentProduct
            gain.Quantity = this.CostDetail.TaskCost.DocumentProduct.Quantity ?? 0;
            gain.DCut = (Part.IsDCut ?? false) ? Part.DCut : 0;

            gain.DCut1 = (Part.IsDCut ?? false) ? Part.DCut1 : 0;
            gain.DCut2 = (Part.IsDCut ?? false) ? Part.DCut2 : 0;

            GainPartOnPrinting = gain;
        }
    }
}