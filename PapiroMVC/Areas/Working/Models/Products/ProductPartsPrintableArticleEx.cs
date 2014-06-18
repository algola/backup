﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [KnownType(typeof(ProductPartsPrintableArticle))]
    [MetadataType(typeof(ProductPartsPrintableArticle_MetaData))]
    public abstract partial class ProductPartsPrintableArticle
    {
        #region Proprietà aggiuntive

        public virtual bool IsInList(IQueryable<Article> arts)
        {
            var sel = arts.OfType<Printable>();
            var cont = (sel.Where(c => c.NameOfMaterial == this.NameOfMaterial &&
                c.TypeOfMaterial == this.TypeOfMaterial &&
                c.Weight == this.Weight &&
                c.Adhesive == this.Adhesive &&
                c.Color == this.Color).Count());

            return (cont > 0);
        }


        public enum TypeOfProductPartsPrintableArticleType : int
        {
            ProductPartSheetArticle = 0,
            ProductPartRigidArticle = 1,
            ProductPartLabelRollArticle = 2,


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
            return this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Color; //ADES +this.Adhesive;                
        }

        public virtual void ToName()
        {
            var x = ProductPart.Product.ProductNameGenerator;
            x = x.Replace("%TYPEMATERIAL", this.TypeOfMaterial);
            x = x.Replace("%NAMEMATERIAL", this.NameOfMaterial);
            x = x.Replace("%COLORMATERIAL", this.Color);
            x = x.Replace("%ADESHIVEMATERIAL", this.Adhesive);

            ProductPart.Product.ProductNameGenerator = x;
        }

    }
}
