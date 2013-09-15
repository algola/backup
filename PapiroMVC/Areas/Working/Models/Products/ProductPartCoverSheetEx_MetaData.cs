using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Products;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public partial class ProductPartCoverSheet_MetaData
    {
        [Required(ErrorMessageResourceType = typeof(ResProductPartCoverSheet), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartCoverSheet), "WidthWings")]
        [Tooltip(typeof(ResProductPartCoverSheet), "WidthWingsToolTip")]
        public Nullable<double> WidthWings { get; set; }

        [Required(ErrorMessageResourceType = typeof(ResProductPartCoverSheet), ErrorMessageResourceName = "RequiredField")]
        [DisplayNameLocalized(typeof(ResProductPartCoverSheet), "Back")]
        [Tooltip(typeof(ResProductPartCoverSheet), "BackToolTip")]
        public Nullable<double> Back { get; set; }
    }
}