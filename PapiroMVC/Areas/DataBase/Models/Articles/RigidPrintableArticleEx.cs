using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PapiroMVC.Validation;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(RigidPrintableArticle_MetaData))]
    public partial class RigidPrintableArticle : Printable, ICloneable, IDeleteRelated
    {
        public RigidPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RigidPrintableArticle;
        }

        #region Added Properties

        #endregion

        #region Handle copy for modify

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RigidPrintableArticle)to).Format = this.Format;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
