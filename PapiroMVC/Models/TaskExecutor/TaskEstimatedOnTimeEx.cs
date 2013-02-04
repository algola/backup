using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public partial class TaskEstimatedOnTime: IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public TaskEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnTime;
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
            ((TaskEstimatedOnTime)to).AvarageRunPerHour = this.AvarageRunPerHour;
            ((TaskEstimatedOnTime)to).UseDifferentRunPerHour = this.UseDifferentRunPerHour;
            ((TaskEstimatedOnTime)to).StartingTime1 = this.StartingTime1;
            ((TaskEstimatedOnTime)to).StartingTime2 = this.StartingTime2;
            ((TaskEstimatedOnTime)to).TimeForfait = this.TimeForfait;
            ((TaskEstimatedOnTime)to).CostPerHourRunning = this.CostPerHourRunning;
            ((TaskEstimatedOnTime)to).CostPerHourStarting = this.CostPerHourStarting;
        }

        #endregion

    }
}
