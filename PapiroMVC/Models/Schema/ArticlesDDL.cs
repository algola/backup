using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class ArticlesDDL :IDDL
    {
        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("articles");
            dbS.AddColumnToTable("articles", "CodArticle", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("articles", "ArticleName", SchemaDb.String, "100");

            //supplier
            dbS.AddColumnToTable("articles", "CodSupplierBuy", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "CodSupplierMaker", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "SupplierMaker", SchemaDb.String, "50");

            //foreign key
            dbS.AddForeignKey("articles", "CodSupplierBuy", "CustomerSuppliers", "CodCustomerSupplier");
            dbS.AddForeignKey("articles", "CodSupplierMaker", "CustomerSuppliers", "CodCustomerSupplier");


            dbS.AddColumnToTable("articles", "UnitOfMeasure", SchemaDb.String, "50");

            // 0 = Printable // 1 = NoPrintable 
            dbS.AddColumnToTable("articles", "SelectorPrintable", SchemaDb.Int, "0");

            //Printable            
            dbS.AddColumnToTable("articles", "TypeOfMaterial", SchemaDb.String, "200");
            dbS.AddColumnToTable("articles", "NameOfMaterial", SchemaDb.String, "200");
            
            dbS.AddColumnToTable("articles", "Color", SchemaDb.String, "100");
            dbS.AddColumnToTable("articles", "Weight", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("articles", "Hand", SchemaDb.Double, "0");

            // 0 = Sheet // 1 = Roll // 2 = Wide 
            dbS.AddColumnToTable("articles", "SelectorSheetRollWide", SchemaDb.Int, "0");

            //Sheet
            dbS.AddColumnToTable("articles", "Format", SchemaDb.String, "9");
            dbS.AddColumnToTable("articles", "NoPinza", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articles", "NoBv", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("articles", "SheetPerPacked", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("articles", "SheetPerPallet", SchemaDb.IntUS, "0");
            
            //Roll
            dbS.AddColumnToTable("articles", "Width", SchemaDb.IntUS, "0");

            //Rigid
            dbS.AddColumnToTable("articles", "Thikness", SchemaDb.Double, "0");
            
            //Object
            dbS.AddColumnToTable("articles", "Size", SchemaDb.String, "50");
            dbS.AddColumnToTable("articles", "PrintableFormat", SchemaDb.String, "9");
            dbS.AddColumnToTable("articles", "MqForafait", SchemaDb.Double, "0");

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
            dbS.AddColumnToTable("articlecost", "CostPerMq", SchemaDb.Double, "0");
            dbS.AddColumnToTable("articlecost", "CostPerMl", SchemaDb.Double, "0");

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
        }
    }
}
