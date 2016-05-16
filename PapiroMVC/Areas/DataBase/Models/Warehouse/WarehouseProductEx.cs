using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    public partial class WarehouseProduct : WarehouseItem
    {

        public WarehouseProduct()
        {
            TypeOfWarehouse = WarehouseType.WarehouseProduct;
        }

 
    }
}
