using PapiroMVC.Models.Resources.Account;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class OrderRows_MetaData
    {
       

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "DescriptionToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Description")]
        public string Description { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "MontlyPriceOrderRowsToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "MontlyPriceOrderRows")]
        public string MontlyPrice { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "MonthsToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Months")]
        public Nullable<int> Months { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "DiscountOrderRowsToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "DiscountOrderRows")]
        public string Discount { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "TotalOrderRowsToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "TotalOrderRows")]
        public string Total { get; set; }
       
    }
}