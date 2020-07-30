using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.DTOs.Vendors
{
    public class VendorsRootObjectDto : ISerializableObject
    {
        public VendorsRootObjectDto()
        {
            Vendors = new List<VendorDto>();
        }

        [JsonProperty("Vendors")]
        public IList<VendorDto> Vendors { get; set; }

        public string GetPrimaryPropertyName()
        {
            return "Vendors";
        }

        public Type GetPrimaryPropertyType()
        {
            return typeof (VendorDto);
        }
    }
}