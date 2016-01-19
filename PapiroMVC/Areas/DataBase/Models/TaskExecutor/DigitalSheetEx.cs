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
    [MetadataType(typeof(DigitalSheet_MetaData))]
    public partial class DigitalSheet : Digital
    {

        public override double GetStarts(string codOptionTypeOfTask)
        {

            var colors = GetColorFR(codOptionTypeOfTask);
            double starts = 1;

            if (colors.cToPrintR > 0)
            {
                starts = 2;
            }

            return starts;
        }



        public DigitalSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.DigitalSheet;
        }

        #region Added Properties

        #endregion

        public override string GetEditMethod()
        {
            return "EditDigitalSheet";
        }
    }
}
