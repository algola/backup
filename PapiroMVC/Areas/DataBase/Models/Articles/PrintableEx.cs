using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Validation;


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
            //LANGFILE
            var resman = new System.Resources.ResourceManager(typeof(Strings).FullName, typeof(Strings).Assembly);

            return base.ToString() + this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Weight + " " + resman.GetString("Weight");        
        }



    }
}

