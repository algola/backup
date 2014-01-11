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
    }

}