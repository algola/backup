using PapiroMVC.Models.Resources.MenuProduct;
using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class MenuProduct_MetaData : PrePostPress_MetaData
    {

        [DisplayNameLocalized(typeof(ResMenuProduct), "CodMenuProduct")]
        [Tooltip(typeof(ResMenuProduct), "CodMenuProductToolTip")]
        public string CodMenuProduct { get; set; }

        [DisplayNameLocalized(typeof(ResMenuProduct), "Enabled")]
        [Tooltip(typeof(ResMenuProduct), "EnabledToolTip")]
        public Nullable<bool> Enabled { get; set; }

        [DisplayNameLocalized(typeof(ResMenuProduct), "Enabled")]
        [Tooltip(typeof(ResMenuProduct), "EnabledToolTip")]
        public string CodCategory { get; set; }

        [DisplayNameLocalized(typeof(ResMenuProduct), "Hidden")]
        [Tooltip(typeof(ResMenuProduct), "HiddenToolTip")]
        public Nullable<bool> Hidden { get; set; }

        [DisplayNameLocalized(typeof(ResMenuProduct), "IndexOf")]
        [Tooltip(typeof(ResMenuProduct), "IndexOfToolTip")]
        public Nullable<int> IndexOf { get; set; }

        [DisplayNameLocalized(typeof(ResMenuProduct), "IndexOfCategory")]
        [Tooltip(typeof(ResMenuProduct), "IndexOfCategoryToolTip")]
        public Nullable<int> IndexOfCategory { get; set; }
    }


}
