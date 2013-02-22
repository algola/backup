using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(SheetPrintableArticle_MetaData))]
    public partial class SheetPrintableArticle : Printable, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public SheetPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.SheetPrintableArticle;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {                  
                   //Specify validation property                       
                   "Format"
                   //    "FormatMax",
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
                if (proprieta == "Format" && this.Format != null)
                {
                    Regex exp = new Regex(@"^[0-9.,]{1,5}x[0-9.,]{1,5}$", RegexOptions.IgnoreCase);
                    if (!exp.IsMatch(this.Format.ToString()))
                    {
                        result = "Messagge Error";
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

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((SheetPrintableArticle)to).Format = this.Format;
            ((SheetPrintableArticle)to).NoPinza = this.NoPinza;
            ((SheetPrintableArticle)to).SheetPerPacked = this.SheetPerPacked;
            ((SheetPrintableArticle)to).SheetPerPallet = this.SheetPerPallet;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.Format + " ";
        }
    }
}
