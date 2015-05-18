using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class SERIGRAFIAROTOLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public SERIGRAFIAROTOLO()
        {
            CodTypeOfTask = "SERIGRAFIAROTOLO";
            TaskName = "Serigrafia";
            
            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIAROTOLO";
            optTk.CodOptionTypeOfTask = "SERIGRAFIAROTOLO_NO";
            optTk.OptionName = "Nessuna serigrafia";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIAROTOLO";
            optTk.CodOptionTypeOfTask = "SERIGRAFIAROTOLO_SI";
            optTk.OptionName = "Stampa Serigrafica";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);
        }
        
    }
}