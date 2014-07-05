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
    
    [KnownType(typeof(ProductPartRigid))]
    [MetadataType(typeof(ProductPartRigidEx_MetaData))]
    public partial class ProductPartRigid : ProductPart, IPrintDocX
    {
        public ProductPartRigid()
        {
            TypeOfProductPart = ProductPartType.ProductPartRigid;
        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);
        }


    }


}
