using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PrePostPress_MetaData : TaskExecutor_MetaData
    {
        [DisplayNameLocalized(typeof(ResPrePostPress), "IsUnitComputationManual")]
        [Tooltip(typeof(ResPrePostPress), "IsUnitComputationManualToolTip")]
        public Nullable<bool> IsUnitComputationManual { get; set; }
    }
}
