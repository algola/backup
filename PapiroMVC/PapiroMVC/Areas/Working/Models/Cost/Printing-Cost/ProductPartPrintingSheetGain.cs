using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class ProductPartPrintingSheetGain : ProductPartPrintingGain
    {
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------
        public string LargerFormat { get; set; }




        /// <summary>
        /// if is true system computes gain with Perfecting and put result in aspecific properties
        /// </summary>
        //        public bool UsePerfecting { get; set; }

    }

}