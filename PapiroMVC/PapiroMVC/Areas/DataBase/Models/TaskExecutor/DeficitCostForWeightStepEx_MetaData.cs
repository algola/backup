using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class DeficitCostForWeightStep_MetaData : Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResDeficitCostForWeightStep), "DeficitRate")]
        public Nullable<double> DeficitRate { get; set; }
    }
}