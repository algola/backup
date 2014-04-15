using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    //each type has a specific view with particular property
    //ie: single sheet has a max gain and so other
    public partial class ProductPartSingleSheetPrinting : ProductPartSheetPrinting
    {
        public ProductPartSingleSheetPrinting()
        {
            TypeOfProductPartPrinting = ProductPartPrintingType.ProductPartSingleSheetPrinting;
        }


        public override void Update()
        {
            if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingSheetGainSingle();
            }

            base.Update();

            switch (Part.TypeOfProductPart)
            {
                case ProductPart.ProductPartType.ProductPartSingleSheet:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSingleSheet)Part).SubjectNumber ?? 1;
                    break;
                case ProductPart.ProductPartType.ProductPartCoverSheet:
                    break;
                case ProductPart.ProductPartType.ProductPartBookSheet:
                    break;
                case ProductPart.ProductPartType.ProductPartBlockSheet:
                    break;
                case ProductPart.ProductPartType.ProductPartSinglePlotter:
                    break;
                case ProductPart.ProductPartType.ProductPartRigid:
                    break;
                case ProductPart.ProductPartType.ProductPartSingleLabelRoll:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSingleLabelRoll)Part).SubjectNumber ?? 1;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).AutoDCut = AutoCutParameter;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).GiraVerso = true;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).ForceSideOnSide = 1;

                    break;
                default:
                    break;
            }

            ((ProductPartPrintingSheetGainSingle)GainPartOnPrinting).UsePerfecting = false;
            GainPartOnPrinting.CalculateGain();

            //if (((ProductPartPrintingSheetGainSingle)GainPartOnPrinting).AutoDCut)
            //{
            //    this.Part.DCut1 = GainPartOnPrinting.DCut1;
            //    this.Part.DCut2 = GainPartOnPrinting.DCut2;                
            //}

        }
    }

}