using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.TaskExecutor;

namespace PapiroMVC.Models
{

    public partial class AvarageRunPerRunStep_MetaData : Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResAvarageRunPerRunStep), "AvarageRunPerHour")]
        public Nullable<long> AvarageRunPerHour { get; set; }
    }
}