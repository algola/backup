using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(Digital_MetaData))]
    public abstract partial class Digital : PrinterMachine, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""
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

            ((Digital)to).ColorSide1 = this.ColorSide1;
            ((Digital)to).ColorSide1 = this.ColorSide1;
            ((Digital)to).BWSide1 = this.BWSide1;
            ((Digital)to).BWSide2 = this.BWSide2;

            ((Digital)to).ProofSheetFirstStart = this.ProofSheetFirstStart;
            ((Digital)to).ProofSheetSecondsStart = this.ProofSheetSecondsStart;
            ((Digital)to).ProductionWaste = this.ProductionWaste;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
