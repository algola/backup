using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{


    class UpdateDDL : IDDL
    {
        SchemaDb dbS;

        public UpdateDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            dbS.Ctx = ctx;
            dbS.AddColumnToTable("documentstate", "CodState", SchemaDb.String, "100");

        }

    }
    class ArticlesDDL : IDDL
    {
        SchemaDb dbS;

        public ArticlesDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {

            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("articles");
            dbS.AddColumnToTable("articles", "CodArticle", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("articles", "ArticleName", SchemaDb.String, "100");

            //supplier
            dbS.AddColumnToTable("articles", "CodSupplierBuy", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "CodSupplierMaker", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "SupplierMaker", SchemaDb.String, "50");

            dbS.AddColumnToTable("articles", "NoUseInEstimateCalculation", SchemaDb.Bool, "0");

            dbS.AddForeignKey("articles", "CodSupplierMaker", "CustomerSuppliers", "CodCustomerSupplier");


            dbS.AddColumnToTable("articles", "UnitOfMeasure", SchemaDb.String, "50");

            // 0 = Printable // 1 = NoPrintable 
            dbS.AddColumnToTable("articles", "SelectorPrintable", SchemaDb.Int, "0");

            //Printable            
            dbS.AddColumnToTable("articles", "TypeOfMaterial", SchemaDb.String, "200");
            dbS.AddColumnToTable("articles", "NameOfMaterial", SchemaDb.String, "200");

            dbS.AddColumnToTable("articles", "Color", SchemaDb.String, "100");
            dbS.AddColumnToTable("articles", "Adhesive", SchemaDb.String, "100");

            dbS.AddColumnToTable("articles", "Weight", SchemaDb.Double, "0");
            //UPDATE TO DOUBLE
            //"articles", "Weight"
            //"articles", "SuppWeight"
            dbS.ChangeColumnToDouble("articles", "Weight");
            dbS.ChangeColumnToDouble("articles", "SuppWeight");




            dbS.AddColumnToTable("articles", "Hand", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "SuppOfMaterial", SchemaDb.String, "100");
            dbS.AddColumnToTable("articles", "SuppWeight", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "SuppHand", SchemaDb.Double, "0");


            // 0 = Sheet // 1 = Roll // 2 = Wide 
            dbS.AddColumnToTable("articles", "SelectorSheetRollWide", SchemaDb.Int, "0");

            //Sheet
            dbS.AddColumnToTable("articles", "Format", SchemaDb.String, "9");
            dbS.AddColumnToTable("articles", "NoPinza", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articles", "NoBv", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articles", "SheetPerPacked", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("articles", "SheetPerPallet", SchemaDb.IntUS, "0");

            //Roll
            dbS.AddColumnToTable("articles", "Width", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "Tags", SchemaDb.String, "255");

            //Rigid
            dbS.AddColumnToTable("articles", "Thikness", SchemaDb.Double, "0"); //mm
            dbS.AddColumnToTable("articles", "ToNexMq", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articles", "FromMinFormat", SchemaDb.String, "9"); //quadratura minima

            //Object
            dbS.AddColumnToTable("articles", "Size", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "PrintableFormat", SchemaDb.String, "9");
            dbS.AddColumnToTable("articles", "MqForafait", SchemaDb.Double, "0");

            dbS.AddColumnToTable("articles", "Selector", SchemaDb.Int, "0");

            //NoPrintableType
            // 0 = die
            dbS.AddColumnToTable("articles", "NoPrintableType", SchemaDb.Int, "0");

            //DieType
            // 0 = Sheet // 1 = FlatRoll // 2 = Flexo 
            dbS.AddColumnToTable("articles", "CodDie", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "DieType", SchemaDb.Int, "0");
            dbS.AddColumnToTable("articles", "Z", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "Width", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "PrintingFormat", SchemaDb.String, "20");
            dbS.AddColumnToTable("articles", "Format", SchemaDb.String, "20");
            dbS.AddColumnToTable("articles", "FormatB", SchemaDb.String, "20");

            dbS.AddColumnToTable("articles", "DCut1", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "DCut2", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articles", "MaxGain1", SchemaDb.Int, "0");
            dbS.AddColumnToTable("articles", "MaxGain2", SchemaDb.Int, "0");
            dbS.AddColumnToTable("articles", "Description", SchemaDb.String, "255");
            // 0 = Quadrato // 1 = Ovale // 2 = Sagomato // 3 rotonda
            dbS.AddColumnToTable("articles", "FormatType", SchemaDb.Int, "0");

            dbS.AddColumnToTable("articles", "CodTaskExecutor", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "TaskExecutorName", SchemaDb.String, "50");

            //serigraphy
            dbS.AddColumnToTable("articles", "MeshRow", SchemaDb.Int, "0");
            dbS.AddColumnToTable("articles", "GainMqPerLt", SchemaDb.Int, "0");
            // dbS.ChangeColumnToDouble("articles", "GainMqPerLt");

            dbS.AddForeignKey("articles", "CodTaskExecutor", "taskexecutors", "CodTaskExecutor");


            //Index
            dbS.AddIndex("articles", "ArticleName");
            dbS.AddIndex("articles", "TypeOfMaterial,SupplierMaker");
            dbS.AddIndex("articles", "TypeOfMaterial,SupplierMaker,Color");
            dbS.AddIndex("articles", "TypeOfMaterial,SupplierMaker,Color,Weight");

            //ArticleCost
            dbS.AddTable("articlecost");
            //primary key
            dbS.AddColumnToTable("articlecost", "CodArticleCost", SchemaDb.StringPK, "50");

            //foreign key
            dbS.AddColumnToTable("articlecost", "CodArticle", SchemaDb.String, "50");
            dbS.AddForeignKey("articlecost", "CodArticle", "articles", "CodArticle");

            //Selector             // 0 = Sheet // 1 = Roll // 2 = Wide 
            dbS.AddColumnToTable("articlecost", "SelectorArticleCost", SchemaDb.Int, "0");

            //Sheet
            //cost
            dbS.AddColumnToTable("articlecost", "CostPerKg", SchemaDb.String, "20");
            dbS.AddColumnToTable("articlecost", "CostPerSheet", SchemaDb.String, "20");
            dbS.AddColumnToTable("articlecost", "CostPerUnit", SchemaDb.String, "20");

            // 0 = SheetPrintableArticlePakedCost // 1 = SheetPrintableArticleCuttedCost // 2 = SheetPrintableArticlePalletCost
            //selector typeof cost sheet
            dbS.AddColumnToTable("articlecost", "SelectorArticleSheetPrintableCost", SchemaDb.Int, "0");

            // SheetPrintableArticleCuttedCost
            dbS.AddColumnToTable("articlecost", "UseTheSameCostOfPalletAfterKg", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articlecost", "UseTheSameCostOfSheetAfterSheet", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("articlecost", "KgPallet", SchemaDb.Long, "0");
            dbS.AddColumnToTable("articlecost", "KgSheet", SchemaDb.Long, "0");


            //Roll
            //Cost
            dbS.AddColumnToTable("articlecost", "CostPerMq", SchemaDb.String, "20");
            dbS.AddColumnToTable("articlecost", "CostPerMl", SchemaDb.String, "20");

            // 0 = Standard // 1 = Cutted 
            //selector typeof cost sheet
            dbS.AddColumnToTable("articlecost", "SelectorArticleRollPrintableCost", SchemaDb.Int, "0");

            // SheetPrintableArticleCuttedCost
            dbS.AddColumnToTable("articlecost", "UseTheSameCostOfStandardWidthAfterKg", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articlecost", "UseTheSameCostOfStandardWidthAfterMl", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("articlecost", "Kg", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articlecost", "Ml", SchemaDb.Long, "0");

            dbS.AddColumnToTable("articlecost", "SelectorArticleRigidPrintableCost", SchemaDb.Int, "0");
            dbS.AddColumnToTable("articlecost", "SelectorArticleObjectPrintableCost", SchemaDb.Int, "0");

            //solo per i NoPrintableCost
            //0 costo unitario, 1 costo al kg, 2 costo al mq
            dbS.AddColumnToTable("articlecost", "TypeOfCost", SchemaDb.Int, "0");


        }
    }
}
