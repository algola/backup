using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IWarehouseRepository : IGenericRepository<Warehouse>
    {
        IQueryable<WarehouseArticleMov> GetAllMovs(string codWarehouseArticle);
        void AddMov(WarehouseArticleMov entity);
        void EditMov(WarehouseArticleMov entity);
        string GetNewMovCode(WarehouseArticleMov entity);
        void UpdateArticle(Warehouse entity);
        Warehouse GetSingleArticle(string codArticle,string codWarehouse);
        Warehouse GetSingleProduct(string codProduct, string codWarehouse);
        IQueryable<WarehouseArticleMov> GetAllMovsProduct(string codProduct);
        IQueryable<WarehouseArticleMov> GetAllMovsArticle(string codArticle);

        //list of kind of locations 
        IQueryable<WarehouseSpec> GetWarehouseList();

    }
}

