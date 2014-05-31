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
    [MetadataType(typeof(ControlTableRoll_MetaData))]
    public partial class ControlTableRoll : PrePostPress
    {

        public ControlTableRoll()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.ControlTableRoll;
        }

        #region Proprietà aggiuntive

        #endregion

    }

}
