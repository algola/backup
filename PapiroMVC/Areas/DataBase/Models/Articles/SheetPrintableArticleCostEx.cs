using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;



namespace PapiroMVC.Models
{
    [MetadataType(typeof(SheetPrintableArticleCost_MetaData))]
    public abstract partial class SheetPrintableArticleCost : ArticleCost, ICloneable, IDeleteRelated
    {

        #region Added Properties

        #endregion

       

        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((SheetPrintableArticleCost)to).CostPerKg = this.CostPerKg;
            ((SheetPrintableArticleCost)to).CostPerSheet = this.CostPerSheet;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}

