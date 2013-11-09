using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public class DocumentProductViewModel
    {
        public Product Product { get; set; }
        public List<DocumentProduct> DocumentProduct { get; set; }
    }
}