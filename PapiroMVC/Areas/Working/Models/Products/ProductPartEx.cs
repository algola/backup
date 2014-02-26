using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPart))]
    [MetadataType(typeof(ProductPart_MetaData))]
    public partial class ProductPart
    {

        public virtual void UpdateOpenedFormat()
        {
            this.FormatOpened = this.Format;
        }

        public bool ShowDCut { get; set; }

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
            ProductPartSinglePlotter = 4,

            ProductPartRigid = 5,
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

    }
}
