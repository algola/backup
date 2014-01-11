using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(BindingAvarageRunPerRunStep_MetaData))]
    public partial class BindingAvarageRunPerRunStep : Step
    {

        public BindingAvarageRunPerRunStep()
        {
            this.TypeOfStep = Step.StepType.BindingAvarageRunPerRunStep;
        }

        #region Added Properties

        #endregion

    }
}
