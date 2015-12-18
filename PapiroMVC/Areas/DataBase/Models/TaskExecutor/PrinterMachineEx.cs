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


        //get number of impants by type of task!! ex: 2 colors --> 2 implants
        public override double GetImplants(string codOptionTypeOfTask)
        {

            var x =GetColorFR(codOptionTypeOfTask);

            //Starts is used with printerFormat to have
            return Math.Ceiling(x.cToPrintT);
        }


    }
}
