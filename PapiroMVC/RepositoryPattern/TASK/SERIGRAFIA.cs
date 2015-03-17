using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class SERIGRAFIA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public SERIGRAFIA()
        {
            CodTypeOfTask = "SERIGRAFIA";
            TaskName = "Serigrafia";

            
            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_NO";
            optTk.OptionName = "Nessuna serigrafia";
            optTk.IdexOf = 0;
            //Nessuna anta
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_UVPROT";
            optTk.OptionName = "Serigrafia UV protettiva";
            optTk.IdexOf = 1;
            //2 ante
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "SERIGRAFIA";
            optTk.CodOptionTypeOfTask = "SERIGRAFIA_UVSPESSORATA";
            optTk.OptionName = "Serigrafia UV spessorata";
            optTk.IdexOf = 2;
            //3 ante
            this.OptionTypeOfTasks.Add(optTk);


        }


        
    }
}