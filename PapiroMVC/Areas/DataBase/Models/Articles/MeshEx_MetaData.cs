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
    public partial class Mesh_MetaData : NoPrintable_MetaData
    {

        [DisplayNameLocalized(typeof(ResArticle), "MeshRow")]
        [Tooltip(typeof(ResArticle), "MeshRowToolTip")]
        public Nullable<int> MeshRow { get; set; }

        [DisplayNameLocalized(typeof(ResArticle), "GainMqPerLt")]
        [Tooltip(typeof(ResArticle), "GainMqPerLtToolTip")]
        public Nullable<int> GainMqPerLt { get; set; }
    }
}