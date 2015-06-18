using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartTaskOption_Metadata))]
    public partial class ProductPartTaskOption
    {
        public enum ProductPartTaskOptionType : int
        {
            ProductPartSerigraphyOption = 0,
            ProductPartHotPrintingOption = 1,
        }

        public ProductPartTaskOptionType TypeOfProductPartTaskOption
        {
            get;
            protected set;
        }


    }
}