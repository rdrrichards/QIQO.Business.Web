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
    public class QIQORoleStore<TRole> : IRoleStore<TRole>, //, IQueryableRoleStore<TRole>
        IRoleClaimStore<TRole>
        where TRole : Role
    {
        private readonly IServiceFactory _serviceFactory;

        public QIQORoleStore(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
                {
                    try
                    {
                        RoleClaim rc = new RoleClaim() { RoleID = role.RoleId, ClaimType = claim.ValueType, ClaimValue = claim.Value };
                        Task<int> result = role_service.AddClaimAsync(role, rc);
                        scope.Complete();
                        return Task.FromResult(IdentityResult.Success);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }

        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
                {
                    try
                    {
                        role.RoleId = Guid.NewGuid();
                        Task<int> result = role_service.CreateAsync(role);
                        scope.Complete();
                        return Task.FromResult(IdentityResult.Success);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
                {
                    try
                    {
                        Task<bool> result = role_service.DeleteAsync(role);
                        scope.Complete();
                        return Task.FromResult(IdentityResult.Success);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
        }

        public void Dispose()
        {
            //_service_factory = null;
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
            {
                try
                {
                    return role_service.FindByIdAsync(roleId) as Task<TRole>;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
            {
                try
                {
                    return role_service.FindByNameAsync(normalizedRoleName) as Task<TRole>;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }

        public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            List<Claim> bs = new List<Claim>();
            return Task.FromResult((IList<Claim>)bs);
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.RoleId.ToString());
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(role.Name);
        }

        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.NormalizedName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.Name = roleName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (IIdentityRoleService role_service = _serviceFactory.CreateClient<IIdentityRoleService>())
                {
                    try
                    {
                        Task<bool> result = role_service.UpdateAsync(role);
                        scope.Complete();
                        return Task.FromResult(IdentityResult.Success);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }
    }
}
