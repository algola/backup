﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [Serializable]
    [KnownType(typeof(ProductPartRigid))]
    [MetadataType(typeof(ProductPartRigidEx_MetaData))]
    public partial class ProductPartRigid : ProductPart
    {
        public ProductPartRigid()
        {
            TypeOfProductPart = ProductPartType.ProductPartRigid;
        }
    }
}
