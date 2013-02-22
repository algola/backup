using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Services
{


    public class SemiNumericComparer : IComparer<string>
    {
        public int Compare(string s1, string s2)
        {
            if (IsNumeric(s1) && IsNumeric(s2))
            {
                if (Convert.ToInt32(s1) > Convert.ToInt32(s2)) return 1;
                if (Convert.ToInt32(s1) < Convert.ToInt32(s2)) return -1;
                if (Convert.ToInt32(s1) == Convert.ToInt32(s2)) return 0;
            }

            if (IsNumeric(s1) && !IsNumeric(s2))
                return -1;

            if (!IsNumeric(s1) && IsNumeric(s2))
                return 1;

            return string.Compare(s1, s2, true);
        }

        public static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }



    public abstract class GenericRepository<C, T> : IGenericRepository<T>
        where T : class
        where C : DbContext , new()
    {
        public virtual void SetDbName(string name)
        {
            if (_entities != null)
            {
                _entities.Dispose();
            }

            _entities = new C();
            if (name != null)
            {
                _entities.Database.Connection.ConnectionString = _entities.Database.Connection.ConnectionString.Replace("db",name);
                _entities.Database.Connection.Open();
            } 
            _entities.Configuration.ProxyCreationEnabled = false;
        }

        private C _entities;

        public C Context
        {
            get 
            {                
                if (_entities == null)
                {                                        
                    _entities = new C();
                    _entities.Configuration.ProxyCreationEnabled = false;
                }
                 
                return _entities; 
            }
            
            set 
            { 
                _entities = value; 
            }
        }

        public virtual IQueryable<T> GetAll()
        {
            IQueryable<T> query = this.Context.Set<T>();
            return query;
        }

        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = this.Context.Set<T>().Where(predicate);
            return query;
        }

        public virtual void Add(T entity)
        {
            this.Context.Set<T>().Add(entity);
        }

        public virtual void Delete(T entity)
        {
            this.Context.Set<T>().Remove(entity);
        }

        public virtual void Edit(T entity)
        {
            this.Context.Entry(entity).State = System.Data.EntityState.Modified;
        }

        public virtual void Save()
        {
            this.Context.SaveChanges();
        }
    }
}