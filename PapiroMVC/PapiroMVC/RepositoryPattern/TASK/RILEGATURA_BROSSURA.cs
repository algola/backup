using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class RILEGATURA_BROSSURA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public RILEGATURA_BROSSURA()
        {
            CodTypeOfTask = "RILEGATURA_BROSSURA";
            TaskName = "Rilegatura Brossura";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_BROSSURA";
            optTk.CodOptionTypeOfTask = "RILEGATURA_BROSSURA_NO";
            optTk.OptionName = "Nessuna Rilegatura Bossura";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_BROSSURA";
            optTk.CodOptionTypeOfTask = "RILEGATURA_BROSSURA_FILOREFE";
            optTk.OptionName = "Filorefe";
            optTk.IdexOf = 1;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_BROSSURA";
            optTk.CodOptionTypeOfTask = "RILEGATURA_BROSSURA_GREGATA_FRESATA";
            optTk.OptionName = "Gregata fresata";
            optTk.IdexOf = 2;
            //
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}