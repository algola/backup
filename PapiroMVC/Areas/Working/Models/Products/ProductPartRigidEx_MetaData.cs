using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public partial class ProductPartRigidEx_MetaData : ProductPart_MetaData
    {
        public Nullable<int> SubjectNumber { get; set; }
    }
}