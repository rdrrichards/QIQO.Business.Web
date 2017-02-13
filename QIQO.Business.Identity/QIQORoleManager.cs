using System.Collections.Generic;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using QIQO.Business.Client.Entities;

namespace QIQO.Business.Identity
{
    public class QIQORoleManager : RoleManager<Role>
    {
        public QIQORoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger, 
            IHttpContextAccessor contextAccessor) : 
            base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }
    }
}
