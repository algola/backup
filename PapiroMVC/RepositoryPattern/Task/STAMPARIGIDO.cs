using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPARIGIDO : TypeOfTask
    {
        OptionTypeOfTask optTk;

        public STAMPARIGIDO()
        {
            CodTypeOfTask = "STAMPARIGIDO";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_BASSA";
            optTk.OptionName = "Stampa UV bassa qualità";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_BASSAW";
            optTk.OptionName = "Stampa UV bassa qualità e bianco";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_BASSADN";
            optTk.OptionName = "Stampa UV bassa qualità Day&Nigth";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_MEDIA";
            optTk.OptionName = "Stampa UV media qualità";
            optTk.IdexOf = 4;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_MEDIAW";
            optTk.OptionName = "Stampa UV media qualità e bianco";
            optTk.IdexOf = 5;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_MEDIADN";
            optTk.OptionName = "Stampa UV media qualità Day&Nigth";
            optTk.IdexOf = 6;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_ALTA";
            optTk.OptionName = "Stampa UV alta qualità";
            optTk.IdexOf = 7;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_ALTAW";
            optTk.OptionName = "Stampa UV alta qualità e bianco";
            optTk.IdexOf = 8;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPARIGIDO";
            optTk.CodOptionTypeOfTask = "STAMPARIGIDO_ALTADN";
            optTk.OptionName = "Stampa UV alta qualità Day&Nigth";
            optTk.IdexOf = 9;
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}