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
    [MetadataType(typeof(BindingTsk_MetaData))]
    public partial class BindingTsk : PrePostPress
    {

        public BindingTsk()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.Binding;
        }

        #region Proprietà aggiuntive

        #endregion

    }


}
