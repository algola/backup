using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{

    public class FormatTypeForDropDown
    {
        public string FormatTypeName { get; set; }
        public int FormatType { get; set; }
    }

    public class DescForDropDown
    {
        public string Name { get; set; }
        public int Cod { get; set; }
    }

    [Serializable]
    // [MetadataType(typeof(Die_MetaData))]
    public partial class Die : NoPrintable
    {
        public Die()
        {
            TypeOfArticle = ArticleType.DieSheet;
        }

        #region Added Properties

        #endregion

        public override string ToString()
        {
            return base.ToString() + this.ArticleName;
        }


        public Nullable<bool> ZMetric { get; set; }

        public int GetZFromCm(double cm)
        {
            int ret;

            if (!(ZMetric ?? false))
            {
                ret = Convert.ToInt32(((Convert.ToDouble(cm) * 8) / 2.54));
            }
            else
            {
                ret = Convert.ToInt32((Convert.ToDouble(cm)) / 3.1415 * 10);
            }

            return ret;
        }

        public double GetCmFromZ(int z)
        {
            double ret;

            if (!(ZMetric ?? false))
            {
                ret = (Convert.ToDouble(z) / 8) * 2.54;
            }
            else
            {
                ret = (Convert.ToDouble(z)) * 3.1415 / 10;
            }

            return ret;
        }
    }
}

