using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class INTERCALATURA : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public INTERCALATURA()
        {
            CodTypeOfTask = "INTERCALATURA";
            TaskName = "Intercalatura";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "INTERCALATURA";
            optTk.CodOptionTypeOfTask = "INTERCALATURA_NO";
            optTk.OptionName = "No intercalatura";
            optTk.IdexOf = 0;
            //No intercalatura
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "INTERCALATURA";
            optTk.CodOptionTypeOfTask = "INTERCALATURA_2COPIE";
            optTk.OptionName = "2 Copie di intercalatura";
            optTk.IdexOf = 1;
            //2 copie intercalatura
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "INTERCALATURA";
            optTk.CodOptionTypeOfTask = "INTERCALATURA_3COPIE";
            optTk.OptionName = "3 Copie di intercalatura";
            optTk.IdexOf = 2;
            //3 copie intercalatura
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "INTERCALATURA";
            optTk.CodOptionTypeOfTask = "INTERCALATURA_4COPIE";
            optTk.OptionName = "4 Copie di intercalatura";
            optTk.IdexOf = 3;
            //4 copie intercalatura
            this.OptionTypeOfTasks.Add(optTk);


        }
    }
}