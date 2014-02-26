using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class MenuProductDDL :IDDL
    {
        SchemaDb dbS;

        public MenuProductDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            
            dbS.Ctx = ctx;

            //First Table
            dbS.AddTable("menuproduct");
            dbS.AddColumnToTable("menuproduct", "CodMenuProduct", SchemaDb.StringPK, "50");
            dbS.AddColumnToTable("menuproduct", "Enabled", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("menuproduct", "CodCategory", SchemaDb.String, "50");
            dbS.AddColumnToTable("menuproduct", "Hidden", SchemaDb.Bool, "0");
            dbS.AddColumnToTable("menuproduct", "IndexOf", SchemaDb.Int, "0");
            dbS.AddColumnToTable("menuproduct", "IndexOfCategory", SchemaDb.Int, "0");


        }
    }
}
