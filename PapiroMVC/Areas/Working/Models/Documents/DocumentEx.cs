using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(Document_MetaData))]
    public partial class Document
    {
        public NewProductCommand NewProductCommand { get; set; }

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

        public enum DocumentType : int
        {
            Estimate = 0
        }

        public DocumentType TypeOfDocument
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

    }
}
