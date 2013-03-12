using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public abstract partial class PrinterMachine_MetaData : TaskExecutor_MetaData
    {
        [DisplayNameLocalized(typeof(ResPrinterMachine), "InkUsage")]
        [Tooltip(typeof(ResPrinterMachine), "InkUsageToolTip")]
        public Nullable<double> InkUsage { get; set; }
        [DisplayNameLocalized(typeof(ResPrinterMachine), "InkUsageForfait")]
        [Tooltip(typeof(ResPrinterMachine), "InkUsageForfaitToolTip")]
        public Nullable<double> InkUsageForfait { get; set; }
    }
}
