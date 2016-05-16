using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC.Models
{

    /// <summary>
    /// data used to create and edit
    /// </summary>
    public class NewMovViewModel  
    {
        public Boolean IsProduct
        { get; set; }

        public WarehouseItem ArticleOrProduct
        { get; set; }

        public WarehouseArticleMov Mov
        { get; set; }

        public String CodWarehouseTo
        { get; set; }

    }
}
 