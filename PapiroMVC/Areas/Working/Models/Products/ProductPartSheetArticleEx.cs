using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartSheetArticle))]
    [MetadataType(typeof(ProductPartSheetArticleEx_MetaData))]
    public partial class ProductPartSheetArticle : ProductPartsPrintableArticle, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public override bool IsInList(IQueryable<Article> arts)
        {
            var sel = arts.OfType<Printable>();
            var cont = (sel.Where(c => c.NameOfMaterial == this.NameOfMaterial &&
                c.TypeOfMaterial == this.TypeOfMaterial &&
                c.Weight == this.Weight &&
                c.Color == this.Color).Count());

            return (cont > 0);

        }


        public ProductPartSheetArticle()
        {
            TypeOfProductPartsPrintableArticle = TypeOfProductPartsPrintableArticleType.ProductPartSheetArticle;
        }


        public override string ToString()
        {
            return base.ToString();
        }

        #region Proprietà aggiuntive

        #endregion


        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""
               };

        public string Error
        {
            get
            {
                return null;
            }
        }

        public virtual string this[string proprieta]
        {
            get
            {
                string result = null;
                return result;
            }
        }

        //Check validation of entity
        public virtual bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret;
            }
        }

        #endregion

        #region Handle copy for modify

        public virtual void Copy(ProductPartSheetArticle to)
        {
            //All properties of object
            //and pointer of sons


        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartSheetArticle copyOfObject = (ProductPartSheetArticle)Activator.CreateInstance(kindOfObject);
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
