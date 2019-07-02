using Microsoft.AspNetCore.Identity;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace QIQO.Business.Identity
{
    public class QIQOUserStore<TUser> : IUserStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserEmailStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserRoleStore<TUser>,
        IUserClaimStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserLoginStore<TUser>
        where TUser : User
    {
        private readonly IServiceFactory _serviceFactory;

        public QIQOUserStore(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (Claim claim in claims)
            {
                user.Claims.Add(new UserClaim() { UserID = user.UserId, ClaimID = Guid.NewGuid(), ClaimType = claim.ValueType, ClaimValue = claim.Value });
            }
            return Task.FromResult(0);
        }

        public Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            UserLogin ul = new UserLogin() { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey, ProviderDisplayName = login.ProviderDisplayName, UserID = user.UserId };
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService identity_service = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<int> result = identity_service.AddLoginAsync(user, ul);
                    scope.Complete();
                    return result;
                }
            }
        }

        public Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<int> result = proxy.AddToRoleAsync(user, roleName);
                    scope.Complete();
                    return result;
                }
            }
        }

        // IUserStore
        public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    user.UserId = Guid.NewGuid();
                    int user_key = await proxy.CreateAsync(user);
                }
                scope.Complete();
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (proxy)
                {

                    bool user_key = await proxy.DeleteAsync(user);
                    scope.Complete();
                }
                return IdentityResult.Success;
            }
        }

        public void Dispose()
        {
            // _serviceFactory = null;
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.FindByEmailAsync(normalizedEmail) as Task<TUser>;
            }
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.FindByIdAsync(userId) as Task<TUser>;
            }
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.FindByLoginAsync(loginProvider, providerKey) as Task<TUser>;
            }
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.FindByNameAsync(normalizedUserName) as Task<TUser>;
            }
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                IList<Claim> user_logins = new List<Claim>();
                Task<IList<UserClaim>> user_claims = proxy.GetClaimsAsync(user);
                foreach (UserClaim claim in user_claims.Result)
                {
                    user_logins.Add(new Claim(claim.ClaimType, claim.ClaimValue));
                }
                return Task.FromResult(user_logins); // Task.FromResult(logins.Result.Select(u => new UserLoginInfo("", "", "")).ToList());
            }
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.LockoutEnd);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                IList<UserLoginInfo> user_logins = new List<UserLoginInfo>();
                Task<IList<UserLogin>> logins = proxy.GetLoginsAsync(user);
                foreach (UserLogin usr in logins.Result)
                {
                    user_logins.Add(new UserLoginInfo(usr.LoginProvider, usr.ProviderKey, usr.ProviderDisplayName));
                }
                return Task.FromResult(user_logins); // Task.FromResult(logins.Result.Select(u => new UserLoginInfo("", "", "")).ToList());
            }
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>();
            using (proxy)
            {
                return proxy.GetRolesAsync(user);
            }
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.SecurityStamp);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserId.ToString());
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.UserName);
        }

        public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                IList<TUser> user_logins = new List<TUser>();
                Task<IList<User>> users_claim = proxy.GetUsersForClaimAsync(new UserClaim() { ClaimType = claim.Type, ClaimValue = claim.Value });
                foreach (User usr in users_claim.Result)
                {
                    user_logins.Add((TUser)new User() { UserId = usr.UserId, Email = usr.Email, UserName = usr.UserName });
                }
                return Task.FromResult(user_logins); // Task.FromResult(logins.Result.Select(u => new UserLoginInfo("", "", "")).ToList());
            }
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.GetUsersInRoleAsync(roleName) as Task<IList<TUser>>;
            }
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
            {
                return proxy.IsInRoleAsync(user, roleName);
            }
        }

        public Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<UserClaim> usr_clams = new List<UserClaim>();
            foreach (Claim uc in claims)
                usr_clams.Add(new UserClaim() { ClaimType = uc.ValueType, ClaimValue = uc.Value });

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<int> result = proxy.RemoveClaimsAsync(user, usr_clams);
                    scope.Complete();
                    return result;
                }
            }
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<bool> result = proxy.RemoveFromRoleAsync(user, roleName);
                    scope.Complete();
                    return result;
                }
            }
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<bool> result = proxy.RemoveLoginAsync(user, loginProvider, providerKey);
                    return result;
                }
            }
        }

        public Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    Task<int> result = proxy.ReplaceClaimAsync(user,
                    new UserClaim() { ClaimType = claim.ValueType, ClaimValue = claim.Value },
                    new UserClaim() { ClaimType = newClaim.ValueType, ClaimValue = newClaim.Value });
                    scope.Complete();
                    return result;
                }
            }
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.LockoutEnd = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityUserService proxy = _serviceFactory.CreateClient<IIdentityUserService>())
                {
                    int id = await proxy.UpdateAsync(user);
                    scope.Complete();
                }
                return IdentityResult.Success;
            }
        }
    }
}
