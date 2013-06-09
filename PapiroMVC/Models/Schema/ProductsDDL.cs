using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class ProductsDDL :IDDL
    {
        SchemaDb dbS;

        public ProductsDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("products");
            dbS.AddColumnToTable("products", "CodProduct", SchemaDb.StringPK, "50");
//            dbS.AddColumnToTable("products", "ProductName", SchemaDb.String, "100");


            dbS.AddColumnToTable("products", "CodCustomer", SchemaDb.String, "50");

            //customer
            dbS.AddColumnToTable("products", "CodCustomer", SchemaDb.String, "50");
            dbS.AddColumnToTable("products", "Customer", SchemaDb.String, "50");

            //foreign key
            dbS.AddForeignKey("products", "CodCustomer", "CustomerSuppliers", "CodCustomerSupplier");

            // 0 = Estimate // 1 = Estimate Model // 2 = Order ... 
            dbS.AddColumnToTable("products", "SelectorProduct", SchemaDb.Int, "0");

            //Index
            dbS.AddIndex("products", "ProductName");


            //---------------------------------------------------------------------------------------------

            //Second Table
            dbS.AddTable("productparts");
            dbS.AddColumnToTable("productparts", "CodProductPart", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("productparts", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("productparts", "CodProduct", "products", "CodProduct");

            dbS.AddColumnToTable("productparts", "ProductPartName", SchemaDb.String, "100");

            // 0 = single sheet // 1 = Book // 2 = Block // 3 = Roll // etc... 
            dbS.AddColumnToTable("productparts", "SelectorProductPart", SchemaDb.Int, "0");


            //---------------------------------------------------------------------------------------------

            //Third Table
            dbS.AddTable("productpartsprintablearticle");
            dbS.AddColumnToTable("productpartsprintablearticle", "CodProductPartPrintableArticle", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("productpartsprintablearticle", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productpartsprintablearticle", "CodProductPart", "productparts", "CodProductPart");

            dbS.AddColumnToTable("productpartsprintablearticle", "ProductPartPrintableArticleName", SchemaDb.String, "100");
            // 0 = single sheet // 1 = Book // 2 = Block // 3 = Roll // etc... 
            dbS.AddColumnToTable("productpartsprintablearticle", "SelectorProductPartPrintableArticle", SchemaDb.Int, "0");


            //---------------------------------------------------------------------------------------------


            //Second Table
            dbS.AddTable("producttasks");
            dbS.AddColumnToTable("producttasks", "CodProductTask", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("producttasks", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("producttasks", "CodProduct", "products", "CodProduct");

            dbS.AddColumnToTable("producttasks", "ProductTaskName", SchemaDb.String, "100");

            // 0 = single sheet // 1 = Book // 2 = Block // 3 = Roll // etc... 
            dbS.AddColumnToTable("producttasks", "SelectorProductTask", SchemaDb.Int, "0");

            //---------------------------------------------------------------------------------------------

            //task related at parts

            //Third Table
            dbS.AddTable("productparttasks");
            dbS.AddColumnToTable("productparttasks", "CodProductPartTask", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("productparttasks", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productparttasks", "CodProductPart", "productparts", "CodProductPart");

            dbS.AddColumnToTable("productparttasks", "CodProductTask", SchemaDb.String, "50");
            dbS.AddForeignKey("productparttasks", "CodProductTask", "producttasks", "CodProductTask");


        }
    }
}
