using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Areas.DataBase.ViewModels
{

    public class CustomerSupplierSearchOption
    {
        [DisplayNameLocalized(typeof(Strings), "BusinessName")]
        public String BusinessName
        {
            get;
            set;
        }

        [DisplayNameLocalized(typeof(Strings),"City")]
        public String City
        {
            get;
            set;
        }

        [DisplayNameLocalized(typeof(Strings), "VatNumber")]
        public String VatNumber
        {
            get;
            set;
        }

        public bool IsCustomer
        {
            get;
            set;
        }

        public bool IsSupplier
        {
            get;
            set;
        }

    }

    public class CustomerSupplierIndexViewModel
    {
        public List<PapiroMVC.Models.CustomerSupplier> List
        { 
            get; 
            set; 
        }

        /*
        public CustomerSupplierSearchOption Option
        {
            get;
            set;
        }
        */
        public CustomerSupplierIndexViewModel()
        {
            /*
            Option = new CustomerSupplierSearchOption();
            Option.IsCustomer = true;
            Option.IsSupplier = true;
             * */
        }
    }
}