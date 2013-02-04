using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace PapiroMVC.ViewModel
{
    public class CustomerSupplierViewModel
    {
        public CustomerSupplier CustomerSupplier
        {
            get; 
            set;
        }

        public List<CustomerSupplier> CustomerSuppliers
        { 
            get; 
            set;
        }
    }
}