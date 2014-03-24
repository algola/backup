using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(NoPrintable_MetaData))]
    public partial class NoPrintable : Article
    {
        public NoPrintable()
        {
            TypeOfArticle = ArticleType.NoProntable;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

    }
}

