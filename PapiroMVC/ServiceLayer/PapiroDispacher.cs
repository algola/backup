using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.ServiceLayer
{
    public partial class PapiroService
    {
        //this event is raised afer Cost Detail is updated
        public event EventHandler<CostDetail> AfterCostDetailUpdated;

    }
}