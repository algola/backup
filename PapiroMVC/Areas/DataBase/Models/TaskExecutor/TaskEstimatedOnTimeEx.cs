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
    [MetadataType(typeof(TaskEstimatedOnTime_MetaData))]
    public partial class TaskEstimatedOnTime : TaskEstimatedOn
    {

        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, PrintingColor colors, int makereadies, double running, double weight)
        {
            TimeSpan totalTimeR = new TimeSpan(0, 0, 0);
            var totalTimeA = this.StartingTime1 ?? TimeSpan.Zero;

            //avviamenti successivi
            for (int i = 0; i < starts-1; i++)
            {
                totalTimeA += (StartingTime2 ?? TimeSpan.Zero);
            }

            //avviamento a colore
            for (int i = 0; i < colors.cToPrintT; i++)
            {
                totalTimeA += (StartingTimePerColor ?? TimeSpan.Zero);
            }

            Nullable<long> avarage = null ;
            Nullable<double> deficitRate = null;



            if (UseDifferentDeficitForWeightStep ?? false)
            {
                try
                {
                    deficitRate = Convert.ToDouble(steps.OfType<DeficitForWeightStep>().Where(x => x.FromUnit <= weight && weight <= x.ToUnit).FirstOrDefault().DeficitRate);
                }
                catch (NullReferenceException)
                {
                    deficitRate = 0;
                }
            }
            else
            {
                deficitRate = 0;
            }



            if (UseDifferentRunPerHour ?? false)
            {
                try
                {
                    avarage = steps.OfType<AvarageRunPerRunStep>().Where(x => x.FromUnit <= running && running <= x.ToUnit).FirstOrDefault().AvarageRunPerHour;                

                }
                catch (NullReferenceException)
                {
                    avarage = AvarageRunPerHour;
                }
            }
            else
            {
                avarage = AvarageRunPerHour;
            }

            if (avarage == null)
            {
                totalTimeR = TimeSpan.Zero;
            }
            else
            {
                avarage = avarage - ((avarage / 100) * (long) (deficitRate??0));
                
                //ore
                var tot = (running / avarage);
                var hour = (double)Math.Truncate((decimal)tot);
                var min = (double)Math.Truncate((decimal)((tot - hour) * 60));
                totalTimeR += TimeSpan.FromHours(hour) + TimeSpan.FromMinutes(min);
            }


            var costA = CostPerHourStarting;
            var totalA = (totalTimeA.TotalMinutes) / 60 * Convert.ToDouble(costA, Thread.CurrentThread.CurrentUICulture);

            var totalR = (totalTimeR.TotalMinutes) / 60 * Convert.ToDouble(CostPerHourRunning, Thread.CurrentThread.CurrentUICulture);

            CostAndTime ct = new CostAndTime { Cost = totalA + totalR, Time = totalTimeA + totalTimeR };

            return ct;
        }


        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, int makereadies, double running)
        {
            return GetCost(codOptionTypeOfTask, starts, new PrintingColor(), makereadies, running,0);
        }

        public TaskEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnTime;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}
