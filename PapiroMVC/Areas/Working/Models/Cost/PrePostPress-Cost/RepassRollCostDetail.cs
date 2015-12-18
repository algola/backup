using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        public override ImplantCostDetail CreatorImplantCostDetail()
        {
            ImplantCostDetail ret = new ImplantCostDetail();

            if (this.TaskCost.ProductPartTask.CodOptionTypeOfTask.Contains("SERIGRAFIA"))
            {
                ret = new ImplantMeshCostDetail();
            }

            if (this.TaskCost.ProductPartTask.CodOptionTypeOfTask.Contains("STAMPAACALDO"))
            {
                ret = new ImplantHotPrintingCostDetail();
            }

            return ret;

        }

        double _calculateMqImplant=-1;
        public double CalculatedMqImplant
        {
            get
            {
                if (_calculateMqImplant <0)
                {
                    UpdateCoeff();
                }
                return _calculateMqImplant;
            }
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

                var print = Printers.FirstOrDefault();
                Console.WriteLine(print.ProductPartPrinting.CalculatedSide1Gain);
                Console.WriteLine(print.ProductPartPrinting.CalculatedSide2Gain);

                //sperimentale potrebbe essere inserita nella procedura anche il controllo della doppia pinza, etc...
                if (SheetCut.IsValid(tsk.FormatMax, ProductPart.FormatOpened, tsk.FormatMin))
                {
                    //i tagli che vanno bene nel formato minimo e massimo
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin, false, print.ProductPartPrinting.CalculatedSide2Gain);
                }
                else
                {
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
                        WorkingFormat = x[k].GetCuttedFormat(BuyingFormat,false)) ;
                }

                foreach (var item in x)
                {
                    item.CutName = item.GetCuttedFormat(BuyingFormat,false);
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
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat,false);
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

        //0 Serigrafia //1 Laminazione
        public int TypeOfRepass
        {
            get
            {
                int ret = 0;
                if (this.TaskCost.ProductPartTask.TypeOfProductPartTask == ProductPartTask.ProductPartTasksType.ProductPartSerigraphy)
                {
                    ret = 0;
                }

                if (this.TaskCost.ProductPartTask.TypeOfProductPartTask == ProductPartTask.ProductPartTasksType.ProductPartHotPrinting)
                {
                    ret = 1;
                }

                return ret;
            }

        }



        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            Error = 0;
            base.InitCostDetail(tskExec, articles);
        }

        double gain = 1;
        double gainForRun = 1;

        double gainOnSide1 = 1;
        double gainOnSide2 = 1;

        private void reDoneGains()
        {
            var x = Cuts;
            double gainSide1Printer = 1;
            double gainSide2Printer = 1;

            Cut currentCut = new Cut("manuale", 1, 1);

            foreach (var item in Cuts)
            {
                var res = item.GetCuttedFormat(BuyingFormat, false);
                if (res == WorkingFormat)
                {
                    gainOnSide1 = item.PartsOnSide1;
                    gainOnSide2 = item.PartsOnSide2;

                    gain = item.Gain;
                    currentCut = item;
                }
            }

            Console.WriteLine(Cuts);


            double dCut1 = 0, dCut2 = 0;
            string format = "1x1";

            if (Printers != null)
            {
                foreach (var fromP in this.Printers)
                {
                    //                    Starts = fromP.GainOnSide1;
                    gainForRun *= fromP.GainForRunForPrintableArticle ?? 1;
                    gainSide1Printer *= fromP.ProductPartPrinting.CalculatedSide1Gain;
                    gainSide2Printer *= fromP.ProductPartPrinting.CalculatedSide2Gain;

                    dCut1 = fromP.ProductPartPrinting.CalculatedDCut1;
                    dCut2 = fromP.ProductPartPrinting.CalculatedDCut2;

                    format = fromP.ProductPart.Format;
                }
            }

            gainSide1Printer = gainSide1Printer / currentCut.PartsOnSide1;
            gainSide2Printer = gainSide2Printer / currentCut.PartsOnSide2;

            double side1 = 2 + format.GetSide1() * gainSide1Printer + dCut1 * (gainSide1Printer - 1);
            double side2 = 2 + format.GetSide2() * gainSide2Printer + dCut2 * (gainSide2Printer - 1);

            _calculateMqImplant = side1 * side2 / 10000;

        
        }


        public override double GainOnSide1
        {
            get
            {
                reDoneGains();
                return gainOnSide1;
            }
            set
            {
                base.GainOnSide1 = value;
            }
        }


        public override double GainOnSide2
        {
            get
            {
                reDoneGains();
                return gainOnSide2;
            }
            set
            {
                base.GainOnSide2 = value;
            }
        }


        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //a questo punto vorrei arrivare ad avere

            //devo capire quale tipo quantità usare e che moltiplicatore usare!!!!
            //lo devo salvare in una proprietà del dettaglio costo
            reDoneGains();

            //calcolo di quanti impianti sono necessari!!!!
            Implants = TaskexEcutorSelected.GetImplants(TaskCost.ProductPartTask.CodOptionTypeOfTask);
            Starts = TaskexEcutorSelected.GetStarts(TaskCost.ProductPartTask.CodOptionTypeOfTask);


            GainForRun = Starts * gainForRun / gain;
            TypeOfQuantity = Convert.ToInt16(QuantityType.RunTypeOfQuantity);  //TaskexEcutorSelected.TypeOfImplantQuantity;

        }

        public override void Update()
        {
            base.Update();
            this.UpdateCoeff();                
            //voglio fare l'update dei dostdetail simili che hanno la stessa macchina        
        }
        
        public override double Quantity(double qta)
        {
            double quantita = 0;
            quantita = Math.Ceiling(qta * (GainForRun ?? 1));
            return quantita;
        }

        double totCostInk = 0;

        public override double UnitCost(double qta)
        {

            #region read and calculate Ink --> totCostInk

            totCostInk = 0;

            double quantita = 0;
            quantita = Math.Ceiling(qta * (GainForRun ?? 1));

            var fto = this.TaskCost.ProductPartTask.ProductPart.FormatOpened;
            var mqTot = (fto.GetSide1() * fto.GetSide2() / 10000) * qta;


            if (this.TypeOfRepass == 0)
            {
                List<ProductPartSerigraphyOption> optSeris = new List<ProductPartSerigraphyOption>();
                //serigraphy options where we can find the inks and types
                foreach (var item in this.TaskCost.ProductPartTask.ProductPartTaskOptions.OfType<ProductPartSerigraphyOption>())
                {
                    optSeris.Add((ProductPartSerigraphyOption)item);
                }

                foreach (var item in optSeris)
                {

                    int mqPrint = 0;
                    mqPrint = Convert.ToInt32(Math.Ceiling(mqTot * (item.Overlay ?? 0) / 100));

                    //var seriSpec = option
                    var inkSpec = _articles.OfType<Ink>().FirstOrDefault(x => x.ArticleName == item.InkSerigraphy);
                    var typeSeri = (Mesh)_articles.OfType<Mesh>().FirstOrDefault(x => x.ArticleName == item.TypeOfTaskSerigraphy);

                    if (inkSpec != null && typeSeri != null)
                    {
                        int ltTot = Convert.ToInt32(Math.Ceiling(mqPrint / Convert.ToDouble(typeSeri.GainMqPerLt ?? 1)));
                        totCostInk += ltTot * Convert.ToDouble(inkSpec.ArticleCosts.OfType<NoPrintableArticleCostKg>().FirstOrDefault().CostPerKg, Thread.CurrentThread.CurrentUICulture);
                    }
                }

            }
            else
            {
                List<ProductPartHotPrintingOption> optSeris = new List<ProductPartHotPrintingOption>();
                //serigraphy options where we can find the inks and types
                foreach (var item in this.TaskCost.ProductPartTask.ProductPartTaskOptions.OfType<ProductPartHotPrintingOption>())
                {
                    optSeris.Add((ProductPartHotPrintingOption)item);
                }

                foreach (var item in optSeris)
                {
                    int mqPrint = 0;
                    mqPrint = Convert.ToInt32(mqTot);

                    //var seriSpec = option
                    var foilSpec = _articles.OfType<Foil>().FirstOrDefault(x => x.ArticleName == item.Foil);

                    if (foilSpec != null)
                    {
                        totCostInk += mqPrint * Convert.ToDouble(foilSpec.ArticleCosts.OfType<NoPrintableArticleCostMq>().FirstOrDefault().CostPerMq, Thread.CurrentThread.CurrentUICulture);
                    }
                }
            }


            #endregion

            if (!IsValid)
            {
                return 0;
            }

            RollChanges = 0;

            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!
            double total = 0;

            total = totCostInk;

            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();

            try
            {
                try
                {
                    var x = TaskExecutor.GetColorFR(TaskCost.ProductPartTask.CodOptionTypeOfTask);
                    var costCalcolous = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault();

                    totalCT = costCalcolous.GetCost(
                        TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, x.cToPrintR, RollChanges ?? 0, (int)(x.cToPrintT + x.cToPrintTNoImplant), running: Quantity(qta));
                }
                catch (NotImplementedException)
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1,  Quantity(qta));
                }
                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;

                //calcolo del tempo e del costo
                total = totCostInk;
                total += totalCT.Cost;
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

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);

            var description = String.Empty;
            if (TypeOfRepass == 0)
            {
                List<ProductPartSerigraphyOption> optSeris = new List<ProductPartSerigraphyOption>();
                //serigraphy options where we can find the inks and types
                foreach (var item in this.TaskCost.ProductPartTask.ProductPartTaskOptions.OfType<ProductPartSerigraphyOption>())
                {
                    description += item.TypeOfTaskSerigraphy + " " + item.InkSerigraphy + "\n";
                }

            }
            else
            {
                List<ProductPartHotPrintingOption> optSeris = new List<ProductPartHotPrintingOption>();
                //serigraphy options where we can find the inks and types
                foreach (var item in this.TaskCost.ProductPartTask.ProductPartTaskOptions.OfType<ProductPartHotPrintingOption>())
                {
                    description += item.Foil + " " + item.Format + "\n";
                }

            }

            doc.AddCustomProperty(new Novacode.CustomProperty("CostDetail.OptionTask", description));
            doc.AddCustomProperty(new Novacode.CustomProperty("PPP.PrintingFormat", this.WorkingFormat));

        }


    }
}