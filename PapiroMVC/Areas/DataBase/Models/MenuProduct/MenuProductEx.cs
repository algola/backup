using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PapiroMVC.Models
{
    [MetadataType(typeof(MenuProduct_MetaData))]
    public partial class MenuProduct
    {

        public string Name { get; set; }

        public bool IsSelected
        {
            get;
            set;
        }
    }

    //Order By 
    public struct MyStruct
    {
        public String CodCategory { get; set; }
        public String IndexOfCategory { get; set; }
    };
}