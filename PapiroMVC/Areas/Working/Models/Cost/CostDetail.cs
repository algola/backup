using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PapiroMVC.Models
{
    //in questa viewmodel carico il costo
    //ed espongo l'eleco delle macchine che possono eseguire il task

    //se è un articolo--> ?? decider

    //    [MetadataType(typeof(TaskCostDetail_MetaData))]
    public class CostDetail
    {
        public enum CostDetailType : int
        {
            PrintingSheetCostDetail = 0,  //digital and litho sheet
        }

        public CostDetailType TypeOfCostDetail
        {
            get;
            protected set;
        }


        /// <summary>
        /// List of TaskExecutor that can be done execution
        /// </summary>
        public String CodTaskExecutorSelected { get; set; }
        public List<TaskExecutor> TaskExecutors { get; set; }
        public Cost TaskCost { get; set; }

        /// <summary>
        /// Product Part in case is a cost of ProductPart
        /// </summary>
        ///

        //protected ProductPart pPart;
        public ProductPart ProductPart
        {
            get;
            set;
        }

        public virtual void Update() 
        { }
    } 
}