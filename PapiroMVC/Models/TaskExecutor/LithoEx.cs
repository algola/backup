using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public abstract partial class Litho : IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public Litho()
        {
            this.ControPinza = 0;
            this.PrintingUnit = 4;
            this.IsEstimatedOnTime = true;
            this.Sheetwise = false;
            this.SheetwiseAfterPrintingUnit = 0;
            this.WeightMax = 500;
            this.WeightMin = 0;           
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       "PrintingUnit"
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];

                if (proprieta == "PrintingUnit")
                {
                    if (this.PrintingUnit < 0)
                    {
                        result = "Messagge Error";
                    }
                }

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

        public override void Copy(TaskExecutor to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((Litho)to).PrintingUnit = this.PrintingUnit;
            ((Litho)to).SheetwiseAfterPrintingUnit = this.SheetwiseAfterPrintingUnit;
            ((Litho)to).SheetwiseAfterPrintingUnit = this.SheetwiseAfterPrintingUnit;
            ((Litho)to).Sheetwise = this.Sheetwise;
            ((Litho)to).WashUpTime = this.WashUpTime;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
