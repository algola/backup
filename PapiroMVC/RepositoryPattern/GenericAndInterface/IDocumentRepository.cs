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



        dbEntities GetContext();

        Document GetFromSession();
        void SaveOnSession(Document a);
        IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct);
        IQueryable<Cost> GetCostsByCodDocumentProductNoT(string codDocumentProduct);

        Cost GetCost(string codCost);
        Cost GetCostNoT(string codCost);

        void EditCost(Cost c);
        void Edit(Document entity, bool deep);
        IQueryable<DocumentProduct> GetAllDocumentProducts();
        IQueryable<DocumentProduct> GetAllDocumentProductsSimply();
        
        IQueryable<Product> GetAllProducts();

        IQueryable<State> GetAllStates();
        IQueryable<DocumentState> GetAllDocumentStates(string codDocument);

        IQueryable<ReportOrderName> GetAllReportOrderName(string databaseName);

        Document GetEstimateEcommerce(string codCustomerSupplier);

        void DeleteDocumentProduct(DocumentProduct documentProduct);
    }
}
