using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public  abstract partial class ProductPartPrintingGain
    {

        public Nullable<double> Pinza { get; set; }
        public Nullable<double> ControPinza { get; set; }
        public Nullable<double> Laterale { get; set; }

             
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------        
        public string SmallerFormat { get; set; }

        //this is mul to SubjectNumber to calculate maxShape
        public int Quantity { get; set; }
        
        //this method calculate gain with aux of CalculateShapeOnBuyingFormat()
        public abstract void CalculateGain();
        protected abstract Makeready CalculateShapeOnBuyingFormat();

    }

}