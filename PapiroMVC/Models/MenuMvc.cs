using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class MenuMvc
    {
        public int Id { get; set; }
        public string TextName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public object RouteValues { get; set; }
        public object HtmlAttributes { get; set; } 
        public int ParentId { get; set; }
        public int SortOrder { get; set; }

    }
}