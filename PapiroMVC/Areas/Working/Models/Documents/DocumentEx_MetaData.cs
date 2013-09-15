using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using PapiroMVC.Models.Resources.Document;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    public abstract partial class Document_MetaData
    {

        [DisplayNameLocalized(typeof(ResDocuments), "CodDocument")]
        [Tooltip(typeof(ResDocuments), "CodDocumentToolTip")]
        public string CodDocument { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "DocumentName")]
        [Tooltip(typeof(ResDocuments), "DocumentNameToolTip")]
        public string DocumentName { get; set; }


        [DisplayNameLocalized(typeof(ResDocuments), "CodCustomer")]
        [Tooltip(typeof(ResDocuments), "CodCustomerToolTip")]
        public string CodCustomer { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "Customer")]
        [Tooltip(typeof(ResDocuments), "CustomerToolTip")]
        public string Customer { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "DateDocument")]
        [Tooltip(typeof(ResDocuments), "DateDocumentToolTip")]
        public Nullable<int> DateDocument { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "Number")]
        [Tooltip(typeof(ResDocuments), "NumberToolTip")]
        public Nullable<int> Number { get; set; }

        [DisplayNameLocalized(typeof(ResDocuments), "Notes")]
        [Tooltip(typeof(ResDocuments), "NotesToolTip")]
        public string Notes { get; set; }



    }
}