using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.Controllers;

namespace Todo.Tests.Web
{
    [TestClass]
    public class AuthControllerLogoutTests
    {
        [TestMethod]
        public void test_if_logout_called_with_no_current_user_redirect_to_home()
        {
            // arrange
            var controller = new AuthController(
                new Mock<IAuthenticateUserService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.Logout() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Main", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void test_if_executing_logout_nulls_out_current_user()
        {
            // arrange
            var sessionContext = new Mock<ISessionContext>();
            var controller = new AuthController(new Mock<IAuthenticateUserService>().Object,
                sessionContext.Object);

            // act
            controller.ExecLogout();

            // assert
            sessionContext.VerifySet(x => x.CurrentUser = null, Times.Once);
        }

        [TestMethod]
        public void test_that_successful_logout_redirects_to_login_view()
        {
            // arrange
            var authController = new AuthController(
                new Mock<IAuthenticateUserService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = authController.ExecLogout() as RedirectToRouteResult;

            // assert
            Assert.AreEqual("Login", result.RouteValues["action"]);
        }
    }
}
