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
using System.Threading.Tasks;

namespace PapiroMVC.Areas.Account.Controllers
{
    [Authorize]
    public class AccountController : ControllerAlgolaBase
    {

        private readonly IProfileRepository profDataRep;

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

        public AccountController(IProfileRepository _profDataRep)
        {
            profDataRep = _profDataRep;
        }

        

        //
        // GET: /Account/Login

        [HttpGet]
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
        public void LoginAsync(LoginModel model, string returnUrl)
        {
            TempData["errorMessage"] = false;
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                 //   base.UpdateDatabase(model.UserName);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        AsyncManager.Parameters.Add("returnUrl", returnUrl);
                    }
                    else
                    {
                        AsyncManager.Parameters.Add("returnUrl", "");
                    }

                    AsyncManager.Parameters.Add("redirect", true);
                    AsyncManager.Parameters.Add("model", model);


//                    AsyncManager.OutstandingOperations.Increment(1);
                    System.Threading.Tasks.Task.Factory.StartNew(() => longJob(model));                
                }
                else
                {
                    AsyncManager.Parameters.Add("redirect", false);
                    AsyncManager.Parameters.Add("model", model);
                    AsyncManager.Parameters.Add("returnUrl", returnUrl);
                    ModelState.AddModelError("PersError", "LoginMessageError");
                }
            }

        }

        private void longJob(LoginModel model)
        {
            base.UpdateDatabase(model.UserName);
            //you can also set the parameter here
            //AsyncManager.Parameters.Add("redirect", false);

            
            //if we want to wait the end
            //            AsyncManager.OutstandingOperations.Decrement();
        }


        public ActionResult LoginCompleted(LoginModel model, string returnUrl, bool redirect)
        {
            if (redirect)
            {
                if (returnUrl == String.Empty)
                {
                    TempData["welcomeMessage"] = true;
                    // return RedirectToAction("Index", "Home", new { area = "" });
                    return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "" }) });
                }
                else
                {
                    return Redirect(returnUrl);
                }
            }
            return PartialView("_Login",model);
        }

        /*
        
        [AllowAnonymous]
        [HttpPost]   
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            TempData["errorMessage"] = false;
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
                        TempData["welcomeMessage"] = true;
                       // return RedirectToAction("Index", "Home", new { area = "" });
                        return Json(new { redirectUrl = Url.Action("Index", "Home", new { area = "" }) });
                    }
                }
                else
                {
                    ModelState.AddModelError("PersError", "LoginMessageError");
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("_Login", model);
        }
        
         */

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
                    //TO DO: create a new user in main database
                    //
                    
                    var nProf = new Profile();
                    nProf.Name = model.UserName;
                    nProf.CompanyName = model.OrganizationName;
                    nProf.Base = model.Base;
                    nProf.Culture = model.Culture;
                    nProf.Phone = model.Phone;
                    nProf.TaxCode = model.TaxCode;
                    nProf.Refeere = model.Refeere;
                    nProf.VatNumber = model.VatNumber;
                    

                    profDataRep.Add(nProf);
                    profDataRep.Save();

                    this.SendConfirmationEmail(model.UserName);
                    return RedirectToAction("Confirmation", "Account");                
                }
                else
                {
                    ModelState.AddModelError("PersError", createStatus.ToString());
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        //
        // GET: /Account/EditProfile

        public ActionResult EditProfile()
        {
            var profile = profDataRep.GetSingle(this.CurrentUser.ToString());
            return View(profile);
        }

        //
        // POST: /Account/EditProfile

        [HttpPost]
        public ActionResult EditProfile(Profile model)
        {
            if (ModelState.IsValid)
            {

                try 
                {
                    profDataRep.Edit(model);
                    profDataRep.Save();
                    return Json(new { redirectUrl = Url.Action("EditProfileSuccess") });
                }
                
                catch (Exception e)
                {
                    ModelState.AddModelError("PersError", "GenericError");
                }
            }

            // If we got this far, something failed, redisplay form

            return PartialView("_EditProfile",model);
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
                    ModelState.AddModelError("PersError", "OldPasswordError");
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("_ChangePassword",model);
        }

        //
        // GET: /Account/RecoveryPassword
                [AllowAnonymous]

        public ActionResult RecoveryPassword()
        {
            return View();
        }

        //
        // POST: /Account/RecoveryPassword
                [AllowAnonymous]

        [HttpPost]
        public ActionResult RecoveryPassword(RecoveryPasswordModel model)
        {
            if (ModelState.IsValid)
            {

                string currentUser = String.Empty;
                try
                {
                     currentUser = Membership.GetUserNameByEmail(model.Email); 
                }
                catch (Exception)
                {

                }

                if (currentUser != String.Empty)
                {

                    
                }
                return Json(new { redirectUrl = Url.Action("EmailSent") });
                //return RedirectToAction("EmailSent", "Account");  



            }

            // If we got this far, something failed, redisplay form
            return PartialView("_RecoveryPassword",model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        public ActionResult EditProfileSuccess()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Confirmation()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult EmailSent()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Verify(string ID)
        {
            try
            {
                MembershipUser user = Membership.GetUser(providerUserKey: ID);
                if (!user.IsApproved)
                {
                    user.IsApproved = true;
                    Membership.UpdateUser(user);

                    //when usere has logged in system, database will be updated. 
                    base.UpdateDatabase(user.UserName);

                    FormsAuthentication.SetAuthCookie(Membership.GetUser(user.ProviderUserKey).UserName, createPersistentCookie: false);
                    TempData["message"] = "Activated";
                }
                else
                {
                    FormsAuthentication.SignOut();
                    TempData["message"] = "JustActivated";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["message"] = "Error";                
            }
            return View();
        }

        private new IEnumerable<string> GetErrorsFromModelState()
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
