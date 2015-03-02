using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(Warehouse_MetaData))]
    public partial class Warehouse
    {
        #region Proprietà aggiuntive
        public enum WarehouseType : int
        {
            WarehouseProduct = 0,
            WarehouseArticle = 1
        }

        public WarehouseType TypeOfWarehouse
        {
            get;
            protected set;
        }

        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
           return "";
        }

    }
}
