using System;
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

        public Cut(String desc, double cutOnSide2, double cutOnSide1)
        {
            this.CodCut = desc;
            PartsOnSide2 = cutOnSide2;
            PartsOnSide1 = cutOnSide1;

            if (PartsOnSide1 == 0)
            {
                PartsOnSide1 = 1;
            }

            if (PartsOnSide2 == 0)
            {
                PartsOnSide2 = 1;
            }

        }

        public double PartsOnSide2 { get; set; }
        public double PartsOnSide1 { get; set; }
        public String ManualFormat { get; set; }

        public double Gain
        {
            get
            {
                var den = (PartsOnSide1) * (PartsOnSide2);
                var res = den != 0 ? 1 / den : 1;
                return res;
            }
        }

        public string GetCuttedFormat(string format, bool maxLast = true)
        {
            string result;

            double side1 = format.GetSide1();
            double side2 = format.GetSide2();

            //if (side1 < side2)
            //{
            var s1 = PartsOnSide1 > 0 ? side1 / PartsOnSide1 : side1;
            var s2 = PartsOnSide2 > 0 ? side2 / PartsOnSide2 : side2;


            if (!maxLast)
            {
                result = s1.ToString("0.##") + "x" + s2.ToString("0.##");
            }
            else
            {
                result = (s1 < s2) ? s1.ToString("0.##") + "x" + s2.ToString("0.##") : s2.ToString("0.##") + "x" + s1.ToString("0.##");
            }

            //            result = (s1 < s2 && !maxLast) ? s1.ToString("0.##") + "x" + s2.ToString("0.##") : s2.ToString("0.##") + "x" + s1.ToString("0.##");
            //}
            //else
            //{
            //    var s1 = CutOnSide2 > 0 ? side1 / CutOnSide2 : side1;
            //    var s2 = CutOnSide1 > 0 ? side2 / CutOnSide1 : side2;

            //    result = s1 < s2 ? s1.ToString("0.#") + "x" + s2.ToString("0.#") : s2.ToString("0.#") + "x" + s1.ToString("0.#");
            //}

            return (ManualFormat == null || ManualFormat == String.Empty) ? result : ManualFormat;
        }
    }

    public static class SheetCut
    {
        private static Dictionary<String, Cut> cuts;
        static SheetCut()
        {
        }

        public static List<Cut> Cuts()
        {
            return cuts.Values.OrderByDescending(x => x.CutName).ToList();
        }

        public static bool IsValid(string maxFormat, string minFormat, string ftoRes)
        {

            if ((maxFormat.GetSide1() + maxFormat.GetSide2()) == 0 && (minFormat.GetSide1() + minFormat.GetSide2()) == 0)
            {
                return true;
            }

            return (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2());


            //var resMin = (ftoRes.GetSide1() <= maxFormat.GetSide1() &&
            //    ftoRes.GetSide2() <= maxFormat.GetSide2()) ||
            //    (ftoRes.GetSide2() <= maxFormat.GetSide1() &&
            //    ftoRes.GetSide1() <= maxFormat.GetSide2());


            //var resMax = (ftoRes.GetSide1() >= minFormat.GetSide1() &&
            //            ftoRes.GetSide2() >= minFormat.GetSide2()) ||
            //            (ftoRes.GetSide1() >= minFormat.GetSide1() &&
            //            ftoRes.GetSide2() >= minFormat.GetSide2());

            //return resMin && resMax;

        }


        public static bool IsValidOnMax(string maxFormat, string minFormat, string ftoRes)
        {

            if ((maxFormat.GetSide1() + maxFormat.GetSide2()) == 0 && (minFormat.GetSide1() + minFormat.GetSide2()) == 0)
            {
                return true;
            }

            return (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2());
        }

        public static List<Cut> Cuts(string buyingFormat, string maxFormat, string minFormat, bool noCuts = false, int gain2 = 0, int gain1 = 0, bool wantOnlyPair = false)
        {

            cuts = new Dictionary<String, Cut>();
            cuts.Add("ct1-1", new Cut("ct1-0", 1, 1)); //70x100
            cuts.Add("ct2-1", new Cut("ct1-1", 2, 1)); //50x70
            cuts.Add("ct2-2", new Cut("ct2-0", 2, 2)); //35x50
            cuts.Add("ct3-0", new Cut("ct3-0", 3, 1)); //33,3x70
            cuts.Add("ct3-2", new Cut("ct3-2", 3, 2)); //33,3x35
            cuts.Add("ct3-3", new Cut("ct3-3", 3, 3)); //23,3x33,3

            cuts.Add("ct4-2", new Cut("ct4-2", 4, 2)); //25x35
            cuts.Add("ct4-3", new Cut("ct4-3", 4, 3)); //23,3x25

            cuts.Add("ct5-2", new Cut("ct5-2", 5, 2)); //20x35
            cuts.Add("ct4-4", new Cut("ct4-4", 4, 4)); //17,5x25
            cuts.Add("ct5-4", new Cut("ct5-4", 5, 4)); //17,5x20


            //if i have to cut on gain!!!
            if (gain2 != 0)
            {
                cuts = new Dictionary<String, Cut>();
                for (int i = gain2; i > 0; i--)
                {
                    var res2 = i != 0 ? (double)gain2 / (double)i : 1;

                    for (int k = gain1; k > 0; k--)
                    {
                        if (!wantOnlyPair || ((k == 0 || gain1 % k == 0) && ((i == 0) || gain2 % i == 0)))
                        {

                            var res1 = k != 0 ? (double)gain1 / (double)k : 1;
                            cuts.Add("ct" + k.ToString() + "-" + i.ToString(),
                                new Cut("ct1-" + k.ToString() + "-" + i.ToString(), res2 != 0 ? res2 : 1, res1 != 0 ? res1 : 1));
                        }
                    }
                }
            }

            foreach (var item in cuts.Values)
            {
                var ftoRes = item.GetCuttedFormat(buyingFormat);
                item.Valid = (ftoRes.GetSide1() <= maxFormat.GetSide1() && ftoRes.GetSide2() <= maxFormat.GetSide2()) &&
                (

                (ftoRes.GetSide1() >= minFormat.GetSide1() && ftoRes.GetSide2() >= minFormat.GetSide2() ||
                (ftoRes.GetSide2() >= minFormat.GetSide1() && ftoRes.GetSide1() >= minFormat.GetSide2())

                )); ;

                Console.WriteLine(item.Valid);

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


}