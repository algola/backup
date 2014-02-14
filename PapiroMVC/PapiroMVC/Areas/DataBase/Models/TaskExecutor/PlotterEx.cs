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
    [MetadataType(typeof(Plotter_MetaData))]
    public abstract partial class Plotter : TaskExecutor
    {
        public override double Starts(string codOptionTypeOfTask)
        {
            return 1;
        }

        #region Added Properties

        #endregion

    }
}
