using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Todo.Web.Context;
using Todo.Web.Controllers;
using Todo.Web.ViewModels;

namespace Todo.Tests.Web
{
    [TestClass]
    public class ListControllerEditTests
    {
        [TestMethod]
        public void test_if_list_is_not_found_edit_returns_notfound_http_result()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<TodoList>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var controller = new ListController(
                contextMock.Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.Edit(Guid.NewGuid()).GetAwaiter().GetResult() as HttpNotFoundResult;

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void test_if_list_is_found_it_is_sent_down_through_viewmodel_to_view()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var sourceList = new List<TodoList> { new TodoList { Id = idGuid, Owner = new User() } };
            var dbSetMock = MockFactory.CreateMockDbset(sourceList);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var controller = new ListController(
                contextMock.Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.Edit(idGuid).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);

            var viewData = result.Model as TodoListViewModel;
            Assert.IsNotNull(viewData);
            Assert.AreEqual(idGuid, viewData.ListId);
        }

        [TestMethod]
        public void test_if_edit_model_state_is_not_valid_the_view_is_returned_with_the_submitted_data()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var controller = new ListController(
                new Mock<IContext>().Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            controller.ModelState.AddModelError("test", "test");
            var result = controller.Edit(idGuid, new TodoListViewModel { Title = "test" }).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);

            var viewData = result.Model as TodoListViewModel;
            Assert.IsNotNull(viewData);
            Assert.AreEqual("test", viewData.Title);
        }

        [TestMethod]
        public void test_if_modelstate_is_valid_edit_called_is_made_one_time()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var saveServiceMock = new Mock<ISaveListService>();
            var sessionContextMock = new Mock<ISessionContext>();
            sessionContextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                saveServiceMock.Object,
                new Mock<IDeleteService>().Object,
                sessionContextMock.Object);

            // act
            controller.Edit(idGuid, new TodoListViewModel()).GetAwaiter().GetResult();

            // assert
            saveServiceMock.Verify(
                x => x.UpdateTodoList(idGuid, It.IsAny<TodoListViewModel>(), It.IsAny<Guid>()), Times.Once);
        }

        [TestMethod]
        public void test_if_update_is_successful_a_redirect_to_view_list_is_returned()
        {
            var idGuid = Guid.NewGuid();
            var saveServiceMock = new Mock<ISaveListService>();
            var sessionContextMock = new Mock<ISessionContext>();
            sessionContextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                saveServiceMock.Object,
                new Mock<IDeleteService>().Object,
                sessionContextMock.Object);

            // act
            var result = controller.Edit(idGuid, new TodoListViewModel()).GetAwaiter().GetResult() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("View", result.RouteValues["action"]);
            Assert.AreEqual(idGuid, result.RouteValues["id"]);
        }

        [TestMethod]
        public void test_if_resource_sharing_exception_is_raised_during_edit_action_error_view_is_returned()
        {
            var saveServiceMock = new Mock<ISaveListService>();
            saveServiceMock.Setup(x => x.UpdateTodoList(It.IsAny<Guid>(), It.IsAny<TodoListViewModel>(), It.IsAny<Guid>()))
                .ThrowsAsync(new ResourceSharingException());

            var sessionContextMock = new Mock<ISessionContext>();
            sessionContextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                saveServiceMock.Object,
                new Mock<IDeleteService>().Object,
                sessionContextMock.Object);

            // act
            var result = controller.Edit(Guid.NewGuid(), new TodoListViewModel()).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }
    }
}
