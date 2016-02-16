using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;
using System.Reflection;
using PapiroMVC.Models.Resources.Products;

namespace Services
{
    public class TypeOfTaskRepository : GenericRepository<dbEntities, TypeOfTask>, ITypeOfTaskRepository
    {

        public IQueryable<OptionTypeOfTask> GetAllOptionTypeOfTask()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);
            return Context.OptionTypeOfTasks;
        }

        public OptionTypeOfTask GetSingleOptionTypeOfTask(string id)
        {
            return this.GetAllOptionTypeOfTask().FirstOrDefault(x => x.CodOptionTypeOfTask == id);
        }

        public override IQueryable<TypeOfTask> GetAll()
        {
            Console.WriteLine(Context.Database.Connection.ConnectionString);

            var typeOfTasksDb = Context.TypeOfTasks.Include("OptionTypeOfTasks");

            foreach (var item in typeOfTasksDb)
            {
                var lstOptToDel = item.OptionTypeOfTasks.Where(x => x.CodOptionTypeOfTask.Contains("-")).ToArray();

                foreach (var item2 in lstOptToDel)
                {
                    item.OptionTypeOfTasks.Remove(item.OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item2.CodOptionTypeOfTask));
                }
            }

            var tbCode = new TypeOfTask[26];

            tbCode[0] = new PIEGA() { CodCategoryOfTask = "PREPOST" };
            tbCode[1] = new PLASTIFICATURA() { CodCategoryOfTask = "PREPOST" };
            tbCode[2] = new VERNICIATURA() { CodCategoryOfTask = "PREPOST" };
            tbCode[3] = new RILEGATURA_BROSSURA() { CodCategoryOfTask = "RILEGATURA" };
            tbCode[4] = new RILEGATURA_PM() { CodCategoryOfTask = "RILEGATURA" };
            tbCode[5] = new RILEGATURA_SPIRALE() { CodCategoryOfTask = "RILEGATURA" };
            tbCode[6] = new TAGLIO() { CodCategoryOfTask = "PREPOST" };
            tbCode[7] = new FUSTELLATURA() { CodCategoryOfTask = "PREPOST" };
            tbCode[8] = new STAMPA() { CodCategoryOfTask = "STAMPA" };
            tbCode[9] = new ACCOPPIATURA() { CodCategoryOfTask = "PREPOST" };
            tbCode[10] = new INTERCALATURA() { CodCategoryOfTask = "PREPOST" };
            tbCode[11] = new FORO() { CodCategoryOfTask = "PREPOST" };
            tbCode[12] = new RILIEVO_A_SECCO() { CodCategoryOfTask = "PREPOST" };
            tbCode[13] = new STAMPARIGIDO() { CodCategoryOfTask = "STAMPARIGIDO" };
            
            tbCode[14] = new STAMPAOFF() { CodCategoryOfTask = "STAMPA" };
            tbCode[15] = new STAMPAOFFeDIGITALE() { CodCategoryOfTask = "STAMPA" };
            tbCode[16] = new STAMPADIGITALE() { CodCategoryOfTask = "STAMPA" };

            tbCode[17] = new STAMPAETICHROTOLO() { CodCategoryOfTask = "STAMPAETICHROTOLO" };

            tbCode[18] = new STAMPAMORBIDO() { CodCategoryOfTask = "STAMPAMORBIDO" };

            tbCode[19] = new TAVOLOCONTROLLO() { CodCategoryOfTask = "TAVOLOCONTROLLO" };
            tbCode[20] = new STAMPAACALDOROTOLO() { CodCategoryOfTask = "PREPOSTROLL" };
            tbCode[21] = new FUSTELLATURAROTOLO() { CodCategoryOfTask = "PREPOSTROLL" };

            tbCode[22] = new SERIGRAFIA() { CodCategoryOfTask = "STAMPA" };
            tbCode[23] = new SERIGRAFIAROTOLO() { CodCategoryOfTask = "STAMPA" };

            tbCode[24] = new SERIGRAFIASOLOTIPI() { CodCategoryOfTask = "STAMPA" };

            tbCode[25] = new STAMPANEW() { CodCategoryOfTask = "STAMPA" };

            foreach (var item in tbCode)
            {

                var typeOfTaskDb = typeOfTasksDb.FirstOrDefault(x => x.CodTypeOfTask == item.CodTypeOfTask);


                if (typeOfTaskDb == null)
                {
                    var x = new TypeOfTask
                    {
                        CodCategoryOfTask = item.CodCategoryOfTask,
                        CodTypeOfTask = item.CodTypeOfTask,
                        TimeStampTable = DateTime.Now,
                        TaskName =  item.TaskName
                        
                    };
                    typeOfTaskDb = x;
                    this.Add(typeOfTaskDb);
                }
                else
                {
                    item.TimeStampTable = DateTime.Now;

                    Context.Entry(typeOfTaskDb).CurrentValues.SetValues(item);
                    Context.Entry(typeOfTaskDb).State = System.Data.Entity.EntityState.Modified;
                }

                var opt = item.OptionTypeOfTasks;

                {
                    foreach (var optItem in opt)
                    {
                        var y = typeOfTaskDb.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == optItem.CodOptionTypeOfTask);

                        if (y != null)
                        {
                            optItem.TimeStampTable = DateTime.Now;
                //            this.EditOptionTypeOfTask(optItem);
                        }
                        else
                        {
                            optItem.TimeStampTable = DateTime.Now;
                            typeOfTaskDb.OptionTypeOfTasks.Add(optItem);
                            Context.Entry(optItem).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                }



                this.Save();
            }

            try
            {
                var X = Context.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == "FUSTELLATURA_NO_STACCO");
                if (X!=null)
                {
                    Context.Entry(X).State = System.Data.Entity.EntityState.Deleted;                    
                }

                var Y = Context.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == "FUSTELLATURA_STACCO");
                if (Y!=null)
                {
                    Context.Entry(Y).State = System.Data.Entity.EntityState.Deleted;                    
                }

                var Z = Context.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == "TAGLIO_DOPPIO");
                if (Z!=null)
                {
                    Context.Entry(Z).State = System.Data.Entity.EntityState.Deleted;
                }

                Context.SaveChanges();
            }
            catch (Exception)
            {

            }


            return Context.TypeOfTasks.Include("OptionTypeOfTasks");
        }


        class OptionTypeOfTaskComparer : IEqualityComparer<OptionTypeOfTask>
        {
            #region IEqualityComparer<Contact> Members

            public bool Equals(OptionTypeOfTask x, OptionTypeOfTask y)
            {
                return x.CodOptionTypeOfTask.Equals(y.CodOptionTypeOfTask);
            }

            public int GetHashCode(OptionTypeOfTask obj)
            {
                return obj.CodOptionTypeOfTask.GetHashCode();
            }

            #endregion
        }


        public IQueryable<TypeOfTask> GetAll(string codTypeOfTask)
        {
            return Context.TypeOfTasks.AsNoTracking().Where(o => o.CodTypeOfTask == codTypeOfTask);
        }

        public TypeOfTask GetSingle(string codTypeOfTask)
        {
            var query = Context.TypeOfTasks.AsNoTracking().Include("OptionTypeOfTasks").FirstOrDefault(x => x.CodTypeOfTask == codTypeOfTask);
            return query;
        }

        public void EditOptionTypeOfTask(OptionTypeOfTask entity)
        {


            var dbEnt = this.Context.OptionTypeOfTasks.SingleOrDefault(x => x.CodOptionTypeOfTask == entity.CodOptionTypeOfTask);

            if (dbEnt !=null)
            {
                if (dbEnt.CodTypeOfTask == "PLASTIFICATURA")
                {
                    Console.Write(dbEnt.CodOptionTypeOfTask);
                    Console.Write(dbEnt.IdexOf);
                }
                    Context.Entry(dbEnt).CurrentValues.SetValues(entity);
                    Context.Entry(dbEnt).State = System.Data.Entity.EntityState.Modified;
            }
            
        
        }
    }
}