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
    [MetadataType(typeof(TaskEstimatedOnUnit_MetaData))]
    public abstract partial class TaskEstimatedOnUnit : TaskEstimatedOn
    {
        #region Proprietà aggiuntive
        #endregion

    }

}
