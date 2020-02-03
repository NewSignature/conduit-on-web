using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services;
using Todo.Services.Impl;

namespace Todo.Tests.Services
{
    [TestClass]
    public class AuthenticateUserServiceTests
    {
        [TestMethod]
        public void test_if_user_is_not_found_by_username_getuser_returns_null()
        {
            // arrange
            var dbSetMock = MockFactory.CreateMockDbset(new List<User>());
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Users).Returns(dbSetMock.Object);

            var service = new AuthenticateUserService(
                new Mock<IPasswordHashService>().Object,
                contextMock.Object);

            // act
            var result = service.GetUserByUsernameAndPassword("test", "test").GetAwaiter().GetResult();

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void test_if_user_is_found_but_password_does_not_match_null_is_returned()
        {
            // arrange
            var userList = new List<User> { new User { Username = "test", Password = "test" } };
            var dbSetMock = MockFactory.CreateMockDbset(userList);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Users).Returns(dbSetMock.Object);

            var hashServiceMock = new Mock<IPasswordHashService>();
            hashServiceMock.Setup(x => x.GetPasswordHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("test2");

            var service = new AuthenticateUserService(
                hashServiceMock.Object,
                contextMock.Object);

            // act
            var result = service.GetUserByUsernameAndPassword("test", "test").GetAwaiter().GetResult();

            // assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void test_if_passwords_match_for_selected_user_that_user_is_returned()
        {
            // arrange
            var idGuid = Guid.NewGuid();
            var userList = new List<User> { new User { Id = idGuid, Username = "test", Password = "test" } };
            var dbSetMock = MockFactory.CreateMockDbset(userList);
            var contextMock = new Mock<IContext>();
            contextMock.SetupGet(x => x.Users).Returns(dbSetMock.Object);

            var hashServiceMock = new Mock<IPasswordHashService>();
            hashServiceMock.Setup(x => x.GetPasswordHash(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("test");

            var service = new AuthenticateUserService(
                hashServiceMock.Object,
                contextMock.Object);

            // act
            var result = service.GetUserByUsernameAndPassword("test", "test").GetAwaiter().GetResult();

            // assert
            Assert.IsNotNull(result);
            Assert.AreEqual(idGuid, result.Id);
        }
    }
}
