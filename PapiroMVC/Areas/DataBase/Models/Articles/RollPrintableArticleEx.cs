using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(RollPrintableArticle_MetaData))]
    public partial class RollPrintableArticle : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        public RollPrintableArticle()
        {
            this.TypeOfArticle = Article.ArticleType.RollPrintableArticle;
        }

        #region Added Properties

        #endregion

        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {                  
                   //Specify validation property                                          
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

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((RollPrintableArticle)to).Width = this.Width;

            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion

    }
}
