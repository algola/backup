using PapiroMVC.Areas.Working.Controllers;
using Services;
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
                var users = System.Web.Security.Roles.GetUsersInRole("Pending");

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




    public class AuthorizeModuleAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var thisController = ((ProductController)filterContext.Controller);
            IMenuProductRepository prodRepository = thisController.MenuProductRepository;
            string user = thisController.CurrentUser.UserName;

            //Sperimentale
            // first look at routedata then at request parameter:
            var id = (filterContext.RequestContext.RouteData.Values["id"] as string) ?? ("" as string);


            //associazione tra la categoria del prodotto e la Role
            Dictionary<string, string> prodMod = new Dictionary<string, string>();
            prodMod.Add("FogliSingoli", "SmallFormat");
            prodMod.Add("Book", "SmallFormat");
            prodMod.Add("GrandeFormato", "WideFormat");
            prodMod.Add("Rotoli", "Label");
            prodMod.Add("Cliche", "Cliche");
            prodMod.Add("Description", "Description");

            bool redirect = false;

            if (prodRepository != null)
            {
                var product = prodRepository.GetSingle(id);

                Console.WriteLine(product.CodCategory);

                if (product != null)
                {
                   
                    if (!System.Web.Security.Roles.RoleExists(product.CodCategory))
                    {
                        System.Web.Security.Roles.CreateRole(product.CodCategory);
                    }

                    //if is Generico 
                    if (product.CodCategory == "Description")
                    {
                        try
                        {
                            //user that is not in... 
                            var _users = System.Web.Security.Roles.GetUsersInRole(prodMod.SingleOrDefault(k => k.Key == product.CodCategory).Value);
                            if (!_users.Contains(user))
                            {
                                System.Web.Security.Roles.AddUserToRole(user, "Description");
                            }
                        }
                        catch (Exception e)
                        {
                            Console.Write(e.Message);
                            System.Web.Security.Roles.AddUserToRole(user, "Description");
                        }                        
                    }

                    //traduzione e ricerca dal prodotto -> modulo
                    var users = System.Web.Security.Roles.GetUsersInRole(prodMod.SingleOrDefault(k => k.Key == product.CodCategory).Value);
                    redirect = !users.Contains(user);
                }
            }

            if (redirect)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Account",
                        action = "EditProfile",
                        view = "EditProfileModule",
                        area = "Account"
                    }));
            }

            //sperimentale ----------------------------------------

        }

    }
}