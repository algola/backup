using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Novacode;

namespace PapiroMVC.Models
{

    public class CostDetailGrouped
    {
        public List<CostDetail> CostDetails { get; set; }
        public string CodTaskExecutorSelected { get; set; }
    }


}