using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

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

            var typeOfTasks = Context.TypeOfTasks.Include("OptionTypeOfTasks");

            foreach (var item in typeOfTasks)
            {
                var lstOptToDel = item.OptionTypeOfTasks.Where(x => x.CodOptionTypeOfTask.Contains("-")).ToArray();

                foreach (var item2 in lstOptToDel)
                {
                    item.OptionTypeOfTasks.Remove(item.OptionTypeOfTasks.FirstOrDefault(y => y.CodOptionTypeOfTask == item2.CodOptionTypeOfTask));
                }
            }

            var tbCode = new TypeOfTask[24];

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
            tbCode[20] = new STAMPAACALDOROTOLO() { CodCategoryOfTask = "PREPOST" };
            tbCode[21] = new FUSTELLATURAROTOLO() { CodCategoryOfTask = "PREPOST" };

            tbCode[22] = new SERIGRAFIA() { CodCategoryOfTask = "STAMPA" };
            tbCode[23] = new SERIGRAFIAROTOLO() { CodCategoryOfTask = "STAMPA" };


            foreach (var item in tbCode)
            {

                var trv = typeOfTasks.FirstOrDefault(x => x.CodTypeOfTask == item.CodTypeOfTask);

                if (trv == null)
                {
                    var x = new TypeOfTask
                    {
                        CodCategoryOfTask = item.CodCategoryOfTask,
                        CodTypeOfTask = item.CodTypeOfTask,
                        TimeStampTable = DateTime.Now,
                        TaskName = item.TaskName
                    };
                    trv = x;
                    this.Add(trv);
                }
                else
                {
                    //                    this.Edit(trv);                    
                }

                var opt = item.OptionTypeOfTasks;

                {
                    foreach (var optItem in opt)
                    {
                        var y = trv.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == optItem.CodOptionTypeOfTask);

                        if (y != null)
                        {
                            //optItem.Copy(y);
                            //this.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                            y.TimeStampTable = DateTime.Now;
                        }
                        else
                        {
                            optItem.TimeStampTable = DateTime.Now;
                            trv.OptionTypeOfTasks.Add(optItem);
                            Context.Entry(optItem).State = System.Data.Entity.EntityState.Added;
                        }
                    }
                }



                this.Save();
            }

            try
            {
                var X = Context.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == "FUSTELLATURA_NO_STACCO");
                Context.Entry(X).State = System.Data.Entity.EntityState.Deleted;

                var Y = Context.OptionTypeOfTasks.FirstOrDefault(x => x.CodOptionTypeOfTask == "FUSTELLATURA_STACCO");
                Context.Entry(Y).State = System.Data.Entity.EntityState.Deleted;

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
            return Context.TypeOfTasks.Where(o => o.CodTypeOfTask == codTypeOfTask);
        }

        public TypeOfTask GetSingle(string codTypeOfTask)
        {
            var query = Context.TypeOfTasks.Include("OptionTypeOfTasks").FirstOrDefault(x => x.CodTypeOfTask == codTypeOfTask);
            return query;
        }

        public void EditOptionTypeOfTask(OptionTypeOfTask entity)
        {
           this.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        
        }
    }
}