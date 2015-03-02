using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PapiroMVC.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NoValidationAttribute : ValidationAttribute, IClientValidatable
    {
        public override bool IsValid(object value)
        {
            return true;
        }

        public NoValidationAttribute()
        {

        }
        public override string FormatErrorMessage(string name)
        {
            return "The " + name + " field contains an invalid credit card number.";
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            yield return new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "novalidation"
            };
        }
    }


    public class DisplayNameLocalizedAttribute : DisplayNameAttribute
    {
        public DisplayNameLocalizedAttribute(Type resourceName, string resourceKey)
        {
            this.resourceKey = resourceKey;
            this.resourceName = resourceName;
        }

        public override string DisplayName
        {
            get
            {
                var resman = new System.Resources.ResourceManager(resourceName.FullName, resourceName.Assembly);
                string displayName = resman.GetString(resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", resourceKey)
                    : displayName;
            }
        }

        private string resourceKey { get; set; }
        private Type resourceName { get; set; }
    }

    public class TooltipAttribute : DescriptionAttribute
    {
        public TooltipAttribute(Type resourceName, string resourceKey)
        {
            this.resourceKey = resourceKey;
            this.resourceName = resourceName;
        }

        public override string Description
        {
            get
            {
                var resman = new System.Resources.ResourceManager(resourceName.FullName, resourceName.Assembly);
                string displayName = resman.GetString(resourceKey);

                return string.IsNullOrEmpty(displayName)
                    ? string.Format("[[{0}]]", resourceKey)
                    : displayName;
            }
        }

        private string resourceKey { get; set; }
        private Type resourceName { get; set; }
    }

}
