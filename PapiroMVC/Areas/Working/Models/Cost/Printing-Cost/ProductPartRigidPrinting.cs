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

    }

}