using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DieSheet_MetaData))]
    public partial class DieSheet : Die
    {
        public DieSheet()
        {
            TypeOfArticle = ArticleType.DieSheet;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

        public override string GetEditMethod()
        {
            return "EditDieSheet";
        }
    }
}

