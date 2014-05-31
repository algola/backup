using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Services;

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

        public void DocumentProductsCodeRigen(bool deeep = false)
        {
            this.EstimateNumber = this.EstimateNumber == null ? (0).ToString().PadLeft(6, '0') : this.EstimateNumber.PadLeft(6, '0');

            //prodotti in documento
            var ppart = this.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()).ToList();
            foreach (var item in this.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()))
            {
                item.CodDocumentProduct = this.CodDocument + "-" + ppart.IndexOf(item).ToString();
                item.CodDocument = this.CodDocument;
                item.TimeStampTable = DateTime.Now;

                var costl = item.Costs.OrderBy(x => x.CodCost).ToList();
                foreach (var itemCost in item.Costs.OrderBy(x => x.CodCost))
                {
                    itemCost.TimeStampTable = DateTime.Now;
                    itemCost.CodDocumentProduct = item.CodDocumentProduct;
                    itemCost.CodCost = item.CodDocumentProduct + "-" + costl.IndexOf(itemCost).ToString();

                    if (deeep)
                    {
                        foreach (var itemCostDetail in itemCost.CostDetails)
                        {
                            itemCostDetail.CodCost = itemCost.CodCost;
                            itemCostDetail.CodCostDetail = itemCost.CodCost;
                            itemCostDetail.CostDetailCostCodeRigen();
                        }                        
                    }

                }
            }

            this.TimeStampTable = DateTime.Now;

        }

    }
}
