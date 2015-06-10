using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Foil_MetaData))]
    public partial class Foil:NoPrintable 
    {
        public Foil()
        {
            TypeOfArticle = ArticleType.Foil;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return this.ArticleName;
        }

        public override string GetEditMethod()
        {
            return "EditFoil";
        }
    }
}

