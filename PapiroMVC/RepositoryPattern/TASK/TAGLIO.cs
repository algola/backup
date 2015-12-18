using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class TAGLIO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public TAGLIO()
        {
            CodTypeOfTask = "TAGLIO";
            TaskName = "Taglio";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "TAGLIO";
            optTk.CodOptionTypeOfTask = "TAGLIO_NO";
            optTk.OptionName = "No taglio";
            optTk.IdexOf = 0;
            //No taglio
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "TAGLIO";
            optTk.CodOptionTypeOfTask = "TAGLIO_AL_VIVO";
            optTk.OptionName = "Taglio al vivo";
            optTk.IdexOf = 1;
            //Taglio al vivo
            this.OptionTypeOfTasks.Add(optTk);


            //optTk = new OptionTypeOfTask();
            //optTk.CodTypeOfTask = "TAGLIO";
            //optTk.CodOptionTypeOfTask = "TAGLIO_DOPPIO";
            //optTk.OptionName = "Doppio taglio";
            //optTk.IdexOf = 2;
            ////Doppio taglio
            //this.OptionTypeOfTasks.Add(optTk);

        }
    }
}