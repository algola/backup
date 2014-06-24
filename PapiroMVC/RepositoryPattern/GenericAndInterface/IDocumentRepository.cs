using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IDocumentRepository : IGenericRepository<Document>
    {
        string GetNewCode(Document a);
        string GetNewEstimateNumber(Document a);
        string GetNewOrderNumber(Document a);
        IQueryable<DocumentProduct> GetDocumentProductsByCodProduct(string codProduct);
        DocumentProduct GetDocumentProductByCodDocumentProduct(string codDocumentProduct);
        Document GetFromSession();
        void SaveOnSession(Document a);
        IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct);
        Cost GetCost(string codCost);
        void EditCost(Cost c);
        void Edit(Document entity, bool deep);
        IQueryable<DocumentProduct> GetAllDocumentProducts();

        

        Document GetEstimateEcommerce(string codCustomerSupplier);
    }
}
