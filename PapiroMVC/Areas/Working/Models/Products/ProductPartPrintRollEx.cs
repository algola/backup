using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using PapiroMVC.Models.Resources.Products;
using PapiroMVC.Views.Shared.App_LocalResources;
using System.Reflection;
using System.Runtime.Serialization;

namespace PapiroMVC.Models
{

    [MetadataType(typeof(ProductPartPrintRoll_Metadata))]
    public partial class ProductPartPrintRoll : ProductPartTask, IDataErrorInfo, ICloneable
    {

        public ProductPartPrintRoll()
        {
            TypeOfProductPartTask = ProductPartTasksType.ProductPartPrintRoll;
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public object Clone()
        {
            //creo una copia dell'oggetto da utilizzare per le modifiche
            var kindOfObject = this.GetType();

            //istanzio una copia che sarà gestita dall'invio
            ProductPartPrintRoll copyOfObject = (ProductPartPrintRoll)Activator.CreateInstance(kindOfObject);
            this.Copy(copyOfObject);

            return copyOfObject;
        }

        public override void Copy(ProductPartTask to)
        {
            base.Copy(to);
            ((ProductPartPrintRoll)to).PrintSide = this.PrintSide;
            ((ProductPartPrintRoll)to).ColorFormulation = this.ColorFormulation;
        }

        private bool _retro;
        public bool Retro
        {
            get
            {
                return _retro;
            }
            set
            {
                _retro = value;
            }
        }

        private bool _vernice;
        public bool Vernice
        {
            get
            {
                return _vernice;
            }
            set
            {
                _vernice = value;
            }
        }

        public override string ToString()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);
            var ext = (string)t.GetProperty("PrintSide" + (this.PrintSide ?? 0).ToString()).GetValue(null, null);

            return base.ToString() + (ext == "" ? ext : " " + ext);
        }

        public override string ToStringInfo()
        {
            Type t = typeof(PapiroMVC.Models.Resources.Products.ResProductPartTask);
            var col = (string)t.GetProperty("ColorFormulation" + (this.ColorFormulation ?? 0).ToString()).GetValue(null, null);

            return (col == "" ? col : col + " ") + ToString();
        }

    }



}
