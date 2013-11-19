using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    //each type has a specific view with particular property
    //ie: single sheet has a max gain and so other
    public partial class ProductPartSinglePlotterPrinting : ProductPartPlotterPrinting
    {
        public ProductPartSinglePlotterPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartSinglePlotterPrinting;
        }

        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingPlotterGainSingle();            
            }

            base.Update();

            ((ProductPartPrintingPlotterGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSinglePlotter)Part).SubjectNumber ?? 1;

            GainPartOnPrinting.CalculateGain();
        }
    }

}