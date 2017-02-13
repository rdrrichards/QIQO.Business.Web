using System;

namespace QIQO.Business.Client.Entities
{
    public class UserClaim
    {
        public Guid ClaimID { get; set; }
        public Guid UserID { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
