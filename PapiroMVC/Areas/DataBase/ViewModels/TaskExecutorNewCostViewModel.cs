using PapiroMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PapiroMVC.Models
{
    public class TaskExecutorNewCostViewModel
    {

        Boolean isEstimatedOnMq;
        Boolean isEstimatedOnRun;
        Boolean isEstimatedOnTime;

        public string TaskExecutorName { get; set; }
        public string CodTaskExecutor { get; set; }

        public string CodTypeOfTask {get; set; }
        public string ReturnUrl { get; set; }

        public TaskEstimatedOn.EstimatedOnType TypeTaskExecutorEstimatedOn 
        {
            get
            {
                if (isEstimatedOnMq)
                    return TaskEstimatedOn.EstimatedOnType.OnMq;

                if (isEstimatedOnRun)
                    return TaskEstimatedOn.EstimatedOnType.OnRun;

                if (isEstimatedOnTime)
                    return TaskEstimatedOn.EstimatedOnType.OnTime;

                return
                    TaskEstimatedOn.EstimatedOnType.OnTime;
            }

            set
            {
                switch (value)
                {
                    case TaskEstimatedOn.EstimatedOnType.OnRun:
                        {
                            isEstimatedOnMq = false;
                            isEstimatedOnRun = true;
                            isEstimatedOnTime = false;
                            break;
                        }
                    case TaskEstimatedOn.EstimatedOnType.OnTime:
                        {
                            isEstimatedOnMq = false;
                            isEstimatedOnRun = false;
                            isEstimatedOnTime = true;
                            break;
                        }
                    case TaskEstimatedOn.EstimatedOnType.OnMq:
                        {
                            isEstimatedOnMq = true;
                            isEstimatedOnRun = false;
                            isEstimatedOnTime = false;
                            break;
                        }
                    default:
                        {
                            isEstimatedOnMq = false;
                            isEstimatedOnRun = false;
                            isEstimatedOnTime = true;
                            break;
                        }
                }
            
            }
        
        }

    }
}