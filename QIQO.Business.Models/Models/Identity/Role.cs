using System;
using System.Collections.Generic;

namespace QIQO.Business.Client.Entities
{
    public class Role
    {
        public Guid RoleId { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public List<RoleClaim> Roles { get; set; } = new List<RoleClaim>();
        public List<User> Users { get; set; } = new List<User>();

    }
}
