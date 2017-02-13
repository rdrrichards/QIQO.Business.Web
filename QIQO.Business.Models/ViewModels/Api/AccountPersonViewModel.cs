using System;

namespace QIQO.Business.ViewModels.Api
{
    public class AccountPersonViewModel
    {
        //roleInCompany : string;
        public string RoleInCompany { get; set; }

        //entityPersonKey : number;
        public int EntityPersonKey { get; set; }

        //startDate : Date;
        public DateTime StartDate { get; set; }

        //endDate : Date;
        public DateTime EndDate { get; set; }

        //comment : string;
        public string Comment { get; set; }

        //companyRoleType : string;
        public string CompanyRoleType { get; set; }
        public int PersonKey { get; set; }
        public string PersonCode { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonMI { get; set; }
        public string PersonLastName { get; set; }
    }
}
