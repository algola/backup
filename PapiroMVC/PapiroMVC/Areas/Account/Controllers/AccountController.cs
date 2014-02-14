﻿using System;
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
using Braintree;
using Ninject;

namespace PapiroMVC.Areas.Account.Controllers
{
    [Authorize]
    public class AccountController : ControllerAlgolaBase
    {
        private readonly IProfileRepository profDataRep;
        protected IMenuProductRepository profMenuRep;
        protected ICustomerSupplierRepository crFrom;
        protected ICustomerSupplierRepository crTo;

        protected ITaskExecutorRepository trFrom;
        protected ITaskExecutorRepository trTo;

        protected IArticleRepository arFrom;
        protected IArticleRepository arTo;

        protected ITypeOfTaskRepository ttrFrom;
        protected ITypeOfTaskRepository ttrTo;

        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        /***
         * send email to just registered user
         * 
         */

        public ActionResult ResultPending()
        {
            return View();
        }

        public ActionResult Pending()
        {
            return View("PendingAmm");
        }

        [HttpPost]
        public ActionResult Pending(FormCollection collection)
        {
            var profile = profDataRep.GetSingle(this.CurrentUser.ToString());

            CustomerRequest request = new CustomerRequest
            {
                Company = profile.CompanyName,
                CreditCard = new CreditCardRequest
                {
                    BillingAddress = new CreditCardAddressRequest
                    {
                        Company = profile.CompanyName,
                        StreetAddress = profile.Base
                    },
                    Number = collection["number"],
                    ExpirationMonth = collection["month"],
                    ExpirationYear = collection["year"],
                    CVV = collection["cvv"]
                }
            };

            Result<Braintree.Customer> result = Constants.Gateway.Customer.Create(request);

            if (result.IsSuccess())
            {
                try
                {
                    Roles.RemoveUserFromRole(CurrentUser.ToString(), "Pending");
                    profile.BrianTreeCustomerId = result.Target.Id;
                }
                catch (Exception)
                {

                }

                Braintree.Customer customer = result.Target;
                ViewData["CustomerName"] = customer.FirstName + " " + customer.LastName;
                ViewData["Message"] = "";
            }
            else
            {
                ViewData["Message"] = result.Message;
            }

            return View("ResultPending");

        }

        public void SendConfirmationEmail(string userName)
        {
            Type res = typeof(PapiroMVC.Models.Resources.Account.Registration);

            MembershipUser user = Membership.GetUser(userName);
            string confirmationGuid = user.ProviderUserKey.ToString();
            string verifyUrl = HttpContext.Request.Url.GetLeftPart(UriPartial.Authority) +
                             "/account/verify?ID=" + confirmationGuid;

            var password = user.GetPassword();
            //var newPassword = member.ResetPassword(currentUser);

            var messagePwd = string.Format("Login: {0}\r\n", user.UserName);
            messagePwd += string.Format("Password: {0}\r\n", password);

            string messageStr = string.Format("{0}\r\n", (string)res.GetProperty("RegistrationBody1").GetValue(null, null) ?? "");
            messageStr += string.Format("{0}\r\n", (string)res.GetProperty("RegistrationBody2").GetValue(null, null) ?? "");
            messageStr += string.Format("\r\n{0}\r\n", messagePwd);
            messageStr += string.Format("{0}\r\n", (string)res.GetProperty("RegistrationBody3").GetValue(null, null) ?? "");
            messageStr += string.Format("{0}\r\n", (string)res.GetProperty("RegistrationBody4").GetValue(null, null) ?? "");
            messageStr += string.Format("\r\n{0}\r\n", verifyUrl);
            messageStr += string.Format("\r\n{0}\r\n", (string)res.GetProperty("RegistrationBodyF1").GetValue(null, null) ?? "");

            var message = new MailMessage("papirosoftware@gmail.com", user.Email)
            {
                Subject = (string)res.GetProperty("RegistrationTitle").GetValue(null, null) ?? "",
                Body = messageStr
            };

            message.Bcc.Add(new MailAddress("a.degola@algola.com"));

            var client = new SmtpClient();
            client.EnableSsl = true;
            client.Credentials = new System.Net.NetworkCredential("papirosoftware@gmail.com", "Ele875147@");
            client.Port = 587;
            client.Send(message);
        }

        //constructor
        public AccountController(IProfileRepository _profDataRep,
            IMenuProductRepository _profMenuRep,
            ICustomerSupplierRepository _crFrom,
            ICustomerSupplierRepository _crTo,
            IArticleRepository _arFrom,
            IArticleRepository _arTo,
            ITaskExecutorRepository _trFrom,
            ITaskExecutorRepository _trTo,
            ITypeOfTaskRepository _ttrFrom,
            ITypeOfTaskRepository _ttrTo)
        {
            //TODOCHRIS
            //passare al costruttore anche un riferimento di tipo IMenuProductRepository
            //e fare le stesse cose che si fanno ora per il IProfileRepository
            profMenuRep = _profMenuRep;
            profDataRep = _profDataRep;
            crFrom = _crFrom;
            crTo = _crTo;
            arFrom = _arFrom;
            arTo = _arTo;
            trFrom = _trFrom;
            trTo = _trTo;
            ttrFrom = _ttrFrom;
            ttrTo = _ttrTo;
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoginInvite(string returnUrl)
        {
            return PartialView("_LoginInvite");
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
                var xx=Membership.GetUser(model.UserName);
                var yy = xx.IsApproved;

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
            //base.UpdateDatabase(model.UserName);

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
            return PartialView("_Login", model);
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
            return RedirectToAction("Index", "Home", new { Area = "" });
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
                    //                    return RedirectToAction("Confirmation", "Account");
                    return Json(new { redirectUrl = Url.Action("Confirmation") });

                }
                else
                {
                    ModelState.AddModelError("PersError", createStatus.ToString());
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("_Register", model);
        }


        //
        // GET: /Account/EditProfile

        public ActionResult EditProfile()
        {

            //TODOCHRIS
            //non dovrai passare alla View(profile) un oggetto di tipo profile... ma dovrai creare un oggetto nuovo (new) di tipo
            //ViewModel 
            //assegnare alla proprietà Profile il profDataRep etc...
            //e alla prorpietà MenuProducts il getAll() dal repository del MenuProduct

            //ricolrdati che devi fare il ToList() del getall perchè vogliamo una lista nel ViewModel

            var menuP = profMenuRep.GetAll().ToList();
            var profile = profDataRep.GetSingle(this.CurrentUser.ToString());
            // return View(profile);

            var temp = new ProfileViewModel();
            temp.Profile = profile;
            temp.MenuProducts = menuP;

            return View(temp);

        }

        //
        // POST: /Account/EditProfile

        [HttpPost]
        public ActionResult EditProfile(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    profDataRep.Edit(model.Profile);
                    profDataRep.Save();

                    //            //TODOCHRIS il viewmodel model ha una proprietà che è la lista dei menuproduct
                    //            //la scorri e per ciascun menu-prodotto in lista: 
                    //            //lo cerchi nel repository mediante la chiave primaria e il risultato lo assegni ad una variabile temp

                    //            foreach (var item in model.MenuProducts)
                    //            {
                    //                var temp=profMenuRep.GetSingle(item.CodMenuProduct);
                    //                temp.Hidden = item.Hidden;
                    //                profMenuRep.Edit(temp);
                    //            }
                    //            profMenuRep.Save();


                    //            aggiorni la prorpietà "Hidden" di temp con quelle dell'elemento corrente nel ciclo.
                    //            applichi il metodo Edit del repository dei menu-prodotti al temp

                    //            al termine del ciclo applichi il metodo Save del repository (più o meno come sopra per il profilo!!!!)

                    return Json(new { redirectUrl = Url.Action("EditProfileSuccess") });
                }

                catch (Exception e)
                {
                    ModelState.AddModelError("PersError", "GenericError");
                }
            }

            //If we got this far, something failed, redisplay form
            return PartialView("_EditProfile", model);
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
                    return Json(new { redirectUrl = Url.Action("ChangePasswordSuccess") });
                }
                else
                {
                    ModelState.AddModelError("PersError", "OldPasswordError");
                }
            }

            // If we got this far, something failed, redisplay form
            return PartialView("_ChangePassword", model);
        }

        //
        // GET: /Account/RecoveryPassword
        [AllowAnonymous]

        public ActionResult RecoveryPassword()
        {
            TempData["EmailError"] = false;

            return View();
        }

        //
        // POST: /Account/RecoveryPassword
        [AllowAnonymous]

        [HttpPost]
        public ActionResult RecoveryPassword(RecoveryPasswordModel model)
        {
            Type res = typeof(PapiroMVC.Models.Resources.Account.Registration);

            if (ModelState.IsValid)
            {
                TempData["EmailError"] = false;

                string currentUser = String.Empty;
                MembershipUser member;

                try
                {
                    currentUser = Membership.GetUserNameByEmail(model.Email);
                }
                catch (Exception)
                {
                    TempData["EmailError"] = false;
                }

                if (currentUser != String.Empty && currentUser != null)
                {
                    //devo inviare la password                    
                    member = Membership.GetUser(currentUser);

                    member.UnlockUser();
                    var password = member.GetPassword();
                    //var newPassword = member.ResetPassword(currentUser);

                    var messagePwd = string.Format("Login: {0}\r\n", member.UserName);
                    messagePwd += string.Format("Password: {0}\r\n", password);

                    string messageStr = string.Format("{0}\r\n", (string)res.GetProperty("RecoveryBody1").GetValue(null, null) ?? "");
                    messageStr += string.Format("{0}\r\n", (string)res.GetProperty("RecoveryBody2").GetValue(null, null) ?? "");
                    messageStr += string.Format("\r\n{0}\r\n", messagePwd);
                    messageStr += string.Format("{0}\r\n", (string)res.GetProperty("RegistrationBodyF1").GetValue(null, null) ?? "");

                    var message = new MailMessage("papirosoftware@gmail.com", member.Email)
                    {
                        Subject = (string)res.GetProperty("RecoveryTitle").GetValue(null, null) ?? "",
                        Body = messageStr
                    };

                    try
                    {
                        var client = new SmtpClient();
                        client.EnableSsl = true;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("papirosoftware@gmail.com", "Ele875147@");
                        client.Port = 587;
                        client.Send(message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Could not send e-mail. Exception caught: " + e);
                    }

                    return Json(new { redirectUrl = Url.Action("EmailSent") });
                    //return RedirectToAction("EmailSent", "Account");  
                }
                else
                {
                    TempData["EmailError"] = true;
                    // If we got this far, something failed, redisplay form
                    return PartialView("_RecoveryPassword", model);
                }

            }

            TempData["EmailError"] = true;
            // If we got this far, something failed, redisplay form
            return PartialView("_RecoveryPassword", model);
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

                    if (!Roles.RoleExists("Pending"))
                        Roles.CreateRole("Pending");

                    Roles.AddUserToRole(user.UserName, "Pending");
                    Membership.UpdateUser(user);

                    //when usere has logged in system, database will be updated. 
                    base.UpdateDatabase(user.UserName);

                    FormsAuthentication.SetAuthCookie(Membership.GetUser(user.ProviderUserKey).UserName, createPersistentCookie: false);
                    TempData["message"] = "Activated";

                    //customer
                    crTo.SetDbName(user.UserName);
                    crFrom.SetDbName("examples");

                    var cFs = crFrom.GetAll();
                    foreach (var cf in cFs)
                    {
                        crTo.Add(cf);
                    }

                    crTo.Save();

                    //articles
                    arTo.SetDbName(user.UserName);
                    arFrom.SetDbName("examples");

                    var aFs = arFrom.GetForImport();
                    foreach (var a in aFs)
                    {
                        arTo.Add(a);
                    }

                    arTo.Save();


                    //typeoftask
                    ttrTo.SetDbName(user.UserName);
                    ttrFrom.SetDbName("examples");
                    var ttFs = ttrFrom.GetAll();
                    foreach (var a in ttFs)
                    {
                        ttrTo.Add(a);
                    }

                    ttrTo.Save();


                    //taskExecutors
                    trTo.SetDbName(user.UserName);
                    trFrom.SetDbName("examples");
                    var tFs = trFrom.GetAll();
                    foreach (var a in tFs)
                    {
                        trTo.Add(a);
                    }

                    trTo.Save();

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