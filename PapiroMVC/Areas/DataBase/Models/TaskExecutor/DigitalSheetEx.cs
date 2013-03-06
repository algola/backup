using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    public partial class DigitalSheet : Digital, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public DigitalSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.DigitalSheet;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   "ProofSheetFirstStart"
                   //Specify validation property
                   //    "FormatMin",
                   //    "FormatMax",
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];

                if (proprieta == "ProofSheetFirstStart")
                {
                    if (this.ProofSheetFirstStart < 0)
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

            ((DigitalSheet)to).ProofSheetFirstStart = this.ProofSheetFirstStart;
            ((DigitalSheet)to).ProofSheetSecondsStart = this.ProofSheetSecondsStart;
            ((DigitalSheet)to).ProductionWaste = this.ProductionWaste;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
