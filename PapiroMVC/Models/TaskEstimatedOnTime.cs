//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TaskEstimatedOnTime : TaskEstimatedOn
    {
        public Nullable<long> AvarageRunPerHour { get; set; }
        public Nullable<bool> UseDifferentRunPerHour { get; set; }
        public Nullable<System.TimeSpan> StartingTime1 { get; set; }
        public Nullable<System.TimeSpan> StartingTime2 { get; set; }
        public Nullable<System.TimeSpan> TimeForfait { get; set; }
        public string CostPerHourRunning { get; set; }
        public string CostPerHourStarting { get; set; }
        public Nullable<bool> UseDifferentDeficitForWeightStep { get; set; }
    }
}
