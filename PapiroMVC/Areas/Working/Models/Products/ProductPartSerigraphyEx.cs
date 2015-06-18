using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Views.Shared.App_LocalResources;
using System.Reflection;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartSerigraphy_Metadata))]
    public partial class ProductPartSerigraphy : ProductPartTask, IDataErrorInfo, ICloneable
    {

        public ProductPartSerigraphy()
        {
            TypeOfProductPartTask = ProductPartTasksType.ProductPartSerigraphy;
        }
      
        public bool IsSelected
        {
            get;
            set;
        }

    }

}
