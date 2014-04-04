using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
        public List<MenuProduct> MenuProducts { get; set; }
         
    }
}