using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(CostPerColorStep_MetaData))]
    public partial class CostPerColorStep : Step
    {

        public CostPerColorStep()
        {
            this.TypeOfStep = Step.StepType.CostPerColor;
        }

        #region Added Properties

        #endregion

    }
}
