using System.Linq;
using PapiroMVC.Models;

namespace Services
{

    public class CustomerSupplierRepository : GenericRepository<dbEntities, CustomerSupplier>, ICustomerSupplierRepository
    {
        public override IQueryable<CustomerSupplier> GetAll()
        {
            return Context.customersuppliers.Include("customersupplierbases").Include("customersupplierbases.typeofbase");
        }

        public override void Edit(CustomerSupplier entity)
        {
            foreach (var item in entity.CustomerSupplierBases)
            {
                Context.Entry(item).State = System.Data.EntityState.Modified;
            }
            base.Edit(entity);
        }

        public CustomerSupplier GetSingle(string codCustomerSupplier)
        {
            var query = Context.customersuppliers.Include("customersupplierbases").Include("customersupplierbases.typeofbase").FirstOrDefault(x => x.CodCustomerSupplier == codCustomerSupplier);
            return query;
        }
    }
}