using Ninject.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PapiroMVC.Models
{
    public partial class User
    {
        public string ResetPassword(string userName)
        {
            var user = Membership.GetUser(userName);

            string newPassword = "";
            if (user == null)
            {
                throw new Exception("User name not found");
            }
            else
            {
                string resetPassword = user.ResetPassword();
                //string friendlyPassword = GenerateHumanFriendlyPassword();
                //if (user.ChangePassword(newPassword, friendlyPassword))
                newPassword = resetPassword;

            }
            return newPassword;
        }

        private string GenerateHumanFriendlyPassword()
        {
            string[] passwordRoots = { "Pennsylvania", "Missouri", "Kansas", "Washington", "Alabama",
            "California", "Portland", "Texas", "Nebraska", "Wisconsin" };

            Random r = new Random(DateTime.Now.Millisecond);
            int random = r.Next(10, 1000);
            int root = random % 10;
            return passwordRoots[root] + random.ToString();
        }

    }
}