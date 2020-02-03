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
    public class ListControllerDeleteTests
    {
        [TestMethod]
        public void test_if_list_is_not_found_http_not_found_is_returned()
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
            var result = controller.Delete(Guid.NewGuid()).GetAwaiter().GetResult() as HttpNotFoundResult;

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void test_if_list_is_found_view_is_returned_for_list()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var sourceList = new List<TodoList>
            {
                new TodoList { Id = idGuid, Owner = new User() }
            };
            var dbSetMock = MockFactory.CreateMockDbset(sourceList);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var controller = new ListController(
                contextMock.Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.Delete(idGuid).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);

            var viewData = result.Model as TodoListViewModel;
            Assert.IsNotNull(viewData);
            Assert.AreEqual(idGuid, viewData.ListId);
        }

        [TestMethod]
        public void test_if_resource_sharing_exception_is_thrown_error_view_is_returned()
        {
            // arrange
            var deleteMock = new Mock<IDeleteService>();
            deleteMock.Setup(x => x.DeleteList(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ThrowsAsync(new ResourceSharingException());

            var sessionContextMock = new Mock<ISessionContext>();
            sessionContextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                new Mock<ISaveListService>().Object,
                deleteMock.Object,
                sessionContextMock.Object);

            // act
            var result = controller.ExecDelete(Guid.NewGuid()).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Error", result.ViewName);
        }

        [TestMethod]
        public void test_if_list_delete_succeeds_redirect_result_for_list_index_is_returned()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var deleteMock = new Mock<IDeleteService>();
            deleteMock.Setup(x => x.DeleteList(It.IsAny<Guid>(), It.IsAny<Guid>()));

            var sessionContextMock = new Mock<ISessionContext>();
            sessionContextMock.SetupGet(x => x.CurrentUser).Returns(new User());

            var controller = new ListController(
                new Mock<IContext>().Object,
                new Mock<ISaveListService>().Object,
                deleteMock.Object,
                sessionContextMock.Object);

            // act
            var result = controller.ExecDelete(idGuid).GetAwaiter().GetResult() as RedirectToRouteResult;

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            deleteMock.Verify(x => x.DeleteList(idGuid, It.IsAny<Guid>()), Times.Once);
        }
    }
}
