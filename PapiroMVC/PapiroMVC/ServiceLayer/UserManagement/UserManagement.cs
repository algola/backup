using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Security;

namespace PapiroMVC.ServiceLayer.UserManagement
{
    public class UserManagement
    {
        /// <summary>
        /// Init UserManagement
        /// </summary>
        public UserManagement()
        {

        }

        /// <summary>
        /// after registration, system sends email with link to procede with verification
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool SendConfirmationEmail(string userName, string domain)
        {
            try
            {
                Type res = typeof(PapiroMVC.Models.Resources.Account.Registration);

                MembershipUser user = Membership.GetUser(userName);
                string confirmationGuid = user.ProviderUserKey.ToString();

                //domain is HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)

                string verifyUrl = domain +
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

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        /// <summary>
        /// this method is used when users invite another user to reach some db
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public bool SendInvitationEmail(string userName, string domain)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// check if user passed in parameter is a valid user in system
        /// </summary>
        /// <param name="user">username</param>
        /// <returns></returns>
        public bool IsUserRegistered(string user)
        {
            return true;
        }
       
        /// <summary>
        /// recover forgotten password via e-mail address
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        public bool RecoverPassword(string mail)
        {
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Roles RolesOfUser(string user)
        {
            //call IsUserRegistered
            //if ok --> retur roles
            //   no --> null
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        List<string> UsersInDb(string db)
        {
            //query profile table and return only user projection 
            throw new NotImplementedException();
        }

    }
}