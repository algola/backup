using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Objects;

namespace TestSchemaManagement.Model
{    
    class PrinterMachinesDDL :IDDL
    {
        public void UpdateSchema(ObjectContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("PrinterMachines");
            dbS.AddColumnToTable("PrinterMachines", "CodPrinterMachine", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("PrinterMachines", "PrinterName", SchemaDb.String, "100");
            dbS.AddColumnToTable("PrinterMachines", "Version", SchemaDb.String, "4");
            dbS.AddColumnToTable("PrinterMachines", "Dismissed", SchemaDb.Bool, "0");

            dbS.AddColumnToTable("PrinterMachines", "FormatMin", SchemaDb.String, "9");
            dbS.AddColumnToTable("PrinterMachines", "FormatMax", SchemaDb.String, "9");
            dbS.AddColumnToTable("PrinterMachines", "WeightMin", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "WeightMax", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "Pinza", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterMachines", "ControPinza", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterMachines", "Laterale", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterMachines", "InkUsage", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterMachines", "InkUsageForfait", SchemaDb.Double, "0");

            //litho
            dbS.AddColumnToTable("PrinterMachines", "PrintingUnit", SchemaDb.ByteUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "SheetwiseAfterPrintingUnit", SchemaDb.ByteUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "Sheetwise", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "WashUpTime", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterMachines", "ChangePlateTime", SchemaDb.Time, "0");

            // 0 = Litho // 1 = Digital 
            dbS.AddColumnToTable("PrinterMachines", "SelectorLithoDigital", SchemaDb.Int, "0");

            // 0 = Binding  
            dbS.AddColumnToTable("PrinterMachines", "SelectorBindingPrePostPress", SchemaDb.Int, "0");

            // 0 = lithosheet // 1 = lithoweb // 2 = digialsheet // 3 = digitalroll // 4 = plotter
            //litosheet
            dbS.AddColumnToTable("PrinterMachines", "Selector", SchemaDb.Int, "0");

            //0 = PrinterMachine // 1 = PrePostPress
            dbS.AddColumnToTable("PrinterMachines", "SelectorExecutor", SchemaDb.Int, "0");

            // prooﬁng paper
            dbS.AddColumnToTable("PrinterMachines", "ProofSheetFirstStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "ProofSheetSecondsStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterMachines", "ProductionWaste", SchemaDb.Double, "0");

            //roll
            dbS.AddColumnToTable("PrinterMachines", "PaperFirstStartLenght", SchemaDb.IntUS, "0");

            //digital
            dbS.AddColumnToTable("PrinterMachines", "BWSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "BWSide2", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "ColorSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "ColorSide2", SchemaDb.Bool, "0");

            //plotter                
            dbS.AddColumnToTable("PrinterMachines", "Width", SchemaDb.IntUS, "0");

            //PrePostPress IsUnitComputationManual
            dbS.AddColumnToTable("PrinterMachines", "IsUnitComputationManual", SchemaDb.Bool, "0");

            //Index
            dbS.AddIndex("PrinterMachines", "PrinterName");

            //Estimated On
            dbS.AddColumnToTable("PrinterMachines", "IsEstimatedOnTime", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "IsEstimatedOnRun", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterMachines", "IsEstimatedOnMq", SchemaDb.Bool, "0");

            dbS.AddTable("PrinterEstimatedOn");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CodPrinterMachine", SchemaDb.String, "50");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CodPrinterEstimatedOn", SchemaDb.StringPK, "50");
            dbS.AddForeignKey("PrinterEstimatedOn", "CodPrinterMachine", "PrinterMachines", "CodPrinterMachine");
            
            
            
            // 0 = Time // 1 = Unit // 2 = BindingTime // 3 = BindingUnit
            dbS.AddColumnToTable("PrinterEstimatedOn", "SelectorUnitTime", SchemaDb.Int, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "SelectorMqRun", SchemaDb.Int, "0");

            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "UseDifferentRunPerHour", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "UseDifferentDeficitForWeightStep", SchemaDb.Bool, "0");
  
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime1", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime2", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "TimeForfait", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerHourRunning", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerHourStarting", SchemaDb.Double, "0");

            //BindingEstimatedOnTime

            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour4", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime4", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour6", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime6", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour8", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime8", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour12", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime12", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour16", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime16", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour24", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime24", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHour32", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTime32", SchemaDb.Time, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "AvarageRunPerHourBinding", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingTimeBinding", SchemaDb.Time, "0");

            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "UseDifferentCostPerUnit", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "UseDifferentDeficitOnCostForWeightStep", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost1", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost2", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostForfait", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostForfaitForSheet", SchemaDb.Double, "0");



            //BindingEstimatedOnUnit
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit4", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost4", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit6", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost6", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit8", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost8", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit12", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost12", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit16", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost16", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit24", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost24", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "CostPerUnit32", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "StartingCost32", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "BindingCost", SchemaDb.Double, "0");
            dbS.AddColumnToTable("PrinterEstimatedOn", "BindingStartingCost", SchemaDb.Double, "0");


            dbS.AddTable("Steps");
            dbS.AddColumnToTable("Steps", "CodPrinterEstimatedOn", SchemaDb.String, "50");
            dbS.AddColumnToTable("Steps", "IdStep", SchemaDb.IntPK, "0");
            dbS.AddColumnToTable("Steps", "FromUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "ToUnit", SchemaDb.Double, "0");

            //0 step per run //1 cost per run //2 percentage for weigth // 3 binding time // 4 binding cost
            dbS.AddColumnToTable("Steps", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("Steps", "AvarageRunPerHour", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "DeficitRate", SchemaDb.Double, "0");

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
            dbS.AddColumnToTable("Steps", "CostPerUnit4", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost4", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit6", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost6", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit8", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost8", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit12", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost12", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit16", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost16", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit24", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost24", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "CostPerUnit32", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "StartingCost32", SchemaDb.Double, "0");
            dbS.AddColumnToTable("Steps", "BindingCost", SchemaDb.Double, "0");




            //FK
            dbS.AddForeignKey("Steps", "CodPrinterEstimatedOn", "PrinterEstimatedOn", "CodPrinterEstimatedOn");

        }
    }
}
