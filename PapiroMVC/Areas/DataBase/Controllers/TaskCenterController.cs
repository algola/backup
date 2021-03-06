﻿using System;
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
using Mvc.HtmlHelpers;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    [AuthorizeUser]
    public partial class TaskCenterController : PapiroMVC.Controllers.ControllerAlgolaBase
    {

        private readonly ITaskCenterRepository taskCenterRepository;
        private readonly IDocumentRepository documentRepository;

        protected dbEntities db;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            taskCenterRepository.SetDbName(CurrentDatabase);
            documentRepository.SetDbName(CurrentDatabase);
        }

        public TaskCenterController(ITaskCenterRepository _tskExDataRep,
            IDocumentRepository _docDataRep)
        {
            taskCenterRepository = _tskExDataRep;
            documentRepository = _docDataRep;

            this.Disposables.Add(taskCenterRepository);
        }

        /// <summary>
        /// List of taskExecutor filtered by codTypeOfTask
        /// </summary>
        /// <param name="codTypeOfTask"></param>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View("Index");
        }


        [HttpParamAction]
        [HttpPost]
        public ActionResult CreateTaskCenter(TaskCenter model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    {
                        model.CodTaskCenter = taskCenterRepository.GetNewCode(model);

                        var state = documentRepository.GetAllStates().FirstOrDefault(x => x.StateName == model.StateName);

                        if (state != null)
                        {
                            model.CodState = state.CodState;
                        }
                        else
                        {
                            model.CodState = null;
                        }

                        taskCenterRepository.Add(model);
                        taskCenterRepository.Save();

                    }
                    //hooray it passed - go back to index
                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }

            ViewBag.ActionMethod = "CreateTaskCenter";
            return PartialView("_EditAndCreateTaskCenter", model);
        }


        [HttpGet]
        public ActionResult CreateTaskCenter()
        {
            ViewBag.ActionMethod = "CreateTaskCenter";
            var x = new TaskCenter();
            return View(x);
        }

        [HttpGet]
        public ActionResult EditTaskCenter(string id)
        {
            TaskCenter tskEx = new TaskCenter();
            tskEx = (TaskCenter)taskCenterRepository.GetSingle(id);

            if (tskEx.State != null)
            {
                tskEx.StateName = tskEx.State.StateName;
            }


            if (tskEx == null)
                return HttpNotFound();

            ViewBag.ActionMethod = "EditTaskCenter";
            return View("EditTaskCenter", tskEx);
        }


        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangeTaskCenterOrder(string ids)
        {

            string[] strings = Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(ids);

            int i = 0;
            foreach (var x in strings)
            {
                var c = this.taskCenterRepository.GetSingle(x);
                c.IndexOf = i++;
                taskCenterRepository.Edit(c);
            }

            taskCenterRepository.Save();

            return null;
        }



        [HttpParamAction]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditTaskCenter(TaskCenter c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var state = documentRepository.GetAllStates().FirstOrDefault(x => x.StateName == c.StateName);

                    if (state != null)
                    {
                        c.CodState = state.CodState;
                    }
                    else
                    {
                        c.CodState = null;
                    }

                    taskCenterRepository.Edit(c);
                    taskCenterRepository.Save();
                    return Json(new { redirectUrl = Url.Action("Index") });
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Something went wrong. Message: " + ex.Message);
                }
            }
            ViewBag.ActionMethod = "EditTaskCenter";
            return PartialView("_EditAndCreateTaskCenter", c);
        }

        public ActionResult TaskCenterList(GridSettings gridSettings)
        {
            //use it in filter
            string codTaskCenterFilter = string.Empty;
            string taskExecutorNameFilter = string.Empty;
            string formatMaxFilter = string.Empty;


            //if gridsetting is a search option
            if (gridSettings.isSearch)
            {
                //pull search field
                codTaskCenterFilter = gridSettings.where.rules.Any(r => r.field == "CodExecutor") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodExecutor").data : string.Empty;

                taskExecutorNameFilter = gridSettings.where.rules.Any(r => r.field == "TaskCenterName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TaskCenterName").data : string.Empty;

            }

            //read all
            var q = taskCenterRepository.GetAll().OrderBy(x => x.IndexOf).AsQueryable();

            //execute filter
            if (!string.IsNullOrEmpty(codTaskCenterFilter))
            {
                q = q.Where(c => c.CodTaskCenter.ToLower().Contains(codTaskCenterFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(taskExecutorNameFilter))
            {
                q = q.Where(c => c.TaskCenterName.ToLower().Contains(taskExecutorNameFilter.ToLower()));
            }

            //if is sorting
            switch (gridSettings.sortColumn)
            {
                case "CodTaskCenter":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodTaskCenter) : q.OrderBy(c => c.CodTaskCenter);
                    break;
                case "TaskCenterName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.TaskCenterName) : q.OrderBy(c => c.TaskCenterName);
                    break;
                case "IndexOf":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.IndexOf) : q.OrderBy(c => c.IndexOf);
                    break;
            }

            var q2 = q.ToList();
            var q3 = q2.Skip((gridSettings.pageIndex - 1) * gridSettings.pageSize).Take(gridSettings.pageSize).ToList();

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
                    from a in q3
                    select new
                    {
                        id = a.CodTaskCenter,
                        cell = new string[] 
                        {                                
                            a.CodTaskCenter,
                             a.CodTaskCenter,
                            a.TaskCenterName,
                            (a.IndexOf??0).ToString(),

                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }





    }
}
