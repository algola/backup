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
    [MetadataType(typeof(Litho_MetaData))]
    public abstract partial class Litho : PrinterMachine
    {

        public Litho()
        {
            this.ControPinza = 0;
            this.PrintingUnit = 4;
            this.Sheetwise = false;
            this.SheetwiseAfterPrintingUnit = 0;
            this.WeightMax = 500;
            this.WeightMin = 0;           
        }

        #region Added Properties

        #endregion

    }
}
