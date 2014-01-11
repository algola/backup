using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(AvarageRunPerRunStep_MetaData))]
    public partial class AvarageRunPerRunStep : Step
    {

        public AvarageRunPerRunStep()
        {
            this.TypeOfStep = Step.StepType.AvarageRunForRun;
        }

        #region Added Properties

        #endregion


    }
}
