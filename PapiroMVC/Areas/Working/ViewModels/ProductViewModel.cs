using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{


    [MetadataType(typeof(ProductViewModel_MetaData))]
    public class ProductViewModel
    {
        public String DocumentName { get; set; }

        public String ProductRefName { get; set; }
        public String Customer { get; set; }

        public Product Product { get; set; }
        public int Quantity { get; set; }
        public ProductViewModel()
        {
           // Quantities = new Quantita();
        }
    }
}
