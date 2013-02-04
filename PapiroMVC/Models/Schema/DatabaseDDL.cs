using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Data.Entity;

namespace PapiroMVC.Model
{
    interface IDDL
    {
        void UpdateSchema(DbContext ctx);
    }

}
