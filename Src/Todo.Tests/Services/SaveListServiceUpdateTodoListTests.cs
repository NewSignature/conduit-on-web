using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Common;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services.Impl;
using Todo.Web.ViewModels;

namespace Todo.Tests.Services
{
    [TestClass]
    public class SaveListServiceUpdateTodoListTests
    {
        [TestMethod]
        public void test_if_update_is_made_by_non_owning_user_exception_is_thrown()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var listSource = new List<TodoList>
            {
                new TodoList { OwnerId = Guid.NewGuid() }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new SaveListService(contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<ResourceSharingException>(
                () => service.UpdateTodoList(idGuid, new TodoListViewModel(), Guid.NewGuid()));
        }

        [TestMethod]
        public void test_if_update_is_successful_savechanges_is_called_one_time()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var listSource = new List<TodoList>
            {
                new TodoList { Id = idGuid, OwnerId = ownerGuid }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new SaveListService(contextMock.Object);

            // act
            service.UpdateTodoList(idGuid, new TodoListViewModel(), ownerGuid).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
