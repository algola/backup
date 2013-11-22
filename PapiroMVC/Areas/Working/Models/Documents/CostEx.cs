using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Cost_MetaData))]
    public partial class Cost : IDataErrorInfo, ICloneable, IDeleteRelated
    {
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
                String codParte = String.Empty;

                /* se è una STAMPA 
                 * dovrò selezionare il tipo di macchina anche a seconda del tipo di lavoro
                 * etichette in rotolo, manifesti etc...
                 * per ora carico.
                 */

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
                        case TaskExecutor.ExecutorType.LithoWeb:
                            break;
                        case TaskExecutor.ExecutorType.DigitalSheet:
                            cv = new PrintingSheetCostDetail();
                            cv.TaskExecutors = tskExec.ToList();
                            cv.TaskCost = this;
                            break;
                        case TaskExecutor.ExecutorType.DigitalWeb:
                            break;
                        case TaskExecutor.ExecutorType.Plotter:
                            cv = new PrintingPlotterCostDetail();

                            cv.TaskCost = this;
                            cv.InitCostDetail(tskExec, articles);

                            ((PrintingPlotterCostDetail)cv).BuyingWidth =
                                 (((PrintingPlotterCostDetail)cv).BuyingWidth == 0 || ((PrintingPlotterCostDetail)cv).BuyingWidth == null) ?
                                 (((PrintingPlotterCostDetail)cv).BuyingWidths != null) && (((PrintingPlotterCostDetail)cv).BuyingWidths.Count > 0) ? ((PrintingPlotterCostDetail)cv).BuyingWidths.FirstOrDefault() : 0
                                 : ((PrintingPlotterCostDetail)cv).BuyingWidth;

                            //TODO: E' da calcolare il formato di stampa a seconda del formato macchina
                            ((PrintingPlotterCostDetail)cv).PrintingFormat =
                                (((PrintingPlotterCostDetail)cv).PrintingFormat == "" || ((PrintingPlotterCostDetail)cv).PrintingFormat == null) ?
                                ""
                                : ((PrintingPlotterCostDetail)cv).PrintingFormat;

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
            }

            #endregion

            #region Materiale
            
            if (this.CodProductPartPrintableArticle != null)
            {
                var productPartPrintableArticle = this.ProductPartsPrintableArticle;
                productPart = this.ProductPartsPrintableArticle.ProductPart;

                //il tipo di materiale dipende dalla stampa o dal tipo di prodotto?               
                //productPartPrintabelArticles = productPart.ProductPartPrintableArticles;
            }

            #endregion

            cv.ProductPart = productPart;

            cv.TaskCost = this;
            cv.CodCost = this.CodCost;
            cv.CodCostDetail = this.CodCost;

            return cv;
        }

    }
}
