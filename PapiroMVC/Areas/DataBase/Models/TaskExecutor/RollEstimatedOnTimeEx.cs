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
    [MetadataType(typeof(RollEstimatedOnTime_MetaData))]
    public partial class RollEstimatedOnTime : TaskEstimatedOnTime
    {
        //starts = 1 per avviamento, i successivi son
        public override double GetCost(string codOptionTypeOfTask, double starts, double rollChanges, int colors, double running)
        {
            TimeSpan totalTimeA;
            totalTimeA = this.StartingTime1 ?? TimeSpan.Zero;

            for (int i = 0; i < rollChanges; i++)
            {
                totalTimeA += (StartingTime2 ?? TimeSpan.Zero);
            }

            for (int i = 0; i < colors; i++)
            {
                totalTimeA += (StartingTimePerColor ?? TimeSpan.Zero);
            }

            var costA = CostPerHourStarting;
            var totalA = (totalTimeA.TotalMinutes) / 60 * Convert.ToDouble(costA, Thread.CurrentThread.CurrentUICulture);


            var avrR = AvarageRunPerHour;

            var stepR = this.steps.OfType<AvarageRunPerRunStep>().Where(x => x.FromUnit <= running && x.ToUnit >= running).FirstOrDefault();

            if (stepR != null)
            {
                //leggo il costo differente per ciascun colore
                avrR = stepR.AvarageRunPerHour == null ? avrR : stepR.AvarageRunPerHour;
            }

            TimeSpan totalTimeR = TimeSpan.Zero;
            if (avrR == null)
            {
                totalTimeR = TimeSpan.Zero;
            }
            else
            {
                //minuti //nella flexo AvarageRunPerHour = m/sec
                var tot = (running / avrR ?? 1);
                totalTimeR += TimeSpan.FromMinutes(tot);
            }

            //leggo il costo differente per ciascun colore
            var costH = CostPerHourRunning;

            var step = this.steps.OfType<CostPerColorStep>().Where(x => x.FromUnit == colors).FirstOrDefault();

            if (step != null)
            {
                //leggo il costo differente per ciascun colore
                costH = step.CostPerUnit == null ? costH : step.CostPerUnit.ToString();
            }

            var totalR = (totalTimeR.TotalMinutes) / 60 * Convert.ToDouble(costH, Thread.CurrentThread.CurrentUICulture);

            return totalA + totalR;
        }

        public RollEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.RollEstimatedOnTime;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}
