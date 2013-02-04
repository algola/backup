using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class Supplier : CustomerSupplier
    {
        public Supplier()
        {
            this.TypeOfCustomerSupplier = CustomerSupplierType.Supplier;
        }
    }
}