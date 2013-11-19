using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPart_MetaData))]
    public partial class ProductPart : ICloneable, IDeleteRelated
    {
        public virtual void UpdateOpenedFormat()
        {
            this.FormatOpened = this.Format;
        }

        protected List<ProductPartsPrintableArticle> productPartPrintableArticles;
        public List<ProductPartsPrintableArticle> ProductPartsPrintableArticlePerView
        {
            get
            {
                if (productPartPrintableArticles == null)
                { 
                    productPartPrintableArticles = this.ProductPartPrintableArticles.ToList();
                }
                return productPartPrintableArticles;
            }

            set 
            {
                productPartPrintableArticles = value;
                ProductPartPrintableArticles = productPartPrintableArticles;
            }

        }

        protected List<ProductPartTask> productPartTasks;
        public List<ProductPartTask> ProductPartTasksPerView
        {
            get
            {
                if (productPartTasks == null)
                {
                    productPartTasks = this.ProductPartTasks.ToList();
                }

                return productPartTasks;

            }

            set
            {
                productPartTasks = value;
                ProductPartTasks = productPartTasks;
            }

        }
          
        #region Proprietà aggiuntive       
        public enum ProductPartType : int
        {
            ProductPartSingleSheet = 0,
            ProductPartCoverSheet = 1,
            ProductPartBookSheet = 2,
            ProductPartBlockSheet = 3,
            ProductPartSinglePlotter = 4
        }

        public ProductPartType TypeOfProductPart
        {
            get;
            protected set;
        }

        /// <summary>
        /// Formato peronalizzato, usato quando l'utente non trova il formato giusto nel DownBox
        /// </summary>
        protected String formatPersonalized;
        public String FormatPersonalized 
        {
            get 
            {
                return formatPersonalized;
            }
            set
            {
                formatPersonalized = value;

                if (formatPersonalized != null && formatPersonalized != String.Empty)
                {
                    this.Format = formatPersonalized;
                }
            }
        }

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {

            var ptArt = String.Empty;
            foreach (var item in ProductPartPrintableArticles)
            {
                ptArt += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            var pTasks = String.Empty;
            foreach (var item in this.ProductPartTasks)
            {
                pTasks += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            return ptArt + pTasks;
        }

        #region Handle copy for modify

        public virtual void Copy(ProductPart to)
        {
            //All properties of object
            //and pointer of sons

            to.CodProduct = this.CodProduct;
            to.TimeStampTable = this.TimeStampTable;
            to.CodProductPart = this.CodProductPart;
            to.Format = this.Format;
            to.PrintingType = this.PrintingType;
            to.Product = this.Product;
            to.ProductPartName = this.ProductPartName;
            to.ProductPartPrintableArticles = this.ProductPartPrintableArticles;
            to.ProductPartTasks = this.ProductPartTasks;
            to.ServicesNumber = this.ServicesNumber;
            to.SubjectNumber = this.SubjectNumber;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPart copyOfObject = (ProductPart)Activator.CreateInstance(kindOfObject);
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
