using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class DieFlatRoll_MetaData : Die_MetaData
    {
        [DisplayNameLocalized(typeof(ResArticle), "TaskExecutorName")]
        public string TaskExecutorName { get; set; }




    }
}