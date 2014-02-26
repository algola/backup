using System.Data.Entity;

namespace TestSchemaManagement.Model
{
    interface IDDL
    {
        void UpdateSchema(DbContext ctx);
    }

}
