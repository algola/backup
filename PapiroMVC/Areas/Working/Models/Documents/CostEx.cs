using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(Cost_MetaData))]
    public partial class Cost : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive

        public bool IsSelected
        {
            get;
            set;
        }


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

        public virtual void Copy(Cost to)
        {
            //All properties of object
            //and pointer of sons
            to.CodCost = this.CodCost;
            to.CodDocumentProduct = this.CodDocumentProduct;
            to.CodProductPartPrintableArticle = this.CodProductPartPrintableArticle;
            to.CodProductPartTask = this.CodProductPartTask;
            to.CodProductTask = this.CodProductTask;
            to.DocumentProduct = this.DocumentProduct;
            to.ProductPartsPrintableArticle = this.ProductPartsPrintableArticle;
            to.ProductPartTask = this.ProductPartTask;
            to.ProductTask = this.ProductTask;
            to.TimeStampTable = this.TimeStampTable;

            to.Description = this.Description;
            to.Quantity = this.Quantity;
            to.UnitCost = this.UnitCost;
            to.TotalCost = this.TotalCost;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Cost copyOfObject = (Cost)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //DocumentProduct partCopy = (DocumentProduct)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
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
