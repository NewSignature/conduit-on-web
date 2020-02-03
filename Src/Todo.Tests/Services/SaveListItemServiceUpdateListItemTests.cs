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
    public class SaveListItemServiceUpdateListItemTests
    {
        [TestMethod]
        public void test_if_parentlist_is_not_owned_by_user_exception_is_thrown()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var listItemsSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = Guid.NewGuid() }
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listItemsSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<ResourceSharingException>(
                () => service.UpdateListItem(idGuid, new EditTodoListItemViewModel(), Guid.NewGuid())).GetAwaiter().GetResult();
        }

        [TestMethod]
        public void test_if_item_is_marked_complete_and_is_incomplete_the_completed_on_value_is_set()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var listItemsSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = ownerGuid }
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listItemsSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);
            var viewModel = new EditTodoListItemViewModel { IsComplete = true };

            // act
            service.UpdateListItem(idGuid, viewModel, ownerGuid).GetAwaiter().GetResult();

            // assert
            Assert.IsNotNull(listItemsSource.First().CompletedOn);
        }

        [TestMethod]
        public void test_if_item_is_complete_and_marked_incomplete_the_completedon_value_is_null()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var listItemsSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = ownerGuid },
                    CompletedOn = DateTime.Now
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listItemsSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);
            var viewModel = new EditTodoListItemViewModel { IsComplete = false };

            // act
            service.UpdateListItem(idGuid, viewModel, ownerGuid).GetAwaiter().GetResult();

            // assert
            Assert.IsNull(listItemsSource.First().CompletedOn);
        }

        [TestMethod]
        public void test_if_update_is_successful_savechanges_is_called_one_time()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var ownerGuid = Guid.NewGuid();
            var listItemsSource = new List<TodoListItem>
            {
                new TodoListItem
                {
                    Id = idGuid,
                    List = new TodoList { OwnerId = ownerGuid }
                }
            };
            var dbSetMock = MockFactory.CreateMockDbset(listItemsSource);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.ListItems).Returns(dbSetMock.Object);

            var service = new SaveListItemService(contextMock.Object);
            var viewModel = new EditTodoListItemViewModel { IsComplete = true };

            // act
            service.UpdateListItem(idGuid, viewModel, ownerGuid).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
