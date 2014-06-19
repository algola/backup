using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DieSemiRoll_MetaData))]
    public partial class DieSemiRoll : Die
    {
        public DieSemiRoll()
        {
            TypeOfArticle = ArticleType.DieSemiRoll;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

    }
}

