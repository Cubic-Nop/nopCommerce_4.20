using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.Vendors
{
    public class VendorsCountRootObject
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}