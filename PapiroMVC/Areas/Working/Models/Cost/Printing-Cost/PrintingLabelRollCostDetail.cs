using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingLabelRollCostDetail : PrintingCostDetail
    {

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
                        PrintingFormat = x[k].GetCuttedFormat(BuyingFormat)) ;
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
                    var ele = x.Find(z => z.CutName == PrintingFormat);
                    if (ele == null && PrintingFormat != null)
                    {
                        var toAdd = new Cut("manual", 0, 0);
                        toAdd.ManualFormat = PrintingFormat;
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat);
                        x.Add(toAdd);
                    }
                }

                return x;
            }
        }

        public PrintingLabelRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingLabelRollCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }

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
                        lst.Add(new ProductFormatName { FormatName = "h" + bF.GetSide1() + " z" + (bF.GetSide2() / 2.54 * 8).ToString(), CodFormat = bF });
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
                    case ProductPart.ProductPartType.ProductPartSingleLabelRoll:
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
            foreach (var buyingFormat in BuyingFormats)
            {
                ppP.CostDetail = this;

                ppP.Part = this.ProductPart;
                ppP.PrintingFormat = buyingFormat;
                ppP.Update();

                if (ppP.CalculatedDCut2 >= 0.2 && ppP.CalculatedDCut2 <= 0.6 &&
                    ((ppP.CalculatedDCut1 >= ppP.CalculatedDCut2)||ppP.CalculatedSide1Gain==1))
                {
                    pHint.Add(new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ppP.CalculatedDCut1,
                        DCut2 = ppP.CalculatedDCut2,
                        BuyingFormat = buyingFormat,
                        PrintingFormat = buyingFormat
                    });
                }
            }

            List<string> aux = pHint.Select(z => z.BuyingFormat).ToList();
            List<double> zValids = new List<double>();

            //scorro la lista ed estraggo solo gli z validi
            foreach (var item in aux)
            {
                var z = item.GetSide2();
                if (!zValids.Contains(z))
                {
                    zValids.Add(z);
                }
            }

            //bande di carta calcolate semplicemente con la resa, dati gli z validi e il maxWidth della macchina Flexo

            foreach (var item in zValids)
            {
                double dCut1 = 0;
                double dCut2 = 0;
                var newBands = this.GetOptimalWidthFlexo(item, ppP.Part.Format, dCut1, dCut2, 34);
                foreach (var nB in newBands)
                {
                    ppP.PrintingFormat = nB.ToString() + "x" + item.ToString();
                    ppP.Update();
                    pHint.Add(new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ppP.CalculatedDCut1,
                        DCut2 = ppP.CalculatedDCut2,
                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat
                    });
                }
            }

            PrintingHints = pHint;
            BuyingFormats = pHint.Select(x => x.BuyingFormat).ToList();

            if (BuyingFormats.Count == 0)
            {
                //no format
                Error = 3;
            }

        }


        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
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
                               x.NameOfMaterial == item.NameOfMaterial)
                                   .Select(x => x.Width).ToList();
            }


            //**********************************************
            var tskCurrent = tskExec.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
            if (tskCurrent == null)
            {
                tskCurrent = tskExec.FirstOrDefault();
            }

            var zList = new List<string>();

            foreach (var item in tskCurrent.TaskExecutorCylinders)
            {
                zList.Add((item.Z / 8 * 2.54).ToString());
            }

            //combino i width con gli Z

            foreach (var width in widthList)
            {
                foreach (var z in zList)
                {
                    var x = (width ?? 0).ToString("0.00", Thread.CurrentThread.CurrentUICulture) + "x" + z;
                    if (!formats.Contains(x))
                        formats.Add((width ?? 0).ToString("0.00", Thread.CurrentThread.CurrentUICulture) + "x" + z);
                }
            }

            BuyingFormats = formats;
            //**********************************************

            #endregion
        }


        public override List<PrintedArticleCostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<PrintedArticleCostDetail> lst = new List<PrintedArticleCostDetail>();

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

        public List<double> GetOptimalWidthFlexo(double z, string SmallerFormat, double DCut1, double DCut2, double maxSide1)
        {

            var taskexEcutorSelected = TaskExecutors.SingleOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);

            double pinza = taskexEcutorSelected.Pinza ?? 0;
            double controPinza = taskexEcutorSelected.ControPinza ?? 0;
            double laterale = taskexEcutorSelected.Laterale ?? 0;

            var GiraVerso = true;

            double minusSide1 = GiraVerso ? laterale * 2 : pinza + controPinza;
            var ddpminusSide1 = GiraVerso ? laterale * 2 : pinza * 2;
            var minusSide2 = GiraVerso ? pinza + controPinza : laterale * 2;

            var res = new List<double>();

            try
            {
                int gain2_2 = (int)decimal.Truncate((decimal)((
                    (z - minusSide2) / (SmallerFormat.GetSide2())
                    )));

                //doppio taglio calcolato su SideOnSide 2
                double dCut2_2Res = ((z) - (SmallerFormat.GetSide2() * (gain2_2))) / (gain2_2);

                int gain1_1 = 1;
                while (

                    Math.Ceiling(
                    (gain1_1 * SmallerFormat.GetSide1() +
                    (gain1_1 - 1) * dCut2_2Res +
                    minusSide1) * 100
                    ) / 100

                    <= maxSide1)
                {
                    res.Add(Math.Ceiling(
                    (gain1_1 * SmallerFormat.GetSide1() +
                    (gain1_1 - 1) * dCut2_2Res +
                    minusSide1) * 100
                    ) / 100);

                    gain1_1++;
                }

                DCut1 = dCut2_2Res;
                DCut2 = DCut1;
            }
            catch (Exception)
            {
            }
            return res;
        }
    }
}