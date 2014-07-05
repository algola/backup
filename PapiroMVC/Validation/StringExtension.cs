using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace PapiroMVC.Models
{

    //Extension methods must be defined in a static class
    public static class StringExtension
    {
        public static string PurgeFileName(this string filename)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(c, '-');
            }

            return filename;

        }




        // This is the extension method.
        // The first parameter takes the "this" modifier
        // and specifies the type for which the method is defined.
        public static string Format(this string str)
        {
            return "";
        }

        public static double GetSide1(this string format)
        {
            format = format == null ? "0x0" : format;

            Regex pattern = new Regex(@"^(?<side1>(\d{1,4})((\,\d{0,5}){0,1}))[xX](?<side2>(\d{1,4})((\,\d{0,5}){0,1})?$)");
            Match match = pattern.Match(format);
            return double.Parse(match.Groups["side1"].Value);
        }
        public static double GetSide2(this string format)
        {
            format = format == null ? "0x0" : format;

            Regex pattern = new Regex(@"^(?<side1>(\d{1,4})((\,\d{0,5}){0,1}))[xX](?<side2>(\d{1,4})((\,\d{0,5}){0,1})?$)");
            Match match = pattern.Match(format);
            return double.Parse(match.Groups["side2"].Value);
        }
    }

}