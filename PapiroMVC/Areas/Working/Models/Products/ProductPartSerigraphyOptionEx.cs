using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartSerigraphyOption_Metadata))]

    public partial class ProductPartSerigraphyOption : ProductPartTaskOption
    {

        public ProductPartSerigraphyOption()
        {

            TypeOfProductPartTaskOption = ProductPartTaskOptionType.ProductPartSerigraphyOption;
        }

        public override object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartSerigraphyOption copyOfObject = (ProductPartSerigraphyOption)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);


            return copyOfObject;
        }

        public override void Copy(ProductPartTaskOption to)
        {
            base.Copy(to);

            ((ProductPartSerigraphyOption)to).TypeOfTaskSerigraphy = TypeOfTaskSerigraphy;
            ((ProductPartSerigraphyOption)to).InkSerigraphy = InkSerigraphy;
            ((ProductPartSerigraphyOption)to).Overlay = Overlay;
        }


    }
}