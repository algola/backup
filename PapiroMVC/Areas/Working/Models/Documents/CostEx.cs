using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using PapiroMVC.Validation;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Cost_MetaData))]
    public partial class Cost : ICloneable
    {

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Cost copyOfObject = (Cost)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(Cost to)
        {


            to.TimeStampTable = this.TimeStampTable;
            to.CodCost = this.CodCost;
            //        to.CodDocumentProduct = this.    	 		
            to.CodProductTask = this.CodProductTask;
            to.CodProductPartTask = this.CodProductPartTask;
            to.CodProductPartPrintableArticle = this.CodProductPartPrintableArticle;
            to.Description = this.Description;
            to.Quantity = this.Quantity;
            to.UnitCost = this.UnitCost;
            to.TotalCost = this.TotalCost;
            to.ForceZero = this.ForceZero;
            to.Hidden = this.Hidden;
            to.CodProductPartImplantTask = this.CodProductPartImplantTask;
            to.Locked = this.Locked;
            to.CodItemGraph = this.CodItemGraph;
            to.Markup = this.Markup;
            to.GranTotalCost = this.GranTotalCost;
            to.TypeOfCalcolous = this.TypeOfCalcolous;

            //   to.DocumentProduct = this.DocumentProduct;

            to.ProductPartsPrintableArticle = this.ProductPartsPrintableArticle;
            to.ProductPartTask = this.ProductPartTask;
            to.ProductTask = this.ProductTask;

            to.ProductPartImplantTask = this.ProductPartImplantTask;

            //foreach (var costDet in CostDetails)
            //{
            //    var cd= (CostDetail) costDet.Clone();
            //    //cd.TaskCost=to;
            //    to.CostDetails.Add(cd);
            //}

        }

        /// <summary>
        /// update cost quantity and so other stuff
        /// </summary>
        public virtual void Update()
        {
            var cd = CostDetails.FirstOrDefault();

            if (cd == null)
            {
                throw new Exception();
            }

            //rigenera i coefficienti del dettaglio
            cd.UpdateCoeff();

            if (!(this.Locked ?? false))
            {
                this.Quantity = cd.Quantity((double)this.DocumentProduct.Quantity);
                this.UnitCost = cd.UnitCost((double)this.DocumentProduct.Quantity).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                var xx = Convert.ToDouble(UnitCost, Thread.CurrentThread.CurrentUICulture);
                var tot = xx * Quantity;

                this.TotalCost = (tot ?? 0).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

                this.GranTotalCost = (Convert.ToDouble(this.TotalCost, Thread.CurrentThread.CurrentUICulture) +
               (Convert.ToDouble(this.TotalCost, Thread.CurrentThread.CurrentUICulture) *
               ((this.Markup ?? 1) / 100))).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

                this.Hidden = (cd.TypeOfQuantity == (int)CostDetail.QuantityType.NOTypeOfQuantity);
            }

        }

        #region Proprietà aggiuntive

        public bool IsSelected
        {
            get;
            set;
        }


        #endregion

        public CostDetail MakeCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            CostDetail cv = null;
            ProductPart productPart = null;

            #region Materiale

            if (this.CodProductPartPrintableArticle != null)
            {
                var productPartPrintableArticle = this.ProductPartsPrintableArticle;
                productPart = this.ProductPartsPrintableArticle.ProductPart;

                var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                //il tipo di materiale dipende dalla stampa o dal tipo di prodotto?               
                //productPartPrintabelArticles = productPart.ProductPartPrintableArticles;
            }

            #endregion

            #region Impianto
            if (this.CodProductPartImplantTask != null)
            {
                productPart = this.ProductPartImplantTask.ProductPart;
                //                var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));
                var task = productPart.ProductPartTasks.FirstOrDefault(x => x.CodProductPartTask == this.CodProductPartImplantTask);

            }


            #endregion

            #region Lavorazione
            //E' una lavorazione!!!!
            String codTypeOfTask = String.Empty;
            if (this.CodProductPartTask != null)
            {
                codTypeOfTask = this.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
                productPart = this.ProductPartTask.ProductPart;
            }


            if (codTypeOfTask == "TAVOLOCONTROLLO")
            {
                Console.WriteLine("Tavolo di controllo");
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

                tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);

                if (tskExec.Count() > 0)
                {

                    switch (tskExec.FirstOrDefault().TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.ControlTableRoll:
                            cv = new PrePostPressCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);

                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            break;

                        default:
                            break;

                    }

                    cv.ProductPart = productPart;

                    cv.CodCost = this.CodCost;
                    cv.CodCostDetail = this.CodCost;

                }

            }

            if (codTypeOfTask == "STAMPA" ||
                codTypeOfTask == "STAMPARIGIDO" ||
                codTypeOfTask == "STAMPAMORBIDO" ||
                codTypeOfTask == "STAMPAOFFeDIGITALE" ||
                codTypeOfTask == "STAMPAETICHROTOLO")
            {
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

                tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);

                if (tskExec.Count() > 0)
                {

                    switch (tskExec.FirstOrDefault().TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.LithoSheet:
                            cv = new PrintingSheetCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);

                            ((PrintingSheetCostDetail)cv).BuyingFormat =
                                 (((PrintingSheetCostDetail)cv).BuyingFormat == "" || ((PrintingSheetCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingSheetCostDetail)cv).BuyingFormats != null) && (((PrintingSheetCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingSheetCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingSheetCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingSheetCostDetail)cv).PrintingFormat =
                                (((PrintingSheetCostDetail)cv).PrintingFormat == "" || ((PrintingSheetCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingSheetCostDetail)cv).BuyingFormat
                                : ((PrintingSheetCostDetail)cv).PrintingFormat;



                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            break;
                        case TaskExecutor.ExecutorType.LithoRoll:
                            break;
                        case TaskExecutor.ExecutorType.DigitalSheet:
                            cv = new PrintingSheetCostDetail();
                            cv.TaskExecutors = tskExec.ToList();
                            cv.TaskCost = this;
                            break;
                        case TaskExecutor.ExecutorType.DigitalRoll:
                            break;
                        case TaskExecutor.ExecutorType.PlotterRoll:

                            cv = new PrintingRollCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);


                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            ((PrintingRollCostDetail)cv).BuyingFormat =
                                 (((PrintingRollCostDetail)cv).BuyingFormat == "" || ((PrintingRollCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingRollCostDetail)cv).BuyingFormats != null) && (((PrintingRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingRollCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingRollCostDetail)cv).PrintingFormat =
                                (((PrintingRollCostDetail)cv).PrintingFormat == "" || ((PrintingRollCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingRollCostDetail)cv).BuyingFormat
                                : ((PrintingRollCostDetail)cv).PrintingFormat;


                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            cv.ProductPart = productPart;

                            ((PrintingRollCostDetail)cv).FuzzyAlgo();

                            Console.WriteLine(((PrintingRollCostDetail)cv).BuyingFormats);


                            if (((PrintingRollCostDetail)cv).PrintingFormat == null)
                            {

                                ((PrintingRollCostDetail)cv).BuyingFormat =
                                     (((PrintingRollCostDetail)cv).BuyingFormat == "" || ((PrintingRollCostDetail)cv).BuyingFormat == null) ?
                                     (((PrintingRollCostDetail)cv).BuyingFormats != null) && (((PrintingRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                     : ((PrintingRollCostDetail)cv).BuyingFormat;

                                //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                                ((PrintingRollCostDetail)cv).PrintingFormat =
                                    (((PrintingRollCostDetail)cv).PrintingFormat == "" || ((PrintingRollCostDetail)cv).PrintingFormat == null) ?
                                    ((PrintingRollCostDetail)cv).BuyingFormat
                                    : ((PrintingRollCostDetail)cv).PrintingFormat;
                            }

                            break;


                        case TaskExecutor.ExecutorType.PlotterSheet:
                            cv = new PrintingSheetCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);

                            ((PrintingSheetCostDetail)cv).BuyingFormat =
                                 (((PrintingSheetCostDetail)cv).BuyingFormat == "" || ((PrintingSheetCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingSheetCostDetail)cv).BuyingFormats != null) && (((PrintingSheetCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingSheetCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingSheetCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingSheetCostDetail)cv).PrintingFormat =
                                (((PrintingSheetCostDetail)cv).PrintingFormat == "" || ((PrintingSheetCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingSheetCostDetail)cv).BuyingFormat
                                : ((PrintingSheetCostDetail)cv).PrintingFormat;

                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            break;


                        case TaskExecutor.ExecutorType.Flexo:
                            cv = new PrintingLabelRollCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);


                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }



                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            cv.ProductPart = productPart;

                            ((PrintingLabelRollCostDetail)cv).FuzzyAlgo();


                            ((PrintingLabelRollCostDetail)cv).BuyingFormat =
                                 (((PrintingLabelRollCostDetail)cv).BuyingFormat == "" || ((PrintingLabelRollCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingLabelRollCostDetail)cv).BuyingFormats != null) && (((PrintingLabelRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingLabelRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingLabelRollCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingLabelRollCostDetail)cv).PrintingFormat =
                                (((PrintingLabelRollCostDetail)cv).PrintingFormat == "" || ((PrintingLabelRollCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingLabelRollCostDetail)cv).BuyingFormat
                                : ((PrintingLabelRollCostDetail)cv).PrintingFormat;



                            break;

                        case TaskExecutor.ExecutorType.PrePostPress:
                            break;
                        case TaskExecutor.ExecutorType.Binding:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    //
                    var e = new NoTaskExecutorException();
                    e.Data.Add("CodTypeOfTask", codTypeOfTask);
                    throw e;

                }


                cv.ProductPart = productPart;


                cv.TaskCost = this;
                cv.CodCost = this.CodCost;
                cv.CodCostDetail = this.CodCost;
            }

            #endregion

            return cv;
        }


        public CostDetail MakeCostDetail2(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles, IQueryable<Cost> costsDocumentProduct, Guid guid)
        {
            return null;
        }

    }
}
