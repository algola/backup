using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public partial class MakereadyPrintingBookSheet : Makeready
    {
        /// <summary>
        /// is the number of the shapes that page has to draw
        /// </summary>
        public int PrintablePages { get; set; }
        public int PrintedPages { get; set; }

        public MakereadyPrintingBookSheet()
        {
            Signatures = new Signature[6];
            Signatures[0] = new Signature(4, "quartino");
            Signatures[1] = new Signature(8, "ottavo");
            Signatures[2] = new Signature(12, "dodicesimo");
            Signatures[3] = new Signature(16, "sedicesimo");
            Signatures[4] = new Signature(24, "ventiquattresimo");
            Signatures[5] = new Signature(32, "trentaduesimo");
        }

        public Signature[] Signatures { get; set; } //each element has the name

        public int CodMaxSegn { get; set; }

        public void UpdateSignatures()
        {
            if (CodMaxSegn == 0)
            {
                CodMaxSegn = 32;
            }

            //1 calcolo la tipologia di segnature che stampo con un foglio macchina
            Signatures = Signatures.Where(x => x.CodSegn <= CodMaxSegn).ToArray();

            //used for calcolus
            var printedPages = PrintedPages;

            while (printedPages > 0)
            {
                for (int i = Signatures.Count() - 1; i >= 0; i--)
                {
                    var currSegn = Signatures[i].CodSegn;
                    if (printedPages >= currSegn)
                    {
                        var segns = (int)decimal.Truncate(printedPages / currSegn);
                        Signatures[i].Count = segns;
                        printedPages = printedPages % currSegn;
                    }
                }
            }

        }

    }


}