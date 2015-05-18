using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace PapiroMVC.Models
{

    public partial class PrintingFlatRollCostDetail : PrintingZRollCostDetail
    {

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintingFlatRollCostDetail to2 = (PrintingFlatRollCostDetail)to;

            to2.BuyingFormat = this.BuyingFormat;
            to = to2;

        }

        public PrintingFlatRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingFlatRollCostDetail;
        }


        //dovrei sapere all'interno della

        public override void FuzzyAlgo()
        {
            List<String> newBuyingFormats = new List<string>();
            var pHint = new List<PrintingHint>();

            var ppP = new ProductPartSingleSheetPrinting();

            //Scorro i Dies e se rientrano nelle tolleranze!!!!
            var dies = new List<Die>();

            ppP.CostDetail = this;
            ppP.Part = this.ProductPart;

            foreach (var die in Dies.Where(x => x.FormatType == ProductPart.FormatType))
            {
                var side1 = ppP.Part.Format.GetSide1();
                var side2 = ppP.Part.Format.GetSide2();

                if (Math.Abs(die.Format.GetSide1() - side1) <= DieTollerance &&
                    Math.Abs(die.Format.GetSide2() - side2) <= DieTollerance)
                {

                    ppP.PrintingFormat = die.PrintingFormat;
                    ppP.MaxGain1 = die.MaxGain1 ?? 0;
                    ppP.MaxGain2 = die.MaxGain2 ?? 0;
                    ppP.Update();

                    //voglio aggiungere le fustelle valide!!!!
                    var pHDie = new PrintingHint
                    {
                        Format = die.Format,
                        //DCut1 = ppP.CalculatedDCut1,
                        DCut1 = die.DCut1,
                        DCut2 = die.DCut2,
                        BuyingFormat = die.PrintingFormat,

                        PrintingFormat = die.PrintingFormat,
                        Description = "h" + die.PrintingFormat.GetSide1() + " z" + (die.GetZFromCm(die.PrintingFormat.GetSide2())).ToString() + "*",
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = die.MaxGain1 ?? 0,
                        GainOnSide2 = die.MaxGain2 ?? 0,

                        DeltaDCut2 = 0
                    };

                    pHint.Add(pHDie);

                }
            }

            var labelSide2 = this.ProductPart.Format.GetSide2();
            var labelCut2 = this.ProductPart.DCut2 ?? 0;

            //calculate max Gain on Side2 based on TaskExecutor MaxFormat
            var steps = Math.Floor(TaskexEcutorSelected.FormatMax.GetSide2() / labelSide2 + labelCut2);

            foreach (var width in BuyingWidths)
            {
                for (int step = 1; step <= steps; step++)
                {
                    ppP.AutoCutParameter = false;
                    var buyingFormat = width + "x" + step* (labelCut2+labelSide2);


                    if (SheetCut.IsValid(TaskexEcutorSelected.FormatMax, TaskexEcutorSelected.FormatMin, buyingFormat))
                    {

                        ppP.CostDetail = this;

                        ppP.Part = this.ProductPart;
                        ppP.PrintingFormat = buyingFormat;
                        ppP.AutoCutParameter = true;
                        ppP.MaxGain1 = 0;
                        ppP.MaxGain2 = 0;
                        ppP.Update();
                        haveAlmostOne = (ppP.CalculatedGain > 0) || haveAlmostOne;

                        int maxGain1 = 0;

                        for (int i = 0; i < 10 && (ppP.CalculatedSide2Gain - i) > 0; i++)
                        {

                            ppP.MaxGain2 = ppP.CalculatedSide2Gain - i;
                            ppP.Update();

                            for (int k = 0; k < 6 && (ppP.CalculatedSide1Gain - k) > 0; k++)
                            {
                                //provo a togliere una posa in banda se l'interspazio di passo è > di quello della banda
                                if (ppP.CalculatedDCut1 < ppP.CalculatedDCut2)
                                {
                                    //tolgo una posa in banda
                                    maxGain1 = ppP.CalculatedSide1Gain - k;
                                    ppP.MaxGain1 = maxGain1;
                                    ppP.Update();
                                }

                                //mi calcolo l'interspazio più piccolo... mi servirà se non ho formati validi per ricavarne almeno uno
                                if (ppP.CalculatedGain > 0)
                                {
                                    smallerCalculatedDCut = ppP.CalculatedDCut2 < smallerCalculatedDCut ? ppP.CalculatedDCut2 : smallerCalculatedDCut;
                                }


                                var pH =
                                    new PrintingHint
                                {
                                    Format = ppP.Part.Format,
                                    DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                    DCut2 = ppP.CalculatedDCut2,
                                    BuyingFormat = buyingFormat,
                                    PrintingFormat = buyingFormat,
                                    MaxGain2 = ppP.MaxGain2,
                                    MaxGain1 = ppP.MaxGain1,
                                    //                            Description = "h" + buyingFormat.GetSide1() + " z" + (buyingFormat.GetSide2() / 2.54 * 8).ToString(),
                                    CalculatedGain = ppP.CalculatedGain,
                                    GainOnSide1 = ppP.CalculatedSide1Gain,
                                    GainOnSide2 = ppP.CalculatedSide2Gain,
                                    DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - ppP.CalculatedDCut2)
                                };

                                pH.Description = buyingFormat;

                                if (!pHint.Contains(pH, new PrintingHintComparer()))
                                {
                                    //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                                    pHint.Add(pH);
                                }


                                if (ppP.CalculatedSide2Gain >= 1 && ppP.MaxGain2 == 0)
                                {

                                    //provo ad aggiungere una nuova resa con interspazio negativo
                                    var lessZero = Math.Abs((ppP.Part.Format.GetSide2() - (ppP.CalculatedDCut2 * ppP.CalculatedSide2Gain))) / (ppP.CalculatedSide2Gain + 1);
                                    lessZero = Math.Truncate(lessZero * 1000 + 1) / 1000;
                                    smallerCalculatedDCutLessZero = lessZero < smallerCalculatedDCutLessZero ? lessZero : smallerCalculatedDCutLessZero;

                                    pH =
                                        new PrintingHint
                                        {
                                            Format = ppP.Part.Format,
                                            DCut1 = 0, // DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                            DCut2 = lessZero * (-1),
                                            BuyingFormat = buyingFormat,
                                            PrintingFormat = buyingFormat,
                                            MaxGain2 = ppP.MaxGain2 + 1,
                                            CalculatedGain = ppP.CalculatedGain + ppP.CalculatedSide1Gain,
                                            GainOnSide1 = ppP.CalculatedSide1Gain,
                                            GainOnSide2 = ppP.CalculatedSide2Gain + 1,
                                            DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - lessZero * (-1))
                                        };

                                    pH.Description = buyingFormat;


                                    if (!pHint.Contains(pH, new PrintingHintComparer()))
                                    {
                                        //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                                        pHint.Add(pH);
                                    }

                                    //ripristino l'autocut
                                    ppP.AutoCutParameter = true;

                                }

                            }
                        }

                    }//End if
                }
            }

            ppP.MaxGain2 = 0;

            if (PrintingFormat != "" && PrintingFormat != null)
            {
                var pH = new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ProductPartPrinting.CalculatedDCut1,
                        DCut2 = ProductPartPrinting.CalculatedDCut2,
                        BuyingFormat = PrintingFormat,
                        PrintingFormat = PrintingFormat,
                        MaxGain2 = ProductPartPrinting.MaxGain2,
                        MaxGain1 = ProductPartPrinting.MaxGain1,
                        //   Description = "h" + PrintingFormat.GetSide1() + " z" + (PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ProductPartPrinting.CalculatedGain,
                        GainOnSide1 = ProductPartPrinting.CalculatedSide1Gain,
                        GainOnSide2 = ProductPartPrinting.CalculatedSide2Gain,
                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - ProductPartPrinting.CalculatedDCut2)
                    };

                pH.Description = PrintingFormat;

                // se non ho trovato hint e ho almeno un PrintigFormat allora
                //inserisco il printing format negli hint
                if (!pHint.Contains(pH, new PrintingHintComparer()))
                {
                    //ppP.PrintingFormat = PrintingFormat;
                    //ppP.Update();
                    pHint.Add(pH);
                }

            }
            //qui inizio a togliere un po di pHint            
            var pHint1 = pHint.Where(x => x.DCut2 >= ProductPart.MinDCut
                && x.DCut2 <= ProductPart.MaxDCut && (x.DCut1 >= x.DCut2 || (x.DCut1 == 0 && x.MaxGain1 == 1))).ToList();

            if (pHint1.Count <= 1)
            {
                //fascette gommate
                if (ProductPart.MinDCut == 0)
                {
                    var pHint2 = pHint.Where(x => x.DCut2 == smallerCalculatedDCutLessZero * -1);
                    pHint1 = pHint.Where(x => x.DCut2 >= 0 && x.DCut2 <= smallerCalculatedDCut && (x.DCut1 >= x.DCut2 || x.DCut1 == 0)).ToList();
                    var pHint3 = pHint1.Union(pHint2);
                    pHint1 = pHint3.ToList();
                }
                else
                {
                    var smaller = pHint.Where(x => x.DCut1 >= x.DCut2 || x.DCut1 == 0).Select(x => x.DeltaDCut2).Min();
                    var pHintLast1 = pHint.Where(x => x.DeltaDCut2 == smaller && (x.DCut1 >= x.DCut2 || x.DCut1 == 0));

                    try
                    {
                        var smaller2 = pHint.Where(x => (x.DCut1 >= x.DCut2 || x.DCut1 == 0) && x.DeltaDCut2 != smaller).Select(x => x.DeltaDCut2).Min();
                        var pHintLast2 = pHint.Where(x => x.DeltaDCut2 == smaller2 && x.DeltaDCut2 != smaller && (x.DCut1 >= x.DCut2 || x.DCut1 == 0));


                        pHint1 = pHintLast1.Union(pHintLast2).ToList();

                    }
                    catch (Exception)
                    {
                        pHint1 = pHintLast1.ToList();

                    }
                    //                    pHint1 = pHint.Where(x => x.DCut2 >= 0 && x.DCut2 <= smallerCalculatedDCut && (x.DCut1 >= x.DCut2 || x.DCut1 == 0)).ToList();
                }
            }

            pHint = pHint1.ToList();

            List<PrintingHint> widthValids = new List<PrintingHint>();// List<double>();

            //scorro la lista ed estraggo solo gli z validi
            foreach (var item in pHint)
            {
                var z = item.BuyingFormat.GetSide2();
                if (!widthValids.Select(x => x.BuyingFormat.GetSide2()).Contains(z))
                {
                    widthValids.Add(item);
                }
            }

            ppP.Part = (ProductPart)ppP.Part.Clone();

            //bande di carta calcolate semplicemente con la resa, dati gli z validi e il maxWidth della macchina Flexo
            foreach (var item in widthValids)
            {
                double dCut1 = 0;
                double dCut2 = 0;
                var newBands = this.GetOptimizedWidths(item.PrintingFormat.GetSide2(), item.MaxGain2, ppP.Part.Format, ref dCut1, ref  dCut2, 34, ProductPart.TypeOfDCut1 ?? 0);
                foreach (var nB in newBands)
                {

                    ppP.PrintingFormat = nB.ToString() + "x" + item.PrintingFormat.GetSide2();  // item.Z.ToString();
                    ppP.MaxGain1 = 0;
                    ppP.MaxGain2 = 0;
                    ppP.AutoCutParameter = false;
                    ppP.Part.DCut2 = dCut2;
                    ppP.Part.DCut1 = dCut1;
                    ppP.Update();

                    int i = 1;

                    //non vale per le fascette gommate
                    if (ProductPart.MinDCut != 0)
                    {
                        while (ppP.CalculatedDCut1 < ppP.CalculatedDCut2 && (ppP.CalculatedSide1Gain - i) > 0)
                        {
                            var maxGain1 = ppP.CalculatedSide1Gain - i;
                            ppP.MaxGain1 = maxGain1;
                            ppP.Update();
                        }
                    }
                    var pH = new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                        DCut2 = item.DCut2 ?? ppP.CalculatedDCut2,
                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat,
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain,
                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - (item.DCut2 ?? ppP.CalculatedDCut2))

                    };

                    pH.Description = ppP.PrintingFormat; //"h" + ppP.PrintingFormat.GetSide1() + " z" + (ppP.PrintingFormat.GetSide2() / 2.54 * 8).ToString(),


                    if (!pHint.Contains(pH, new PrintingHintComparer()))
                    {
                        pHint.Add(pH);
                    }
                }
            }


            PrintingHints = pHint;
            BuyingFormats = pHint.Select(x => x.BuyingFormat).ToList();

            if (BuyingFormats.Count == 0)
            {
                //no format
                Error = 3;
            }
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
  

        public override void UpdateCoeff()
        {
            RollChanges = 0;

            //calcolo di quanti impianti sono necessari!!!!
            Implants = TaskexEcutorSelected.GetImplants(TaskCost.ProductPartTask.CodOptionTypeOfTask);

        }

        public override double Quantity(double qta)
        {

            CalculatedMl = 0;
            CalculatedKg = 0;
            CalculatedRun = 0;

            return 1;
        }

        public override double UnitCost(double qta)
        {
            return 0;
        }

        public override void MergeField(Novacode.DocX doc)
        {
            base.MergeField(doc);
            if (ProductPartPrinting != null)
            {
                TaskexEcutorSelected = TaskExecutors.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                ProductPartPrinting.MergeField(doc);
            }
        }
    }


}