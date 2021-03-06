﻿using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class Step_MetaData
    {
        [DisplayNameLocalized(typeof(ResStep), "TimeStampTable")]
        [Tooltip(typeof(ResStep), "TimeStampTableToolTip")]
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(ResStep), "CodTaskEstimatedOn")]
        [Tooltip(typeof(ResStep), "CodTaskEstimatedOnToolTip")]
        public string CodTaskEstimatedOn { get; set; }
        [DisplayNameLocalized(typeof(ResStep), "IdStep")]
        [Tooltip(typeof(ResStep), "IdStepToolTip")]
        public long IdStep { get; set; }
        [DisplayNameLocalized(typeof(ResStep), "FromUnit")]
        [Tooltip(typeof(ResStep), "FromUnitToolTip")]
        public Nullable<double> FromUnit { get; set; }
        [DisplayNameLocalized(typeof(ResStep), "ToUnit")]
        [Tooltip(typeof(ResStep), "ToUnitToolTip")]
        public Nullable<double> ToUnit { get; set; }

        [DisplayNameLocalized(typeof(ResStep), "Format")]
        [Tooltip(typeof(ResStep), "FormatToolTip")]
        public string Format { get; set; }


    }
}
