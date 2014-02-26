using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    //each type has a specific view with particular property
    //ie: single sheet has a max gain and so other
    public partial class ProductPartSingleRollPrinting : ProductPartRollPrinting
    {
        public ProductPartSingleRollPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartSingleRollPrinting;
        }

        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingRollGainSingle();            
            }

            base.Update();

            ((ProductPartPrintingRollGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSinglePlotter)Part).SubjectNumber ?? 1;

            GainPartOnPrinting.CalculateGain();
        }
    }

}