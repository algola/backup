using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartPrintRollOption_Metadata))]

    public partial class ProductPartPrintRollOption : ProductPartTaskOption
    {

        public ProductPartPrintRollOption()
        {

            TypeOfProductPartTaskOption = ProductPartTaskOptionType.ProductPartPrintRollOption;
        }

        public override object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartPrintRollOption copyOfObject = (ProductPartPrintRollOption)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);


            return copyOfObject;
        }

        public override void Copy(ProductPartTaskOption to)
        {
            base.Copy(to);

            ((ProductPartPrintRollOption)to).TypeOfTaskPrint = TypeOfTaskPrint;
            ((ProductPartPrintRollOption)to).Ink = Ink;
            ((ProductPartPrintRollOption)to).Overlay = Overlay;
        }


    }
}