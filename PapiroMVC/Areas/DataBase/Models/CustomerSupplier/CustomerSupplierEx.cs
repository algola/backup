using System.Text.RegularExpressions;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Validation;


namespace PapiroMVC.Models
{
        [Serializable]
    [MetadataType(typeof(CustomerSupplier_MetaData))]
    public abstract partial class CustomerSupplier : IDataErrorInfo
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

    }

}
