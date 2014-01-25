using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(NewProductCommand_MetaData))]
    public class NewProductCommand
    {
        public string NewProduct { get; set; }

    }
}