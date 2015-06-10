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

    public partial class ProductPartSerigraphy : ProductPartTask, IDataErrorInfo, ICloneable
    {

        public ProductPartSerigraphy()
        {
            TypeOfProductPartTask = ProductPartTasksType.ProductPartSerigraphy;
            this.OptionsProductPartSerigraphy = new List<OptionProductPartSerigraphy>();        
        }
      
        public bool IsSelected
        {
            get;
            set;
        }

        public virtual List<OptionProductPartSerigraphy> OptionsProductPartSerigraphy
        { 
            get; 
            set; 
        }

    }

    public partial class OptionProductPartSerigraphy
    {

        public string TypeOfTaskSerigraphy
        { 
            get; 
            set; 
        }

        public string InkSerigraphy
        {
            get;
            set;
        }

    }

}
