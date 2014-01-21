﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPAOFF : TypeOfTask
    {
        OptionTypeOfTask optTk;

        public STAMPAOFF()
        {
            CodTypeOfTask = "STAMPAOFF";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFF";
            optTk.CodOptionTypeOfTask = "STAMPAOFF_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = 0;
            //
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFF";
            optTk.CodOptionTypeOfTask = "STAMPAOFF_FR_COL";
            optTk.OptionName = "Stampa fronte/retro a colori";
            optTk.IdexOf = 1;
            //FR Col
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFF";
            optTk.CodOptionTypeOfTask = "STAMPAOFF_FR_BN";
            optTk.OptionName = "Stampa fronte/retro in bianco e nero";
            optTk.IdexOf = 2;
            //FR BN
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFF";
            optTk.CodOptionTypeOfTask = "STAMPAOFF_FRONTE_COL";
            optTk.OptionName = "Stampa fronte a colori";
            optTk.IdexOf = 3;
            //F Col
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAOFF";
            optTk.CodOptionTypeOfTask = "STAMPAOFF_FRONTE_BN";
            optTk.OptionName = "Stampa fronte bianco e nero";
            optTk.IdexOf = 4;
            //F BN
            this.OptionTypeOfTasks.Add(optTk);

        }
    }
}