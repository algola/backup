using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(RollPrintableArticleCuttedCost_MetaData))]
    public partial class RollPrintableArticleCuttedCost : RollPrintableArticleCost, ICloneable, IDeleteRelated
    {
        public RollPrintableArticleCuttedCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.RollPrintableArticleCuttedCost;
        }

        #region Added Properties

        #endregion


        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RollPrintableArticleCuttedCost)to).UseTheSameCostOfStandardWidthAfterKg = this.UseTheSameCostOfStandardWidthAfterKg;
            ((RollPrintableArticleCuttedCost)to).UseTheSameCostOfStandardWidthAfterMl = this.UseTheSameCostOfStandardWidthAfterMl;
            ((RollPrintableArticleCuttedCost)to).Kg = this.Kg;
            ((RollPrintableArticleCuttedCost)to).Ml = this.Ml;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
