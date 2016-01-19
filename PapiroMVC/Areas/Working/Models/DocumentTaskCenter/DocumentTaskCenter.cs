using System;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    using Novacode;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    public partial class DocumentTaskCenter
    {

        public virtual void MergeField(DocX doc)
        {
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentTaskCenter.DocumentName", this.DocumentName));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentTaskCenter.CodDocumentTaskCenter", this.CodDocumentTaskCenter));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentTaskCenter.FieldA", this.FieldA));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentTaskCenter.FieldB", this.FieldB));
            doc.AddCustomProperty(new Novacode.CustomProperty("DocumentTaskCenter.FieldC", this.FieldC));
        
        
        
        }

    }
}