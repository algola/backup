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
            ProductPartSingleRollPrinting = 3,
            ProductPartRigidPrinting = 5,

            ProductPartSingleLabelRollPrinting = 6
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

        public virtual double CalculatedStarts
        {
            get
            {
                return GainPartOnPrinting.Makereadies.Count;
            }
        }

        public virtual double CalculatedGain
        {
            get
            {
                return (double)GainPartOnPrinting.Makereadies.Average(x => x.CalculatedGain ?? 1);
            }
        }

        public virtual double CalculatedMq
        {
            get
            {
                string format = Part.FormatOpened;
                if (format == null || format == "")
                {
                    format = Part.Format;
                }

                return format.GetSide1() * format.GetSide2() / 10000 * (Part.SubjectNumber ?? 1);
            }
        }

        /// <summary>
        /// Ml in stampa del formato di stampa
        /// </summary>
        public virtual double CalculatedMl
        {
            get
            {
                return PrintingFormat.GetSide2() / 100;
            }
        }

        public virtual double CalculatedMqPrintingFormat
        {
            get
            {
                string format = PrintingFormat;
                return format.GetSide1() * format.GetSide2() / 10000;
            }
        }

        public virtual int CalculatedSide1Gain
        {
            get
            {
                return GainPartOnPrinting.Makereadies.FirstOrDefault().ShapeOnSide1 ?? 0;
            }
        }

        public virtual int CalculatedSide2Gain
        {
            get
            {
                return GainPartOnPrinting.Makereadies.FirstOrDefault().ShapeOnSide2 ?? 0;
            }
        }

        public virtual double CalculatedDCut1
        {
            get
            {
                return GainPartOnPrinting.DCut1 ?? 0;
            }
        }

        public virtual double CalculatedDCut2
        {
            get
            {
                return GainPartOnPrinting.DCut2 ?? 0;
            }
        }


    }
}