
namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Profile_MetaData))]
    public partial class Profile : IDataErrorInfo, ICloneable, IDeleteRelated
    {


        #region Error Handle

        private static readonly string[] proprietaDaValidare =
               {
                   //Specify validation property
                       ""
               };

        public string Error
        {
            get
            {
                return null;
            }
        }

        public virtual string this[string proprieta]
        {
            get
            {
                string result = null;
                return result;
            }
        }

        //Check validation of entity
        public virtual bool IsValid
        {
            get
            {
                bool ret = true;
                foreach (string prop in proprietaDaValidare)
                {
                    if (this[prop] != null)
                        ret = false;
                }
                return ret;
            }
        }

        #endregion

        #region Handle copy for modifyArticle

        public virtual void Copy(Profile to)
        {
            //All properties of object
            //and pointer of sons
            to.Name = this.Name;
            to.CompanyName = this.CompanyName;
            to.Base = this.Base;

            //must: modify with child
            to.Refeere = this.Refeere;
            to.Culture = this.Culture;
            to.Phone = this.Phone;
            to.VatNumber = this.VatNumber;
            to.TaxCode = this.TaxCode;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Profile copyOfObject = (Profile)Activator.CreateInstance(kindOfObject);
            //l'oggetto copia sarà una copia del contenuto dell'oggetto originale
            this.Copy(copyOfObject);

            //CREATE DUPLICATION OF ANY FIRST GENERATION OF CHILD
            //Example

            return copyOfObject;
        }

        public void ChildsNull()
        {
            //Set all chied to null 

            //Example
            //this.Prodotto = null;
        }

        #endregion

        public override string ToString()
        {
            return "";
        }

    }
}
