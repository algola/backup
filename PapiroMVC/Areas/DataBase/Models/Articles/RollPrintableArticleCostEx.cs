using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;



namespace PapiroMVC.Models
{   
    public abstract partial class RollPrintableArticleCost : ArticleCost, ICloneable, IDeleteRelated
    {

        #region Added Properties

        #endregion

        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RollPrintableArticleCost)to).CostPerMq = this.CostPerMq;
            ((RollPrintableArticleCost)to).CostPerMl = this.CostPerMl;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}

