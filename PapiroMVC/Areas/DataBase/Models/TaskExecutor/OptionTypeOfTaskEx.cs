using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(OptionTypeOfTask_MetaData))]
    public partial class OptionTypeOfTask : IDataErrorInfo, ICloneable, IDeleteRelated
    {
       
        #region Proprietà aggiuntive
        public enum OptionTypeOfTaskType
        {
            AvarageRunForRun,
            CostPerRun,
            CostPerMq,
            DeficitForWeight,
            DeficitOnCostForWeight,
            BindingAvarageRunPerRunStep,
            BindingCostPerRunStep
        }

        public OptionTypeOfTaskType TypeOfOptionTypeOfTask
        {
            get;
            protected set;
        }

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
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

        public virtual void Copy(OptionTypeOfTask to)
        {
            //All properties of object
            //and pointer of sons

            to.IdexOf = this.IdexOf;
            to.TimeStampTable = this.TimeStampTable;
            to.CodOptionTypeOfTask = this.CodOptionTypeOfTask;
            to.CodTypeOfTask = this.CodTypeOfTask;
            to.OptionName = this.OptionName;
            to.ProductPartTasks = this.ProductPartTasks;
            to.ProductTasks = this.ProductTasks;
            to.TypeOfOptionTypeOfTask = this.TypeOfOptionTypeOfTask;
            to.TypeOfTask = this.TypeOfTask;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            OptionTypeOfTask copyOfObject = (OptionTypeOfTask)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //Prodotto copiaProdotto = (Prodotto)Activator.CreateInstance(copiaProdottoInDocumento.Prodotto.GetType());
            ////l'oggetto copisa sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(copiaProdotto);

            //sulla copia del prodotto in documento assegno la copia del suo prodotto
            //Example
            //copiaProdottoInDocumento.Prodotto = null;
            //copiaProdotto.ProdottoInDocumento = null;
            //copiaProdottoInDocumento.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInDocumento.Add(copiaProdottoInDocumento);
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
