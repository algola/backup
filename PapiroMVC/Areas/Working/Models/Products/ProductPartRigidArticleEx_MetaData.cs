﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public partial class ProductPartRigidArticleEx_MetaData : ProductPartsPrintableArticle_MetaData
    {

        [DisplayNameLocalized(typeof(ResProductPartRigidArticle), "RoundTo")]
        [Tooltip(typeof(ResProductPartRigidArticle), "RoundToToolTip")]
        public Nullable<bool> RoundTo { get; set; }
    }
}