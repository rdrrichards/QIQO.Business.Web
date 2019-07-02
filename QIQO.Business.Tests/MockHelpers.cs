using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using QIQO.Business.Client.Entities;
using QIQO.Business.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.Test
{
    public static class MockHelpers
    {
        public static StringBuilder LogMessage = new StringBuilder();

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            Mock<IUserStore<TUser>> store = new Mock<IUserStore<TUser>>();
            Mock<UserManager<TUser>> mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());
            return mgr;
        }

        public static Mock<RoleManager<TRole>> MockRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        {
            store = store ?? new Mock<IRoleStore<TRole>>().Object;
            List<IRoleValidator<TRole>> roles = new List<IRoleValidator<TRole>>();
            roles.Add(new RoleValidator<TRole>());
            return new Mock<RoleManager<TRole>>(store, roles, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null);
        }

        public static Mock<ILogger<T>> MockILogger<T>(StringBuilder logStore = null) where T : class
        {
            logStore = logStore ?? LogMessage;
            Mock<ILogger<T>> logger = new Mock<ILogger<T>>();
            logger.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(),
                It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Func<object, Exception, string> formatter) =>
                {
                    if (formatter == null)
                    {
                        logStore.Append(state.ToString());
                    }
                    else
                    {
                        logStore.Append(formatter(state, exception));
                    }
                    logStore.Append(" ");
                });
            logger.Setup(x => x.BeginScope(It.IsAny<object>())).Callback((object state) =>
            {
                logStore.Append(state.ToString());
                logStore.Append(" ");
            });
            logger.Setup(x => x.IsEnabled(LogLevel.Debug)).Returns(true);
            logger.Setup(x => x.IsEnabled(LogLevel.Warning)).Returns(true);

            return logger;
        }

        public static QIQOUserManager TestUserManager(IUserStore<User> store = null) // where User : class
        {
            store = store ?? new Mock<IUserStore<User>>().Object;
            Mock<IOptions<IdentityOptions>> options = new Mock<IOptions<IdentityOptions>>();
            IdentityOptions idOptions = new IdentityOptions();
            idOptions.Lockout.AllowedForNewUsers = false;
            options.Setup(o => o.Value).Returns(idOptions);
            List<IUserValidator<User>> userValidators = new List<IUserValidator<User>>();
            Mock<IUserValidator<User>> validator = new Mock<IUserValidator<User>>();
            userValidators.Add(validator.Object);
            List<PasswordValidator<User>> pwdValidators = new List<PasswordValidator<User>>();
            pwdValidators.Add(new PasswordValidator<User>());
            QIQOUserManager userManager = new QIQOUserManager(store, options.Object, new PasswordHasher<User>(),
                userValidators, pwdValidators, new UpperInvariantLookupNormalizer(),
                new IdentityErrorDescriber(), null,
                new Mock<ILogger<QIQOUserManager>>().Object);
            validator.Setup(v => v.ValidateAsync(userManager, It.IsAny<User>()))
                .Returns(Task.FromResult(IdentityResult.Success)).Verifiable();
            return userManager;
        }

        //public static RoleManager<TRole> TestRoleManager<TRole>(IRoleStore<TRole> store = null) where TRole : class
        //{
        //    store = store ?? new Mock<IRoleStore<TRole>>().Object;
        //    var roles = new List<IRoleValidator<TRole>>();
        //    roles.Add(new RoleValidator<TRole>());
        //    return new QIQORoleManager(store, roles,
        //        new UpperInvariantLookupNormalizer(),
        //        new IdentityErrorDescriber(),
        //        null,
        //        null);
        //}

    }
}