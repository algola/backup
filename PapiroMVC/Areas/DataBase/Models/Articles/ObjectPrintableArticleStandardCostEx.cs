using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ObjectPrintableArticleStandardCost_MetaData))]
    public partial class ObjectPrintableArticleStandardCost : ObjectPrintableArticleCost,  ICloneable, IDeleteRelated
    {
        public ObjectPrintableArticleStandardCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.ObjectPrintableArticleStandardCost;
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
