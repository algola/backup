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
            optTk.IdexOf = -1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_SI";
            optTk.OptionName = "Stampa serigrafica";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_1";
            optTk.OptionName = "Stampa Serigrafica 1 colore";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_2";
            optTk.OptionName = "Stampa Serigrafica 2 colori";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_3";
            optTk.OptionName = "Stampa Serigrafica 3 colori";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk);
        }
    }
}