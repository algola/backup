﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ITypeOfTaskRepository : IGenericRepository<TypeOfTask>
    {
        new TypeOfTask GetSingle(string codTypeOfTask);
        IQueryable<TypeOfTask> GetAll(string codCustomerSupplier);
        IQueryable<OptionTypeOfTask> GetAllOptionTypeOfTask();
        OptionTypeOfTask GetSingleOptionTypeOfTask(string id);

        void EditOptionTypeOfTask(OptionTypeOfTask c);

    }

}
