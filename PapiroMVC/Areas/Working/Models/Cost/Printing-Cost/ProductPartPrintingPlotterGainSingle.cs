using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class ProductPartPrintingPlotterGainSingle : ProductPartPrintingPlotterGain
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
                while (SubjectNumber > 0)
                {
                    var res = this.CalculateShapeOnBuyingFormat();
                    this.Makereadies.Add(res);
                    SubjectNumber -= ((MakereadyPrintingSinglePlotter)res).PrintedSubjects ?? SubjectNumber;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("");
            }
        }

        //Plotter BuyingFormat
        protected override Makeready CalculateShapeOnBuyingFormat()
        {

            MakereadyPrintingSinglePlotter gr = new MakereadyPrintingSinglePlotter();

            try
            {
                int gain1_1 = (int)decimal.Truncate(Convert.ToDecimal(Width) / SmallerFormat.GetSide1());
                int gain2_2 = (int)System.Math.Ceiling(Convert.ToDecimal(SubjectNumber / gain1_1));

                var gSideOnSide = gain1_1 * gain2_2;

                var gain1_2 = (int)decimal.Truncate(Convert.ToDecimal(Width) / SmallerFormat.GetSide2());
                var gain2_1 = (int)System.Math.Ceiling(Convert.ToDecimal(SubjectNumber / gain1_2));

                var gSideNotSide = gain1_2 * gain2_1;

               // gr.TypeOfPerfecting = "";

                if (gain2_2*Width <= gain2_1*Width)
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

                

                var calculatedShape = MaxShape != 0 ? Math.Min((gr.ShapeOnSide1??0) * (gr.ShapeOnSide2??0), MaxShape ?? ((gr.ShapeOnSide1??0) * (gr.ShapeOnSide2??0))) : (gr.ShapeOnSide1??0) * (gr.ShapeOnSide2??0);

                if (SubjectNumber <= calculatedShape)
                {
                    gr.PrintedShapes = (int)decimal.Truncate(calculatedShape / (SubjectNumber != 0 ? SubjectNumber : 1)) * (SubjectNumber != 0 ? SubjectNumber : 1);
                    gr.PrintedSubjects = gr.PrintedShapes / SubjectNumber;
                }
                else
                {
                    gr.PrintedShapes = calculatedShape;
                    gr.PrintedSubjects = gr.PrintedShapes;
                }

                gr.CalculatedGain = gr.PrintedShapes / gr.PrintedSubjects;

            }
            catch (Exception)
            {
                gr.SideOnSide = true;
                gr.ShapeOnSide1 = 0;
                gr.ShapeOnSide2 = 0;
                gr.PrintedSubjects = 0;
                throw new Exception();
            }
            return gr;
        }
    }
}