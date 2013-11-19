using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ObjectPrintableArticleCost_MetaData))]
    public abstract partial class ObjectPrintableArticleCost : ArticleCost,  ICloneable, IDeleteRelated
    {

        #region Added Properties

        #endregion

        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((ObjectPrintableArticleCost)to).CostPerUnit = this.CostPerUnit;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}

