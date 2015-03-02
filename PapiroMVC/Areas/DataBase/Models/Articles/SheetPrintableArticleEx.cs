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
    [MetadataType(typeof(SheetPrintableArticle_MetaData))]
    public partial class SheetPrintableArticle : Printable
    {
        public SheetPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.SheetPrintableArticle;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {

            //LANGFILE
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);


            // Get the resource strings for the day, year, and holiday  
            // using the current UI culture. 

            return this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Format + " " + this.Weight + " " + resman.GetString("Weight"); 
        }
    }
}
