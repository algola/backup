using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class CostPerColorStep_MetaData : Step_MetaData
    {

        [RegularExpressionLocalized(typeof(ResTaskEstimatedOn), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResCostPerColorStep), "CostPerUnit")]
        public Nullable<double> CostPerUnit { get; set; }
    }
}
