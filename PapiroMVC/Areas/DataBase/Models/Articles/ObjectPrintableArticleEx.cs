using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(ObjectPrintableArticle_MetaData))]
    public partial class ObjectPrintableArticle : Printable, IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public ObjectPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.ObjectPrintableArticle;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {                  
                   //Specify validation property                       
                   "PrintableFormat"
                   //    "FormatMax",
               };

        public override string this[string proprieta]
        {
            get
            {
                string result = base[proprieta];
                if (proprieta == "PrintableFormat" && this.PrintableFormat != null)
                {
                    Regex exp = new Regex(@"^[0-9.,]{1,5}x[0-9.,]{1,5}$", RegexOptions.IgnoreCase);
                    if (!exp.IsMatch(this.PrintableFormat.ToString()))
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

            ((ObjectPrintableArticle)to).PrintableFormat = this.PrintableFormat;
            ((ObjectPrintableArticle)to).Size = this.Size;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
