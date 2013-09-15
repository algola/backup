using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(Document_MetaData))]
    public partial class Document : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive


        protected List<DocumentProduct> productParts;
        public List<DocumentProduct> DocumentProductsPerView
        {
            get
            {
                if (productParts == null)
                { 
                    productParts = this.DocumentProducts.ToList();
                }

                return productParts;

            }

            set 
            {
                productParts = value;
                DocumentProducts = productParts;
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

        public virtual void Copy(Document to)
        {
            //All properties of object
            //and pointer of sons
            to.CodCustomer = this.CodCustomer;
            to.CodDocument = this.CodDocument;
            to.Customer = this.Customer;
            to.CustomerSupplier = this.CustomerSupplier;
            to.DateDocument = this.DateDocument;
            to.DocumentName = this.DocumentName;
            to.DocumentProducts = this.DocumentProducts;
            to.Notes = this.Notes;
            to.Number = this.Number;
            to.TimeStampTable = this.TimeStampTable;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Document copyOfObject = (Document)Activator.CreateInstance(kindOfObject);
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
