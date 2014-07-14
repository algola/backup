using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPAACALDOROTOLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public STAMPAACALDOROTOLO()
        {
            CodTypeOfTask = "STAMPAACALDOROTOLO";
            TaskName = "Stampa a caldo";


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAACALDOROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAACALDOROTOLO_NO";
            optTk.OptionName = "No stampa a caldo";
            optTk.IdexOf = 0;
            //No stampa a caldo
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAACALDOROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAACALDOROTOLO_1";
            optTk.OptionName = "1 colore";
            optTk.IdexOf = 1;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAACALDOROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAACALDOROTOLO_2";
            optTk.OptionName = "2 colori";
            optTk.IdexOf = 2;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAACALDOROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAACALDOROTOLO_3";
            optTk.OptionName = "3 colori";
            optTk.IdexOf = 3;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAACALDOROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAACALDOROTOLO_4";
            optTk.OptionName = "4 colori";
            optTk.IdexOf = 4;
            //Con stacco
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}