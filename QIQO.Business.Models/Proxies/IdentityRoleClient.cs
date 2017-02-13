using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Proxies
{
    public class IdentityRoleClient : ProxyBase, IIdentityRoleService
    {
        private IIdentityRoleService channel;

        public IdentityRoleClient() : this(EndpointConfiguration.NetTcpBinding_IIdentityRoleService)
        {

        }
        public IdentityRoleClient(EndpointConfiguration endpoint)
        {
            channel = new ChannelFactory<IIdentityRoleService>(GetBindingForEndpoint(EndpointConfigurationType.NetTcpBinding),
                GetEndpointAddress(endpoint)).CreateChannel();
        }

        public int AddClaim(Role role, RoleClaim claim)
        {
            return channel.AddClaim(role, claim);
        }

        public Task<int> AddClaimAsync(Role role, RoleClaim claim)
        {
            return channel.AddClaimAsync(role, claim);
        }

        public int Create(Role role)
        {
            return channel.Create(role);
        }

        public Task<int> CreateAsync(Role role)
        {
            return channel.CreateAsync(role);
        }

        public bool Delete(Role role)
        {
            return channel.Delete(role);
        }

        public Task<bool> DeleteAsync(Role role)
        {
            return channel.DeleteAsync(role);
        }

        public void Dispose()
        {
            //if (channel != null)
            //{
            //    channel.Dispose();
            //    channel = null;
            //}
        }

        public Role FindById(string roleId)
        {
            return channel.FindById(roleId);
        }

        public Task<Role> FindByIdAsync(string roleId)
        {
            return channel.FindByIdAsync(roleId);
        }

        public Role FindByName(string normalizedRoleName)
        {
            return channel.FindByName(normalizedRoleName);
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName)
        {
            return channel.FindByNameAsync(normalizedRoleName);
        }

        public IList<RoleClaim> GetClaims(Role role)
        {
            return channel.GetClaims(role);
        }

        public Task<IList<RoleClaim>> GetClaimsAsync(Role role)
        {
            return channel.GetClaimsAsync(role);
        }

        public int RemoveClaim(Role role, RoleClaim claim)
        {
            return channel.RemoveClaim(role, claim);
        }

        public Task<int> RemoveClaimAsync(Role role, RoleClaim claim)
        {
            return channel.RemoveClaimAsync(role, claim);
        }

        public bool Update(Role role)
        {
            return channel.Update(role);
        }

        public Task<bool> UpdateAsync(Role role)
        {
            return channel.UpdateAsync(role);
        }

        protected override EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
        {
            if ((endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IIdentityRoleService))
            {
                return new EndpointAddress("http://localhost:7476/QIQOIdentityRoleService/QIQOIdentityRoleService");
            }
            if ((endpointConfiguration == EndpointConfiguration.NetTcpBinding_IIdentityRoleService))
            {
                return new EndpointAddress("net.tcp://localhost:7478/QIQOIdentityRoleService/QIQOIdentityRoleService");
            }
            throw new System.InvalidOperationException(string.Format("\"Cannot find endpoint with name \'{0}\'.\"", endpointConfiguration));
        }
    }
}
