using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class Flexo_MetaData : Litho_MetaData
    {

        [DisplayNameLocalized(typeof(ResFlexo), "PrintingUnit")]
        [Tooltip(typeof(ResFlexo), "PrintingUnitToolTip")]
        public Nullable<short> PrintingUnit { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "PaperFirstStartLenght")]
        [Tooltip(typeof(ResFlexo), "PaperFirstStartLenghtToolTip")]
        public Nullable<long> PaperFirstStartLenght { get; set; }

        [DisplayNameLocalized(typeof(ResFlexo), "Width")]
        [Tooltip(typeof(ResFlexo), "WidthToolTip")]
        public Nullable<double> FlexoWidth { get; set; }

    }
}