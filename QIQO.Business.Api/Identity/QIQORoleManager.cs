using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using QIQO.Business.Client.Entities;
using System.Collections.Generic;

namespace QIQO.Business.Identity
{
    public class QIQORoleManager : RoleManager<Role>
    {
        public QIQORoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) :
            base(store, roleValidators, keyNormalizer, errors, logger) // contextAccessor
        {
        }
    }
}
