using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Algola.Validation
{

    [AttributeUsage(AttributeTargets.Property)]
    public class MoneyAttribute : ValidationAttribute, IClientValidatable 
    {
//        private object _valueToCompare { get; set; }

        private readonly Regex _condition = new Regex(@"^\$(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$", RegexOptions.IgnoreCase);
       
        public MoneyAttribute()
        {
        }

        public override bool IsValid(object value)
        {
            try
            {
                return _condition.IsMatch(value.ToString());               
            }
            catch (Exception)
            {
                return false;
            }
        }

        public class ModelClientValidationMoneyRule : ModelClientValidationRule
        {
            public ModelClientValidationMoneyRule(string errorMessage, string regex)
                : base()
            {
                this.ErrorMessage = errorMessage;
                this.ValidationType = "money";
                this.ValidationParameters.Add("propertyvalue", regex);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationMoneyRule(ErrorMessage, @"^\$(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$");
        }
    }
}