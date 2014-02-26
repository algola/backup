using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IGenericRepository<T> : IDisposable where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetSingle(string co);
        void Add(T entity);
        void Delete(T entity);
        void Edit(T entity);
        void Save();
        void SetDbName(string name);        
    }

}