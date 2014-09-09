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
    [MetadataType(typeof(Digital_MetaData))]
    public abstract partial class Digital : PrinterMachine
    {

        public override double GetImplants(string codOptionTypeOfTask)
        {
            return 0;
        }

        #region Added Properties

        #endregion

    }
}
