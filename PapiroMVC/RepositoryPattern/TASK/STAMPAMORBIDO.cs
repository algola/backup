using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPAMORBIDO : TypeOfTask
    {
        OptionTypeOfTask optTk;

        public STAMPAMORBIDO()
        {
            CodTypeOfTask = "STAMPAMORBIDO";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAMORBIDO";
            optTk.CodOptionTypeOfTask = "STAMPAMORBIDO_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAMORBIDO";
            optTk.CodOptionTypeOfTask = "STAMPAMORBIDO_BASSA";
            optTk.OptionName = "Stampa UV bassa qualità";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAMORBIDO";
            optTk.CodOptionTypeOfTask = "STAMPAMORBIDO_MEDIA";
            optTk.OptionName = "Stampa UV media qualità";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAMORBIDO";
            optTk.CodOptionTypeOfTask = "STAMPAMORBIDO_ALTA";
            optTk.OptionName = "Stampa UV alta qualità";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}