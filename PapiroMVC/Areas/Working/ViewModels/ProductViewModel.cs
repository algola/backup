using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public List<int> Quantities { get; set; }

        public ProductViewModel()
        {
            Quantities = new List<int>();
        }
    }
}