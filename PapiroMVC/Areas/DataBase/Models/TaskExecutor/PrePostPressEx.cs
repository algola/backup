﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(PrePostPress_MetaData))]
    public partial class PrePostPress : TaskExecutor, IDataErrorInfo, ICloneable, IDeleteRelated
    {


        public PrePostPress()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.PrePostPress;
        }

        #region Proprietà aggiuntive

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
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
            base.Copy(to);
            //All properties of object
            //and pointer of sons

            ((PrePostPress)to).IsUnitComputationManual = this.IsUnitComputationManual;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion
    }

}
