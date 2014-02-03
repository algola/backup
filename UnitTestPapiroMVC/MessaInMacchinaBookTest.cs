using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class MessaInMacchinaBookTest
    {
        [TestMethod]
        public void Rigido()
        {

            ProductPartRigid part = new ProductPartRigid();
            part.Format = "15x21";
            part.SubjectNumber = 1;
//            part.UpdateOpenedFormat();

            ProductPartSingleSheetPrinting prodPartPrint = new ProductPartSingleSheetPrinting();
            prodPartPrint.PrintingFormat = "250x100";
            prodPartPrint.Part = part;

            prodPartPrint.Update();

            var x = prodPartPrint.GainPartOnPrinting.Makereadies.Count;

        }
    }
}
