
namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Orders_MetaData))]
    public partial class Orders
    {
        public override string ToString()
        {
            return "";
        }

    }
}
