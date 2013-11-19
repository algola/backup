using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public partial class ProductPartPrinting
    {
        public enum ProductPartPrintingType : int
        {
            ProductPartSingleSheetPrinting = 0,
            ProductPartCoverSheetPrinting = 1,
            ProductPartBookSheetPrinting = 2,
            ProductPartSinglePlotterPrinting = 3,
            ProductPartMultiplePlotterPrinting = 4
        }

        public ProductPartPrintingType TypeOfProductPartPrinting
        {
            get;
            set;
        }

    //    public virtual ProductPartPrintingGain GainPartOnPrinting { get; set; }

        public virtual void Update()
        {

        }

        public ProductPartPrintingGain GainPartOnPrinting 
        {
            get
            {
                return GainPartOnPrintings.FirstOrDefault();
            }
            set
            {
                GainPartOnPrintings.Clear();
                GainPartOnPrintings.Add(value);
            }
        }

    }













}