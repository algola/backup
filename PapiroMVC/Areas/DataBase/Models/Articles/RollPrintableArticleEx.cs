using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(RollPrintableArticle_MetaData))]
    public partial class RollPrintableArticle : Printable, ICloneable, IDeleteRelated
    {
        public RollPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RollPrintableArticle;
        }

        #region Added Properties

        #endregion

        #region Handle copy for modify

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RollPrintableArticle)to).Width = this.Width;
            ((RollPrintableArticle)to).MqForafait = this.MqForafait;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.Width + " ";
        }
    }
}
