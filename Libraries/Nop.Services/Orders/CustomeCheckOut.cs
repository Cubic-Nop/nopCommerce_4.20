using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace Nop.Services.Orders
{
    public class CustomeCheckOut
    {
        public int CustomerId { get; set; }
        public ICollection<CutomeProduct> CustomeProducts { get; set; }
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
    }
}