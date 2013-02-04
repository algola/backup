using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ICustomerSupplierBaseRepository : IGenericRepository<CustomerSupplierBase>
    {
        CustomerSupplierBase GetSingle(string codCustomerSupplierBase);
        IQueryable<CustomerSupplierBase> GetAll(string codCustomerSupplier);
    }

}
