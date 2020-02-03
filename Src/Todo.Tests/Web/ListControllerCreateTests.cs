using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class ListControllerCreateTests
    {
        [TestMethod]
        public void test_if_model_state_is_not_valid_create_view_returned()
        {
            // arrange
            var authController = new ListController(
                new Mock<IContext>().Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            authController.ModelState.AddModelError("test", "test");
            var result = authController.Create(new CreateListViewModel { Title = "test" }).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);

            var viewData = result.Model as CreateListViewModel;
            Assert.IsNotNull(viewData);
            Assert.AreEqual("test", viewData.Title);
        }

        [TestMethod]
        public void test_if_creation_is_successful_list_view_view_is_shown()
        {
            // arrange
            var listId = Guid.NewGuid();
            var saveListMock = new Mock<ISaveListService>();
            saveListMock.Setup(x => x.CreateTodoList(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(new TodoList { Id = listId });

            var contextMock = new Mock<ISessionContext>();
            contextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                saveListMock.Object,
                new Mock<IDeleteService>().Object,
                contextMock.Object);

            // act
            var result = controller.Create(new CreateListViewModel()).GetAwaiter().GetResult() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("View", result.RouteValues["action"]);
            Assert.AreEqual(listId, result.RouteValues["id"]);
            saveListMock.Verify(x => x.CreateTodoList(It.IsAny<string>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}
