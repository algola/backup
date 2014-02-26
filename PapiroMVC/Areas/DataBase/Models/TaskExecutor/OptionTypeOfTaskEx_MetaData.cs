using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class OptionTypeOfTask_MetaData
    {
        [DisplayNameLocalized(typeof(ResOptionTypeOfTask), "OptionName")]
        [Tooltip(typeof(ResOptionTypeOfTask), "OptionNameToolTip")]
        public string OptionName { get; set; }

        [DisplayNameLocalized(typeof(ResOptionTypeOfTask), "IdexOf")]
        [Tooltip(typeof(ResOptionTypeOfTask), "IdexOfToolTip")]
        public Nullable<int> IdexOf { get; set; }
    }
}
