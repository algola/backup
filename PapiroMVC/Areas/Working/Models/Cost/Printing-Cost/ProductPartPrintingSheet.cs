using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    /// <summary>
    /// Get PrintingFormat and calculating gain on this Format based on specifit type
    /// </summary>
    public partial class ProductPartSheetPrinting : ProductPartPrinting
    {
    

        public override void Update()
        {
             

            //copiato da base
            
             if (GainPartOnPrinting == null)
            {
                GainPartOnPrinting = new ProductPartPrintingSheetGainSingle();
            }



            //Update originale
             var gain = (ProductPartPrintingSheetGain)GainPartOnPrinting;
             TaskExecutor tsk;

             try
             {
                 tsk = this.CostDetail.TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == this.CostDetail.CodTaskExecutorSelected);
                 gain.Pinza = tsk.Pinza;
                 gain.ControPinza = tsk.ControPinza;
                 gain.Laterale = tsk.Laterale;
             }
             catch (Exception)
             {
                 gain.Pinza = 0;
                 gain.ControPinza = 0;
                 gain.Laterale = 0;
             }

             gain.LargerFormat = PrintingFormat;
             gain.SmallerFormat = Part.FormatOpened;

             if (gain.SmallerFormat == "" || gain.SmallerFormat == null)
             {
                 gain.SmallerFormat = Part.Format;
             }

             //posso spostare questa assegnazione quando modifico la quantità del
             //DocumentProduct
             gain.Quantity = this.CostDetail.TaskCost.DocumentProduct.Quantity ?? 0;
             gain.DCut = (Part.IsDCut ?? false) ? Part.DCut : 0;

             gain.DCut1 = (Part.IsDCut ?? false) ? Part.DCut1 : 0;
             gain.DCut2 = (Part.IsDCut ?? false) ? Part.DCut2 : 0;

             GainPartOnPrinting = gain;


            //fine Update originale


            base.Update();

            switch (Part.TypeOfProductPart)
            {
                case ProductPart.ProductPartType.ProductPartSingleSheet:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSingleSheet)Part).SubjectNumber ?? 1;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain2 = MaxGain2;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain1 = MaxGain1;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).ForceSideOnSide = ForceSide;

                    break;
                case ProductPart.ProductPartType.ProductPartSoft:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSoft)Part).SubjectNumber ?? 1;
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
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartRigid)Part).SubjectNumber ?? 1;
                    break;
                case ProductPart.ProductPartType.ProductPartSingleLabelRoll:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartSingleLabelRoll)Part).SubjectNumber ?? 1;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).LateralMinDCut = LateralMinDCut;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).AutoDCut = AutoCutParameter;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain2 = MaxGain2;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain1 = MaxGain1;

                    //PREDISPOSIZIONE PER CAMBIARE NELLA MESSA IN MACCHINA IL TIPO DI CALCOLO SU TYPEOFDCUT1
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).TypeOfDCut1 = Part.TypeOfDCut1 ?? 0;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).GiraVerso = true;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).ForceSideOnSide = 1;

                    break;
                case ProductPart.ProductPartType.ProductPartDoubleLabelRoll:
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).TypeOfDCut2 = 1;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).SubjectNumber = ((ProductPartDoubleLabelRoll)Part).SubjectNumber ?? 1;

                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).AutoDCut = AutoCutParameter;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain2 = MaxGain2;
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).MaxGain1 = MaxGain1;

                    //PREDISPOSIZIONE PER CAMBIARE NELLA MESSA IN MACCHINA IL TIPO DI CALCOLO SU TYPEOFDCUT1
                    ((ProductPartPrintingSheetGainSingle)this.GainPartOnPrinting).TypeOfDCut1 = Part.TypeOfDCut1??0;
                    
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