using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.TaskExecutor;

namespace PapiroMVC.Models
{
    public partial class DigitalWeb_MetaData : Digital_MetaData
    {
        [DisplayNameLocalized(typeof(ResDigitalWeb), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResDigitalWeb), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }
    }
}
