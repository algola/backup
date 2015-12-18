﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(WarehouseSpec_MetaData))]
    public partial class WarehouseSpec
    {
        #region Proprietà aggiuntive
        #endregion

        public bool IsSelected
        {
            get;
            set;
        }

        public override string ToString()
        {
           return this.WarehouseName;
        }

    }
}