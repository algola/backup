using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Novacode;

namespace PapiroMVC.Models
{
    //in questa viewmodel carico il costo
    //ed espongo l'eleco delle macchine che possono eseguire il task

    //se è un articolo--> ?? decider

            

    //    [MetadataType(typeof(TaskCostDetail_MetaData))]
    public partial class CostDetail : ICloneable, IPrintDocX
    {

        protected IQueryable<Article> _articles;

        public bool JustUpdated {get;set;}
        protected bool justInited = false;
        public bool JustPrintedInOrder { get; set; }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }

        /// <summary>
        /// get the partialview name
        /// </summary>
        public virtual string PartialViewName
        {
            get
            {
                return "_" + TypeOfCostDetail.ToString();
            }
        }


        public virtual List<CostDetail> CreateRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            return new List<CostDetail>();
        }

        public virtual ImplantCostDetail CreatorImplantCostDetail()
    {
        return new ImplantCostDetail();
    }

        public virtual List<CostDetail> GetRelatedImplantCostDetail(string codProductPartTask, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

            var x = CreatorImplantCostDetail();

            x.ComputedBy = this;
            x.ProductPart = this.ProductPart;

            //devo pescare il costo e associarlo al dettaglio
            if (x.CodCost == null)
            {
                var xxxx = costs.ToList();

                var cost = costs.Where(pp => pp.CodProductPartImplantTask == codProductPartTask).FirstOrDefault();
                //da non usare MAIIII                    x.TaskCost = cost;
                x.CodCost = cost.CodCost;
                x.CodCostDetail = cost.CodCost;

                x.CostDetailCostCodeRigen();
            }

            //GUID
            x.Guid = this.Guid;
            this.Computes.Add(x);
            lst.Add(x);

            return lst;
        }

        //this is used by get this.ComputedBy.ProductPartPrinting.PrintingFormat
        /// <summary>
        /// use this property to get PrintingFormat
        /// </summary>
        public string PrintingFormat
        {
            get
            {
                try
                {
                    if (this.ProductPartPrinting != null)
                    {
                        return this.ProductPartPrinting.PrintingFormat;
                    }
                    else
                    {
                        return this.Printers.First().ProductPartPrinting.PrintingFormat;
                    }
                }
                catch (Exception)
                {
                    return "0x0";
                }
            }
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            CostDetail copyOfObject = (CostDetail)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(CostDetail to)
        {

            to.TimeStampTable = this.TimeStampTable;
            to.CodCostDetail = this.CodCostDetail;
            to.CodTaskExecutorSelected = this.CodTaskExecutorSelected;
            to.CodCost = null;
            to.CodProductPart = this.CodProductPart;
            to.CodComputedBy = this.CodComputedBy;
            to.Starts = this.Starts;
            //      to. = this.Washes;
            to.GainForRun = this.GainForRun;
            to.GainForRunForPrintableArticle = this.GainForRunForPrintableArticle;
            to.GainForMqRun = this.GainForMqRun;
            to.GainForMqRunForPrintableArticle = this.GainForMqRunForPrintableArticle;
            to.TypeOfQuantity = this.TypeOfQuantity;
            to.GainForWeigthRun = this.GainForWeigthRun;
            to.GainForWeigthRunForPrintableArticle = this.GainForWeigthRunForPrintableArticle;
            to.Error = this.Error;
            to.Guid = this.Guid;
            to.RollChanges = this.RollChanges;

            to.CalculatedMl = this.CalculatedMl;
            to.CalculatedMq = this.CalculatedMq;
            to.CalculatedKg = this.CalculatedKg;
            to.CalculatedRun = this.CalculatedRun;

            to.Implants = this.Implants;

            //            public virtual Cost TaskCost = this.

            to.ProductPart = this.ProductPart;
            to.TaskexEcutorSelected = this.TaskexEcutorSelected;

            to.Computes = null;
            to.ComputedBy = null;

            if (ProductPartPrinting != null)
            {
                to.ProductPartPrinting = (ProductPartPrinting)ProductPartPrinting.Clone();
                //   to.ProductPartPrinting.CostDetail = to;
            }

            if (GainPrintingOnBuying != null)
            {
                var x = (ProductPartPrintingGain)GainPrintingOnBuying.Clone();
                //          x.CostDetail = to;
                to.GainPrintingOnBuying = x;
            }

            to.TaskCost = null;

        }

        public enum QuantityType : int
        {
            RunTypeOfQuantity = 0,
            MqWorkTypeOfQuantity = 1,
            WeigthTypeOfQuantity = 2, //quantità e prezzo al kg
            NColorPerMqTypeOfQuantity = 3,
            RunLengthMlTypeOfQuantity = 4, //ml di stampa
            NumberTypeOfQuantity = 5, //numero generico
            NOTypeOfQuantity = 10, //numero generico

        }

        private List<CostDetail> _previous;
        public List<CostDetail> Previouses
        {
            get
            {
                if (_previous == null)
                {
                    _previous = new List<CostDetail>();
                }
                return _previous;
            }
            set
            {
                _previous = value;
            }
        }

        //riferimento ai costi di stampa della parte
        public IQueryable<String> CodPartPrintingCostDetail
        { get; set; }

        private List<CostDetail> _printer;
        public List<CostDetail> Printers
        {
            get
            {
                if (_printer == null)
                {
                    _printer = new List<CostDetail>();
                }
                return _printer;
            }
            set
            {
                _printer = value;
            }
        }

        public enum CostDetailType : int
        {
            PrintingSheetCostDetail = 0,  //digital and litho sheet and rigid
            PrintingRollCostDetail = 1,  //digital and litho sheet and plotter

            PrintingZRollCostDetail = 2, // Cylinder
            PrintingFlatRollCostDetail = 3, // Flat

            PrintedSheetArticleCostDetail = 10,
            PrintedRollArticleCostDetail = 11,
            PrintedRigidArticleCostDetail = 12,

            ImplantCostDetail = 100,
            ImplantMeshCostDetail=101,
            ImplantHotPrintingCostDetail = 102,

            PrePostPressCostDetail = 200,
            ControlTableCostDetail = 201,

            RepassRollCostDetail = 202
        }

        public CostDetailType TypeOfCostDetail
        {
            get;
            protected set;
        }

        //codTaskExecutor 
        public void SetTaskexecutor(IQueryable<TaskExecutor> tskExecs, string codTaskExecutor)
        {
            var tsk = tskExecs.Where(x => x.CodTaskExecutor == codTaskExecutor).FirstOrDefault();

            tsk = tsk != null ? tsk : tskExecs.FirstOrDefault();

            if (tsk != null)
            {
                CodTaskExecutorSelected = tsk.CodTaskExecutor;
                TaskexEcutorSelected = tsk;
            }
        }


        public virtual double GainOnSide1 { get; set; }
        public virtual double GainOnSide2 { get; set; }

        public bool IsValid
        {
            get
            {
                return !(Error != 0 && Error != null);
            }
        }

        public List<TaskExecutor> TaskExecutors { get; set; }

        public IQueryable<CostDetail> _costDetailList;
        public virtual void Update(IQueryable<CostDetail> costDetailList)
        {
            _costDetailList = costDetailList;
        }

        public virtual void Update()
        {
            Console.WriteLine("");
        }

        public virtual void UpdateCoeff()
        {
            if (TaskExecutors != null)
            {
                TaskexEcutorSelected = TaskExecutors.Where(x => x.CodTaskExecutor == CodTaskExecutorSelected).FirstOrDefault();                
            }
            Console.WriteLine("");
        }

        IQueryable<TaskExecutor> tskExec;
        IQueryable<Article> articles;
        IQueryable<OptionTypeOfTask> optionTypeOfTasks;

        public virtual void InitCostDetail(IQueryable<TaskExecutor> _tskExec, IQueryable<Article> _articles)
        {
            if (!justInited)
            {

                articles = _articles;
                tskExec = _tskExec;


                if (ComputedBy != null)
                {
                    ComputedBy.InitCostDetail(_tskExec, _articles);
                }
                //     TaskCost = taskCost;

                tskExec = _tskExec;
                articles = _articles;

                //reset error
                Error = 0;

                justInited = true;
            }
        }

        public ProductPartPrintingGain GainPrintingOnBuying
        {
            get
            {
                return GainPrintingOnBuyings.FirstOrDefault();
            }
            set
            {
                GainPrintingOnBuyings.Clear();
                GainPrintingOnBuyings.Add(value);
            }
        }


        public virtual void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
            //           this.CodCostDetail = this.CodCost;


            if (ProductPart != null)
            {
                CodProductPart = ProductPart.CodProductPart;
            }

            if (ComputedBy != null)
            {
                if (CodComputedBy == null)
                {
                    ComputedBy.CostDetailCostCodeRigen();                    
                }
            }

            if (Computes != null)
            {
                foreach (var item in Computes)
                {
                    item.CodComputedBy = this.CodCostDetail;
                    item.CostDetailCostCodeRigen();
                }
            }

            if (this.GainPrintingOnBuying != null)
            {
                GainPrintingOnBuying.CodProductPartPrintingGain = CodCostDetail + "-CDET";
                GainPrintingOnBuying.TimeStampTable = DateTime.Now;

                GainPrintingOnBuying.CodProductPartPrintingGainBuying = CodCostDetail;

                var mrArray = GainPrintingOnBuying.Makereadies.ToList();
                foreach (var mr in GainPrintingOnBuying.Makereadies)
                {
                    mr.CodMakeready = GainPrintingOnBuying.CodProductPartPrintingGain + "-" + mrArray.IndexOf(mr);
                    mr.CodProductPartPrintingGain = GainPrintingOnBuying.CodProductPartPrintingGain;
                    mr.TimeStampTable = DateTime.Now;
                }
            }


            if (this.TypeOfCostDetail == CostDetailType.PrePostPressCostDetail || this.TypeOfCostDetail == CostDetailType.RepassRollCostDetail)
            {
                Console.Write("ciao");
            }

            if (this.ProductPartPrinting != null)
            {
                ProductPartPrinting.CodProductPartPrinting = CodCostDetail;
                ProductPartPrinting.TimeStampTable = DateTime.Now;

                if (ProductPartPrinting.GainPartOnPrinting != null)
                {
                    ProductPartPrinting.GainPartOnPrinting.CodProductPartPrinting = CodCostDetail;
                    ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain = ProductPartPrinting.CodProductPartPrinting;
                    ProductPartPrinting.GainPartOnPrinting.TimeStampTable = DateTime.Now;

                    var mrArray = ProductPartPrinting.GainPartOnPrinting.Makereadies.OrderBy(x => x.CodMakeready).ToList();
                    foreach (var mr in ProductPartPrinting.GainPartOnPrinting.Makereadies.OrderBy(x => x.CodMakeready))
                    {
                        mr.CodMakeready = ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain + "-" + mrArray.IndexOf(mr);
                        mr.CodProductPartPrintingGain = ProductPartPrinting.GainPartOnPrinting.CodProductPartPrintingGain;
                        mr.TimeStampTable = DateTime.Now;
                    }
                }
            }




        }

        Double _quantity;

        public virtual double Quantity(double qta, CostDetail.QuantityType type = CostDetail.QuantityType.NOTypeOfQuantity)
        {
            double ret;

            var typeOfQuantity = type == CostDetail.QuantityType.NOTypeOfQuantity ? TypeOfQuantity : (Nullable<int>)type;
            switch ((QuantityType)(typeOfQuantity ?? 0))
            {
                case QuantityType.RunTypeOfQuantity:
                    ret = Math.Ceiling(qta * this.GainForRun ?? 0);
                    break;
                case QuantityType.MqWorkTypeOfQuantity:
                    //se la lavorazione è prezzata a mq allora devo moltiplicare per i mq
                    ret = Math.Truncate(1000 * qta * (this.GainForMqRun ?? 0)) / 1000;
                    break;
                case QuantityType.WeigthTypeOfQuantity:
                    ret = Math.Ceiling(qta * this.GainForRun ?? 0);
                    break;
                case QuantityType.NColorPerMqTypeOfQuantity:
                    //se la lavorazione è prezzata a mq allora devo moltiplicare per i mq
                    ret = Math.Truncate(1000 * qta * (this.GainForMqRun ?? 0)) / 1000;
                    break;
                default:
                    ret = Math.Ceiling(qta * this.GainForRun ?? 0);
                    break;
            }

            return ret;
        }


        public virtual double QuantityMaterial(double qta, CostDetail.QuantityType type = CostDetail.QuantityType.NOTypeOfQuantity)
        {
            double ret;

            var typeOfQuantity = type==CostDetail.QuantityType.NOTypeOfQuantity? TypeOfQuantity:(Nullable<int>)type;

            switch ((QuantityType)(typeOfQuantity ?? 0))
            {
                case QuantityType.RunTypeOfQuantity:
                    ret = Math.Ceiling(qta * this.GainForRunForPrintableArticle ?? 0);
                    break;
                case QuantityType.MqWorkTypeOfQuantity:
                    //se la lavorazione è prezzata a mq allora devo moltiplicare per i mq
                    ret = Math.Truncate(1000 * qta * (this.GainForMqRunForPrintableArticle ?? 0)) / 1000;
                    break;
                case QuantityType.WeigthTypeOfQuantity:
                    ret = Math.Ceiling(qta * this.GainForRunForPrintableArticle ?? 0);
                    break;
                case QuantityType.NColorPerMqTypeOfQuantity:
                    //se la lavorazione è prezzata a mq allora devo moltiplicare per i mq
                    ret = Math.Truncate(1000 * qta * (this.GainForMqRunForPrintableArticle ?? 0)) / 1000;
                    break;
                default:
                    ret = Math.Ceiling(qta * this.GainForRunForPrintableArticle ?? 0);
                    break;
            }

            return ret;
        }

        public virtual double UnitCost(double qta)
        {
            return 0;
            //in questo metodo voglio calcolare il prezzo unitario
            //throw new NotImplementedException();
        }

        public virtual void MergeField(DocX doc)
        {
            TaskCost.MergeField(doc);

            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.Starts", this.Starts ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForRun", this.GainForRun ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForRunForPrintableArticle", this.GainForRunForPrintableArticle ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForMqRun", this.GainForMqRun ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForMqRunForPrintableArticle", this.GainForMqRunForPrintableArticle ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.TypeOfQuantity", this.TypeOfQuantity ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForWeigthRun", this.GainForWeigthRun ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.GainForWeigthRunForPrintableArticle", this.GainForWeigthRunForPrintableArticle ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.Error", this.Error ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.RollChanges", this.RollChanges ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.CalculatedMq", this.CalculatedMq ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.CalculatedMl", this.CalculatedMl ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.CalculatedKg", this.CalculatedKg ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.CalculatedRun", this.CalculatedRun ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.CalculatedTime", (this.CalculatedTime ?? new TimeSpan(0, 0, 0)).ToString()));

            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.Implants", this.Implants ?? 0));            

            if (ProductPartPrinting != null)
            {
                TaskexEcutorSelected = TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                ProductPartPrinting.MergeField(doc);
            }

        }



    }
}