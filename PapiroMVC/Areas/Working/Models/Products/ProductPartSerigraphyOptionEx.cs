using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartSerigraphyOption_Metadata))]

    public partial class ProductPartSerigraphyOption : ProductPartTaskOption
    {

        public ProductPartSerigraphyOption()
        {

            TypeOfProductPartTaskOption = ProductPartTaskOptionType.ProductPartSerigraphyOption;
        }
    }
}