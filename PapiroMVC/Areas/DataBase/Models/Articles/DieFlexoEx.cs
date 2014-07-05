﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(DieFlexo_MetaData))]
    public partial class DieFlexo : Die
    {
        public DieFlexo()
        {
            TypeOfArticle = ArticleType.DieFlexo;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

    }
}
