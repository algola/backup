using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Customer_MetaData))]
    public partial class Customer : CustomerSupplier
    {
        public Customer()
        {
            this.TypeOfCustomerSupplier = CustomerSupplierType.Customer;
        }
    }
}