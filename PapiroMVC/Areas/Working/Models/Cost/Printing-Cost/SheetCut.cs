using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    //Extension methods must be defined in a static class
    public static class StringExtension
    {
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

    public class Cut
    {
        /// <summary>
        /// this cut fits in taskexecutor
        /// </summary>
        public bool Valid { get; set; }

        public String CodCut { get; set; }

        /// <summary>
        /// it is exposed on views
        /// </summary>
        public String CutName { get; set; }
        public static T Max<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) > 0) ? x : y;
        }
        public static T Min<T>(T x, T y)
        {
            return (Comparer<T>.Default.Compare(x, y) < 0) ? x : y;
        }

        public Cut(String desc, byte longSide, byte shortSide)
        {
            this.CodCut = desc;
            LongSide = longSide;
            ShortSide = shortSide;
        }

        byte LongSide { get; set; }
        byte ShortSide { get; set; }
        public String ManualFormat { get; set; }

        public string GetCuttetFormat(string format)
        {
            string result;

            double side1 = format.GetSide1();
            double side2 = format.GetSide2();

            if (side1 < side2)
            {
                var s1 = ShortSide > 0 ? side1 / ShortSide : side1;
                var s2 = LongSide > 0 ? side2 / LongSide : side2;

                result = s1 < s2 ? s1.ToString("0.#") + "x" + s2.ToString("0.#") : s2.ToString("0.#") + "x" + s1.ToString("0.#");
            }
            else
            {
                var s1 = LongSide > 0 ? side1 / LongSide : side1;
                var s2 = ShortSide > 0 ? side2 / ShortSide : side2;

                result = s1 < s2 ? s1.ToString("0.#") + "x" + s2.ToString("0.#") : s2.ToString("0.#") + "x" + s1.ToString("0.#");
            }

            return (ManualFormat == null || ManualFormat == String.Empty) ? result : ManualFormat;
        }

    }

    public static class SheetCut
    {
        private static Dictionary<String, Cut> cuts;
        static SheetCut()
        {
            cuts = new Dictionary<String, Cut>();
            cuts.Add("ct0-0", new Cut("ct1-0", 0, 0)); //70x100
            cuts.Add("ct2-0", new Cut("ct1-1", 2, 0)); //50x70
            cuts.Add("ct2-2", new Cut("ct2-0", 2, 2)); //35x50
            cuts.Add("ct3-0", new Cut("ct3-0", 3, 0)); //33,3x70
            cuts.Add("ct3-2", new Cut("ct3-2", 3, 2)); //33,3x35
            cuts.Add("ct3-3", new Cut("ct3-3", 3, 3)); //23,3x33,3

            cuts.Add("ct4-2", new Cut("ct4-2", 4, 2)); //25x35
            cuts.Add("ct4-3", new Cut("ct4-3", 4, 3)); //23,3x25

            cuts.Add("ct5-2", new Cut("ct5-2", 5, 2)); //20x35
            cuts.Add("ct4-4", new Cut("ct4-4", 4, 4)); //17,5x25
            cuts.Add("ct5-4", new Cut("ct5-4", 5, 4)); //17,5x20
        }

        public static List<Cut> Cuts()
        {
            return cuts.Values.ToList();
        }

        public static bool IsValid(string maxFormat, string minFormat, string ftoRes)
        {
            return (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2());
        }

        public static List<Cut> Cuts(string buyingFormat, string maxFormat, string minFormat)
        {
            foreach (var item in cuts.Values)
            {
                var ftoRes = item.GetCuttetFormat(buyingFormat);
                item.Valid = (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2()); ;
            }

            return cuts.Values.ToList();
        }

        //calcola in base al codice di taglio il formato tagliato
        public static String CuttedFormat(string format, string codCut)
        {
            var x = String.Empty;
            try
            {
                x = cuts[codCut].GetCuttetFormat(format);
            }
            catch (Exception)
            {
                x = format;
            }
            return x;
        }
    }
}