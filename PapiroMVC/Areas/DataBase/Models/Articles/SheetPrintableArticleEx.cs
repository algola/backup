using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(SheetPrintableArticle_MetaData))]
    public partial class SheetPrintableArticle : Printable, ICloneable, IDeleteRelated
    {
        public SheetPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.SheetPrintableArticle;
        }

        #region Added Properties

        #endregion


        #region Handle copy for modify

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((SheetPrintableArticle)to).Format = this.Format;
            ((SheetPrintableArticle)to).NoPinza = this.NoPinza;
            ((SheetPrintableArticle)to).SheetPerPacked = this.SheetPerPacked;
            ((SheetPrintableArticle)to).SheetPerPallet = this.SheetPerPallet;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.Format + " ";
        }
    }
}
