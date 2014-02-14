using SchemaManagemet;
using System.Data.Entity;

namespace TestSchemaManagement.Model
{
    class GridColumnsPerUserDDL : IDDL
    {

        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("GridColumnsPerUser");
            dbS.AddColumnToTable("GridColumnsPerUser", "IdColumnsPerUser", SchemaDb.IntPK, "10");
            dbS.AddColumnToTable("GridColumnsPerUser", "CodUser", SchemaDb.String, "100");
            dbS.AddColumnToTable("GridColumnsPerUser", "CodGrid", SchemaDb.String, "100");
            dbS.AddColumnToTable("GridColumnsPerUser", "Visibility", SchemaDb.String, "50");

            dbS.AddForeignKey("GridColumnsPerUser", "CodUser", "Users", "CodUser");

        }
    }
}
