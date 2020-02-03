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
using Todo.Services;
using Todo.Services.Impl;
using Todo.Web.ViewModels;

namespace Todo.Tests.Services
{
    [TestClass]
    public class CreateUserServiceTests
    {
        [TestMethod]
        public void test_if_create_user_is_called_for_existing_username_exception_is_thrown()
        {
            // arrange
            var userList = new List<User> { new User { Username = "testuser" } };
            var dbSetMock = MockFactory.CreateMockDbset(userList);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Users).Returns(dbSetMock.Object);

            var service = new CreateUserService(
                new Mock<IPasswordHashService>().Object,
                contextMock.Object);

            // act
            // assert
            Assert.ThrowsExceptionAsync<DuplicateUserException>(() => service.CreateUser(new CreateUserViewModel { Username = "testuser" }));
        }

        [TestMethod]
        public void test_if_user_creation_succeeds_savechanges_is_called_with_new_user_in_context()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<User>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Users).Returns(dbSetMock.Object);

            var service = new CreateUserService(
                new Mock<IPasswordHashService>().Object,
                contextMock.Object);

            // act
            service.CreateUser(new CreateUserViewModel { Username = "testuser", Password = "password" }).GetAwaiter().GetResult();

            // assert
            contextMock.Verify(x => x.Users.Add(It.Is<User>(x1 => x1.Username == "testuser")), Times.Once);
            contextMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
