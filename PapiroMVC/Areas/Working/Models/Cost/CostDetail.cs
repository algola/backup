using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    //in questa viewmodel carico il costo
    //ed espongo l'eleco delle macchine che possono eseguire il task

    //se è un articolo--> ?? decider

    //    [MetadataType(typeof(TaskCostDetail_MetaData))]
    public partial class CostDetail : ICloneable
    {

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
        public List<CostDetail> Printeres
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

            PrintingLabelRollCostDetail = 2, // label roll

            PrintedSheetArticleCostDetail = 10,
            PrintedRollArticleCostDetail = 11,
            PrintedRigidArticleCostDetail = 12,

            ImplantCostDetail = 100,
            PrePostPressCostDetail = 200

        }

        public CostDetailType TypeOfCostDetail
        {
            get;
            protected set;
        }


        //TEMPORANEO forse e' meglio salvarlo??
        public virtual int GainOnSide1
        {
            get;
            set;
        }

        //TEMPORANEO forse è meglio salvarlo nel db??
        public virtual int GainOnSide2
        {
            get;
            set;
        }

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

        }

        public virtual void UpdateCoeff()
        {

        }

        public virtual void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            if (ComputedBy != null)
            {
                ComputedBy.InitCostDetail(tskExec, articles);
            }
            //     TaskCost = taskCost;
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

            if (Computes != null)
            {
                foreach (var item in Computes)
                {
                    item.CodComputedBy = this.CodCostDetail;
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

        public virtual double Quantity(double qta)
        {
            double ret;

            switch ((QuantityType)(TypeOfQuantity ?? 0))
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

        public virtual double UnitCost(double qta)
        {
            return 0;
            //in questo metodo voglio calcolare il prezzo unitario
            //throw new NotImplementedException();
        }

    }
}