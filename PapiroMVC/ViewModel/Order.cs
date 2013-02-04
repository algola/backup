using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace PapiroMVC.ViewModel
{
    public class Order
    {
        public Customer Customer
        { get; set; }

        public string OrderCode
        { get; set; }

        public string Description
        { get; set; }
    }
}