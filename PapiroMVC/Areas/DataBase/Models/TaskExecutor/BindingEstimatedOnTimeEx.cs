using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public partial class BindingEstimatedOnTime: TaskEstimatedOn, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public BindingEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.BindingOnTime;
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
            ((BindingEstimatedOnTime)to).CostPerHourRunning = this.CostPerHourRunning;
            ((BindingEstimatedOnTime)to).CostPerHourStarting = this.CostPerHourStarting;
            ((BindingEstimatedOnTime)to).StartingTime4 = this.StartingTime4;
            ((BindingEstimatedOnTime)to).StartingTime6 = this.StartingTime6;
            ((BindingEstimatedOnTime)to).StartingTime8 = this.StartingTime8;
            ((BindingEstimatedOnTime)to).StartingTime12 = this.StartingTime12;
            ((BindingEstimatedOnTime)to).StartingTime16 = this.StartingTime16;
            ((BindingEstimatedOnTime)to).StartingTime24 = this.StartingTime24;
            ((BindingEstimatedOnTime)to).StartingTime32 = this.StartingTime32;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour4 = this.AvarageRunPerHour4;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour6 = this.AvarageRunPerHour6;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour8 = this.AvarageRunPerHour8;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour12 = this.AvarageRunPerHour12;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour16 = this.AvarageRunPerHour16;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour24 = this.AvarageRunPerHour24;
            ((BindingEstimatedOnTime)to).AvarageRunPerHour32 = this.AvarageRunPerHour32;

            ((BindingEstimatedOnTime)to).StartingTimeBinding = this.StartingTimeBinding;
            ((BindingEstimatedOnTime)to).AvarageRunPerHourBinding = this.AvarageRunPerHourBinding;
        
        }

        #endregion

    }
}
