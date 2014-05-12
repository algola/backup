using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Step_MetaData))]
    public abstract partial class Step
    {
       
        #region Proprietà aggiuntive
        public enum StepType
        {
            AvarageRunForRun,
            CostPerRun,
            CostPerMq,
            DeficitForWeight,
            DeficitOnCostForWeight,
            BindingAvarageRunPerRunStep,
            BindingCostPerRunStep,
            CostPerColor
        }

        public StepType TypeOfStep
        {
            get;
            protected set;
        }

        #endregion

    }
}
