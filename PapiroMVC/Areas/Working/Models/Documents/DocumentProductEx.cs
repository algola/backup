using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(DocumentProduct_MetaData))]
    public partial class DocumentProduct : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public void UpdateCost()
        {
            double total = 0;
            foreach (var item in Costs)
            {
                total += !(item.ForceZero ?? false) ? Convert.ToDouble(item.TotalCost, Thread.CurrentThread.CurrentUICulture) : 0;
            }
            TotalAmount = total.ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);
            UnitPrice = ((total / Quantity ?? 0).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture));
        }

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

        public void InitCost()
        {
            Cost cost;

            this.CodProduct = Product.CodProduct;
            this.ProductName = Product.ProductName;

            //for each ProductTask / ProductPartTask / ProductPartsPrintableArticle in Product
            //adding new cost

            //ProductTask
            //only code 
            //                            foreach (var productTask in product.ProductTasks.Where(x => !x.CodOptionTypeOfTask.Contains("_NO")))
            foreach (var productTask in Product.ProductTasks.Where(x => !x.CodOptionTypeOfTask.Contains("_NO")))
            {
                cost = new Cost();
                cost.DocumentProduct = this;
                cost.CodDocumentProduct = this.CodDocumentProduct;

                cost.CodProductTask = productTask.CodProductTask;
                cost.Description = "***Lavorazione del prodotto";

                if (productTask.CodOptionTypeOfTask.Contains("_NO"))
                {
                    cost.Hidden = true;
                    cost.ForceZero = true;
                }
                this.Costs.Add(cost);
            }

            //ProductPartTask & ProductPartsPrintableArticle
            foreach (var productPart in Product.ProductParts)
            {
                #region ProductPartPrintableArticle
                foreach (var productPartsPrintableArticle in productPart.ProductPartPrintableArticles)
                {
                    cost = new Cost();
                    cost.DocumentProduct = this;
                    cost.CodDocumentProduct = this.CodDocumentProduct;

                    cost.CodProductPartPrintableArticle = productPartsPrintableArticle.CodProductPartPrintableArticle;
                    cost.ProductPartsPrintableArticle = productPartsPrintableArticle;
                    cost.ProductPartsPrintableArticle.ProductPart = productPart;

                    cost.Description = productPartsPrintableArticle.ToString();
                    cost.Description += (productPart.ProductPartName ?? "") == "" ? "" : " (" + productPart.ProductPartName + ")";

                    this.Costs.Add(cost);
                }
                #endregion

                #region ProductPartTask
                foreach (var productPartTask in productPart.ProductPartTasks)
                {
                    cost = new Cost();
                    cost.DocumentProduct = this;
                    cost.CodDocumentProduct = this.CodDocumentProduct;

                    cost.CodProductPartTask = productPartTask.CodProductPartTask;
                    cost.ProductPartTask = productPartTask;
                    cost.ProductPartTask.ProductPart = productPart;

                    cost.Description = productPartTask.ToString();
                    cost.Description += (productPart.ProductPartName ?? "") == "" ? "" : " (" + productPart.ProductPartName + ")";
                    if (cost.Description != "")
                    {
                        cost.Description = char.ToUpper(cost.Description[0]) + cost.Description.Substring(1);
                    }

                    if (productPartTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        cost.Hidden = true;
                        cost.ForceZero = true;
                    }

                    this.Costs.Add(cost);

                    #region impianti
                    cost = new Cost();
                    cost.DocumentProduct = this;
                    cost.CodDocumentProduct = this.CodDocumentProduct;

                    cost.CodProductPartImplantTask = productPartTask.CodProductPartTask;
                    cost.ProductPartImplantTask = productPartTask;
                    cost.ProductPartImplantTask.ProductPart = productPart;

                    String str = productPartTask.ToString();
                    str = str != "" ? str.Substring(0, str.IndexOf(" ")) : "";

                    cost.Description = "impianti " + str;
                    cost.Description += (productPart.ProductPartName ?? "") == "" ? "" : " (" + productPart.ProductPartName + ")";
                    cost.Description = char.ToUpper(cost.Description[0]) + cost.Description.Substring(1);

                    if (productPartTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        cost.Hidden = true;
                        cost.ForceZero = true;
                    }

                    if (productPartTask.ImplantHidden == false)
                    {
                        cost.Hidden = false;
                        cost.ForceZero = false;
                    }

                    this.Costs.Add(cost);

                    #endregion
                }
                #endregion
            }


        }
    }
}
