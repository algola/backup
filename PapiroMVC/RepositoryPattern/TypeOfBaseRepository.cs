using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class TypeOfBaseRepository : GenericRepository<dbEntities, TypeOfBase>, ITypeOfBaseRepository
    {
        public override IQueryable<TypeOfBase> GetAll()
        {
            var c = Context.typeofbase;

            var tbCode = new String[3]; 
            tbCode[0] = "0001";
            tbCode[1] = "0002";
            tbCode[2] = "0003";

            foreach (var item in tbCode)
	        {
                var trv = c.FirstOrDefault(x => x.CodTypeOfBase == item);

                if (trv==null)
                {
                    Context.typeofbase.Add(new TypeOfBase { CodTypeOfBase = item, TimeStampTable = DateTime.Now });
                }

                Context.SaveChanges();
	        }
            
            return Context.typeofbase;
        }

        public IQueryable<TypeOfBase> GetAll(string codTypeOfBase)
        {
            return Context.typeofbase.Where(o => o.CodTypeOfBase == codTypeOfBase);
        }

        public new TypeOfBase GetSingle(string codTypeOfBase)
        {
            var query = Context.typeofbase.FirstOrDefault(x => x.CodTypeOfBase == codTypeOfBase);
            return query;
        }
    }
}