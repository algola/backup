using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Ink_MetaData))]
    public partial class Ink:NoPrintable     {
        public Ink()
        {
            TypeOfArticle = ArticleType.Ink;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return this.ArticleName;
        }

    }
}

