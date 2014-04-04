using PapiroMVC.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ProductPartPrintingSheetGainSingle : ProductPartPrintingSheetGain
    {
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------
        public int SubjectNumber { get; set; }
        //this is the maxShape imposed by input

        //if DCut Is Calculated Automatically or set by Part
        //false = set by Part
        //true = calculated automatically
        public bool AutoDCut { get; set; }

        //true = pinza sul lato lungo
        public bool GiraVerso { get; set; }

        public ProductPartPrintingSheetGainSingle()
        {
            AutoDCut = false;
            GiraVerso = false;
            //            ForceSideOnSide = 0;
        }

        public override void CalculateGain()
        {
            if (Makereadies == null)
            {
                Makereadies = new List<Makeready>();
            }

            this.Makereadies.Clear();
            SubjectNumber = SubjectNumber == 0 ? 1 : SubjectNumber;

            try
            {
                bool zero = false;
                while (SubjectNumber > 0 && !zero)
                {
                    var res = this.CalculateShapeOnBuyingFormat();
                    zero = ((((MakereadyPrintingSingleSheet)res).PrintedSubjects ?? 0) == 0);
                    this.Makereadies.Add(res);
                    SubjectNumber -= ((MakereadyPrintingSingleSheet)res).PrintedSubjects ?? SubjectNumber;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("");
            }
        }

        //Sheet BuyingFormat
        protected override Makeready CalculateShapeOnBuyingFormat()
        {
            MakereadyPrintingSingleSheet gr = new MakereadyPrintingSingleSheet();

            double pinza = Pinza ?? 0;
            double controPinza = ControPinza ?? 0;
            double laterale = Laterale ?? 0;

            var dCut = DCut ?? 0;
            var dCut1 = DCut1 ?? 0;
            var dCut2 = DCut2 ?? 0;

            var forceSideOnSide = ForceSideOnSide ?? 0;

            if (AutoDCut)
            {
                dCut1 = 0;
                dCut2 = 0;
            }

            double minusSide1 = GiraVerso ? laterale * 2 : pinza + controPinza;
            var ddpminusSide1 = GiraVerso ? laterale * 2 : pinza * 2;
            var minusSide2 = GiraVerso ? pinza + controPinza : laterale * 2;

            try
            {
                int gain1_1 = (int)decimal.Truncate((decimal)((
                    (LargerFormat.GetSide1() - minusSide1 + dCut1) / (SmallerFormat.GetSide1() + dCut1)
                    )));

                //doppio taglio calcolato su SideOnSide 1
                double dCut1_1Res = ((LargerFormat.GetSide1() - minusSide1) - (SmallerFormat.GetSide1() * (gain1_1))) / ((gain1_1 - 1) == 0 ? 1 : (gain1_1 - 1));


                int gain1_1ddp = (int)decimal.Truncate((decimal)(((
                    LargerFormat.GetSide1() - ddpminusSide1 + dCut1) / (SmallerFormat.GetSide1() + dCut1)
                    )));

                int gain2_2 = (int)decimal.Truncate((decimal)((
                    (LargerFormat.GetSide2() - minusSide2 + dCut2) / (SmallerFormat.GetSide2() + dCut2)
                    )));

                //doppio taglio calcolato su SideOnSide 2
                double dCut2_2Res = ((LargerFormat.GetSide2() - minusSide2) - (SmallerFormat.GetSide2() * (gain2_2))) / (gain2_2);

                //TODO: controllare la pinza e doppia pinza!!!!!
                int gain2_2Perf = (gain2_2 % 2) == 0 ? gain2_2 : gain2_2 - 1;
                int gain1_1Perf = (gain1_1ddp % 2) == 0 ? gain1_1ddp : gain1_1ddp - 1;

                var gSideOnSide = gain1_1 * gain2_2;
                var gSideOnSide16 = gain1_1 * gain2_2Perf;
                var gSideOnSide12 = gain1_1Perf * gain2_2;

                var gain1_2 = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - minusSide1 + dCut2) / (SmallerFormat.GetSide2() + dCut2))));
                var gain1_2ddp = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - ddpminusSide1 + dCut2) / (SmallerFormat.GetSide2() + dCut2))));

                //doppio taglio calcolato su SideNotSide 1
                double dCut1_2Res = ((LargerFormat.GetSide1() - minusSide1) - (SmallerFormat.GetSide2() * (gain1_2))) / (gain1_2);

                var gain2_1 = (int)decimal.Truncate(((decimal)((LargerFormat.GetSide2() - minusSide2 + dCut1) / (SmallerFormat.GetSide1() + dCut1))));

                //doppio taglio calcolato su SideNotSide 1
                double dCut2_1Res = ((LargerFormat.GetSide2()) - (SmallerFormat.GetSide1() * (gain2_1))) / (gain2_1);

                int gain2_1Perf = (gain2_1 % 2) == 0 ? gain2_1 : gain2_1 - 1;
                int gain1_2Perf = (gain1_2ddp % 2) == 0 ? gain1_2ddp : gain1_2ddp - 1;

                var gSideNotSide = gain1_2 * gain2_1;
                var gSideNotSide16 = gain1_2 * gain2_1Perf;
                var gSideNotSide12 = gain1_2Perf * gain2_1;

                if (!(UsePerfecting ?? false))
                {
                    gr.TypeOfPerfecting = "";

                    if ((gSideOnSide >= gSideNotSide && forceSideOnSide != 2) ||
                    (forceSideOnSide == 1))
                    {
                        //decido se è SideOnSide
                        gr.SideOnSide = true;
                        gr.ShapeOnSide1 = gain1_1;
                        gr.ShapeOnSide2 = gain2_2;

                        if (AutoDCut)
                        {
                            dCut1_1Res = dCut1_1Res > (dCut2_2Res * 2) ? dCut2_2Res * 2 : dCut1_1Res;

                            var tempDCut2 = Math.Round(dCut2_2Res * 10000) / 10000;
                            var tempDCut1 = Math.Round(dCut1_1Res * 10000) / 10000;

                            DCut2 = Math.Truncate(tempDCut2 * 100) / 100;
                            DCut1 = Math.Truncate(tempDCut1 * 100) / 100;

                            if (tempDCut1 > minusSide1 / 2)
                            {
                                //ingombro
                                var ingo = (SmallerFormat.GetSide1() + dCut1_1Res) * (gain1_1);
                                var restoIngo = (LargerFormat.GetSide1() - ingo);

                                if (((LargerFormat.GetSide1() - (SmallerFormat.GetSide1() + tempDCut1) * (gain1_1)) <= minusSide1))
                                {
                                    tempDCut1 = Math.Max(tempDCut2, minusSide1 / 2);
                                    DCut1 = Math.Truncate(tempDCut1 * 100) / 100;
                                }
                            }

                            if (gain1_1 == 1)
                            {
                                DCut1 = 0;
                            }
                        }

                    }
                    else
                    {
                        gr.SideOnSide = false;
                        gr.ShapeOnSide1 = gain1_2;
                        gr.ShapeOnSide2 = gain2_1;

                        if (AutoDCut)
                        {
                            DCut1 = 0;//dCut2_1Res;
                            DCut2 = 0;//dCut1_2Res;
                        }
                    }
                }

                else
                {
                    List<int> a = new List<int>();
                    a.Add(gSideOnSide12);
                    a.Add(gSideOnSide16);
                    a.Add(gSideNotSide12);
                    a.Add(gSideNotSide16);

                    var maxGain = a.Max();

                    //è da preferire il 16 sul 12
                    if (maxGain == gSideOnSide16)
                    {
                        gr.TypeOfPerfecting = "16";
                        gr.SideOnSide = true;
                        gr.ShapeOnSide1 = gain1_1;
                        gr.ShapeOnSide2 = gain2_2Perf;
                    }
                    else
                    {
                        if (maxGain == gSideNotSide16)
                        {
                            gr.TypeOfPerfecting = "16";
                            gr.SideOnSide = false;
                            gr.ShapeOnSide1 = gain1_2;
                            gr.ShapeOnSide2 = gain2_1Perf;
                        }
                        else
                        {
                            if (maxGain == gSideOnSide12)
                            {
                                gr.TypeOfPerfecting = "12";
                                gr.SideOnSide = true;
                                gr.ShapeOnSide1 = gain1_1Perf;
                                gr.ShapeOnSide2 = gain2_2;
                            }
                            else
                            {
                                gr.TypeOfPerfecting = "12";
                                gr.SideOnSide = false;
                                gr.ShapeOnSide1 = gain1_2Perf;
                                gr.ShapeOnSide2 = gain2_1;
                            }
                        }
                    }
                }

                var calculatedShape = MaxShape != 0 ? Math.Min((gr.ShapeOnSide1 ?? 0) * (gr.ShapeOnSide2 ?? 0), MaxShape ?? ((gr.ShapeOnSide1 ?? 0) * (gr.ShapeOnSide2 ?? 0))) : (gr.ShapeOnSide1 ?? 0) * (gr.ShapeOnSide2 ?? 0);

                if (calculatedShape == 0)
                {
                    throw new ZeroGainException();
                }

                var subMolQta = SubjectNumber * Quantity;

                if (subMolQta <= calculatedShape)
                {
                    gr.PrintedShapes = (int)decimal.Truncate(calculatedShape / (subMolQta != 0 ? subMolQta : 1)) * (subMolQta != 0 ? subMolQta : 1);
                    //se le quantità non superano le printed shape... allora limito le shape alle quantità

                    if (gr.PrintedShapes > subMolQta)
                    {
                        gr.PrintedShapes = subMolQta;
                    }

                    gr.PrintedSubjects = subMolQta;
                }
                else
                {
                    gr.PrintedShapes = calculatedShape;
                    gr.PrintedSubjects = gr.PrintedShapes;
                }

                gr.CalculatedGain = gr.PrintedShapes / SubjectNumber;

            }
            catch (ZeroGainException)
            {
                gr.SideOnSide = true;
                gr.ShapeOnSide1 = 0;
                gr.ShapeOnSide2 = 0;
                gr.PrintedSubjects = 0;
                gr.CalculatedGain = 0;
                gr.PrintedShapes = 0;
            }
            return gr;
        }

    }
}