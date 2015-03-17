using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        void SincroSupplier(Article a, ICustomerSupplierRepository customerSupplierRepository, string supplierMaker, string supplyerBuy);
        string GetNewCode(Article a, ICustomerSupplierRepository customerSupplierRepository, string supplierMaker, string supplyerBuy);
        //Article GetSingle(string codArticle);   
        IQueryable<Article> GetForImport();

    }
}

