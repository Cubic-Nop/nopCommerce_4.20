using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Nop.Core.Domain.Orders;
using Nop.Services.Tasks;
using Nop.Services.Vendors;

namespace Nop.Services.Orders
{
    public class SendOrdersTask : IScheduleTask
    {
        #region Fields

        private readonly IOrderService _orderService;
        private static readonly HttpClient _client = new HttpClient();
        private readonly IVendorService _vendorService;
        #endregion
        #region Ctor

        public SendOrdersTask(IOrderService orderService, IVendorService vendorService)
        {
            _orderService = orderService;
            _vendorService = vendorService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Executes a task
        /// </summary>
        public void Execute()
        {
            var list = _orderService.GetOrdersNotContainsAxId();
            if (list.Any())
            {
                foreach (var order in list)
                {
                    var result = System.Threading.Tasks.Task.Run(async () => await SendOrder(order)).Result;
                    var currentOrder = _orderService.GetOrderById(order.Id);
                    if (!string.IsNullOrEmpty(result))
                    {
                        currentOrder.AXLink = result;
                        _orderService.UpdateOrder(currentOrder);
                    }
                }
            }
        }
        private async System.Threading.Tasks.Task<string> SendOrder(Order order)
        {

            var _obj = JsonConvert.SerializeObject(new CustomeCheckOut()
            {
                CustomerId = order.CustomerId,
                CustomerAccount = order.Customer.AccountNum,
                CustomeProducts = order.OrderItems.Select(l => new CutomeProduct()
                {
                    Price = l.Product.Price,
                    Qunatity = l.Quantity,
                    SKU = l.Product.Sku,
                    VendorIdAX = _vendorService.GetVendorById(l.Product.VendorId)?.Name,
                    VendorId = l.Product.VendorId
                }).ToList()
            });
            //var uri = "http://localhost:56794/api/Sales";
            var uri = "http://localhost:5432/api/Sales";
            HttpResponseMessage response = await _client.PostAsync(uri, new StringContent(_obj, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        #endregion
    }
}