using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class SERIGRAFIASOLOTIPI : TypeOfTask
    {
        OptionTypeOfTask optTk;
        //usata solo
        public SERIGRAFIASOLOTIPI()
        {
            CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            TaskName = "Serigrafia";
            
            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_UVLUCIDO";
            optTk.OptionName = "UV Lucida";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_UVOPACO";
            optTk.OptionName = "UV Opaca";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_NO";
            optTk.OptionName = "No Braille";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_MEDIOSPESSORE";
            optTk.OptionName = "Braille medio Sp.";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_ALTOSPESSORE";
            optTk.OptionName = "Braille alto Sp.";
            optTk.IdexOf = 4;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIASOLOTIPI";
            optTk.CodOptionTypeOfTask = "SERIGRAFIASOLOTIPI_FONDOPIENO";
            optTk.OptionName = "Braille Fondo Pieno";
            optTk.IdexOf = 5;
            this.OptionTypeOfTasks.Add(optTk);
        }
    }
}