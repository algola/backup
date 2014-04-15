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
    public partial class Cost : IDataErrorInfo, ICloneable, IDeleteRelated
    {
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

            this.Quantity = cd.Quantity((double)this.DocumentProduct.Quantity);
            this.UnitCost = cd.UnitCost((double)this.DocumentProduct.Quantity).ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);
            var xx = Convert.ToDouble(UnitCost, Thread.CurrentThread.CurrentUICulture);
            var tot = xx * Quantity;

            this.TotalCost = (tot ?? 0).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);
        }

        #region Proprietà aggiuntive

        public bool IsSelected
        {
            get;
            set;
        }


        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""
               };

        public string Error
        {
            get
            {
                return null;
            }
        }

        public virtual string this[string proprieta]
        {
            get
            {
                string result = null;
                return result;
            }
        }

        //Check validation of entity
        public virtual bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret;
            }
        }

        #endregion

        #region Handle copy for modify

        public virtual void Copy(Cost to)
        {
            //All properties of object
            //and pointer of sons
            to.CodCost = this.CodCost;
            to.CodDocumentProduct = this.CodDocumentProduct;
            to.CodProductPartPrintableArticle = this.CodProductPartPrintableArticle;
            to.CodProductPartTask = this.CodProductPartTask;
            to.CodProductTask = this.CodProductTask;
            to.DocumentProduct = this.DocumentProduct;
            to.ProductPartsPrintableArticle = this.ProductPartsPrintableArticle;
            to.ProductPartTask = this.ProductPartTask;
            to.ProductTask = this.ProductTask;
            to.TimeStampTable = this.TimeStampTable;

            to.Description = this.Description;
            to.Quantity = this.Quantity;
            to.UnitCost = this.UnitCost;
            to.TotalCost = this.TotalCost;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Cost copyOfObject = (Cost)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example
            //DocumentProduct partCopy = (DocumentProduct)Activator.CreateInstance(copyOfObject.Prodotto.GetType());
            ////l'oggetto partCopy sarà una copia del contenuto dell'oggetto originale
            //this.Prodotto.Copia(partCopy);

            //sulla copia del prodotto in producto assegno la copia del suo prodotto
            //Example
            //copiaProdottoInProducto.Prodotto = null;
            //copiaProdotto.ProdottoInProducto = null;
            //copiaProdottoInProducto.Prodotto = copiaProdotto;
            //copiaProdotto.ProdottoInProducto.Add(copiaProdottoInProducto);
            //END COPY OF CHILD

            return copyOfObject;
        }

        public void ChildsNull()
        {
            //Set all chied to null 

            //Example
            //this.Prodotto = null;
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
                var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                //il tipo di materiale dipende dalla stampa o dal tipo di prodotto?               
                //productPartPrintabelArticles = productPart.ProductPartPrintableArticles;
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


            if (codTypeOfTask == "STAMPA")
            {

            }

            if (codTypeOfTask == "STAMPA" ||
                codTypeOfTask == "STAMPARIGIDO" ||
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

                            ((PrintingRollCostDetail)cv).BuyingWidth =
                                 (((PrintingRollCostDetail)cv).BuyingWidth == 0 || ((PrintingRollCostDetail)cv).BuyingWidth == null) ?
                                 (((PrintingRollCostDetail)cv).BuyingWidths != null) && (((PrintingRollCostDetail)cv).BuyingWidths.Count > 0) ? ((PrintingRollCostDetail)cv).BuyingWidths.FirstOrDefault() : 0
                                 : ((PrintingRollCostDetail)cv).BuyingWidth;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingRollCostDetail)cv).PrintingFormat =
                                (((PrintingRollCostDetail)cv).PrintingFormat == "" || ((PrintingRollCostDetail)cv).PrintingFormat == null) ?
                                ""
                                : ((PrintingRollCostDetail)cv).PrintingFormat;

                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
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

                            ((PrintingLabelRollCostDetail)cv).BuyingFormat =
                                 (((PrintingLabelRollCostDetail)cv).BuyingFormat == "" || ((PrintingLabelRollCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingLabelRollCostDetail)cv).BuyingFormats != null) && (((PrintingLabelRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingLabelRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingLabelRollCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingLabelRollCostDetail)cv).PrintingFormat =
                                (((PrintingLabelRollCostDetail)cv).PrintingFormat == "" || ((PrintingLabelRollCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingLabelRollCostDetail)cv).BuyingFormat
                                : ((PrintingLabelRollCostDetail)cv).PrintingFormat;


                            if (cv.TaskExecutors.FirstOrDefault() != null)
                            {
                                cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
                            }

                            cv.ProductPart = productPart;

                            ((PrintingLabelRollCostDetail)cv).FuzzyAlgo();

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
            CostDetail cv;
            cv = CostDetails.FirstOrDefault();

            #region Create new detail
            if (this.CostDetails.FirstOrDefault() == null)
            {
                if (this.CodProductPartPrintableArticle != null)
                {
                    var codDP = this.CodDocumentProduct;
                    var productPartPrintableArticle = this.ProductPartsPrintableArticle;
                    var productPart = this.ProductPartsPrintableArticle.ProductPart;
                    var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                    var cost = costsDocumentProduct.FirstOrDefault(x => x.CodProductPartTask == task.CodProductPartTask);

                    //passo alla messa in macchina del lavoro
                    cv = cost.MakeCostDetail2(tskExec, articles, costsDocumentProduct, guid);
                }
                else
                {

                    #region Materiale
                    ProductPart productPart = null;

                    if (this.CodProductPartPrintableArticle != null)
                    {
                        var productPartPrintableArticle = this.ProductPartsPrintableArticle;
                        productPart = this.ProductPartsPrintableArticle.ProductPart;

                        var task = productPart.ProductPartTasks.FirstOrDefault(x => x.OptionTypeOfTask.CodTypeOfTask.Contains("STAMPA"));

                        //il tipo di materiale dipende dalla stampa o dal tipo di prodotto?               
                        //productPartPrintabelArticles = productPart.ProductPartPrintableArticles;
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


                    if (codTypeOfTask == "STAMPA")
                    {

                    }

                    if (codTypeOfTask == "STAMPA" ||
                        codTypeOfTask == "STAMPARIGIDO" ||
                        codTypeOfTask == "STAMPAOFFeDIGITALE")
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

                                    ((PrintingRollCostDetail)cv).BuyingWidth =
                                         (((PrintingRollCostDetail)cv).BuyingWidth == 0 || ((PrintingRollCostDetail)cv).BuyingWidth == null) ?
                                         (((PrintingRollCostDetail)cv).BuyingWidths != null) && (((PrintingRollCostDetail)cv).BuyingWidths.Count > 0) ? ((PrintingRollCostDetail)cv).BuyingWidths.FirstOrDefault() : 0
                                         : ((PrintingRollCostDetail)cv).BuyingWidth;

                                    //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                                    ((PrintingRollCostDetail)cv).PrintingFormat =
                                        (((PrintingRollCostDetail)cv).PrintingFormat == "" || ((PrintingRollCostDetail)cv).PrintingFormat == null) ?
                                        ""
                                        : ((PrintingRollCostDetail)cv).PrintingFormat;

                                    if (cv.TaskExecutors.FirstOrDefault() != null)
                                    {
                                        cv.CodTaskExecutorSelected = tskExec.FirstOrDefault().CodTaskExecutor;
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
                    }

                    #endregion

                    cv.ProductPart = productPart;

                    cv.TaskCost = this;
                    cv.CodCost = this.CodCost;
                    cv.CodCostDetail = this.CodCost;

                }

            }
            #endregion

            else
            {

            }

            cv.Update();

            //  this.CostDetails.Add(cv);
            return cv;
        }

    }
}
