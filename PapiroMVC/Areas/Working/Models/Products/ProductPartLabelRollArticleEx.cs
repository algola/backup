using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartLabelRollArticle))]
    [MetadataType(typeof(ProductPartLabelRollArticleEx_MetaData))]
    public partial class ProductPartLabelRollArticle : ProductPartsPrintableArticle
    {
    }
}
