﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18408
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models.Resources.Articles {
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
    public class ResRigidPrintableArticle {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResRigidPrintableArticle() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PapiroMVC.Models.Resources.Articles.ResRigidPrintableArticle", typeof(ResRigidPrintableArticle).Assembly);
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
        ///   Looks up a localized string similar to (^(\d{1,3}(\.\d{3})*|(\d+))(\,\d{0,5})?$)|(^$).
        /// </summary>
        public static string CurrencyValidation {
            get {
                return ResourceManager.GetString("CurrencyValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (^(\d{1,3}(\.\d{3})*|(\d+))(\,\d{0,5})?$)|(^$).
        /// </summary>
        public static string CurrencyValidation1 {
            get {
                return ResourceManager.GetString("CurrencyValidation1", resourceCulture);
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
        ///   Looks up a localized string similar to Formato valuta non valido (es. valido: 0,8756).
        /// </summary>
        public static string CurrencyValidationError1 {
            get {
                return ResourceManager.GetString("CurrencyValidationError1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Formato (cm x cm).
        /// </summary>
        public static string Format {
            get {
                return ResourceManager.GetString("Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quadratura minima.
        /// </summary>
        public static string FromMinFormat {
            get {
                return ResourceManager.GetString("FromMinFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quando si imposta una quadratura minima, nel calcolo del materiale si userà questa quadratura come minimale.
        /// </summary>
        public static string FromMinFormatToolTip {
            get {
                return ResourceManager.GetString("FromMinFormatToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^$|^(\d{1,4})((\,\d{0,5}){0,1})[xX](\d{1,4})((\,\d{0,5}){0,1})?$.
        /// </summary>
        public static string FromMinFormatValidation {
            get {
                return ResourceManager.GetString("FromMinFormatValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to formato di quadratura minima non valido. Es. 70x70 .
        /// </summary>
        public static string FromMinFormatValidationError {
            get {
                return ResourceManager.GetString("FromMinFormatValidationError", resourceCulture);
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
        ///   Looks up a localized string similar to richiesto per completare l&apos;operazione.
        /// </summary>
        public static string RequiredField1 {
            get {
                return ResourceManager.GetString("RequiredField1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        public static string String1 {
            get {
                return ResourceManager.GetString("String1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Arrotonda a Mq successivo.
        /// </summary>
        public static string ToNexMq {
            get {
                return ResourceManager.GetString("ToNexMq", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quando si seleziona questa opzione, il prezzo del materiale è arrotondato al mq successivo calcolato.
        /// </summary>
        public static string ToNexMqToolTip {
            get {
                return ResourceManager.GetString("ToNexMqToolTip", resourceCulture);
            }
        }
    }
}