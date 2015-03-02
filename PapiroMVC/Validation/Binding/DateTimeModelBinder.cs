using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Validation
{
    public class DateTimeModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            object result = null;

            // Don't do this here!
            // It might do bindingContext.ModelState.AddModelError
            // and there is no RemoveModelError!
            // 
            // result = base.BindModel(controllerContext, bindingContext);

            string modelName = bindingContext.ModelName;
            String attemptedValue = bindingContext.ValueProvider.GetValue(modelName).AttemptedValue;

            if (attemptedValue == String.Empty)
                attemptedValue = "";
            else
            { 
            attemptedValue = attemptedValue.Replace('.',':');
            }
            try
            {
                DateTime o;
                bool success = DateTime.TryParse(attemptedValue, new CultureInfo(CultureInfo.CurrentCulture.ToString()), DateTimeStyles.None, out o);
                result = o;

            }
            catch (FormatException e)
            {
                bindingContext.ModelState.AddModelError(modelName, e);
            }

            return result;
        }
    }
}