using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PapiroMVC.Validation;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(RigidPrintableArticle_MetaData))]
    public partial class RigidPrintableArticle : Printable
    {
        public RigidPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RigidPrintableArticle;
        }

        #region Added Properties

        #endregion


    }
}
