using System;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    public partial class ProductFormatName
    {
        public String CodFormat { get; set; }
        public String FormatName { get; set; }

        public PrintingHint PHint { get; set; } 
    }
}
