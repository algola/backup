using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class LithoWeb_MetaData : Litho_MetaData
    {
        [DisplayNameLocalized(typeof(ResLithoWeb), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResLithoWeb), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }
    }
}