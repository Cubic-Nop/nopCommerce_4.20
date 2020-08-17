using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Connections;
using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Base;

namespace Nop.Plugin.Api.DTOs.CustomeProductAttributes
{
    [JsonObject(Title = "product_attribute")]
    public class CustomeProductAttributesDto
    {
        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        [JsonProperty("sku")]
        public string Sku { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("attribute_name")]
        public string AttributeName { get; set; }
        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("attribute_id")]
        public long AttributeId { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute options
        /// </summary>
        [JsonProperty("values")]
        public List<ValueDto> Values { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        public CustomeProductAttributesDto()
        {
            Values = new List<ValueDto>();
        }
    }
}