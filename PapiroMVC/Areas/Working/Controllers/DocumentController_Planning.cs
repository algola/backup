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
using Microsoft.AspNet.SignalR;
using PapiroMVC.Hubs;
using System.Globalization;


namespace PapiroMVC.Areas.Working.Controllers
{



    public partial class DocumentController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        [HttpParamAction]
        [HttpGet]
        public ActionResult Planning(bool isView=true)
        {
            ViewBag.isView = isView;
            ViewBag.groupName = CurrentDatabase;
            var x = taskCenterRepository.GetAll().OrderBy(i => i.IndexOf).ToList();
            return View(x);
        }

        public ActionResult PlanningDetail(string id)
        {
            ViewBag.groupName = CurrentDatabase;
            var tskCenter = taskCenterRepository.GetAll().FirstOrDefault(x => x.CodTaskCenter == id);
            return View(tskCenter);
        }

        public ActionResult ClearTaskCenter(string id)
        {
            var tsks = taskCenterRepository.GetAll();

            var lstDocumentTaskCenter = taskCenterRepository.GetDocumentsTaskCenter(id);

            foreach (var item in lstDocumentTaskCenter)
            {
                taskCenterRepository.DeleteDocumentTaskCenter(item);
            }

            taskCenterRepository.Save();

            return Json(new
            {
                codTaskCenter = id
            }, JsonRequestBehavior.AllowGet);

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

        /// <summary>
        /// reset when drop another taskcenter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult ResetFinishedDocumentTask(string id)
        {
            var lstDocumentTaskCenter = taskCenterRepository.GetDocumentsTaskCenter("");
            var c = lstDocumentTaskCenter.FirstOrDefault(x => x.CodDocumentTaskCenter == id);

            c.IndexOf = 0;
            c.Finished = false;
            c.Started = false;
            c.AssignedAt = DateTime.Now;
            c.StartedAt = null;
            c.AlarmFinishingAt = null;
            c.AlarmStartingAt = null;

            taskCenterRepository.EditDocumentTaskCenter(c);
            taskCenterRepository.Save();

            return null;
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
            //            var q = taskCenterRepository.GetDocumentsTaskCenter(codTaskCenter);
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
                            a.CodDocumentTaskCenter,
                            a.FieldA,
                            a.FieldB,
                            a.FieldC,

                            (a.AssignedAt??DateTime.Now).ToString("d"),
                            (a.AlarmStartingAt??DateTime.Now).ToString("d"),  
                          
                            (a.Started??false).ToString(),
                            (a.Started??false)?(a.StartedAt??DateTime.Now).ToString("d"):"",
                            (a.Started??false)?(a.AlarmFinishingAt??DateTime.Now).ToString("d"):"",

                            (a.Finished??false).ToString(),

                            //used to color
                            //yellow --> to start today
                            ((a.Started??false)?false:(a.AlarmStartingAt??DateTime.Now).ToString("d")==DateTime.Now.ToString("d")).ToString(),                            
                            //red!!!
                            ((a.Started??false)?false:(a.AlarmStartingAt??DateTime.Now)<DateTime.Now).ToString(),
                            //yellow
                            (((a.Finished??false)?false:(a.AlarmFinishingAt??DateTime.Now).ToString("d")==DateTime.Now.ToString("d"))&&(a.Started??false)).ToString(),
                            //red
                            (((a.Finished??false)?false:(a.AlarmFinishingAt??DateTime.Now)<DateTime.Now)&&(a.Started??false)).ToString(),

                            (a.Finished??false).ToString(),
                            (a.Started??false).ToString(),

                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeTaskCenterOrder(string ids, string codTaskCenter, string finisheds, string starteds)
        {
            string[] stringsIds = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ids);
            string[] finishedS = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(finisheds);
            string[] startedS = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(starteds);

            //reset Alarm based on...
            var tskCenter = taskCenterRepository.GetSingle(codTaskCenter);
            taskCenterRepository.SetDbName(CurrentDatabase);

            //IDS are codDocumentTaskCenter
            int i = 0;
            foreach (var x in stringsIds)
            {
                var c = this.taskCenterRepository.GetDocumentTaskCenter(x);
                c.IndexOf = i++;
                c.CodTaskCenter = codTaskCenter;

                if (finishedS.Count() > 0)
                {
                    c.Finished = Convert.ToBoolean(finishedS[i - 1]);
                }

                if (startedS.Count() > 0)
                {
                    //started
                    if (!(c.Started ?? false) && (Convert.ToBoolean(startedS[i - 1])))
                    {
                        c.StartedAt = DateTime.Today;
                        c.AlarmFinishingAt = DateTime.Today.AddDays(tskCenter.AlarmFinishAfterDays ?? 0);
                    }
                    //se non è partito non voglio scrivere l'allarme sulla fine
                    c.Started = Convert.ToBoolean(startedS[i - 1]);

                }


                if ((c.AssignedAt ?? DateTime.Today).ToString("d") == DateTime.Today.ToString("d") && c.AlarmStartingAt== null)
                {
                    c.AlarmStartingAt = DateTime.Today.AddDays(tskCenter.AlarmStartAfterDays ?? 0);
                }


                taskCenterRepository.EditDocumentTaskCenter(c);

                //var taskCenter = taskCenterRepository.GetSingle(codTaskCenter);
                //var doc = documentRepository.GetSingle(x);
                //doc.DocumentStates = documentRepository.GetAllDocumentStates(x).ToList();

                //foreach (var item in doc.DocumentStates)
                //{
                //    item.Selected = (item.CodState == taskCenter.CodState);
                //}

                //documentRepository.Edit(doc);
                //documentRepository.Save();

                taskCenterRepository.Save();
            }

            UpdateAndReloadPlanning();
            return null;
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult SaveTaskCenterOrderState()
        {
            var tskCenters = taskCenterRepository.GetAll().ToList();
            foreach (var tskC in tskCenters)
            {
                var docs = taskCenterRepository.GetDocumentsTaskCenter(tskC.CodTaskCenter).Select(x => x.CodDocument).ToList();

                foreach (var x in docs)
                {
                    var taskCenter = taskCenterRepository.GetSingle(tskC.CodTaskCenter);
                    var doc = documentRepository.GetSingle(x);
                    doc.DocumentStates = documentRepository.GetAllDocumentStates(x).ToList();

                    foreach (var item in doc.DocumentStates)
                    {
                        item.Selected = (item.CodState == taskCenter.CodState);
                    }

                    documentRepository.Edit(doc);
                    documentRepository.Save();
                }

            }

            UpdateAndReloadPlanning();
            return null;
        }



        public ActionResult PrintDocumentsTaskCenter(string codTaskCenter)
        {
            int id;
            string reportName = "DocumentsTaskCenter";

            //carico il TaskCenter
            TaskCenter tsk = taskCenterRepository.GetSingle(codTaskCenter);

            string fileNameMain = Path.Combine(Server.MapPath(@"~/Report"), reportName + ".docx");

            string retName = tsk.TaskCenterName + "-" + DateTime.Now.ToString("yyyymmdd");
            retName = retName.PurgeFileName();

            string fileNameSaveAs = Path.Combine(Server.MapPath(@"~/Report"), retName + ".docx");
            string fileNameSaveAsAfterRepair = fileNameSaveAs;// Path.Combine(Server.MapPath(@"~/Report"), retName + "AfterRepair.docx");

            // Store a global reference to the executing assembly.
            g_assembly = Assembly.GetExecutingAssembly();

            //// Create the document in memory:
            var docMain = DocX.Load(fileNameMain);

            //questo array mi serve per il merge
            Queue<string> files = new Queue<string>();

            tsk.MergeField(docMain);

            var docsTsk = taskCenterRepository.GetDocumentsTaskCenter(codTaskCenter).OrderBy(x => x.IndexOf);
            Console.WriteLine(docsTsk);

            foreach (var doc in docsTsk)
            {
                string fileNameDetail = Path.Combine(Server.MapPath(@"~/Report"), "DocumentTaskCenter.docx");
                var docPrintCD = DocX.Load(fileNameDetail);

                doc.MergeField(docPrintCD);
                docPrintCD.SaveAs(Path.Combine(Server.MapPath(@"~/Report"), "DocTskCenter" + doc.CodDocumentTaskCenter + ".docx"));
                files.Enqueue(Path.Combine(Server.MapPath(@"~/Report"), "DocTskCenter" + doc.CodDocumentTaskCenter + ".docx"));
            }

            docMain.SaveAs(fileNameSaveAs);
            docMain.Dispose();

            id = 0;
            foreach (var file in files.Reverse())
            {
                using (WordprocessingDocument myDoc = WordprocessingDocument.Open(fileNameSaveAs, true))
                {
                    var altChunkId = "AltChunkId" + id++;
                    Console.WriteLine(altChunkId);
                    var mainPart = myDoc.MainDocumentPart;
                    var chunk = mainPart.AddAlternativeFormatImportPart(DocumentFormat.OpenXml.Packaging.AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                    using (System.IO.FileStream fileStream = System.IO.File.Open(file, System.IO.FileMode.Open))
                    {
                        chunk.FeedData(fileStream);
                    }
                    var altChunk = new DocumentFormat.OpenXml.Wordprocessing.AltChunk();
                    altChunk.Id = altChunkId;

                    var last = mainPart.Document.Body.Elements<DocumentFormat.OpenXml.Wordprocessing.Paragraph>().Last();
                    mainPart.Document.Body.InsertAfter(altChunk, last);
                    mainPart.Document.Save();
                }
            }

            UpdateAndReloadPlanning();
            return File(fileNameSaveAsAfterRepair, "application/file", retName + ".docx");

        }


        [HttpPost]
        public ActionResult EditDocumentTaskCenter(DocumentTaskCenter c)
        {
            var toUpdate = taskCenterRepository.GetDocumentTaskCenter(c.CodDocumentTaskCenter);


            toUpdate.DocumentName = c.DocumentName;

            toUpdate.FieldA = c.FieldA;
            toUpdate.FieldB = c.FieldB;
            toUpdate.FieldC = c.FieldC;

            try
            {
                toUpdate.AlarmStartingAt = DateTime.Parse(c.AlarmStartingAtString, new CultureInfo(CultureInfo.CurrentCulture.ToString()), System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception)
            { }

            try
            {
                toUpdate.AlarmFinishingAt = DateTime.Parse(c.AlarmFinishingAtString, new CultureInfo(CultureInfo.CurrentCulture.ToString()), System.Globalization.DateTimeStyles.AssumeLocal);
            }
            catch (Exception)
            { }


            taskCenterRepository.EditDocumentTaskCenter(toUpdate);
            taskCenterRepository.Save();

            
            UpdateAndReloadPlanning();
            return null;
        }


        private void UpdateAndReloadPlanning()
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<PlanningHub>();
            //PlanningHub.Update("");
        }
    }

}
