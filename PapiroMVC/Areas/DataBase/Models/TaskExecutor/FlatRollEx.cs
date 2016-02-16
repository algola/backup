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
    [MetadataType(typeof(FlatRoll_MetaData))]
    public partial class FlatRoll : Litho
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public FlatRoll()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.FlatRoll;
        }

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.NColorPerMqTypeOfQuantity;
                return ret;
            }
        }

        public override string GetEditMethod()
        {
            return "EditFlatRoll";
        }


        #region Added Properties

        #endregion


        /// <summary>
        /// get printing color (task vs machine)
        /// </summary>
        /// <param name="codOptionTypeOfTask"></param>
        /// <returns></returns>
        public override double GetStarts(string codOptionTypeOfTask)
        {
            var colors = GetColorFR(codOptionTypeOfTask);
            double total = 0;
            total = (colors.cToPrintT + colors.cToPrintTNoImplant) == 0 ? 1 : (colors.cToPrintT + colors.cToPrintTNoImplant);

            double ret = 0;

            if (codOptionTypeOfTask.Contains("SERIGRAFIA"))
            {
                ret = Math.Ceiling(total / this.SerigraphyPrintingUnit ?? 1);                
            }

            if (codOptionTypeOfTask.Contains("STAMPAACALDO"))
            {
                ret = Math.Ceiling(total / this.FoilStampingPrintingUnit ?? 1);
            }


            if (codOptionTypeOfTask.Contains("STAMPAETICHROTOLO"))
            {
                ret = Math.Ceiling(total / this.PrintingUnit ?? 1);
//                ret = total;
            }

            //Starts is used with printerFormat to have
            return ret;
        }


    }
}
