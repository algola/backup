﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    public class RollPrintableArticleCost_MetaData
    {
        [DisplayNameLocalized(typeof(Strings),"CostPerMl")]
        [CurrencyLocalized(typeof(Strings), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerMl { get; set; }
        [DisplayNameLocalized(typeof(Strings),"CostPerMq")]
        [CurrencyLocalized(typeof(Strings), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerMq { get; set; }
    }
}