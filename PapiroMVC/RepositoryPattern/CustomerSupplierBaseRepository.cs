using System.Linq;
using PapiroMVC.Models;

namespace Services
{
    public class CustomerSupplierBaseRepository : GenericRepository<dbEntities, CustomerSupplierBase>, ICustomerSupplierBaseRepository
    {


        public IQueryable<CustomerSupplierBase> GetAll(string codCustomerSupplier)
        {
            return Context.customersupplierbases.Include("typeofbase").Where(o => o.CodCustomerSupplier == codCustomerSupplier);
        }

        public CustomerSupplierBase GetSingle(string codCustomerSupplierBase)
        {
            var query = Context.customersupplierbases.Include("typeofbase").FirstOrDefault(x => x.CodCustomerSupplierBase == codCustomerSupplierBase);
            return query;
        }
    }
}