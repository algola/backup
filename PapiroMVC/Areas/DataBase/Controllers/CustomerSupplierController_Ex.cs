using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;

namespace PapiroMVC.Areas.DataBase.Controllers
{   
    public partial class CustomerSupplierController : PapiroMVC.Controllers.ControllerBase
    {
        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult CustomerSupplierBusinessNameAutoComplete(string term)
        {
            CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems = customerSuppliers.Where(
            item => item.BusinessName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from custSupp in filteredItems
                             select new
                             {
                                 id = custSupp.CodCustomerSupplier,
                                 label = custSupp.BusinessName,
                                 value = custSupp.BusinessName
                             };
            return Json(projection.ToList(), JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult SupplierBusinessNameAutoComplete(string term)
        {
            CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().OfType<Supplier>().ToArray();

            var filteredItems = customerSuppliers.Where(
            item => item.BusinessName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
            );

            var projection = from custSupp in filteredItems
                             select new
                             {
                                 id = custSupp.CodCustomerSupplier,
                                 label = custSupp.BusinessName,
                                 value = custSupp.BusinessName
                             };
            return Json(projection.ToList(), JsonRequestBehavior.AllowGet);

        }



        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult CustomerSupplierCityAutoComplete(string term)
        {
            string[] allCities = customerSupplierBaseRepository.GetAll().Select(x=>x.City).ToArray();

            var cities = from d in allCities.Distinct()
                         where (d.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0)
                         select new 
                         { 
                             label = d,
                             value = d
                         };

            return Json(cities.ToList(), JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult CustomerSupplierList(GridSettings gridSettings)
        {
            string codCustomerSupplierFilter = string.Empty;
            string businessNameFilter = string.Empty;
            string vatNumberFilter = string.Empty;
            string taxCodeFilter = string.Empty;
            
            if (gridSettings.isSearch)
            {
                codCustomerSupplierFilter = gridSettings.where.rules.Any(r => r.field == "CodCustomerSupplier") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "CodCustomerSupplier").data : string.Empty;

                businessNameFilter = gridSettings.where.rules.Any(r => r.field == "BusinessName") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "BusinessName").data : string.Empty;
                
                vatNumberFilter = gridSettings.where.rules.Any(r => r.field == "VatNumber") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "VatNumber").data : string.Empty;
                
                taxCodeFilter = gridSettings.where.rules.Any(r => r.field == "TaxCode") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "TaxCode").data : string.Empty;
                
            }
            var q = customerSupplierRepository.GetAll();

            if (!string.IsNullOrEmpty(codCustomerSupplierFilter))
            {
                q = q.Where(c => c.CodCustomerSupplier.ToLower().Contains(codCustomerSupplierFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(businessNameFilter))
            {
                q = q.Where(c => c.BusinessName.ToLower().Contains(businessNameFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(vatNumberFilter))
            {
                q = q.Where(c => c.VatNumber.ToLower().Contains(vatNumberFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(taxCodeFilter))
            {
                q = q.Where(c => c.TaxCode.ToLower().Contains(taxCodeFilter.ToLower()));
            }

            switch (gridSettings.sortColumn)
            {
                case "CodCustomerSupplier":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.CodCustomerSupplier) : q.OrderBy(c => c.CodCustomerSupplier);
                    break;
                case "BusinessName":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.BusinessName) : q.OrderBy(c => c.BusinessName);
                    break;
                case "VatNumber":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.VatNumber) : q.OrderBy(c => c.VatNumber);
                    break;
                case "TaxCode":
                    q = (gridSettings.sortOrder == "desc") ? q.OrderByDescending(c => c.TaxCode) : q.OrderBy(c => c.TaxCode);
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
                        id = a.CodCustomerSupplier,
                        cell = new string[] 
                        {                       
                            a.CodCustomerSupplier,
                            a.CodCustomerSupplier,
                            a.BusinessName, 
                            a.VatNumber, 
                            a.TaxCode, 
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    
        public ActionResult CustomerSupplierBaseList(string id, GridSettings gridSettings)
        {
            string addressFilter = string.Empty;
            string cityFilter = string.Empty;
            string countryFilter = string.Empty;
            string provinceFilter = string.Empty;
            
            #region filtering
            if (gridSettings.isSearch)
            {
                addressFilter = gridSettings.where.rules.Any(r => r.field == "Address") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Address").data : string.Empty;
                cityFilter = gridSettings.where.rules.Any(r => r.field == "City") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "City").data : string.Empty;
                countryFilter = gridSettings.where.rules.Any(r => r.field == "Country") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Country").data : string.Empty;
                provinceFilter = gridSettings.where.rules.Any(r => r.field == "Province") ?
                    gridSettings.where.rules.FirstOrDefault(r => r.field == "Province").data : string.Empty;
            }
            var q = customerSupplierBaseRepository.GetAll();

            q = q.Where(c => c.CodCustomerSupplier == id);
 
            if (!string.IsNullOrEmpty(addressFilter))
            {
                q = q.Where(c => c.Address.ToLower().Contains(addressFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(cityFilter))
            {
                q = q.Where(c => c.City.ToLower().Contains(cityFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(countryFilter))
            {
                q = q.Where(c => c.Country.ToLower().Contains(countryFilter.ToLower()));
            }

            if (!string.IsNullOrEmpty(provinceFilter))
            {
                q = q.Where(c => c.Province.ToLower().Contains(provinceFilter.ToLower()));
            }
            #endregion

            #region Ordering
            switch (gridSettings.sortColumn)
            {
                case "Address":
                    q = (gridSettings.sortOrder == "desc") ? 
                        q.OrderByDescending(c => c.Address) : 
                        q.OrderBy(c => c.Address);
                    break;
                case "City":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.City) :
                        q.OrderBy(c => c.City);
                    break;
                case "PostalCode":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.PostalCode) :
                        q.OrderBy(c => c.PostalCode);
                    break;
                case "Country":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Country) :
                        q.OrderBy(c => c.Country);
                    break;
                case "Phone":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Phone) :
                        q.OrderBy(c => c.Phone);
                    break;
                case "Fax":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Fax) :
                        q.OrderBy(c => c.Fax);
                    break;
                case "Email":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Email) :
                        q.OrderBy(c => c.Email);
                    break;
                case "Note":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Note) :
                        q.OrderBy(c => c.Note);
                    break;
                case "Referee":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Referee) :
                        q.OrderBy(c => c.Referee);
                    break;
                case "Pec":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Pec) :
                        q.OrderBy(c => c.Pec);
                    break;
                case "Province":
                    q = (gridSettings.sortOrder == "desc") ?
                        q.OrderByDescending(c => c.Province) :
                        q.OrderBy(c => c.Province);
                    break;
            }
            #endregion

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
                        id = a.CodCustomerSupplierBase,
                        cell = new string[] 
                        { 
                            a.CodCustomerSupplierBase,
                            a.Address,
                            a.PostalCode,
                            a.City,
                            a.Country,
                            a.Phone,
                            a.Fax,
                            a.Email,
                            a.Note,
                            a.Referee,
                            a.Pec,
                            a.Province,                        
                        }
                    }
                ).ToArray()
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }

}