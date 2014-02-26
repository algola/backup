using System;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{
    [DataContract]
    public partial class ProductFormatName
    {
        [DataMember]
        public String CodFormat { get; set; }

        [DataMember]
        public String FormatName { get; set; }
    }
}
