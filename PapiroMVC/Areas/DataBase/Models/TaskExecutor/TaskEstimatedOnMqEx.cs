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
    [MetadataType(typeof(TaskEstimatedOnMq_MetaData))]
    public  partial class TaskEstimatedOnMq: TaskEstimatedOnUnit
    {
        public TaskEstimatedOnMq()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.OnMq;
        }

        #region Proprietà aggiuntive
        #endregion

    }



}
