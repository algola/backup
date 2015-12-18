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
    [MetadataType(typeof(LithoSheet_MetaData))]
    public partial class LithoSheet : Litho
    {

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {

                CostDetail.QuantityType ret = CostDetail.QuantityType.NumberTypeOfQuantity;
                return ret;
            }
        }

        public override double GetStarts(string codOptionTypeOfTask)
        {
            PrintingColor x = GetColorFR(codOptionTypeOfTask);

            //ci sarebbe da distinguere se è un b/v oppure n, per ora no bianca e volta assieme
            return Math.Ceiling(x.cToPrintT / (double)this.PrintingUnit);
        }

        public override double GetWashes(string codOptionTypeOfTask)
        {
            //ci sarebbe da distinguere se è un b/v oppure n, per ora no bianca e volta assieme
            return GetStarts(codOptionTypeOfTask);
        }



        public LithoSheet()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.LithoSheet;
        }

        #region Added Properties

        #endregion


        public override string GetEditMethod()
        {
            return "EditLithoSheet";
        }
    }
}
