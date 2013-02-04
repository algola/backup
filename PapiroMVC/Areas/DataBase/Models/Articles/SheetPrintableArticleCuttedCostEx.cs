using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(SheetPrintableArticleCuttedCost_MetaData))]
    public partial class SheetPrintableArticleCuttedCost : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public SheetPrintableArticleCuttedCost()
        {
            this.TypeOfArticleCost = ArticleCost.ArticleCostType.SheetPrintableArticleCuttedCost;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {                  
                   //Specify validation property                       
                   //    "FormatMax",
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
                return result;
            }
        }

        //Check validation of entity
        public override bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret && base.IsValid;
            }
        }

        #endregion

        #region Handle copy for modify

        public override void Copy(ArticleCost to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((SheetPrintableArticleCuttedCost)to).UseTheSameCostOfPalletAfterKg = this.UseTheSameCostOfPalletAfterKg;
            ((SheetPrintableArticleCuttedCost)to).UseTheSameCostOfSheetAfterSheet = this.UseTheSameCostOfSheetAfterSheet;
            ((SheetPrintableArticleCuttedCost)to).KgPallet = this.KgPallet;
            ((SheetPrintableArticleCuttedCost)to).KgSheet = this.KgSheet;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
