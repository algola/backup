using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ICustomerSupplierRepository : IGenericRepository<CustomerSupplier>
    {
        CustomerSupplier GetSingle(string codCustomerSupplier);
    }

}
