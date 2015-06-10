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


        #region Added Properties

        #endregion

    }
}
