using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.TaskExecutor;

namespace PapiroMVC.Models
{

    public partial class BindingEstimatedOnRun_MetaData : TaskEstimatedOn_MetaData
    {
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit4")]
        public Nullable<double> CostPerUnit4 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost4")]
        public Nullable<double> StartingCost4 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit6")]
        public Nullable<double> CostPerUnit6 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost6")]
        public Nullable<double> StartingCost6 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit8")]
        public Nullable<double> CostPerUnit8 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost8")]
        public Nullable<double> StartingCost8 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit12")]
        public Nullable<double> CostPerUnit12 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost12")]
        public Nullable<double> StartingCost12 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit16")]
        public Nullable<double> CostPerUnit16 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost16")]
        public Nullable<double> StartingCost16 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit24")]
        public Nullable<double> CostPerUnit24 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost24")]
        public Nullable<double> StartingCost24 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "CostPerUnit32")]
        public Nullable<double> CostPerUnit32 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "StartingCost32")]
        public Nullable<double> StartingCost32 { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "BindingCost")]
        public Nullable<double> BindingCost { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "UseDifferentCostPerUnit")]
        public Nullable<bool> UseDifferentCostPerUnit { get; set; }
        [RegularExpressionLocalized(typeof(ResBindingEstimatedOnRun), "CurrencyValidation", "CurrencyValidationError")]

        [DisplayNameLocalized(typeof(ResBindingEstimatedOnRun), "BindingStartingCost")]
        public Nullable<double> BindingStartingCost { get; set; }
    }
}