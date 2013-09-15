using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public class PLASTIFICATURA : TypeOfTask
    {
        
        OptionTypeOfTask optTk;


          public PLASTIFICATURA()
          {
              CodTypeOfTask = "PLASTIFICATURA";
              TaskName = "Plastificatura";

              optTk = new OptionTypeOfTask();
              optTk.CodTypeOfTask = "PLASTIFICATURA";
              optTk.CodOptionTypeOfTask = "PLASTIFICATURA_NO";
              optTk.OptionName = "NO";
              optTk.IdexOf = 0;
              //Nessun lato opaco/lucido
              this.OptionTypeOfTasks.Add(optTk);

              optTk = new OptionTypeOfTask();
              optTk.CodTypeOfTask = "PLASTIFICATURA";
              optTk.CodOptionTypeOfTask = "PLASTIFICATURA_OPACA1";
              optTk.OptionName = "1 lato opaco";
              optTk.IdexOf = 3;
              //1 lato opaco
              this.OptionTypeOfTasks.Add(optTk);

              optTk = new OptionTypeOfTask();
              optTk.CodTypeOfTask = "PLASTIFICATURA";
              optTk.CodOptionTypeOfTask = "PLASTIFICATURA_LUCIDA1";
              optTk.OptionName = "1 lato lucido";
              optTk.IdexOf = 1;
              //1 lato lucido
              this.OptionTypeOfTasks.Add(optTk);

              optTk = new OptionTypeOfTask();
              optTk.CodTypeOfTask = "PLASTIFICATURA";
              optTk.CodOptionTypeOfTask = "PLASTIFICATURA_OPACA2";
              optTk.OptionName = "2 lati opachi";
              optTk.IdexOf = 4;
              //2 lati opachi
              this.OptionTypeOfTasks.Add(optTk);

              optTk = new OptionTypeOfTask();
              optTk.CodTypeOfTask = "PLASTIFICATURA";
              optTk.CodOptionTypeOfTask = "PLASTIFICATURA_LUCIDA2";
              optTk.OptionName = "2 lati lucidi";
              optTk.IdexOf = 2;
              //2 lati lucidi
              this.OptionTypeOfTasks.Add(optTk);

          }
    }
}