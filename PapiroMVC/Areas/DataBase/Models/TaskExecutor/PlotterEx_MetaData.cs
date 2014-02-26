using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class Plotter_MetaData : TaskExecutor_MetaData
    {
        //[DisplayNameLocalized(typeof(ResPlotter), "Width")]
        //[Tooltip(typeof(ResPlotter), "WidthToolTip")]
        //public Nullable<long> Width { get; set; }

        [DisplayNameLocalized(typeof(ResPlotter), "ColorJet")]
        [Tooltip(typeof(ResPlotter), "ColorJetToolTip")]
        public Nullable<bool> ColorJet { get; set; }
        [DisplayNameLocalized(typeof(ResPlotter), "WhiteUV")]
        [Tooltip(typeof(ResPlotter), "WhiteUVToolTip")]
        public Nullable<bool> WhiteUV { get; set; }
        [DisplayNameLocalized(typeof(ResPlotter), "ColorUV")]
        [Tooltip(typeof(ResPlotter), "ColorUVToolTip")]
        public Nullable<bool> ColorUV { get; set; }
        [DisplayNameLocalized(typeof(ResPlotter), "Cutting")]
        [Tooltip(typeof(ResPlotter), "CuttingToolTip")]
        public Nullable<bool> Cutting { get; set; }



    }
}
