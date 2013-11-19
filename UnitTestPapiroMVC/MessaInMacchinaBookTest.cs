using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class MessaInMacchinaBookTest
    {
        [TestMethod]
        public void MessaInMac()
        {

            ProductPartBookSheet part = new ProductPartBookSheet();
            part.Pages = 90;
            part.Format = "15x21";
            part.UpdateOpenedFormat();

            ProductPartBookSheetPrinting prodPartPrint = new ProductPartBookSheetPrinting();
            prodPartPrint.PrintingFormat = "70x100";
            prodPartPrint.Part = part;

            prodPartPrint.Update();

            var x = prodPartPrint.GainPartOnPrinting.Makereadies.Count;

        }
    }
}
