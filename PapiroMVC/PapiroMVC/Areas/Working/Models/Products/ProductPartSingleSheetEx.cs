using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartSingleSheet_MetaData))]
    public partial class ProductPartSingleSheet : ProductPart, ICloneable, IDeleteRelated
    {

        public override string ToString()
        {

            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);

            String s = String.Empty;

            if ((FormatOpened ?? Format) == Format)
            {
                s = (string)t.GetProperty("FormatOnly").GetValue(null, null) + " " + FormatOpened;
            }
            else
            {
                s = (string)t.GetProperty("FormatOpened").GetValue(null, null) + " " + FormatOpened + " " +
                   (string)t.GetProperty("Format").GetValue(null, null) + " " + Format;
            }

            return s + " " +
                base.ToString();
        }

        public ProductPartSingleSheet()
        {
            TypeOfProductPart = ProductPart.ProductPartType.ProductPartSingleSheet;
        }

        #region Proprietà aggiuntive
        #endregion

        #region Handle copy for modify

        public virtual void Copy(ProductPartSingleSheet to)
        {
            //All properties of object
            //and pointer of sons

            to.RawCut = this.RawCut;
            to.SubjectNumber = this.SubjectNumber;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartSingleSheet copyOfObject = (ProductPartSingleSheet)Activator.CreateInstance(kindOfObject);
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

        public void ChildsNull()
        {
            //Set all chied to null 

            //Example
            //this.Prodotto = null;
        }

        #endregion
    }
}
