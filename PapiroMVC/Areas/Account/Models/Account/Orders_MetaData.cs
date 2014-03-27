using PapiroMVC.Models.Resources.Account;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Orders_MetaData
    {
    
        public System.DateTime TimeStampTable { get; set; }
        public string CodOrder { get; set; }

        public string Name { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "OrderDateToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "OrderDate")]
        public Nullable<System.DateTime> OrderDate { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "StatusToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Status")]
        public Nullable<int> Status { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "PriceToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Price")]
        public string Price { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "DiscountOrderToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "DiscountOrder")]
        public Nullable<double> Discount { get; set; }

        [Tooltip(typeof(PapiroMVC.Models.Resources.Account.Registration), "TotalToolTip")]
        [DisplayNameLocalized(typeof(PapiroMVC.Models.Resources.Account.Registration), "Total")]
        public string Total { get; set; }
    
       
    }
}

     