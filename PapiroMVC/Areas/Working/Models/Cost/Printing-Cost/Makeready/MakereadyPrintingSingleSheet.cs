using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class MakereadyPrintingSingleSheet : Makeready
    {
        #region notused
        public int ShapeOnSide1Optimized { get; set; }
        public int ShapeOnSide2Optimized { get; set; }
        #endregion

        /// <summary>
        /// is the number of the shapes that page has to draw
        /// </summary>
        public int PrintedShapes { get; set; }
        public int PrintedSubjects { get; set; }
    }

}