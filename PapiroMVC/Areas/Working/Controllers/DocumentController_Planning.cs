using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PapiroMVC.Models;
using Services;
using Ninject.Planning.Bindings;
using System.Web.Security;
using PapiroMVC.DbCodeManagement;
using PapiroMVC.Validation;
using Novacode;
using System.IO;
using System.Reflection;
using PapiroMVC.ServiceLayer;
using System.Threading;
using Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml.Packaging;
using System.Threading.Tasks;
using Mvc.HtmlHelpers;


namespace PapiroMVC.Areas.Working.Controllers
{



    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        [HttpParamAction]
        [HttpGet]
        public ActionResult Planning()
        {
            var x = taskCenterRepository.GetAll().OrderBy(i => i.IndexOf).ToList();
            return View(x);
        }


        public ActionResult ClearFirstTaskCenter()
        {
            var tsks = taskCenterRepository.GetAll();

            var min = tsks.Min(x => x.IndexOf);
            var max = tsks.Max(x => x.IndexOf);

            var lstDocumentTaskCenter = taskCenterRepository.GetDocumentsTaskCenter(tsks.FirstOrDefault(x => x.IndexOf == min).CodTaskCenter);

            foreach (var item in lstDocumentTaskCenter)
            {
                taskCenterRepository.DeleteDocumentTaskCenter(item);
            }


            taskCenterRepository.Save();

            return Json(new
            {
                codTaskCenter = tsks.FirstOrDefault(x => x.IndexOf == min).CodTaskCenter
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult ClearLastTaskCenter()
        {
            var tsks = taskCenterRepository.GetAll();

            var min = tsks.Min(x => x.IndexOf);
            var max = tsks.Max(x => x.IndexOf);

            var lstDocumentTaskCenter = taskCenterRepository.GetDocumentsTaskCenter(tsks.FirstOrDefault(x => x.IndexOf == max).CodTaskCenter);

            foreach (var item in lstDocumentTaskCenter)
            {
                taskCenterRepository.DeleteDocumentTaskCenter(item);
            }

            taskCenterRepository.Save();

            return Json(new
            {
                codTaskCenter = tsks.FirstOrDefault(x => x.IndexOf == max).CodTaskCenter
            }, JsonRequestBehavior.AllowGet);


        }


        public ActionResult DocumentTaskCenter(GridSettings gridSettings, string codTaskCenter)
        {

            var q = taskCenterRepository.GetDocumentsTaskCenter(codTaskCenter).OrderBy(x => x.IndexOf);
            var q3 = q.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize);

            int totalRecords = q.Count();

            // create json data
            int pageIndex = gridSettings.pageIndex;
            int pageSize = gridSettings.pageSize;

            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            int startRow = (pageIndex - 1) * pageSize;
            int endRow = startRow + pageSize;

            var jsonData = new
            {
                total = totalPages,
                page = pageIndex,
                records = totalRecords,
                rows =
                (
                    from a in q3.ToList()
                    select new
                    {
                        id = a.CodDocumentTaskCenter,
                        cell = new string[] 
                        {                       
                            a.DocumentName,
                            a.CodDocumentTaskCenter
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeTaskCenterOrder(string ids, string codTaskCenter)
        {
            string[] stringsIds = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ids);

            //IDS are codDocumentTaskCenter
            int i = 0;
            foreach (var x in stringsIds)
            {
                var c = this.taskCenterRepository.GetDocumentTaskCenter(x);
                c.IndexOf = i++;
                c.CodTaskCenter = codTaskCenter;
                taskCenterRepository.EditDocumentTaskCenter(c);

                var taskCenter = taskCenterRepository.GetSingle(codTaskCenter);
                var doc = documentRepository.GetSingle(x);
                doc.DocumentStates = documentRepository.GetAllDocumentStates(x).ToList();

                foreach (var item in doc.DocumentStates)
                {
                    item.Selected = (item.CodState == taskCenter.CodState);
                }

                documentRepository.Edit(doc);
            }

            documentRepository.Save();
            taskCenterRepository.Save();
            return null;
        }



    }


}
