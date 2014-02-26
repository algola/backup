using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class ACCOPPIATURA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public ACCOPPIATURA()
        {
            CodTypeOfTask = "ACCOPPIATURA";
            TaskName = "Accoppiatura";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "ACCOPPIATURA";
            optTk.CodOptionTypeOfTask = "ACCOPPIATURA_NO";
            optTk.OptionName = "No accoppiatura";
            optTk.IdexOf = 0;
            //No Accoppiatura
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "ACCOPPIATURA";
            optTk.CodOptionTypeOfTask = "ACCOPPIATURA_1LATO";
            optTk.OptionName = "Accoppiatura 1 lato";
            optTk.IdexOf = 1;
            //1 lato
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "ACCOPPIATURA";
            optTk.CodOptionTypeOfTask = "ACCOPPIATURA_2LATI";
            optTk.OptionName = "Accoppiatura 2 lati";
            optTk.IdexOf = 2;
            //2 lati
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "ACCOPPIATURA";
            optTk.CodOptionTypeOfTask = "ACCOPPIATURA_3LATI";
            optTk.OptionName = "Accoppiatura 3 lati";
            optTk.IdexOf = 3;
            //3 lati
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}