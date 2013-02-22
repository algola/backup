using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ICustomerSupplierRepository : IGenericRepository<CustomerSupplier>
    {
        string GetNewCode(CustomerSupplier c);
        CustomerSupplier GetSingle(string codCustomerSupplier);
    }

}
