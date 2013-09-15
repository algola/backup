using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class RILEGATURA_SPIRALE : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public RILEGATURA_SPIRALE()
        {
            CodTypeOfTask = "RILEGATURA_SPIRALE";
            TaskName = "Rilegatura Spirale";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_SPIRALE";
            optTk.CodOptionTypeOfTask = "RILEGATURA_SPIRALE_NO";
            optTk.OptionName = "Nessuna Rilegatura Spirale";
            optTk.IdexOf = 0;
            //Nessuna 
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_SPIRALE";
            optTk.CodOptionTypeOfTask = "RILEGATURA_SPIRALE_SI";
            optTk.OptionName = "Rilegatura Spirale";
            optTk.IdexOf = 1;
            //Spirale
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}