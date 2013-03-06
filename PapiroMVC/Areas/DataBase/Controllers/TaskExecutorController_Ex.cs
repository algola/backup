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
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.DataBase.Controllers
{
    public partial class TaskExecutorController : PapiroMVC.Controllers.ControllerBase
    {

        //example of autocpmplete                
        public ActionResult FormatMaxAutoComplete(string term)
        {
            TaskExecutor[] filteredItems = taskExecutorRepository.GetAll().Where(
            item => item.FormatMax.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            ).ToArray();

            var projection = from art in filteredItems
                             select new
                             {
                                 id = art.FormatMax,
                                 label = art.FormatMax,
                                 value = art.FormatMax
                             };
            return Json(projection.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult RollPrintableTaskExecutorList(GridSettings gridSettings)
        {
            //common serarch and order
            var q = TaskExecutorList(gridSettings).OfType<RollPrintableArticle>();

            string widthArticleFilter = string.Empty;
            string weightArticleFilter = string.Empty;

            if (gridSettings.isSearch)
            {
                widthArticleFilter = gridSettings.where.rules.Any(r => r.field == "Width") ?
                        gridSettings.where.rules.FirstOrDefault(r => r.field == "Width").data : string.Empty;

                weightArticleFilter = gridSettings.where.rules.Any(r => r.field == "Weight") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Weight").data : string.Empty;
            }

            if (!string.IsNullOrEmpty(widthArticleFilter))
            {
                try
                {
                    var widthtDouble = Convert.ToDouble(widthArticleFilter);
                    q = q.Where(c => c.Width == widthtDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }

            if (!string.IsNullOrEmpty(weightArticleFilter))
            {
                try
                {
                    var weightDouble = Convert.ToDouble(weightArticleFilter);
                    q = q.Where(c => c.Weight == weightDouble);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
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
                        id = a.CodArticle,
                        cell = new string[] 
                        {                       
                            a.CodArticle,
                            a.CodArticle,
                            a.TypeOfMaterial,
                            a.NameOfMaterial,
                            a.Color,
                            a.Weight.ToString(),
                            a.Width.ToString(),
                            a.CustomerSupplierMaker.BusinessName,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMq,
                            ((RollPrintableArticleStandardCost)a.ArticleCosts.First(x => 
                                x.TypeOfArticleCost == ArticleCost.ArticleCostType.RollPrintableArticleStandardCost)).CostPerMl,            
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }


        private IQueryable<TaskExecutor> TaskExecutorList(GridSettings gridSettings)
        {
            //use it in filter
            string codTaskExecutor = string.Empty;


            //if gridsetting is a search option
            if (gridSettings.isSearch)
            {
                //pull search field
                codTaskExecutor = gridSettings.where.rules.Any(r => r.field == "NameOfColumsToSearch") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "NameOfColumsToSearch").data : string.Empty;
            }

            //read all
            var q = taskExecutorRepository.GetAll();


            //execute filter
            if (!string.IsNullOrEmpty(codTaskExecutor))
            {
                q = q.Where(c => c.CodTaskExecutor.ToLower().Contains(codTaskExecutor.ToLower()));
            }

            /*
            if (!string.IsNullOrEmpty(articleNameFilter))
            {
                q = q.Where(c => c.ArticleName.ToLower().Contains(articleNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(supplierNameFilter))
            {
                q = q.Where(c => c.CustomerSupplierMaker.BusinessName.ToLower().Contains(supplierNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(nameOfMaterialFilter))
            {
                q = q.Where(c => c.NameOfMaterial.ToLower().Contains(nameOfMaterialFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeOfMaterialFilter))
            {
                q = q.Where(c => c.TypeOfMaterial.ToLower().Contains(typeOfMaterialFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(colorFilter))
            {
                q = q.Where(c => c.Color.ToLower().Contains(supplierNameFilter.ToLower()));
            }

             */

            //if is sorting
            switch (gridSettings.sortColumn)
            {
                case "NameOfColumn":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodTaskExecutor) : q.OrderBy(c => c.CodTaskExecutor);
                    break;
/*                case "ArticleName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.ArticleName) : q.OrderBy(c => c.ArticleName);
                    break;
                case "SupplierName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CustomerSupplierMaker.BusinessName) : q.OrderBy(c => c.CustomerSupplierMaker.BusinessName);
                    break;
*/            }

            return q;
        }
    }
}
