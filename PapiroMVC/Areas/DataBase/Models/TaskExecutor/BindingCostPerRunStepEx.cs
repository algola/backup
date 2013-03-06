using System;
using System.ComponentModel;


namespace PapiroMVC.Models
{
    public partial class BindingCostPerRunStep: Step , IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public BindingCostPerRunStep()
        {
            this.TypeOfStep = Step.StepType.BindingCostPerRunStep;
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

            ((BindingCostPerRunStep)to).StartingCost4 = this.StartingCost4;
            ((BindingCostPerRunStep)to).CostPerUnit4 = this.CostPerUnit4;
            ((BindingCostPerRunStep)to).StartingCost6 = this.StartingCost6;
            ((BindingCostPerRunStep)to).CostPerUnit6 = this.CostPerUnit6;
            ((BindingCostPerRunStep)to).StartingCost8 = this.StartingCost8;
            ((BindingCostPerRunStep)to).CostPerUnit8 = this.CostPerUnit8;
            ((BindingCostPerRunStep)to).StartingCost12 = this.StartingCost12;
            ((BindingCostPerRunStep)to).CostPerUnit12 = this.CostPerUnit12;
            ((BindingCostPerRunStep)to).StartingCost16 = this.StartingCost16;
            ((BindingCostPerRunStep)to).CostPerUnit16 = this.CostPerUnit16;
            ((BindingCostPerRunStep)to).StartingCost24 = this.StartingCost24;
            ((BindingCostPerRunStep)to).CostPerUnit24 = this.CostPerUnit24;
            ((BindingCostPerRunStep)to).StartingCost32 = this.StartingCost32;
            ((BindingCostPerRunStep)to).CostPerUnit32 = this.CostPerUnit32;


        }

        #endregion
    }
}
