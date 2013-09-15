using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(DocumentProduct_MetaData))]
    public partial class DocumentProduct : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive

 
        protected List<Cost> costProductParts;
        public List<Cost> CostsPerView
        {
            get
            {
                if (costProductParts == null)
                { 
                    costProductParts = this.Costs.ToList();
                }

                return costProductParts;

            }

            set 
            {
                costProductParts = value;
                Costs = costProductParts;
            }

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

        public virtual void Copy(DocumentProduct to)
        {
            //All properties of object
            //and pointer of sons
            to.CodDocument = this.CodDocument;
            to.CodDocumentProduct = this.CodDocumentProduct;
            to.CodProduct = this.CodProduct;
            to.Costs = this.Costs;
            to.Document = this.Document;
            to.Product = this.Product;
            to.ProductName = this.ProductName;
            to.Quantity = this.Quantity;
            to.TimeStampTable = this.TimeStampTable;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            DocumentProduct copyOfObject = (DocumentProduct)Activator.CreateInstance(kindOfObject);
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
