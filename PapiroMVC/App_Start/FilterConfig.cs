using PapiroMVC.Validation.Error;
using System.Web;
using System.Web.Mvc;

namespace PapiroMVC
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomHandleErrorAttribute());
        }
    }
}
