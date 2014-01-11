using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(TaskExecutor_MetaData))]
    public abstract partial class TaskExecutor : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        /// <summary>
        /// filters whitch taskexecutor can run codTypeOfTask task
        /// </summary>
        /// <param name="codTypeOfTask"></param>
        /// <returns></returns>
        public static IQueryable<TaskExecutor> FilterByTask(IQueryable<TaskExecutor>tskExec, string codTypeOfTask)
        {
            if (codTypeOfTask == "STAMPARIGIDO")
            {
                tskExec = tskExec.OfType<PlotterSheet>();
            }

            if (codTypeOfTask == "STAMPAOFF")
            {
                tskExec = tskExec.OfType<LithoSheet>();
            }

            if (codTypeOfTask == "STAMPAOFFeDIGITALE")
            {
                var tskExec1 = tskExec.OfType<LithoSheet>();
                var tskExec2 = tskExec.OfType<DigitalSheet>();

                tskExec = tskExec1.Union<TaskExecutor>(tskExec2);
            }

            return tskExec;

        }

        public virtual double Starts(string codOptionTypeOfTask)
        {
            throw new Exception("Not implemented");
        }

        public CostDetail.QuantityType TypeOfQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.RunTypeOfQuantity;
                var estimatedOn = this.SetTaskExecutorEstimatedOn.FirstOrDefault();
                if (estimatedOn != null)
                {
                    switch (estimatedOn.TypeOfEstimatedOn)
                    {
                        case TaskEstimatedOn.EstimatedOnType.OnRun:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.OnMq:
                            ret = CostDetail.QuantityType.MqTypeOfQuantity;
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnRun:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.DigitalOnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.PlotterOnMq:
                            ret = CostDetail.QuantityType.MqTypeOfQuantity;
                            break;
                        default:
                            break;
                    }
                }

                return ret;
            }
        }

        #region Proprietà aggiuntive
        public enum ExecutorType : int
        {
            LithoSheet = 0,
            LithoRoll = 1,
            DigitalSheet = 2,
            DigitalRoll = 3,
            PlotterSheet = 4,
            PlotterRoll = 5,
            PrePostPress = 6,
            Binding = 7
        }

        public string[] CodTypeOfTaskList { get; set; }


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
            to.CodTypeOfTask = this.CodTypeOfTask;
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
