
namespace PapiroMVC.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(Profile_MetaData))]
    public partial class Profile 
    {

        public override string ToString()
        {
            return "";
        }


        public string Number
        { get; set; }

        public string Month
        { get; set; }

        public string Year
        { get; set; }

        public string Cvv
        { get; set; }


    }
}
