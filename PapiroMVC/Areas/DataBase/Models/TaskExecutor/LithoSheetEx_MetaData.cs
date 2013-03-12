using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class LithoSheet_MetaData : Litho_MetaData
    {
        [DisplayNameLocalized(typeof(ResLithoSheet), "ProofSheetFirstStart")]
        [Tooltip(typeof(ResLithoSheet), "ProofSheetFirstStartToolTip")]
        public Nullable<long> ProofSheetFirstStart { get; set; }
        [DisplayNameLocalized(typeof(ResLithoSheet), "ProofSheetSecondsStart")]
        [Tooltip(typeof(ResLithoSheet), "ProofSheetSecondsStartToolTip")]
        public Nullable<long> ProofSheetSecondsStart { get; set; }
        [DisplayNameLocalized(typeof(ResLithoSheet), "ProductionWaste")]
        [Tooltip(typeof(ResLithoSheet), "ProductionWasteToolTip")]
        public Nullable<double> ProductionWaste { get; set; }
    }
}
