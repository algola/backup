using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public partial class BindingEstimatedOnRun: IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public BindingEstimatedOnRun()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.BindingOnRun;
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
            ((BindingEstimatedOnRun)to).StartingCost4 = this.StartingCost4;
            ((BindingEstimatedOnRun)to).StartingCost6 = this.StartingCost6;
            ((BindingEstimatedOnRun)to).StartingCost8 = this.StartingCost8;
            ((BindingEstimatedOnRun)to).StartingCost12 = this.StartingCost12;
            ((BindingEstimatedOnRun)to).StartingCost16 = this.StartingCost16;
            ((BindingEstimatedOnRun)to).StartingCost24 = this.StartingCost24;
            ((BindingEstimatedOnRun)to).StartingCost32 = this.StartingCost32;
            ((BindingEstimatedOnRun)to).CostPerUnit4 = this.CostPerUnit4;
            ((BindingEstimatedOnRun)to).CostPerUnit6 = this.CostPerUnit6;
            ((BindingEstimatedOnRun)to).CostPerUnit8 = this.CostPerUnit8;
            ((BindingEstimatedOnRun)to).CostPerUnit12 = this.CostPerUnit12;
            ((BindingEstimatedOnRun)to).CostPerUnit16 = this.CostPerUnit16;
            ((BindingEstimatedOnRun)to).CostPerUnit24 = this.CostPerUnit24;
            ((BindingEstimatedOnRun)to).CostPerUnit32 = this.CostPerUnit32;

            ((BindingEstimatedOnRun)to).BindingCost = this.BindingCost;
            ((BindingEstimatedOnRun)to).UseDifferentCostPerUnit = this.UseDifferentCostPerUnit;
            ((BindingEstimatedOnRun)to).BindingStartingCost = this.BindingStartingCost;
        
        }

        #endregion

    }
}
