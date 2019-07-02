using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Contracts
{
    [ServiceContract]
    public interface IIdentityRoleService : IServiceContract, IDisposable
    {
        [OperationContract]
        int Create(Role role);
        [OperationContract]
        bool Delete(Role role);
        [OperationContract]
        Role FindById(string roleId);
        [OperationContract]
        Role FindByName(string normalizedRoleName);
        [OperationContract]
        bool Update(Role role);

        // Role Claims operations
        [OperationContract]
        int AddClaim(Role role, RoleClaim claim);
        [OperationContract]
        IList<RoleClaim> GetClaims(Role role);
        [OperationContract]
        int RemoveClaim(Role role, RoleClaim claim);

        [OperationContract]
        Task<int> CreateAsync(Role role);
        [OperationContract]
        Task<bool> DeleteAsync(Role role);
        [OperationContract]
        Task<Role> FindByIdAsync(string roleId);
        [OperationContract]
        Task<Role> FindByNameAsync(string normalizedRoleName);
        [OperationContract]
        Task<bool> UpdateAsync(Role role);

        // Role Claims operations
        [OperationContract]
        Task<int> AddClaimAsync(Role role, RoleClaim claim);
        [OperationContract]
        Task<IList<RoleClaim>> GetClaimsAsync(Role role);
        [OperationContract]
        Task<int> RemoveClaimAsync(Role role, RoleClaim claim);


    }
}
