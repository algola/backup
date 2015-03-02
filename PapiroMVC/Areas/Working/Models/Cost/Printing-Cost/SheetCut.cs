﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{

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

        public string GetCuttedFormat(string format)
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


    //public class CutForLabel
    //{
    //    /// <summary>
    //    /// this cut fits in taskexecutor
    //    /// </summary>
    //    public bool Valid { get; set; }

    //    public String CodCutForLabel { get; set; }

    //    /// <summary>
    //    /// it is exposed on views
    //    /// </summary>
    //    public String CutName { get; set; }
    //    public static T Max<T>(T x, T y)
    //    {
    //        return (Comparer<T>.Default.Compare(x, y) > 0) ? x : y;
    //    }
    //    public static T Min<T>(T x, T y)
    //    {
    //        return (Comparer<T>.Default.Compare(x, y) < 0) ? x : y;
    //    }

    //    public CutForLabel(String desc, int z)
    //    {
    //        this.CodCutForLabel = desc;
    //        Z = z / 8 * 2.54;
    //    }

    //    double Z { get; set; }
    //    public String ManualFormat { get; set; }

    //    public string GetCuttedFormat(string width)
    //    {
    //        string result;
    //        result = width + "x" + Z.ToString("0.#");
    //        return (ManualFormat == null || ManualFormat == String.Empty) ? result : ManualFormat;
    //    }
    //}


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
            return cuts.Values.OrderByDescending(x => x.CutName).ToList();
        }

        public static bool IsValid(string maxFormat, string minFormat, string ftoRes)
        {
            return (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2());
        }

        public static List<Cut> Cuts(string buyingFormat, string maxFormat, string minFormat, bool noCuts = false)
        {
            foreach (var item in cuts.Values)
            {
                var ftoRes = item.GetCuttedFormat(buyingFormat);
                item.Valid = (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2()); ;
            }

            if (noCuts)
            {
                //if is noCut returns only key=ct0-0 that rappresent no cut
                return cuts.Where(x => x.Key == "ct0-0").ToDictionary(y => y.Key, g => g.Value).Values.ToList();                
            }
            else
            {
                return cuts.Values.ToList();
            }
        }

        //calcola in base al codice di taglio il formato tagliato
        public static String CuttedFormat(string format, string codCut)
        {
            var x = String.Empty;
            try
            {
                x = cuts[codCut].GetCuttedFormat(format);
            }
            catch (Exception)
            {
                x = format;
            }
            return x;
        }
    }


    //public static class LabelCut
    //{
    //    private static Dictionary<String, CutForLabel> cuts;

    //    public static List<int> Zs
    //    {
    //        set
    //        {
    //            cuts = new Dictionary<String, CutForLabel>();

    //            int i = 0;
    //            foreach (var z in value)
    //            {
    //                cuts.Add("ct0-0", new CutForLabel("ct1-" + (i++).ToString(), z));
    //            }
    //        }             
    //    }
        
    //    static LabelCut()
    //    {
    //    }

    //    public static List<CutForLabel> Cuts()
    //    {
    //        return cuts.Values.OrderByDescending(x => x.CutName).ToList();
    //    }

    //    public static bool IsValid(string maxFormat, string minFormat, string ftoRes)
    //    {
    //        return true;
    //        //return (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
    //        //    (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2());
    //    }

    //    public static List<CutForLabel> Cuts(string width, string maxFormat, string minFormat)
    //    {
    //        foreach (var item in cuts.Values)
    //        {
    //            var ftoRes = item.GetCuttedFormat(width);
    //            item.Valid = true;// (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
    //            //(ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2()); ;
    //        }

    //        return cuts.Values.ToList();
    //    }

    //    //calcola in base al codice di taglio il formato tagliato
    //    public static String CuttedFormat(string width, string codCut)
    //    {
    //        var x = String.Empty;
    //        try
    //        {
    //            x = cuts[codCut].GetCuttedFormat(width);
    //        }
    //        catch (Exception)
    //        {
    //            x = width;
    //        }
    //        return x;
    //    }
    //}
}