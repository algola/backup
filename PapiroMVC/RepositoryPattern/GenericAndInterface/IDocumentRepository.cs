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
        IQueryable<DocumentProduct> GetDocumentProductByCodProduct(string codProduct);
        Document GetFromSession();
        void SaveOnSession(Document a);
        IQueryable<Cost> GetCostsByCodDocumentProduct(string codDocumentProduct);
        Cost GetCost(string codCost);
    }
}
