using Novacode;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace PapiroMVC.Models
{
    [Serializable]
    [MetadataType(typeof(TaskCenter_MetaData))]
    public partial class TaskCenter
    {

        public string[] TaskCenters { get; set; }

        string stateName = "";
        public string StateName
        {

            get
            {
                if (State != null)
                {
                    stateName = State.StateName;
                }
                return stateName;

            }
            set
            {
                stateName = value;
            }

        }


        public virtual void MergeField(DocX doc)
        {
            doc.AddCustomProperty(new Novacode.CustomProperty("CodTaskCenter", this.CodTaskCenter));
            doc.AddCustomProperty(new Novacode.CustomProperty("TaskCenterName", this.TaskCenterName));
        }


    }
}
