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
                    zero = ((((MakereadyPrintingSingleSheet)res).PrintedSubjects??0)==0);
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

            try
            {
                int gain1_1 = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - pinza - controPinza + dCut) / (SmallerFormat.GetSide1() + dCut))));
                int gain1_1ddp = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - pinza - pinza + dCut) / (SmallerFormat.GetSide1() + dCut))));

                int gain2_2 = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide2() - laterale - laterale + dCut) / (SmallerFormat.GetSide2() + dCut))));

                //TODO: controllare la pinza e doppia pinza!!!!!
                int gain2_2Perf = (gain2_2 % 2) == 0 ? gain2_2 : gain2_2 - 1;
                int gain1_1Perf = (gain1_1ddp % 2) == 0 ? gain1_1ddp : gain1_1ddp - 1;

                var gSideOnSide = gain1_1 * gain2_2;
                var gSideOnSide16 = gain1_1 * gain2_2Perf;
                var gSideOnSide12 = gain1_1Perf * gain2_2;

                var gain1_2 = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - pinza - controPinza + dCut) / (SmallerFormat.GetSide2() + dCut))));
                var gain1_2ddp = (int)decimal.Truncate((decimal)(((LargerFormat.GetSide1() - pinza - pinza + dCut) / (SmallerFormat.GetSide2() + dCut))));

                var gain2_1 = (int)decimal.Truncate(((decimal)((LargerFormat.GetSide2() - laterale - laterale + dCut) / (SmallerFormat.GetSide1() + dCut))));

                int gain2_1Perf = (gain2_1 % 2) == 0 ? gain2_1 : gain2_1 - 1;
                int gain1_2Perf = (gain1_2ddp % 2) == 0 ? gain1_2ddp : gain1_2ddp - 1;

                var gSideNotSide = gain1_2 * gain2_1;
                var gSideNotSide16 = gain1_2 * gain2_1Perf;
                var gSideNotSide12 = gain1_2Perf * gain2_1;

                if (!(UsePerfecting ?? false))
                {
                    gr.TypeOfPerfecting = "";

                    if (gSideOnSide >= gSideNotSide)
                    {
                        //decido se è SideOnSide
                        gr.SideOnSide = true;
                        gr.ShapeOnSide1 = gain1_1;
                        gr.ShapeOnSide2 = gain2_2;
                    }
                    else
                    {
                        gr.SideOnSide = false;
                        gr.ShapeOnSide1 = gain1_2;
                        gr.ShapeOnSide2 = gain2_1;
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


                if(calculatedShape == 0)
                {
                    throw new ZeroGainException();
                }

                //if (SubjectNumber <= calculatedShape)
                //{
                //    gr.PrintedShapes = (int)decimal.Truncate(calculatedShape / (SubjectNumber != 0 ? SubjectNumber : 1)) * (SubjectNumber != 0 ? SubjectNumber : 1);
                //    gr.PrintedSubjects =  SubjectNumber;
                //}
                //else
                //{
                //    gr.PrintedShapes = calculatedShape;
                //    gr.PrintedSubjects = gr.PrintedShapes;
                //}

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