using System;
using System.Collections.Generic;

namespace QIQO.Business.ViewModels.Api
{
    public class InvoiceViewModel
    {
        //invoiceKey : number;
        public int InvoiceKey { get; set; }

        //invoiceNumber : string;
        public string InvoiceNumber { get; set; }

        //invoiceEntryDate : Date;
        public DateTime InvoiceEntryDate { get; set; }

        //invoiceStatusDate : Date;
        public DateTime InvoiceStatusDate { get; set; }

        //invoiceStatus : string;
        public string InvoiceStatus { get; set; }

        //invoiceCompleteDate? : Date;
        public DateTime? InvoiceCompleteDate { get; set; }

        //invoiceShipDate? : Date;
        public DateTime? InvoiceShipDate { get; set; }

        //invoiceDeliverByDate? : Date;
        public DateTime? InvoiceDeliverByDate { get; set; }

        //invoiceItemCount : number;
        public int InvoiceItemCount { get; set; }

        //invoiceValueSum : number;
        public decimal InvoiceValueSum { get; set; }

        //account : IAccount;
        public AccountViewModel Account { get; set; }

        //invoiceItems? : IInvoiceItem[];
        public List<InvoiceItemViewModel> InvoiceItems { get; set; } = new List<InvoiceItemViewModel>();

        //salesRepName : string;
        public string SalesRepName { get; set; }

        //accountRepName : string;
        public string AccountRepName { get; set; }


        //accountContactName : string;
        public string AccountContactName { get; set; }

        //accountCode : string;
        public string AccountCode { get; set; }
    }
}
