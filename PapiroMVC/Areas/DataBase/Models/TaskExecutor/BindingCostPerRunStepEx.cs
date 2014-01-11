using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(BindingCostPerRunStep_MetaData))]
    public partial class BindingCostPerRunStep: Step 
    {

        public BindingCostPerRunStep()
        {
            this.TypeOfStep = Step.StepType.BindingCostPerRunStep;
        }

        #region Added Properties

        #endregion

    }
}
