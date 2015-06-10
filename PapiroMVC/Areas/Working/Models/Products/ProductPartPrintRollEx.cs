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

    [MetadataType(typeof(ProductPartPrintRoll_Metadata))]
    public partial class ProductPartPrintRoll : ProductPartTask, IDataErrorInfo, ICloneable
    {

        public ProductPartPrintRoll()
        {
            TypeOfProductPartTask = ProductPartTasksType.ProductPartPrintRoll;
        }

        public bool IsSelected
        {
            get;
            set;
        }
        private bool _retro;
        public bool Retro
        {
            get
            {
                return _retro;
            }
            set
            {
                _retro = value;
            }
        }

        private bool _vernice;
        public bool Vernice 
        {
            get
            {
                return _vernice;
            }
            set
            {
                _vernice = value;
            }
        }


    }


}
