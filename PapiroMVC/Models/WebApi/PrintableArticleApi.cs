using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models.WebApi
{
    public class PrintableArticleApi
    {
        public string TypeOfMaterial { get; set; }
        public string NameOfMaterial { get; set; }
        public string Color { get; set; }
        public Nullable<long> Weight { get; set; }   
    }
}