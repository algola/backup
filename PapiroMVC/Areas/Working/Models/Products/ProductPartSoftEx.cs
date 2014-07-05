using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Novacode;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartSoft))]
    [MetadataType(typeof(ProductPartSoftEx_MetaData))]
    public partial class ProductPartSoft : ProductPart, IPrintDocX
    {
        public ProductPartSoft()
        {
            TypeOfProductPart = ProductPartType.ProductPartSoft;
        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);
        }

    }
}
