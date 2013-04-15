using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Step_MetaData))]
    public abstract partial class Step : IDataErrorInfo, ICloneable, IDeleteRelated
    {
       
        #region Proprietà aggiuntive
        public enum StepType
        {
            AvarageRunForRun,
            CostPerRun,
            CostPerMq,
            DeficitForWeight,
            DeficitOnCostForWeight,
            BindingAvarageRunPerRunStep,
            BindingCostPerRunStep
        }

        public StepType TypeOfStep
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

        public virtual void Copy(Step to)
        {
            //All properties of object
            //and pointer of sons

            to.CodTaskEstimatedOn = this.CodTaskEstimatedOn;
            to.taskexecutorestimatedon = this.taskexecutorestimatedon;
            to.TimeStampTable = this.TimeStampTable;
            to.ToUnit = this.ToUnit;
            to.FromUnit = this.FromUnit;
            to.TypeOfStep = this.TypeOfStep;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Step copyOfObject = (Step)Activator.CreateInstance(kindOfObject);
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
