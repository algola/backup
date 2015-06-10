using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;

namespace PapiroMVC.Validation
{

    public class ProductPartTaskModelBinder : DefaultModelBinder
    {

        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeOfProductPartTask");

            var strType = "PapiroMVC.Models." + (string)typeValue.ConvertTo(typeof(string));
            
            var type = Type.GetType(
                strType,
                true
            );
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }


        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
                var ret= base.BindModel(controllerContext, bindingContext);


                var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeOfProductPartTask");

                if (typeValue.AttemptedValue.ToString() == "ProductPartPrintRoll")
                {
                    ((ProductPartPrintRoll)ret).CodOptionTypeOfTask = ((ProductPartPrintRoll)ret).CodOptionTypeOfTask.Replace("RETRO", "");
                    ((ProductPartPrintRoll)ret).CodOptionTypeOfTask = ((ProductPartPrintRoll)ret).CodOptionTypeOfTask.Replace("VERNICE", "");

                    if (((ProductPartPrintRoll)ret).Retro)
                    {
                        ((ProductPartPrintRoll)ret).CodOptionTypeOfTask = ((ProductPartPrintRoll)ret).CodOptionTypeOfTask + "RETRO";
                    }
                    if (((ProductPartPrintRoll)ret).Vernice)
                    {
                        ((ProductPartPrintRoll)ret).CodOptionTypeOfTask = ((ProductPartPrintRoll)ret).CodOptionTypeOfTask + "VERNICE";
                    }
                    
                }
                return ret;
        }

    }
}