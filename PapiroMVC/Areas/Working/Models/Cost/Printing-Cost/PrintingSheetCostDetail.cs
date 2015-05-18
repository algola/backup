using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingSheetCostDetail : PrintingCostDetail
    {


        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintingSheetCostDetail to2 = (PrintingSheetCostDetail)to;

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

        public PrintingSheetCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingSheetCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }

        //every changes fire this update
        public override void Update()
        {
            if (ProductPartPrinting == null)
            {
                switch (ProductPart.TypeOfProductPart)
                {
                    case ProductPart.ProductPartType.ProductPartSingleSheet:
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartCoverSheet:
                        this.ProductPartPrinting = new ProductPartCoverSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartBookSheet:
                        this.ProductPartPrinting = new ProductPartBookSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartBlockSheet:
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartRigid:
                        this.HideBuyingInView = true;
                        this.ProductPartPrinting = new ProductPartRigidPrinting();
                        break;
                    case ProductPart.ProductPartType.ProductPartSoft:
                        this.HideBuyingInView = true;
                        this.ProductPartPrinting = new ProductPartSoftPrinting();
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

            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).LargerFormat = this.BuyingFormat;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SmallerFormat = this.PrintingFormat;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).Quantity = 0;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SubjectNumber = 0;
            ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).CalculateGain();

            //devo anche rifare la messa in macchina della parte!!!
            if (this.ProductPartPrinting != null)
            {
                this.ProductPartPrinting.Part = this.ProductPart;
                this.ProductPartPrinting.PrintingFormat = this.PrintingFormat;

                this.ProductPartPrinting.Update();
            }

            UpdateCoeff();
        }


        //nuovo appena inserito per calcolo dei coefficienti per il piano
        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            long paperFirstStartL = 0;
            long paperSecondStart = 0;

            //questi valori dipendono da quanti sono i colori
            try
            {
                paperFirstStartL = ((PrinterMachine)TaskexEcutorSelected).ProofSheetFirstStart??0;
                paperSecondStart = ((PrinterMachine)TaskexEcutorSelected).ProofSheetSecondsStart??0;
            }
            catch (Exception)
            {
            }

            var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            RollChanges = 0;

            //calcolo di quanti impianti sono necessari!!!!
            Implants = TaskexEcutorSelected.GetImplants(TaskCost.ProductPartTask.CodOptionTypeOfTask);

            var fgWaste = paperFirstStartL + (RollChanges * 0 ?? 0); //lo zero va sostituito con i cambi lastra!!!!

        }

        public override double Quantity(double qta)
        {

            //mi serve calcolare la quantità con gli scarti!!!


            if (TaskexEcutorSelected == null)
            {
                UpdateCoeff();
            }

            double ret;
            double mqMat = 0;
            double mlMat = 0;
            double runMat = 0;
            double kgMat = 0;

            Nullable<long> proof1=0;
            Nullable<long> proof2=0;

            try
            {
                proof1 = ((PrinterMachine)TaskexEcutorSelected).ProofSheetFirstStart;
                proof2 = ((PrinterMachine)TaskexEcutorSelected).ProofSheetSecondsStart;
            }
            catch (Exception)
            {
            }

            var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            var waste = (proof1 ?? 0) + ((Starts - 1) * proof2 ?? 0);

            CalculatedMl = 0;

            mqMat = Math.Ceiling((runs + waste) * ProductPart.Format.GetSide1() * ProductPart.Format.GetSide2() / 10000);
            CalculatedMq = mqMat;

            kgMat = 0;
            CalculatedKg = kgMat;

            CalculatedRun = runs + waste;

            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.RunTypeOfQuantity:

                    ret = CalculatedRun ?? 0;
                    break;

                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;
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
            //
            //voglio sapere quali sono i formati degli articoli ma gli articoli che posso stampare dipendono dal tipo di macchina!!!!
            foreach (var item in productPartPrintabelArticles)
            {
                //accomunano lo stesso tipo!!!
                var CurTskE = tskExec.FirstOrDefault();

                if (CurTskE.TypeOfExecutor == TaskExecutor.ExecutorType.LithoSheet ||
                    CurTskE.TypeOfExecutor == TaskExecutor.ExecutorType.DigitalSheet)
                {
                    var formatList = articles.OfType<SheetPrintableArticle>()
                                .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                    x.Color == item.Color &&
                                    x.Adhesive == item.Adhesive &&
                                    x.NameOfMaterial == item.NameOfMaterial)
                                        .Select(x => x.Format);

                    formats = formats.Union(formatList.ToList()).ToList();

                }

                if (CurTskE.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterSheet)
                {
                    var formatList = articles.OfType<RigidPrintableArticle>()
                                .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                    x.Color == item.Color &&
                                    x.Adhesive == item.Adhesive &&
                                    x.NameOfMaterial == item.NameOfMaterial)
                                        .Select(x => x.Format);

                    formats = formats.Union(formatList.ToList()).ToList();
                }

            }

            BuyingFormats = formats;

            #endregion
        }

        public override List<CostDetail> CreateRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

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