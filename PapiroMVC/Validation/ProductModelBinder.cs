using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;

namespace PapiroMVC.Validation
{

    public class ProductModelBinder : DefaultModelBinder
    {
        //MVC
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            var typeValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".TypeOfProduct");

            var strType = "PapiroMVC.Models." + (string)typeValue.ConvertTo(typeof(string));
            
            var type = Type.GetType(
                strType,
                true
            );
            var model = Activator.CreateInstance(type);
            bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, type);
            return model;
        }

        }
}