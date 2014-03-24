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
    [MetadataType(typeof(LithoRoll_MetaData))]
    public partial class LithoRoll : Litho
    {

        public LithoRoll()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.LithoRoll;
        }

        #region Added Properties

        #endregion

    }
}
