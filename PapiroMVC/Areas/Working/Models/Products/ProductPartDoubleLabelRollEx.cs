using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Novacode;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartDoubleLabelRoll))]
    [MetadataType(typeof(ProductPartDoubleLabelRoll_MetaData))]
    public partial class ProductPartDoubleLabelRoll : ProductPart, IPrintDocX
    {

        public override List<PrintingHint> SelectValidpHint(List<PrintingHint> pHint, double smallerCalculatedDCutLessZero, double smallerCalculatedDCut)
        {
            List<PrintingHint> pHint1;

            pHint1 = pHint.Where(x => x.DCut2 / 2 >= this.MinDCut
                    && x.DCut2 / 2 <= this.MaxDCut && (x.DCut1 >= x.DCut2 / 2 || (x.DCut1 == 0 && x.MaxGain1 == 1))).ToList();

            if (pHint1.Count <= 1)
            {
                //fascette gommate
                if (MinDCut == 0)
                {
                    var pHint2 = pHint.Where(x => x.DCut2/2 == smallerCalculatedDCutLessZero * -1);
                    pHint1 = pHint.Where(x => x.DCut2/2 >= 0 && x.DCut2/2 <= smallerCalculatedDCut && (x.DCut1 >= x.DCut2/2 || x.DCut1 == 0)).ToList();
                    var pHint3 = pHint1.Union(pHint2);
                    pHint1 = pHint3.ToList();
                }
                else
                {
                    var smaller = pHint.Where(x => (x.DCut1 >= x.DCut2/2) || x.DCut1 == 0).Select(x => x.DeltaDCut2).Min();      
                    var pHintLast1 = pHint.Where(x => x.DeltaDCut2 == smaller && (x.DCut1 >= x.DCut2/2 || x.DCut1 == 0));

                    try
                    {
                        var smaller2 = pHint.Where(x => (x.DCut1 >= x.DCut2/2 || x.DCut1 == 0) && x.DeltaDCut2 != smaller).Select(x => x.DeltaDCut2).Min();
                        var pHintLast2 = pHint.Where(x => x.DeltaDCut2 == smaller2 && x.DeltaDCut2 != smaller && (x.DCut1 >= x.DCut2/2 || x.DCut1 == 0));

                        pHint1 = pHintLast1.Union(pHintLast2).ToList();

                    }
                    catch (Exception)
                    {
                        pHint1 = pHintLast1.ToList();

                    }
                    //                    pHint1 = pHint.Where(x => x.DCut2/2 >= 0 && x.DCut2/2 <= smallerCalculatedDCut && (x.DCut1 >= x.DCut2/2 || x.DCut1 == 0)).ToList();
                }
            }




            return pHint1;
       
        }

        public override string Formatmm
        {
            get
            {
                return "";
            }
            set
            {
                base.Formatmm = value;
            }
        }


        //formato in mm
        public virtual String FormatAmm
        {
            get
            {
                if (FormatA == null)
                {
                    return null;
                }
                else
                {
                    return (FormatA.GetSide1() * 10).ToString() + "x" + (FormatA.GetSide2() * 10).ToString();
                }
            }

            set
            {
                try
                {
                    FormatA = (value.GetSide1() / 10).ToString() + "x" + (value.GetSide2() / 10).ToString();
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }



        //formato in mm
        public virtual String FormatBmm
        {
            get
            {
                if (FormatB == null)
                {
                    return null;
                }
                else
                {
                    return (FormatB.GetSide1() * 10).ToString() + "x" + (FormatB.GetSide2() * 10).ToString();
                }
            }

            set
            {
                try
                {
                    FormatB = (value.GetSide1() / 10).ToString() + "x" + (value.GetSide2() / 10).ToString();
                }
                catch (Exception)
                {
                    throw;
                }

            }
        }


        public override void UpdateOpenedFormat()
        {
            var aSide1 = this.FormatA.GetSide1();
            var aSide2 = this.FormatA.GetSide2();

            var bSide1 = this.FormatB.GetSide1();
            var bSide2 = this.FormatB.GetSide2();

            //merge FormatA and FormatB
            FormatOpened = Math.Max(aSide1, bSide1).ToString("0.##") + "x" + (aSide2 + bSide2).ToString("0.##");

            Format = FormatA + " + " + FormatB; 


        }


        //***RIGUARDARE
        public override string ToString()
        {

            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);

            String s = String.Empty;

           s = (string)t.GetProperty("FormatA").GetValue(null, null) + " " + FormatA;
           s +=  " " + (string)t.GetProperty("FormatB").GetValue(null, null) + " " + FormatB;
           
            return s + " " +
                base.ToString();
        }

        public ProductPartDoubleLabelRoll()
        {
            TypeOfProductPart = ProductPart.ProductPartType.ProductPartDoubleLabelRoll;
        }

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);

            doc.AddCustomProperty(new Novacode.CustomProperty("ProductPart.FormatA", this.FormatA));
            doc.AddCustomProperty(new Novacode.CustomProperty("ProductPart.FormatB", this.FormatB));
        }
        #region Proprietà aggiuntive
        #endregion


        public override void ToName()
        {
            var x = Product.ProductNameGenerator;
            x = x.Replace("%PARTFORMATOPENAMMSIDE1", this.FormatAmm.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENAMMSIDE2", this.FormatAmm.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPENAMM", this.FormatAmm);

            x = x.Replace("%PARTFORMATOPENBMMSIDE1", this.FormatBmm.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENBMMSIDE2", this.FormatBmm.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPENBMM", this.FormatBmm);

            x = x.Replace("%PARTFORMATOPENASIDE1", this.FormatA.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENASIDE2", this.FormatA.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPENA", this.FormatA);

            x = x.Replace("%PARTFORMATOPENBSIDE1", this.FormatB.GetSide1().ToString());
            x = x.Replace("%PARTFORMATOPENBSIDE2", this.FormatB.GetSide2().ToString());
            x = x.Replace("%PARTFORMATOPENB", this.FormatB);

            base.ToName();
        }

    }
}
