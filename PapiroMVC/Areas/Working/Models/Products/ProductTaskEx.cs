using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductTask))]
    [MetadataType(typeof(ProductTask_MetaData))]
    public partial class ProductTask 
    {

        public bool IsSelected
        {
            get;
            set;
        }


        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);
            return (string)t.GetProperty("Cod" + this.CodOptionTypeOfTask).GetValue(null, null);
        }

    }
}
