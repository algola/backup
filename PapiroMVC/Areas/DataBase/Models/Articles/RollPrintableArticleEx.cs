using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Resources;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(RollPrintableArticle_MetaData))]
    public partial class RollPrintableArticle : Printable
    {
        public RollPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RollPrintableArticle;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {

            //LANGFILE
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);


            // Get the resource strings for the day, year, and holiday  
            // using the current UI culture. 



            return base.ToString() + " " + resman.GetString("Height") +
            this.Width + " ";
        }

        public override string GetEditMethod()
        {
            return "EditRollPrintableArticle";
        }

        public override double TransformQuantity(double quantity, CostDetail.QuantityType from)
        {
            if (from == CostDetail.QuantityType.MqWorkTypeOfQuantity)
            {
                return Math.Floor(quantity / ((Width ?? 0) / 100));
            }
            else
            {
                return quantity;
            }
        }

    }
}
