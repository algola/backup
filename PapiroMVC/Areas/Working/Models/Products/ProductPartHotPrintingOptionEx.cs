using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartHotPrintingOption_Metadata))]
    public partial class ProductPartHotPrintingOption: ProductPartTaskOption
    {
        public ProductPartHotPrintingOption()
        {
            this.TypeOfProductPartTaskOption = ProductPartTaskOptionType.ProductPartHotPrintingOption;
        }


        public override object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartHotPrintingOption copyOfObject = (ProductPartHotPrintingOption)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);


            return copyOfObject;
        }

        public override void Copy(ProductPartTaskOption to)
        {
            base.Copy(to);

            ((ProductPartHotPrintingOption)to).Foil = Foil;
            ((ProductPartHotPrintingOption)to).Format = Format;
        }
    }
}