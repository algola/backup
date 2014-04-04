
namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Profile_MetaData))]
    public partial class Profile 
    {

        public bool CardIsValid { get; set; }

        public override string ToString()
        {
            return "";
        }

    }
}
