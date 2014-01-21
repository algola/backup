using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    //each type has a specific view with particular property
    //ie: single sheet has a max gain and so other
    public partial class ProductPartRigidPrinting : ProductPartSheetPrinting
    {
        public ProductPartRigidPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartRigidPrinting;
        }

        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingSheetGainSingle();            
            }

            base.Update();

            ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartRigid)Part).SubjectNumber ?? 1;
            ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).Quantity = this.CostDetail.TaskCost.DocumentProduct.Quantity??0;

            ((ProductPartPrintingSheetGainSingle)GainPartOnPrinting).UsePerfecting = false;
            GainPartOnPrinting.CalculateGain();
        }
    }

}