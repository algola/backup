using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class TaskExecutorCylinder_MetaData
    {
        [DisplayNameLocalized(typeof(ResTaskExecutorCylinder), "Z")]
        [Tooltip(typeof(ResTaskExecutorCylinder), "ZToolTip")]
        public Nullable<int> Z { get; set; }

        [DisplayNameLocalized(typeof(ResTaskExecutorCylinder), "Quantity")]
        [Tooltip(typeof(ResTaskExecutorCylinder), "QuantityToolTip")]
        public Nullable<int> Quantity { get; set; }    
    }
}