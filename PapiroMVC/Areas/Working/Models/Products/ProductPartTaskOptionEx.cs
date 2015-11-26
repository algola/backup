using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartTaskOption_Metadata))]
    public partial class ProductPartTaskOption : ICloneable
    {
        public enum ProductPartTaskOptionType : int
        {
            ProductPartSerigraphyOption = 0,
            ProductPartHotPrintingOption = 1,
            ProductPartPrintRollOption = 2
        }

        public ProductPartTaskOptionType TypeOfProductPartTaskOption
        {
            get;
            protected set;
        }

        public virtual object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartTaskOption copyOfObject = (ProductPartTaskOption)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //ProductPart partCopy = (ProductPart)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
            ////l'oggetto partCopy sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(partCopy);

            //sulla copia del prodotto in producto assegno la copia del suo prodotto
            //Example
            //copiaProdottoInProducto.Prodotto = null;
            //copiaProdotto.ProdottoInProducto = null;
            //copiaProdottoInProducto.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInProducto.Add(copiaProdottoInProducto);
            //END COPY OF CHILD

            return copyOfObject;
        }

        public virtual void Copy(ProductPartTaskOption to)
        {
            to.CodProductPartTaskOption = null;
            to.CodProductPartTask = null;
        }


    }
}