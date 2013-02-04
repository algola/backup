using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace TestSchemaManagement.Model
{
    interface IDDL
    {
        void UpdateSchema(ObjectContext ctx);
    }

}
