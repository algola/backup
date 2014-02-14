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
    [MetadataType(typeof(SheetPrintableArticle_MetaData))]
    public partial class SheetPrintableArticle : Printable
    {
        public SheetPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.SheetPrintableArticle;
        }

        #region Added Properties

        #endregion


        public override string ToString()
        {
            return base.ToString() + this.Format + " ";
        }
    }
}
