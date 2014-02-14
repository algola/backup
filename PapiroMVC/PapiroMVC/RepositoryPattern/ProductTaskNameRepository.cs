using System;
using System.Linq;
using PapiroMVC.Models;
using PapiroMVC.DbCodeManagement;
using System.Threading;
using System.Data;
using System.Collections.Generic;

namespace Services
{
    public class ProductTaskNameRepository : IProductTaskNameRepository
    {
        public String[] GetAllById(string id)
        {

            String[] ret;

            switch (id)
            {
                case "Buste":
                    ret= new String[0]{};
                    break;
                case "BigliettiVisita":
                    ret = new String[2] { "PLASTIFICATURA_NO", "VERNICIATURA_NO" };
                    break;
                case "PuntoMetallico":
                    ret = new String[2] { "VERNICIATURA_NO", "RILEGATURA_PM_NO" };
                    break;
           
                default :
                    ret = new String[0]{};
                    break;
            }

            return ret;
        }
    }
}