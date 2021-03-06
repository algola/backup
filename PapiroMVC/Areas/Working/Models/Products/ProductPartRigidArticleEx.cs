﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [KnownType(typeof(ProductPartRigidArticle))]
    [MetadataType(typeof(ProductPartRigidArticleEx_MetaData))]
    public partial class ProductPartRigidArticle : ProductPartsPrintableArticle
    {
        public ProductPartRigidArticle()
        {
            TypeOfProductPartsPrintableArticle = TypeOfProductPartsPrintableArticleType.ProductPartRigidArticle;
        }
    }
}
