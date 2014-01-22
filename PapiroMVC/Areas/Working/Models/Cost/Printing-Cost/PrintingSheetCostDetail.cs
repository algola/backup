using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrintingSheetCostDetail : PrintingCostDetail
    {

        public List<Cut> Cuts
        {
            get
            {
                var tsk = TaskExecutors.Where(it => it.CodTaskExecutor == CodTaskExecutorSelected).FirstOrDefault();

                var y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin);
                List<Cut> x = new List<Cut>();

                x = y.Where(k => k.Valid).ToList();

                if (PrintingFormat != null && PrintingFormat != "")
                {
                    var k = 0;
                    for (int i = 0; i < x.Count &&
                        !SheetCut.IsValid(tsk.FormatMax, tsk.FormatMin, PrintingFormat);
                        i++,
                        PrintingFormat = x[k].GetCuttetFormat(BuyingFormat)) ;
                }

                foreach (var item in x)
                {
                    item.CutName = item.GetCuttetFormat(BuyingFormat);
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
                        toAdd.CutName = toAdd.GetCuttetFormat(BuyingFormat);
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
                        this.ProductPartPrinting = new ProductPartRigidPrinting();
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
                                    x.NameOfMaterial == item.NameOfMaterial)
                                        .Select(x => x.Format);

                    formats = formats.Union(formatList.ToList()).ToList();

                }

                if (CurTskE.TypeOfExecutor == TaskExecutor.ExecutorType.PlotterSheet)
                {
                    var formatList = articles.OfType<RigidPrintableArticle>()
                                .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                                    x.Color == item.Color &&
                                    x.NameOfMaterial == item.NameOfMaterial)
                                        .Select(x => x.Format);

                    formats = formats.Union(formatList.ToList()).ToList();
                }

            }

            BuyingFormats = formats;

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
                    var cost = costs.Where(pp => pp.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).FirstOrDefault();
                    //da non usare MAIIII                    x.TaskCost = cost;
                    x.CodCost = cost.CodCost;
                    x.CodCostDetail = cost.CodCost;

                    x.CostDetailCostCodeRigen();

                }

                this.Computes.Add(x);
                lst.Add(x);


            }

            return lst;
        }

    }

}