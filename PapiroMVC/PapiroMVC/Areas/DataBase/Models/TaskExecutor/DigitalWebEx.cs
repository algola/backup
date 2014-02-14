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
    [MetadataType(typeof(DigitalRoll_MetaData))]
    public partial class DigitalRoll : Digital
    {

        public DigitalRoll()
        {
            this.TypeOfExecutor = TaskExecutor.ExecutorType.DigitalRoll;
        }

        #region Added Properties

        #endregion

    }
}
