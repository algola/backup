using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [DataContract(IsReference = true)]
    [Serializable]
    [KnownType(typeof(OptionTypeOfTask))]
    [MetadataType(typeof(OptionTypeOfTask_MetaData))]
    public partial class OptionTypeOfTask
    {
       
        #region Proprietà aggiuntive
        public enum OptionTypeOfTaskType
        {
            AvarageRunForRun,
            CostPerRun,
            CostPerMq,
            DeficitForWeight,
            DeficitOnCostForWeight,
            BindingAvarageRunPerRunStep,
            BindingCostPerRunStep
        }

        public OptionTypeOfTaskType TypeOfOptionTypeOfTask
        {
            get;
            protected set;
        }

        #endregion

    }
}
