using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{    
    [MetadataType(typeof(Product_MetaData))]
    public abstract partial class Product : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Proprietà aggiuntive

        //list of all TypeOfTask in the system
        public IQueryable<TypeOfTask> SystemTaskList { get; set; }

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
                    productTasks = this.ProductTasks.OrderBy(x=>x.IndexOf).ToList();
                }

                return productTasks;
            }

            set
            {
                productTasks = value;
                ProductTasks = productTasks;
            }

        }

        //initialize only posiible part task like... plastificatura etc...
        public virtual List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPA", "PLASTIFICATURA" };

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
        public String[] ProductTaskName { get; set; }

        /// <summary>
        /// list of Formats in product
        /// </summary>
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
            ProductRigid = 3
        }

        public ProductType TypeOfProduct
        {
            get;
            protected set;
        }


        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);

            var pParts = String.Empty;
            foreach (var item in this.ProductParts)
            {
                pParts += item.ToString() == String.Empty ? "" : item.ToString() + "\n";
            }

            var pTasks = String.Empty;
            foreach (var item in this.ProductTasks)
            {
                pTasks += item.ToString()==String.Empty?"":item.ToString() + "\n";
            }
            
            return pParts + pTasks;

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

        public virtual void Copy(Product to)
        {
            //All properties of object
            //and pointer of sons
            to.CodMenuProduct = this.CodMenuProduct;
            to.CodProduct = this.CodProduct;
            to.TimeStampTable = this.TimeStampTable;
            to.ProductParts = this.ProductParts;
            to.ProductTasks = this.ProductTasks;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Product copyOfObject = (Product)Activator.CreateInstance(kindOfObject);
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
