﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        new Profile GetSingle(string name);
    }
}