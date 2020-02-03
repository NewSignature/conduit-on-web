using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services.Impl;

namespace Todo.Tests.Services
{
    [TestClass]
    public class SaveListServiceCreateTodoListTests
    {
        [TestMethod]
        public void test_save_is_called_for_list_with_save_data()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<TodoList>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Lists).Returns(dbSetMock.Object);

            var service = new SaveListService(contextMock.Object);

            // act
            service.CreateTodoList("test", Guid.NewGuid()).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.Lists.Add(It.Is<TodoList>(x1 => x1.Title == "test")), Times.Once);
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
