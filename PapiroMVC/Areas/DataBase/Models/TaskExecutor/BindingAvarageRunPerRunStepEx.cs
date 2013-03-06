using System;
using System.ComponentModel;


namespace PapiroMVC.Models
{
    public partial class BindingAvarageRunPerRunStep : Step, IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public BindingAvarageRunPerRunStep()
        {
            this.TypeOfStep = Step.StepType.BindingAvarageRunPerRunStep;
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

            ((BindingAvarageRunPerRunStep)to).StartingTime4 = this.StartingTime4;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour4 = this.AvarageRunPerHour4;
            ((BindingAvarageRunPerRunStep)to).StartingTime6 = this.StartingTime6;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour6 = this.AvarageRunPerHour6;
            ((BindingAvarageRunPerRunStep)to).StartingTime8 = this.StartingTime8;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour8 = this.AvarageRunPerHour8;
            ((BindingAvarageRunPerRunStep)to).StartingTime12 = this.StartingTime12;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour12 = this.AvarageRunPerHour12;
            ((BindingAvarageRunPerRunStep)to).StartingTime16 = this.StartingTime16;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour16 = this.AvarageRunPerHour16;
            ((BindingAvarageRunPerRunStep)to).StartingTime24 = this.StartingTime24;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour24 = this.AvarageRunPerHour24;
            ((BindingAvarageRunPerRunStep)to).StartingTime32 = this.StartingTime32;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHour32 = this.AvarageRunPerHour32;

            ((BindingAvarageRunPerRunStep)to).StartingTimeBinding = this.StartingTimeBinding;
            ((BindingAvarageRunPerRunStep)to).AvarageRunPerHourBinding = this.AvarageRunPerHourBinding;

        }

        #endregion
    }
}
