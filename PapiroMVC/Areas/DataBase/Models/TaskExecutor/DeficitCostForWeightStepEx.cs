using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DeficitCostForWeightStep_MetaData))]
    public partial class DeficitOnCostForWeightStep : Step
    {
        public DeficitOnCostForWeightStep()
        {
            this.TypeOfStep = Step.StepType.DeficitOnCostForWeight;
        }

        #region Added Properties

        #endregion
    }
}
