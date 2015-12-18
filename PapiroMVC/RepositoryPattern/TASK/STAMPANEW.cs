using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPANEW : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public STAMPANEW()
        {
            CodTypeOfTask = "STAMPANEW";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPANEW";
            optTk.CodOptionTypeOfTask = "STAMPANEW_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            var max = 6;

            for (int i = 1; i <= max; i++)
            {
                for (int k = 0; k <= i; k++)
                {
                    optTk = new OptionTypeOfTask();
                    optTk.CodTypeOfTask = "STAMPANEW";
                    optTk.CodOptionTypeOfTask = "STAMPANEW_" + i.ToString() + "+" + k.ToString();
                    optTk.OptionName = i.ToString() + "+" + k.ToString();
                    optTk.IdexOf = i * max + k;

                    this.OptionTypeOfTasks.Add(optTk);
                }
            }
        }
    }
}