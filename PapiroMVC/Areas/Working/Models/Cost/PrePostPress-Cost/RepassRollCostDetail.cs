using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public partial class RepassRollCostDetail : PrePostPressCostDetail, ICloneable
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            RepassRollCostDetail to2 = (RepassRollCostDetail)to;

            to = to2;

        }

        public RepassRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.RepassRollCostDetail;
        }

        /// <summary>
        /// this cuts returns a plausible format by 
        /// </summary>
        public List<Cut> Cuts
        {
            get
            {
                List<Cut> y;

                //task executor corrente
                var tsk = TaskExecutors.Where(it => it.CodTaskExecutor == CodTaskExecutorSelected).FirstOrDefault();

                //combino per ora gli z della macchina con le larghezze
                //è da tenere presente dei colori!!!!
                //il formato massimo della macchina deve essere calcolato come la larghezza x il massimo Z

                //sperimentale potrebbe essere inserita nella procedura anche il controllo della doppia pinza, etc...
                if (SheetCut.IsValid(tsk.FormatMax, ProductPart.FormatOpened, tsk.FormatMin))
                {
                    //i tagli che vanno bene nel formato minimo e massimo
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin);
                }
                else
                {

                    var print = Printers.FirstOrDefault();
                    Console.WriteLine(print.ProductPartPrinting.CalculatedSide1Gain);
                    Console.WriteLine(print.ProductPartPrinting.CalculatedSide2Gain);

                    //i tagli che vanno bene nel formato minimo e formato lavoro
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, ProductPart.FormatOpened, false, print.ProductPartPrinting.CalculatedSide2Gain);
                }




                //ma solo quelli validi
                List<Cut> x = new List<Cut>();
                x = y.Where(k => k.Valid).ToList();

                if (WorkingFormat != null && WorkingFormat != "")
                {
                    var k = 0;
                    for (int i = 0; i < x.Count &&
                        !SheetCut.IsValid(tsk.FormatMax, tsk.FormatMin, WorkingFormat);
                        i++,
                        WorkingFormat = x[k].GetCuttedFormat(BuyingFormat)) ;
                }

                foreach (var item in x)
                {
                    item.CutName = item.GetCuttedFormat(BuyingFormat);
                }

                //Controllo del formato se è presente nell'elenco dei formati
                //Se non è presente lo aggiungo

                //da controllare solo se l'elenco non è vuoto    
                if (x.Count > 0)
                {
                    var ele = x.Find(z => z.CutName == WorkingFormat);
                    if (ele == null && WorkingFormat != null)
                    {
                        var toAdd = new Cut("manual", 0, 0);
                        toAdd.ManualFormat = WorkingFormat;
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat);
                        x.Add(toAdd);
                    }
                }

                return x;
            }
        }


        public virtual void GetCostFromList()
        {
            //throw new NotImplementedException();
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            Error = 0;
            base.InitCostDetail(tskExec, articles);
        }

        public override void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
        }




        //lo voglio prendere dalla stampa!!! che deve esserci SEMPRE (per ora)
        public string BuyingFormat
        {
            get
            {
                string ret = String.Empty;

                foreach (var fromP in this.Printers)
                {
                    ret = fromP.PrintingFormat;
                }
                return ret;

            }
        }


        //public string PrintingFormat
        //{
        //    get;
        //    set;
        //}


        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //a questo punto vorrei arrivare ad avere

            //devo capire quale tipo quantità usare e che moltiplicatore usare!!!!
            //lo devo salvare in una proprietà del dettaglio costo

            var x = Cuts;

            foreach (var item in Cuts)
            {
                var res = item.GetCuttedFormat(BuyingFormat);
                if (res == WorkingFormat)
                {
                    GainForRun = item.Gain;
                }
            }

            Console.WriteLine(Cuts);



            if (Printers != null)
            {

                double gainForRun = 1;
                foreach (var fromP in this.Printers)
                {
                    Starts = fromP.GainOnSide1;
                    gainForRun *= fromP.GainForRun ?? 1;
                }

            }

            TypeOfQuantity = Convert.ToInt16(QuantityType.RunTypeOfQuantity);  //TaskexEcutorSelected.TypeOfImplantQuantity;
        }


        public override void Update()
        {
            base.Update();
            this.UpdateCoeff();
        }

        public override double Quantity(double qta)
        {
            double quantita = 0;
            int typeOfQ = 0;

            if (Printers != null)
            {
                foreach (var item in Printers)
                {
                    quantita += (item.CalculatedMl * 100) / this.WorkingFormat.GetSide2() ?? 0;
                    //                    this.TypeOfQuantity = item.TypeOfQuantity;
                }

            }

            return Math.Ceiling(quantita * GainForRun ?? 1);
        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            try
            {
                var labelPerRoll = ((ProductPartSingleLabelRoll)this.Printers.FirstOrDefault().ProductPart).LabelsPerRoll;
                if (labelPerRoll != null)
                {
                    RollChanges = (qta / labelPerRoll) / this.Printers.FirstOrDefault().ProductPartPrinting.CalculatedSide1Gain;
                }
            }
            catch (Exception)
            {
                RollChanges = 0;
                //throw;
            }


            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!
            double total = 0;
            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();

            try
            {
                try
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, 1,0, RollChanges ?? 0, (int)(Starts ?? 0), Quantity(qta));
                }
                catch (NotImplementedException)
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, Quantity(qta));
                }
                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;

                //calcolo del tempo e del costo
                total = totalCT.Cost;
                CalculatedTime = totalCT.Time;

            }
            catch (NullReferenceException)
            {
                total = 0;
                Error = 2;
            }

            if (TaskCost.Quantity != null)
            {
                return total / Quantity(qta);

            }
            else
            {
                return 0;
            }

        }

    }
}