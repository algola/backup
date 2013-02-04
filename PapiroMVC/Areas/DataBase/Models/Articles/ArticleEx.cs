using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [MetadataType(typeof(Article_MetaData))]
    abstract partial class Article : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive
        public enum ArticleType : int
        {
            SheetPrintableArticle = 0,
            RollPrintableArticle = 1,
            RigidPrintableArticle = 2,
            ObjectPrintableArticle = 3
        }

        public ArticleType TypeOfArticle
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

        public virtual void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons

            to.CodArticle = this.CodArticle;
            to.ArticleName = this.ArticleName;
            to.TimeStampTable = this.TimeStampTable;

            //must: modify with child
            to.CodSupplierBuy = this.CodSupplierBuy;
            to.CodSupplierMaker = this.CodSupplierMaker;
            to.UnitOfMeasure = this.UnitOfMeasure;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Article copyOfObject = (Article)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example

            foreach (var item in this.ArticleCosts)
            {
                copyOfObject.ArticleCosts.Add((ArticleCost)item.Clone());                   
            }
            
            //Prodotto copiaProdotto = (Prodotto)Activator.CreateInstance(copiaProdottoInDocumento.Prodotto.GetType());
            ////l'oggetto copisa sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(copiaProdotto);

            //sulla copia del prodotto in documento assegno la copia del suo prodotto
            //Example
            //copiaProdottoInDocumento.Prodotto = null;
            //copiaProdotto.ProdottoInDocumento = null;
            //copiaProdottoInDocumento.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInDocumento.Add(copiaProdottoInDocumento);
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
