using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(DigitalRoll_MetaData))]
    public partial class DigitalRoll : Digital, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public DigitalRoll()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.DigitalRoll;
        }

        #region Added Properties

        #endregion

        #region Error Handler

        private static readonly string[] proprietaDaValidare =
               {
                   "PaperFirstStartLenght"
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


            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
