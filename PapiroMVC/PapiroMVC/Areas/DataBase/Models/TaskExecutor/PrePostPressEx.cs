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
    [MetadataType(typeof(PrePostPress_MetaData))]
    public partial class PrePostPress : TaskExecutor
    {

        public PrePostPress()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.PrePostPress;
        }

        #region Proprietà aggiuntive

        #endregion

    }

}
