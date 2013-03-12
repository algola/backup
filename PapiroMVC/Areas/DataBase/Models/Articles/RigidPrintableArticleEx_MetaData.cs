using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;


namespace PapiroMVC.Models
{
    public class RigidPrintableArticle_MetaData : Printable_MetaData
    {
        [DisplayNameLocalized(typeof(ResRigidPrintableArticle), "Format")]
        public string Format { get; set; }
    }
}