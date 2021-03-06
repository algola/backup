﻿using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class PlotterRoll_MetaData : Plotter_MetaData
    {
        [DisplayNameLocalized(typeof(ResPlotter), "Width")]
        [Tooltip(typeof(ResPlotter), "WidthToolTip")]
        public Nullable<long> Width { get; set; }
    }
}
