﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models.Resources.CustomerSupplier {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ResCustomerSupplier {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResCustomerSupplier() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PapiroMVC.Models.Resources.CustomerSupplier.ResCustomerSupplier", typeof(ResCustomerSupplier).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Ragione Sociale.
        /// </summary>
        public static string BusinessName {
            get {
                return ResourceManager.GetString("BusinessName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La ragione sociale del cliente comparirà nei documenti riferiti ad esso e potrà essere usata per la ricerca.
        /// </summary>
        public static string BusinessNameToolTip {
            get {
                return ResourceManager.GetString("BusinessNameToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Codice Cliente (se lasciato vuoto sarà assegnato dal sistema).
        /// </summary>
        public static string CodCustomerSupplier {
            get {
                return ResourceManager.GetString("CodCustomerSupplier", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to se lasciato vuoto sarà assegnato dal sistema.
        /// </summary>
        public static string CodCustomerSupplierToolTip {
            get {
                return ResourceManager.GetString("CodCustomerSupplierToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (^(\d{1,3}(\.\d{3})*|(\d+))(\,\d{0,5})?$)|(^$).
        /// </summary>
        public static string CurrencyValidation {
            get {
                return ResourceManager.GetString("CurrencyValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Formato valuta non valido (es. valido: 0,8756).
        /// </summary>
        public static string CurrencyValidationError {
            get {
                return ResourceManager.GetString("CurrencyValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to In Disuso.
        /// </summary>
        public static string Outdated {
            get {
                return ResourceManager.GetString("Outdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Le anagrafiche contrassegnate come &quot;In Disuso&quot; non possono essere usate per nuovi documenti.
        /// </summary>
        public static string OutdatedToolTip {
            get {
                return ResourceManager.GetString("OutdatedToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to richiesto per completare l&apos;operazione.
        /// </summary>
        public static string RequiredField {
            get {
                return ResourceManager.GetString("RequiredField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Codice Fiscale.
        /// </summary>
        public static string TaxCode {
            get {
                return ResourceManager.GetString("TaxCode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Codice Fiscale.
        /// </summary>
        public static string TaxCodeToolTip {
            get {
                return ResourceManager.GetString("TaxCodeToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Partita Iva.
        /// </summary>
        public static string VatNumber {
            get {
                return ResourceManager.GetString("VatNumber", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Partita Iv.
        /// </summary>
        public static string VatNumberToolTip {
            get {
                return ResourceManager.GetString("VatNumberToolTip", resourceCulture);
            }
        }
    }
}
