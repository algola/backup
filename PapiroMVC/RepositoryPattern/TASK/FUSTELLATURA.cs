using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class FUSTELLATURA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public FUSTELLATURA()
        {
            CodTypeOfTask = "FUSTELLATURA";
            TaskName = "Fustellatura";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FUSTELLATURA";
            optTk.CodOptionTypeOfTask = "FUSTELLATURA_NO";
            optTk.OptionName = "No fustellatura";
            optTk.IdexOf = 0;
            //No fustellatura
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FUSTELLATURA";
            optTk.CodOptionTypeOfTask = "FUSTELLATURA_STACCO";
            optTk.OptionName = "Con stacco";
            optTk.IdexOf = 1;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FUSTELLATURA";
            optTk.CodOptionTypeOfTask = "FUSTELLATURA_NO_STACCO";
            optTk.OptionName = "Senza stacco";
            optTk.IdexOf = 2;
            //senza stacco
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}