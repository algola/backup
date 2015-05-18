using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class SERIGRAFIA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public SERIGRAFIA()
        {
            CodTypeOfTask = "SERIGRAFIA";
            TaskName = "Serigrafia";
            
            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_NO";
            optTk.OptionName = "Nessuna serigrafia";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_SI";
            optTk.OptionName = "Stampa Serigrafica";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);
        }


        
    }
}