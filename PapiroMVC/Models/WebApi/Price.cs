using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models.WebApi
{
    public class Price
    {
        public DateTime Date { get; set; }
        public string UnitPrice { get; set; }
        public string TotalPrice { get; set; }

    }
}