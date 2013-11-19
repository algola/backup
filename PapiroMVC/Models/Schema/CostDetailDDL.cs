using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;
using System.Data.Entity;

namespace PapiroMVC.Model
{
    class CostDetailDDL : IDDL
    {
        SchemaDb dbS;

        public CostDetailDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {

            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("costdetails");
            dbS.AddColumnToTable("costdetails", "CodCostDetail", SchemaDb.StringPK, "50");

            dbS.AddColumnToTable("costdetails", "CodTaskExecutorSelected", SchemaDb.String, "50");
            dbS.AddColumnToTable("costdetails", "CodCost", SchemaDb.String, "50");
            dbS.AddColumnToTable("costdetails", "CodProductPart", SchemaDb.String, "50");

            //foregin key
            dbS.AddForeignKey("costdetails", "CodCost", "costs", "CodCost");
            dbS.AddForeignKey("costdetails", "CodTaskExecutorSelected", "taskexecutors", "CodTaskExecutor");
            dbS.AddForeignKey("costdetails", "CodProductPart", "productparts", "CodProductPart");


            // 0 = Printing // 1 = PrintedArticle 
            dbS.AddColumnToTable("costdetails", "Selector", SchemaDb.Int, "0");

            dbS.AddColumnToTable("costdetails", "PrintingFormat", SchemaDb.String, "20");
            dbS.AddColumnToTable("costdetails", "BuyingFormat", SchemaDb.String, "20");

            dbS.AddColumnToTable("costdetails", "BuyingWidth", SchemaDb.Double, "0");

            dbS.AddColumnToTable("costdetails", "SelectorPrinting", SchemaDb.Int, "0");
            dbS.AddColumnToTable("costdetails", "SelectorPrintedArticle", SchemaDb.Int, "0");

            dbS.AddColumnToTable("costdetails", "CostPerKg", SchemaDb.String, "20");
            dbS.AddColumnToTable("costdetails", "CostPerSheet", SchemaDb.String, "20");
            dbS.AddColumnToTable("costdetails", "CostPerUnit", SchemaDb.String, "20");
            dbS.AddColumnToTable("costdetails", "CostPerMq", SchemaDb.String, "20");
            dbS.AddColumnToTable("costdetails", "CostPerMl", SchemaDb.String, "20");


            dbS.AddColumnToTable("costdetails", "CodComputedBy", SchemaDb.String, "50");
            dbS.AddForeignKey("costdetails", "CodComputedBy", "costdetails", "CodCostDetail");


            //First Table
            dbS.AddTable("productpartprinting");
            dbS.AddColumnToTable("productpartprinting", "CodProductPartPrinting", SchemaDb.StringPK, "50");
            //           dbS.AddColumnToTable("productpartprinting", "CodCostDetail", SchemaDb.String, "50");

            dbS.AddColumnToTable("productpartprinting", "Selector", SchemaDb.Int, "0");

            dbS.AddColumnToTable("productpartprinting", "PrintingFormat", SchemaDb.String, "20");
            dbS.AddColumnToTable("productpartprinting", "Width", SchemaDb.Double, "0");

            dbS.AddColumnToTable("productpartprinting", "SelectorSheet", SchemaDb.Int, "0");
            dbS.AddColumnToTable("productpartprinting", "SelectorPlotter", SchemaDb.Int, "0");

            dbS.AddForeignKey("productpartprinting", "CodProductPartPrinting", "costdetails", "CodCostDetail");


            dbS.AddColumnToTable("productpartprinting", "CodProductPart", SchemaDb.String, "50");
            dbS.AddForeignKey("productpartprinting", "CodProductPart", "productparts", "CodProductPart");

            ////First Table
            dbS.AddTable("productpartprintinggain");


            dbS.AddColumnToTable("productpartprintinggain", "CodProductPartPrintingGain", SchemaDb.StringPK, "50");

            dbS.AddColumnToTable("productpartprintinggain", "CodProductPartPrinting", SchemaDb.String, "50");
            dbS.AddColumnToTable("productpartprintinggain", "CodProductPartPrintingGainBuying", SchemaDb.String, "50");

            dbS.AddForeignKey("productpartprintinggain", "CodProductPartPrinting", "productpartprinting", "CodProductPartPrinting");
            dbS.AddForeignKey("productpartprintinggain", "CodProductPartPrintingGainBuying", "costdetails", "CodCostDetail");

            dbS.AddColumnToTable("productpartprintinggain", "UsePerfecting", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("productpartprintinggain", "MaxShape", SchemaDb.Int, "0");

            // 0 = Sheet // 1 = Plotter 
            dbS.AddColumnToTable("productpartprintinggain", "Selector", SchemaDb.Int, "0");

            dbS.AddColumnToTable("productpartprintinggain", "SelectorSheet", SchemaDb.Int, "0");
            dbS.AddColumnToTable("productpartprintinggain", "SelectorPlotter", SchemaDb.Int, "0");

            //////First Table
            dbS.AddTable("makereadies");
            dbS.AddColumnToTable("makereadies", "CodMakeready", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("makereadies", "CodProductPartPrintingGain", SchemaDb.String, "50");
            dbS.AddForeignKey("makereadies", "CodProductPartPrintingGain", "productpartprintinggain", "CodProductPartPrintingGain");

            dbS.AddColumnToTable("makereadies", "ShapeOnSide1", SchemaDb.Int, "0");
            dbS.AddColumnToTable("makereadies", "ShapeOnSide2", SchemaDb.Int, "0");

            dbS.AddColumnToTable("makereadies", "ShapeOnShape", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("makereadies", "TypeOfPerfecting", SchemaDb.String, "20");
            dbS.AddColumnToTable("makereadies", "CalculatedGain", SchemaDb.Decimal, "0");

            dbS.AddColumnToTable("makereadies", "Selector", SchemaDb.Int, "0");

            dbS.AddColumnToTable("makereadies", "PrintedShapes", SchemaDb.Int, "0");
            dbS.AddColumnToTable("makereadies", "PrintedSubjects", SchemaDb.Int, "0");

            dbS.AddColumnToTable("makereadies", "PrintablePages", SchemaDb.Int, "0");
            dbS.AddColumnToTable("makereadies", "PrintedPages", SchemaDb.Int, "0");
            dbS.AddColumnToTable("makereadies", "CodMaxSegn", SchemaDb.Int, "0");

        }
    }
}
