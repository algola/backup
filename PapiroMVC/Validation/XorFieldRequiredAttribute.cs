using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PapiroMVC.Validation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class XorFieldRequired : ValidationAttribute 
    {
        private const string _defaultErrorMessage = "'{0}' or '{1}' are required.";
        private readonly object _typeId = new object();

        private List<string> fields;

        public XorFieldRequired(string[] field, Type ResourceType, string errorMessageResourceName)
        {
            base.ErrorMessageResourceName = errorMessageResourceName;
            base.ErrorMessageResourceType = ResourceType;

            fields = new List<string>();
            foreach (var item in field)
            {
                fields.Add(item);
            }
        }
 
        public List<string> Fields 
        {
            get
            {
                return fields;
            }
            private set
            {
                fields = value;
            }
        }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }
 
        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString, Fields);
        }
 
        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            int count = 0;

            foreach (var item in Fields)
            {
                if (String.IsNullOrEmpty((properties.Find(item, true).GetValue(value) as string))) count++;                
            }

            return (count==1)?true:false;

        }
    }
}