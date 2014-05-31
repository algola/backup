using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class TAVOLOCONTROLLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public TAVOLOCONTROLLO()
        {
            CodTypeOfTask = "TAVOLOCONTROLLO";
            TaskName = "Tavolo di controllo";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "TAVOLOCONTROLLO";
            optTk.CodOptionTypeOfTask = "TAVOLOCONTROLLO_NO";
            optTk.OptionName = "NO";
            optTk.IdexOf = 0;
            //Nessun lato opaco/lucido
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "TAVOLOCONTROLLO";
            optTk.CodOptionTypeOfTask = "TAVOLOCONTROLLO_SI";
            optTk.OptionName = "eseguire";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}