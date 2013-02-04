using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace UnitTestPapiroMVC
{
    [TestClass]
    public class UnitTest2
    {

        public static class ExpandableHexConverter
        {
            public enum ExpandLevel
            {
                A = 11,
                B,
                C,
                D,
                E,
                F,
                G,
                H,
                I,
                J,
                K,
                L,
                M,
                N,
                O,
                P,
                Q,
                R,
                S,
                T,
                U,
                V,
                W,
                X,
                Y,
                Z,
                UseCaseSensitive = 62
            }

            public static string ToHex(long value, ExpandLevel ExpandBy)
            {
                return loopRemainder(value, (long)ExpandBy);
            }

            public static long ToInt64(string value, ExpandLevel ExpandBy)
            {
                value = validate(value, ExpandBy);
                long returnvalue = 0;
                for (int i = 0; i < value.Length; i++)
                    returnvalue += (long)Math.Pow((long)ExpandBy,
                                   (value.Length - (i + 1))) * CharToVal(value[i]);
                return returnvalue;
            }

            private static string loopRemainder(long value, long PowerOf)
            {
                long x = 0;
                long y = Math.DivRem(value, PowerOf, out x);
                if (y > 0)
                    return loopRemainder(y, PowerOf) + ValToChar(x).ToString();
                else
                    return ValToChar(x).ToString();
            }
            private static char ValToChar(long value)
            {
                if (value > 9)
                {
                    int ascii = (65 + ((int)value - 10));
                    if (ascii > 90)
                        ascii += 6;
                    return (char)ascii;
                }
                else
                    return value.ToString()[0];
            }
            private static long CharToVal(char value)
            {
                long chrval = (int)value;
                if (chrval > 96)
                    return (chrval - 71) + 10;
                else if (chrval > 64)
                    return (chrval - 65) + 10;
                else
                    return int.Parse(value.ToString());
            }
            private static string validate(string input, ExpandLevel ExpandBy)
            {
                string validchars = "";
                string rtnval = "";
                for (long c = 0; c < (long)ExpandBy; c++)
                    validchars += ValToChar(c);
                foreach (char i in input)
                    if (validchars.Contains(i.ToString()))
                        rtnval += i;
                return rtnval;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            //NOTE: UseCaseSensitive Level selected and the value 
            //to convert is larger that the previus example.
            string HexValue = ExpandableHexConverter.ToHex(3843,
                                  ExpandableHexConverter.ExpandLevel.UseCaseSensitive);
            Console.Write(HexValue);

            //NOTE: UseCaseSensitive Level selected and the value 
            //to convert is larger that the previus example.
            long IntValue = ExpandableHexConverter.ToInt64(HexValue,
                                  ExpandableHexConverter.ExpandLevel.UseCaseSensitive);
            Console.Write(IntValue);
            
        }
    }
}
