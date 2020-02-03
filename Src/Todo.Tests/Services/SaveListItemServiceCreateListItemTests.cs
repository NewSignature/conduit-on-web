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
    public class SaveListItemServiceCreateListItemTests
    {
        [TestMethod]
        public void test_if_parentlist_is_not_owned_by_user_exception_is_thrown()
        {
            // arrange
            var listIdGuid = Guid.NewGuid();
            var listSource = new List<TodoList>()
            {
                new TodoList
                {
                    Id = listIdGuid,
                    OwnerId = Guid.NewGuid()
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<ResourceSharingException>(
                () => service.CreateListItem(listIdGuid, new CreateListItemViewModel(), Guid.NewGuid()));
        }

        [TestMethod]
        public void test_if_creation_is_successful_context_is_saved_with_new_list_item_entity()
        {
            var listIdGuid = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var listSource = new List<TodoList>()
            {
                new TodoList
                {
                    Id = listIdGuid,
                    OwnerId = ownerGuid,
                    Items = new List<TodoListItem>()
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);

            // act
            service.CreateListItem(listIdGuid, new CreateListItemViewModel() { ItemName = "test" }, ownerGuid).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
