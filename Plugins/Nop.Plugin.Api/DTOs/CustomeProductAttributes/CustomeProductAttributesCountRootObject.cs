using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.CustomeProductAttributes
{
    public class CustomeProductAttributesCountRootObject
    {
        [JsonProperty("count")]
        public int Count { get; set; }
    }
}