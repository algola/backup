using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class ProductTask_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(ResProductTask), "CodProductTask")]
        [Tooltip(typeof(ResProductTask), "CodProductTaskToolTip")]
        public string CodProductTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "CodProduct")]
        [Tooltip(typeof(ResProductTask), "CodProductToolTip")]
        public string CodProduct { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "ProductTaskName")]
        [Tooltip(typeof(ResProductTask), "ProductTaskNameToolTip")]
        public string ProductTaskName { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "SelectorProductTas")]
        [Tooltip(typeof(ResProductTask), "SelectorProductTaskToolTip")]
        public Nullable<int> SelectorProductTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "CodOptionTypeOfTask")]
        [Tooltip(typeof(ResProductTask), "CodOptionTypeOfTaskToolTip")]
        public string CodOptionTypeOfTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "Hidden")]
        [Tooltip(typeof(ResProductTask), "HiddenToolTip")]
        public Nullable<bool> Hidden { get; set; }

        [DisplayNameLocalized(typeof(ResProductTask), "IndexOf")]
        [Tooltip(typeof(ResProductTask), "IndexOfToolTip")]
        public Nullable<int> IndexOf { get; set; }
    }
}