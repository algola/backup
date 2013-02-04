﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Services
{
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