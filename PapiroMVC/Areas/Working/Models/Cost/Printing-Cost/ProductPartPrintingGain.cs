using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public abstract partial class ProductPartPrintingGain : ICloneable
    {

        public Nullable<double> Pinza { get; set; }
        public Nullable<double> ControPinza { get; set; }
        public Nullable<double> Laterale { get; set; }


        //SETUPS
        //------------------------------------------------------------------------------------------------------------------------        
        public string SmallerFormat { get; set; }

        //this is mul to SubjectNumber to calculate maxShape
        public int Quantity { get; set; }

        //this method calculate gain with aux of CalculateShapeOnBuyingFormat()
        public abstract void CalculateGain();
        protected abstract Makeready CalculateShapeOnBuyingFormat();



        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartPrintingGain copyOfObject = (ProductPartPrintingGain)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(ProductPartPrintingGain to)
        {

            to.TimeStampTable = this.TimeStampTable;

            to.CodProductPartPrinting = null;
            to.CodProductPartPrintingGain = null;
            to.CodProductPartPrintingGainBuying = null;

            to.UsePerfecting = this.UsePerfecting;
            to.MaxShape = this.MaxShape;
            to.DCut = this.DCut;
            to.IsDCut = this.IsDCut;
            to.DCut1 = this.DCut1;
            to.DCut2 = this.DCut2;
            to.ForceSideOnSide = this.ForceSideOnSide;

            foreach (var mk in Makereadies)
            {
                var mk2 = (Makeready)mk.Clone();
//                mk2.ProductPartPrintingGain = to;
                if (to.Makereadies == null)
                {
                    to.Makereadies = new HashSet<Makeready>();
                }
                to.Makereadies.Add(mk2);

            }

            //        public virtual CostDetail CostDetail = this.

            //            to.ProductPartPrinting = this.ProductPartPrinting;

        }
    }

}