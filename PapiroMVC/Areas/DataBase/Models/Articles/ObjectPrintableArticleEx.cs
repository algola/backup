using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ObjectPrintableArticle_MetaData))]
    public partial class ObjectPrintableArticle : Printable,  ICloneable, IDeleteRelated
    {
        public ObjectPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.ObjectPrintableArticle;
        }

        #region Added Properties

        #endregion


        #region Handle copy for modify

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((ObjectPrintableArticle)to).PrintableFormat = this.PrintableFormat;
            ((ObjectPrintableArticle)to).Size = this.Size;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
