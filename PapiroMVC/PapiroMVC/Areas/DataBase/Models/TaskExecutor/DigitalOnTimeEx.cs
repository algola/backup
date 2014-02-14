﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DigitalOnTime_MetaData))]
    public partial class DigitalOnTime: TaskEstimatedOnTime
    {
        public DigitalOnTime()
        {
            this.TypeOfEstimatedOn = TaskEstimatedOn.EstimatedOnType.DigitalOnTime;
        }

        #region Proprietà aggiuntive

        #endregion


    }
}