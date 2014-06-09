using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{
    class ProductsDDL : IDDL
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
            dbS.AddColumnToTable("products", "ProductName", SchemaDb.String, "255");
            dbS.AddColumnToTable("products", "ProductRefName", SchemaDb.String, "255");
            dbS.AddColumnToTable("products", "id", SchemaDb.String, "100");
            dbS.AddColumnToTable("products", "Format", SchemaDb.String, "50");


            ////customer
            //dbS.AddColumnToTable("products", "CodCustomer", SchemaDb.String, "50");
            //dbS.AddColumnToTable("products", "Customer", SchemaDb.String, "50");

            ////foreign key
            //dbS.AddForeignKey("products", "CodCustomer", "CustomerSuppliers", "CodCustomerSupplier");
            //Index
            dbS.AddIndex("products", "ProductName");
            dbS.AddIndex("products", "ProductRefName");

            // 0 = ProductSingleSheet // 1 = ProductBookSheet // 2 = ProductBlockSheet
            dbS.AddColumnToTable("products", "SelectorProduct", SchemaDb.Int, "0");

            //ProductSingleSheet
            //1 only ProductPart of ProductPartSingleSheet

            //ProductBookSheet
            //0..1 part of ProductPartCoverSheet
            //n parts of ProductPartBookSheet

            //ProductBlockSheet
            //0..1 part of ProductPartCoverSheet
            //n parts of ProductPartBlockSheet

            //Second Table
            dbS.AddTable("productparts");
            dbS.AddColumnToTable("productparts", "CodProductPart", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("productparts", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("productparts", "CodProduct", "products", "CodProduct");

            dbS.AddColumnToTable("productparts", "ProductPartName", SchemaDb.String, "100");

            //taken from list
            dbS.AddColumnToTable("productparts", "PrintingType", SchemaDb.String, "20");
            dbS.AddColumnToTable("productparts", "Format", SchemaDb.String, "20");


            //only per label
            dbS.AddColumnToTable("productparts", "LabelsPerRoll", SchemaDb.Int, "0");
            //mm
            dbS.AddColumnToTable("productparts", "SoulDiameter", SchemaDb.Double, "0");
            //mm
            dbS.AddColumnToTable("productparts", "MaxDiameter", SchemaDb.Double, "0");

            dbS.AddColumnToTable("productparts", "CodProductPart_", SchemaDb.String, "50");


            // 0 = ProductPartSingleSheet // 1 =  ProductPartCoverSheet // 2 = ProductPartBookSheet // 3 = ProductPartBlockSheet // etc... 
            dbS.AddColumnToTable("productparts", "SelectorProductPart", SchemaDb.Int, "0");

            //ProductPartSingleSheet
            dbS.AddColumnToTable("productparts", "SubjectNumber", SchemaDb.Int, "0");
            dbS.AddColumnToTable("productparts", "RawCut", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("productparts", "ServicesNumber", SchemaDb.Int, "0");

            //ProductPartCoverSheet
            dbS.AddColumnToTable("productparts", "WidthWings", SchemaDb.Double, "0"); //larghezza alette
            dbS.AddColumnToTable("productparts", "Back", SchemaDb.Double, "0"); //aletezza dorso

            //ProductPartBookSheet
            dbS.AddColumnToTable("productparts", "FormatOpened", SchemaDb.String, "20");
            dbS.AddColumnToTable("productparts", "Pages", SchemaDb.Int, "0");
            dbS.AddColumnToTable("productparts", "DCut", SchemaDb.Double, "0");
            dbS.AddColumnToTable("productparts", "IsDCut", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("productparts", "DCut1", SchemaDb.Double, "0");
            dbS.AddColumnToTable("productparts", "DCut2", SchemaDb.Double, "0");

           //this is for label where we have to stay into MaxDCut
            dbS.AddColumnToTable("productparts", "HaveDCutLimit", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("productparts", "MaxDCut", SchemaDb.Double, "0");
            dbS.AddColumnToTable("productparts", "MinDCut", SchemaDb.Double, "0");

            //0 or Null --> Free //1 = SideOnSide //2 = !SideOnSide
            dbS.AddColumnToTable("productparts", "SideOnSide", SchemaDb.Int, "0");

            //ProductPartBlockSheet

            //---------------------------------------------------------------------------------------------

            //Third Table
            dbS.AddTable("productpartsprintablearticle");
            dbS.AddColumnToTable("productpartsprintablearticle", "CodProductPartPrintableArticle", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("productpartsprintablearticle", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productpartsprintablearticle", "CodProductPart", "productparts", "CodProductPart");

            dbS.AddColumnToTable("productpartsprintablearticle", "ProductPartPrintableArticleName", SchemaDb.String, "100");

            dbS.AddColumnToTable("productpartsprintablearticle", "TypeOfMaterial", SchemaDb.String, "200");
            dbS.AddColumnToTable("productpartsprintablearticle", "NameOfMaterial", SchemaDb.String, "200");

            dbS.AddColumnToTable("productpartsprintablearticle", "Color", SchemaDb.String, "100");
            dbS.AddColumnToTable("productpartsprintablearticle", "Adhesive", SchemaDb.String, "100");
            dbS.AddColumnToTable("productpartsprintablearticle", "Weight", SchemaDb.IntUS, "0");

            // 0 = ProductPartSheetArticle //  etc... 
            dbS.AddColumnToTable("productpartsprintablearticle", "SelectorProductPartPrintableArticle", SchemaDb.Int, "0");

            //ProductPartSheetArticle

            //---------------------------------------------------------------------------------------------

            //Second Table
            dbS.AddTable("producttasks");
            dbS.AddColumnToTable("producttasks", "CodProductTask", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("producttasks", "CodOptionTypeOfTask", SchemaDb.String, "50");
            dbS.AddColumnToTable("producttasks", "Hidden", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("producttasks", "ImplantHidden", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("producttasks", "IndexOf", SchemaDb.Int, "0");

            dbS.AddColumnToTable("producttasks", "CodItemGraph", SchemaDb.String, "20");
            

            //foreign key
            dbS.AddColumnToTable("producttasks", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("producttasks", "CodProduct", "products", "CodProduct");
            dbS.AddForeignKey("producttasks", "CodOptionTypeOfTask", "optiontypeoftask", "CodOptionTypeOfTask");

            dbS.AddColumnToTable("producttasks", "ProductTaskName", SchemaDb.String, "100");

            // 0 = single sheet // 1 = Book // 2 = Block // 3 = Roll // etc... 
            dbS.AddColumnToTable("producttasks", "SelectorProductTask", SchemaDb.Int, "0");

            //---------------------------------------------------------------------------------------------

            //task related at parts

            //Third Table
            dbS.AddTable("productparttasks");
            dbS.AddColumnToTable("productparttasks", "CodProductPartTask", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("productparttasks", "CodOptionTypeOfTask", SchemaDb.String, "50");
            dbS.AddColumnToTable("productparttasks", "Hidden", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("productparttasks", "ImplantHidden", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("productparttasks", "IndexOf", SchemaDb.Int, "0");

            dbS.AddColumnToTable("productparttasks", "CodItemGraph", SchemaDb.String, "20");

            //foreign key
            dbS.AddColumnToTable("productparttasks", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productparttasks", "CodProductPart", "productparts", "CodProductPart");
            dbS.AddForeignKey("productparttasks", "CodOptionTypeOfTask", "optiontypeoftask", "CodOptionTypeOfTask");

            //---------------------------------------------------------------------------------------------

            //task related at parts (more than one)

            //Third Table
            dbS.AddTable("productpartstoproducttask");
            dbS.AddColumnToTable("productpartstoproducttask", "CodProductPartToProductTask", SchemaDb.StringPK, "50");

            //foreign key to parts!
            dbS.AddColumnToTable("productpartstoproducttask", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productpartstoproducttask", "CodProductPart", "productparts", "CodProductPart");

            //foreign key to productTask!
            dbS.AddColumnToTable("productpartstoproducttask", "CodProductTask", SchemaDb.String, "50");
            dbS.AddForeignKey("productpartstoproducttask", "CodProductTask", "producttasks", "CodProductTask");





            dbS.AddColumnToTable("productparttasks", "CodItemGraph", SchemaDb.String, "20"); //nodo
            dbS.AddColumnToTable("producttasks", "CodItemGraph", SchemaDb.String, "20"); //nodo

            dbS.AddTable("productgraphs");
            dbS.AddColumnToTable("productgraphs", "CodProductGraph", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("productgraphs", "CodItemGraph", SchemaDb.String, "20"); //nodo
            dbS.AddColumnToTable("productgraphs", "CodItemGraphLink", SchemaDb.String, "20"); //adiacenza
            //la particolarità è che l'adiacenza è sempre successiva e mai a ritroso
            
            //foreign key
            dbS.AddColumnToTable("productgraphs", "CodProduct", SchemaDb.String, "50");
            dbS.AddForeignKey("productgraphs", "CodProduct", "products", "CodProduct");



        }
    }
}
