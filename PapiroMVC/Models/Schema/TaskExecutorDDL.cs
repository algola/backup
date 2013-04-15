using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class TaskExecutorsDDL :IDDL
    {
        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("taskexecutors");
            dbS.AddColumnToTable("taskexecutors", "CodTaskExecutor", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("taskexecutors", "TaskExecutorName", SchemaDb.String, "100");
            dbS.AddColumnToTable("taskexecutors", "Version", SchemaDb.String, "4");
            dbS.AddColumnToTable("taskexecutors", "Dismissed", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("taskexecutors", "FormatMin", SchemaDb.String, "9");
            dbS.AddColumnToTable("taskexecutors", "FormatMax", SchemaDb.String, "9");
            dbS.AddColumnToTable("taskexecutors", "WeightMin", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "WeightMax", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "Pinza", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutors", "ControPinza", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutors", "Laterale", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutors", "InkUsage", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutors", "InkUsageForfait", SchemaDb.Double, "0");

            //litho
            dbS.AddColumnToTable("taskexecutors", "PrintingUnit", SchemaDb.ByteUS, "0");
            dbS.AddColumnToTable("taskexecutors", "SheetwiseAfterPrintingUnit", SchemaDb.ByteUS, "0");
            dbS.AddColumnToTable("taskexecutors", "Sheetwise", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "WashUpTime", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutors", "ChangePlateTime", SchemaDb.Time, "0");

            // 0 = Litho // 1 = Digital 
            dbS.AddColumnToTable("taskexecutors", "SelectorLithoDigital", SchemaDb.Int, "0");

            // 0 = Binding  
            dbS.AddColumnToTable("taskexecutors", "SelectorBindingPrePostPress", SchemaDb.Int, "0");

            // 0 = lithosheet // 1 = lithoweb // 2 = digialsheet // 3 = digitalroll // 4 = plotter
            //litosheet
            dbS.AddColumnToTable("taskexecutors", "Selector", SchemaDb.Int, "0");

            //0 = PrinterMachine // 1 = PrePostPress
            dbS.AddColumnToTable("taskexecutors", "SelectorExecutor", SchemaDb.Int, "0");

            // prooﬁng paper
            dbS.AddColumnToTable("taskexecutors", "ProofSheetFirstStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "ProofSheetSecondsStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "ProductionWaste", SchemaDb.Double, "0");

            //roll
            dbS.AddColumnToTable("taskexecutors", "PaperFirstStartLenght", SchemaDb.IntUS, "0");

            //digital
            dbS.AddColumnToTable("taskexecutors", "BWSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "BWSide2", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorSide2", SchemaDb.Bool, "0");

            //plotter                
            dbS.AddColumnToTable("taskexecutors", "Width", SchemaDb.IntUS, "0");

            //PrePostPress IsUnitComputationManual
            dbS.AddColumnToTable("taskexecutors", "IsUnitComputationManual", SchemaDb.Bool, "0");

            //Index
            dbS.AddIndex("taskexecutors", "TaskExecutorName");

            //Estimated On
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnTime", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnRun", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnMq", SchemaDb.Bool, "0");

            dbS.AddTable("taskexecutorestimatedon");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CodTaskExecutor", SchemaDb.String, "50");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CodTaskEstimatedOn", SchemaDb.StringPK, "50");
            dbS.AddForeignKey("taskexecutorestimatedon", "CodTaskExecutor", "taskexecutors", "CodTaskExecutor");
            
            
            
            // 0 = Time // 1 = Unit // 2 = BindingTime // 3 = BindingUnit
            dbS.AddColumnToTable("taskexecutorestimatedon", "SelectorUnitTime", SchemaDb.Int, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "SelectorMqRun", SchemaDb.Int, "0");

            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentRunPerHour", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentDeficitForWeightStep", SchemaDb.Bool, "0");
  
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime1", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime2", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "TimeForfait", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourRunning", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourStarting", SchemaDb.String, "20");

            //BindingEstimatedOnTime

            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour4", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime4", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour6", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime6", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour8", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime8", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour12", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime12", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour16", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime16", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour24", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime24", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHour32", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime32", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "AvarageRunPerHourBinding", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTimeBinding", SchemaDb.Time, "0");

            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentCostPerUnit", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentDeficitOnCostForWeightStep", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost1", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost2", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfait", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfaitForSheet", SchemaDb.String, "20");


            //BindingEstimatedOnUnit
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit4", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost4", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit6", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost6", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit8", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost8", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit12", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost12", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit16", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost16", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit24", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost24", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit32", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost32", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "BindingCost", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "BindingStartingCost", SchemaDb.String, "20");


            dbS.AddTable("Steps");
            dbS.AddColumnToTable("Steps", "CodTaskEstimatedOn", SchemaDb.String, "50");
            dbS.AddColumnToTable("Steps", "IdStep", SchemaDb.IntPK, "0");
            dbS.AddColumnToTable("Steps", "FromUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "ToUnit", SchemaDb.Double, "0");

            //0 step per run //1 cost per run //2 percentage for Weight // 3 binding time // 4 binding cost
            dbS.AddColumnToTable("Steps", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "DeficitRate", SchemaDb.String, "20");

            //Binding On Time
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour4", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime4", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour6", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime6", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour8", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime8", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour12", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime12", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour16", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime16", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour24", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime24", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour32", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTime32", SchemaDb.Time, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHourBinding", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "StartingTimeBinding", SchemaDb.Time, "0");
           
            //BindingEstimatedOnUnit
            dbS.AddColumnToTable("Steps", "CostPerUnit4", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost4", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit6", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost6", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit8", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost8", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit12", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost12", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit16", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost16", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit24", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost24", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "CostPerUnit32", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "StartingCost32", SchemaDb.String, "20");
            dbS.AddColumnToTable("Steps", "BindingCost", SchemaDb.String, "20");




            //FK
            dbS.AddForeignKey("Steps", "CodTaskEstimatedOn", "taskexecutorestimatedon", "CodTaskEstimatedOn");

        }
    }
}
