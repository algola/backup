using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(RollPrintableArticle_MetaData))]
    public partial class RollPrintableArticle : Printable
    {
        public RollPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RollPrintableArticle;
        }

        #region Added Properties

        #endregion


        public override string ToString()
        {
            return base.ToString() + this.Width + " ";
        }
    }
}
