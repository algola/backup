using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [Serializable]
    [MetadataType(typeof(DocumentProduct_MetaData))]
    public partial class DocumentProduct : ICloneable
    {

        public string MqDescription { get; set; }
        public string FgDescription { get; set; }
        public string MlDescription { get; set; }
        public string NrDescription { get; set; }
        public string UpDescription { get; set; }
        public string UpDescription1000 { get; set; }
        public string AmountDescription { get; set; }
        public string QtyDescription { get; set; }

        string _documentProductNameGenerator = "";
        public String DocumentProductNameGenerator
        {
            get
            {
                if (_documentProductNameGenerator == "")
                {
                    _documentProductNameGenerator = QtyDescription + ": %QUANTITY - " +
                        " %UNITPRICE " +
                      "";//  AmountDescription + " %TOTALPRICE@@%SUPPCOST@";
                }

                return _documentProductNameGenerator;
            }
            set
            {
                _documentProductNameGenerator = value;
            }
        }

        public virtual void ToName()
        {
            var x = DocumentProductNameGenerator;
            x = x.Replace("%QUANTITY", Quantity.ToString());

            if (Math.Truncate(Convert.ToDouble(UnitPrice) * 100)==0)
            {
                x = x.Replace("%UNITPRICE", UpDescription1000 + " " + (Convert.ToDouble(UnitPrice) * 1000).ToString());
            }
            else
            {
                x = x.Replace("%UNITPRICE", UpDescription + " " + UnitPrice);
            }
            x = x.Replace("%TOTALPRICE", TotalAmount);

            foreach (var c in Costs)
            {
                if ((c.TypeOfCalcolous??0) == 1 && c.CostDetails != null && c.CostDetails.Count > 0)
                {
                    var um = String.Empty;
                    switch (c.CostDetails.FirstOrDefault().TypeOfQuantity)
                    {
                        //RunTypeOfQuantity = 0,
                        case 0:
                            um = FgDescription;
                            break;
                        case 1:
                            um = MqDescription;
                            break;
                        case 4:
                            um = MlDescription;
                            break;
                        case 2:
                        case 5:
                        default:
                            um = NrDescription;
                            break;

                    }

                    var unitCost = (Convert.ToDouble(c.TotalCost) / (c.Quantity??1)).ToString("#,0.0000", Thread.CurrentThread.CurrentUICulture);


                    x = x.Replace("%SUPPCOST", c.Description + " " + um + ": " + c.Quantity + " " + UpDescription + ": " + c.UnitCost + " " + AmountDescription + ": " + c.TotalCost);
                    x += "@%SUPPCOST";
                }
            }

            x = x.Replace("@%SUPPCOST", "");
            DocumentProductNameGenerator = x;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            DocumentProduct copyOfObject = (DocumentProduct)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(DocumentProduct to)
        {

            to.TimeStampTable = this.TimeStampTable;
            to.CodDocumentProduct = this.CodDocumentProduct;
            to.CodDocument = this.CodDocument;
            to.ProductName = this.ProductName;
            to.CodProduct = this.CodProduct;
            to.Quantity = this.Quantity;
            to.UnitPrice = this.UnitPrice;
            to.TotalAmount = this.TotalAmount;

            to.Document = this.Document;
            to.Product = this.Product;

            List<CostDetail> cds = new List<CostDetail>();
            foreach (var cost in Costs)
            {
                var c = (Cost)cost.Clone();
                to.Costs.Add(c);

                foreach (var cd in c.CostDetails)
                {
                    cds.Add(cd);
                }
            }


            //foreach (var cd in cds)
            //{
            //    if (cd.CodComputedBy != "" && cd.CodComputedBy != null)
            //    {
            //        var c1 = cds.FirstOrDefault(x => x.CodCostDetail == cd.CodComputedBy);
            //        if (c1.Computes == null)
            //            c1.Computes = new HashSet<CostDetail>();
            //        c1.Computes.Add(cd);
            //        cd.ComputedBy=c1;
            //    }
            //}

            //CostDetail has two propery-> computed by and computes
            //         virtual ICollection<Cost> Costs 

        }

        public void UpdateCost()
        {
            double total = 0;
            foreach (var item in Costs)
            {
                if (item.TypeOfCalcolous == null || item.TypeOfCalcolous == 0)
                {
                    total += !(item.ForceZero ?? false) ? Convert.ToDouble(item.GranTotalCost, Thread.CurrentThread.CurrentUICulture) : 0;
              //      total = Math.Round(total / 100) * 100;
                }
            }
            UnitPrice = ((total / Quantity ?? 0).ToString("#,0.00000", Thread.CurrentThread.CurrentUICulture));
            TotalAmount = (Convert.ToDouble(UnitPrice,Thread.CurrentThread.CurrentCulture) * (Quantity??0)).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);
        
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

        //#region Error Handle

        //private static readonly string[] proprietaDaValidare =
        //       {
        //           //Specify validation property
        //               ""
        //       };

        //public string Error
        //{
        //    get
        //    {
        //        return null;
        //    }
        //}

        //public virtual string this[string proprieta]
        //{
        //    get
        //    {
        //        string result = null;
        //        return result;
        //    }
        //}

        ////Check validation of entity
        //public virtual bool IsValid
        //{
        //    get
        //    {
        //        bool ret = true;
        //        foreach (string prop in proprietaDaValidare)
        //        {
        //            if (this[prop] != null)
        //                ret = false;
        //        }
        //        return ret;
        //    }
        //}

        //#endregion

        //#region Handle copy for modify

        //public virtual void Copy(DocumentProduct to)
        //{
        //    //All properties of object
        //    //and pointer of sons
        //    to.CodDocument = this.CodDocument;
        //    to.CodDocumentProduct = this.CodDocumentProduct;
        //    to.CodProduct = this.CodProduct;
        //    to.Costs = this.Costs;
        //    to.Document = this.Document;
        //    to.Product = this.Product;
        //    to.ProductName = this.ProductName;
        //    to.Quantity = this.Quantity;
        //    to.TimeStampTable = this.TimeStampTable;

        //}

        //public object Clone()
        //{
        //    //creo una copia dell'oggetto da utilizzare per le modifiche
        //    var kindOfObject = this.GetType();

        //    //istanzio una copia che sarà gestita dall'invio
        //    DocumentProduct copyOfObject = (DocumentProduct)Activator.CreateInstance(kindOfObject);
        //    //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
        //    this.Copy(copyOfObject);

        //    //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
        //    //Example
        //    //DocumentProduct partCopy = (DocumentProduct)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
        //    ////l'oggetto partCopy sarà una copia del contenuto dell'oggetto originale
        //    //this.Prodotto.Copia(partCopy);

        //    //sulla copia del prodotto in producto assegno la copia del suo prodotto
        //    //Example
        //    //copiaProdottoInProducto.Prodotto = null;
        //    //copiaProdotto.ProdottoInProducto = null;
        //    //copiaProdottoInProducto.Prodotto = copiaProdotto;
        //    //copiaProdotto.ProdottoInProducto.Add(copiaProdottoInProducto);
        //    //END COPY OF CHILD

        //    return copyOfObject;
        //}

        //public void ChildsNull()
        //{
        //    //Set all chied to null 

        //    //Example
        //    //this.Prodotto = null;
        //}

        //#endregion

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
                cost.CodItemGraph = productTask.CodItemGraph;

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
                    cost.CodItemGraph = productPartTask.CodItemGraph;

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
                    //cost
                    cost.TypeOfCalcolous = 1;

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

                    cost.Hidden = productPartTask.ImplantHidden;
                    cost.ForceZero = productPartTask.ImplantHidden;

                    this.Costs.Add(cost);

                    #endregion
                }
                #endregion
            }


        }
    }
}
