using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public class Signature
    {
        public Signature()
        {

        }

        public Signature(int codSegn, String segnName)
        {
            CodSegn = codSegn;
            SegnName = segnName;
            Count = 0;
        }
        public int CodSegn { get; set; }
        public String SegnName { get; set; }
        public int Count { get; set; }
    }

    public partial class ProductPartPrintingSheetGainBook : ProductPartPrintingSheetGain
    {
        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------
        public int PageToPrint { get; set; }
        //this is the maxShape imposed by input

        public byte MaxSignature { get; set; }
        public bool Signature { get; set; }

        public override void CalculateGain()        
        {
            if (Makereadies == null)
            {
                Makereadies = new List<Makeready>();
            }

            this.Makereadies.Clear();
            PageToPrint = (int)decimal.Truncate((PageToPrint == 0 ? 4 : PageToPrint) / 4) * 4;

            try
            {
                while (PageToPrint > 0)
                {
                    var res = this.CalculateShapeOnBuyingFormat();
                    this.Makereadies.Add(res);
                    PageToPrint -= ((MakereadyPrintingBookSheet)res).PrintedPages ?? PageToPrint;
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
            MakereadyPrintingBookSheet ret = new MakereadyPrintingBookSheet();

            try
            {
                int gain1_1 = (int)decimal.Truncate(LargerFormat.GetSide1() / SmallerFormat.GetSide1());
                int gain2_2 = (int)decimal.Truncate(LargerFormat.GetSide2() / SmallerFormat.GetSide2());

                int gain2_2Perf = (gain2_2 % 2) == 0 ? gain2_2 : gain2_2 - 1;
                int gain1_1Perf = (gain1_1 % 2) == 0 ? gain1_1 : gain1_1 - 1;

                var gSideOnSide = gain1_1 * gain2_2;
                var gSideOnSide16 = gain1_1 * gain2_2Perf;
                var gSideOnSide12 = gain1_1Perf * gain2_2;

                var gain1_2 = (int)decimal.Truncate(LargerFormat.GetSide1() / SmallerFormat.GetSide2());
                var gain2_1 = (int)decimal.Truncate(LargerFormat.GetSide2() / SmallerFormat.GetSide1());

                int gain2_1Perf = (gain2_1 % 2) == 0 ? gain2_1 : gain2_1 - 1;
                int gain1_2Perf = (gain1_2 % 2) == 0 ? gain1_2 : gain1_2 - 1;

                var gSideNotSide = gain1_2 * gain2_1;
                var gSideNotSide16 = gain1_2 * gain2_1Perf;
                var gSideNotSide12 = gain1_2Perf * gain2_1;

                if (!(UsePerfecting??false))
                {
                    ret.TypeOfPerfecting = "";

                    if (gSideOnSide >= gSideNotSide)
                    {
                        //decido se è SideOnSide
                        ret.SideOnSide = true;
                        ret.ShapeOnSide1 = gain1_1;
                        ret.ShapeOnSide2 = gain2_2;
                    }
                    else
                    {
                        ret.SideOnSide = false;
                        ret.ShapeOnSide1 = gain1_2;
                        ret.ShapeOnSide2 = gain2_1;
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
                        ret.TypeOfPerfecting = "16";
                        ret.SideOnSide = true;
                        ret.ShapeOnSide1 = gain1_1;
                        ret.ShapeOnSide2 = gain2_2Perf;
                    }
                    else
                    {
                        if (maxGain == gSideNotSide16)
                        {
                            ret.TypeOfPerfecting = "16";
                            ret.SideOnSide = false;
                            ret.ShapeOnSide1 = gain1_2;
                            ret.ShapeOnSide2 = gain2_1Perf;
                        }
                        else
                        {
                            if (maxGain == gSideOnSide12)
                            {
                                ret.TypeOfPerfecting = "12";
                                ret.SideOnSide = true;
                                ret.ShapeOnSide1 = gain1_1Perf;
                                ret.ShapeOnSide2 = gain2_2;
                            }
                            else
                            {
                                ret.TypeOfPerfecting = "12";
                                ret.SideOnSide = false;
                                ret.ShapeOnSide1 = gain1_2Perf;
                                ret.ShapeOnSide2 = gain2_1;
                            }
                        }
                    }
                }
               
                var calculatedShape = MaxShape != 0 ? Math.Min((ret.ShapeOnSide1 ?? 0) * (ret.ShapeOnSide2 ?? 0), MaxShape ?? ((ret.ShapeOnSide1 ?? 0) * (ret.ShapeOnSide2 ?? 0))) : (ret.ShapeOnSide1 ?? 0) * (ret.ShapeOnSide2 ?? 0);

                //4 pages per shape

                //se le pagine da stampare sono meno delle pagine che possono starci su un foglio
                if (PageToPrint <= calculatedShape * 4)
                {
                    ret.PrintablePages = (int)decimal.Truncate(calculatedShape * 4 / (PageToPrint != 0 ? PageToPrint : 4)) * (PageToPrint != 0 ? PageToPrint : 4);
                    ret.PrintedPages = PageToPrint;
                    ret.CalculatedGain = ret.PrintablePages / ret.PrintedPages;
                }
                else
                {
                    ret.PrintablePages = calculatedShape * 4;
                    ret.PrintedPages = ret.PrintablePages;
                    ret.CalculatedGain = 1;
                }

                ret.UpdateSignatures();

            }
            catch (Exception)
            {
                ret.SideOnSide = true;
                ret.ShapeOnSide1 = 0;
                ret.ShapeOnSide2 = 0;
                ret.PrintedPages = 0;
                throw new Exception();
            }

            return ret;
        }

    }

}