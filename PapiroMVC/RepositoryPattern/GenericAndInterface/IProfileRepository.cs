using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
         Profile GetSingle(string name);
         void SyncroModules(string codProfile);
         Module GetSingleModule(string codModuleName);
         void SaveModule(Module m);

    }
}

