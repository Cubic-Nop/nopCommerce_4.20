using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.DTOs.Base;
using Nop.Plugin.Api.DTOs.Images;
using Nop.Plugin.Api.DTOs.Languages;
using Nop.Plugin.Api.DTOs.SpecificationAttributes;
using Nop.Plugin.Api.Validators;

namespace Nop.Plugin.Api.DTOs.Vendors
{
    [JsonObject(Title = "vendor_notes")]
    public class VendorNoteDto : BaseDto
    {
        /// <summary>
        /// Gets or sets the note
        /// </summary>
        [JsonProperty("note")]
        public string Note { get; set; }
        /// <summary>
        /// Gets or sets the date and time of vendor note creation
        /// </summary>
        [JsonProperty("create_date")]
        public DateTime CreatedOnUtc { get; set; }
    }
}