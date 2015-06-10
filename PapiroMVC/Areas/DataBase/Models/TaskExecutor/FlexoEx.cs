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
    [MetadataType(typeof(Flexo_MetaData))]
    public partial class Flexo : Litho
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public Flexo()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.Flexo;
        }

        public override CostDetail.QuantityType TypeOfImplantQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.NColorPerMqTypeOfQuantity;
                return ret;
            }
        }


        public int CurrentZ
        { get; set; }

        /// <summary>
        /// Check if Cylinder 0 are in list
        /// so view has row to insert a new cylinder Z
        /// </summary>
        public void CheckZeroCylinder()
        {
            var x = this.TaskExecutorCylinders.FirstOrDefault(y => y.Z == 0);
            if (x == null)
            {
                this.TaskExecutorCylinders.Add(new TaskExecutorCylinder { Z = 999, Quantity = 0 });
            }
        }


        public int GetZFromCm(double cm)
        {
            int ret;

            if (!(ZMetric ?? false))
            {
                ret = Convert.ToInt32(((Convert.ToDouble(cm) * 8) / 2.54));
            }
            else
            {
                ret = Convert.ToInt32((Convert.ToDouble(cm)) / 3.1415 * 10);
            }

            return ret;
        }

        public double GetCmFromZ(int z)
        {
            double ret;

            if (ZMetric??false)
            {
                ret = (Convert.ToDouble(z)) * 0.31415;
            }
            else
            {
                ret = (Convert.ToDouble(z) / 8) * 2.54;
            }

            return ret;
        }

        #region Added Properties

        #endregion

        public override string GetEditMethod()
        {
            return "EditFlexo";
        }

    }
}
