using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    using PapiroMVC.Models.Resources.TaskExecutor;
    using PapiroMVC.Validation;
    using System;
    using System.Collections.Generic;

    public partial class BindingCostPerRunStep_MetaData : Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit4")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit4 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost4")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost4 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit6")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit6 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost6")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost6 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit8")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit8 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost8")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost8 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit12")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit12 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost12")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost12 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit16")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit16 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost16")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost16 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit24")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit24 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost24")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost24 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "CostPerUnit32")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> CostPerUnit32 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "StartingCost32")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> StartingCost32 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingCostPerRunStep), "BindingCost")]         
        [RegularExpressionLocalized(typeof(ResBindingCostPerRunStep), "CurrencyValidation", "CurrencyValidationError")]
        public Nullable<double> BindingCost { get; set; }
    }
}