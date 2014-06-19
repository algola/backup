using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Articles;

namespace PapiroMVC.Models
{
    public partial class DieFlexo_MetaData : Die_MetaData
    {
       
       
        [DisplayNameLocalized(typeof(ResArticle), "Width")]
        public Nullable<double> Width { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "Z")]
        public Nullable<double> Z { get; set; }
        public string PrintingFormat { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "DCut1Flexo")]
        public Nullable<double> DCut1 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "DCut2Flexo")]
        public Nullable<double> DCut2 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "MaxGain1Flexo")]
        public Nullable<int> MaxGain1 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "MaxGain2Flexo")]
        public Nullable<int> MaxGain2 { get; set; }
        [DisplayNameLocalized(typeof(ResArticle), "Description")]
        public string Description { get; set; }
    }
}