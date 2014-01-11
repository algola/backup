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
    [MetadataType(typeof(ObjectPrintableArticle_MetaData))]
    public partial class ObjectPrintableArticle : Printable
    {
        public ObjectPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.ObjectPrintableArticle;
        }

        #region Added Properties

        #endregion

    }
}
