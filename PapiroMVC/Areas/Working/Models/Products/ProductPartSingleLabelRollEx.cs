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
    
    [KnownType(typeof(ProductPartSingleLabelRoll))]
    [MetadataType(typeof(ProductPartSingleLabelRoll_MetaData))]
    public partial class ProductPartSingleLabelRoll : ProductPart, IPrintDocX
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

        public override void MergeField(DocX doc)
        {
            base.MergeField(doc);

            doc.AddCustomProperty(new Novacode.CustomProperty("ProductPart.LabelsPerRoll", this.LabelsPerRoll??0));
            doc.AddCustomProperty(new Novacode.CustomProperty("ProductPart.SoulDiameter", this.SoulDiameter ?? 0));
            doc.AddCustomProperty(new Novacode.CustomProperty("ProductPart.MaxDiameter", this.MaxDiameter ?? 0));
        }
        #region Proprietà aggiuntive
        #endregion

    }
}
