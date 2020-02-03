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

namespace Todo.Tests.Services
{
    [TestClass]
    public class DeleteServiceDeleteListTests
    {
        [TestMethod]
        public void test_if_list_is_not_found_savechanges_is_not_called()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<TodoList>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            service.DeleteList(Guid.NewGuid(), Guid.NewGuid()).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [TestMethod]
        public void test_if_list_is_found_but_owned_by_someone_else_exception_is_thrown()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var listSource = new List<TodoList> { new TodoList { Id = idGuid, OwnerId = Guid.NewGuid() } };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<ResourceSharingException>(() => service.DeleteList(idGuid, Guid.NewGuid()));
        }

        [TestMethod]
        public void test_if_delete_is_successful_savechanges_is_called_with_list_removed_from_context()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var userGuid = Guid.NewGuid();
            var listSource = new List<TodoList> { new TodoList { Id = idGuid, OwnerId = userGuid } };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            service.DeleteList(idGuid, userGuid).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.Lists.Remove(It.Is<TodoList>(x1 => x1.Id == idGuid)), Times.Once);
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
