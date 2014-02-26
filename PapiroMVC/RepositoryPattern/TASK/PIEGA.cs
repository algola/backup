using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class PIEGA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public PIEGA()
        {
            CodTypeOfTask = "PIEGA";
            TaskName = "Piega";

            
            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "PIEGA";
            optTk.CodOptionTypeOfTask = "PIEGA_NO";
            optTk.OptionName = "Nessuna anta";
            optTk.IdexOf = 0;
            //Nessuna anta
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "PIEGA";
            optTk.CodOptionTypeOfTask = "PIEGA_2ANTE";
            optTk.OptionName = "Piega 2 Ante";
            optTk.IdexOf = 1;
            //2 ante
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "PIEGA";
            optTk.CodOptionTypeOfTask = "PIEGA_3ANTE";
            optTk.OptionName = "Piega 3 Ante";
            optTk.IdexOf = 2;
            //3 ante
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "PIEGA";
            optTk.CodOptionTypeOfTask = "PIEGA_4ANTE";
            optTk.OptionName = "Piega 4 Ante";
            optTk.IdexOf = 3;
            //4 ante
            this.OptionTypeOfTasks.Add(optTk);
           

        }


        
    }
}