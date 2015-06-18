using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartHotPrintingOption_Metadata))]
    public partial class ProductPartHotPrintingOption: ProductPartTaskOption
    {
        public ProductPartHotPrintingOption()
        {
            this.TypeOfProductPartTaskOption = ProductPartTaskOptionType.ProductPartHotPrintingOption;
        }
    }
}