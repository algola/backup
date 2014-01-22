using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var value = "asas@LOGOS";
            Regex reg = new Regex("^[a-zA-Z0-9]*$", RegexOptions.IgnoreCase);
            if (value == null) value = "";
            var xxx = reg.IsMatch(value.ToString());

            Console.Write(xxx);
        }
    }
}
