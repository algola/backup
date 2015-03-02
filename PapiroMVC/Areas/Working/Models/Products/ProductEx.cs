using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Novacode;


namespace PapiroMVC.Models
{

    [KnownType(typeof(Product))]
    [MetadataType(typeof(Product_MetaData))]

    public abstract partial class Product : IPrintDocX
    {
        #region Proprietà aggiuntive

        //da mettere poi nelle proprietà persistenti nel database
        public String ProductNameGenerator { get; set; }

        public bool ShowDCut { get; set; }
        public Nullable<double> DCut { get; set; }

        public Nullable<double> DCut1 { get; set; }
        public Nullable<double> DCut2 { get; set; }

        /// <summary>
        /// list of all TypeOfTask in the system
        /// </summary>
        /// 
        [DataMember]
        public List<TypeOfTask> SystemTaskList { get; set; }


        protected List<ProductGraphLink> productGraphLinks;
        public List<ProductGraphLink> ProductGraphLinksPerView
        {
            get
            {
                if (productGraphLinks == null)
                {
                    productGraphLinks = this.ProductGraphLinks.ToList();
                }

                return productGraphLinks;

            }

            set
            {
                productGraphLinks = value;
                ProductGraphLinks = productGraphLinks;
            }

        }


        protected List<ProductPart> productParts;
        public List<ProductPart> ProductPartsPerView
        {
            get
            {
                if (productParts == null)
                {
                    productParts = this.ProductParts.ToList();
                }

                return productParts;

            }

            set
            {
                productParts = value;
                ProductParts = productParts;
            }

        }

        protected List<ProductTask> productTasks;
        public List<ProductTask> ProductTasksPerView
        {
            get
            {
                if (productTasks == null)
                {
                    productTasks = this.ProductTasks.OrderBy(x => x.IndexOf).ToList();
                }

                return productTasks;
            }

            set
            {
                productTasks = value;
                ProductTasks = productTasks;
            }

        }

        //initialize only possible part task like... plastificatura etc...
        public virtual List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPAOFFeDIGITALE", "PLASTIFICATURA", "FUSTELLATURA" };

            foreach (var item in codTypeOfTasks)
            {
                pt = new ProductPartTask();
                //default selection
                pt.OptionTypeOfTask = SystemTaskList.FirstOrDefault(x => x.CodTypeOfTask == item).OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item + "_NO");
                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true;
                tsksInPart.Add(pt);
            }

            return tsksInPart;
        }

        public virtual void InitProduct()
        {
            InitPageTask();
            InitProductTask();

            ProductTask prodTask;
            //show task from task name array
            for (int i = 0; i < ProductTaskName.Count(); i++)
            {
                prodTask = ProductTasks.First(x => x.CodOptionTypeOfTask == ProductTaskName[i]);
                prodTask.Hidden = false;
                prodTask.IndexOf = i;
            }
        }

        /// <summary>
        /// list of Task in product (definition of product)
        /// </summary>
        [DataMember]
        public String[] ProductTaskName { get; set; }

        /// <summary>
        /// list of Formats in product
        /// </summary>
        [DataMember]
        public ProductFormatName[] FormatsName { get; set; }

        //init for this kind of product
        public virtual void InitPageTask()
        {
            this.TsksInPage = SystemTaskList.ToList();
        }

        public virtual void InitProductTask()
        {
            var tsksInProduct = new List<ProductTask>();

            ProductTask pt;

            foreach (var item in SystemTaskList)
            {
                pt = new ProductTask();
                //default selection
                pt.OptionTypeOfTask = item.OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item.CodTypeOfTask + "_NO");
                pt.CodOptionTypeOfTask = pt.OptionTypeOfTask.CodOptionTypeOfTask;
                pt.Hidden = true;
                tsksInProduct.Add(pt);
            }

            this.ProductTasks = tsksInProduct;

        }

        [DataMember]
        public List<TypeOfTask> TsksInPage
        {
            get;
            set;
        }

        public enum ProductType : int
        {
            ProductSingleSheet = 0,
            ProductBlockSheet = 1,
            ProductBookSheet = 2,
            ProductRigid = 3,

            ProductSingleLabelRoll = 4,
            ProductSoft = 5,

            ProductEmpty = 6
        }

        [DataMember]
        public ProductType TypeOfProduct
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            var pParts = String.Empty;
            foreach (var item in this.ProductParts)
            {
                pParts += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            var pTasks = String.Empty;
            foreach (var item in this.ProductTasks)
            {
                pTasks += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            var sb = pParts + pTasks;
            sb.ToString().TrimEnd('\r', '\n');
            return sb;

        }


        public virtual void ToName()
        {

            var x = ProductNameGenerator;
            x = x.Replace("%PRODNAME", this.ProductRefName);
            ProductNameGenerator = x;

            var pParts = String.Empty;
            foreach (var item in this.ProductParts)
            {
                item.ToName();
            }

            var pTasks = String.Empty;
            foreach (var item in this.ProductTasks)
            {
                item.ToName();
            }

        }

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }


        public void CheckConsistency()
        {
            foreach (var productPart in ProductParts)
            {

                #region ProductPartTask
                //prima di tutto scorro le lavorazioni e controllo se devo resettarne alcune
                foreach (var productPartTask in productPart.ProductPartTasks.OrderBy(z => z.IndexOf))
                {

                    if (productPartTask.IfSelectedResetOtherCodItemGraph != null && !productPartTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        var codItemToReset = (productPartTask.IfSelectedResetOtherCodItemGraph ?? "").Split('+');

                        foreach (var it in codItemToReset.Where(x => x != ""))
                        {
                            var ptToReset = productPart.ProductPartTasks.SingleOrDefault(x => x.CodItemGraph == it);
                            ptToReset.CodOptionTypeOfTask = ptToReset.CodOptionTypeOfTask.Substring(0, ptToReset.CodOptionTypeOfTask.IndexOf("_")) + "_NO";
                            ptToReset.Hidden = true;                           
                            Console.WriteLine(ptToReset.CodOptionTypeOfTask);
                        }

                    }

                }

                #endregion
            }

        }
        public virtual void ProductCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;

            foreach (var item in this.WarehouseArticles)
            {
                item.TimeStampTable = DateTime.Now;
                item.CodProduct = this.CodProduct;
                item.CodWarehouseArticle = item.CodWarehouse + "P" + CodProduct;
            }

            //parti del prodotto
            var ppart = this.ProductParts.OrderBy(x => x.CodProductPart).ToList();
            foreach (var item in this.ProductParts)
            {
                item.Product = this;
                item.UpdateOpenedFormat();

                item.CodProductPart = this.CodProduct + "-" + ppart.IndexOf(item).ToString();
                item.CodProduct = this.CodProduct;
                item.TimeStampTable = DateTime.Now;

                //task della parte del prodotto
                var pptask = item.ProductPartTasks.OrderBy(y => y.CodProductPartTask).ToList();
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.ProductPart = item;
                    item2.CodProductPart = item.CodProductPart;
                    item2.TimeStampTable = DateTime.Now;
                    item2.CodProductPartTask = item.CodProductPart + "-" + pptask.IndexOf(item2).ToString().PadLeft(3, '0');
                }

                //articoli della parte del prodotto
                var pppart = item.ProductPartPrintableArticles.OrderBy(z => z.CodProductPartPrintableArticle).ToList();
                foreach (var item2 in item.ProductPartPrintableArticles)
                {
                    item2.ProductPart = item;
                    item2.CodProductPart = item.CodProductPart;
                    item2.TimeStampTable = DateTime.Now;
                    item2.CodProductPartPrintableArticle = item.CodProductPart + "-" + pppart.IndexOf(item2).ToString().PadLeft(3, '0');
                }

            }

            //task del prodotto
            var pt = this.ProductTasks.OrderBy(pp => pp.CodProductTask).ToList();
            foreach (var item in this.ProductTasks)
            {
                item.Product = this;
                item.CodProductTask = this.CodProduct + "-" + (pt.IndexOf(item).ToString()).PadLeft(3, '0');
                item.CodProduct = this.CodProduct;
                item.TimeStampTable = DateTime.Now;
            }

            if (this.ProductName == "" || this.ProductName == null)
            {
                if (!String.IsNullOrEmpty(ProductNameGenerator))
                {
                    this.ToName();

                    var x = ProductNameGenerator;
                    x = x.Replace("%PARTTASKS", "");
                    x = x.Replace("%TASKS", "");
                    ProductNameGenerator = x;

                    this.ProductName = ProductNameGenerator;
                }
                else
                {
                    this.ProductName = this.ToString();
                }
            }

            //task del prodotto
            //  var gr = this.ProductGraphLinks.OrderBy(pp => pp.CodItemGraphLink).ToList();
            foreach (var item in this.ProductGraphLinks)
            {
                item.CodProductGraph = this.CodProduct + "-" + item.CodItemGraph + "-" + item.CodItemGraphLink;
                item.CodProduct = this.CodProduct;
                item.TimeStampTable = DateTime.Now;
            }


        }

        public virtual void ProductUpdateTimeStamp()
        {
            this.TimeStampTable = DateTime.Now;

            foreach (var item in this.ProductParts)
            {
                item.TimeStampTable = DateTime.Now;
                foreach (var item2 in item.ProductPartTasks)
                {
                    item2.TimeStampTable = DateTime.Now;
                }
                foreach (var item2 in item.ProductPartPrintableArticles)
                {
                    item2.TimeStampTable = DateTime.Now;
                }

            }

            //task del prodotto
            foreach (var item in this.ProductTasks)
            {
                item.TimeStampTable = DateTime.Now;
            }
        }

        public virtual void MergeField(DocX doc)
        {
            doc.AddCustomProperty(new Novacode.CustomProperty("Product.CodProduct", this.CodProduct));
            doc.AddCustomProperty(new Novacode.CustomProperty("Product.ProductName", this.ProductName.Replace("@", Environment.NewLine)));
            doc.AddCustomProperty(new Novacode.CustomProperty("Product.ProductRefName", this.ProductRefName));
            //doc.AddCustomProperty(new Novacode.CustomProperty("Product.Format", "ciao"));
        }

    }
}
