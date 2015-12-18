using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class DeficitForWeightStep_MetaData : Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResDeficitForWeightStep), "DeficitRate")]
        [Range(-10000,99.9)]
        public Nullable<double> DeficitRate { get; set; }
    }
}