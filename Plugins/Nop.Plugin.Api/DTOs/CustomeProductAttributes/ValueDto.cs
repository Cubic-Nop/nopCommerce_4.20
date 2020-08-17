using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.CustomeProductAttributes
{
    [JsonObject(Title = "values")]
    public class ValueDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("price")]

        public decimal Price { get; set; }
        [JsonProperty("display_order")]

        public int DisplayOrder { get; set; }
    }
}
