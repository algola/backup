using InteractivePreGeneratedViews;
using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services
{

    /// <summary>
    /// Returns -1 instead of 1 if y is IsNullOrEmpty when x is Not.
    /// </summary>
    public class EmptyStringsAreLast : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (String.IsNullOrEmpty(y) && !String.IsNullOrEmpty(x))
            {
                return -1;
            }
            else if (!String.IsNullOrEmpty(y) && String.IsNullOrEmpty(x))
            {
                return 1;
            }
            else if (String.IsNullOrEmpty(y) && String.IsNullOrEmpty(x))
            {
                return String.Compare(x, y);
            }
            else
            {
                x = (x.Contains("-")) ? x.Substring(x.LastIndexOf('-') + 1, x.Length - x.LastIndexOf('-') - 1).PadLeft(6, '0') : x;
                y = (y.Contains("-")) ? y.Substring(y.LastIndexOf('-') + 1, y.Length - y.LastIndexOf('-') - 1).PadLeft(6, '0') : y;

                return String.Compare(x, y);
            }

        }
    }

    /// <summary>
    /// use for union
    /// </summary>
    class DocumentProductComparer : IEqualityComparer<DocumentProduct>
    {
        public bool Equals(DocumentProduct p1, DocumentProduct p2)
        {
            if (p1.CodDocumentProduct == null && p1.CodDocumentProduct == null)
            {
                return false;
            }
            else
            {
                return p1.CodDocumentProduct == p2.CodDocumentProduct;

            }
        }

        public int GetHashCode(DocumentProduct p)
        {
            return (p.CodDocumentProduct == null) ? 0 : p.CodDocumentProduct.GetHashCode();
        }
    }


    /// <summary>
    /// use for union
    /// </summary>
    class CostComparer : IEqualityComparer<Cost>
    {
        public bool Equals(Cost p1, Cost p2)
        {
            if (p1.CodCost == null && p1.CodCost == null)
            {
                return false;
            }
            else
            {
                return p1.CodCost == p2.CodCost;
            }
        }

        public int GetHashCode(Cost p)
        {
            return (p.CodCost == null) ? 0 : p.CodCost.GetHashCode();
        }
    }

    class ProductPartComparer : IEqualityComparer<ProductPart>
    {
        public bool Equals(ProductPart p1, ProductPart p2)
        {
            if (p1.CodProductPart == null && p1.CodProductPart == null)
            {
                return false;
            }
            else
            {
                return p1.CodProductPart == p2.CodProductPart;

            }
        }

        public int GetHashCode(ProductPart p)
        {
            return (p.CodProductPart == null) ? 0 : p.CodProductPart.GetHashCode();
        }
    }

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

    public abstract class GenericRepository<C, T> : IGenericRepository<T>, IDisposable
        where T : class
        where C : DbContext, new()
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
                _entities.Database.Connection.ConnectionString = _entities.Database.Connection.ConnectionString.Replace("db", name);
                _entities.Database.Connection.Open();
            }
            _entities.Configuration.ProxyCreationEnabled = false;
            _entities.Configuration.AutoDetectChangesEnabled = false;
            _entities.Configuration.ValidateOnSaveEnabled = false;
        }

        private C _entities;

        public C Context
        {
            get
            {
                var inizio = DateTime.Now;

                if (_entities == null)
                {

                    _entities = new C();

                    _entities.Configuration.ProxyCreationEnabled = false;
                    _entities.Configuration.AutoDetectChangesEnabled = false;
                    _entities.Configuration.ValidateOnSaveEnabled = false;

  
                }

                var tempo = DateTime.Now.Subtract(inizio);
                Console.Write(tempo);

                return _entities;
            }

            set
            {
                _entities = value;
            }
        }

        public virtual void Dispose()
        {

            Dispose(true);
            GC.SuppressFinalize(this);

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_entities != null)
                {
                    _entities.Dispose();
                    _entities = null;
                }
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
            this.Context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public virtual void Save()
        {
            Context.Configuration.AutoDetectChangesEnabled = false;
            Context.Configuration.ValidateOnSaveEnabled = false;
            this.Context.SaveChanges();
        }

        public virtual T GetSingle(string Cod)
        {
            return null;
        }
    }
}