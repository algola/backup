using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public class Quantita : List<int>
    {
    }

    [MetadataType(typeof(ProductViewModel_MetaData))]
    public class ProductViewModel
    {
        public String ProductName { get; set; }
        public String Customer { get; set; }

        public Product Product { get; set; }
        public Quantita Quantities { get; set; }

        public ProductViewModel()
        {
            Quantities = new Quantita();
        }
    }
}
