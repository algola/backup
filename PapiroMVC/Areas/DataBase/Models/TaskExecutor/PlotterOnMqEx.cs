using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(PlotterOnMq_MetaData))]
    public partial class PlotterOnMq : TaskEstimatedOnMq
    {

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, double mq)
        {

            double costMq = 0;

            if (codOptionTypeOfTask.Contains("BASSA"))
            {
                costMq += Convert.ToDouble(CostUVLow, Thread.CurrentThread.CurrentUICulture);
            }

            if (codOptionTypeOfTask.Contains("ALTA"))
            {
                costMq += Convert.ToDouble(CostUVHight, Thread.CurrentThread.CurrentUICulture);
            }

            if (codOptionTypeOfTask.Contains("MEDIA"))
            {
                costMq += Convert.ToDouble(CostUVMed, Thread.CurrentThread.CurrentUICulture);
            }

            if (codOptionTypeOfTask.Contains("W"))
            {
                costMq += Convert.ToDouble(CostWhite, Thread.CurrentThread.CurrentUICulture);
            }

            if (codOptionTypeOfTask.Contains("DN"))
            {
                costMq *= 2;
                costMq += Convert.ToDouble(CostWhite, Thread.CurrentThread.CurrentUICulture);                
            }

            var total = Convert.ToDouble(StartingCost1, Thread.CurrentThread.CurrentUICulture) +
                Convert.ToDouble(StartingCost2, Thread.CurrentThread.CurrentUICulture) * (starts - 1) +
                costMq * mq;

            CostAndTime ct = new CostAndTime { Cost = total ,Time = new TimeSpan(0,0,0) };

            return ct;

        }

        public PlotterOnMq()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.PlotterOnMq;
        }

        #region Proprietà aggiuntive
        #endregion
    }



}
