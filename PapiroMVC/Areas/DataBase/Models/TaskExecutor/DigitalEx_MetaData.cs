using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PapiroMVC.Models
{

    public abstract partial class Digital_MetaData : PrinterMachine_MetaData
    {
        [DisplayNameLocalized(typeof(ResDigital), "BWSide1")]
        [Tooltip(typeof(ResDigital), "BWSide1ToolTip")]
        public Nullable<bool> BWSide1 { get; set; }
        [DisplayNameLocalized(typeof(ResDigital), "BWSide2")]
        [Tooltip(typeof(ResDigital), "BWSide2ToolTip")]
        public Nullable<bool> BWSide2 { get; set; }
        [DisplayNameLocalized(typeof(ResDigital), "ColorSide1")]
        public Nullable<bool> ColorSide1 { get; set; }
        [DisplayNameLocalized(typeof(ResDigital), "ColorSide2")]
        public Nullable<bool> ColorSide2 { get; set; }
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