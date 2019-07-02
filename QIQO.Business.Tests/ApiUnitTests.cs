using QIQO.Business.Api.Controllers;
using QIQO.Business.Core;
using QIQO.Business.Services;
using Xunit;

namespace QIQO.Business.Tests
{
    public class ApiUnitTests
    {
        [Fact]
        public void AccountControllerShouldCreate()
        {
            // Arrange
            var mockServiceFactory = new Mock<IServiceFactory>();
            var mockEntityService = new Mock<IEntityService>();
            // var mockQIQOUserManager = new Mock<QIQOUserManager>();


            // mockAccountClient.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns(123);

            // SUT
            var ac = new AccountController(mockServiceFactory.Object, mockEntityService.Object, null);

            // Act
            // var ret_val = sut.CreateAccount(account);

            // Assert
            Assert.IsType<AccountController>(ac);
        }
    }
}
