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
    public partial class ProductPartTask_MetaData
    {
        public System.DateTime TimeStampTable { get; set; }
        [DisplayNameLocalized(typeof(ResProductPartTask), "CodProductPartTask")]
        [Tooltip(typeof(ResProductPartTask), "CodProductPartTaskToolTip")]
        public string CodProductPartTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "CodProductPart")]
        [Tooltip(typeof(ResProductPartTask), "CodProductPartToolTip")]
        public string CodProductPart { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "ProductPart")]
        [Tooltip(typeof(ResProductPartTask), "ProductPartProductPartToolTip")]
        public virtual ProductPart ProductPart { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "CodOptionTypeOfTask")]
        [Tooltip(typeof(ResProductPartTask), "CodOptionTypeOfTaskToolTip")]
        public string CodOptionTypeOfTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "OptionTypeOfTask")]
        [Tooltip(typeof(ResProductPartTask), "OptionTypeOfTaskkToolTip")]
        public virtual OptionTypeOfTask OptionTypeOfTask { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "Hidden")]
        [Tooltip(typeof(ResProductPartTask), "HiddenToolTip")]
        public Nullable<bool> Hidden { get; set; }

        [DisplayNameLocalized(typeof(ResProductPartTask), "IndexOf")]
        [Tooltip(typeof(ResProductPartTask), "IndexOfToolTip")]
        public Nullable<int> IndexOf { get; set; }

    }
}