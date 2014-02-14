using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(CostPerMqStep_MetaData))]
    public partial class CostPerMqStep: Step
    {

        public CostPerMqStep()
        {
            this.TypeOfStep = Step.StepType.CostPerMq;
        }

        #region Added Properties

        #endregion

    }
}
