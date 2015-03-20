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
        public static IQueryable<TaskExecutor> FilterByTask(IQueryable<TaskExecutor> tskExecList, string codTypeOfTask)
        {



            if (codTypeOfTask == "TAVOLOCONTROLLO")
            {
                tskExecList = tskExecList.OfType<ControlTableRoll>();
            }

            if (codTypeOfTask == "STAMPARIGIDO")
            {
                tskExecList = tskExecList.OfType<PlotterSheet>();
            }

            if (codTypeOfTask == "STAMPAMORBIDO")
            {
                tskExecList = tskExecList.OfType<PlotterRoll>();
            }

            if (codTypeOfTask == "STAMPAOFF")
            {
                tskExecList = tskExecList.OfType<LithoSheet>();
            }

            if (codTypeOfTask == "STAMPAOFFeDIGITALE")
            {
                var tskExec1 = tskExecList.OfType<LithoSheet>();
                var tskExec2 = tskExecList.OfType<DigitalSheet>();

                tskExecList = tskExec1.Union<TaskExecutor>(tskExec2);
            }

            //CodTypeOfTask
            if (codTypeOfTask == "STAMPAETICHROTOLO")
            {
             //   var tskExec1 = tskExecList.OfType<Flexo>();
             //   var tskExec2 = tskExecList.OfType<ControlTableRoll>();

                tskExecList = tskExecList.OfType<Flexo>();
            }


            //Created for Editor Machines
            if (codTypeOfTask == "STAMPAETICHROTOLO_LIST")
            {
                var tskExec1 = tskExecList.OfType<Flexo>();
                var tskExec2 = tskExecList.OfType<ControlTableRoll>();
                var tskExec3 = tskExecList.OfType<FlatRoll>();

                tskExecList = tskExec1.Union<TaskExecutor>(tskExec2).Union(tskExec3);
            }


            if (codTypeOfTask == "FUSTELLATURA")
            {
                tskExecList = tskExecList.OfType<TaskExecutor>().Where(x => x.CodTypeOfTask == "FUSTELLATURA");
            }


            if (codTypeOfTask == "INPIANO")
            {
                var tskExec1 = tskExecList.OfType<LithoSheet>();
                var tskExec2 = tskExecList.OfType<DigitalSheet>();
                var tskExec3 = tskExecList.OfType<TaskExecutor>().Where(x => x.CodTypeOfTask == "FUSTELLATURA");

                tskExecList = tskExec1.Union<TaskExecutor>(tskExec2).Union<TaskExecutor>(tskExec3);

            }


            return tskExecList;

        }

        public virtual double GetStarts(string codOptionTypeOfTask)
        {
            throw new Exception("Not implemented");
        }

        public virtual double GetWashes(string codOptionTypeOfTask)
        {
            return 0;
        }

        public virtual void GetColorFR(string codOptionTypeOfTask, out double cToPrintF, out double cToPrintR, out double cToPrintT)
        {
            throw new NotImplementedException();
        }

        public virtual double GetImplants(string codOptionTypeOfTask)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// tipo di quantità
        /// </summary>
        public virtual CostDetail.QuantityType TypeOfQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.RunTypeOfQuantity;

                if (TypeOfExecutor == ExecutorType.Flexo)
                {
                    ret = CostDetail.QuantityType.RunLengthMlTypeOfQuantity;
                }

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
                            ret = CostDetail.QuantityType.MqWorkTypeOfQuantity;
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.BindingOnRun:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.DigitalOnTime:
                            break;
                        case TaskEstimatedOn.EstimatedOnType.PlotterOnMq:
                            ret = CostDetail.QuantityType.MqWorkTypeOfQuantity;
                            break;
                        default:
                            break;
                    }
                }

                return ret;
            }
        }

        public virtual CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.NOTypeOfQuantity;
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
            Binding = 7,
            Flexo = 8,
            FlatRoll = 9,

            ControlTableRoll = 10

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
