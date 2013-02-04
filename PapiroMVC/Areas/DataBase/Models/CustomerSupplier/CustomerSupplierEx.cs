using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{

    [MetadataType(typeof(CustomerSupplierEx_MetaData))]
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
                   "CodCustomerSupplier",
                   "BusinessName",
                   "VatNumber",
                   "TaxCode"
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
                //validazione della proprietà Ragione Sociale
                if (proprieta == "CodCustomerSupplier")
                {
                    if (this.CodCustomerSupplier != null)
                    {
                        Regex exp = new Regex(@"^[A-Za-z0-9]{3,20}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.CodCustomerSupplier))
                        {
                            result = "Formato del codice non corretto";
                        }
                    }
                    else
                    {
                        result = "";
                    }
                }

                //validazione della proprietà Ragione Sociale
                if (proprieta == "BusinessName")
                {
                    if (this.BusinessName != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{1,255}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.BusinessName))
                        {
                            result = "Superata la lunghezza della ragione sociale consentita";
                        }
                    }
                    else
                    {
                        result = "Nessuna ragione sociale inserita";
                    }
                }

                //validazione della proprietà Partita Iva
                if (proprieta == "VatNumber")
                {
                    if (this.VatNumber != null)
                    {
                        Regex exp = new Regex(@"^[0-9]{11}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.VatNumber))
                        {
                            result = "Partita Iva non inserita correttamente";
                        }
                    }
                    else
                    {
                        result = "Nessuna partita iva inserita";
                    }
                }

                ////validazione della proprietà Codice Fiscale
                //if (proprieta == "TaxCode")
                //{
                //    if (this.TaxCode != null)
                //    {
                //        Regex exp = new Regex(@"^[A-Za-z]{6}[0-9]{2}[A-Za-z][0-9]{2}[A-Za-z][0-9]{3}[A-Za-z]$", RegexOptions.IgnoreCase);
                //        if (!exp.IsMatch(this.TaxCode))
                //        {
                //            result = "Codice fiscale non inserito correttamente";
                //        }
                //    }
                //    else
                //    {
                //        result = "Nessun codice fiscale inserito";
                //    }
                //}

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
