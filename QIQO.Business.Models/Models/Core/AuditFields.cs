using System;

namespace QIQO.Business.Client.Entities
{
    public class AuditFields
    {
        public string AddedUserID { get; set; }
        public DateTime AddedDateTime { get; set; }
        public string UpdateUserID { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
