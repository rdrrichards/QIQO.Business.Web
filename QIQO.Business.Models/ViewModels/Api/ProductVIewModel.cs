using System;

namespace QIQO.Business.ViewModels.Api
{
    public class ProductViewModel
    {
        //productKey : number;
        public int ProductKey { get; set; }

        //productCode : string;
        public string ProductCode { get; set; }

        //productType : string;
        public string ProductType { get; set; }

        //productName : string;
        public string ProductName { get; set; }

        public string ProductDesc { get; set; }

        //productShortDesc : string;
        public string ProductShortDesc { get; set; }

        //productLongDesc : string;
        public string ProductLongDesc { get; set; }
        
        //productImagePath : string;
        public string ProductImagePath { get; set; }
        
        //productBasePrice : number;
        public decimal ProductBasePrice { get; set; }
        
        //productBaseQuantity : number;
        public int ProductBaseQuantity { get; set; }

        public DateTime ProductLastUpdated { get; set; }
    }
}
