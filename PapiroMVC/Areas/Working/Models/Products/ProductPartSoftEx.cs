﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartSoft))]
    [MetadataType(typeof(ProductPartSoftEx_MetaData))]
    public partial class ProductPartSoft : ProductPart
    {
        public ProductPartSoft()
        {
            TypeOfProductPart = ProductPartType.ProductPartSoft;
        }
    }
}