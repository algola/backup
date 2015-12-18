using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrePostPressCostDetail : CostDetail, ICloneable
    {


        public override void CostDetailCostCodeRigen()
        {
            this.TimeStampTable = DateTime.Now;
        }



        public override void UpdateCoeff()
        {
            base.UpdateCoeff();
            //a questo punto vorrei arrivare ad avere

            //devo capire quale tipo quantità usare e che moltiplicatore usare!!!!
            //lo devo salvare in una proprietà del dettaglio costo

            double gain = 1;


            if (ProductPart != null)
            {


                var x = Cuts;

                Cut currentCut = new Cut("manuale", 1, 1);

                foreach (var item in Cuts)
                {
                    var res = item.GetCuttedFormat(BuyingFormat,false);
                    if (res == WorkingFormat)
                    {
                        gain = item.Gain;
                        currentCut = item;
                    }
                }

                Console.WriteLine(Cuts);

                double gainSide1 = 1;
                double gainSide2 = 1;
                double gainForRun = 1;

                double dCut1 = 0, dCut2 = 0;
                string format = "1x1";

                if (Printers != null)
                {
                    foreach (var fromP in this.Printers)
                    {
                        //                    Starts = fromP.GainOnSide1;
                        gainForRun *= fromP.GainForRunForPrintableArticle ?? 1;
                        gainSide1 *= fromP.ProductPartPrinting.CalculatedSide1Gain;
                        gainSide2 *= fromP.ProductPartPrinting.CalculatedSide2Gain;

                        dCut1 = fromP.ProductPartPrinting.CalculatedDCut1;
                        dCut2 = fromP.ProductPartPrinting.CalculatedDCut2;

                        format = fromP.ProductPart.Format;
                    }
                }

                gainSide1 = gainSide1 / currentCut.PartsOnSide1;
                gainSide2 = gainSide2 / currentCut.PartsOnSide2;

                if (this.TaskCost.ProductPartTask.CodOptionTypeOfTask.Contains("TAGLIO"))
                {
                    //nuber of cuts
                    double cuts = 0;
                    cuts += dCut1 == 0 ? gainSide1 + 1 : gainSide1 * 2;
                    cuts += dCut2 == 0 ? gainSide2 + 1 : gainSide2 * 2;

                    if (cuts != 0)
                    {
                        gain = 1 / cuts;
                    }
                    else
                    {
                        gain = 1;
                    }
                }

            }
            else
            {
                Console.WriteLine();
            }

            TypeOfQuantity = Convert.ToInt16(QuantityType.RunTypeOfQuantity);  //TaskexEcutorSelected.TypeOfImplantQuantity;

            GainForRun = gain;

            //            _calculateMqImplant = side1 * side2 / 10000;

            //calcolo di quanti impianti sono necessari!!!!
            //    Implants = TaskexEcutorSelected.GetImplants(TaskCost.ProductPartTask.CodOptionTypeOfTask);
            //   Starts = TaskexEcutorSelected.GetStarts(TaskCost.ProductPartTask.CodOptionTypeOfTask);
            //            GainForRun = Starts * gainForRun / gain;


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
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin, false, print.ProductPartPrinting.CalculatedSide2Gain, print.ProductPartPrinting.CalculatedSide1Gain, true);
                }
                else
                {
                    //i tagli che vanno bene nel formato minimo e formato lavoro
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, ProductPart.FormatOpened, false, print.ProductPartPrinting.CalculatedSide2Gain, print.ProductPartPrinting.CalculatedSide1Gain, true);
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
                        WorkingFormat = x[k].GetCuttedFormat(BuyingFormat, false)) ;
                }

                foreach (var item in x)
                {
                    item.CutName = item.GetCuttedFormat(BuyingFormat, false);
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
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat, false);
                        x.Add(toAdd);
                    }
                }

                return x;
            }
        }



        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {

            if (!justInited)
            {
                base.InitCostDetail(tskExec, articles);

                _articles = articles.ToList().AsQueryable();

                String codTypeOfTask = String.Empty;
                //Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
                codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
                tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);
                TaskExecutors = tskExec.ToList();

                //            if (TaskexEcutorSelected == null && CodTaskExecutorSelected != "")

                if (CodTaskExecutorSelected != "")
                {
                    TaskexEcutorSelected = TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                }
            }

        }

        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!
            double total = 0;
            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();

            try
            {
                totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(codOptionTypeOfTask: TaskCost.ProductPartTask.CodOptionTypeOfTask, starts: Starts ?? 1, makereadis: 0, colors: new PrintingColor(), running: Quantity(qta), weight: 0);
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


        public override double Quantity(double qta)
        {
            double quantita = 0;
            int typeOfQ = 0;

            if (Printers != null)
            {
                foreach (var item in Printers)
                {
                    quantita += item.TaskCost.QuantityMaterial ?? 0;
                    this.TypeOfQuantity = item.TypeOfQuantity;
                }
            }

            if (true)
            {
                return quantita / (GainForRun ?? 1);
            }

        }

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrePostPressCostDetail to2 = (PrePostPressCostDetail)to;
            to2.WorkingFormat = this.WorkingFormat;
            to = to2;

        }

        public PrePostPressCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrePostPressCostDetail;
        }





    }





}