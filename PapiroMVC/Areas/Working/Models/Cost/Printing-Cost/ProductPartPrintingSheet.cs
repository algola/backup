using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public class ProductPartPrintingSheet
    {
        public enum ProductPartPrintingType : int
        {
            ProductPartSingleSheetPrinting = 0,
            ProductPartCoverSheetPrinting = 1,
            ProductPartBookSheetPrinting = 2,
        }

        public ProductPartPrintingType TypeOfProductPartPrinting
        {
            get;
            set;
        }

        /// <summary>
        /// this property accepts in input o provides in output the printing format
        /// over compute 
        /// </summary>
        public String PrintingFormat { get; set; }

        /// <summary>
        /// this method c
        /// </summary>
        public virtual ProductPart Part { get; set; }

        public ProductPartPrintingSheetGain GainPartOnPrinting { get; set; }
        
        public virtual void Update()
        {
            this.GainPartOnPrinting.LargerFormat = PrintingFormat;
            this.GainPartOnPrinting.SmallerFormat = Part.FormatOpened;
            if (this.GainPartOnPrinting.SmallerFormat == "" || this.GainPartOnPrinting.SmallerFormat == null)
            {
                this.GainPartOnPrinting.SmallerFormat = Part.Format;
            }

            }

    }

}