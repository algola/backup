using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{


    public abstract class CustomerSupplierViewModel
    {

        protected CustomerSupplier customerSupplier;

        public CustomerSupplier CustomerSupplier {
            get
            {
                return customerSupplier;
            }
            set
            {
                customerSupplier = value;
            }
        }
        
        public CustomerSupplierBase CustomerSupplierBase { get; set; }

    }

    public class CustomerViewModel : CustomerSupplierViewModel
    {
    }

    public class SupplierViewModel : CustomerSupplierViewModel
    {
        public SupplierViewModel()
        {
            customerSupplier = new Supplier();
            CustomerSupplierBase = new CustomerSupplierBase();
        }
    }

}