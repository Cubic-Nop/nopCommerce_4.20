using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.Products
{
    public class VendorsCountRootObject
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}