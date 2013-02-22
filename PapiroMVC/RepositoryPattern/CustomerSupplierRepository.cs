using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;

namespace Services
{

    public class CustomerSupplierRepository : GenericRepository<dbEntities, CustomerSupplier>, ICustomerSupplierRepository
    {
        /// <summary>
        /// Take next code in string numerical order.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public string GetNewCode(CustomerSupplier c)
        {

            var codes = (from COD in this.GetAll() select COD.CodCustomerSupplier).ToArray().OrderBy(x => x, new SemiNumericComparer());

            var csCode = codes.Last();

            if (csCode == null)
                csCode = "0";
            return AlphaCode.GetNextCode(csCode);

        }

        
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