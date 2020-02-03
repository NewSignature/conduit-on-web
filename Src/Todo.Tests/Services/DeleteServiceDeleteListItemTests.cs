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
    public class DeleteServiceDeleteListItemTests
    {
        [TestMethod]
        public void test_if_listitem_is_not_found_savechanges_is_not_called()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<TodoListItem>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            service.DeleteListItem(Guid.NewGuid(), Guid.NewGuid()).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        }

        [TestMethod]
        public void test_if_listitem_is_found_but_list_is_owned_by_someone_else_exception_is_thrown()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var itemSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = Guid.NewGuid() }
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(itemSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<ResourceSharingException>(() => service.DeleteListItem(idGuid, Guid.NewGuid()));
        }

        [TestMethod]
        public void test_if_delete_is_successful_savechanges_is_called_with_listitem_removed_from_context()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var userGuid = Guid.NewGuid();
            var listSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = userGuid }
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new DeleteService(contextMock.Object);

            // act
            service.DeleteListItem(idGuid, userGuid).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.ListItems.Remove(It.Is<TodoListItem>(x1 => x1.Id == idGuid)), Times.Once);
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
