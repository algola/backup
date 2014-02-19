using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [Serializable]
    [KnownType(typeof(TypeOfTask))]

   // [MetadataType(typeof(Step_MetaData))]
    public partial class TypeOfTask
    {       
        #region Proprietà aggiuntive

        public bool IsSelected { get; set; }

        #endregion
    }
}
