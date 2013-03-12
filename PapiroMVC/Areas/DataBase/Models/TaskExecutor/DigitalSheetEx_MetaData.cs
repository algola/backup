using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PapiroMVC.Models
{
    public partial class DigitalSheet_MetaData : Digital_MetaData
    {
        [DisplayNameLocalized(typeof(ResDigitalSheet), "ProofSheetFirstStart")]
        [Tooltip(typeof(ResDigitalSheet), "ProofSheetFirstStartToolTip")]
        public Nullable<long> ProofSheetFirstStart { get; set; }
        [DisplayNameLocalized(typeof(ResDigitalSheet), "ProofSheetSecondsStart")]
        [Tooltip(typeof(ResDigitalSheet), "ProofSheetSecondStartToolTip")]
        public Nullable<long> ProofSheetSecondsStart { get; set; }
        [DisplayNameLocalized(typeof(ResDigitalSheet), "ProductionWaste")]
        [Tooltip(typeof(ResDigitalSheet), "ProductionWasteToolTip")]
        public Nullable<double> ProductionWaste { get; set; }
    }
}
