using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Document;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class PrintingCostDetail_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }

       // [Tooltip(typeof(ResCostDetail), "LateralToolTip")]
        [DisplayNameLocalized(typeof(ResCostDetail), "Lateral")]
        public Nullable<double> Lateral { get; set; }

     //   [Tooltip(typeof(ResCostDetail), "ForceLateralToolTip")]
        [DisplayNameLocalized(typeof(ResCostDetail), "ForceLateral")]
        public Nullable<bool> ForceLateral { get; set; }

    }
}