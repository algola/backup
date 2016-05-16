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
        class ModuleOrder
        {
            public string CodModule { get; set; }
            public int IndexOf { get; set; }
            public string PermaLink { get; set; }
        }


        private List<ModuleOrder> modulesOrders { get; set; }


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


        public ProfileRepository()
        {

            modulesOrders = new List<ModuleOrder>();

            modulesOrders.Add(new ModuleOrder { CodModule = "Estimate", IndexOf = 0, PermaLink =  "http://www.gestionestampa.com/papirostar/preventivi-modulo-assistenza/" });
            modulesOrders.Add(new ModuleOrder { CodModule = "SmallFormat", IndexOf = 1, PermaLink = " http://www.gestionestampa.com/papirostar/piccolo-formato-modulo-assistenza/" });
            modulesOrders.Add(new ModuleOrder { CodModule = "WideFormat", IndexOf = 2, PermaLink = " http://www.gestionestampa.com/papirostar/grande-formato-modulo-assistenza/" });
            modulesOrders.Add(new ModuleOrder { CodModule = "Label", IndexOf = 3, PermaLink = "http://www.gestionestampa.com/papirostar/modulo-papiro-star-etichette/" });
            modulesOrders.Add(new ModuleOrder { CodModule = "Cliche", IndexOf = 4, PermaLink = "" });
            modulesOrders.Add(new ModuleOrder { CodModule = "Planning", IndexOf = 5, PermaLink = " http://www.gestionestampa.com/papirostar/pianificazione-modulo-assistenza/ ‎" });
            modulesOrders.Add(new ModuleOrder { CodModule = "Warehouse", IndexOf = 6, PermaLink = " http://www.gestionestampa.com/papirostar/grande-formato-modulo-assistenza/" });

        }

        public override Profile GetSingle(string codProfile)
        {
            //            var query = Context.Profiles.Include("Modules").FirstOrDefault(x => x.Name == codProfile);
            var query = Context.Profiles.FirstOrDefault(x => x.Name == codProfile);


            try
            {
                query.Modules = Context.Modules.Where(x => x.Name == codProfile).ToList();

                foreach (var item in query.Modules)
                {
                    SyncroUsers(item);
                }


            }
            catch (Exception e)
            {
                query = null;
            }

            return query;
        }


        private string GetPermaLink(string item)
        {
            var ret = String.Empty;

            try
            {
                ret = modulesOrders.FirstOrDefault(x => x.CodModule == item).PermaLink;
            }
            catch (Exception)
            {
                
            }
        
            return ret;

        }


        private int GetIndexOf(string item)
        {
            int ret = 1000;

            try
            {
                ret = modulesOrders.FirstOrDefault(x => x.CodModule == item).IndexOf;
            }
            catch (Exception)
            {

            }

            return ret;

        }


        public void SyncroModules(string codProfile)
        {
            var p = GetSingle(codProfile);
            var modules = GetModules();
            foreach (var item in modules)
            {
                var result = p.Modules.SingleOrDefault(x => x.CodModuleName.ToUpper() == item.ToUpper() + codProfile.ToUpper());
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
                    newMod.PermaLink = GetPermaLink(item);
                    newMod.IndexOf = GetIndexOf(item);

                    this.Context.Set<Module>().Add(newMod);
                }
                else
                {
                    //result.MontlyPrice = "5,00";
                    //result.Status = (int)Module.StatusType.Valuating;
                    //result.ActivationDate = DateTime.Now;
                    //result.ExpirationDate = DateTime.Now.AddMonths(2);
                    result.PermaLink = GetPermaLink(item);
                    result.CheckStatus();
                    result.IndexOf = GetIndexOf(item);
                    this.Context.Entry(result).State = System.Data.Entity.EntityState.Modified;
                }

                Context.SaveChanges();
            }
        }


        public List<string> GetModules()
        {
            var x = new List<string>();

            x= modulesOrders.Select(z => z.CodModule).ToList();

            return x;
        }

        protected void SyncroUsers(Module m)
        {
            if (m.CodModule == "SmallFormat" ||
                m.CodModule == "WideFormat" ||
                m.CodModule == "Label" ||
                m.CodModule == "Cliche" ||
                m.CodModule == "Warehouse"
                )
            {                
                var est = Context.Modules.SingleOrDefault(x => x.CodModuleName == "Estimate" + m.Name);
                m.Users = est!=null?est.Users:0;
            }

        }

        public Module GetSingleModule(string codModuleName)
        {
            var m = Context.Modules.SingleOrDefault(x => x.CodModuleName == codModuleName);
            SyncroUsers(m);
            return m;
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