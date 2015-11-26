using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Collections.Generic;

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

        public override Profile GetSingle(string codProfile)
        {
//            var query = Context.Profiles.Include("Modules").FirstOrDefault(x => x.Name == codProfile);
            var query = Context.Profiles.FirstOrDefault(x => x.Name == codProfile);


            try
            {
                query.Modules = Context.Modules.Where(x => x.Name == codProfile).ToList();
            }
            catch (Exception e)
            {
                query = null;
            }
            
            return query;
        }

        public void SyncroModules(string codProfile)
        {
            var p = GetSingle(codProfile);
            var modules = GetModules();
            foreach (var item in modules)
            {
                var result = p.Modules.SingleOrDefault(x => x.CodModuleName.ToUpper() == item.ToUpper()+codProfile.ToUpper());
                if (result == null)
                {
                    var newMod = new Module();
                    newMod.TimeStampTable = DateTime.Now;
                    newMod.CodModule = item;
                    newMod.CodModuleName = item + codProfile;
                    newMod.Name = p.Name;
                    newMod.Status = (int)Module.StatusType.Valuating;
                    newMod.ActivationDate = DateTime.Now;
                    newMod.ExpirationDate = DateTime.Now.AddMonths(2);
                    newMod.MontlyPrice = "5,00";
                    newMod.Discount = 0;

                    this.Context.Set<Module>().Add(newMod);
                }
                else
                {
                    //result.MontlyPrice = "5,00";
                    //result.Status = (int)Module.StatusType.Valuating;
                    //result.ActivationDate = DateTime.Now;
                    //result.ExpirationDate = DateTime.Now.AddMonths(2);
                    result.CheckStatus();
                    this.Context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                }
                
                Context.SaveChanges();
            }
        }


        public List<string> GetModules() 
        {
            var x = new List<string>();

            x.Add("SmallFormat"); 
            x.Add("WideFormat");
            x.Add("Label");
            x.Add("Cliche");
            return x;
        }

        public Module GetSingleModule(string codModuleName)
        {
            return Context.Modules.SingleOrDefault(x => x.CodModuleName == codModuleName);
        }


        public void SaveModule(Module m)
        {
            this.Context.Entry(m).State = System.Data.Entity.EntityState.Modified;
            Context.SaveChanges();
        }

        public override void SetDbName(string name)
        {
            base.SetDbName(name);
        }
    }
}