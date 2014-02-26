using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(CostPerRunStep_MetaData))]
    public partial class CostPerRunStepBW : Step
    {

        public CostPerRunStepBW()
        {
            this.TypeOfStep = Step.StepType.CostPerRun;
        }

        #region Added Properties

        #endregion

    }
}
