using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    [Serializable]
    public partial class LithoSheet : Litho, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public LithoSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.LithoSheet;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {                  
                   //Specify validation property
                   //    "FormatMin",
                   //    "FormatMax",
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
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

            ((LithoSheet)to).ProofSheetFirstStart = this.ProofSheetFirstStart;
            ((LithoSheet)to).ProofSheetSecondsStart = this.ProofSheetSecondsStart;
            ((LithoSheet)to).ProductionWaste = this.ProductionWaste;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
