using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaManagemet;
using System.Data.Entity;

namespace TestSchemaManagement.Model
{
    class UserDDL : IDDL
    {

        public void UpdateSchema(DbContext ctx)
        {
            var dbS = new SchemaDb();
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("Users");
            dbS.AddColumnToTable("GridColumnsPerUser", "CodUser", SchemaDb.StringPK, "100");
            dbS.AddColumnToTable("GridColumnsPerUser", "UserName", SchemaDb.String, "100");
            dbS.AddColumnToTable("GridColumnsPerUser", "Password", SchemaDb.String, "100");
            dbS.AddColumnToTable("GridColumnsPerUser", "Culture", SchemaDb.String, "255");

            //            dbS.AddForeignKey("GridColumnsPerUser", "CodUser", "Users", "CodUser");

        }
    }
}
