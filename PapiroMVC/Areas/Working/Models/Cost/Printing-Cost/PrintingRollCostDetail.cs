using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingRollCostDetail : PrintingCostDetail
    {


        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintingRollCostDetail to2 = (PrintingRollCostDetail)to;

            to2.BuyingFormat = this.BuyingFormat;

            to = to2;

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

                //sperimentale potrebbe essere inserita nella procedura anche il controllo della doppia pinza, etc...
                if (SheetCut.IsValid(tsk.FormatMax, ProductPart.FormatOpened, tsk.FormatMin))
                {
                    //i tagli che vanno bene nel formato minimo e massimo
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin);
                }
                else
                {
                    //i tagli che vanno bene nel formato minimo e formato lavoro
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, ProductPart.FormatOpened);
                }


                //ma solo quelli validi
                List<Cut> x = new List<Cut>();
                x = y.Where(k => k.Valid).ToList();

                if (PrintingFormat != null && PrintingFormat != "")
                {
                    var k = 0;
                    for (int i = 0; i < x.Count &&
                        !SheetCut.IsValid(tsk.FormatMax, tsk.FormatMin, PrintingFormat);
                        i++,
                        PrintingFormat = x[k].GetCuttedFormat(BuyingFormat,false)) ;
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
                    var ele = x.Find(z => z.CutName == PrintingFormat);
                    if (ele == null && PrintingFormat != null)
                    {
                        var toAdd = new Cut("manual", 0, 0);
                        toAdd.ManualFormat = PrintingFormat;
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat,false);
                        x.Add(toAdd);
                    }
                }

                return x;
            }
        }

        public PrintingRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingRollCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<double> BuyingWidths { get; set; }


        //il tipo è ingannevole... in realtà serve per proporre un'associazione tra nome e formato immediata
        public List<ProductFormatName> BuyingFormatsName
        {
            get
            {
                var lst = new List<ProductFormatName>();
                var curTsk = this.TaskExecutors.SingleOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                if (curTsk.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
                {
                    foreach (var bF in BuyingFormats)
                    {
                        lst.Add(new ProductFormatName { FormatName = "h" + bF.GetSide1() + " z" + (((Flexo)curTsk).GetZFromCm(bF.GetSide2() / 2.54 * 8)).ToString(), CodFormat = bF });
                    }
                }
                else
                {
                    foreach (var bF in BuyingFormats)
                    {
                        lst.Add(new ProductFormatName { CodFormat = bF, FormatName = bF });
                    }
                }

                return lst;
            }

        }

        //every changes fire this update
        public override void Update()
        {

            if (ProductPartPrinting == null)
            {
                switch (ProductPart.TypeOfProductPart)
                {
                    case ProductPart.ProductPartType.ProductPartSoft:
                        this.HideBuyingInView = true;
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();
                        break;
                    default:
                        throw new Exception();
                        break;
                }
            }

            this.ProductPartPrinting.CostDetail = this;

            if (GainPrintingOnBuying == null)
            {
                GainPrintingOnBuying = new ProductPartPrintingSheetGainSingle();
            }

            //la stampa è condizionata al fatto che il PrintingFormat stia in macchina!!!
            //quindi è da scegliere tra i possibili formati "spaccati" dal formato acquisto
            //la lettura della proprietà Cuts ricalcola il PrintingFormat!!!!
            var fake = this.Cuts;

            //i formati di stampa devono essere condizionati anche dal formato del lavoro da stampare
            //dalla pinza e dalla contropinza e laterale!!!!

            if (BuyingFormats == null)
            {
                Error = 3;
                BuyingFormats = new List<String>();
                BuyingFormats.Add(BuyingFormat);

            }

            if (BuyingFormats.Count == 0)
            {
                Error = 3;
            }
            else
            {
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).LargerFormat = this.BuyingFormat;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SmallerFormat = this.PrintingFormat;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).Quantity = 1;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SubjectNumber = 1;

                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).CalculateGain();

                //devo anche rifare la messa in macchina della parte!!!
                if (this.ProductPartPrinting != null)
                {
                    this.ProductPartPrinting.Part = this.ProductPart;
                    this.ProductPartPrinting.PrintingFormat = this.PrintingFormat;

                    this.ProductPartPrinting.Update();
                }
            }

            this.UpdateCoeff();

        }


        public List<PrintingHint> PrintingHints
        {
            get;
            set;
        }

        public void FuzzyAlgo()
        {
            List<String> newBuyingFormats = new List<string>();

            var pHint = new List<PrintingHint>();

            var ppP = new ProductPartSingleSheetPrinting();

            //bande di carta calcolate semplicemente con la resa, dati gli z validi e il maxWidth della macchina Flexo
            foreach (var item in BuyingWidths)
            {
                ppP.CostDetail = this;
                ppP.Part = this.ProductPart;
                ppP.AutoCutParameter = true;
                ppP.PrintingFormat =    item.ToString()+ "x" +ProductPart.Format.GetSide1();                
                ppP.Update();
                if (ppP.CalculatedGain > 0)
                {
                    pHint.Add(new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ppP.CalculatedDCut1,
                        DCut2 = ppP.CalculatedDCut2,
                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat,
                        Description = ppP.PrintingFormat,
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain

                    });
                }

                if (ProductPart.Format.GetSide1() != ProductPart.Format.GetSide2())
                {
                    ppP.CostDetail = this;
                    ppP.Part = this.ProductPart;
                    ppP.AutoCutParameter = true;
                    ppP.PrintingFormat = item.ToString() + "x" + ProductPart.Format.GetSide2();
                    ppP.Update();
                    if (ppP.CalculatedGain > 0)
                    {
                        pHint.Add(new PrintingHint
                        {
                            Format = ppP.Part.Format,
                            DCut1 = ppP.CalculatedDCut1,
                            DCut2 = ppP.CalculatedDCut2,
                            BuyingFormat = ppP.PrintingFormat,
                            PrintingFormat = ppP.PrintingFormat,
                            Description = ppP.PrintingFormat,
                            CalculatedGain = ppP.CalculatedGain,
                            GainOnSide1 = ppP.CalculatedSide1Gain,
                            GainOnSide2 = ppP.CalculatedSide2Gain

                        });

                    }
                }

            }

            PrintingHints = pHint;
            BuyingFormats = pHint.Select(x => x.BuyingFormat).ToList();

            if (BuyingFormats.Count == 0)
            {
                //no format
                Error = 3;
                BuyingFormats.Add(ppP.Part.Format);
            }
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            ////calcoli per mt lineari!!!!!!!!
            //var paperFirstStartLenght = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
            //var paperSecondStartLenght = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;

            //var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            ////mul with cm GetSide2 Printing Format
            //var mtRuns = runs * PrintingFormat.GetSide2() / 100;

            ////i 2000 dovrebbero essere rapportati alla carta!!!
            //RollChanges = Math.Truncate((mtRuns + paperFirstStartLenght ?? 0) / 2000);

            ////calcolo di quanti impianti sono necessari!!!!
            //Implants = TaskexEcutorSelected.Implants(TaskCost.ProductPartTask.CodOptionTypeOfTask);

            //var mtWaste = (paperFirstStartLenght ?? 0) + (RollChanges * paperSecondStartLenght ?? 0);


        }

        public override double Quantity(double qta, CostDetail.QuantityType type = CostDetail.QuantityType.NOTypeOfQuantity)
        {

            var typeOfQuantity = type == CostDetail.QuantityType.NOTypeOfQuantity ? TypeOfQuantity : (Nullable<int>)type;

            double ret = 0;

            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.MqWorkTypeOfQuantity:
                    ret = base.Quantity(qta);
                    break;

                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            if (!justInited)
            {
                base.InitCostDetail(tskExec, articles);

                String codTypeOfTask = String.Empty;
                Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
                codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
                tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);
                TaskExecutors = tskExec.ToList();

                ICollection<ProductPartsPrintableArticle> productPartPrintabelArticles = new List<ProductPartsPrintableArticle>();
                productPartPrintabelArticles = TaskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles;

                #region Format
                List<string> formats = new List<string>();
                var widthList = new List<Nullable<double>>();

                //
                //voglio sapere quali sono i formati degli articoli ma gli articoli che posso stampare dipendono dal tipo di macchina!!!!
                foreach (var item in productPartPrintabelArticles)
                {
                    widthList = articles.OfType<RollPrintableArticle>()
                               .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                   x.Color == item.Color &&
                                   x.Adhesive == item.Adhesive &&
                                   x.Weight == item.Weight &&
                                   x.NameOfMaterial == item.NameOfMaterial)
                                       .Select(x => x.Width).ToList();
                }

                foreach (var width in widthList)
                {
                    if (BuyingWidths == null) BuyingWidths = new List<double>();
                    BuyingWidths.Add(width ?? 0);
                }

                #endregion
                
            }
        }

        public override List<CostDetail> CreateRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

            //per ciascun materiale stampabile...
            foreach (var item in this.ProductPart.ProductPartPrintableArticles)
            {

                var xx = articles.GetArticlesByProductPartPrintableArticle(item).FirstOrDefault();

                PrintedArticleCostDetail x;

                switch (xx.TypeOfArticle)
                {
                    case Article.ArticleType.SheetPrintableArticle:
                        x = new PrintedSheetArticleCostDetail();
                        break;
                    case Article.ArticleType.RollPrintableArticle:
                        x = new PrintedRollArticleCostDetail();
                        break;
                    case Article.ArticleType.RigidPrintableArticle:
                        x = new PrintedRigidArticleCostDetail();
                        break;
                    case Article.ArticleType.ObjectPrintableArticle:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }

                x.ComputedBy = this;
                x.ProductPart = this.ProductPart;

                //devo pescare il costo e associarlo al dettaglio
                if (x.CodCost == null)
                {
                    var xxxx = costs.ToList();

                    var cost = costs.Where(pp => pp.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).FirstOrDefault();
                    //da non usare MAIIII                    x.TaskCost = cost;
                    x.CodCost = cost.CodCost;
                    x.CodCostDetail = cost.CodCost;

                    x.CostDetailCostCodeRigen();

                }

                //GUID
                x.Guid = this.Guid;
                this.Computes.Add(x);
                lst.Add(x);
            }

            return lst;
        }

    }
}