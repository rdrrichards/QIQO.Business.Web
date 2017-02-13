using System;

namespace QIQO.Business.Client.Entities
{
    public class UserLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
        public Guid UserID { get; set; }
    }
}
