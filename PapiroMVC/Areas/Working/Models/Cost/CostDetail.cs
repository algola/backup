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
    public partial class CostDetail
    {
        public enum QuantityType : int
        {
            RunTypeOfQuantity = 0,
            MqWorkTypeOfQuantity = 1,
            WeigthTypeOfQuantity = 2, //quantità e prezzo al kg
            MqSheetTypeOfQuantity = 3,
            RunLengthMlTypeOfQuantity = 4, //ml di stampa

            NumberTypeOfQuantity = 5, //numero generico


            NOTypeOfQuantity = 10, //numero generico
        }

        public enum CostDetailType : int
        {
            PrintingSheetCostDetail = 0,  //digital and litho sheet and rigid
            PrintingRollCostDetail = 1,  //digital and litho sheet and plotter

            PrintingLabelRollCostDetail = 2, // label roll

            PrintedSheetArticleCostDetail = 10,
            PrintedRollArticleCostDetail = 11,
            PrintedRigidArticleCostDetail = 12,


            ImplantCostDetail = 100

        }

        public CostDetailType TypeOfCostDetail
        {
            get;
            protected set;
        }


        public bool IsValid
        {
            get
            {
                return !(Error != 0 && Error != null);
            }
        }

        public List<TaskExecutor> TaskExecutors { get; set; }

        public virtual void Update()
        {

        }

        public virtual void UpdateCoeff()
        {

        }

        public virtual void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
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

            if (this.GainPrintingOnBuying != null)
            {
                GainPrintingOnBuying.CodProductPartPrintingGain = CodCostDetail + "-CDET";
                GainPrintingOnBuying.TimeStampTable = DateTime.Now;

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
                case QuantityType.MqSheetTypeOfQuantity:
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