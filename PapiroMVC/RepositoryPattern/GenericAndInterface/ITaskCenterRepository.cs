using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapiroMVC.Models;

namespace Services
{
    public interface ITaskCenterRepository : IGenericRepository<TaskCenter>
    {
        string GetNewCode(TaskCenter c);
        new TaskCenter GetSingle(string codTaskCenter);
        IQueryable<DocumentTaskCenter> GetDocumentsTaskCenter(string codTaskCenter);
        DocumentTaskCenter GetDocumentTaskCenter(string codDocumentTaskCenter);

        void AddNewDocumentTaskCenter(DocumentTaskCenter entity);
        void EditDocumentTaskCenter(DocumentTaskCenter entity);

        void DeleteDocumentTaskCenter(DocumentTaskCenter entity);

    }
}
