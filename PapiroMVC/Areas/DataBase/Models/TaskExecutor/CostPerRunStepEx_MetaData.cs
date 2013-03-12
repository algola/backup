using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class CostPerRunStep_MetaData : Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResCostPerRunStep), "CostPerUnit")]
        public Nullable<double> CostPerUnit { get; set; }
    }
}
