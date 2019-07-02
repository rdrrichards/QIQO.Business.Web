using System;
using System.Collections.Generic;

namespace QIQO.Business.ViewModels.Api
{
    public class OrderViewModel
    {
        //orderKey : number;
        public int OrderKey { get; set; }

        //orderNumber : string;
        public string OrderNumber { get; set; }

        //orderEntryDate : Date;
        public DateTime OrderEntryDate { get; set; }

        //orderStatusDate : Date;
        public DateTime OrderStatusDate { get; set; }

        //orderStatus : string;
        public string OrderStatus { get; set; }

        //orderCompleteDate? : Date;
        public DateTime? OrderCompleteDate { get; set; }

        //orderShipDate? : Date;
        public DateTime? OrderShipDate { get; set; }

        //orderDeliverByDate? : Date;
        public DateTime? OrderDeliverByDate { get; set; }

        //orderItemCount : number;
        public int OrderItemCount { get; set; }

        //orderValueSum : number;
        public decimal OrderValueSum { get; set; }

        //account : IAccount;
        public AccountViewModel Account { get; set; }

        //orderItems? : IOrderItem[];
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

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
