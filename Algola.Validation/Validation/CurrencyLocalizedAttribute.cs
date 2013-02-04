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
    public class CurrencyLocalizedAttribute : ValidationAttribute, IClientValidatable 
    {
        private object _valueToCompare { get; set; }

        private readonly Condition _condition = Condition.EqualTo;

        public CurrencyLocalizedAttribute(Type ResourceType, string errorMessageResourceName, string valueToCompareName)
        {
            base.ErrorMessageResourceName = errorMessageResourceName;
            base.ErrorMessageResourceType = ResourceType;
            string displayName = "";

            var resman = new System.Resources.ResourceManager(ResourceType);
            
            //error message
            displayName = resman.GetString(errorMessageResourceName);

            base.ErrorMessage = string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", errorMessageResourceName)
                : displayName;

            //value To Compare
            displayName = resman.GetString(valueToCompareName);

            _valueToCompare = string.IsNullOrEmpty(displayName)
                ? string.Format("[[{0}]]", valueToCompareName)
                : displayName;

            //ignore it
            _condition = Condition.EqualTo;
        }

        public override bool IsValid(object value)
        {
            try
            {
                Regex reg = new Regex(_valueToCompare.ToString(),RegexOptions.IgnoreCase);
                return reg.IsMatch(value.ToString());                       
            }
            catch (Exception)
            {
                return false;
            }
        }

        public class ModelClientValidationCurrencyLocalizedRule : ModelClientValidationRule
        {
            public ModelClientValidationCurrencyLocalizedRule(string errorMessage, Condition condition, object propertyValue)
                : base()
            {
                this.ErrorMessage = errorMessage;
                this.ValidationType = "currency";
                this.ValidationParameters.Add("propertyvalue", propertyValue);
                this.ValidationParameters.Add("condition", (int)condition);
            }
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationCurrencyLocalizedRule(ErrorMessage, _condition, _valueToCompare);
        }
    }
}