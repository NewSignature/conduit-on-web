using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    public class ListControllerViewTests
    {
        [TestMethod]
        public void test_if_list_is_not_found_not_found_result_is_returned()
        {
            // arrange
            var source = new List<TodoList>();
            var dbSetMock = MockFactory.CreateMockDbset(source);
            var contextMock = new Mock<IContext>();
            contextMock.Setup(x => x.Lists).Returns(dbSetMock.Object);

            var controller = new ListController(
                contextMock.Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.View(Guid.NewGuid()).GetAwaiter().GetResult() as HttpNotFoundResult;

            // assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void test_if_list_is_found_it_returns_to_the_view_result()
        {
            // arrange
            var listId = Guid.NewGuid();
            var listSource = new List<TodoList>() { new TodoList { Id = listId, Owner = new User() } };
            var dbSetMock = MockFactory.CreateMockDbset<TodoList>(listSource);

            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var controller = new ListController(
                contextMock.Object,
                new Mock<ISaveListService>().Object,
                new Mock<IDeleteService>().Object,
                new Mock<ISessionContext>().Object);

            // act
            var result = controller.View(listId).GetAwaiter().GetResult() as ViewResult;

            // assert
            Assert.IsNotNull(result);

            var viewData = result.Model as TodoListViewModel;
            Assert.IsNotNull(viewData);
            Assert.AreEqual(listId, viewData.ListId);
        }
    }
}
