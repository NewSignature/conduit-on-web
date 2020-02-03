using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.Controllers;
using Todo.Web.ViewModels;

namespace Todo.Tests.Web
{
    [TestClass]
    public class AuthControllerLoginTests
    {
        [TestMethod]
        public void test_if_on_login_request_modelstate_is_not_valid_viewresult_is_returned()
        {
            // arrange
            var authController = new AuthController(new Mock<IAuthenticateUserService>().Object, new Mock<ISessionContext>().Object);

            // act
            authController.ModelState.AddModelError("test", "test");
            var result = authController.Login(new LoginViewModel()).GetAwaiter().GetResult() as ActionResult;

            // assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result as ViewResult);
        }

        [TestMethod]
        public void test_bad_login_error_returned_if_authenticate_user_fails()
        {
            // arrange
            var authService = new Mock<IAuthenticateUserService>();
            var authController = new AuthController(authService.Object, new Mock<ISessionContext>().Object);

            // act
            var result = authController.Login(new LoginViewModel() { Username = string.Empty, Password = string.Empty })
                .GetAwaiter().GetResult() as ActionResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(string.Empty, authController.ModelState["OperationResult"].Value);
        }

        [TestMethod]
        public void test_successful_login_redirects_to_list_index_view()
        {
            // arrange
            var authService = new Mock<IAuthenticateUserService>();
            authService.Setup(x => x.GetUserByUsernameAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User() { Username = "test" });
            var sessionContext = new Mock<ISessionContext>();
            var authController = new AuthController(authService.Object, sessionContext.Object);

            // act
            var result = authController.Login(new LoginViewModel()).GetAwaiter().GetResult() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("List", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            sessionContext.VerifySet(x => x.CurrentUser = It.Is<User>(x1 => x1.Username == "test"), Times.Once, "Current User was not set");
        }
    }
}
