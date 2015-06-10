using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public partial class ProductPartPrintRoll_Metadata : ProductPartTask_MetaData
    {
        [DisplayNameLocalized(typeof(ResProductPartTask), "Retro")]
        [Tooltip(typeof(ResProductPartTask), "RetroToolTip")]
        public bool Retro { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "Vernice")]
        [Tooltip(typeof(ResProductPartTask), "VerniceToolTip")]
        public bool Vernice { get; set; }
    }
}