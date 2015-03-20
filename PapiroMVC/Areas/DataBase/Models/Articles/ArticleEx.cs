using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Article_MetaData))]
    abstract partial class Article
    {

        /// <summary>
        /// return the type of quantity to display 
        /// </summary>
        public CostDetail.QuantityType TypeOfQuantity
        {
            get
            {
                CostDetail.QuantityType ret = CostDetail.QuantityType.RunTypeOfQuantity;

                switch (TypeOfArticle)
                {
                    case ArticleType.SheetPrintableArticle:
                        ret = CostDetail.QuantityType.RunTypeOfQuantity;
                        break;
                    case ArticleType.RollPrintableArticle:
                        ret = CostDetail.QuantityType.MqWorkTypeOfQuantity;
                        break;
                    case ArticleType.RigidPrintableArticle:
                        ret = CostDetail.QuantityType.MqWorkTypeOfQuantity;
                        break;
                    case ArticleType.ObjectPrintableArticle:
                        break;
                    default:
                        break;
                }

                return ret;
            }
        }

        #region Proprietà aggiuntive
        public enum ArticleType : int
        {
            SheetPrintableArticle = 0,
            RollPrintableArticle = 1,
            RigidPrintableArticle = 2,
            ObjectPrintableArticle = 3,

            NoPrintable = 4,

            DieSheet = 5,
            DieFlatRoll = 6,
            DieFlexo = 7,
            Ink = 8,
            Foil = 9


        }

        public ArticleType TypeOfArticle
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
           return "";
        }

    }
}
