using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DeficitForWeightStep_MetaData))]
    public partial class DeficitForWeightStep : Step
    {

        public DeficitForWeightStep()
        {
            this.TypeOfStep = Step.StepType.DeficitForWeight;
        }

        #region Added Properties

        #endregion
    }
}
