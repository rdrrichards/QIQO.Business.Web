using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Moq;
using QIQO.Business.Client.Entities;
using QIQO.Business.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

// I stole many of this from the ASP.NET Identity unit tests
namespace QIQO.Business.Tests
{
    public class IdentityUnitTests
    {
        public class CustomUserManager : QIQOUserManager
        {
            public CustomUserManager() : base(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null)
            { }
        }

        public class CustomRoleManager : QIQORoleManager
        {
            public CustomRoleManager() : base(new Mock<IRoleStore<Role>>().Object, null, null, null, null)
            { }
        }

        [Fact]
        public async Task CreateCallsStore()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.CreateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            store.Setup(s => s.GetUserNameAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.UserName)).Verifiable();
            store.Setup(s => s.SetNormalizedUserNameAsync(user, user.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.CreateAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task CreateCallsUpdateEmailStore()
        {
            // Setup
            Mock<IUserEmailStore<User>> store = new Mock<IUserEmailStore<User>>();
            User user = new User { UserName = "Foo", Email = "Foo@foo.com" };
            store.Setup(s => s.CreateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            store.Setup(s => s.GetUserNameAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.UserName)).Verifiable();
            store.Setup(s => s.GetEmailAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.Email)).Verifiable();
            store.Setup(s => s.SetNormalizedEmailAsync(user, user.Email.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.SetNormalizedUserNameAsync(user, user.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.CreateAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task DeleteCallsStore()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.DeleteAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.DeleteAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task UpdateCallsStore()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.GetUserNameAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.UserName)).Verifiable();
            store.Setup(s => s.SetNormalizedUserNameAsync(user, user.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.UpdateAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task UpdateWillUpdateNormalizedEmail()
        {
            // Setup
            Mock<IUserEmailStore<User>> store = new Mock<IUserEmailStore<User>>();
            User user = new User { UserName = "Foo", Email = "email" };
            store.Setup(s => s.GetUserNameAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.UserName)).Verifiable();
            store.Setup(s => s.GetEmailAsync(user, CancellationToken.None)).Returns(Task.FromResult(user.Email)).Verifiable();
            store.Setup(s => s.SetNormalizedUserNameAsync(user, user.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.SetNormalizedEmailAsync(user, user.Email.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.UpdateAsync(user);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task SetUserNameCallsStore()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User();
            store.Setup(s => s.SetUserNameAsync(user, "foo", CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.GetUserNameAsync(user, CancellationToken.None)).Returns(Task.FromResult("foo")).Verifiable();
            store.Setup(s => s.SetNormalizedUserNameAsync(user, "FOO", CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.SetUserNameAsync(user, "foo");

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task FindByIdCallsStore()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.FindByIdAsync(user.UserId.ToString(), CancellationToken.None)).Returns(Task.FromResult(user)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            User result = await userManager.FindByIdAsync(user.UserId.ToString());

            // Assert
            Assert.Equal(user, result);
            store.VerifyAll();
        }

        [Fact]
        public async Task FindByNameCallsStoreWithNormalizedName()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.FindByNameAsync(user.UserName.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(user)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            User result = await userManager.FindByNameAsync(user.UserName);

            // Assert
            Assert.Equal(user, result);
            store.VerifyAll();
        }

        [Fact]
        public async Task CanFindByNameCallsStoreWithoutNormalizedName()
        {
            // Setup
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            User user = new User { UserName = "Foo" };
            store.Setup(s => s.FindByNameAsync(user.UserName, CancellationToken.None)).Returns(Task.FromResult(user)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);
            userManager.KeyNormalizer = null;

            // Act
            User result = await userManager.FindByNameAsync(user.UserName);

            // Assert
            Assert.Equal(user, result);
            store.VerifyAll();
        }

        [Fact]
        public async Task FindByEmailCallsStoreWithNormalizedEmail()
        {
            // Setup
            Mock<IUserEmailStore<User>> store = new Mock<IUserEmailStore<User>>();
            User user = new User { Email = "Foo" };
            store.Setup(s => s.FindByEmailAsync(user.Email.ToUpperInvariant(), CancellationToken.None)).Returns(Task.FromResult(user)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            User result = await userManager.FindByEmailAsync(user.Email);

            // Assert
            Assert.Equal(user, result);
            store.VerifyAll();
        }

        [Fact]
        public async Task CanFindByEmailCallsStoreWithoutNormalizedEmail()
        {
            // Setup
            Mock<IUserEmailStore<User>> store = new Mock<IUserEmailStore<User>>();
            User user = new User { Email = "Foo" };
            store.Setup(s => s.FindByEmailAsync(user.Email, CancellationToken.None)).Returns(Task.FromResult(user)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);
            userManager.KeyNormalizer = null;

            // Act
            User result = await userManager.FindByEmailAsync(user.Email);

            // Assert
            Assert.Equal(user, result);
            store.VerifyAll();
        }

        [Fact]
        public async Task AddToRolesCallsStore()
        {
            // Setup
            Mock<IUserRoleStore<User>> store = new Mock<IUserRoleStore<User>>();
            User user = new User { UserName = "Foo" };
            string[] roles = new string[] { "A", "B", "C", "C" };
            store.Setup(s => s.AddToRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.AddToRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.AddToRoleAsync(user, "C", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();

            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(false))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(false))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "C", CancellationToken.None))
                .Returns(Task.FromResult(false))
                .Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.AddToRolesAsync(user, roles);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
            store.Verify(s => s.AddToRoleAsync(user, "C", CancellationToken.None), Times.Once());
        }

        [Fact]
        public async Task AddToRolesFailsIfUserInRole()
        {
            // Setup
            Mock<IUserRoleStore<User>> store = new Mock<IUserRoleStore<User>>();
            User user = new User { UserName = "Foo" };
            string[] roles = new[] { "A", "B", "C" };
            store.Setup(s => s.AddToRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(true))
                .Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.AddToRolesAsync(user, roles);

            // Assert
            IdentityResultAssert.IsFailure(result, new IdentityErrorDescriber().UserAlreadyInRole("B"));
            store.VerifyAll();
        }

        [Fact]
        public async Task RemoveFromRolesCallsStore()
        {
            // Setup
            Mock<IUserRoleStore<User>> store = new Mock<IUserRoleStore<User>>();
            User user = new User { UserName = "Foo" };
            string[] roles = new[] { "A", "B", "C" };
            store.Setup(s => s.RemoveFromRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.RemoveFromRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.RemoveFromRoleAsync(user, "C", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(true))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(true))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "C", CancellationToken.None))
                .Returns(Task.FromResult(true))
                .Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.RemoveFromRolesAsync(user, roles);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task RemoveFromRolesFailsIfNotInRole()
        {
            // Setup
            Mock<IUserRoleStore<User>> store = new Mock<IUserRoleStore<User>>();
            User user = new User { UserName = "Foo" };
            string[] roles = new string[] { "A", "B", "C" };
            store.Setup(s => s.RemoveFromRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "A", CancellationToken.None))
                .Returns(Task.FromResult(true))
                .Verifiable();
            store.Setup(s => s.IsInRoleAsync(user, "B", CancellationToken.None))
                .Returns(Task.FromResult(false))
                .Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.RemoveFromRolesAsync(user, roles);

            // Assert
            IdentityResultAssert.IsFailure(result, new IdentityErrorDescriber().UserNotInRole("B"));
            store.VerifyAll();
        }

        [Fact]
        public async Task AddClaimsCallsStore()
        {
            // Setup
            Mock<IUserClaimStore<User>> store = new Mock<IUserClaimStore<User>>();
            User user = new User { UserName = "Foo" };
            Claim[] claims = new Claim[] { new Claim("1", "1"), new Claim("2", "2"), new Claim("3", "3") };
            store.Setup(s => s.AddClaimsAsync(user, claims, CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.AddClaimsAsync(user, claims);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task AddClaimCallsStore()
        {
            // Setup
            Mock<IUserClaimStore<User>> store = new Mock<IUserClaimStore<User>>();
            User user = new User { UserName = "Foo" };
            Claim claim = new Claim("1", "1");
            store.Setup(s => s.AddClaimsAsync(user, It.IsAny<IEnumerable<Claim>>(), CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.AddClaimAsync(user, claim);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task UpdateClaimCallsStore()
        {
            // Setup
            Mock<IUserClaimStore<User>> store = new Mock<IUserClaimStore<User>>();
            User user = new User { UserName = "Foo" };
            Claim claim = new Claim("1", "1");
            Claim newClaim = new Claim("1", "2");
            store.Setup(s => s.ReplaceClaimAsync(user, It.IsAny<Claim>(), It.IsAny<Claim>(), CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.ReplaceClaimAsync(user, claim, newClaim);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task CheckPasswordWillRehashPasswordWhenNeeded()
        {
            // Setup
            Mock<IUserPasswordStore<User>> store = new Mock<IUserPasswordStore<User>>();
            Mock<IPasswordHasher<User>> hasher = new Mock<IPasswordHasher<User>>();
            User user = new User { UserName = "Foo" };
            string pwd = "password";
            string hashed = "hashed";
            string rehashed = "rehashed";

            store.Setup(s => s.GetPasswordHashAsync(user, CancellationToken.None))
                .ReturnsAsync(hashed)
                .Verifiable();
            store.Setup(s => s.SetPasswordHashAsync(user, It.IsAny<string>(), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
            store.Setup(x => x.UpdateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(IdentityResult.Success));

            hasher.Setup(s => s.VerifyHashedPassword(user, hashed, pwd)).Returns(PasswordVerificationResult.SuccessRehashNeeded).Verifiable();
            hasher.Setup(s => s.HashPassword(user, pwd)).Returns(rehashed).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);
            userManager.PasswordHasher = hasher.Object;

            // Act
            bool result = await userManager.CheckPasswordAsync(user, pwd);

            // Assert
            Assert.True(result);
            store.VerifyAll();
            hasher.VerifyAll();
        }


        [Fact]
        public async Task RemoveClaimsCallsStore()
        {
            // Setup
            Mock<IUserClaimStore<User>> store = new Mock<IUserClaimStore<User>>();
            User user = new User { UserName = "Foo" };
            Claim[] claims = new Claim[] { new Claim("1", "1"), new Claim("2", "2"), new Claim("3", "3") };
            store.Setup(s => s.RemoveClaimsAsync(user, claims, CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.RemoveClaimsAsync(user, claims);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task RemoveClaimCallsStore()
        {
            // Setup
            Mock<IUserClaimStore<User>> store = new Mock<IUserClaimStore<User>>();
            User user = new User { UserName = "Foo" };
            Claim claim = new Claim("1", "1");
            store.Setup(s => s.RemoveClaimsAsync(user, It.IsAny<IEnumerable<Claim>>(), CancellationToken.None))
                .Returns(Task.FromResult(0))
                .Verifiable();
            store.Setup(s => s.UpdateAsync(user, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
            QIQOUserManager userManager = MockHelpers.TestUserManager(store.Object);

            // Act
            IdentityResult result = await userManager.RemoveClaimAsync(user, claim);

            // Assert
            Assert.True(result.Succeeded);
            store.VerifyAll();
        }

        [Fact]
        public async Task CheckPasswordWithNullUserReturnsFalse()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new EmptyStore());
            Assert.False(await manager.CheckPasswordAsync(null, "whatevs"));
        }

        [Fact]
        public async Task UsersEmailMethodsFailWhenStoreNotImplemented()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            Assert.False(manager.SupportsUserEmail);
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.FindByEmailAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.SetEmailAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.GetEmailAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.IsEmailConfirmedAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.ConfirmEmailAsync(null, null));
        }

        [Fact]
        public async Task UsersPhoneNumberMethodsFailWhenStoreNotImplemented()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            Assert.False(manager.SupportsUserPhoneNumber);
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.SetPhoneNumberAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.SetPhoneNumberAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.GetPhoneNumberAsync(null));
        }

        [Fact]
        public async Task TokenMethodsThrowWithNoTokenProvider()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            User user = new User();
            await Assert.ThrowsAsync<NotSupportedException>(
                async () => await manager.GenerateUserTokenAsync(user, "bogus", null));
            await Assert.ThrowsAsync<NotSupportedException>(
                async () => await manager.VerifyUserTokenAsync(user, "bogus", null, null));
        }

        [Fact]
        public async Task PasswordMethodsFailWhenStoreNotImplemented()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            Assert.False(manager.SupportsUserPassword);
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.CreateAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.ChangePasswordAsync(null, null, null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.AddPasswordAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.RemovePasswordAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.CheckPasswordAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.HasPasswordAsync(null));
        }

        [Fact]
        public async Task SecurityStampMethodsFailWhenStoreNotImplemented()
        {
            Mock<IUserStore<User>> store = new Mock<IUserStore<User>>();
            store.Setup(x => x.GetUserIdAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(Guid.NewGuid().ToString()));
            QIQOUserManager manager = MockHelpers.TestUserManager(store.Object);
            Assert.False(manager.SupportsUserSecurityStamp);
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.UpdateSecurityStampAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(() => manager.GetSecurityStampAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(
                    () => manager.VerifyChangePhoneNumberTokenAsync(new User(), "1", "111-111-1111"));
            await Assert.ThrowsAsync<NotSupportedException>(
                    () => manager.GenerateChangePhoneNumberTokenAsync(new User(), "111-111-1111"));
        }

        [Fact]
        public async Task LoginMethodsFailWhenStoreNotImplemented()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            Assert.False(manager.SupportsUserLogin);
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.AddLoginAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.RemoveLoginAsync(null, null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.GetLoginsAsync(null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.FindByLoginAsync(null, null));
        }

        [Fact]
        public async Task ClaimMethodsFailWhenStoreNotImplemented()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            Assert.False(manager.SupportsUserClaim);
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.AddClaimAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.ReplaceClaimAsync(null, null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.RemoveClaimAsync(null, null));
            await Assert.ThrowsAsync<NotSupportedException>(async () => await manager.GetClaimsAsync(null));
        }

        private class ATokenProvider : IUserTwoFactorTokenProvider<User>
        {
            public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
            {
                throw new NotImplementedException();
            }

            public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
            {
                throw new NotImplementedException();
            }

            public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
            {
                throw new NotImplementedException();
            }
        }



        private async Task VerifyException<TException>(Func<Task> code, string expectedMessage) where TException : Exception
        {
            TException error = await Assert.ThrowsAsync<TException>(code);
            Assert.Equal(expectedMessage, error.Message);
        }

        [Fact]
        public void DisposeAfterDisposeDoesNotThrow()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            manager.Dispose();
            manager.Dispose();
        }

        [Fact]
        public async Task PasswordValidatorBlocksCreate()
        {
            // TODO: Can switch to Mock eventually
            QIQOUserManager manager = MockHelpers.TestUserManager(new EmptyStore());
            manager.PasswordValidators.Clear();
            manager.PasswordValidators.Add(new BadPasswordValidator<User>());
            IdentityResultAssert.IsFailure(await manager.CreateAsync(new User(), "password"),
                BadPasswordValidator<User>.ErrorMessage);
        }

        [Fact]
        public async Task ResetTokenCallNoopForTokenValueZero()
        {
            User user = new User() { UserName = Guid.NewGuid().ToString() };
            Mock<IUserLockoutStore<User>> store = new Mock<IUserLockoutStore<User>>();
            store.Setup(x => x.ResetAccessFailedCountAsync(user, It.IsAny<CancellationToken>())).Returns(() =>
            {
                throw new Exception();
            });
            QIQOUserManager manager = MockHelpers.TestUserManager(store.Object);

            IdentityResultAssert.IsSuccess(await manager.ResetAccessFailedCountAsync(user));
        }

        [Fact]
        public async Task ManagerPublicNullChecks()
        {
            Assert.Throws<ArgumentNullException>("store",
                () => new UserManager<User>(null, null, null, null, null, null, null, null, null));

            QIQOUserManager manager = MockHelpers.TestUserManager(new NotImplementedStore());

            await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await manager.CreateAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await manager.CreateAsync(null, null));
            await
                Assert.ThrowsAsync<ArgumentNullException>("password",
                    async () => await manager.CreateAsync(new User(), null));
            await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await manager.UpdateAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user", async () => await manager.DeleteAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("claim", async () => await manager.AddClaimAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("claim", async () => await manager.ReplaceClaimAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("claims", async () => await manager.AddClaimsAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("userName", async () => await manager.FindByNameAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("login", async () => await manager.AddLoginAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("loginProvider",
                async () => await manager.RemoveLoginAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("providerKey",
                async () => await manager.RemoveLoginAsync(null, "", null));
            await Assert.ThrowsAsync<ArgumentNullException>("email", async () => await manager.FindByEmailAsync(null));
            Assert.Throws<ArgumentNullException>("provider", () => manager.RegisterTokenProvider("whatever", null));
            await Assert.ThrowsAsync<ArgumentNullException>("roles", async () => await manager.AddToRolesAsync(new User(), null));
            await Assert.ThrowsAsync<ArgumentNullException>("roles", async () => await manager.RemoveFromRolesAsync(new User(), null));
        }

        [Fact]
        public async Task MethodsFailWithUnknownUserTest()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new EmptyStore());
            manager.RegisterTokenProvider("whatever", new NoOpTokenProvider());
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetUserNameAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetUserNameAsync(null, "bogus"));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AddClaimAsync(null, new Claim("a", "b")));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AddLoginAsync(null, new UserLoginInfo("", "", "")));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AddPasswordAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AddToRoleAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AddToRolesAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ChangePasswordAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetClaimsAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetLoginsAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetRolesAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.IsInRoleAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.RemoveClaimAsync(null, new Claim("a", "b")));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.RemoveLoginAsync(null, "", ""));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.RemovePasswordAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.RemoveFromRoleAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.RemoveFromRolesAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ReplaceClaimAsync(null, new Claim("a", "b"), new Claim("a", "c")));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.UpdateSecurityStampAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetSecurityStampAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.HasPasswordAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GeneratePasswordResetTokenAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ResetPasswordAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.IsEmailConfirmedAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GenerateEmailConfirmationTokenAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ConfirmEmailAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetEmailAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetEmailAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.IsPhoneNumberConfirmedAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ChangePhoneNumberAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.VerifyChangePhoneNumberTokenAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetPhoneNumberAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetPhoneNumberAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetTwoFactorEnabledAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetTwoFactorEnabledAsync(null, true));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GenerateTwoFactorTokenAsync(null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.VerifyTwoFactorTokenAsync(null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetValidTwoFactorProvidersAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.VerifyUserTokenAsync(null, null, null, null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.AccessFailedAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.ResetAccessFailedCountAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetAccessFailedCountAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetLockoutEnabledAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetLockoutEnabledAsync(null, false));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.SetLockoutEndDateAsync(null, DateTimeOffset.UtcNow));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.GetLockoutEndDateAsync(null));
            await Assert.ThrowsAsync<ArgumentNullException>("user",
                async () => await manager.IsLockedOutAsync(null));
        }

        [Fact]
        public async Task MethodsThrowWhenDisposedTest()
        {
            QIQOUserManager manager = MockHelpers.TestUserManager(new NoopUserStore());
            manager.Dispose();
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddClaimAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddClaimsAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddLoginAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddPasswordAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddToRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.AddToRolesAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.ChangePasswordAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GetClaimsAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GetLoginsAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GetRolesAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.IsInRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemoveClaimAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemoveClaimsAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemoveLoginAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemovePasswordAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemoveFromRoleAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.RemoveFromRolesAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.FindByLoginAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.FindByIdAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.FindByNameAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.CreateAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.CreateAsync(null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.UpdateAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.DeleteAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.ReplaceClaimAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.UpdateSecurityStampAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GetSecurityStampAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GeneratePasswordResetTokenAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.ResetPasswordAsync(null, null, null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.GenerateEmailConfirmationTokenAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.IsEmailConfirmedAsync(null));
            await Assert.ThrowsAsync<ObjectDisposedException>(() => manager.ConfirmEmailAsync(null, null));
        }

        private class BadPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
        {
            public static readonly IdentityError ErrorMessage = new IdentityError { Description = "I'm Bad." };

            public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
            {
                return Task.FromResult(IdentityResult.Failed(ErrorMessage));
            }
        }

        private class EmptyStore :
            IUserPasswordStore<User>,
            IUserClaimStore<User>,
            IUserLoginStore<User>,
            IUserEmailStore<User>,
            IUserPhoneNumberStore<User>,
            IUserLockoutStore<User>,
            IUserTwoFactorStore<User>,
            IUserRoleStore<User>,
            IUserSecurityStampStore<User>
        {
            public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<IList<Claim>>(new List<Claim>());
            }

            public Task AddClaimsAsync(User user, IEnumerable<Claim> claim, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claim, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult("");
            }

            public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<User>(null);
            }

            public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<DateTimeOffset?>(DateTimeOffset.MinValue);
            }

            public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<IList<UserLoginInfo>>(new List<UserLoginInfo>());
            }

            public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<User>(null);
            }

            public void Dispose()
            {
            }

            public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(IdentityResult.Success);
            }

            public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<User>(null);
            }

            public Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<User>(null);
            }

            public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<string>(null);
            }

            public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult("");
            }

            public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<IList<string>>(new List<string>());
            }

            public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult("");
            }

            public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(false);
            }

            public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<string>(null);
            }

            public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<string>(null);
            }

            public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<string>(null);
            }

            public Task SetNormalizedUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }

            public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<IList<User>>(new List<User>());
            }

            public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<IList<User>>(new List<User>());
            }

            public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult("");
            }

            public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult(0);
            }
        }

        private class NoOpTokenProvider : IUserTwoFactorTokenProvider<User>
        {
            public string Name { get; } = "Noop";

            public Task<string> GenerateAsync(string purpose, UserManager<User> manager, User user)
            {
                return Task.FromResult("Test");
            }

            public Task<bool> ValidateAsync(string purpose, string token, UserManager<User> manager, User user)
            {
                return Task.FromResult(true);
            }

            public Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
            {
                return Task.FromResult(true);
            }
        }

        private class NotImplementedStore :
            IUserPasswordStore<User>,
            IUserClaimStore<User>,
            IUserLoginStore<User>,
            IUserRoleStore<User>,
            IUserEmailStore<User>,
            IUserPhoneNumberStore<User>,
            IUserLockoutStore<User>,
            IUserTwoFactorStore<User>
        {
            public Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<User> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<User> FindByNameAsync(string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetNormalizedUserNameAsync(User user, string userName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            Task<IdentityResult> IUserStore<User>.CreateAsync(User user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            Task<IdentityResult> IUserStore<User>.UpdateAsync(User user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            Task<IdentityResult> IUserStore<User>.DeleteAsync(User user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }

            public Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }

            public Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
            {
                throw new NotImplementedException();
            }
        }

    }
}
