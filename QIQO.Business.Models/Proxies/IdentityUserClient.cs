using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class IdentityUserClient : ProxyBase, IIdentityUserService
    {
        private IIdentityUserService channel;

        public IdentityUserClient() : this(EndpointConfiguration.NetTcpBinding_IIdentityUserService)
        {

        }
        public IdentityUserClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IIdentityUserService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int AddClaims(User user, IEnumerable<UserClaim> claims)
        {
            return channel.AddClaims(user, claims);
        }

        public Task<int> AddClaimsAsync(User user, IEnumerable<UserClaim> claims)
        {
            return channel.AddClaimsAsync(user, claims);
        }

        public int AddLogin(User user, UserLogin user_login)
        {
            return channel.AddLogin(user, user_login);
        }

        public Task<int> AddLoginAsync(User user, UserLogin user_login)
        {
            return channel.AddLoginAsync(user, user_login);
        }

        public int AddToRole(User user, string roleName)
        {
            return channel.AddToRole(user, roleName);
        }

        public Task<int> AddToRoleAsync(User user, string roleName)
        {
            return channel.AddToRoleAsync(user, roleName);
        }

        public int Create(User user)
        {
            return channel.Create(user);
        }

        public Task<int> CreateAsync(User user)
        {
            return channel.CreateAsync(user);
        }

        public bool Delete(User user)
        {
            return channel.Delete(user);
        }

        public Task<bool> DeleteAsync(User user)
        {
            return channel.DeleteAsync(user);
        }

        public void Dispose()
        {
            //if (channel != null)
            //{
            //    channel.Dispose();
            //    channel = null;
            //}
        }

        public User FindByEmail(string normalizedEmail)
        {
            return channel.FindByEmail(normalizedEmail);
        }

        public Task<User> FindByEmailAsync(string normalizedEmail)
        {
            return channel.FindByEmailAsync(normalizedEmail);
        }

        public User FindById(string userId)
        {
            return channel.FindById(userId);
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return channel.FindByIdAsync(userId);
        }

        public User FindByLogin(string loginProvider, string providerKey)
        {
            return channel.FindByLogin(loginProvider, providerKey);
        }

        public Task<User> FindByLoginAsync(string loginProvider, string providerKey)
        {
            return channel.FindByLoginAsync(loginProvider, providerKey);
        }

        public User FindByName(string normalizedUserName)
        {
            return channel.FindByName(normalizedUserName);
        }

        public Task<User> FindByNameAsync(string normalizedUserName)
        {
            return channel.FindByNameAsync(normalizedUserName);
        }

        public IList<UserClaim> GetClaims(User user)
        {
            return channel.GetClaims(user);
        }

        public Task<IList<UserClaim>> GetClaimsAsync(User user)
        {
            return channel.GetClaimsAsync(user);
        }

        public IList<UserLogin> GetLogins(User user)
        {
            return channel.GetLogins(user);
        }

        public Task<IList<UserLogin>> GetLoginsAsync(User user)
        {
            return channel.GetLoginsAsync(user);
        }

        public IList<string> GetRoles(User user)
        {
            return channel.GetRoles(user);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return channel.GetRolesAsync(user);

        }

        public IList<User> GetUsersForClaim(UserClaim claim)
        {
            return channel.GetUsersForClaim(claim);
        }

        public Task<IList<User>> GetUsersForClaimAsync(UserClaim claim)
        {
            return channel.GetUsersForClaimAsync(claim);
        }

        public IList<User> GetUsersInRole(string roleName)
        {
            return channel.GetUsersInRole(roleName);
        }

        public Task<IList<User>> GetUsersInRoleAsync(string roleName)
        {
            return channel.GetUsersInRoleAsync(roleName);
        }

        public bool IsInRole(User user, string roleName)
        {
            return channel.IsInRole(user, roleName);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return channel.IsInRoleAsync(user, roleName);
        }

        public int RemoveClaims(User user, IEnumerable<UserClaim> claims)
        {
            return channel.RemoveClaims(user, claims);
        }

        public Task<int> RemoveClaimsAsync(User user, IEnumerable<UserClaim> claims)
        {
            return channel.RemoveClaimsAsync(user, claims);
        }

        public bool RemoveFromRole(User user, string roleName)
        {
            return channel.RemoveFromRole(user, roleName);
        }

        public Task<bool> RemoveFromRoleAsync(User user, string roleName)
        {
            return channel.RemoveFromRoleAsync(user, roleName);
        }

        public bool RemoveLogin(User user, string loginProvider, string providerKey)
        {
            return channel.RemoveLogin(user, loginProvider, providerKey);
        }

        public Task<bool> RemoveLoginAsync(User user, string loginProvider, string providerKey)
        {
            return channel.RemoveLoginAsync(user, loginProvider, providerKey);
        }

        public int ReplaceClaim(User user, UserClaim claim, UserClaim newClaim)
        {
            return channel.ReplaceClaim(user, claim, newClaim);
        }

        public Task<int> ReplaceClaimAsync(User user, UserClaim claim, UserClaim newClaim)
        {
            return channel.ReplaceClaimAsync(user, claim, newClaim);
        }

        public int Update(User user)
        {
            return channel.Update(user);
        }

        public Task<int> UpdateAsync(User user)
        {
            return channel.UpdateAsync(user);
        }

        protected override EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IIdentityUserService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOIdentityUserService/QIQOIdentityUserService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IIdentityUserService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOIdentityUserService/QIQOIdentityUserService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}
