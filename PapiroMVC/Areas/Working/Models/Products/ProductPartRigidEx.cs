using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartRigidEx_MetaData))]
    public partial class ProductPartRigid : ProductPart
    {
        public ProductPartRigid()
        {
            TypeOfProductPart = ProductPartType.ProductPartRigid;
        }
    }
}
