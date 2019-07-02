// using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Client.Proxies;
using Xunit;

namespace QIQO.Business.Tests
{
    public class AccountClientTests
    {

        [Fact, AssumeIdentity("QIQOOrderEntryAdmin")]
        public void AccountClientCreateAccountReturnsInt()
        {
            // Arrange
            Account account = new Account()
            {
                AccountKey = 123,
                AccountCode = "TEST123",
                AccountName = "Test Account",
                AccountDesc = "Test Account Description",
                AccountDBA = "Test Account DBA"
            };
            Mock<IAccountService> mockAccountClient = new Mock<IAccountService>();

            mockAccountClient.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns(123);

            // SUT
            AccountClient sut = new AccountClient(EndpointConfiguration.NetTcpBinding_IAccountService);

            // Act
            int ret_val = sut.CreateAccount(account);

            // Assert
            Assert.Equal(123, ret_val);
        }

        [Fact, AssumeIdentity("QIQOOrderEntryAdmin")]
        public void AccountClientDeleteAccountReturnsBoolean()
        {
            // Arrange
            Account account = new Account() { AccountKey = 123 };
            Mock<IAccountService> mockAccountClient = new Mock<IAccountService>();

            mockAccountClient.Setup(m => m.DeleteAccount(It.IsAny<Account>())).Returns(true);

            // SUT
            AccountClient sut = new AccountClient(EndpointConfiguration.NetTcpBinding_IAccountService);

            // Act
            bool ret_val = sut.DeleteAccount(account);

            // Assert
            Assert.True(ret_val);
        }
    }
}
