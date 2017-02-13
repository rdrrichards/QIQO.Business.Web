using System;

namespace QIQO.Business.ViewModels.Api
{
    public class FeeScheduleViewModel
    {
        //feeScheduleKey : number;
        public int FeeScheduleKey { get; set; }

        ////companyKey : number;
        //accountKey : number;
        public int AccountKey { get; set; }

        //productKey : number;
        public int ProductKey { get; set; }

        //feeScheduleStartDate : Date;
        public DateTime FeeScheduleStartDate { get; set; }

        //feeScheduleEndDate : Date;
        public DateTime FeeScheduleEndDate { get; set; }

        //feeScheduleTypeCode : string;
        public string FeeScheduleTypeCode { get; set; }

        //feeScheduleValue : number;
        public decimal FeeScheduleValue { get; set; }

        //productDesc : string;
        public string ProductDesc { get; set; }

        //productCode : string;
        public string ProductCode { get; set; }
    }
}
