using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System;

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
            var csCode = (from COD in this.GetAll() select COD.CodCustomerSupplier).Max();
            return AlphaCode.GetNextCode(csCode ?? "0").PadLeft(6, '0');
        }
        
        public override IQueryable<CustomerSupplier> GetAll()
        {
            return Context.customersuppliers.Include("customersupplierbases").Include("customersupplierbases.typeofbase");
        }

        public override void Edit(CustomerSupplier entity)
        {
            foreach (var item in entity.CustomerSupplierBases)
            {
                Context.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            base.Edit(entity);
        }

        public CustomerSupplier GetSingle(string codCustomerSupplier)
        {
            var query = Context.customersuppliers.Include("customersupplierbases").Include("customersupplierbases.typeofbase").FirstOrDefault(x => x.CodCustomerSupplier == codCustomerSupplier);
            return query;
        }

        public override void Add(CustomerSupplier entity)
        {
            entity.TimeStampTable = DateTime.Now;
            base.Add(entity);
        }

    }
}