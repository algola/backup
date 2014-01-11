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
    [MetadataType(typeof(BindingEstimatedOnRun_MetaData))]
    public partial class BindingEstimatedOnRun: TaskEstimatedOn
    {
        public BindingEstimatedOnRun()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.BindingOnRun;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}
