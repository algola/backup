using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(CostPerRunStep_MetaData))]
    public partial class CostPerRunStep : Step
    {

        public CostPerRunStep()
        {
            this.TypeOfStep = Step.StepType.CostPerRun;
        }

        #region Added Properties

        #endregion

    }
}
