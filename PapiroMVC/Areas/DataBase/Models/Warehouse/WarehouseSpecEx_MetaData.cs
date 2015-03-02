using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class WarehouseSpec_MetaData
    {

        [DisplayNameLocalized(typeof(ResArticle), "CodWarehouse")]
        [Tooltip(typeof(ResArticle), "CodWarehouseToolTip")]
        public string CodWarehouse { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "WarehouseName")]
        [Tooltip(typeof(ResArticle), "WarehouseNameToolTip")]
        public string WarehouseName { get; set; }



    }
}