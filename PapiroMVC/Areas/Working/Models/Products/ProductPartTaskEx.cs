using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Views.Shared.App_LocalResources;
using System.Reflection;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [KnownType(typeof(ProductPartTask))]
    [MetadataType(typeof(ProductPartTask_MetaData))]
    public partial class ProductPartTask : IDataErrorInfo, ICloneable, IDeleteRelated
    {

        #region Proprietà aggiuntive
        public enum ProductPartTasksType : int
        {
            ProductPartTask = 0,
            ProductPartSerigraphy = 1,
            ProductPartPrintRoll = 2,
            ProductPartHotPrinting = 3
        }

        public ProductPartTasksType TypeOfProductPartTask
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

        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);

            if (this.CodOptionTypeOfTask.Contains("STAMPANEW"))
            {
                if (CodOptionTypeOfTask.Contains("_NO"))
                {
                    return (string)t.GetProperty("Cod" + this.CodOptionTypeOfTask).GetValue(null, null);
                }
                else
                {
                    PrintingColor colors = TaskExecutor.GetColorFR(CodOptionTypeOfTask);
                    if (colors.cToPrintT == 1)
                    {
                        return (string)t.GetProperty("CodSTAMPANEW1").GetValue(null, null);
                    }
                    else
                    {
                        return (string)t.GetProperty("CodSTAMPANEWS").GetValue(null, null).ToString().Replace("XXX", colors.cToPrintF.ToString() + "+" + colors.cToPrintR.ToString());
                    }
                }
            }
            else
                return (string)t.GetProperty("Cod" + this.CodOptionTypeOfTask).GetValue(null, null);



        }

        public virtual string ToStringInfo()
        {
            return ToString();
        }

        public virtual string ImplantToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);
            string ret = String.Empty;

            try
            {
                var extract = this.CodOptionTypeOfTask.Substring(0, this.CodOptionTypeOfTask.IndexOf('_')) + "_Implant";
                ret = (string)t.GetProperty("Cod" + extract).GetValue(null, null);
            }
            catch
            {

            }

            return ret;
        }

        protected List<ProductPartTaskOption> productPartTaskOptions;
        public List<ProductPartTaskOption> ProductPartTaskOptionsPerView
        {
            get
            {
                if (productPartTaskOptions == null)
                {
                    productPartTaskOptions = this.ProductPartTaskOptions.ToList();
                }

                return productPartTaskOptions;

            }

            set
            {
                productPartTaskOptions = value;
                ProductPartTaskOptions = productPartTaskOptions;
            }

        }

        public virtual void ToName()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);

            if (ProductPart != null)
            {

                var x = ProductPart.Product.ProductNameGenerator;

                if (x.Contains("%PRINTPARTTASK") && this.CodOptionTypeOfTask.Contains("STAMPA"))
                {
                    x = x.Replace("%PRINTPARTTASK", this.ToString());
                }
                else
                {
                    x = x.Replace("%PARTTASKS", this.ToString() + " %PARTTASKS");
                }

                ProductPart.Product.ProductNameGenerator = x;
            }

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

        public virtual void Copy(ProductPartTask to)
        {
            //All properties of object
            //and pointer of sons
            to.IndexOf = this.IndexOf;
            to.Hidden = this.Hidden;
            to.TimeStampTable = this.TimeStampTable;
            to.CodProductPart = this.CodProductPart;
            to.CodProductPartTask = this.CodProductPartTask;
            to.OptionTypeOfTask = this.OptionTypeOfTask;
            to.CodOptionTypeOfTask = this.CodOptionTypeOfTask;


            foreach (var mk in ProductPartTaskOptions)
            {
                var mk2 = (ProductPartTaskOption)mk.Clone();
                //                mk2.ProductPartPrintingGain = to;
                if (to.ProductPartTaskOptions == null)
                {
                    to.ProductPartTaskOptions = new HashSet<ProductPartTaskOption>();
                }

                to.ProductPartTaskOptions.Add(mk2);

            }


        }

        public virtual object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartTask copyOfObject = (ProductPartTask)Activator.CreateInstance(kindOfObject);
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
