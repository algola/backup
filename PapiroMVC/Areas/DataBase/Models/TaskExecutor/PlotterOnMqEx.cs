using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(PlotterOnMq_MetaData))]
    public  partial class PlotterOnMq: TaskEstimatedOnMq, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public PlotterOnMq()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.PlotterOnMq;
        }

        #region Proprietà aggiuntive
        #endregion

        #region Error Handler

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""                       
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = null;
                ////validazione della proprietà Note
                //if (proprieta == "Note")
                //{
                //    if (this.Note != null)
                //    {
                //        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,255}$", RegexOptions.IgnoreCase);
                //        if (!exp.IsMatch(this.Note))
                //        {
                //            result = "Superata la lunghezza delle note consentita";
                //        }
                //    }
                //}
                ////validazione della proprietà Prodotto
                //if (proprieta == "Prodotto")
                //{
                //    if (this.Prodotto == null)
                //    {
                //        result = "Nessuno prodotto selezionato";
                //    }
                //}
                return result;
            }
        }

        //Check validation of entity
        public override bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret && base.IsValid;
            }
        }

        #endregion

        #region Handle copy for modify

        public override void Copy(TaskEstimatedOn to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((PlotterOnMq)to).CostInkJetLow = this.CostInkJetLow;
            ((PlotterOnMq)to).CostInkJetMed = this.CostInkJetMed;
            ((PlotterOnMq)to).CostInkJetHight = this.CostInkJetHight;
            ((PlotterOnMq)to).CostUVLow = this.CostUVLow;
            ((PlotterOnMq)to).CostUVMed = this.CostUVMed;
            ((PlotterOnMq)to).CostUVHight = this.CostUVHight;
            ((PlotterOnMq)to).CostWhite = this.CostWhite;
            ((PlotterOnMq)to).CostCutting = this.CostCutting;
        }

        #endregion
    }



}
