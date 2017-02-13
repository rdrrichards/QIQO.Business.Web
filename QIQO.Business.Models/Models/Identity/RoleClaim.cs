using System;

namespace QIQO.Business.Client.Entities
{
    public class RoleClaim
    {
        public Guid ClaimID { get; set; }
        public Guid RoleID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}