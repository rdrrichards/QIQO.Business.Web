namespace QIQO.Business.ViewModels.Api
{
    public class InvoiceItemViewModel
    {
        //invoiceItemKey : number;
        public int InvoiceItemKey { get; set; }

        //invoiceKey : number;
        public int InvoiceKey { get; set; }

        //invoiceItemSeq : number;
        public int InvoiceItemSeq { get; set; }

        //productKey : number;
        public int ProductKey { get; set; }

        //productCode : string;
        public string ProductCode { get; set; }

        //productName : string;
        public string ProductName { get; set; }

        //productDesc : string;
        public string ProductDesc { get; set; }

        //product : IProduct;
        public ProductViewModel Product { get; set; }

        //quantity : number;
        public int Quantity { get; set; }

        //invoiceItemPrice : number;
        public decimal InvoiceItemPrice { get; set; }

        //invoiceLineTotal : number;
        public decimal InvoiceLineTotal { get; set; }


        //invoiceItemShipAddress : IAddress;
        public AddressViewModel InvoiceItemShipAddress { get; set; }


        //invoiceItemBillAddress : IAddress;
        public AddressViewModel InvoiceItemBillAddress { get; set; }

        //invoiceItemStatus : string;
        public string InvoiceItemStatus { get; set; }


        //salesRepName : string;
        public string SalesRepName { get; set; }

        //accountRepName : string;
        public string AccountRepName { get; set; }
    }
}
