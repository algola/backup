using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [MetadataType(typeof(Printable_MetaData))]
    abstract public partial class Printable : Article, ICloneable, IDeleteRelated
    {
       
        #region Added Properties

        #endregion

        #region Handle copy for modify

        public override void Copy(Article to)
        {
            //All properties of object
            //and pointer of sons
            base.Copy(to);

            ((Printable)to).TypeOfMaterial = this.TypeOfMaterial;
            ((Printable)to).NameOfMaterial = this.NameOfMaterial;
            ((Printable)to).Thikness = this.Thikness; 

            ((Printable)to).Color = this.Color;
            ((Printable)to).Weight = this.Weight;
            ((Printable)to).Hand = this.Hand;


            //to.Quantita = this.Quantita;
            //to.Prezzo = this.Prezzo;
            //to.Descrizione = this.Descrizione;
        }

        #endregion


        public override string ToString()
        {
            return base.ToString() + this.TypeOfMaterial + " " + this.NameOfMaterial + " " + this.Weight + " ";
        }

    }
}

