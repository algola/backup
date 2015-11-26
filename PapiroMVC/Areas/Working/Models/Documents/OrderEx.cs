using Novacode;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{


    public class ReportOrderName
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

  

    [MetadataType(typeof(Order_MetaData))]
    public partial class Order : Document
    {
        public Order()
        {
            TypeOfDocument = DocumentType.Order;
        }

        ///
        public IQueryable<ReportOrderName> ReportOrderNames { get; set; }

        public override void MergeField(DocX doc)
        {

            doc.AddCustomProperty(new Novacode.CustomProperty("CustomerBusinessName", this.CustomerSupplier.BusinessName));

            doc.AddCustomProperty(new Novacode.CustomProperty("OrderNumber", OrderNumber));
            doc.AddCustomProperty(new Novacode.CustomProperty("OrderNumberSerie", OrderNumberSerie));

            doc.AddCustomProperty(new Novacode.CustomProperty("EstimateNumber", this.OrderProduct.Document.EstimateNumber));
            doc.AddCustomProperty(new Novacode.CustomProperty("EstimateNumberSerie", this.OrderProduct.Document.EstimateNumberSerie));

            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentName", this.DocumentName));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentDate", (this.DateDocument ?? DateTime.Now).ToString("d")));
        }

    }

}