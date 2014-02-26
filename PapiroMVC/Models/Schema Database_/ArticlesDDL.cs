using SchemaManagemet;
using System.Data.Entity;

namespace TestSchemaManagement.Model
{    
    class ArticlesDDL :IDDL
    {
        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("Articles");
            dbS.AddColumnToTable("Articles", "CodArticle", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("Articles", "ArticleName", SchemaDb.String, "100");

            //supplier
            dbS.AddColumnToTable("Articles", "CodSupplierBuy", SchemaDb.String, "50");
            dbS.AddColumnToTable("Articles", "SupplierMaker", SchemaDb.String, "50");

            dbS.AddColumnToTable("Articles", "UnitOfMeasure", SchemaDb.String, "50");

            // 0 = Printable // 1 = NoPrintable 
            dbS.AddColumnToTable("Articles", "SelectorPrintable", SchemaDb.Int, "0");

            //Printable            
            dbS.AddColumnToTable("Articles", "TypeOfMaterial", SchemaDb.String, "200");
            dbS.AddColumnToTable("Articles", "NameOfMaterial", SchemaDb.String, "200");
            
            dbS.AddColumnToTable("Articles", "Color", SchemaDb.String, "100");
            dbS.AddColumnToTable("Articles", "Weight", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Articles", "Hand", SchemaDb.Double, "0");

            // 0 = Sheet // 1 = Roll // 2 = Wide 
            dbS.AddColumnToTable("Articles", "SelectorSheetRollWide", SchemaDb.Int, "0");

            //Sheet
            dbS.AddColumnToTable("Articles", "Format", SchemaDb.String, "9");
            dbS.AddColumnToTable("Articles", "NoPinza", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("Articles", "NoBv", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("Articles", "SheetPerPacked", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Articles", "SheetPerPallet", SchemaDb.IntUS, "0");
            
            //Roll
            dbS.AddColumnToTable("Articles", "Width", SchemaDb.IntUS, "0");

            //Rigid
            dbS.AddColumnToTable("Articles", "Thikness", SchemaDb.Double, "0");
            
            //Object
            dbS.AddColumnToTable("Articles", "Size", SchemaDb.String, "50");
            dbS.AddColumnToTable("Articles", "PrintableFormat", SchemaDb.String, "9");

            //Index
            dbS.AddIndex("Articles", "ArticleName");
            dbS.AddIndex("Articles", "TypeOfMaterial,SupplierMaker");
            dbS.AddIndex("Articles", "TypeOfMaterial,SupplierMaker,Color");
            dbS.AddIndex("Articles", "TypeOfMaterial,SupplierMaker,Color,Weight");

            //ArticleCost
            dbS.AddTable("ArticleCost");
            //primary key
            dbS.AddColumnToTable("ArticleCost", "CodArticleCost", SchemaDb.StringPK, "50"); 

            //foreign key
            dbS.AddColumnToTable("ArticleCost", "CodArticle", SchemaDb.String, "50"); 
            dbS.AddForeignKey("ArticleCost", "CodArticle", "Articles", "CodArticle");

            //Selector             // 0 = Sheet // 1 = Roll // 2 = Wide 
            dbS.AddColumnToTable("ArticleCost", "SelectorArticleCost", SchemaDb.Int, "0");

            //Sheet
            //cost
            dbS.AddColumnToTable("ArticleCost", "CostPerKg", SchemaDb.Double, "0");
            dbS.AddColumnToTable("ArticleCost", "CostPerSheet", SchemaDb.Double, "0");
            dbS.AddColumnToTable("ArticleCost", "CostPerUnit", SchemaDb.Double, "0");
            
            // 0 = SheetPrintableArticlePakedCost // 1 = SheetPrintableArticleCuttedCost // 2 = SheetPrintableArticlePalletCost
            //selector typeof cost sheet
            dbS.AddColumnToTable("ArticleCost", "SelectorArticleSheetPrintableCost", SchemaDb.Int, "0");

            // SheetPrintableArticleCuttedCost
            dbS.AddColumnToTable("ArticleCost", "UseTheSameCostOfPalletAfterKg", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("ArticleCost", "UseTheSameCostOfSheetAfterSheet", SchemaDb.Bool, "0");
            
            dbS.AddColumnToTable("ArticleCost", "KgPallet", SchemaDb.Long, "0");
            dbS.AddColumnToTable("ArticleCost", "KgSheet", SchemaDb.Long, "0");


            //Roll
            //Cost
            dbS.AddColumnToTable("ArticleCost", "CostPerMq", SchemaDb.Double, "0");
            dbS.AddColumnToTable("ArticleCost", "CostPerMl", SchemaDb.Double, "0");

            // 0 = Standard // 1 = Cutted 
            //selector typeof cost sheet
            dbS.AddColumnToTable("ArticleCost", "SelectorArticleRollPrintableCost", SchemaDb.Int, "0");

            // SheetPrintableArticleCuttedCost
            dbS.AddColumnToTable("ArticleCost", "UseTheSameCostOfStandardWidthAfterKg", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("ArticleCost", "UseTheSameCostOfStandardWidthAfterMl", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("ArticleCost", "Kg", SchemaDb.Double, "0");
            dbS.AddColumnToTable("ArticleCost", "Ml", SchemaDb.Long, "0");


            dbS.AddColumnToTable("ArticleCost", "SelectorArticleRigidPrintableCost", SchemaDb.Int, "0");
            dbS.AddColumnToTable("ArticleCost", "SelectorArticleObjectPrintableCost", SchemaDb.Int, "0");
        }
    }
}
