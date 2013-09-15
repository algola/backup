using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartsPrintableArticle_MetaData))]
    public abstract partial class ProductPartsPrintableArticle : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive       
     
        public enum TypeOfProductPartsPrintableArticleType : int
        {
            ProductPartSheetArticle = 0,
        }

        public TypeOfProductPartsPrintableArticleType TypeOfProductPartsPrintableArticle
        {
            get;
            protected set;
        }
        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Color;            
        }

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

        public virtual void Copy(ProductPartsPrintableArticle to)
        {
            //All properties of object
            //and pointer of sons
            to.CodProductPart = this.CodProductPart;
            to.CodProductPartPrintableArticle = this.CodProductPartPrintableArticle;
            to.Color = this.Color;
            to.NameOfMaterial = this.NameOfMaterial;
            to.ProductPartPrintableArticleName = this.ProductPartPrintableArticleName;
            to.TimeStampTable = this.TimeStampTable;
            to.Weight = this.Weight;
            to.ProductPart = this.ProductPart;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartsPrintableArticle copyOfObject = (ProductPartsPrintableArticle)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //ProductPartsPrintableArticlePart partCopy = (ProductPartsPrintableArticlePart)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
            ////l'oggetto partCopy sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(partCopy);

            //sulla copia del prodotto in ProductPartsPrintableArticleo assegno la copia del suo prodotto
            //Example
            //copiaProdottoInProductPartsPrintableArticleo.Prodotto = null;
            //copiaProdotto.ProdottoInProductPartsPrintableArticleo = null;
            //copiaProdottoInProductPartsPrintableArticleo.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInProductPartsPrintableArticleo.Add(copiaProdottoInProductPartsPrintableArticleo);
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
