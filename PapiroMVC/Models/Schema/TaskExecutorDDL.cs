using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class TaskExecutorsDDL : IDDL
    {
        SchemaDb dbS;

        public TaskExecutorsDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {           
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("taskexecutors");
            dbS.AddColumnToTable("taskexecutors", "CodTaskExecutor", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("taskexecutors", "TaskExecutorName", SchemaDb.String, "100");
            dbS.AddColumnToTable("taskexecutors", "CodTask", SchemaDb.String, "50");
            dbS.AddColumnToTable("taskexecutors", "Version", SchemaDb.String, "4");
            dbS.AddColumnToTable("taskexecutors", "Dismissed", SchemaDb.Bool, "0");

            //universal code used for supplier's item
            dbS.AddColumnToTable("taskexecutors", "UniversalCodPapiro", SchemaDb.String, "50");

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
            dbS.AddColumnToTable("taskexecutors", "PrintingUnit", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "SheetwiseAfterPrintingUnit", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "Sheetwise", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "WashUpTime", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutors", "ChangePlateTime", SchemaDb.Time, "0");

            // 0 = Litho // 1 = Digital // 2 = Flexo
            dbS.AddColumnToTable("taskexecutors", "SelectorLithoDigital", SchemaDb.Int, "0");

            // 0 = Binding  
            dbS.AddColumnToTable("taskexecutors", "SelectorBindingPrePostPress", SchemaDb.Int, "0");

            // 0 = lithosheet // 1 = lithoroll // 2 = digialsheet // 3 = digitalroll // 4 = plotter
            //litosheet
            dbS.AddColumnToTable("taskexecutors", "Selector", SchemaDb.Int, "0");

            //0 = PrinterMachine // 1 = PrePostPress
            dbS.AddColumnToTable("taskexecutors", "SelectorExecutor", SchemaDb.Int, "0");

            // prooﬁng paper
            dbS.AddColumnToTable("taskexecutors", "ProofSheetFirstStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "ProofSheetSecondsStart", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "ProductionWaste", SchemaDb.Double, "0");

            //roll meter
            dbS.AddColumnToTable("taskexecutors", "PaperFirstStartLenght", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "PaperSecondStartLenght", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("taskexecutors", "FlexoWidth", SchemaDb.Double, "0");

            //digital
            dbS.AddColumnToTable("taskexecutors", "BWSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "BWSide2", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorSide1", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorSide2", SchemaDb.Bool, "0");

            //plotter                
            dbS.AddColumnToTable("taskexecutors", "Width", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorJet", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "WhiteUV", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "ColorUV", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "Cutting", SchemaDb.Bool, "0");

            //PrePostPress IsUnitComputationManual
            dbS.AddColumnToTable("taskexecutors", "IsUnitComputationManual", SchemaDb.Bool, "0");
            
            //Index
            dbS.AddIndex("taskexecutors", "TaskExecutorName");

            //Estimated On
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnTime", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnRun", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnMq", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnTimeBinding", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutors", "IsEstimatedOnRunBinding", SchemaDb.Bool, "0");

            dbS.ChangeColumnToDouble("taskexecutors", "Width");

            //implant cost yes/no
            dbS.AddColumnToTable("taskexecutors", "HasImplant", SchemaDb.Bool, "0");

            //costo degli impianti stampa
            dbS.AddColumnToTable("taskexecutors", "CostImplant", SchemaDb.String, "20");


            dbS.AddTable("taskexecutorcylinders");
            dbS.AddColumnToTable("taskexecutorcylinders", "CodTaskExecutor", SchemaDb.String, "50");
            dbS.AddColumnToTable("taskexecutorcylinders", "CodTaskExecutorCylinder", SchemaDb.StringPK, "50");
            dbS.AddForeignKey("taskexecutorcylinders", "CodTaskExecutor", "taskexecutors", "CodTaskExecutor");

            dbS.AddColumnToTable("taskexecutorcylinders", "Z", SchemaDb.Int, "0");
            dbS.AddColumnToTable("taskexecutorcylinders", "Quantity", SchemaDb.Int, "0");

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
            dbS.AddColumnToTable("taskexecutorestimatedon", "CodOptionTypeOfTask", SchemaDb.String, "50");  
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime1", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTime2", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingTimePerColor", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "TimeForfait", SchemaDb.Time, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourRunning", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourStarting", SchemaDb.String, "20");

            dbS.ChangeColumnToString("taskexecutorestimatedon", "CostPerHourRunning");
            dbS.ChangeColumnToString("taskexecutorestimatedon", "CostPerHourStarting");


            dbS.AddForeignKey("taskexecutorestimatedon", "CodOptionTypeOfTask", "optiontypeoftask", "CodOptionTypeOfTask");


            //0 = DigitalOnRime // 1 = DigitalOnRun
            dbS.AddColumnToTable("taskexecutorestimatedon", "SelectorTimeRunDigital", SchemaDb.Int, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourRunningBW", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerHourStartingBW", SchemaDb.String, "20");

            //0 = PlotterMq
            dbS.AddColumnToTable("taskexecutorestimatedon", "SelectorPlotterMq", SchemaDb.Int, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostInkJetLow", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostInkJetMed", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostInkJetHight", SchemaDb.String, "20");

            dbS.AddColumnToTable("taskexecutorestimatedon", "CostUVLow", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostUVMed", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostUVHight", SchemaDb.String, "20");

            dbS.AddColumnToTable("taskexecutorestimatedon", "CostWhite", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostCutting", SchemaDb.String, "20");

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

            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnit", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentCostPerUnit", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentDeficitOnCostForWeightStep", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost1", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost2", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfait", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfaitForSheet", SchemaDb.String, "20");

            //DigitalOnRun
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostPerUnitBW", SchemaDb.Double, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentCostPerUnitBW", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "UseDifferentDeficitOnCostForWeightStepBW", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost1BW", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "StartingCost2BW", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfaitBW", SchemaDb.String, "20");
            dbS.AddColumnToTable("taskexecutorestimatedon", "CostForfaitForSheetBW", SchemaDb.String, "20");

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


            dbS.AddTable("steps");
            dbS.AddColumnToTable("steps", "CodTaskEstimatedOn", SchemaDb.String, "50");
            dbS.AddColumnToTable("steps", "IdStep", SchemaDb.IntPK, "0");
            dbS.AddColumnToTable("steps", "FromUnit", SchemaDb.Double, "0");
            dbS.AddColumnToTable("steps", "ToUnit", SchemaDb.Double, "0");

            //0 step per run //1 cost per run //2 percentage for Weight // 3 binding time // 4 binding cost
            dbS.AddColumnToTable("steps", "Selector", SchemaDb.Int, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "CostPerUnit", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "DeficitRate", SchemaDb.String, "20");

            //CostPerRunStepBW
            dbS.AddColumnToTable("steps", "SelectorCostBW", SchemaDb.Int, "0");

            //Binding On Time
            dbS.AddColumnToTable("steps", "AvarageRunPerHour4", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime4", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour6", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime6", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour8", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime8", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour12", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime12", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour16", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime16", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour24", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime24", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHour32", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTime32", SchemaDb.Time, "0");
            dbS.AddColumnToTable("steps", "AvarageRunPerHourBinding", SchemaDb.IntUS, "0");
            dbS.AddColumnToTable("steps", "StartingTimeBinding", SchemaDb.Time, "0");
           
            //BindingEstimatedOnUnit
            dbS.AddColumnToTable("steps", "CostPerUnit4", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost4", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit6", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost6", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit8", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost8", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit12", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost12", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit16", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost16", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit24", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost24", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "CostPerUnit32", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "StartingCost32", SchemaDb.String, "20");
            dbS.AddColumnToTable("steps", "BindingCost", SchemaDb.String, "20");

            //FK
            dbS.AddForeignKey("steps", "CodTaskEstimatedOn", "taskexecutorestimatedon", "CodTaskEstimatedOn");

            
            //typeoftask
            dbS.AddTable("typeoftask");
            dbS.AddColumnToTable("typeoftask", "CodTypeOfTask", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("typeoftask", "TaskName", SchemaDb.String, "100");
            dbS.AddColumnToTable("typeoftask", "CodCategoryOfTask", SchemaDb.String, "100");

            //optiontypeoftask
            dbS.AddTable("optiontypeoftask");
            dbS.AddColumnToTable("optiontypeoftask", "CodOptionTypeOfTask", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("optiontypeoftask", "CodTypeOfTask", SchemaDb.String, "50");
            dbS.AddColumnToTable("optiontypeoftask", "OptionName", SchemaDb.String, "100");
            dbS.AddColumnToTable("optiontypeoftask", "IdexOf ", SchemaDb.Int, "0");

            //FK
            dbS.AddForeignKey("optiontypeoftask", "CodTypeOfTask", "typeoftask", "CodTypeOfTask");

            //typeoftask
            dbS.AddTable("taskexecutortypeoftask");
            dbS.AddColumnToTable("taskexecutortypeoftask", "CodTaskExecutorTypeOfTask", SchemaDb.StringPK, "50");            
            dbS.AddColumnToTable("taskexecutortypeoftask", "CodTypeOfTask", SchemaDb.String, "50");
            dbS.AddColumnToTable("taskexecutortypeoftask", "CodTaskExecutor", SchemaDb.String, "50");

            //FK
            dbS.AddForeignKey("taskexecutortypeoftask", "CodTypeOfTask", "typeoftask", "CodTypeOfTask");
            dbS.AddForeignKey("taskexecutortypeoftask", "CodTaskExecutor", "taskexecutors", "CodTaskExecutor");



            //FK IN TASKEXECUTORS
            dbS.AddColumnToTable("taskexecutors", "CodTypeOfTask", SchemaDb.String, "50");
            dbS.AddForeignKey("taskexecutors", "CodTypeOfTask", "typeoftask", "CodTypeOfTask");



        }
    }
}
