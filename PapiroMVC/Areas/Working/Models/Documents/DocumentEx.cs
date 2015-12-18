using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Services;
using Novacode;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Document_MetaData))]
    public partial class Document : IPrintDocX
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
            Estimate = 0,
            Order = 1,
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

        /// <summary>
        /// rigeneration of each code in graph tree
        /// </summary>
        /// <param name="deep"></param>
        public void DocumentProductsCodeRigen(bool deep = false)
        {
            this.EstimateNumber = this.EstimateNumber == null ? (0).ToString().PadLeft(6, '0') : this.EstimateNumber.PadLeft(6, '0');

            var dstates = this.DocumentStates.OrderBy(y => y.CodDocumentState, new EmptyStringsAreLast()).ToList();
            foreach (var item in this.DocumentStates.OrderBy(y => y.CodDocumentState, new EmptyStringsAreLast()))
            {

                item.CodDocumentState = this.CodDocument + "-" + dstates.IndexOf(item).ToString().PadLeft(3, '0');
                
                item.CodDocument = this.CodDocument;
                item.TimeStampTable = DateTime.Now;
            }

            int firtCodDocumentProductToInsert = 0;
            int inserted = 0;

            try
            {
                firtCodDocumentProductToInsert = Convert.ToInt32(this.DocumentProducts.Where(y => y.CodDocumentProduct != null).Max(x => x.CodDocumentProduct).Replace(this.CodDocument + "-", ""))+1;
            }
            catch (Exception)
            {
                
            }


            #region DocumentProduct
            var ppart = this.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()).ToList();
            foreach (var item in this.DocumentProducts.OrderBy(y => y.CodDocumentProduct, new EmptyStringsAreLast()))
            {
                if (item.CodDocumentProduct == "" || item.CodDocumentProduct == null)
                    {
                        item.CodDocumentProduct = this.CodDocument + "-" + (firtCodDocumentProductToInsert + inserted++).ToString().PadLeft(3, '0');
                    }
                item.CodDocument = this.CodDocument;
                item.TimeStampTable = DateTime.Now;

                var costl = item.Costs.OrderBy(x => x.CodCost, new EmptyStringsAreLast()).ToList();
                // var costl = item.Costs.OrderBy(x => x.CodCost).ToList();
                foreach (var itemCost in item.Costs.OrderBy(x => x.CodCost, new EmptyStringsAreLast()))
                {
                    itemCost.TimeStampTable = DateTime.Now;
                    itemCost.CodDocumentProduct = item.CodDocumentProduct;

                    //compatibilità verso le versioni precedenti
                    //******************************************************
                    //se il codice finale è lungo < 3 allora è vecchio ed è un casino e lo lascio così
                    //altrimenti modifico il codice!!!

                    var s = itemCost.CodCost??"";
                    var lastCod = String.Empty;
                    // Loop through all instances of the letter a.
                    int i = 0;
                    while ((i = s.IndexOf('-', i)) != -1)
                    {
                        // Print out the substring.
                        lastCod = s.Substring(i + 1);
                        // Increment the index.
                        i++;
                    }


                    if (lastCod.Length<3 && lastCod != "")
                    {
                        itemCost.CodCost = item.CodDocumentProduct + "-" + costl.IndexOf(itemCost).ToString();                        
                    }
                    else
                    {
                        itemCost.CodCost = item.CodDocumentProduct + "-" + costl.IndexOf(itemCost).ToString().PadLeft(6, '0');                                                    
                    }
                    




                    //compatibilità verso le versioni precedenti
                    //******************************************************

                    if (deep)
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
            #endregion

            this.TimeStampTable = DateTime.Now;

        }

        protected List<DocumentState> documentStates;
        public List<DocumentState> DocumentStatesPerView
        {
            get
            {
                if (documentStates == null)
                {
                    documentStates = this.DocumentStates.OrderBy(x => x.StateNumber).ToList();
                }

                return documentStates;

            }

            set
            {
                documentStates = value;
                DocumentStates = documentStates;
            }

        }


        public virtual void MergeField(DocX doc)
        {

        }



    }
}
