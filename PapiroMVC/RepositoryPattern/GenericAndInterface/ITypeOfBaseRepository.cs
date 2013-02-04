using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ITypeOfBaseRepository : IGenericRepository<TypeOfBase>
    {
        TypeOfBase GetSingle(string codTypeOfBase);
        IQueryable<TypeOfBase> GetAll(string codCustomerSupplier);
    }

}
