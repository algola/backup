using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Views.Shared.App_LocalResources;
using System.Reflection;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartTask_MetaData))]
    public partial class ProductPartTask : IDataErrorInfo, ICloneable, IDeleteRelated
    {

        //public ProductPartTask()
        //{
        //    TypeOfProductPartTask = ProductPartTasksType.ProductPartTask;
        //}

        #region Proprietà aggiuntive       
        public enum ProductPartTasksType : int
        {
            ProductPartTask = 0,
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
            return (string) t.GetProperty("Cod" + this.CodOptionTypeOfTask).GetValue(null, null);
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
        }

        public object Clone()
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
