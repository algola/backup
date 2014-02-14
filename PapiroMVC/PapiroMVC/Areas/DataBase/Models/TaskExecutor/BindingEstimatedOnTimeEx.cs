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
    [MetadataType(typeof(BindingEstimatedOnTime_MetaData))]
    public partial class BindingEstimatedOnTime: TaskEstimatedOn
    {
        public BindingEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.BindingOnTime;
        }

        #region Proprietà aggiuntive

        #endregion


    }
}
