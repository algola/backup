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
        public override double GetStarts(string codOptionTypeOfTask)
        {
            return 1;
        }

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.NOTypeOfQuantity;
                return ret;
            }
        }


        public override double GetImplants(string codOptionTypeOfTask)
        {
                return 0;
        }

        #region Added Properties

        #endregion

    }
}
