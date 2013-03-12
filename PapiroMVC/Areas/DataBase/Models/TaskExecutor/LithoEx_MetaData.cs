using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class Litho_MetaData : PrinterMachine_MetaData
    {
        [DisplayNameLocalized(typeof(ResLitho), "PrintingUnit")]
        [Tooltip(typeof(ResLitho), "PrintingUnitToolTip")]
        public Nullable<short> PrintingUnit { get; set; }
        [DisplayNameLocalized(typeof(ResLitho), "SheetwiseAfterPrintingUnit")]
        [Tooltip(typeof(ResLitho), "SheetwiseAfterPrintingUnitToolTip")]
        public Nullable<long> SheetwiseAfterPrintingUnit { get; set; }
        [DisplayNameLocalized(typeof(ResLitho), "Sheetwise")]
        [Tooltip(typeof(ResLitho), "SheetwiseToolTip")]
        public Nullable<bool> Sheetwise { get; set; }
        
        public Nullable<System.TimeSpan> WashUpTime { get; set; }
        [DisplayNameLocalized(typeof(ResLitho), "ChangePlateTime")]
        [Tooltip(typeof(ResLitho), "ChangePlateTimeToolTip")]
        public Nullable<System.TimeSpan> ChangePlateTime { get; set; }
    }
}
