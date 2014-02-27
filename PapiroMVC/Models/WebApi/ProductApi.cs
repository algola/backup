using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models.WebApi
{
    public class ProductApi
    {
        public ProductApi()
        {
            Prices = new List<Price>();
        }

        /// <summary>
        /// list of price disocunted by date
        /// </summary>
        public List<Price> Prices { get; set; }

        /// <summary>
        /// id of the product type
        /// </summary>
        public string Id { get; set; }
        public string Format { get; set; }

    }
}