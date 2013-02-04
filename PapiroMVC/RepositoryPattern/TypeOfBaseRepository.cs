using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class TypeOfBaseRepository : GenericRepository<dbEntities, TypeOfBase>, ITypeOfBaseRepository
    {

        public IQueryable<TypeOfBase> GetAll(string codTypeOfBase)
        {
            return Context.typeofbase.Where(o => o.CodTypeOfBase == codTypeOfBase);
        }

        public TypeOfBase GetSingle(string codTypeOfBase)
        {
            var query = Context.typeofbase.FirstOrDefault(x => x.CodTypeOfBase == codTypeOfBase);
            return query;
        }
    }
}