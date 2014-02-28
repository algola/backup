using PapiroMVC.Models.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace PapiroMVC.Validation
{
    public class ProductApiModelBinder : IModelBinder
    {

        public bool BindModel(HttpActionContext actionContext, ModelBindingContext context)
        {
            var product = (ProductApi)context.Model ?? new ProductApi();
            var result = context.ValueProvider.GetValue("prices");
            if (result != null)
                product.Prices = result.AttemptedValue.Split(',').Select(d => d.Trim()).Cast<Price>().ToList();

            context.Model = product;
            return true;
        }

        
    }
}
