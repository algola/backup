using System;
using System.Linq;
using System.Web.Mvc;
using PapiroMVC.Models;
using Mvc.HtmlHelpers;
using PapiroMVC.Validation;

namespace PapiroMVC.Areas.DataBase.Controllers
{   
    public partial class CustomerSupplierController : PapiroMVC.Controllers.ControllerAlgolaBase
    {
        /// <summary>
        /// AutoComplete
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult CustomerSupplierBusinessNameAutoComplete(string term)
        {
            PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().ToArray();

            var filteredItems = customerSuppliers.Where(
            item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
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
            PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().OfType<Supplier>().ToArray();

            var filteredItems = customerSuppliers.Where(
            item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
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
        /// Get Autocomplete Customer
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public ActionResult CustomerBusinessNameAutoComplete(string term)
        {
            PapiroMVC.Models.CustomerSupplier[] customerSuppliers = customerSupplierRepository.GetAll().OfType<Customer>().ToArray();


            var filteredItems = customerSuppliers.Where(
            item => !(String.IsNullOrEmpty(item.BusinessName)) && item.BusinessName !=null && item.BusinessName !="" && item.BusinessName.IndexOf(term, StringComparison.InvariantCultureIgnoreCase) >= 0
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
    }

}