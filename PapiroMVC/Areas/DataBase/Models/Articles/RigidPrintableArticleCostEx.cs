using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{

    [MetadataType(typeof(RigidPrintableArticleCost_MetaData))]
    public abstract partial class RigidPrintableArticleCost : ArticleCost, ICloneable, IDeleteRelated
    {
        #region Added Properties

        #endregion


        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RigidPrintableArticleCost)to).CostPerMq = this.CostPerMq;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion
    }
}

