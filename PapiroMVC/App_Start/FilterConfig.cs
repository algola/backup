﻿using PapiroMVC.Validation;
using PapiroMVC.Validation.Error;
using System;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute
            {
                ExceptionType = typeof(NotImplementedException),
                View = "NotImplementedException"
            }, 1);

            filters.Add(new CustomHandleErrorAttribute
            {
                ExceptionType = typeof(ZeroGainException),
                View = "ZeroGainException"
            }, 1);

            filters.Add(new CustomHandleErrorAttribute
            {
                ExceptionType = typeof(Exception),
                View = "ErrorCust"
            }, 1);

            filters.Add(new CustomHandleErrorAttribute
            {
                ExceptionType = typeof(NoTaskEstimatedOnException),
                View = "NoTaskEstimatedOnException"
            }, 1);

            filters.Add(new CustomHandleErrorAttribute
            {
                ExceptionType = typeof(NotFoundResException),
                View = "NotFoundResException"
            }, 1);

        }
    }
}