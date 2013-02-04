using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Algola.Validation
{
    public static class Register
    {
        public static void Attribute(Type attributeType)
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(attributeType, typeof(AjaxValidator));
        }

        internal static void All()
        {
            //Attribute(typeof(IsAttribute));
            //Attribute(typeof(EqualToAttribute));
            //Attribute(typeof(NotEqualToAttribute));
            //Attribute(typeof(GreaterThanAttribute));
            //Attribute(typeof(LessThanAttribute));
            //Attribute(typeof(GreaterThanOrEqualToAttribute));
            //Attribute(typeof(LessThanOrEqualToAttribute));
            Attribute(typeof(RequiredIfAttribute));
            //Attribute(typeof(RequiredIfTrueAttribute));
            //Attribute(typeof(RequiredIfFalseAttribute));
            //Attribute(typeof(RequiredIfEmptyAttribute));
            //Attribute(typeof(RequiredIfNotEmptyAttribute));
            //Attribute(typeof(RequiredIfNotAttribute));
            //Attribute(typeof(RegularExpressionIfAttribute));
            //Attribute(typeof(RequiredIfRegExMatchAttribute));
            //Attribute(typeof(RequiredIfNotRegExMatchAttribute));    
        }
    }
}
