﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
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
    public class ResSheetPrintableArticle {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResSheetPrintableArticle() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PapiroMVC.Models.Resources.Articles.ResSheetPrintableArticle", typeof(ResSheetPrintableArticle).Assembly);
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
        ///   Looks up a localized string similar to formato valuta non valido (es. valido: 0,8756).
        /// </summary>
        public static string CurrencyValidationError {
            get {
                return ResourceManager.GetString("CurrencyValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fomato (cm x cm).
        /// </summary>
        public static string Format {
            get {
                return ResourceManager.GetString("Format", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^(\d{1,4})((\,\d{0,5}){0,1})[xX](\d{1,4})((\,\d{0,5}){0,1})?$.
        /// </summary>
        public static string FormatValidation {
            get {
                return ResourceManager.GetString("FormatValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to formato carta non valido (es. valido: 70x100).
        /// </summary>
        public static string FormatValidationError {
            get {
                return ResourceManager.GetString("FormatValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No pinza.
        /// </summary>
        public static string NoPinza {
            get {
                return ResourceManager.GetString("NoPinza", resourceCulture);
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
        ///   Looks up a localized string similar to Fogli per pacco.
        /// </summary>
        public static string SheetPerPacked {
            get {
                return ResourceManager.GetString("SheetPerPacked", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fogli per pallet.
        /// </summary>
        public static string SheetPerPallet {
            get {
                return ResourceManager.GetString("SheetPerPallet", resourceCulture);
            }
        }
    }
}
