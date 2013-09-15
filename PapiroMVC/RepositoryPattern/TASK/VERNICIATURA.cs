using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class VERNICIATURA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public VERNICIATURA()
        {
            CodTypeOfTask = "VERNICIATURA";
            TaskName = "Verniciatura";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "VERNICIATURA";
            optTk.CodOptionTypeOfTask = "VERNICIATURA_NO";
            optTk.OptionName = "NO";
            optTk.IdexOf = 0;
            //Nessun lato opaco/lucido
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "VERNICIATURA";
            optTk.CodOptionTypeOfTask = "VERNICIATURA_OPACA1";
            optTk.OptionName = "1 lato opaco";
            optTk.IdexOf = 3;
            //1 lato opaco
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "VERNICIATURA";
            optTk.CodOptionTypeOfTask = "VERNICIATURA_OPACA2";
            optTk.OptionName = "2 lati opaca";
            optTk.IdexOf = 4;
            //2 lato opaco
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "VERNICIATURA";
            optTk.CodOptionTypeOfTask = "VERNICIATURA_LUCIDA1";
            optTk.OptionName = "1 latO LUCIDA";
            optTk.IdexOf = 1;
            //2 Lucidi
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "VERNICIATURA";
            optTk.CodOptionTypeOfTask = "VERNICIATURA_LUCIDA2";
            optTk.OptionName = "2 lati lucidi";
            optTk.IdexOf = 2;
            //2 lati lucidi
            this.OptionTypeOfTasks.Add(optTk);


        }
    }
}