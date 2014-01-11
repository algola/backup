using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Printable_MetaData))]
    abstract public partial class Printable : Article
    {
       
        #region Added Properties

        #endregion


        public override string ToString()
        {
            return base.ToString() + this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Weight + " ";
        }

    }
}

