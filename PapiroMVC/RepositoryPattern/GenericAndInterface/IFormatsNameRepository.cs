﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IFormatsNameRepository : IDisposable
    {
        ProductFormatName[] GetAllById(string id);
    }
}
