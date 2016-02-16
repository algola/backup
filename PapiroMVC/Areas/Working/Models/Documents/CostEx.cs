using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using PapiroMVC.Validation;
using Novacode;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Cost_MetaData))]
    public partial class Cost : ICloneable, IPrintDocX
    {


        /// <summary>
        /// use it in PrintingZRollCostDetail to get 
        /// </summary>
        public ProductPartPrintRoll ProductPartPrintRoll
        {
            get
            {
                if (ProductPartTask.TypeOfProductPartTask == Models.ProductPartTask.ProductPartTasksType.ProductPartPrintRoll)
                {
                    return (ProductPartPrintRoll)ProductPartTask;
                }
                else
                {
                    return null;
                }

            }

        }


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
            to.QuantityMaterial = this.QuantityMaterial;
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

            to.Manual = this.Manual;

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
                this.QuantityMaterial = cd.QuantityMaterial((double)this.DocumentProduct.Quantity);

                double unitCost = cd.UnitCost((double)this.DocumentProduct.Quantity);
                this.UnitCost = unitCost.ToString("#,0.000", Thread.CurrentThread.CurrentUICulture);

                var tot = unitCost * Quantity;

                this.TotalCost = (tot ?? 0).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

                this.GranTotalCost = (Convert.ToDouble(this.TotalCost, Thread.CurrentThread.CurrentUICulture) +
               (Convert.ToDouble(this.TotalCost, Thread.CurrentThread.CurrentUICulture) *
               ((this.Markup ?? 0) / 100))).ToString("#,0.00", Thread.CurrentThread.CurrentUICulture);

                //OLD
                //if (!(this.Hidden ?? false))
                //    this.Hidden = (cd.TypeOfQuantity == (int)CostDetail.QuantityType.NOTypeOfQuantity);

                //NEW
                this.Hidden = (cd.TypeOfQuantity == (int)CostDetail.QuantityType.NOTypeOfQuantity);

                //se il costo viene da una lav, controllo l'option

                if (ProductTask != null)
                {
                    if (ProductTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        Hidden = true;
                        ForceZero = true;
                    }
                }

                if (ProductPartImplantTask != null)
                {
                    if (ProductPartImplantTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        Hidden = true;
                        ForceZero = true;
                    }                    
                }

                if (ProductPartTask != null)
                {
                    if (ProductPartTask.CodOptionTypeOfTask.Contains("_NO"))
                    {
                        Hidden = true;
                        ForceZero = true;
                    }
                }
            }

        }

        #region Proprietà aggiuntive

        public bool IsSelected
        {
            get;
            set;
        }


        #endregion

        public CostDetail MakeCostDetail(IQueryable<TaskExecutor> tskExecs, IQueryable<Article> articles, string codTaskExecutor = "")
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

            #region tavolo di controllo rotolo
            if (codTypeOfTask == "TAVOLOCONTROLLO")
            {
                Console.WriteLine("Tavolo di controllo");
                String codParte = String.Empty;

                tskExecs = TaskExecutor.FilterByTask(tskExecs, codTypeOfTask);

                if (tskExecs.Count() > 0)
                {

                    switch (tskExecs.FirstOrDefault().TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.ControlTableRoll:
                            cv = new ControlTableCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExecs, articles);
                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            break;

                        default:
                            break;

                    }

                    cv.ProductPart = productPart;

                    cv.CodCost = this.CodCost;
                    cv.CodCostDetail = this.CodCost;

                }

            }
            #endregion


            #region serigrafia rotolo!!! ripasso!!!
            if (codTypeOfTask == "SERIGRAFIAROTOLO" || codTypeOfTask == "STAMPAACALDOROTOLO")
            {
                Console.WriteLine("Serigrafia rotolo");
                String codParte = String.Empty;

                tskExecs = TaskExecutor.FilterByTask(tskExecs, codTypeOfTask);

                if (tskExecs.Count() > 0)
                {

                    switch (tskExecs.FirstOrDefault().TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.FlatRoll:
                            cv = new RepassRollCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExecs, articles);
                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            break;

                        default:
                            break;

                    }

                    cv.ProductPart = productPart;

                    cv.CodCost = this.CodCost;
                    cv.CodCostDetail = this.CodCost;

                }

            }
            #endregion

            
            
            #region fustellatura
            if (codTypeOfTask == "FUSTELLATURA" 
                || codTypeOfTask == "TAGLIO"
                || codTypeOfTask == "PLASTIFICATURA"                
                )
            {
                Console.WriteLine("Fustellatura");
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

                tskExecs = TaskExecutor.FilterByTask(tskExecs, codTypeOfTask);

                if (tskExecs.Count() > 0)
                {

                    cv = new PrePostPressCostDetail();
                    cv.TaskCost = this;
                    cv.InitCostDetail(tskExecs, articles);

                    if (cv.TaskExecutors != null)
                    {
                        cv.CodTaskExecutorSelected = tskExecs.FirstOrDefault().CodTaskExecutor;
                    }

                    cv.ProductPart = productPart;

                    cv.CodCost = this.CodCost;
                    cv.CodCostDetail = this.CodCost;

                }

            }
            #endregion

            if (codTypeOfTask == "STAMPA" ||
                codTypeOfTask == "STAMPARIGIDO" ||
                codTypeOfTask == "STAMPAMORBIDO" ||
                codTypeOfTask == "STAMPAOFFeDIGITALE" ||
                codTypeOfTask == "STAMPANEW" ||
                codTypeOfTask == "STAMPAETICHROTOLO")
            {
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

                tskExecs = TaskExecutor.FilterByTask(tskExecs, codTypeOfTask);

                //search selected taskexecutor
                var tskFirst = tskExecs.Where(x => x.CodTaskExecutor == codTaskExecutor).FirstOrDefault();

                if (tskExecs.Count() > 0)
                {
                    switch ((tskFirst != null ? tskFirst : tskExecs.FirstOrDefault()).TypeOfExecutor)
                    {
                        case TaskExecutor.ExecutorType.LithoSheet:
                            cv = new PrintingSheetCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExecs, articles);

                            ((PrintingSheetCostDetail)cv).BuyingFormat =
                                 (((PrintingSheetCostDetail)cv).BuyingFormat == "" || ((PrintingSheetCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingSheetCostDetail)cv).BuyingFormats != null) && (((PrintingSheetCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingSheetCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingSheetCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingSheetCostDetail)cv).PrintingFormat =
                                (((PrintingSheetCostDetail)cv).PrintingFormat == "" || ((PrintingSheetCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingSheetCostDetail)cv).BuyingFormat
                                : ((PrintingSheetCostDetail)cv).PrintingFormat;


                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            break;
                        case TaskExecutor.ExecutorType.LithoRoll:
                            break;
                        case TaskExecutor.ExecutorType.DigitalSheet:
                            cv = new PrintingSheetCostDetail();
                            cv.TaskExecutors = tskExecs.ToList();
                            cv.TaskCost = this;
                            break;
                        case TaskExecutor.ExecutorType.DigitalRoll:
                            break;
                        case TaskExecutor.ExecutorType.PlotterRoll:

                            cv = new PrintingRollCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExecs, articles);


                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            ((PrintingRollCostDetail)cv).BuyingFormat =
                                 (((PrintingRollCostDetail)cv).BuyingFormat == "" || ((PrintingRollCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingRollCostDetail)cv).BuyingFormats != null) && (((PrintingRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingRollCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingRollCostDetail)cv).PrintingFormat =
                                (((PrintingRollCostDetail)cv).PrintingFormat == "" || ((PrintingRollCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingRollCostDetail)cv).BuyingFormat
                                : ((PrintingRollCostDetail)cv).PrintingFormat;


                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

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
                            cv.InitCostDetail(tskExecs, articles);

                            ((PrintingSheetCostDetail)cv).BuyingFormat =
                                 (((PrintingSheetCostDetail)cv).BuyingFormat == "" || ((PrintingSheetCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingSheetCostDetail)cv).BuyingFormats != null) && (((PrintingSheetCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingSheetCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingSheetCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingSheetCostDetail)cv).PrintingFormat =
                                (((PrintingSheetCostDetail)cv).PrintingFormat == "" || ((PrintingSheetCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingSheetCostDetail)cv).BuyingFormat
                                : ((PrintingSheetCostDetail)cv).PrintingFormat;


                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            break;



                        case TaskExecutor.ExecutorType.Flexo:
                        case TaskExecutor.ExecutorType.FlatRoll:

                            cv = new PrintingZRollCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExecs, articles);

                            cv.SetTaskexecutor(tskExecs, codTaskExecutor);

                            cv.ProductPart = productPart;

                            ((PrintingZRollCostDetail)cv).DieTollerance = 0.5;
                            //qui voglio solo le fustelle flexo e semiroll
                            ((PrintingZRollCostDetail)cv).Dies = articles.OfType<Die>().ToList();

                            //search valid formats
                            ((PrintingZRollCostDetail)cv).FuzzyAlgo();

                            ((PrintingZRollCostDetail)cv).BuyingFormat =
                                 (((PrintingZRollCostDetail)cv).BuyingFormat == "" || ((PrintingZRollCostDetail)cv).BuyingFormat == null) ?
                                 (((PrintingZRollCostDetail)cv).BuyingFormats != null) && (((PrintingZRollCostDetail)cv).BuyingFormats.Count > 0) ? ((PrintingZRollCostDetail)cv).BuyingFormats.FirstOrDefault() : null
                                 : ((PrintingZRollCostDetail)cv).BuyingFormat;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingZRollCostDetail)cv).PrintingFormat =
                                (((PrintingZRollCostDetail)cv).PrintingFormat == "" || ((PrintingZRollCostDetail)cv).PrintingFormat == null) ?
                                ((PrintingZRollCostDetail)cv).BuyingFormat
                                : ((PrintingZRollCostDetail)cv).PrintingFormat;



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


        public virtual void MergeField(DocX doc)
        {
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.Description", this.Description));
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.Quantity", this.Quantity ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.UnitCost", this.UnitCost));
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.TotalCost", this.TotalCost));
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.Markup", this.Markup ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("Cost.GranTotalCost", this.GranTotalCost));
        }


    }
}
