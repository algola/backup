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
    [MetadataType(typeof(ProductPartHotPrinting_Metadata))]
    public partial class ProductPartHotPrinting : ProductPartTask, IDataErrorInfo, ICloneable
    {

        public ProductPartHotPrinting()
        {
            TypeOfProductPartTask = ProductPartTasksType.ProductPartHotPrinting;
        }
      
        public bool IsSelected
        {
            get;
            set;
        }



        public override object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartHotPrinting copyOfObject = (ProductPartHotPrinting)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);


            return copyOfObject;
        }

                
        public override void Copy(ProductPartTask to)
        {
            base.Copy(to);
        }



    }

}
