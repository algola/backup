using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract class ProductPartPrintingSheetGain
    {
        //this is the list of the results per sheet in case of sheet!!!!!
        public List<Makeready> Makereadys { get; set; }        
        
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------
        public string LargerFormat { get; set; }
        public string SmallerFormat { get; set; }

        /// <summary>
        /// if is true system computes gain with Perfecting and put result in aspecific properties
        /// </summary>
        public bool UsePerfecting { get; set; }

        //this method calculate gain with aux of CalculateShapeOnBuyingFormat()
        public abstract void CalculateGain();
        protected abstract Makeready CalculateShapeOnBuyingFormat();

    }

}