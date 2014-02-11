using SchemaManagemet;
using System.Data.Entity;

namespace PapiroMVC.Model
{    
    class DataBaseDDL : IDDL
    {

        SchemaDb dbS;
        
        public DataBaseDDL(string dbName)
        {
            dbS = new SchemaDb();
            dbS.DatabaseName = dbName;
        }

        public void UpdateSchema(DbContext ctx)
        {
            dbS.Ctx = ctx;
            dbS.CreateDatabase();
        }
    }
}
