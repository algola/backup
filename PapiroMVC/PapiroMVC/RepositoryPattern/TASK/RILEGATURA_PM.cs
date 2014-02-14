using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class RILEGATURA_PM : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public RILEGATURA_PM()
        {
            CodTypeOfTask = "RILEGATURA_PM";
            TaskName = "Rilegatura punto metallico";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_PM";
            optTk.CodOptionTypeOfTask = "RILEGATURA_PM_NO";
            optTk.OptionName = "No rilegatura punto metallico";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILEGATURA_PM";
            optTk.CodOptionTypeOfTask = "RILEGATURA_PM_2PUNTI";
            optTk.OptionName = "2 punti metallici";
            optTk.IdexOf = 1;
            //
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}