using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        string GetNewCode(Product a);
        new Product GetSingle(string codProduct);
    }
}
