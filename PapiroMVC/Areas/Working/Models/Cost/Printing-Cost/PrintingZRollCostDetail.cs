using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace PapiroMVC.Models
{

    public partial class PrintingZRollCostDetail : PrintingCostDetail
    {

        //useed after save Die to hide div
        public Boolean DieSaved
        {
            get;
            set;
        }

        //this property is used by Die choice
        public List<Die> Dies
        {
            get;
            set;
        }

        //cm
        public double DieTollerance
        {
            get;
            set;
        }

        public override void Copy(CostDetail to)
        {
            base.Copy(to);

            PrintingZRollCostDetail to2 = (PrintingZRollCostDetail)to;

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

        public PrintingZRollCostDetail()
        {
            TypeOfCostDetail = CostDetailType.PrintingZRollCostDetail;
        }

        /// <summary>
        /// Elenco dei possibili formati di acquisto 
        /// </summary>

        private List<String> buyingFormats;
        public List<String> BuyingFormats
        {
            get
            {
                return buyingFormats;
            }

            set
            {
                buyingFormats = value;
            }
        }

        //if is zero than no restriction on
        public double Width { get; set; }

        //it is for flatRoll
        public List<double> BuyingWidths { get; set; }


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
                        lst.Add(new ProductFormatName { FormatName = "h" + bF.GetSide1() + " z" + (((Flexo)curTsk).GetZFromCm(bF.GetSide2()).ToString()), CodFormat = bF });
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
                    case ProductPart.ProductPartType.ProductPartDoubleLabelRoll:
                        this.ProductPartPrinting = new ProductPartDoubleSheetPrinting();
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



            //this.DieTollerance = 0.5;
            ////qui voglio solo le fustelle flexo e semiroll
            //this.Dies = articles.OfType<Die>();


            //la stampa è condizionata al fatto che il PrintingFormat stia in macchina!!!
            //quindi è da scegliere tra i possibili formati "spaccati" dal formato acquisto
            //la lettura della proprietà Cuts ricalcola il PrintingFormat!!!!
            var fake = this.Cuts;


            //la stampa deve essere valutata sui formati elaborati dalla fuzzy
            //FuzzyAlgo();


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

        //se esiste almeno una resa e non ho formati validi... provo ad aumentare il range dell'interspazio
        protected bool haveAlmostOne = false;
        //voglio sapere anche qual'è interspazio minore calcolato
        protected double smallerCalculatedDCut = 100;
        //interspazio negativo
        protected double smallerCalculatedDCutLessZero = 100;

        public virtual void FuzzyAlgo()
        {
            if (TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.FlatRoll)
            {
                FuzzyAlgoFlat();
            }
            else
            {
                FuzzyAlgoFlexo();
            }
        }

        //dovrei sapere all'interno della
        private void FuzzyAlgoFlexo()
        {
            List<String> newBuyingFormats = new List<string>();
            var pHint = new List<PrintingHint>();


            //ZMetric for Flexo
            bool zMetric = false;
            if (TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                zMetric = ((Flexo)TaskexEcutorSelected).ZMetric ?? false;
            }


            var ppP = new ProductPartSingleSheetPrinting();

            //Scorro i Dies e se rientrano nelle tolleranze!!!!
            var dies = new List<Die>();


            ppP.CostDetail = this;
            ppP.Part = this.ProductPart;

            if (ProductPart.FormatType != -1)
            {


                foreach (var die in Dies.OfType<DieFlexo>())
                {
                    var side1 = ppP.Part.FormatOpened.GetSide1();
                    var side2 = ppP.Part.FormatOpened.GetSide2();

                    if (Math.Abs(die.Format.GetSide1() - side1) <= DieTollerance &&
                        Math.Abs(die.Format.GetSide2() - side2) <= DieTollerance)
                    {

                        ppP.PrintingFormat = die.PrintingFormat;
                        ppP.MaxGain1 = die.MaxGain1 ?? 0;
                        ppP.MaxGain2 = die.MaxGain2 ?? 0;
                        ppP.AutoCutParameter = true;
                        ppP.LateralMinDCut = true;
                        ppP.Update();

                        //voglio aggiungere le fustelle valide!!!!
                        var pHDie = new PrintingHint
                        {
                            Format = die.Format,
                            //DCut1 = ppP.CalculatedDCut1,
                            //DCut1 = die.DCut1,
                            //DCut2 = die.DCut2,

                            TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

                            DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                            DCut2 = ppP.CalculatedDCut2,

                            BuyingFormat = die.PrintingFormat,

                            PrintingFormat = die.PrintingFormat,
                            Description = "h" + die.PrintingFormat.GetSide1() + " z" + (die.GetZFromCm(die.PrintingFormat.GetSide2())).ToString() + "*",
                            FormatType = (die.FormatType ?? 0).ToString(),
                            CalculatedGain = ppP.CalculatedGain,
                            GainOnSide1 = die.MaxGain1 ?? 0,
                            GainOnSide2 = die.MaxGain2 ?? 0,

                            IsDie=true,

                            DeltaDCut2 = 0
                        };

                        pHint.Add(pHDie);
                    }
                }
            }

            foreach (var buyingFormat in BuyingFormats)
            {
                if (buyingFormat.GetSide2() == 36.83)
                {
                    if (buyingFormat.GetSide1() == 20)
                    {
                        Console.Write("ciao");
                    }
                }


                if (SheetCut.IsValid(TaskexEcutorSelected.FormatMax, TaskexEcutorSelected.FormatMin, buyingFormat))
                {

                    ppP.CostDetail = this;

                    ppP.Part = this.ProductPart;
                    ppP.PrintingFormat = buyingFormat;
                    ppP.AutoCutParameter = true;
                    ppP.LateralMinDCut = true;
                    ppP.MaxGain1 = 0;
                    ppP.MaxGain2 = 0;
                    ppP.Update();
                    haveAlmostOne = (ppP.CalculatedGain > 0) || haveAlmostOne;


                    var calc1Gain = ppP.CalculatedSide1Gain;
                    var calc2Gain = ppP.CalculatedSide2Gain;


                    for (int i = calc2Gain; i > 0; i--)
                    {
                        ppP.MaxGain2 = i;
                        ppP.Update();

                        for (int k = calc1Gain; k > 0; k--)
                        {
                            //provo a togliere una posa in banda se l'interspazio di passo è > di quello della banda
                            if (ppP.CalculatedDCut1 < ppP.CalculatedDCut2)
                            {
                                //tolgo una posa in banda
                                ppP.MaxGain1 = k;
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
                                ZMetric = zMetric,
                                Format = ppP.Part.FormatOpened,
                                DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                DCut2 = ppP.CalculatedDCut2,
                                TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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

                            pH.Description = "h" + buyingFormat.GetSide1() + " z" + pH.Z.ToString();

                            if (!pHint.Contains(pH, new PrintingHintComparer()))
                            {
                                //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                                pHint.Add(pH);
                            }


                            if (ppP.CalculatedSide2Gain >= 1 && ppP.MaxGain2 == 0)
                            {

                                //provo ad aggiungere una nuova resa con interspazio negativo
                                var lessZero = Math.Abs((ppP.Part.FormatOpened.GetSide2() - (ppP.CalculatedDCut2 * ppP.CalculatedSide2Gain))) / (ppP.CalculatedSide2Gain + 1);
                                lessZero = Math.Truncate(lessZero * 1000 + 1) / 1000;
                                smallerCalculatedDCutLessZero = lessZero < smallerCalculatedDCutLessZero ? lessZero : smallerCalculatedDCutLessZero;

                                pH =
                                    new PrintingHint
                                    {
                                        ZMetric = zMetric,
                                        Format = ppP.Part.FormatOpened,
                                        DCut1 = 0, // DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                        DCut2 = lessZero * (-1),
                                        TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

                                        BuyingFormat = buyingFormat,
                                        PrintingFormat = buyingFormat,
                                        MaxGain2 = ppP.MaxGain2 + 1,
                                        CalculatedGain = ppP.CalculatedGain + ppP.CalculatedSide1Gain,
                                        GainOnSide1 = ppP.CalculatedSide1Gain,
                                        GainOnSide2 = ppP.CalculatedSide2Gain + 1,
                                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - lessZero * (-1))
                                    };

                                pH.Description = "h" + buyingFormat.GetSide1() + " z" + pH.Z.ToString();


                                if (!pHint.Contains(pH, new PrintingHintComparer()))
                                {
                                    //il printing hint deve avere i formati stampabili, i suoi DCut, ma anche fustelle simili
                                    pHint.Add(pH);
                                }

                                //ripristino l'autocut
                                ppP.AutoCutParameter = true;
                                ppP.LateralMinDCut = true;

                            }

                        }
                    }

                }//End if
            }
            ppP.MaxGain2 = 0;

            if (PrintingFormat != "" && PrintingFormat != null)
            {
                var pH = new PrintingHint
                    {
                        ZMetric = zMetric,
                        Format = ppP.Part.FormatOpened,
                        DCut1 = ProductPartPrinting.CalculatedDCut1,
                        DCut2 = ProductPartPrinting.CalculatedDCut2,
                        TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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

                pH.Description = "h" + PrintingFormat.GetSide1() + " z" + pH.Z.ToString();

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
            var pHint1 = ProductPart.SelectValidpHint(pHint, smallerCalculatedDCutLessZero, smallerCalculatedDCut);

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

                var newBands = this.GetOptimizedWidths(item.PrintingFormat.GetSide2(), item.MaxGain2, ppP.Part.FormatOpened, ref dCut1, ref  dCut2, 34, ProductPart.TypeOfDCut1 ?? 0, ProductPart.TypeOfDCut2 ?? 0);
                foreach (var nB in newBands)
                {

                    ppP.PrintingFormat = nB.ToString() + "x" + item.PrintingFormat.GetSide2();  // item.Z.ToString();
                    ppP.MaxGain1 = 0;
                    ppP.MaxGain2 = 0;
                    ppP.AutoCutParameter = false;
                    ppP.Part.DCut2 = item.DCut2;//dCut2;
                    ppP.Part.DCut1 = dCut1;
                    ppP.Update();

                    int i = 1;

                    //non vale per le fascette gommate
                    if (ProductPart.MinDCut != 0)
                    {
                        int maxGain1;
                        maxGain1 = ppP.CalculatedSide1Gain;
                        while (ppP.CalculatedDCut1 < ppP.CalculatedDCut2 && (ppP.CalculatedSide1Gain - i) > 1)
                        {
                            maxGain1 = ppP.CalculatedSide1Gain - i++;
                            ppP.MaxGain1 = maxGain1;
                            ppP.Update();
                        }
                    }
                    var pH = new PrintingHint
                    {
                        ZMetric = zMetric,
                        Format = ppP.Part.FormatOpened,
                        DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                        DCut2 = item.DCut2 ?? ppP.CalculatedDCut2,
                        TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

                        BuyingFormat = ppP.PrintingFormat,
                        PrintingFormat = ppP.PrintingFormat,
                        Description = "h" + ppP.PrintingFormat.GetSide1() + " z" + (ppP.PrintingFormat.GetSide2() / 2.54 * 8).ToString(),
                        CalculatedGain = ppP.CalculatedGain,
                        GainOnSide1 = ppP.CalculatedSide1Gain,
                        GainOnSide2 = ppP.CalculatedSide2Gain,
                        DeltaDCut2 = Math.Abs((ProductPart.MaxDCut ?? 0) - (item.DCut2 ?? ppP.CalculatedDCut2))

                    };

                    pH.Description = "h" + ppP.PrintingFormat.GetSide1() + " z" + (pH.Z).ToString();

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

        public override void UpdateCoeff()
        {
            base.UpdateCoeff();

            //calcoli per mt lineari!!!!!!!!
            Nullable<long> paperFirstStartLenght;// = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
            Nullable<long> paperSecondStartLenght;// = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;

            if (TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                paperFirstStartLenght = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
                paperSecondStartLenght = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;
            }
            else
            {
                paperFirstStartLenght = 0;
                paperSecondStartLenght = 0;
            }

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

            //calcoli per mt lineari!!!!!!!!
            Nullable<long> paperFirstStartLenght;// = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
            Nullable<long> paperSecondStartLenght;// = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;

            if (TaskexEcutorSelected.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
            {
                paperFirstStartLenght = ((Flexo)TaskexEcutorSelected).PaperFirstStartLenght;
                paperSecondStartLenght = ((Flexo)TaskexEcutorSelected).PaperSecondStartLenght;
            }
            else
            {
                paperFirstStartLenght = 0;
                paperSecondStartLenght = 0;
            }

            var runs = Math.Ceiling(QuantityProp * this.GainForRun ?? 0);
            var mtRuns = runs * PrintingFormat.GetSide2() / 100;
            var mtWaste = (paperFirstStartLenght ?? 0) + (RollChanges * paperSecondStartLenght ?? 0);

            mlMat = Math.Ceiling(mtRuns + mtWaste);
            CalculatedMl = mlMat;

            mqMat = Math.Ceiling(base.Quantity(qta));
            CalculatedMq = mqMat;

            kgMat = 0;
            CalculatedKg = kgMat;

            CalculatedRun = runs;

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
            if (!justInited)
            {
                base.InitCostDetail(tskExec, articles);

                _articles = articles.ToList().AsQueryable();

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

                //FLAT
                //***********************************************
                foreach (var width in widthList)
                {
                    if (BuyingWidths == null) BuyingWidths = new List<double>();
                    BuyingWidths.Add(width ?? 0);
                }


                //FLEXO
                //**********************************************
                var tskCurrent = tskExec.FirstOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);
                if (tskCurrent == null)
                {
                    tskCurrent = tskExec.FirstOrDefault();
                }

                var zList = new List<string>();

                foreach (var item in tskCurrent.TaskExecutorCylinders)
                {
                    if (tskCurrent.TypeOfExecutor == TaskExecutor.ExecutorType.Flexo)
                    {
                        zList.Add(
                            ((Flexo)tskCurrent).GetCmFromZ(item.Z ?? 0).ToString());
                    }
                    else
                    {
                        zList.Add((Convert.ToDouble(item.Z) / 8 * 2.54).ToString());
                    }
                }

                //combino i width con gli Z

                foreach (var width in widthList.Where(x => x != 0))
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
        }

        public override List<CostDetail> CreateRelatedPrintedCostDetail(IQueryable<Article> articles, IQueryable<Cost> costs)
        {
            List<CostDetail> lst = new List<CostDetail>();

            //per ciascun materiale stampabile...
            foreach (var item in this.ProductPart.ProductPartPrintableArticles)
            {

                var xx = articles.GetArticlesByProductPartPrintableArticle(item).FirstOrDefault();
                PrintedArticleCostDetail newCostDetail;

                switch (xx.TypeOfArticle)
                {
                    case Article.ArticleType.SheetPrintableArticle:
                        newCostDetail = new PrintedSheetArticleCostDetail();
                        break;
                    case Article.ArticleType.RollPrintableArticle:
                        newCostDetail = new PrintedRollArticleCostDetail();
                        break;
                    case Article.ArticleType.RigidPrintableArticle:
                        newCostDetail = new PrintedRigidArticleCostDetail();
                        break;
                    case Article.ArticleType.ObjectPrintableArticle:
                        throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                newCostDetail.ComputedBy = this;
                newCostDetail.ProductPart = this.ProductPart;

                //devo pescare il costo e associarlo al dettaglio
                if (newCostDetail.CodCost == null)
                {
                    var xxxx = costs.ToList();
                    var cost = costs.Where(pp => pp.CodProductPartPrintableArticle == item.CodProductPartPrintableArticle).FirstOrDefault();


                    //da non usare MAIIII                    x.TaskCost = cost;
                    newCostDetail.CodCost = cost.CodCost;
                    newCostDetail.CodCostDetail = cost.CodCost;
                    newCostDetail.CostDetailCostCodeRigen();


                    //if there is another CostDetail
                    if (cost.CostDetails.Count == 0)
                    {
                        //GUID
                        newCostDetail.Guid = this.Guid;
                        this.Computes.Add(newCostDetail);
                        lst.Add(newCostDetail);
                    }


                }



            }
            return lst;
        }

        public List<double> GetOptimizedWidths(double z, int maxGain2, string SmallerFormat, ref double DCut1, ref double DCut2, double maxSide1, int TypeOfDCut1 = 0, int TypeOfDCut2 = 0)
        {

            var taskexEcutorSelected = TaskExecutors.SingleOrDefault(x => x.CodTaskExecutor == CodTaskExecutorSelected);

            double pinza = taskexEcutorSelected.Pinza ?? 0;
            double controPinza = taskexEcutorSelected.ControPinza ?? 0;
            double laterale = taskexEcutorSelected.Laterale ?? 0;

            var GiraVerso = true;


            double minusSide1 = GiraVerso ? laterale * 2 : pinza + controPinza;
            double minusSide1WoLat = GiraVerso ? 0 : pinza + controPinza;


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

                        if (TypeOfDCut2 != 0)
                        {
                            dCut2_2Res /= 2;
                        }

                        break;
                    case 2:
                        dCut2_2Res = 0;
                        break;
                    default:

                        var temp = (Math.Truncate(((z) - (SmallerFormat.GetSide2() * (gain2_2))) / (gain2_2) * 1000) / 1000);

                        if (TypeOfDCut2 != 0)
                        {
                            temp /= 2;
                        }

                        dCut2_2Res = Math.Ceiling(temp
                            * 10
                            ) / 10;
                        break;
                }


                if ((minusSide1 / 2) < dCut2_2Res)
                {
                    minusSide1 = dCut2_2Res * 2;
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


            double totCostInk = 0;

            double quantita = 0;
            quantita = Math.Ceiling(qta * (GainForRun ?? 1));


            //            var fto = this.TaskCost.ProductPartTask.ProductPart.FormatOpened;
            //            var mqTot = (fto.GetSide1() * fto.GetSide2() / 10000) * qta;


            var mqTot = CalculatedMq??0;

            List<ProductPartPrintRollOption> optSeris = new List<ProductPartPrintRollOption>();
            //serigraphy options where we can find the inks and types
            foreach (var item in this.TaskCost.ProductPartTask.ProductPartTaskOptions.OfType<ProductPartPrintRollOption>())
            {
                optSeris.Add((ProductPartPrintRollOption)item);
            }

            foreach (var item in optSeris)
            {
                int mqPrint = 0;
                mqPrint = Convert.ToInt32(Math.Ceiling(mqTot * (item.Overlay ?? 0) / 100));

                //var seriSpec = option
                var inkSpec = _articles.OfType<Ink>().FirstOrDefault(x => x.ArticleName == item.Ink);
                var typeSeri = (Anilox)_articles.OfType<Anilox>().FirstOrDefault(x => x.ArticleName == item.TypeOfTaskPrint);

                if (inkSpec != null && typeSeri != null)
                {
                    int ltTot = Convert.ToInt32(Math.Ceiling(mqPrint / Convert.ToDouble(typeSeri.GainMqPerLt ?? 1)));
                    totCostInk += ltTot * Convert.ToDouble(inkSpec.ArticleCosts.OfType<NoPrintableArticleCostKg>().FirstOrDefault().CostPerKg, Thread.CurrentThread.CurrentUICulture);
                }
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

                    var x = TaskexEcutorSelected.GetColorFR(TaskCost.ProductPartTask.CodOptionTypeOfTask);

                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(
                        TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, x.cToPrintR, RollChanges ?? 0, (int)(x.cToPrintT + x.cToPrintTNoImplant), Quantity(qta));


                }
                catch (NotImplementedException)
                {
                    totalCT = TaskexEcutorSelected.SetTaskExecutorEstimatedOn.FirstOrDefault().GetCost(TaskCost.ProductPartTask.CodOptionTypeOfTask, Starts ?? 1, Quantity(qta));
                }

                Error = (Error != null && Error != 0 && Error != 2) ? 0 : Error;
                //calcolo del tempo e del costo
            
                total = totalCT.Cost;

                total += totCostInk;
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

        private void FuzzyAlgoFlat()
        {
            List<String> newBuyingFormats = new List<string>();
            var pHint = new List<PrintingHint>();

            var ppP = new ProductPartSingleSheetPrinting();

            //Scorro i Dies e se rientrano nelle tolleranze!!!!
            var dies = new List<Die>();

            ppP.CostDetail = this;
            ppP.Part = this.ProductPart;



            if (ProductPart.FormatType != -1)
            {


                foreach (var die in Dies.OfType<DieFlatRoll>())
                {
                    var side1 = ppP.Part.FormatOpened.GetSide1();
                    var side2 = ppP.Part.FormatOpened.GetSide2();

                    if (Math.Abs(die.Format.GetSide1() - side1) <= DieTollerance &&
                        Math.Abs(die.Format.GetSide2() - side2) <= DieTollerance)
                    {

                        ppP.PrintingFormat = die.PrintingFormat;
                        ppP.MaxGain1 = die.MaxGain1 ?? 0;
                        ppP.MaxGain2 = die.MaxGain2 ?? 0;
                        ppP.AutoCutParameter = true;
                        ppP.LateralMinDCut = true;
                        ppP.Update();


                        //voglio aggiungere le fustelle valide!!!!
                        var pHDie = new PrintingHint
                        {
                            Format = die.Format,
                            //DCut1 = ppP.CalculatedDCut1,
                            //DCut1 = die.DCut1,
                            //DCut2 = die.DCut2,

                            DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                            DCut2 = ppP.CalculatedDCut2,
                            TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,


                            BuyingFormat = die.PrintingFormat,

                            PrintingFormat = die.PrintingFormat,
                            Description = "h" + die.PrintingFormat.GetSide1() + " z" + (die.GetZFromCm(die.PrintingFormat.GetSide2())).ToString() + "*",
                            FormatType = (die.FormatType ?? 0).ToString(),
                            CalculatedGain = ppP.CalculatedGain,
                            GainOnSide1 = die.MaxGain1 ?? 0,
                            GainOnSide2 = die.MaxGain2 ?? 0,

                            IsDie=true,

                            DeltaDCut2 = 0
                        };

                        pHint.Add(pHDie);

                    }
                }
            }


            var labelSide2 = this.ProductPart.FormatOpened.GetSide2();
            var labelCut2 = this.ProductPart.DCut2 ?? 0;

            //calculate max Gain on Side2 based on TaskExecutor MaxFormat
            var steps = Math.Floor(TaskexEcutorSelected.FormatMax.GetSide2() / labelSide2 + labelCut2);

            foreach (var width in BuyingWidths)
            {
                for (int step = 1; step <= steps; step++)
                {
                    ppP.AutoCutParameter = false;
                    var buyingFormat = width + "x" + step * (labelCut2 + labelSide2);


                    if (SheetCut.IsValid(TaskexEcutorSelected.FormatMax, TaskexEcutorSelected.FormatMin, buyingFormat))
                    {

                        ppP.CostDetail = this;

                        ppP.Part = this.ProductPart;
                        ppP.PrintingFormat = buyingFormat;
                        ppP.AutoCutParameter = true;
                        ppP.LateralMinDCut = true;
                        ppP.MaxGain1 = 0;
                        ppP.MaxGain2 = 0;
                        ppP.Update();
                        haveAlmostOne = (ppP.CalculatedGain > 0) || haveAlmostOne;


                        var calc1Gain = ppP.CalculatedSide1Gain;
                        var calc2Gain = ppP.CalculatedSide2Gain;

                        for (int i = calc2Gain; i > 0; i--)
                        {
                            ppP.MaxGain2 = ppP.CalculatedSide2Gain - i;
                            ppP.Update();

                            for (int k = calc1Gain; k > 0; k--)
                            {
                                //provo a togliere una posa in banda se l'interspazio di passo è > di quello della banda
                                if (ppP.CalculatedDCut1 < ppP.CalculatedDCut2)
                                {
                                    //tolgo una posa in banda
                                    ppP.MaxGain1 = k;
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
                                        Format = ppP.Part.FormatOpened,
                                        DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                        DCut2 = ppP.CalculatedDCut2,
                                        TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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
                                    var lessZero = Math.Abs((ppP.Part.FormatOpened.GetSide2() - (ppP.CalculatedDCut2 * ppP.CalculatedSide2Gain))) / (ppP.CalculatedSide2Gain + 1);
                                    lessZero = Math.Truncate(lessZero * 1000 + 1) / 1000;
                                    smallerCalculatedDCutLessZero = lessZero < smallerCalculatedDCutLessZero ? lessZero : smallerCalculatedDCutLessZero;

                                    pH =
                                        new PrintingHint
                                        {
                                            Format = ppP.Part.FormatOpened,
                                            DCut1 = 0, // DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                                            DCut2 = lessZero * (-1),
                                            TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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
                                    ppP.LateralMinDCut = true;
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
                    Format = ppP.Part.FormatOpened,
                    DCut1 = ProductPartPrinting.CalculatedDCut1,
                    DCut2 = ProductPartPrinting.CalculatedDCut2,
                    TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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
            var pHint1 = ProductPart.SelectValidpHint(pHint, smallerCalculatedDCutLessZero, smallerCalculatedDCut);

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
                var newBands = this.GetOptimizedWidths(item.PrintingFormat.GetSide2(), item.MaxGain2, ppP.Part.FormatOpened, ref dCut1, ref  dCut2, 34, ProductPart.TypeOfDCut1 ?? 0);
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
                        Format = ppP.Part.FormatOpened,
                        DCut1 = DCut1OnDCut2AndPart(ppP.CalculatedDCut1, ppP.CalculatedDCut2, ppP.Part),
                        DCut2 = item.DCut2 ?? ppP.CalculatedDCut2,
                        TypeOfDCut2 = ppP.Part.TypeOfDCut2 ?? 0,

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

    }


}