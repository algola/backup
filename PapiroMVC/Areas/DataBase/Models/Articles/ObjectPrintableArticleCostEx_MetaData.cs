﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class ObjectPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"CostPerUnit")]
        public Nullable<double> CostPerUnit { get; set; }
    }
}