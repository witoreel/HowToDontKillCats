﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Way2Test1.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Way2Test1.Properties.Resources", typeof(Resources).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A palavra está localizada no índice {0}..
        /// </summary>
        internal static string ConsoleAnswerFound {
            get {
                return ResourceManager.GetString("ConsoleAnswerFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Neste processo de busca foram mortos {0} gatinhos..
        /// </summary>
        internal static string ConsoleAnswerIterationPlural {
            get {
                return ResourceManager.GetString("ConsoleAnswerIterationPlural", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Neste processo de busca foi morto {0} gatinho..
        /// </summary>
        internal static string ConsoleAnswerIterationSingle {
            get {
                return ResourceManager.GetString("ConsoleAnswerIterationSingle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Palavra não encontrada no dicionário..
        /// </summary>
        internal static string ConsoleAnswerNotFound {
            get {
                return ResourceManager.GetString("ConsoleAnswerNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Insira a palavra a qual deseja obter o índice:.
        /// </summary>
        internal static string ConsoleAskKeyword {
            get {
                return ResourceManager.GetString("ConsoleAskKeyword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Deseja realizar uma nova pesquisa? (S/N).
        /// </summary>
        internal static string ConsoleAskReSearch {
            get {
                return ResourceManager.GetString("ConsoleAskReSearch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O programa será finalizado em instantes....
        /// </summary>
        internal static string ConsoleExit {
            get {
                return ResourceManager.GetString("ConsoleExit", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dicionário Online Way2.
        /// </summary>
        internal static string ConsoleHeader {
            get {
                return ResourceManager.GetString("ConsoleHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ===============================================================================.
        /// </summary>
        internal static string ConsoleSeparator {
            get {
                return ResourceManager.GetString("ConsoleSeparator", resourceCulture);
            }
        }
    }
}
