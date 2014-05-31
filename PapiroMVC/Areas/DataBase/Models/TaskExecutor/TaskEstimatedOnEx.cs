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
    [MetadataType(typeof(TaskEstimatedOn_MetaData))]
    public abstract partial class TaskEstimatedOn
    {
       
        #region Proprietà aggiuntive
        public enum EstimatedOnType
        {
            OnRun,
            OnTime,
            OnMq,
            BindingOnTime,
            BindingOnRun,
            DigitalOnTime,
            DigitalOnRun,
            PlotterOnMq,
            RollEstimatedOnTime,
            ControlTableRollEstimatedOnTime
        }

        public EstimatedOnType TypeOfEstimatedOn
        {
            get;
            protected set;
        }

        #endregion

        //usato per i rotoli
        public virtual double GetCost(string codOptionTypeOfTask, double starts, double rollChanges, int makereadis, double running)
        {
            throw new NotImplementedException();
        }

        public virtual double GetCost(string codOptionTypeOfTask, double starts, int makereadis, double running)
        {
            throw new NotImplementedException();
        }

        public virtual double GetCost(string codOptionTypeOfTask, double starts, double mq)
        {
            throw new NotImplementedException();
        }

    }
}
