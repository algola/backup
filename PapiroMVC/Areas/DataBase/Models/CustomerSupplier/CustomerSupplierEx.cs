using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(CustomerSupplier_MetaData))]
    public abstract partial class CustomerSupplier 
    {

        public enum CustomerSupplierType : int
        {
            Customer = 0,
            Supplier = 1,
        }

        public CustomerSupplierType TypeOfCustomerSupplier
        {
            get;
            protected set;
        }

    }

}
