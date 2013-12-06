using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPAOFFaROTOLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public STAMPAOFFaROTOLO()
        {
            CodTypeOfTask = "STAMPAOFFaROTOLO";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_1";
            optTk.OptionName = "Stampa a 1 colore";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_2";
            optTk.OptionName = "Stampa a 2 colori";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_3";
            optTk.OptionName = "Stampa a 3 colori";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_4";
            optTk.OptionName = "Stampa a 4 colori";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_5";
            optTk.OptionName = "Stampa a 5 colori";
            optTk.IdexOf = 4;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_6";
            optTk.OptionName = "Stampa a 6 colori";
            optTk.IdexOf = 5;
            this.OptionTypeOfTasks.Add(optTk);        


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFFaROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAOFFaROTOLO_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = 6;
            this.OptionTypeOfTasks.Add(optTk);
}
    }
}