﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models.Resources.TaskExecutor {
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
    public class ResDigital {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResDigital() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PapiroMVC.Models.Resources.TaskExecutor.ResDigital", typeof(ResDigital).Assembly);
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
        ///   Looks up a localized string similar to Stampa BN fronte.
        /// </summary>
        public static string BWSide1 {
            get {
                return ResourceManager.GetString("BWSide1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se stampa il bn fronte.
        /// </summary>
        public static string BWSide1ToolTip {
            get {
                return ResourceManager.GetString("BWSide1ToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stampa BN retro.
        /// </summary>
        public static string BWSide2 {
            get {
                return ResourceManager.GetString("BWSide2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Se stampa il bn retro.
        /// </summary>
        public static string BWSide2ToolTip {
            get {
                return ResourceManager.GetString("BWSide2ToolTip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stampa  a colori fronte.
        /// </summary>
        public static string ColorSide1 {
            get {
                return ResourceManager.GetString("ColorSide1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stampa  a colori retro.
        /// </summary>
        public static string ColorSide2 {
            get {
                return ResourceManager.GetString("ColorSide2", resourceCulture);
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
        ///   Looks up a localized string similar to richiesto per completare l&apos;operazione.
        /// </summary>
        public static string RequiredField {
            get {
                return ResourceManager.GetString("RequiredField", resourceCulture);
            }
        }
    }
}
