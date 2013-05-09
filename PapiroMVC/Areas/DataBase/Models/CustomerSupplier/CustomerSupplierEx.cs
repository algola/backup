using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{

    [MetadataType(typeof(CustomerSupplier_MetaData))]
    public abstract partial class CustomerSupplier : IDataErrorInfo, ICloneable, IDeleteRelated
    {

        public enum CustomerSupplierType : int
        {
            Customer = 0,
            Supplier = 1,
        }

        public CustomerSupplierType TypeOfCustomerSupplier
        {
            get;
            protected set;
        }

        #region Gestione Errori

        private static readonly string[] proprietaDaValidare =
               {
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

        //per la validazione
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

        #region Metodi per gestire Copie

        public virtual void Copy(CustomerSupplier to)
        {
            //All properties of object
            //and pointer of sons
            to.CodCustomerSupplier = this.CodCustomerSupplier;
            to.BusinessName = this.BusinessName;
            to.VatNumber = this.VatNumber;
            to.TaxCode = this.TaxCode;
            to.Outdated = this.Outdated;
            to.CustomerSupplierBases = this.CustomerSupplierBases;
        }


        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var tipoDelloOggetto = this.GetType();
            //istanzio una copia che sarà gestita dall'invio
            CustomerSupplier copiaCustomerSupplier = (CustomerSupplier)Activator.CreateInstance(tipoDelloOggetto);
            //l'oggetto copisa sarà una copia del contenuto dell'oggetto originale
            this.Copy(copiaCustomerSupplier);

            return copiaCustomerSupplier;
        }

        public void ChildsNull()
        {
        }

        #endregion

    }

}
