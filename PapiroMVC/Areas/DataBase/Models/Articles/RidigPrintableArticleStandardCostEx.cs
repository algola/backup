using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(RigidPrintableArticleStandardCost_MetaData))]
    public partial class RigidPrintableArticleStandardCost : RigidPrintableArticleCost , ICloneable, IDeleteRelated
    {
        public RigidPrintableArticleStandardCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.RigidPrintableArticleStandardCost;
        }

        #region Added Properties

        #endregion


        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
