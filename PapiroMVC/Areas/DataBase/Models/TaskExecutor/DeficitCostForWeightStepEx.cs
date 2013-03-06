using System;
using System.ComponentModel;


namespace PapiroMVC.Models
{
    public partial class DeficitOnCostForWeightStep : Step, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public DeficitOnCostForWeightStep()
        {
            this.TypeOfStep = Step.StepType.DeficitOnCostForWeight;
        }

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

        public override void Copy(Step to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((DeficitForWeightStep)to).DeficitRate = this.DeficitRate;
        }

        #endregion
    }
}
