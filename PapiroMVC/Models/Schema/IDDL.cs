using System.Data.Entity;

namespace PapiroMVC.Model
{
    interface IDDL
    {
        void UpdateSchema(DbContext ctx);
    }

}
