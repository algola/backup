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
    public abstract partial class SheetPrintableArticleCost : IDataErrorInfo, ICloneable, IDeleteRelated
    {

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   "CostPerMq",
                   "CostPerSheet"
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
                
                //validazione della proprietà Address
                if (proprieta == "CostPerKg")
                {
                    if (this.CostPerKg != null)
                    {
                        Regex exp = new Regex(@"^(\d{1,3}(\.\d{3})*|(\d+))(\,\d{2})?$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.CostPerKg))
                        {
                            result = "Valuta non valida";
                        }
                    }
                }
                
                if (proprieta == "CostPerSheet")
                {
                    if (this.CostPerSheet != null)
                    {
                        Regex exp = new Regex(@"^(\d{1,3}(\.\d{3})*|(\d+))(\,\d{2})?$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.CostPerKg))
                        {
                            result = "Valuta non valida";
                        }
                    }
                }


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

            ((SheetPrintableArticleCost)to).CostPerKg = this.CostPerKg;
            ((SheetPrintableArticleCost)to).CostPerSheet = this.CostPerSheet;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}

