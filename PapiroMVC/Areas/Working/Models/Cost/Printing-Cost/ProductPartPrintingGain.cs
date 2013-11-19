using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public  abstract partial class ProductPartPrintingGain
    {
             
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------        
        public string SmallerFormat { get; set; }
        
        //this method calculate gain with aux of CalculateShapeOnBuyingFormat()
        public abstract void CalculateGain();
        protected abstract Makeready CalculateShapeOnBuyingFormat();

    }

}