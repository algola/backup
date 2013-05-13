using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PapiroMVC.Models;
using Services;
using PapiroMVC.Controllers;

namespace PapiroMVC.Areas.Account.Controllers
{

    [Authorize]
    public class AccountController : ControllerAlgolaBase
    {

        IArticleRepository articleDataRep;

        /***
         * send email to just registered user
         * 
         */

        public void SendConfirmationEmail(string userName)
        {
            MembershipUser user = Membership.GetUser(userName);
            string confirmationGuid = user.ProviderUserKey.ToString();
            string verifyUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) +
                             "/account/verify?ID=" + confirmationGuid;

            var message = new MailMessage("papirosoftware@gmail.com", user.Email)
            {
                Subject = "Please confirm your email",
                Body = verifyUrl

            };

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("papirosoftware@gmail.com", "Ele875147@"); 
            client.Port = 587;
            client.Send(message);
        }

        public AccountController(IArticleRepository _articleDataRep)
        {
            articleDataRep = _articleDataRep;
        }

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [AllowAnonymous]
        [HttpPost]   
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    base.UpdateDatabase(model.UserName);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home",new {Area=""});
        }

        //
        // GET: /Account/Register

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
               
                // Attempt to register the user
                MembershipCreateStatus createStatus;
                Membership.CreateUser(model.UserName, 
                    model.Password, model.Email, 
                    passwordQuestion: null, 
                    passwordAnswer: null, 
                    isApproved: false, 
                    providerUserKey: null, 
                    status: out createStatus);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    this.SendConfirmationEmail(model.UserName);
                    return Json(new { redirectUrl = Url.Action("Confirmation", "Account") });
                
                }
                else
                {
                    string msgError = "Questa email risulta già presente nel sistema";
                   
                    if (createStatus == MembershipCreateStatus.DuplicateEmail)
                        msgError = "Questa email risulta già presente nel sistema";

                    ModelState.AddModelError(String.Empty, msgError);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, userIsOnline: true);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return Json(new { redirectUrl = Url.Action("ChangePasswordSuccess")});
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        public ActionResult Welcome()
        {
            return View();
        }

        
        [AllowAnonymous]
        public ActionResult Verify(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                TempData["tempMessage"] =
                        "The user account is not valid. Please try clicking the link in your email again.";
                return View();
            }
            else
            {
                MembershipUser user = Membership.GetUser(providerUserKey:ID); 
                if (!user.IsApproved)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);
                    FormsAuthentication.SetAuthCookie(Membership.GetUser(user.ProviderUserKey).UserName, createPersistentCookie: false);
                    return Json(new { redirectUrl = Url.Action("welcome") }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FormsAuthentication.SignOut();
                    TempData["tempMessage"] = "You have already confirmed your email address... please log in.";
                    return RedirectToAction("Login");
                }
            }
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
