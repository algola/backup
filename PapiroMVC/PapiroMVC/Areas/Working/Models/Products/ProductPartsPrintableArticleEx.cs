using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ProductPartsPrintableArticle_MetaData))]
    public abstract partial class ProductPartsPrintableArticle
    {
        #region Proprietà aggiuntive

        public virtual bool IsInList(IQueryable<Article> arts)
        {
            throw new Exception();
        }


        public enum TypeOfProductPartsPrintableArticleType : int
        {
            ProductPartSheetArticle = 0,
        }

        public TypeOfProductPartsPrintableArticleType TypeOfProductPartsPrintableArticle
        {
            get;
            protected set;
        }
        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
            return this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Color;
        }

    }
}
