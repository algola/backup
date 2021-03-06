﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ProductPartBookSheetPrinting : ProductPartSheetPrinting
    {
        enum EnumSignature : int
        {
            s4 = 4,
            s8 = 8,
            s12 = 12,
            s16 = 16,
            s24 = 24,
            s32 = 32
        }

        public ProductPartBookSheetPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartBookSheetPrinting;
        }

        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingSheetGainBook();
            }

            base.Update();

            ((ProductPartPrintingSheetGainBook)this.GainPartOnPrinting).PageToPrint = ((ProductPartBookSheet)Part).Pages ?? 4;

            ((ProductPartPrintingSheetGainBook)this.GainPartOnPrinting).UsePerfecting = false;
            this.GainPartOnPrinting.CalculateGain();

        }

        public override double CalculatedMq
        {
            get
            {
                return base.CalculatedMq * (((ProductPartBookSheet)Part).Pages?? 4)/4;
            }
        }

    }
}