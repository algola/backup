using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{

    public abstract partial class TaskExecutor : IDataErrorInfo, ICloneable, IDeleteRelated
    {
       
        #region Proprietà aggiuntive
        public enum ExecutorType : int
        {
            LithoSheet=0,
            LithoWeb=1,
            DigitalSheet=2,
            DigitalWeb=3,
            Plotter=4,
            PrePostPress=5,
            Binding=6
        }

        public ExecutorType TypeOfExecutor
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

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       "FormatMin",
                       "FormatMax",
                       "TaskExecutorName"
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
                if (proprieta == "FormatMin" && this.FormatMin != null)
                {
                    Regex exp = new Regex(@"^[0-9.,]{1,5}x[0-9.,]{1,5}$", RegexOptions.IgnoreCase);
                    if (!exp.IsMatch(this.FormatMin.ToString()))
                    {
                        result = "Messagge Error";
                    }
                }
                //Format Validation
                if (proprieta == "FormatMax" && this.FormatMax != null)
                {
                    Regex exp = new Regex(@"^[0-9.,]{1,5}x[0-9.,]{1,5}$", RegexOptions.IgnoreCase);
                    if (!exp.IsMatch(this.FormatMax.ToString()))
                    {
                        result = "Messagge Error";
                    }
                }
                //validazione della proprietà Note
                if (proprieta == "TaskExecutorName")
                {
                    if (this.TaskExecutorName != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,255}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.TaskExecutorName))
                        {
                            result = "Superata la lunghezza delle note consentita";
                        }
                    }
                }
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

        public virtual void Copy(TaskExecutor to)
        {
            //All properties of object
            //and pointer of sons

            to.CodTaskExecutor = this.CodTaskExecutor;
            to.TaskExecutorName = this.TaskExecutorName;
            to.TimeStampTable = this.TimeStampTable;
            to.Version = this.Version;
            to.FormatMin = this.FormatMin;
            to.FormatMax = this.FormatMax;
            to.WeightMax = this.WeightMax;
            to.WeightMin = this.WeightMin;
            to.Pinza = this.Pinza;
            to.ControPinza = this.ControPinza;
            to.Laterale = this.Laterale;
            to.SetTaskExecutorEstimatedOn = this.SetTaskExecutorEstimatedOn;

        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            TaskExecutor copyOfObject = (TaskExecutor)Activator.CreateInstance(kindOfObject);
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
