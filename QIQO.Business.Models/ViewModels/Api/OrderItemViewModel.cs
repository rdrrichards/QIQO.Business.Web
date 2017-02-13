
namespace QIQO.Business.ViewModels.Api
{
    public class OrderItemViewModel
    {
        //orderItemKey : number;
        public int OrderItemKey { get; set; }

        //orderKey : number;
        public int OrderKey { get; set; }

        //orderItemSeq : number;
        public int OrderItemSeq { get; set; }

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

        //orderItemPrice : number;
        public decimal OrderItemPrice { get; set; }

        //orderLineTotal : number;
        public decimal OrderLineTotal { get; set; }


        //orderItemShipAddress : IAddress;
        public AddressViewModel OrderItemShipAddress { get; set; }


        //orderItemBillAddress : IAddress;
        public AddressViewModel OrderItemBillAddress { get; set; }

        //orderItemStatus : string;
        public string OrderItemStatus { get; set; }


        //salesRepName : string;
        public string SalesRepName { get; set; }

        //accountRepName : string;
        public string AccountRepName { get; set; }
    }
}
