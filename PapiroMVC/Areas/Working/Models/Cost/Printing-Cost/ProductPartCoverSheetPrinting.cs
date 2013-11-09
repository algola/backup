using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    //each type has a specific view with particular property
    //ie: single sheet has a max gain and so other
    public class ProductPartCoverSheetPrinting : ProductPartPrintingSheet
    {
        public ProductPartCoverSheetPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartCoverSheetPrinting;
        }

        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingSheetGainSingle();
            }

            base.Update();

            ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartCoverSheet)Part).SubjectNumber ?? 1;

            GainPartOnPrinting.UsePerfecting = false;
            GainPartOnPrinting.CalculateGain();
        }
    }

}