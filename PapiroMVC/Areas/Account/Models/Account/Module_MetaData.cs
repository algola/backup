using PapiroMVC.Models.Resources.Account;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Module_MetaData
    {
        

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "ActivationDateToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "ActivationDate")]
        public Nullable<System.DateTime> ActivationDate { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "ExpirationDateToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "ExpirationDate")]
        public Nullable<System.DateTime> ExpirationDate { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "MontlyPriceToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "MontlyPrice")]
        public string MontlyPrice { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "DiscountToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Discount")]
        public Nullable<double> Discount { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "StatusModuleToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "StatusModule")]
        public Nullable<int> Status {get; set;}
       
    }
}