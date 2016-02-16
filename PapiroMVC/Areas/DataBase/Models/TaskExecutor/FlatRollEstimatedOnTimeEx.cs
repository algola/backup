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
    [MetadataType(typeof(FlatRollEstimatedOnTime_MetaData))]
    public partial class FlatRollEstimatedOnTime : TaskEstimatedOnTime
    {
        //starts = 1 per avviamento, i successivi son
        public override CostAndTime GetCost(string codOptionTypeOfTask, double starts, double retroStarts, double rollChanges, int colors, double running)
        {

            TimeSpan totalTimeA = TimeSpan.Zero;
            TimeSpan totalTimeR = TimeSpan.Zero;

            double costA =0;

            var totalA = (totalTimeA.TotalMinutes) / 60 * costA;

            Nullable<long> avrR = AvarageRunPerHour ?? 0;
            string costH = CostPerHourRunning;


            if (codOptionTypeOfTask.Contains("STAMPAETIC"))
            {
                totalTimeA = this.StartingTime1 ?? TimeSpan.Zero;

                for (int i = 0; i < rollChanges; i++)
                {
                    totalTimeA += (StartingTime2 ?? TimeSpan.Zero);
                }

                for (int i = 0; i < colors; i++)
                {
                    totalTimeA += (StartingTimePerColor ?? TimeSpan.Zero);
                }

                for (int i = 0; i < retroStarts; i++)
                {
                    totalTimeA += (StartingTimeRetro ?? TimeSpan.Zero);
                }

                costA = Convert.ToDouble(CostPerHourStarting, Thread.CurrentThread.CurrentUICulture);
//                var totalA = (totalTimeA.TotalMinutes) / 60 * Convert.ToDouble(costA, Thread.CurrentThread.CurrentUICulture);

                var stepR = this.steps.OfType<AvarageRunPerRunStep>().Where(x => x.FromUnit <= running && x.ToUnit >= running).FirstOrDefault();

                if (stepR != null)
                {
                    //leggo il costo differente per ciascun colore
                    avrR = stepR.AvarageRunPerHour == null ? avrR : stepR.AvarageRunPerHour;
                }

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

                var step = this.steps.OfType<CostPerColorStep>().Where(x => x.FromUnit == colors).FirstOrDefault();

                if (step != null)
                {
                    //leggo il costo differente per ciascun colore
                    costH = step.CostPerUnit == null ? costH : step.CostPerUnit.ToString();
                }

            }


            if (codOptionTypeOfTask.Contains("SERIGRAFIA"))
            {
                totalTimeA = TimeSpan.Zero;
                for (int i = 0; i < colors; i++)
                {
                    totalTimeA += (StartingTimeSerigraphy ?? TimeSpan.Zero);
                }

                costA = Convert.ToDouble(CostPerHourStartingSerigraphy, Thread.CurrentThread.CurrentUICulture);
            }


            if (codOptionTypeOfTask.Contains("STAMPAACALDO"))
            {
                totalTimeA =  TimeSpan.Zero;
                for (int i = 0; i < colors; i++)
                {
                    totalTimeA += (StartingTimeFoilStamping ?? TimeSpan.Zero);
                }

                costA = Convert.ToDouble(CostPerHourStartingFoilStamping, Thread.CurrentThread.CurrentUICulture);
            }




            if (codOptionTypeOfTask.Contains("SERIGRAFIA"))
            {
                avrR=  AvarageRunPerHourSerigraphy;
                costH = CostPerHourRunningSerigraphy;                
            }


            if (codOptionTypeOfTask.Contains("STAMPAACALDO"))
            {
                avrR = AvarageRunPerHourFoilStamping;
                costH = CostPerHourRunningFoilStamping;
            }

            //run
            if (avrR == null | avrR == 0)
            {
                totalTimeR = TimeSpan.Zero;
            }
            else
            {
                //minuti //nella flexo AvarageRunPerHour = m/sec
                var tot = (running / avrR ?? 1);
                totalTimeR += TimeSpan.FromHours(tot);
            }

            var totalR = (totalTimeR.TotalMinutes) / 60 * Convert.ToDouble(costH, Thread.CurrentThread.CurrentUICulture);
            CostAndTime ct = new CostAndTime { Cost = totalA + totalR, Time = totalTimeA + totalTimeR };

            return ct;
        }

        public FlatRollEstimatedOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.FlatRollEstimatedOnTime;
        }

        #region Proprietà aggiuntive

        #endregion

    }
}
