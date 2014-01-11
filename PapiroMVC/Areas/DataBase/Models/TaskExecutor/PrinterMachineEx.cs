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
    [MetadataType(typeof(PrinterMachine_MetaData))]
    public abstract partial class PrinterMachine : TaskExecutor
    {
       
        #region Proprietà aggiuntive

        #endregion

    }
}
