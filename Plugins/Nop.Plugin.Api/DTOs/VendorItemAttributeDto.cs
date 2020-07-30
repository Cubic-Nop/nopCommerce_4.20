using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Base;

namespace Nop.Plugin.Api.DTOs
{
    [JsonObject(Title = "attribute")]
    public class VendorItemAttributeDto : BaseDto
    {
        [JsonProperty("value")]
        public string Value { get; set; }
    }
}
