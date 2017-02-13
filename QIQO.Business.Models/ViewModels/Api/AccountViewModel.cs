using System;
using System.Collections.Generic;

namespace QIQO.Business.ViewModels.Api
{
    public class AccountViewModel
    {
        //accountKey : number;
        public int AccountKey { get; set; }
        public int CompanyKey { get; set; } = 1;

        //accountCode : string;
        public string AccountCode { get; set; }

        //accountName : string;
        public string AccountName { get; set; }

        //accountDesc : string;
        public string AccountDesc { get; set; }

        //accountDBA : string;
        public string AccountDBA { get; set; }

        //accountStartDate : Date;
        public DateTime AccountStartDate { get; set; }

        //accountEndDate : Date;
        public DateTime AccountEndDate { get; set; }

        //addresses : IAddress[];
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();

        //attributes : IEntityAttribute[];
        public List<EntityAttributeViewModel> Attributes { get; set; } = new List<EntityAttributeViewModel>();

        //employees : IAccountPerson[];
        public List<AccountPersonViewModel> Employees { get; set; } = new List<AccountPersonViewModel>();

        public DateTime AccountLastUpdateDate { get; set; }
    }
}
