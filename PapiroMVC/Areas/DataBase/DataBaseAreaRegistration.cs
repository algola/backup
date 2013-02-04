using System.Web.Mvc;

namespace PapiroMVC.Areas.DataBase
{
    public class DataBaseAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "DataBase";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(                
                "DataBase_default",
                "DataBase/{controller}/{action}/{id}",
                new { controller = "DataBase", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
