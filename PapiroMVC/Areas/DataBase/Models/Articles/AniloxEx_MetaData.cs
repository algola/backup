using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    public partial class Anilox_MetaData : NoPrintable_MetaData
    {

        [DisplayNameLocalized(typeof(ResArticle), "AniloxRow")]
        [Tooltip(typeof(ResArticle), "AniloxRowToolTip")]
        public Nullable<int> AniloxRow { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "GainMqPerLt")]
        [Tooltip(typeof(ResArticle), "GainMqPerLtToolTip")]
        public Nullable<int> GainMqPerLt { get; set; }
    }
}