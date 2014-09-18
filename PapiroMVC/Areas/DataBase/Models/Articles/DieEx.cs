using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{

    public class FormatTypeForDropDown
    {
        public string FormatTypeName { get; set; }
        public int FormatType { get; set; }
    }

    public class DescForDropDown
    {
        public string Name { get; set; }
        public int Cod { get; set; }
    }

    [Serializable]
    // [MetadataType(typeof(Die_MetaData))]
    public partial class Die : NoPrintable
    {
        public Die()
        {
            TypeOfArticle = ArticleType.DieSheet;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }

    }
}

