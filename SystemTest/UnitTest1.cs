using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.Models;
using System.IO;

namespace SystemTest
{
    [TestClass]
    public class FileNameTest
    {
        [TestMethod]
        public void PurgeTest()
        {

            string fileNameMain = Path.Combine("c:/Report", "OrderHead.docx").PurgeFileName();

            Console.Write(fileNameMain);

        }
    }
}
