using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PapiroMVC.ServiceLayer.UserManagement;

namespace TestUserManagement
{
    [TestClass]
    public class UserManagementTest
    {
        [TestMethod]
        public void CheckExistUser()
        {
            var x = new UserManagement();
            Assert.IsTrue(x.IsUserRegistered("mattia"));

        }


        [TestMethod]
        public void SendEmail()
        {
            var x = new UserManagement();            
            Assert.IsTrue(x.SendConfirmationEmail("mattia","www.contoso.com"));
        }
    }
}
