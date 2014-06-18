using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class DocumentsDDL :IDDL
    {
        SchemaDb dbS;

        public DocumentsDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {            
            dbS.Ctx = ctx;

            //First Tablef
            dbS.AddTable("documents");
            dbS.AddColumnToTable("documents", "CodDocument", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("documents", "DocumentName", SchemaDb.String, "100");
            dbS.AddColumnToTable("documents", "DateDocument", SchemaDb.Date, "100");
            dbS.AddColumnToTable("documents", "Number", SchemaDb.Int, "0"); // deprecated
            dbS.AddColumnToTable("documents", "EstimateNumber", SchemaDb.String, "20");
            dbS.AddColumnToTable("documents", "EstimateNumberSerie", SchemaDb.String, "20");

            dbS.AddColumnToTable("documents", "Notes", SchemaDb.String, "100");

            dbS.AddColumnToTable("documents", "CodCustomer", SchemaDb.String, "50");
            dbS.AddColumnToTable("documents", "Customer", SchemaDb.String, "50");

            dbS.AddColumnToTable("documents", "Selector", SchemaDb.String, "50");            
            
            //foreign key
            dbS.AddForeignKey("documents", "CodCustomer", "CustomerSuppliers", "CodCustomerSupplier");

            // 0 = Estimate // 1 = ... // 2 = ...
            dbS.AddColumnToTable("documents", "SelectorDocument", SchemaDb.Int, "0");

            // 0 = Ecommerce Estimate
            dbS.AddColumnToTable("documents", "SelectorEstimate", SchemaDb.Int, "0");

            //Index
            dbS.AddIndex("documents", "DocumentName");

            //---------------------------------------------------------------------------------------------

            //Second Table
            //these are the document's rows
            dbS.AddTable("documentproducts");
            dbS.AddColumnToTable("documentproducts", "CodDocumentProduct", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("documentproducts", "CodDocument", SchemaDb.String, "50");
            dbS.AddForeignKey("documentproducts", "CodDocument", "documents", "CodDocument");

            dbS.AddColumnToTable("documentproducts", "ProductName", SchemaDb.String, "255");

            //foreign key
            dbS.AddColumnToTable("documentproducts", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("documentproducts", "CodProduct", "products", "CodProduct");

            dbS.AddColumnToTable("documentproducts", "Quantity", SchemaDb.Long, "0");
            dbS.AddColumnToTable("documentproducts", "UnitPrice", SchemaDb.String, "20");
            dbS.AddColumnToTable("documentproducts", "TotalAmount", SchemaDb.String, "20");

            //dbS.AddColumnToTable("documentproducts", "Markup", SchemaDb.Double, "0");
            //dbS.AddColumnToTable("documentproducts", "GranTotalAmount", SchemaDb.String, "20");


            //Costi

            //---------------------------------------------------------------------------------------------

            //This costs rappresent 
            dbS.AddTable("costs");
            dbS.AddColumnToTable("costs", "CodCost", SchemaDb.StringPK, "50");

            dbS.AddColumnToTable("costs", "CodItemGraph", SchemaDb.String, "20");


            //foreign key for keep all product cost of single product and single quantity
            dbS.AddColumnToTable("costs", "CodDocumentProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("costs", "CodDocumentProduct", "documentproducts", "CodDocumentProduct");

            //this cost would related to a producttask
            dbS.AddColumnToTable("costs", "CodProductTask", SchemaDb.String, "50");
            dbS.AddForeignKey("costs", "CodProductTask", "producttasks", "CodProductTask");

            //this cost would related to a productparttask
            dbS.AddColumnToTable("costs", "CodProductPartTask", SchemaDb.String, "50");
            dbS.AddForeignKey("costs", "CodProductPartTask", "productparttasks", "CodProductPartTask");

            //this cost would related to a implant of productparttask
            dbS.AddColumnToTable("costs", "CodProductPartImplantTask", SchemaDb.String, "50");
            dbS.AddForeignKey("costs", "CodProductPartImplantTask", "productparttasks", "CodProductPartTask");

            //this cost would related to a printable article
            dbS.AddColumnToTable("costs", "CodProductPartPrintableArticle", SchemaDb.String, "50");
            dbS.AddForeignKey("costs", "CodProductPartPrintableArticle", "productpartsprintablearticle", "CodProductPartPrintableArticle");

            //description of type of cost
            dbS.AddColumnToTable("costs", "Description", SchemaDb.String, "255");

            dbS.AddColumnToTable("costs", "Quantity", SchemaDb.Double, "0");
            dbS.AddColumnToTable("costs", "UnitCost", SchemaDb.String, "20");
            dbS.AddColumnToTable("costs", "TotalCost", SchemaDb.String, "20");

            dbS.AddColumnToTable("costs", "Markup", SchemaDb.Double, "0");
            dbS.AddColumnToTable("costs", "GranTotalCost", SchemaDb.String, "20");

            //0 or null = Included
            //1 Aux Cost
            //2 Not Included
            dbS.AddColumnToTable("costs", "TypeOfCalcolous", SchemaDb.Int, "0");



            //force to zero
            dbS.AddColumnToTable("costs", "ForceZero", SchemaDb.Bool, "0");
            //Hide
            dbS.AddColumnToTable("costs", "Hidden", SchemaDb.Bool, "0");
            //Locked
            dbS.AddColumnToTable("costs", "Locked", SchemaDb.Bool, "0");


        }
    }
}
