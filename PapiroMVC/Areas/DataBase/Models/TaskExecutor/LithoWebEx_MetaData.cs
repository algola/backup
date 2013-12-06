using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class LithoRoll_MetaData : Litho_MetaData
    {
        [DisplayNameLocalized(typeof(ResLithoRoll), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResLithoRoll), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }
    }
}