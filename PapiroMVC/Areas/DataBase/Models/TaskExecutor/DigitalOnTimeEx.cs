using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(DigitalOnTime_MetaData))]
    public partial class DigitalOnTime: TaskEstimatedOnTime, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public DigitalOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.DigitalOnTime;
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
            ((DigitalOnTime)to).CostPerHourRunningBW = this.CostPerHourRunningBW;
            ((DigitalOnTime)to).CostPerHourStartingBW = this.CostPerHourStartingBW;
        }

        #endregion

    }
}
