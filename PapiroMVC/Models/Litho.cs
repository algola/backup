//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public abstract partial class Litho : PrinterMachine
    {
        public Nullable<long> PrintingUnit { get; set; }
        public Nullable<long> SheetwiseAfterPrintingUnit { get; set; }
        public Nullable<bool> Sheetwise { get; set; }
        public Nullable<System.TimeSpan> WashUpTime { get; set; }
        public Nullable<System.TimeSpan> ChangePlateTime { get; set; }
    }
}
