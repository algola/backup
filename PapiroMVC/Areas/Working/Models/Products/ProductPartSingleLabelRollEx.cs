using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    
    [KnownType(typeof(ProductPartSingleLabelRoll))]
    [MetadataType(typeof(ProductPartSingleLabelRoll_MetaData))]
    public partial class ProductPartSingleLabelRoll : ProductPart
    {

        public override string ToString()
        {

            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProduct);

            String s = String.Empty;

            if ((FormatOpened ?? Format) == Format)
            {
                s = (string)t.GetProperty("FormatOnly").GetValue(null, null) + " " + FormatOpened;
            }
            else
            {
                s = (string)t.GetProperty("FormatOpened").GetValue(null, null) + " " + FormatOpened + " " +
                   (string)t.GetProperty("Format").GetValue(null, null) + " " + Format;
            }

            return s + " " +
                base.ToString();
        }

        public ProductPartSingleLabelRoll()
        {
            TypeOfProductPart = ProductPart.ProductPartType.ProductPartSingleLabelRoll;
        }

        #region Proprietà aggiuntive
        #endregion

    }
}
