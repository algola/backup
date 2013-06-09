using System.Web.Mvc;

namespace PapiroMVC.Areas.Working
{
    public class WorkingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Working";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Working_default",
                "Working/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
