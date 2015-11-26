using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    public partial class TaskCenter
    {

        public string[] TaskCenters { get; set; }

    }
}
