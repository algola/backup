using PapiroMVC.Models.Resources.TaskExecutor;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public partial class BindingEstimatedOnTime_MetaData : TaskEstimatedOn_MetaData
    {
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour4")]
        public Nullable<long> AvarageRunPerHour4 { get; set; }
        public Nullable<System.TimeSpan> StartingTime4 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour6")]
        public Nullable<long> AvarageRunPerHour6 { get; set; }
        public Nullable<System.TimeSpan> StartingTime6 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour8")]
        public Nullable<long> AvarageRunPerHour8 { get; set; }
        public Nullable<System.TimeSpan> StartingTime8 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour12")]
        public Nullable<long> AvarageRunPerHour12 { get; set; }
        public Nullable<System.TimeSpan> StartingTime12 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour16")]
        public Nullable<long> AvarageRunPerHour16 { get; set; }
        public Nullable<System.TimeSpan> StartingTime16 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour24")]
        public Nullable<long> AvarageRunPerHour24 { get; set; }
        public Nullable<System.TimeSpan> StartingTime24 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHour32")]
        public Nullable<long> AvarageRunPerHour32 { get; set; }
        public Nullable<System.TimeSpan> StartingTime32 { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "AvarageRunPerHourBinding")]
        public Nullable<long> AvarageRunPerHourBinding { get; set; }
        public Nullable<System.TimeSpan> StartingTimeBinding { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "CostPerHourRunning")]
        public Nullable<double> CostPerHourRunning { get; set; }
        [DisplayNameLocalized(typeof(ResBindingEstimatedOnTime), "CostPerHourStarting")]
        public Nullable<double> CostPerHourStarting { get; set; }
    }
}

