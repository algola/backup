﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    public partial class WarehouseArticle : Warehouse
    {

        public WarehouseArticle()
        {
            TypeOfWarehouse = WarehouseType.WarehouseArticle;
        }

 
    }
}
