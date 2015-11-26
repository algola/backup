using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class STAMPAETICHROTOLO : TypeOfTask
    {

        OptionTypeOfTask optTk;

        public STAMPAETICHROTOLO()
        {
            CodTypeOfTask = "STAMPAETICHROTOLO";
            TaskName = "Stampa";

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_1";
            optTk.OptionName = "Stampa a 1 colore";
            optTk.IdexOf = 0;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_1RETRO";
            optTk.OptionName = "Stampa a 1 colore con retro";
            optTk.IdexOf = 1;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_1VERNICE";
            optTk.OptionName = "Stampa a 1 colore con vernice";
            optTk.IdexOf = 2;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_1RETROVERNICE";
            optTk.OptionName = "Stampa a 1 colore con retro e vernice";
            optTk.IdexOf = 3;
            this.OptionTypeOfTasks.Add(optTk); 

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_2";
            optTk.OptionName = "Stampa a 2 colori";
            optTk.IdexOf = 4;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_2RETRO";
            optTk.OptionName = "Stampa a 2 colori con retro";
            optTk.IdexOf = 5;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_2VERNICE";
            optTk.OptionName = "Stampa a 2 colori con vernice";
            optTk.IdexOf = 6;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_2RETROVERNICE";
            optTk.OptionName = "Stampa a 2 colori con retro e vernice";
            optTk.IdexOf = 7;
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_3";
            optTk.OptionName = "Stampa a 3 colori";
            optTk.IdexOf = 8;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_3RETRO";
            optTk.OptionName = "Stampa a 3 colori con retro";
            optTk.IdexOf = 9;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_3VERNICE";
            optTk.OptionName = "Stampa a 3 colori con vernice";
            optTk.IdexOf = 10;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_3RETROVERNICE";
            optTk.OptionName = "Stampa a 3 colori con retro e vernice";
            optTk.IdexOf = 11;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_4";
            optTk.OptionName = "Stampa a 4 colori";
            optTk.IdexOf = 12;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_4RETRO";
            optTk.OptionName = "Stampa a 4 colori con retro";
            optTk.IdexOf = 13;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_4VERNICE";
            optTk.OptionName = "Stampa a 4 colori con vernice";
            optTk.IdexOf = 14;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_4RETROVERNICE";
            optTk.OptionName = "Stampa a 4 colori con retro e vernice";
            optTk.IdexOf = 15;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_5";
            optTk.OptionName = "Stampa a 5 colori";
            optTk.IdexOf = 16;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_5RETRO";
            optTk.OptionName = "Stampa a 5 colori con retro";
            optTk.IdexOf = 17;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_5VERNICE";
            optTk.OptionName = "Stampa a 5 colori con vernice";
            optTk.IdexOf = 18;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_5RETROVERNICE";
            optTk.OptionName = "Stampa a 5 colori con retro e vernice";
            optTk.IdexOf = 19;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_6";
            optTk.OptionName = "Stampa a 6 colori";
            optTk.IdexOf = 20;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_6RETRO";
            optTk.OptionName = "Stampa a 6 colori con retro";
            optTk.IdexOf = 21;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_6VERNICE";
            optTk.OptionName = "Stampa a 6 colori con vernice";
            optTk.IdexOf = 22;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_6RETROVERNICE";
            optTk.OptionName = "Stampa a 6 colori con retro e vernice";
            optTk.IdexOf = 23;
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_7";
            optTk.OptionName = "Stampa a 7 colori";
            optTk.IdexOf = 24;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_7RETRO";
            optTk.OptionName = "Stampa a 7 colori con retro";
            optTk.IdexOf = 25;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_7VERNICE";
            optTk.OptionName = "Stampa a 7 colori con vernice";
            optTk.IdexOf = 26;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_7RETROVERNICE";
            optTk.OptionName = "Stampa a 7 colori con retro e vernice";
            optTk.IdexOf = 27;
            this.OptionTypeOfTasks.Add(optTk);



            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_8";
            optTk.OptionName = "Stampa a 8 colori";
            optTk.IdexOf = 28;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_8RETRO";
            optTk.OptionName = "Stampa a 8 colori con retro";
            optTk.IdexOf = 29;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_8VERNICE";
            optTk.OptionName = "Stampa a 8 colori con vernice";
            optTk.IdexOf = 30;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_8RETROVERNICE";
            optTk.OptionName = "Stampa a 8 colori con retro e vernice";
            optTk.IdexOf = 31;
            this.OptionTypeOfTasks.Add(optTk);


            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_NORETROVERNICE";
            optTk.OptionName = "Stampa con retro e vernice";
            optTk.IdexOf = 32;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_NORETRO";
            optTk.OptionName = "Stampa con retro";
            optTk.IdexOf = 33;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_NOVERNICE";
            optTk.OptionName = "Stampa con vernice";
            optTk.IdexOf = 34;
            this.OptionTypeOfTasks.Add(optTk);

            optTk = new OptionTypeOfTask();
            optTk.CodTypeOfTask = "STAMPAETICHROTOLO";
            optTk.CodOptionTypeOfTask = "STAMPAETICHROTOLO_NO";
            optTk.OptionName = "NO stampa";
            optTk.IdexOf = -1;
            this.OptionTypeOfTasks.Add(optTk);
}
    }
}