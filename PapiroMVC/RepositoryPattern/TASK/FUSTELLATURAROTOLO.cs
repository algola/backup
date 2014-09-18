using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class FUSTELLATURAROTOLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public FUSTELLATURAROTOLO()
        {
            CodTypeOfTask = "FUSTELLATURAROTOLO";
            TaskName = "Fustellatura";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FUSTELLATURAROTOLO";
            optTk.CodOptionTypeOfTask = "FUSTELLATURAROTOLO_NO";
            optTk.OptionName = "No fustellatura";
            optTk.IdexOf = 0;
            //No fustellatura
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FUSTELLATURAROTOLO";
            optTk.CodOptionTypeOfTask = "FUSTELLATURAROTOLO_SI";
            optTk.OptionName = "Con stacco";
            optTk.IdexOf = 1;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}