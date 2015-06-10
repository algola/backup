using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(PlotterRoll_MetaData))]
    public partial class PlotterRoll : Plotter
    {
        public PlotterRoll()
        {
            TypeOfExecutor = ExecutorType.PlotterRoll;
        }

        #region Added Properties

        #endregion

        public override string GetEditMethod()
        {
            return "EditPlotterRoll";
        }
    }
}
