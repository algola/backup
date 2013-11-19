using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;

namespace Services
{
    public class ProfileRepository : GenericRepository<profilesEntities, Profile>, IProfileRepository
    {
        public override void Add(Profile entity)
        {
            base.Add(entity);
        }

        public override IQueryable<Profile> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.Profiles;
        }

        public override void Edit(Profile entity)
        {
            base.Edit(entity);
        }

        public new Profile GetSingle(string codProfile)
        {
            var query = Context.Profiles.FirstOrDefault(x => x.Name == codProfile);
            return query;
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}