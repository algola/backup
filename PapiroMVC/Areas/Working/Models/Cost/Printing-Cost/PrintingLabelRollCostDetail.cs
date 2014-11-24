﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{

    public class ZVaild
    {
        public double Z { get; set; }
        public int MaxGain2 { get; set; }
        public int MaxGain1 { get; set; }

        public Nullable<double> DCut2 { get; set; }
        public Nullable<double> DCut1 { get; set; }

    }

    public partial class PrintingLabelRollCostDetail : PrintingCostDetail
    {


        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintingLabelRollCostDetail to2 = (PrintingLabelRollCostDetail)to;

            to2.BuyingFormat = this.BuyingFormat;

            to = to2;

        }


        public List<Cut> Cuts
        {
            get
            {
                List<Cut> y;

                //task executor corrente
                var tsk = TaskExecutors.Where(it => it.CodTaskExecutor == CodTaskExecutorSelected).FirstOrDefault();

                //se il formato da stampare è < del formato aperto... 
                if (SheetCut.IsValid(tsk.FormatMax, ProductPart.FormatOpened, tsk.FormatMin))
                {
                    //i tagli che vanno bene nel formato minimo e massimo
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, tsk.FormatMin);
                }
                else
                {
                    //i tagli che vanno bene nel formato massimo e formato lavoro
                    y = SheetCut.Cuts(BuyingFormat, tsk.FormatMax, ProductPart.FormatOpened);
                }

                //ma solo quelli validi
                List<Cut> x = new List<Cut>();
                x = y.Where(k => k.Valid).ToList();

                if (PrintingFormat != null && PrintingFormat != "")
                {
                    var k = 0;
                    for (int i = 0; i < x.Count &&
                        !SheetCut.IsValid(tsk.FormatMax, tsk.FormatMin, PrintingFormat);
                        i++,
                        PrintingFormat = x[k].GetCuttedFormat(BuyingFormat)) ;
                }

                foreach (var item in x)
                {
                    item.CutName = item.GetCuttedFormat(BuyingFormat);
                }

                //Controllo del formato se è presente nell'elenco dei formati
                //Se non è presente lo aggiungo

                //da controllare solo se l'elenco non è vuoto    
                if (x.Count > 0)
                {
                    var ele = x.Find(z => z.CutName == PrintingFormat);
                    if (ele == null && PrintingFormat != null)
                    {
                        var toAdd = new Cut("manual", 0, 0);
                        toAdd.ManualFormat = PrintingFormat;
                        toAdd.CutName = toAdd.GetCuttedFormat(BuyingFormat);
                        x.Add(toAdd);
                    }
                }

                return x;
            }
        }

        public PrintingLabelRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingLabelRollCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>
        public List<String> BuyingFormats { get; set; }


        //il tipo è ingannevole... in realtà serve per proporre un'associazione tra nome e formato immediata
        public List<ProductFormatName> BuyingFormatsName
        {
            get
            {
                var lst = new List<ProductFormatName>();
                var curTsk = this.TaskExecutors.SingleOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                if (curTsk.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
                {
                    foreach (var bF in BuyingFormats)
                    {
                        lst.Add(new ProductFormatName { FormatName = "h" + bF.GetSide1() + " z" + (bF.GetSide2() / 2.54 * 8).ToString(), CodFormat = bF });
                    }
                }
                else
                {
                    foreach (var bF in BuyingFormats)
                    {
                        lst.Add(new ProductFormatName { CodFormat = bF, FormatName = bF });
                    }
                }

                return lst;
            }

        }

        //every changes fire this update
        public override void Update()
        {

            if (ProductPartPrinting == null)
            {
                switch (ProductPart.TypeOfProductPart)
                {
                    case ProductPart.ProductPartType.ProductPartSingleLabelRoll:
                        this.ProductPartPrinting = new ProductPartSingleSheetPrinting();

                        break;
                    default:
                        throw new Exception();
                        break;
                }
            }

            this.ProductPartPrinting.CostDetail = this;

            if (GainPrintingOnBuying == null)
            {
                GainPrintingOnBuying = new ProductPartPrintingSheetGainSingle();
            }

            //la stampa è condizionata al fatto che il PrintingFormat stia in macchina!!!
            //quindi è da scegliere tra i possibili formati "spaccati" dal formato acquisto
            //la lettura della proprietà Cuts ricalcola il PrintingFormat!!!!
            var fake = this.Cuts;

            //i formati di stampa devono essere condizionati anche dal formato del lavoro da stampare
            //dalla pinza e dalla contropinza e laterale!!!!

            if (BuyingFormats.Count == 0)
            {
                Error = 3;
            }
            else
            {
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).LargerFormat = this.BuyingFormat;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SmallerFormat = this.PrintingFormat;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).Quantity = 1;
                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).SubjectNumber = 1;

                ((ProductPartPrintingSheetGainSingle)GainPrintingOnBuying).CalculateGain();

                //devo anche rifare la messa in macchina della parte!!!
                if (this.ProductPartPrinting != null)
                {
                    this.ProductPartPrinting.Part = this.ProductPart;
                    this.ProductPartPrinting.PrintingFormat = this.PrintingFormat;

                    this.ProductPartPrinting.Update();
                }

                this.UpdateCoeff();

            }


        }

        //public override void Update(IQueryable<CostDetail> costDetailList)
        //{
        //    base.Update(costDetailList);

        //    var prePosts = costDetailList.OfType<PrePostPressCostDetail>();
        //    foreach (var item in prePosts)
        //    {
        //        item.Update();
        //    }
        //}


        public override int GainOnSide1
        {
            get
            {
                return ProductPartPrinting.CalculatedSide1Gain;
            }
            set
            {
                base.GainOnSide1 = value;
            }
        }

        public override int GainOnSide2
        {
            get
            {
                return ProductPartPrinting.CalculatedSide2Gain;
            }
            set
            {
                base.GainOnSide2 = value;
            }
        }

        public List<PrintingHint> PrintingHints
        {
            get;
            set;
        }


        public double DCut1OnDCut2AndPart(double dcut1, double dcut2, ProductPart p)
        {
            //calculate dcu1 base on dcut2 and productpart
            switch (p.TypeOfDCut1)
            {
                case 2:
                    return 0;
                case 1:
                    return dcut2;
                default:
                    return dcut1;

            }
        }

        public void FuzzyAlgo()
        {
            List<String> newBuyingFormats = new List<string>();
            var pHint = new List<PrintingHint>();

            //se esiste almeno una resa e non ho formati validi... provo ad aumentare il range dell'interspazio
            bool haveAlmostOne = false;
            //voglio sapere anche qual'è interspazio minore calcolato
            double smallerCalculatedDCut = 100;
            //interspazio negativo
            double smallerCalculatedDCutLessZero = 100;

            var ppP = new ProductPartSingleSheetPrinting();

            foreach (var buyingFormat in BuyingFormats)
            {


                if (buyingFormat.GetSide2() == 40.005)
                {
                    Console.Write("ciao");
                }

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
                        //provo a togliere una posa in banda se l'interspazio di banda è > di quello del passo
                        if (ppP.CalculatedDCut1 < ppP.CalculatedDCut2)
                        {
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
                            Description = "h" + buyingFormat.GetSide1() + " z" + (buyingFormat.GetSide2() / 2.54 * 8).ToString(),
                            CalculatedGain = ppP.CalculatedGain,
                            GainOnSide1 = ppP.CalculatedSide1Gain,
                            GainOnSide2 = ppP.CalculatedSide2Gain,
                            DeltaDCut2 = Math.Abs((ProductPart.MaxDCut??0) - ppP.CalculatedDCut2)

                        };

                        if (!pHint.Contains(pH, new PrintingHintComparer()))
                        {
                            //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                            pHint.Add(pH);
                        }


                        if (ppP.CalculatedSide2Gain >= 1 && ppP.MaxGain2==0)
                        {

                            if (buyingFormat.GetSide2() == 40.005)
                            {
                                Console.WriteLine("");
                            }

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
                                    Description = "h" + buyingFormat.GetSide1() + " z" + (buyingFormat.GetSide2() / 2.54 * 8).ToString(),
                                    CalculatedGain = ppP.CalculatedGain + ppP.CalculatedSide1Gain,
                                    GainOnSide1 = ppP.CalculatedSide1Gain,
                                    GainOnSide2 = ppP.CalculatedSide2Gain + 1,
                                    DeltaDCut2 = Math.Abs((ProductPart.MaxDCut??0) - lessZero * (-1))
                                };

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
                        Description = "h" + PrintingFormat.GetSide1() + " z" + (PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ProductPartPrinting.CalculatedGain,
                        GainOnSide1 = ProductPartPrinting.CalculatedSide1Gain,
                        GainOnSide2 = ProductPartPrinting.CalculatedSide2Gain,
                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut??0) - ProductPartPrinting.CalculatedDCut2)

                    };


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
            var pHint1 = pHint.Where(x => x.DCut2 >= ProductPart.MinDCut && x.DCut2 <= ProductPart.MaxDCut && (x.DCut1 >= x.DCut2 || x.DCut1 == 0)).ToList();

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
                    var smaller = pHint.Where(x=>x.DCut1 >= x.DCut2 || x.DCut1 == 0).Select(x => x.DeltaDCut2).Min();
                    var pHintLast1 = pHint.Where(x => x.DeltaDCut2 == smaller && (x.DCut1 >= x.DCut2 || x.DCut1 == 0));

                    var smaller2 = pHint.Where(x => (x.DCut1 >= x.DCut2 || x.DCut1 == 0) && x.DeltaDCut2 != smaller).Select(x => x.DeltaDCut2).Min();
                    var pHintLast2 = pHint.Where(x => x.DeltaDCut2 == smaller2 && x.DeltaDCut2 != smaller && (x.DCut1 >= x.DCut2 || x.DCut1 == 0));


                    pHint1 = pHintLast1.Union(pHintLast2).ToList();
//                    pHint1 = pHint.Where(x => x.DCut2 >= 0 && x.DCut2 <= smallerCalculatedDCut && (x.DCut1 >= x.DCut2 || x.DCut1 == 0)).ToList();
                }
            }


            pHint = pHint1.ToList();

            //            List<string> aux = pHint.Select(z => z.BuyingFormat).ToList();

            List<PrintingHint> zValids = new List<PrintingHint>();// List<double>();

            //scorro la lista ed estraggo solo gli z validi
            foreach (var item in pHint)
            {
                var z = item.BuyingFormat.GetSide2();
                if (!zValids.Select(x => x.BuyingFormat.GetSide2()).Contains(z))
                {
                    zValids.Add(item);
                }
            }



            ppP.Part = (ProductPart)ppP.Part.Clone();

            //bande di carta calcolate semplicemente con la resa, dati gli z validi e il maxWidth della macchina Flexo
            foreach (var item in zValids)
            {
                double dCut1 = 0;
                double dCut2 = 0;
                var newBands = this.GetOptimalWidthFlexo(item.PrintingFormat.GetSide2(), item.MaxGain2, ppP.Part.Format, ref dCut1,ref  dCut2, 34, ProductPart.TypeOfDCut1 ?? 0);
                foreach (var nB in newBands)
                {

                    ppP.PrintingFormat = nB.ToString() + "x" + item.PrintingFormat.GetSide2();  // item.Z.ToString();
                    ppP.MaxGain1 = 0;
                    ppP.MaxGain2 = 0;
                    ppP.AutoCutParameter = false;
                    //ppP.Part.DCut2 = item.DCut2;
                    //ppP.Part.DCut1 = item.DCut1;
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
                        //DCut1 = ppP.CalculatedDCut1,
                        DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                        DCut2 = item.DCut2?? ppP.CalculatedDCut2,
                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat,
                        Description = "h" + ppP.PrintingFormat.GetSide1() + " z" + (ppP.PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain,
                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut??0) - (item.DCut2?? ppP.CalculatedDCut2))

                    };

                    pHint.Add(pH);
                }
            }

            Console.WriteLine(BuyingFormat);

            PrintingHints = pHint;
            BuyingFormats = pHint.Select(x => x.BuyingFormat).ToList();

            if (BuyingFormats.Count == 0)
            {
                //no format
                Error = 3;
            }
        }


        public void FuzzyAlgo_()
        {
            List<String> newBuyingFormats = new List<string>();
            var pHint = new List<PrintingHint>();

            //se esiste almeno una resa e non ho formati validi... provo ad aumentare il range dell'interspazio
            bool haveAlmostOne = false;
            //voglio sapere anche qual'è interspazio minore calcolato
            double smallerCalculatedDCut = 100;

            var ppP = new ProductPartSingleSheetPrinting();
            var maxStep = 2;
            var step = 0;

            do
            {
                step++;

                foreach (var buyingFormat in BuyingFormats)
                {
                    ppP.CostDetail = this;

                    ppP.Part = this.ProductPart;
                    ppP.PrintingFormat = buyingFormat;
                    ppP.AutoCutParameter = true;

                    ppP.Update();
                    haveAlmostOne = (ppP.CalculatedGain > 0) || haveAlmostOne;

                    int maxGain1 = 0;

                    //provo a togliere una posa in banda se l'interspazio di banda è > di quello del passo
                    if (ppP.CalculatedDCut1 < ppP.CalculatedDCut2 && ppP.CalculatedSide2Gain > 1)
                    {
                        maxGain1 = ppP.CalculatedSide1Gain - 1;
                        ppP.MaxGain1 = maxGain1;
                        ppP.Update();
                    }

                    //mi calcolo l'interspazio più piccolo... mi servirà se non ho formati validi per ricavarne almeno uno
                    if (ppP.CalculatedGain > 0)
                    {
                        smallerCalculatedDCut = ppP.CalculatedDCut2 < smallerCalculatedDCut ? ppP.CalculatedDCut2 : smallerCalculatedDCut;
                    }

                    //this kind of cost has limit in Interspace
                    if (ppP.CalculatedDCut2 >= ppP.Part.MinDCut && ppP.CalculatedDCut2 <= ppP.Part.MaxDCut &&
                        ((ppP.CalculatedDCut1 >= ppP.CalculatedDCut2) || ppP.CalculatedSide1Gain == 1))
                    {
                        //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                        pHint.Add(new PrintingHint
                        {
                            Format = ppP.Part.Format,
                            DCut1 = ppP.CalculatedDCut1,
                            DCut2 = ppP.CalculatedDCut2,
                            BuyingFormat = buyingFormat,
                            PrintingFormat = buyingFormat,
                            Description = "h" + buyingFormat.GetSide1() + " z" + (buyingFormat.GetSide2() / 2.54 * 8).ToString(),
                            CalculatedGain = ppP.CalculatedGain,
                            GainOnSide1 = ppP.CalculatedSide1Gain,
                            GainOnSide2 = ppP.CalculatedSide2Gain

                        });
                    }
                    else
                    {
                        //tolgo una posa!!!
                        ppP.MaxGain2 = ppP.CalculatedSide2Gain - 1;
                        ppP.Update();

                        haveAlmostOne = (ppP.CalculatedGain > 0) || haveAlmostOne;

                        //mi calcolo l'interspazio più piccolo... mi servirà se non ho formati validi per ricavarne almeno uno
                        if (ppP.CalculatedGain > 0)
                        {
                            smallerCalculatedDCut = ppP.CalculatedDCut2 < smallerCalculatedDCut ? ppP.CalculatedDCut2 : smallerCalculatedDCut;
                        }

                        if (ppP.CalculatedDCut2 >= ppP.Part.MinDCut && ppP.CalculatedDCut2 <= ppP.Part.MaxDCut &&
                        ((ppP.CalculatedDCut1 >= ppP.CalculatedDCut2) || ppP.CalculatedSide1Gain == 1))
                        {
                            //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                            pHint.Add(new PrintingHint
                            {
                                Format = ppP.Part.Format,
                                DCut1 = ppP.CalculatedDCut1,
                                DCut2 = ppP.CalculatedDCut2,
                                BuyingFormat = buyingFormat,
                                PrintingFormat = buyingFormat,
                                MaxGain2 = ppP.MaxGain2,
                                Description = "h" + buyingFormat.GetSide1() + " z" + (buyingFormat.GetSide2() / 2.54 * 8).ToString(),
                                CalculatedGain = ppP.CalculatedGain,
                                GainOnSide1 = ppP.CalculatedSide1Gain,
                                GainOnSide2 = ppP.CalculatedSide2Gain

                            });
                        }
                        ppP.MaxGain2 = 0;
                    }
                }

                Console.WriteLine(haveAlmostOne);
                if (haveAlmostOne && pHint.Count == 0)
                {
                    //REIMPOSTO I MAX E MIN
                    ppP.Part.MaxDCut = smallerCalculatedDCut;
                }

            } while (haveAlmostOne && pHint.Count == 0 && step < maxStep);

            if (PrintingFormat != "" && PrintingFormat != null)
            {
                // se non ho trovato hint e ho almeno un PrintigFormat allora
                //inserisco il printing format negli hint
                if (pHint.Where(x => x.PrintingFormat == PrintingFormat).Count() == 0)
                {
                    ppP.PrintingFormat = PrintingFormat;
                    ppP.Update();
                    pHint.Add(new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ProductPartPrinting.CalculatedDCut1,
                        DCut2 = ProductPartPrinting.CalculatedDCut2,
                        BuyingFormat = PrintingFormat,
                        PrintingFormat = PrintingFormat,
                        MaxGain2 = ProductPartPrinting.MaxGain2,
                        MaxGain1 = ProductPartPrinting.MaxGain1,
                        Description = "h" + PrintingFormat.GetSide1() + " z" + (PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain

                    });
                }

            }
            //            List<string> aux = pHint.Select(z => z.BuyingFormat).ToList();
            List<ZVaild> zValids = new List<ZVaild>();// List<double>();

            //scorro la lista ed estraggo solo gli z validi
            foreach (var item in pHint)
            {
                var z = item.BuyingFormat.GetSide2();
                if (!zValids.Select(x => x.Z).Contains(z))
                {
                    var zValid = new ZVaild();
                    zValid.Z = z;
                    zValid.MaxGain2 = item.MaxGain2;
                    zValid.DCut2 = item.DCut2;
                    zValids.Add(zValid);
                }
            }

            //bande di carta calcolate semplicemente con la resa, dati gli z validi e il maxWidth della macchina Flexo
            foreach (var item in zValids)
            {
                double dCut1 = 0;
                double dCut2 = 0;
                var newBands = this.GetOptimalWidthFlexo(item.Z, item.MaxGain2, ppP.Part.Format, ref dCut1, ref dCut2, 34);
                foreach (var nB in newBands)
                {
                    ppP.PrintingFormat = nB.ToString() + "x" + item.Z.ToString();
                    ppP.MaxGain2 = item.MaxGain2;
                    ppP.Update();
                    pHint.Add(new PrintingHint
                    {
                        Format = ppP.Part.Format,
                        DCut1 = ppP.CalculatedDCut1,
                        DCut2 = item.DCut2 == null ? ppP.CalculatedDCut2 : item.DCut2,
                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat,
                        Description = "h" + ppP.PrintingFormat.GetSide1() + " z" + (ppP.PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain
                    });
                }
            }

            Console.WriteLine(BuyingFormat);

            PrintingHints = pHint;
            BuyingFormats = pHint.Select(x => x.BuyingFormat).ToList();

            if (BuyingFormats.Count == 0)
            {
                //no format
                Error = 3;
            }
        }

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();


            //calcoli per mt lineari!!!!!!!!
            var paperFirstStartLenght = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
            var paperSecondStartLenght = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;

            var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            //mul with cm GetSide2 Printing Format
            var mtRuns = runs * PrintingFormat.GetSide2() / 100;

            //i 2000 dovrebbero essere rapportati alla carta!!!
            RollChanges = Math.Truncate((mtRuns + paperFirstStartLenght ?? 0) / 2000);

            //calcolo di quanti impianti sono necessari!!!!
            Implants = TaskexEcutorSelected.GetImplants(TaskCost.ProductPartTask.CodOptionTypeOfTask);

            var mtWaste = (paperFirstStartLenght ?? 0) + (RollChanges * paperSecondStartLenght ?? 0);


        }

        public override double Quantity(double qta)
        {

            //mi serve calcolare la quantità con gli scarti!!!


            if (TaskexEcutorSelected == null)
            {
                UpdateCoeff();
            }

            double ret;
            double mqMat = 0;
            double mlMat = 0;
            double runMat = 0;
            double kgMat = 0;


            var paperFirstStartLenght = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
            var paperSecondStartLenght = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;

            var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            var mtRuns = runs * PrintingFormat.GetSide2() / 100;
            var mtWaste = (paperFirstStartLenght ?? 0) + (RollChanges * paperSecondStartLenght ?? 0);

            mlMat = Math.Ceiling(mtRuns + mtWaste);
            CalculatedMl = mlMat;

            mqMat =  Math.Ceiling(base.Quantity(qta));
            CalculatedMq = mqMat;

            kgMat = 0;
            CalculatedKg = kgMat;

            CalculatedRun = 0;

            switch ((QuantityType)(TypeOfQuantity ?? 0))
            {
                case QuantityType.MqWorkTypeOfQuantity:
                    ret = Math.Ceiling(base.Quantity(qta));
                    break;
                case QuantityType.RunLengthMlTypeOfQuantity:
                    //calcoli per mt lineari!!!!!!!!

                    ret = mlMat;
                    break;

                default:
                    ret = base.Quantity(qta);
                    break;
            }

            return ret;
        }

        public override void InitCostDetail(IQueryable<TaskExecutor> tskExec, IQueryable<Article> articles)
        {
            base.InitCostDetail(tskExec, articles);

            String codTypeOfTask = String.Empty;
            //Console.WriteLine(ProductPart); //= TaskCost.ProductPartTask.ProductPart;
            codTypeOfTask = TaskCost.ProductPartTask.OptionTypeOfTask.CodTypeOfTask;
            tskExec = TaskExecutor.FilterByTask(tskExec, codTypeOfTask);
            TaskExecutors = tskExec.ToList();

            ICollection<ProductPartsPrintableArticle> productPartPrintabelArticles = new List<ProductPartsPrintableArticle>();
            productPartPrintabelArticles = TaskCost.ProductPartTask.ProductPart.ProductPartPrintableArticles;

            #region Format
            List<string> formats = new List<string>();
            var widthList = new List<Nullable<double>>();

            //
            //voglio sapere quali sono i formati degli articoli ma gli articoli che posso stampare dipendono dal tipo di macchina!!!!
            foreach (var item in productPartPrintabelArticles)
            {
                widthList = articles.OfType<RollPrintableArticle>()
                           .Where(x => x.TypeOfMaterial == item.TypeOfMaterial &&
                               x.Color == item.Color &&
                               x.Adhesive == item.Adhesive &&
                               x.Weight == item.Weight &&
                               x.NameOfMaterial == item.NameOfMaterial)
                                   .Select(x => x.Width).ToList();
            }


            //**********************************************
            var tskCurrent = tskExec.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
            if (tskCurrent == null)
            {
                tskCurrent = tskExec.FirstOrDefault();
            }

            var zList = new List<string>();

            foreach (var item in tskCurrent.TaskExecutorCylinders)
            {
                zList.Add((Convert.ToDouble(item.Z) / 8 * 2.54).ToString());
            }

            //combino i width con gli Z

            foreach (var width in widthList)
            {
                foreach (var z in zList)
                {
                    var x = (width ?? 0).ToString("0.00", Thread.CurrentThread.CurrentUICulture) + "x" + z;
                    if (!formats.Contains(x))
                        formats.Add((width ?? 0).ToString("0.00", Thread.CurrentThread.CurrentUICulture) + "x" + z);
                }
            }

            BuyingFormats = formats;
            //**********************************************

            #endregion
        }

        public override List<CostDetail> GetRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

            //per ciascun materiale stampabile...
            foreach (var item in this.ProductPart.ProductPartPrintableArticles)
            {

                var xx = articles.GetArticlesByProductPartPrintableArticle(item).FirstOrDefault();

                PrintedArticleCostDetail x;

                switch (xx.TypeOfArticle)
                {
                    case Article.ArticleType.SheetPrintableArticle:
                        x = new PrintedSheetArticleCostDetail();
                        break;
                    case Article.ArticleType.RollPrintableArticle:
                        x = new PrintedRollArticleCostDetail();
                        break;
                    case Article.ArticleType.RigidPrintableArticle:
                        x = new PrintedRigidArticleCostDetail();
                        break;
                    case Article.ArticleType.ObjectPrintableArticle:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                        break;
                }

                x.ComputedBy = this;
                x.ProductPart = this.ProductPart;

                //devo pescare il costo e associarlo al dettaglio
                if (x.CodCost == null)
                {
                    var xxxx = costs.ToList();

                    var cost = costs.Where(pp => pp.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).FirstOrDefault();
                    //da non usare MAIIII                    x.TaskCost = cost;
                    x.CodCost = cost.CodCost;
                    x.CodCostDetail = cost.CodCost;

                    x.CostDetailCostCodeRigen();

                }

                //GUID
                x.Guid = this.Guid;
                this.Computes.Add(x);
                lst.Add(x);
            }




            return lst;
        }

        public List<double> GetOptimalWidthFlexo(double z, int maxGain2, string SmallerFormat,  ref double DCut1, ref double DCut2, double maxSide1, int TypeOfDCut1 = 0)
        {

            var taskexEcutorSelected = TaskExecutors.SingleOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);

            double pinza = taskexEcutorSelected.Pinza ?? 0;
            double controPinza = taskexEcutorSelected.ControPinza ?? 0;
            double laterale = taskexEcutorSelected.Laterale ?? 0;

            var GiraVerso = true;

            double minusSide1 = GiraVerso ? laterale * 2 : pinza + controPinza;
            var ddpminusSide1 = GiraVerso ? laterale * 2 : pinza * 2;
            var minusSide2 = GiraVerso ? pinza + controPinza : laterale * 2;

            var res = new List<double>();

            try
            {
                int gain2_2 = (int)decimal.Truncate((decimal)((
                    (z - minusSide2) / (SmallerFormat.GetSide2())
                    )));

                //limite sulla resa dello z
                if (gain2_2 > maxGain2 && maxGain2 != 0)
                {
                    gain2_2 = maxGain2;
                }

                //doppio taglio calcolato su SideOnSide 2 0,257 --> 0,3
                double dCut2_2Res;

                switch (TypeOfDCut1)
                {
                    case 1:
                        dCut2_2Res =
                    (Math.Truncate(((z) - (SmallerFormat.GetSide2() * (gain2_2))) / (gain2_2) * 1000) / 1000);
                        break;
                        break;
                    case 2:
                        dCut2_2Res = 0;
                        break;
                    default:
                        dCut2_2Res = Math.Ceiling(
                            (Math.Truncate(((z) - (SmallerFormat.GetSide2() * (gain2_2))) / (gain2_2) * 1000) / 1000)
                            * 10
                            ) / 10;
                        break;
                }

                int gain1_1 = 1;
                while (

                    Math.Ceiling(
                    (gain1_1 * SmallerFormat.GetSide1() +
                    (gain1_1 - 1) * dCut2_2Res +
                    minusSide1) * 10
                    ) / 10

                    <= maxSide1)
                {
                    res.Add(Math.Ceiling(
                    (gain1_1 * SmallerFormat.GetSide1() +
                    (gain1_1 - 1) * dCut2_2Res +
                    minusSide1) * 100
                    ) / 100);

                    gain1_1++;
                }

                DCut1 = dCut2_2Res;
                DCut2 = DCut1;
            }
            catch (Exception)
            {
            }
            return res;
        }


        public override double UnitCost(double qta)
        {
            if (!IsValid)
            {
                return 0;
            }

            //devo usare gli avvimaneti, la tiratura totale e i mq
            //passarli ad un metodo della macchina corrente e mi restituisce il costo totale che dividerò per
            //la quantità!!!!

            double total = 0;
            TimeSpan time = new TimeSpan(0, 0, 0);
            CostAndTime totalCT = new CostAndTime();
            try
            {
                try
                {
                    double cToPrintF = 0;
                    double cToPrintR = 0;
                    double cToPrintT = 0;

                    TaskexEcutorSelected.GetColorFR(TaskCost.ProductPartTask.CodOptionTypeOfTask, out cToPrintF, out cToPrintR, out cToPrintT);
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, RollChanges ?? 0, (int)cToPrintT, Quantity(qta));
                
                
                }
                catch (NotImplementedException)
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, Quantity(qta));
                }

                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;
                //calcolo del tempo e del costo
                total = totalCT.Cost;
                CalculatedTime = totalCT.Time;
            }
            catch (NullReferenceException)
            {
                total = 0;
                Error = 2;
            }

            return (total) / Quantity(qta);

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