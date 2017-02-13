using System;
using System.Collections.Generic;

namespace QIQO.Business.ViewModels.Api
{
    public class CartViewModel
    {
        //orderDeliverByDate? : Date;
        public DateTime OrderDeliverByDate { get; set; }

        //account : IAccount;
        public AccountViewModel Account { get; set; }

        //cartItems : ICartItem[];
        public List<CartItemViewModel> CartItems { get; set; }
    }

    public class CartItemViewModel
    {
        //product : IProduct;
        public ProductViewModel Product { get; set; }

        //quantity : number;
        public int Quantity { get; set; }
    }
}
