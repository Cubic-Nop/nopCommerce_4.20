﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.ProductAttributes
{
    public class CustomeProductAttributesRootObjectDto : ISerializableObject
    {
        public CustomeProductAttributesRootObjectDto()
        {
            ProductAttributes = new List<CustomeProductAttributes>();
        }

        [JsonProperty("product_attributes")]
        public IList<CustomeProductAttributes> ProductAttributes { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "product_attributes";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (CustomeProductAttributes);
        }
    }
}