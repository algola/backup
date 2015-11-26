using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class TaskCentersDDL : IDDL
    {
        SchemaDb dbS;

        public TaskCentersDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {           
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("taskcenters");
            dbS.AddColumnToTable("taskcenters", "CodTaskCenter", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("taskcenters", "TaskCenterName", SchemaDb.String, "100");
            dbS.AddColumnToTable("taskcenters", "IndexOf", SchemaDb.Int, "0");

            dbS.AddColumnToTable("taskcenters", "CodState", SchemaDb.String, "100");
            dbS.AddForeignKey("taskcenters", "CodState", "states", "CodStates");


            //First Table
            dbS.AddTable("documenttaskcenter");
            dbS.AddColumnToTable("documenttaskcenter", "CodDocumentTaskCenter", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("documenttaskcenter", "CodTaskCenter", SchemaDb.String, "100");
            dbS.AddColumnToTable("documenttaskcenter", "CodDocument", SchemaDb.String, "100");
            dbS.AddColumnToTable("documenttaskcenter", "IndexOf", SchemaDb.Int, "0");

            //derivated or not!
            dbS.AddColumnToTable("documenttaskcenter", "DocumentName", SchemaDb.String, "255");

            dbS.AddForeignKey("documenttaskcenter", "CodTaskCenter", "taskcenters", "CodTaskCenter");
            dbS.AddForeignKey("documenttaskcenter", "CodDocument", "documents", "CodDocument");


        }
    }
}
