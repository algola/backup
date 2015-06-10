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
    [MetadataType(typeof(PlotterSheet_MetaData))]
    public partial class PlotterSheet : Plotter
    {

        public PlotterSheet()
        {
            TypeOfExecutor = ExecutorType.PlotterSheet;
        }

        #region Added Properties

        #endregion

        public override string GetEditMethod()
        {
            return "EditPlotterSheet";
        }
    }
}
