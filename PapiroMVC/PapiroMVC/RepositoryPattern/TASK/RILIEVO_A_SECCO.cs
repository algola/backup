using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class RILIEVO_A_SECCO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public RILIEVO_A_SECCO()
        {
            CodTypeOfTask = "RILIEVO_A_SECCO";
            TaskName = "Rilievo a sesso";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILIEVO_A_SECCO";
            optTk.CodOptionTypeOfTask = "RILIEVO_A_SECCO_NO";
            optTk.OptionName = "No Rilievo";
            optTk.IdexOf = 0;
            //No rilievo
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "RILIEVO_A_SECCO";
            optTk.CodOptionTypeOfTask = "RILIEVO_A_SECCO_SI";
            optTk.OptionName = "Si rilievo";
            optTk.IdexOf = 1;
            //Si rilievo
            this.OptionTypeOfTasks.Add(optTk);


        }
    }
}