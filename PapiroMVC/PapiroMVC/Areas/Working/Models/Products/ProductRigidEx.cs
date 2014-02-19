using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [Serializable]
    [KnownType(typeof(ProductRigid))]
    [MetadataType(typeof(ProductRigid_MetaData))]
    public partial class ProductRigid : Product, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public ProductRigid()
        {
            this.TypeOfProduct = ProductType.ProductRigid;
        }

        public override void InitProduct()
        {
            base.InitProduct();

            var p = new ProductPartRigidArticle();
            var part = new ProductPartRigid();

            part.DCut = DCut;
            part.ShowDCut = ShowDCut;
            part.IsDCut = false;

            part.ProductPartTasks = this.GetInitalizedPartTask();

            ProductPartTask partTask;
            partTask = part.ProductPartTasks.First(x => x.CodOptionTypeOfTask == "STAMPARIGIDO_NO");
            partTask.Hidden = false;
            partTask.IndexOf = 1;

            part.ProductPartPrintableArticles.Add(p);
            ProductParts.Add(part);

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

        public virtual void Copy(ProductRigid to)
        {
            //All properties of object
            //and pointer of sons
            to.Format = this.Format;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductRigid copyOfObject = (ProductRigid)Activator.CreateInstance(kindOfObject);
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


        public override List<ProductPartTask> GetInitalizedPartTask()
        {
            var tsksInPart = new List<ProductPartTask>();

            ProductPartTask pt;

            String[] codTypeOfTasks = { "STAMPARIGIDO" };

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

        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);
            var s = (string)t.GetProperty("CodMenuProduct" + this.CodMenuProduct).GetValue(null, null);

            return s + " " + base.ToString();
        }

    }
}
