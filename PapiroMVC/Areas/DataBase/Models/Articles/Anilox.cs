using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Anilox_MetaData))]
    public partial class Anilox : NoPrintable
    {
        public Anilox()
        {
            TypeOfArticle = ArticleType.Anilox;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return this.ArticleName;
        }

        public override string GetEditMethod()
        {
            return "EditAnilox";
        }
    }
}

