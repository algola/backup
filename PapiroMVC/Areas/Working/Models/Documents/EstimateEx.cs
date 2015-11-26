using Novacode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{

    public partial class Estimate : Document
    {
        public Estimate()
        {
            TypeOfDocument = DocumentType.Estimate;
        }

        public override void MergeField(DocX doc)
        {
            doc.AddCustomProperty(new Novacode.CustomProperty("CustomerBusinessName", this.CustomerSupplier.BusinessName));

            doc.AddCustomProperty(new Novacode.CustomProperty("EstimateNumber", this.EstimateNumber));
            doc.AddCustomProperty(new Novacode.CustomProperty("EstimateNumberSerie", this.EstimateNumberSerie));

            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentName", this.DocumentName));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentDate", (this.DateDocument ?? DateTime.Now).ToString("d")));
        }

    }

}