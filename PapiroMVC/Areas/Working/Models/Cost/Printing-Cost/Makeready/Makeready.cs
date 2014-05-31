using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class Makeready : ICloneable
    {
        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            Makeready copyOfObject = (Makeready)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public virtual void Copy(Makeready to)
        {
            to.TimeStampTable = this.TimeStampTable;
            //   to.CodMakeready = this.CodMakeready;
            //   to.CodProductPartPrintingGain = this.CodProductPartPrintingGain ;

            to.ShapeOnSide1 = this.ShapeOnSide1;
            to.SideOnSide = this.SideOnSide;
            to.CalculatedGain = this.CalculatedGain;
            to.ShapeOnSide2 = this.ShapeOnSide2;

 //           to.ProductPartPrintingGain = (ProductPartPrintingGain)this.ProductPartPrintingGain.Clone();
        }

        double Side1ISpace { get; set; }
        double Side2ISpace { get; set; }
    }
}