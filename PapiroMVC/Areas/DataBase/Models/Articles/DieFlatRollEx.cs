using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DieFlatRoll_MetaData))]
    public partial class DieFlatRoll : Die
    {
        public DieFlatRoll()
        {
            TypeOfArticle = ArticleType.DieFlatRoll;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

    }
}

