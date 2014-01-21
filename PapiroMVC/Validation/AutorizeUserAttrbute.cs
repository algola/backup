using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace PapiroMVC.Validation
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            
            if (isAuthorized)
            {
                var user = httpContext.User.Identity.Name.ToString();
                var users=System.Web.Security.Roles.GetUsersInRole("Pending");

                isAuthorized = !users.Contains(user);

                return isAuthorized;
            }

            return isAuthorized;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary(
                            new
                            {
                                controller = "Account",
                                action = "Pending",
                                area = "Account"
                            })
                        );
        }

    }

}