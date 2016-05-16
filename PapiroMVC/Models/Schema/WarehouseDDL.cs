using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class WarehouseArticlesDDL :IDDL
    {
        SchemaDb dbS;

        public WarehouseArticlesDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            
            dbS.Ctx = ctx;

            //WarehouseArticleCost
            dbS.AddTable("warehouses");
            dbS.AddColumnToTable("warehouses", "CodWarehouse", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("warehouses", "WarehouseName", SchemaDb.String, "100");


            //First Table
            dbS.AddTable("warehousearticles");
            dbS.AddColumnToTable("warehousearticles", "CodWarehouseArticle", SchemaDb.StringPK, "50");
     //       dbS.AddColumnToTable("warehousearticles", "WarehouseArticleName", SchemaDb.String, "100");

            // 0 = Article // 1 = Final Product
            dbS.AddColumnToTable("warehousearticles", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("warehousearticles", "CodArticle", SchemaDb.String, "100");
            dbS.AddColumnToTable("warehousearticles", "CodProduct", SchemaDb.String, "100");

            dbS.AddForeignKey("warehousearticles", "CodArticle", "articles", "CodArticle");
            dbS.AddForeignKey("warehousearticles", "CodProduct", "products", "CodProduct");

            dbS.AddColumnToTable("warehousearticles", "UnitOfMeasureStore", SchemaDb.String, "50");
            dbS.AddColumnToTable("warehousearticles", "UnitOfMeasureMov", SchemaDb.String, "50");
            dbS.AddColumnToTable("warehousearticles", "UmConversion", SchemaDb.Double, "0");

            dbS.AddColumnToTable("warehousearticles", "QuantityOnHand", SchemaDb.Double, "0");
            dbS.AddColumnToTable("warehousearticles", "Available", SchemaDb.Double, "0");
            dbS.AddColumnToTable("warehousearticles", "PotentialQuantityOnHand", SchemaDb.Double, "0");
            dbS.AddColumnToTable("warehousearticles", "PotentialAvailable", SchemaDb.Double, "0");

            //location!!!
            dbS.AddColumnToTable("warehousearticles", "CodWarehouse", SchemaDb.String, "50");
            dbS.AddForeignKey("warehousearticles", "CodWarehouse", "warehouses", "CodWarehouse");

            //this is the min quantity of this article / product
            dbS.AddColumnToTable("warehousearticles", "MinQuantity", SchemaDb.Double, "0");

            //Last Update
            dbS.AddColumnToTable("warehousearticles", "LastUpdate", SchemaDb.Date, "0");

            //Index
            dbS.AddIndex("warehousearticles", "WarehouseArticleName");
            dbS.AddIndex("warehousearticles", "CodArticleFrom");

            //WarehouseArticleCost
            dbS.AddTable("warehousearticlemovs");

            //primary key
            dbS.AddColumnToTable("warehousearticlemovs", "CodWarehouseArticleMov", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("warehousearticlemovs", "CodWarehouseArticle", SchemaDb.String, "50");
            dbS.AddForeignKey("warehousearticlemovs", "CodWarehouseArticle", "warehousearticles", "CodWarehouseArticle");

            //Data
            dbS.AddColumnToTable("warehousearticlemovs", "Date", SchemaDb.Date, "0");

            //Use is for Ordine and Impegno
            dbS.AddColumnToTable("warehousearticlemovs", "Used", SchemaDb.Bool, "0");

            //quantità movimento
            dbS.AddColumnToTable("warehousearticlemovs", "Quantity", SchemaDb.Double, "0");

            //tipo di movimento --> 0 = scarico, 1 carico, 2 ordine, 3 impegno
            dbS.AddColumnToTable("warehousearticlemovs", "TypeOfMov", SchemaDb.Int, "0");
            dbS.AddColumnToTable("warehousearticlemovs", "UnitOfMeasureMov", SchemaDb.String, "50");
            dbS.AddColumnToTable("warehousearticlemovs", "UmConversion", SchemaDb.Double, "0");

            dbS.AddColumnToTable("warehousearticlemovs", "CodProductPartPrintableArticle", SchemaDb.String, "50");
            dbS.AddForeignKey("warehousearticlemovs", "CodProductPartPrintableArticle", "productpartsprintablearticle", "CodProductPartPrintableArticle");

            dbS.AddColumnToTable("warehousearticlemovs", "CodDocument", SchemaDb.String, "50");
            dbS.AddForeignKey("warehousearticlemovs", "CodDocument", "documents", "CodDocument");

            dbS.AddColumnToTable("warehousearticlemovs", "Note", SchemaDb.String, "255");

            //Index
            dbS.AddIndex("warehousearticlemovs", "TypeOfMov");
            dbS.AddIndex("warehousearticlemovs", "CodProductPartPrintableArticle");


        }
    }
}
