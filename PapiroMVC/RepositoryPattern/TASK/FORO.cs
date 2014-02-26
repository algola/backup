using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class FORO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public FORO()
        {
            CodTypeOfTask = "FORO";
            TaskName = "Foro";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FORO";
            optTk.CodOptionTypeOfTask = "FORO_NO";
            optTk.OptionName = "No foro";
            optTk.IdexOf = 0;
            //no foro
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "FORO";
            optTk.CodOptionTypeOfTask = "FORO_SI";
            optTk.OptionName = "Si foro";
            optTk.IdexOf = 1;
            //si foro
            this.OptionTypeOfTasks.Add(optTk);


        }
    }
}