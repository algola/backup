using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using SchemaManagemet;

namespace TestSchemaManagement.Model
{
    class GridColumnsPerUserDDL : IDDL
    {

        public void UpdateSchema(ObjectContext ctx)
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
