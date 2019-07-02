using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace QIQO.Business.Client.Contracts
{
    [ServiceContract]
    public interface IIdentityUserService : IServiceContract, IDisposable
    {
        [OperationContract]
        int AddLogin(User user, UserLogin user_login);
        [OperationContract]
        int AddToRole(User user, string roleName);
        [OperationContract]
        int Create(User user);
        [OperationContract]
        bool Delete(User user);
        [OperationContract]
        User FindByEmail(string normalizedEmail);
        [OperationContract]
        User FindById(string userId);
        [OperationContract]
        User FindByLogin(string loginProvider, string providerKey);
        [OperationContract]
        User FindByName(string normalizedUserName);
        [OperationContract]
        IList<UserLogin> GetLogins(User user);
        [OperationContract]
        IList<string> GetRoles(User user);
        [OperationContract]
        IList<User> GetUsersInRole(string roleName);
        [OperationContract]
        bool IsInRole(User user, string roleName);
        [OperationContract]
        bool RemoveFromRole(User user, string roleName);
        [OperationContract]
        bool RemoveLogin(User user, string loginProvider, string providerKey);
        [OperationContract]
        int Update(User user);

        // User Claims operations
        [OperationContract]
        int AddClaims(User user, IEnumerable<UserClaim> claims);
        [OperationContract]
        IList<UserClaim> GetClaims(User user);
        [OperationContract]
        IList<User> GetUsersForClaim(UserClaim claim);
        [OperationContract]
        int RemoveClaims(User user, IEnumerable<UserClaim> claims);
        [OperationContract]
        int ReplaceClaim(User user, UserClaim claim, UserClaim newClaim);



        [OperationContract]
        Task<int> AddLoginAsync(User user, UserLogin user_login);
        [OperationContract]
        Task<int> AddToRoleAsync(User user, string roleName);
        [OperationContract]
        Task<int> CreateAsync(User user);
        [OperationContract]
        Task<bool> DeleteAsync(User user);
        [OperationContract]
        Task<User> FindByEmailAsync(string normalizedEmail);
        [OperationContract]
        Task<User> FindByIdAsync(string userId);
        [OperationContract]
        Task<User> FindByLoginAsync(string loginProvider, string providerKey);
        [OperationContract]
        Task<User> FindByNameAsync(string normalizedUserName);
        [OperationContract]
        Task<IList<UserLogin>> GetLoginsAsync(User user);
        [OperationContract]
        Task<IList<string>> GetRolesAsync(User user);
        [OperationContract]
        Task<IList<User>> GetUsersInRoleAsync(string roleName);
        [OperationContract]
        Task<bool> IsInRoleAsync(User user, string roleName);
        [OperationContract]
        Task<bool> RemoveFromRoleAsync(User user, string roleName);
        [OperationContract]
        Task<bool> RemoveLoginAsync(User user, string loginProvider, string providerKey);
        [OperationContract]
        Task<int> UpdateAsync(User user);

        // User Claims operations
        [OperationContract]
        Task<int> AddClaimsAsync(User user, IEnumerable<UserClaim> claims);
        [OperationContract]
        Task<IList<UserClaim>> GetClaimsAsync(User user);
        [OperationContract]
        Task<IList<User>> GetUsersForClaimAsync(UserClaim claim);
        [OperationContract]
        Task<int> RemoveClaimsAsync(User user, IEnumerable<UserClaim> claims);
        [OperationContract]
        Task<int> ReplaceClaimAsync(User user, UserClaim claim, UserClaim newClaim);
    }
}
