using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace Nop.Services.Orders
{
    public class CustomeCheckOut
    {
        public int CustomerId { get; set; }
        public string CustomerAccount { get; set; }
        public ICollection<CutomeProduct> CustomeProducts { get; set; }
        public string ShippingSiteId { get; set; } = "2";
        public string ShippingWarehouseId { get; set; } = "24";
        public CustomeCheckOut()
        {
            CustomeProducts = new HashSet<CutomeProduct>();
        }

    }
    public class CutomeProduct
    {
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Qunatity { get; set; }
        public int VendorId { get; set; }
        public string VendorIdAX { get; set; }
    }
}