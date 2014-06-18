﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models.Resources.ViewModels {
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
    public class ResProductViewModel {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResProductViewModel() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PapiroMVC.Models.Resources.ViewModels.ResProductViewModel", typeof(ResProductViewModel).Assembly);
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
        ///   Looks up a localized string similar to (^(\b|\+|\-|)(\d{1,3}(\.\d{3})*|(\d+))(\,\d{0,5})*(\b|\%|)?$)|(^$).
        /// </summary>
        public static string AutoChangesValidation {
            get {
                return ResourceManager.GetString("AutoChangesValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Inserire un&apos;espressione valita (tipo: +5% oppure -0,50 etc..).
        /// </summary>
        public static string AutoChangesValidationError {
            get {
                return ResourceManager.GetString("AutoChangesValidationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cliente.
        /// </summary>
        public static string Customer {
            get {
                return ResourceManager.GetString("Customer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nome o riferimento al cliente in anagrafia.
        /// </summary>
        public static string CustomerToolTip {
            get {
                return ResourceManager.GetString("CustomerToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rif. gruppo.
        /// </summary>
        public static string DocumentName {
            get {
                return ResourceManager.GetString("DocumentName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gruppo di articoli o riferimento ad essi &lt;a href=&apos;http://www.gestionestampa.com/?p=374&apos; target=&apos;_blank&apos;&gt;Approfondisi&lt;/a&gt;.
        /// </summary>
        public static string DocumentNameToolTip {
            get {
                return ResourceManager.GetString("DocumentNameToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Rif. articolo.
        /// </summary>
        public static string ProductName {
            get {
                return ResourceManager.GetString("ProductName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Nome dell&apos;articolo o riferimento alla commissione del cliente.
        /// </summary>
        public static string ProductNameToolTip {
            get {
                return ResourceManager.GetString("ProductNameToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Quantità.
        /// </summary>
        public static string Quantities {
            get {
                return ResourceManager.GetString("Quantities", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Questo campo è richiesto per completare l&apos;operazione.
        /// </summary>
        public static string RequiredField {
            get {
                return ResourceManager.GetString("RequiredField", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (^0*[1-9][0-9]*(\.[0-9]+)?|0+\.[0-9]*[1-9][0-9]*$).
        /// </summary>
        public static string UIntValidation {
            get {
                return ResourceManager.GetString("UIntValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to La quantità deve essere positiva.
        /// </summary>
        public static string UIntValidationError {
            get {
                return ResourceManager.GetString("UIntValidationError", resourceCulture);
            }
        }
    }
}
