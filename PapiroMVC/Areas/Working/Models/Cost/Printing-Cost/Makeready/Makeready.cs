using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class Makeready
    {
        public int ShapeOnSide1 { get; set; }
        public int ShapeOnSide2 { get; set; }
        /// <summary>
        /// show if side1 is on side1 othewise
        /// </summary>
        public bool SideOnSide { get; set; }

        public string TypeOfPerfecting { get; set; }

        /// <summary>
        /// this is the gain calculated on the sheet
        /// is decimal because 0,5 --> it's need two sheet 
        /// </summary>
        public decimal CalculatedGain { get; set; }
    }
}