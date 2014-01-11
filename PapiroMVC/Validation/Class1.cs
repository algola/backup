using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Validation
{

    public class CustomModelMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata CreateMetadata(
                                 IEnumerable<Attribute> attributes,
                                 Type containerType,
                                 Func<object> modelAccessor,
                                 Type modelType,
                                 string propertyName)
        {

            var data = base.CreateMetadata(
                                 attributes,
                                 containerType,
                                 modelAccessor,
                                 modelType,
                                 propertyName);

            var tooltip = attributes
              .SingleOrDefault(a => typeof(TooltipAttribute) == a.GetType());
            if (tooltip != null) data.AdditionalValues
                        .Add("Tooltip", ((TooltipAttribute)tooltip).Description);

            return data;
        }
    }
}
