using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.CustomeProductAttributes
{
    public class CustomeProductAttributesRootObjectDto : ISerializableObject
    {
        public CustomeProductAttributesRootObjectDto()
        {
            ProductAttributes = new List<CustomeProductAttributesDto>();
        }

        [JsonProperty("product_attributes")]
        public IList<CustomeProductAttributesDto> ProductAttributes { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "product_attributes";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (CustomeProductAttributesDto);
        }
    }
}