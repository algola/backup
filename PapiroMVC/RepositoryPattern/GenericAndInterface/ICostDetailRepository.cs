﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ICostDetailRepository : IGenericRepository<CostDetail>
    {
        bool IsJustSaved(string id, Guid guid);
        CostDetail GetSingleSimple(string codCostDetail);
    }
}
