using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{

    [MetadataType(typeof(CustomerSupplierBase_MetaData))]
    public partial class CustomerSupplierBase : IDataErrorInfo, ICloneable, IDeleteRelated
    {
        #region Gestione Errori

        private static readonly string[] proprietaDaValidare =
               {
                       "TypeOfBase",
                       "Address",
                       "City",
                       "Province",
                       "PostalCode",
                       "Country",
                       "Phone",
                       "Fax",
                       "Email",
                       "Note",
                       "Referee",
                       "Pec"
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
                //validazione della proprietà Tipo CustomerSupplierBase
                //if (proprieta == "TypeOfBase")
                //{
                //    if (this.TypeOfBase == null)
                //    {
                //        result = "Nessun tipo sede selezionato";
                //    }
                //}

                //validazione della proprietà Address
                if (proprieta == "Address")
                {
                    if (this.Address != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,200}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Address))
                        {
                            result = "Superata la lunghezza massima per l'indirizzo consentita";
                        }
                    }
                }

                //validazione della proprietà Città
                if (proprieta == "City")
                {
                    if (this.City != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,100}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.City))
                        {
                            result = "Superata la lunghezza massima per la città consentita";
                        }
                    }
                }

                //validazione della proprietà Province
                if (proprieta == "Province")
                {
                    if (this.Province != null)
                    {
                        Regex exp = new Regex(@"^[A-Za-z]{2}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Province))
                        {
                            result = "Province inserita in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà PostalCode
                if (proprieta == "PostalCode")
                {
                    if (this.PostalCode != null)
                    {
                        Regex exp = new Regex(@"^(V-|I-)?[0-9]{5}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.PostalCode))
                        {
                            result = "PostalCode inserito in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Country
                if (proprieta == "Country")
                {
                    if (this.Country != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,255}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Country))
                        {
                            result = "Country inserito in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Phone
                if (proprieta == "Phone")
                {
                    if (this.Phone != null)
                    {
                        Regex exp = new Regex(@"^[0-9.,-/ ]{0,30}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Phone))
                        {
                            result = "Phone inserito in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Fax
                if (proprieta == "Fax")
                {
                    if (this.Fax != null)
                    {
                        Regex exp = new Regex(@"^[0-9.,-/ ]{0,30}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Fax))
                        {
                            result = "Fax inserito in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Email
                if (proprieta == "Email")
                {
                    if (this.Email != null)
                    {
                        Regex exp = new Regex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Email))
                        {
                            result = "Email inserita in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Email
                if (proprieta == "Pec")
                {
                    if (this.Pec != null)
                    {
                        Regex exp = new Regex(@"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Pec))
                        {
                            result = "Pec inserita in modo non corretto";
                        }
                    }
                }

                //validazione della proprietà Note
                if (proprieta == "Note")
                {
                    if (this.Note != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,255}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Note))
                        {
                            result = "Superata la lunghezza delle note consentita";
                        }
                    }
                }

                //validazione della proprietà Referee
                if (proprieta == "Referee")
                {
                    if (this.Referee != null)
                    {
                        Regex exp = new Regex(@"^[\w\s\x00-\xFF]{0,255}$", RegexOptions.IgnoreCase);
                        if (!exp.IsMatch(this.Referee))
                        {
                            result = "Superata la lunghezza per il riferimento consentita";
                        }
                    }
                }

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

        public virtual void Copy(CustomerSupplierBase to)
        {
            //All properties of object
            //and pointer of sons
            to.CodCustomerSupplier = this.CodCustomerSupplier;
            to.CodCustomerSupplierBase = this.CodCustomerSupplierBase;
            to.CodTypeOfBase = this.CodTypeOfBase;
            to.Address = this.Address;
            to.City = this.City;
            to.Province = this.Province;
            to.PostalCode = this.PostalCode;
            to.Country = this.Country;
            to.Phone = this.Phone;
            to.Fax = this.Fax;
            to.Email = this.Email;
            to.Note = this.Note;
            to.Referee = this.Referee;
            to.Pec = this.Pec;
            to.TypeOfBase = this.TypeOfBase;
        }


        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var tipoDelloOggetto = this.GetType();
            //istanzio una copia che sarà gestita dall'invio
            CustomerSupplierBase copiaCustomerSupplierBase = (CustomerSupplierBase)Activator.CreateInstance(tipoDelloOggetto);
            //l'oggetto copisa sarà una copia del contenuto dell'oggetto originale
            this.Copy(copiaCustomerSupplierBase);

            return copiaCustomerSupplierBase;
        }

        public void ChildsNull()
        {
        }

        #endregion
    }
}
